---
ea_id: 779
status: 
status_options: [Approved, Implemented, Mandatory, Proposed, Validated]
ea_hash: e3b0c442
notes_hash: c32e45b8
---

# <span class="sl" data-layer="uml">reference-data</span> UnitOfMeasure

**Type:** Class  **Stereotype:** reference-data  **StereotypeEx:** reference-data  **FQStereotype:** reference-data  
**Status:** <span id="ea-status-editor" class="ea-status-editor" data-ea-id="779" data-status="" data-options='[&quot;Approved&quot;,&quot;Implemented&quot;,&quot;Mandatory&quot;,&quot;Proposed&quot;,&quot;Validated&quot;]' data-file-path="Emissions/UnitOfMeasure.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31"><span class="status-badge status-not-set">Not Set</span><button class="ea-status-edit-btn" type="button" aria-label="Edit status">&#9998;</button></span>  
**Created:** 2026-02-27  **Modified:** 2026-05-20


[Home](../index.html) / [Data Layer](../Data Layer/index.html) / [Open Footprint Data Model LDM](../Open Footprint Data Model LDM/index.html) / [Emissions](index.html)

<div id="ea-notes-editor" class="ea-notes-editor" data-ea-id="779" data-file-path="Emissions/UnitOfMeasure.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" data-ai-configured="false">
<button id="ea-notes-edit-btn" class="ea-notes-edit-btn" type="button" aria-label="Edit notes">&#9998;</button>
<div class="ea-notes-content">
<!--ea-notes-start-->
<p>UnitOfMeasure is a reference entity that provides the controlled vocabulary of measurement units used throughout the model for quantities, emission factors, parameter values, and product footprint attributes. It supports unit conversion through conversion factor attributes and is linked to a PhysicalQuantityType to enable dimensional analysis. The entity allows the model to be system-of-units-agnostic while maintaining the traceability required for rigorous scientific and regulatory reporting.</p>
<!--ea-notes-end-->
</div>
</div>

## Attributes

<table>
<thead><tr><th>Name</th><th>Type</th><th>Default</th><th>Description</th></tr></thead>
<tbody>
<tr><td>id</td><td>Key</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-0--><p>The unique identifier for this UnitOfMeasure record, typically a UN/CEFACT or QUDT unit code such as MTQ for cubic metres or TNE for metric tonnes, ensuring unambiguous cross-system interoperability.</p><!--ea-row-notes-end:attr-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-0" data-notes-hash="60ba5c67" data-kind="attribute" data-el-id="779" data-attr-name="id" data-attr-type="Key" data-file-path="Emissions/UnitOfMeasure.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-0" style="display:none"><td colspan="4"></td></tr>
<tr><td>system_of_units_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-1--><p>Foreign key to the SystemOfUnits record that this unit belongs to, such as SI, Imperial, or US Customary, enabling validation and conversion path determination.</p><!--ea-row-notes-end:attr-1--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-1" data-notes-hash="59ad0b6c" data-kind="attribute" data-el-id="779" data-attr-name="system_of_units_id" data-attr-type="String" data-file-path="Emissions/UnitOfMeasure.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-1" style="display:none"><td colspan="4"></td></tr>
<tr><td>physical_quantity_type_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-2--><p>Foreign key to the PhysicalQuantityType that this unit measures, such as Mass, Energy, or Volume, supporting dimensional consistency checks in calculation models.</p><!--ea-row-notes-end:attr-2--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-2" data-notes-hash="e953d2c1" data-kind="attribute" data-el-id="779" data-attr-name="physical_quantity_type_id" data-attr-type="String" data-file-path="Emissions/UnitOfMeasure.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-2" style="display:none"><td colspan="4"></td></tr>
<tr><td>unit_of_measure_source_reference_id</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-3--><p>Foreign key to the UnitOfMeasureSourceReference that is the authoritative registry for this unit definition, such as the UN/CEFACT Common Codes or QUDT ontology.</p><!--ea-row-notes-end:attr-3--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-3" data-notes-hash="fd7ecb76" data-kind="attribute" data-el-id="779" data-attr-name="unit_of_measure_source_reference_id" data-attr-type="String" data-file-path="Emissions/UnitOfMeasure.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-3" style="display:none"><td colspan="4"></td></tr>
<tr><td>name</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-4--><p>The full human-readable name of the unit, such as Metric Tonne or Kilowatt-hour, used in labels and documentation.</p><!--ea-row-notes-end:attr-4--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-4" data-notes-hash="16557e47" data-kind="attribute" data-el-id="779" data-attr-name="name" data-attr-type="String" data-file-path="Emissions/UnitOfMeasure.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-4" style="display:none"><td colspan="4"></td></tr>
<tr><td>symbol</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-5--><p>The standard symbol for the unit, such as t, kWh, or m3, used in quantity displays and report tables.</p><!--ea-row-notes-end:attr-5--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-5" data-notes-hash="eaf96ee3" data-kind="attribute" data-el-id="779" data-attr-name="symbol" data-attr-type="String" data-file-path="Emissions/UnitOfMeasure.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-5" style="display:none"><td colspan="4"></td></tr>
<tr><td>conversion_factor_a</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-6--><p>The multiplicative factor A in the linear conversion formula: target_value = (source_value x A) + B, enabling conversion from this unit to a defined base unit of the same physical quantity.</p><!--ea-row-notes-end:attr-6--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-6" data-notes-hash="9339b872" data-kind="attribute" data-el-id="779" data-attr-name="conversion_factor_a" data-attr-type="String" data-file-path="Emissions/UnitOfMeasure.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-6" style="display:none"><td colspan="4"></td></tr>
<tr><td>conversion_factor_b</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-7--><p>The additive constant B in the linear conversion formula: target_value = (source_value x A) + B, required for units with a non-zero offset such as degrees Celsius to Kelvin.</p><!--ea-row-notes-end:attr-7--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-7" data-notes-hash="8196e6d3" data-kind="attribute" data-el-id="779" data-attr-name="conversion_factor_b" data-attr-type="String" data-file-path="Emissions/UnitOfMeasure.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-7" style="display:none"><td colspan="4"></td></tr>
<tr><td>conversion_factor_c</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-8--><p>An optional third conversion parameter for non-linear or two-stage unit conversions, reserved for future use where the A+B formula is insufficient.</p><!--ea-row-notes-end:attr-8--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-8" data-notes-hash="51f70c42" data-kind="attribute" data-el-id="779" data-attr-name="conversion_factor_c" data-attr-type="String" data-file-path="Emissions/UnitOfMeasure.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-8" style="display:none"><td colspan="4"></td></tr>
<tr><td>conversion_factor_d</td><td>String</td><td></td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:attr-9--><p>An optional fourth conversion parameter providing additional flexibility for complex unit conversion formulae requiring more than two coefficients.</p><!--ea-row-notes-end:attr-9--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="attr-9" data-notes-hash="ebffcd44" data-kind="attribute" data-el-id="779" data-attr-name="conversion_factor_d" data-attr-type="String" data-file-path="Emissions/UnitOfMeasure.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="attr-9" style="display:none"><td colspan="4"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Tagged Values

<table>
<thead><tr><th>Name</th><th>Value</th><th>Notes</th></tr></thead>
<tbody>
<tr><td>description</td><td>UnitOfMeasure is a reference entity that provides the controlled vocabulary of measurement units used throughout the model for quantities, emission factors, parameter values, and product footprint attributes.</td><td><span class="ea-row-notes-text"><!--ea-row-notes-start:tag-0--><!--ea-row-notes-end:tag-0--></span><button class="ea-row-notes-edit-btn" type="button" data-surface="table-row" data-row-id="tag-0" data-notes-hash="e3b0c442" data-kind="tagged-value" data-el-id="779" data-tag-name="description" data-tag-value="UnitOfMeasure is a reference entity that provides the controlled vocabulary of measurement units used throughout the model for quantities, emission factors, parameter values, and product footprint attributes." data-file-path="Emissions/UnitOfMeasure.md" data-api-port="8001" data-api-token="cd030999497b4feb1b7abdba556cca130373eb9f6f037d31" aria-label="Edit description">&#9998;</button></td></tr>
<tr class="ea-row-edit" data-row-id="tag-0" style="display:none"><td colspan="3"></td></tr>
</tbody>
</table>

[↑ Back to top](#)

## Relationships

| Type | Stereotype | Connected To |
|------|------------|-------------|
| Association |  | [EmissionReportPeriod](EmissionReportPeriod.html) |
| Association |  | [UnitOfMeasureSourceReference](UnitOfMeasureSourceReference.html) |
| Association |  | [PhysicalQuantityType](PhysicalQuantityType.html) |
| Association |  | [SystemOfUnits](SystemOfUnits.html) |
| Association |  | [EmissionParameterType](EmissionParameterType.html) |
| Association |  | [ProductCarbonFootprint](../Products/ProductCarbonFootprint.html) |
| Association |  | [EmissionActivityParameterValue](../Facilities/EmissionActivityParameterValue.html) |
| Association |  | [EmissionFactor](EmissionFactor.html) |
| Association |  | [EmissionFactor](EmissionFactor.html) |
| Association |  | [EmissionComponent](EmissionComponent.html) |
| Association |  | [EmissionStatement](EmissionStatement.html) |

[↑ Back to top](#)

### Appears on Diagrams

<div class="diagram-thumbs">
  <a href="diagrams/Emissions.html" class="diagram-thumb"><img src="diagrams/Emissions.png" alt="Emissions" loading="lazy"><span>Emissions</span></a>
</div>

[↑ Back to top](#)

### Referenced By

| Type | Stereotype | Source |
|------|------------|--------|
| Association |  | [EmissionParameterType](EmissionParameterType.html) |
| Association |  | [UnitOfMeasureSourceReference](UnitOfMeasureSourceReference.html) |
| Association |  | [PhysicalQuantityType](PhysicalQuantityType.html) |
| Association |  | [SystemOfUnits](SystemOfUnits.html) |

[↑ Back to top](#)

---

## Relationship Graph

<div id="ea-graph-container" data-focal-id="779"></div>

---

*Generated: 2026-08-04 12:35:51*