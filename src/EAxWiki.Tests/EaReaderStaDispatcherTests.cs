using System.Runtime.InteropServices;
using EAxWiki.EA;

namespace EAxWiki.Tests;

public class EaReaderStaDispatcherTests
{
    private sealed record RunResult(
        List<int> RetryNumbers,
        List<Exception> RetryExceptions,
        Exception? Failure,
        int HealthyCount,
        int ReconnectCount);

    private static RunResult Run(
        Action execute,
        Action? reconnect = null,
        Func<Exception, bool>? shouldRetry = null,
        int maxRetries = 1)
    {
        var retryNumbers = new List<int>();
        var retryExceptions = new List<Exception>();
        Exception? failure = null;
        var healthyCount = 0;
        var reconnectCount = 0;

        EaReaderStaDispatcher.ExecuteWithReconnect(
            execute: execute,
            reconnect: () => { reconnectCount++; reconnect?.Invoke(); },
            shouldRetry: shouldRetry ?? (ex => ex is COMException),
            onRetry: (ex, retry, _) => { retryNumbers.Add(retry); retryExceptions.Add(ex); },
            onFailure: ex => failure = ex,
            onHealthy: () => healthyCount++,
            maxRetries: maxRetries);

        return new RunResult(retryNumbers, retryExceptions, failure, healthyCount, reconnectCount);
    }

    [Fact]
    public void Execute_SuccessOnFirstAttempt_CallsHealthyOnce_NoReconnect()
    {
        var result = Run(execute: () => { });

        Assert.Empty(result.RetryNumbers);
        Assert.Null(result.Failure);
        Assert.Equal(1, result.HealthyCount);
        Assert.Equal(0, result.ReconnectCount);
    }

    [Fact]
    public void Execute_ComExceptionFirstAttempt_ReconnectsOnce_ThenHealthy()
    {
        var attempts = 0;
        var result = Run(execute: () =>
        {
            attempts++;
            if (attempts == 1) throw new COMException("EA gone");
        });

        Assert.Equal([1], result.RetryNumbers);
        Assert.Single(result.RetryExceptions);
        Assert.IsType<COMException>(result.RetryExceptions[0]);
        Assert.Null(result.Failure);
        Assert.Equal(1, result.HealthyCount);
        Assert.Equal(1, result.ReconnectCount);
        Assert.Equal(2, attempts);
    }

    [Fact]
    public void Execute_ComExceptionEveryAttempt_FailsAfterMaxRetries_NoHealthy()
    {
        var result = Run(execute: () => throw new COMException("EA gone"));

        Assert.Equal([1], result.RetryNumbers);
        Assert.IsType<COMException>(result.Failure);
        Assert.Equal(0, result.HealthyCount);
        Assert.Equal(1, result.ReconnectCount);
    }

    [Fact]
    public void Execute_NonComException_FailsWithoutReconnect()
    {
        var result = Run(execute: () => throw new InvalidOperationException("boom"));

        Assert.Empty(result.RetryNumbers);
        Assert.IsType<InvalidOperationException>(result.Failure);
        Assert.Equal(0, result.HealthyCount);
        Assert.Equal(0, result.ReconnectCount);
    }

    [Fact]
    public void Execute_ReconnectThrows_FailsWithReconnectException()
    {
        var reconnectError = new InvalidOperationException("reconnect failed");
        var result = Run(
            execute: () => throw new COMException("EA gone"),
            reconnect: () => throw reconnectError);

        Assert.Equal([1], result.RetryNumbers);
        Assert.Same(reconnectError, result.Failure);
        Assert.Equal(0, result.HealthyCount);
        Assert.Equal(1, result.ReconnectCount);
    }

    [Fact]
    public void Dispatch_ComException_IsRetriedThenSucceeds()
    {
        // Regression test for the dead-code bug: WorkItem.Execute must PROPAGATE COMException to
        // the pump's ExecuteWithReconnect (so the reconnect path can run) instead of swallowing it
        // and routing straight to the caller's _onError. Models the exact production wiring:
        //   execute: () => work.Execute(reader)
        //   onFailure: ex => work.SetException(ex)
        var attempts = 0;
        var routedToCaller = new List<Exception>();
        var work = new EaReaderStaDispatcher.WorkItem(
            _ => { attempts++; if (attempts == 1) throw new COMException("EA gone"); },
            ex => routedToCaller.Add(ex));

        var reconnectCount = 0;
        var healthyCount = 0;

        EaReaderStaDispatcher.ExecuteWithReconnect(
            execute: () => work.Execute(null!),
            reconnect: () => reconnectCount++,
            shouldRetry: ex => ex is COMException,
            onRetry: (_, _, _) => { },
            onFailure: ex => throw new Xunit.Sdk.XunitException($"unexpected failure: {ex}"),
            onHealthy: () => healthyCount++,
            maxRetries: 1);

        Assert.Equal(2, attempts);
        Assert.Equal(1, reconnectCount);
        Assert.Equal(1, healthyCount);
        Assert.Empty(routedToCaller);
    }
}
