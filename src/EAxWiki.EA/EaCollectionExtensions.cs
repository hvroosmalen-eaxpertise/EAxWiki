using System.Collections.Generic;
using EAxWiki.EA;

namespace EAxWiki.EA
{
    using EA = global::EA;

    internal static class EaCollectionExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this EA.Collection? coll)
            where T : class
        {
            if (coll == null) yield break;
            for (short i = 0; i < coll.Count; i++)
                if (coll.GetAt(i) is T t) yield return t;
        }

        public static IEnumerable<EA.Element> Elements(this EA.Collection? coll)
        {
            if (coll == null) yield break;
            for (short i = 0; i < coll.Count; i++)
            {
                if (coll.GetAt(i) is EA.Element e) yield return e;
            }
        }

        public static IEnumerable<EA.Diagram> Diagrams(this EA.Collection? coll)
        {
            if (coll == null) yield break;
            for (short i = 0; i < coll.Count; i++)
            {
                if (coll.GetAt(i) is EA.Diagram d) yield return d;
            }
        }

        public static IEnumerable<EA.Package> Packages(this EA.Collection? coll)
        {
            if (coll == null) yield break;
            for (short i = 0; i < coll.Count; i++)
            {
                if (coll.GetAt(i) is EA.Package p) yield return p;
            }
        }

        public static IEnumerable<EA.Attribute> Attributes(this EA.Collection? coll)
        {
            if (coll == null) yield break;
            for (short i = 0; i < coll.Count; i++)
            {
                if (coll.GetAt(i) is EA.Attribute a) yield return a;
            }
        }

        public static IEnumerable<EA.Method> Methods(this EA.Collection? coll)
        {
            if (coll == null) yield break;
            for (short i = 0; i < coll.Count; i++)
            {
                if (coll.GetAt(i) is EA.Method m) yield return m;
            }
        }

        public static IEnumerable<EA.TaggedValue> TaggedValues(this EA.Collection? coll)
        {
            if (coll == null) yield break;
            for (short i = 0; i < coll.Count; i++)
            {
                if (coll.GetAt(i) is EA.TaggedValue t) yield return t;
            }
        }

        public static IEnumerable<EA.Connector> Connectors(this EA.Collection? coll)
        {
            if (coll == null) yield break;
            for (short i = 0; i < coll.Count; i++)
            {
                if (coll.GetAt(i) is EA.Connector c) yield return c;
            }
        }

        public static IEnumerable<EA.DiagramObject> DiagramObjects(this EA.Collection? coll)
        {
            if (coll == null) yield break;
            for (short i = 0; i < coll.Count; i++)
            {
                if (coll.GetAt(i) is EA.DiagramObject o) yield return o;
            }
        }
    }
}
