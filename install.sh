#!/usr/bin/env bash
# EAxWiki bootstrap for Linux/Mac.
# Installs PowerShell Core (pwsh) if missing, then runs install.ps1.

set -euo pipefail

REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

green='\033[0;32m'
yellow='\033[1;33m'
red='\033[0;31m'
nc='\033[0m'

info()  { echo -e "${yellow}[INFO]${nc}  $*"; }
ok()    { echo -e "${green}[OK]${nc}    $*"; }
error() { echo -e "${red}[ERROR]${nc} $*"; exit 1; }

echo ""
echo "====================================="
echo "  EAxWiki Installer (Linux/Mac)"
echo "====================================="
echo ""

# ── Python check ──────────────────────────────────────────────────────────────

if command -v python3 &>/dev/null; then
    ok "Python: $(python3 --version)"
elif command -v python &>/dev/null; then
    ok "Python: $(python --version)"
else
    error "Python 3 not found. Install it via your package manager, e.g.:
  sudo apt install python3   # Debian/Ubuntu
  brew install python        # macOS"
fi

# ── PowerShell Core (pwsh) ────────────────────────────────────────────────────

if command -v pwsh &>/dev/null; then
    ok "PowerShell: $(pwsh --version)"
else
    info "PowerShell Core (pwsh) not found. Installing..."

    if [[ "$OSTYPE" == "linux-gnu"* ]]; then
        # Detect distro
        if command -v apt-get &>/dev/null; then
            info "Detected Debian/Ubuntu — installing via apt..."
            sudo apt-get update -q
            sudo apt-get install -y wget apt-transport-https
            # Microsoft package feed
            wget -q "https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb" -O /tmp/packages-microsoft-prod.deb
            sudo dpkg -i /tmp/packages-microsoft-prod.deb
            sudo apt-get update -q
            sudo apt-get install -y powershell
        elif command -v dnf &>/dev/null; then
            info "Detected Fedora/RHEL — installing via dnf..."
            sudo rpm --import https://packages.microsoft.com/keys/microsoft.asc
            sudo dnf install -y https://packages.microsoft.com/config/rhel/8/packages-microsoft-prod.rpm
            sudo dnf install -y powershell
        else
            error "Unsupported distro. Install PowerShell manually: https://learn.microsoft.com/powershell/scripting/install/installing-powershell-on-linux"
        fi
    elif [[ "$OSTYPE" == "darwin"* ]]; then
        if command -v brew &>/dev/null; then
            info "Detected macOS — installing via Homebrew..."
            brew install --cask powershell
        else
            error "Homebrew not found. Install it from https://brew.sh then re-run this script,
or install PowerShell manually: https://learn.microsoft.com/powershell/scripting/install/installing-powershell-on-macos"
        fi
    else
        error "Unsupported OS: $OSTYPE. Install PowerShell manually: https://learn.microsoft.com/powershell/scripting/install/installing-powershell"
    fi

    ok "PowerShell installed: $(pwsh --version)"
fi

echo ""

# ── Delegate to install.ps1 ───────────────────────────────────────────────────

info "Running install.ps1..."
pwsh -NoLogo "$REPO_ROOT/install.ps1" "$@"
