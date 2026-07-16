# Multi-hop Impact Analysis Mode for the Relationship Graph

**Issue:** #70  
**Date:** 2026-07-14

## Why

Every element page currently embeds a 2-hop relationship graph (rendered via cytoscape + `graph-init.js`). Answering "what breaks if I deprecate this element" requires manually clicking through linked pages. This spec extends the graph with a configurable depth control — the user can increase traversal depth from the default (2 hops) out to N hops, with nodes colored by distance from the focal element.

## Architecture

Replaces the per-page embedded subgraph JSON with a single lightweight `graph-index.json` exported to the wiki root. JS fetches this once and BFS-walks to any depth on the client side.

```
┌──────────────────────┐
│   Export time        │
│                      │
│  GraphIndexExporter  │
│  ─────────────────►  │
│  graph-index.json    │
│  (all nodes + edges) │
│                      │
│  Per element page    │
│  ─ #ea-graph-data    │  ← removed
│  ─ #ea-graph-container + data-focal-id
│                      │
└──────────────────────┘
         │
         ▼
┌──────────────────────┐
│   Browser            │
│                      │
│  graph-init.js       │
│  1. fetch graph-index.json
│  2. build adjacency   │
│  3. BFS at depth N    │
│  4. render cytoscape  │
│                      │
│  Depth control       │
│  ─ dropdown 1..10+All│
│  ─ colors by distance │
└──────────────────────┘
```

## Data Format: graph-index.json

### Nodes

Each element in the model maps to one node entry:

```json
{
  "nodes": [
    {
      "id": 42,
      "label": "Sustainability Goal",
      "fullName": "Sustainability Goal",
      "packageName": "Goals",
      "layer": "business",
      "url": "../Goals/Sustainability Goal.md"
    }
  ],
  "edges": [
    {
      "id": 101,
      "source": 42,
      "target": 57,
      "label": "realizes",
      "sourceLayer": "business"
    }
  ]
}
```

**Size estimate:** ~484 nodes × ~80 bytes + ~2000 edges × ~60 bytes ≈ ~50KB.

### Export

A new `GraphIndexExporter` class produces `graph-index.json` at the wiki root during every export (no `--json` flag required). It iterates each element in `ctx.Elements` and each connector on every element, building the full set of nodes and deduplicated edges. Written to `Path.Combine(ctx.OutputPath, "graph-index.json")`.

### What is removed

- `RelationshipGraphRenderer.cs` — entirely (no longer generates per-page graph data)
- The `<div id="ea-graph-data">` block and its escaped JSON content from every element page
- `expandNode()` function from `graph-init.js` — no longer needed

## Client-side: graph-init.js changes

### Initialization

1. On `DOMContentLoaded` (or MkDocs navigation event), find `#ea-graph-container` and read `data-focal-id`.
2. Fetch `graph-index.json` from the wiki root — checked once, cached by the browser.
3. Build an adjacency list: `{ [nodeId]: { node, neighbors: [edge, targetId][] } }`.
4. BFS from `focalId` to depth `currentDepth` (default 2).
5. Pass resulting subgraph to `.cytoscape({ elements: { nodes, edges } })`.
6. Render using the same cytoscape instance, layout (`cose`), and styling as today.

If `graph-index.json` fails to load (404, network error), fall back to the legacy `<div id="ea-graph-data">` parsing to maintain the current 2-hop behavior for older exports.

### Depth control

A `<select>` (dropdown) is injected adjacent to the graph container with options: 1, 2, 3, 4, 5, 6, 7, 8, 9, Full. Default: 2. On change:

1. Re-run BFS from `focalId` at the selected depth ("Full" = `Infinity` or until no more nodes).
2. Replace cytoscape elements (`.elements()` / `.remove()` + `.add()`) and re-run layout.
3. Color nodes by BFS distance:
   - Distance 0 (focal): `#e65100` (orange, existing focal style)
   - Distance 1: Lighter orange (`#ff8a65`)
   - Distance 2: Warm gray (`#a1887f`)
   - Distance 3+: Cool gray gradient deepening with distance (`#9e9e9e` → `#616161`)
   - Edges match the source node's distance color

The `isFocal` class and thick border remain on the focal node.

### Single-tap behavior

Single tap (or click on non-touch) now **navigates to the element's page** — the same as the current double-tap behavior. Since the full reachable subgraph is already loaded from `graph-index.json`, there is no need for the lazy `expandNode()` fetch. The 280ms single-tap delay is removed.

## Files Changed

| File | Change |
|------|--------|
| **New:** `src/EAxWiki.Export/Exporters/GraphIndexExporter.cs` | New class producing `graph-index.json` |
| **Removed:** `src/EAxWiki.Export/Renderers/RelationshipGraphRenderer.cs` | No longer needed |
| **Modified:** `src/EAxWiki.Export/Exporters/ElementPageWriter.cs` | Remove graph-data embed, keep container + focal-id attribute |
| **Modified:** `src/EAxWiki.Export/Exporters/InfrastructureWriter.cs` | Update embedded graph-init.js source |
| **Modified:** `src/EAxWiki.Export/MarkdownExporter.cs` | Wire in GraphIndexExporter |
| `wiki/graph-init.js` | Regenerated |

## Testing

- Depth=1 shows only direct neighbors (focal + hop-1).
- Depth=2 matches current behavior (focal + hop-1 + hop-2) — no regression.
- Depth=3 shows three hops correctly.
- Depth=Full on a sparse graph terminates without error.
- Diamond pattern (A→B→D, A→C→D): D appears once at depth 2, not duplicated.
- Element with zero relationships: renders empty/placeholder state, no JS error.
- Depth change re-renders without full page reload.
- 404/network error for graph-index.json falls back to legacy `<div id="ea-graph-data">`.
