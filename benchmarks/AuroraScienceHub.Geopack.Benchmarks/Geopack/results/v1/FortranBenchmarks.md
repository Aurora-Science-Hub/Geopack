# COMPREHENSIVE GEOPACK-2008 BENCHMARK RESULTS

**Number of Runs:** 1000
**Reference:** All ratios are relative to RECALC_08

| Function          | Average Time (ns) | Std Dev (ns) | Error (ns) |   Ratio |
|-------------------|------------------:|-------------:|-----------:|--------:|
| RECALC_08         |         1 698.000 |    39602.838 |   2066.380 |   1.000 |
| IGRF_GSW_08       |         1 739.000 |      765.156 |     39.924 |   1.024 |
| IGRF_GEO_08       |         1 710.000 |      663.581 |     34.624 |   1.007 |
| DIP_08            |         1 336.000 |      658.436 |     34.356 |   0.787 |
| SUN_08            |         1 432.000 |      495.602 |     25.859 |   0.843 |
| SPHCAR_08 (J>0)   |         1 344.000 |      523.391 |     27.309 |   0.792 |
| SPHCAR_08 (J<0)   |         1 381.000 |      868.673 |     45.325 |   0.813 |
| BSPCAR_08         |         1 363.000 |      777.068 |     40.546 |   0.803 |
| BCARSP_08         |         1 345.000 |      608.560 |     31.753 |   0.792 |
| GEOMAG_08 (J>0)   |         1 348.000 |      804.701 |     41.987 |   0.794 |
| GEOMAG_08 (J<0)   |         1 340.000 |      663.958 |     34.644 |   0.789 |
| GEIGEO_08 (J>0)   |         1 339.000 |      659.176 |     34.394 |   0.789 |
| GEIGEO_08 (J<0)   |         1 341.000 |      785.708 |     40.996 |   0.790 |
| MAGSM_08 (J>0)    |         1 348.000 |      703.840 |     36.725 |   0.794 |
| MAGSM_08 (J<0)    |         1 318.000 |      470.210 |     24.534 |   0.776 |
| SMGSW_08 (J>0)    |         1 352.000 |      843.094 |     43.991 |   0.796 |
| SMGSW_08 (J<0)    |         1 346.000 |      603.861 |     31.508 |   0.793 |
| GEOGSW_08 (J>0)   |         1 340.000 |      488.507 |     25.489 |   0.789 |
| GEOGSW_08 (J<0)   |         1 318.000 |      486.943 |     25.407 |   0.776 |
| GEODGEO_08 (J>0)  |         1 412.000 |      738.106 |     38.513 |   0.832 |
| GEODGEO_08 (J<0)  |         1 511.000 |      513.949 |     26.817 |   0.890 |
| GSWGSE_08 (J>0)   |         1 317.000 |      465.540 |     24.291 |   0.776 |
| GSWGSE_08 (J<0)   |         1 331.000 |      827.117 |     43.157 |   0.784 |
| SHUETAL_MGNP_08   |         1 758.000 |      936.116 |     48.844 |   1.035 |
| T96_MGNP_08       |         1 415.000 |      797.126 |     41.592 |   0.833 |
| TRACE_08 (DIR=-1) |       316 465.000 |    43209.997 |   2254.593 | 186.375 |
| TRACE_08 (DIR=+1) |       318 740.000 |    39602.838 |   2066.380 | 187.715 |

## Key Observations

- **Most computationally intensive:** TRACE_08 procedures are ~187x slower than RECALC_08
- **Most efficient:** GSWGSE_08 (J>0) at 0.776 ratio
- **Field models:** IGRF procedures are slightly slower than RECALC_08 (1.007-1.024 ratio)
- **Coordinate transformations:** Generally efficient (0.776-0.890 ratio)
- **Magnetopause models:** SHUETAL_MGNP_08 is slightly more expensive than T96_MGNP_08

## Performance Categories

| Category       | Ratio Range | Examples                       |
|----------------|-------------|--------------------------------|
| Very High      | 186-188x    | TRACE_08                       |
| Baseline       | 1.0x        | RECALC_08                      |
| Moderate       | 1.0-1.1x    | IGRF models, SHUETAL_MGNP_08   |
| Efficient      | 0.8-1.0x    | Most coordinate transforms     |
| Most Efficient | 0.77-0.79x  | GSWGSE_08, MAGSM_08, GEOGSW_08 |
