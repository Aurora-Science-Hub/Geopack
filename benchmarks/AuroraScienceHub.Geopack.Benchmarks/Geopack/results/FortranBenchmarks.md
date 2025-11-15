# COMPREHENSIVE GEOPACK-2008 BENCHMARK RESULTS

**Number of Runs:** 1000
**Reference:** All ratios are relative to RECALC_08

| Function          | Average Time (ns) | Std Dev (ns) | Error (ns) |   Ratio |
|-------------------|------------------:|-------------:|-----------:|--------:|
| RECALC_08         |         1 674.000 |      695.851 |     36.308 |   1.000 |
| IGRF_GSW_08       |         1 754.000 |      891.223 |     46.502 |   1.048 |
| IGRF_GEO_08       |         1 722.000 |      622.176 |     32.464 |   1.029 |
| DIP_08            |         1 379.000 |      920.982 |     48.055 |   0.824 |
| SUN_08            |         1 484.000 |      619.781 |     32.339 |   0.886 |
| SPHCAR_08 (J>0)   |         1 340.000 |      476.053 |     24.839 |   0.800 |
| SPHCAR_08 (J<0)   |         1 370.000 |      487.173 |     25.419 |   0.818 |
| BSPCAR_08         |         1 374.000 |      725.707 |     37.866 |   0.821 |
| BCARSP_08         |         1 368.000 |      557.572 |     29.093 |   0.817 |
| GEOMAG_08 (J>0)   |         1 358.000 |      736.463 |     38.427 |   0.811 |
| GEOMAG_08 (J<0)   |         1 352.000 |      688.891 |     35.945 |   0.808 |
| GEIGEO_08 (J>0)   |         1 355.000 |      810.946 |     42.313 |   0.809 |
| GEIGEO_08 (J<0)   |         1 359.000 |      732.568 |     38.224 |   0.812 |
| MAGSM_08 (J>0)    |         1 346.000 |      666.880 |     34.796 |   0.804 |
| MAGSM_08 (J<0)    |         1 320.000 |      466.710 |     24.352 |   0.789 |
| SMGSW_08 (J>0)    |         1 352.000 |      640.708 |     33.431 |   0.808 |
| SMGSW_08 (J<0)    |         1 367.000 |      811.770 |     42.356 |   0.817 |
| GEOGSW_08 (J>0)   |         1 353.000 |      740.905 |     38.659 |   0.808 |
| GEOGSW_08 (J<0)   |         1 330.000 |      470.448 |     24.547 |   0.795 |
| GEODGEO_08 (J>0)  |         1 436.000 |      747.303 |     38.992 |   0.858 |
| GEODGEO_08 (J<0)  |         1 574.000 |      857.473 |     44.741 |   0.940 |
| GSWGSE_08 (J>0)   |         1 351.000 |      730.981 |     38.141 |   0.807 |
| GSWGSE_08 (J<0)   |         1 343.000 |      725.174 |     37.838 |   0.802 |
| SHUETAL_MGNP_08   |         1 753.000 |      726.993 |     37.933 |   1.047 |
| T96_MGNP_08       |         1 402.000 |      585.439 |     30.547 |   0.838 |
| TRACE_08 (DIR=-1) |       286 521.000 |    7 309.103 |    381.371 | 171.159 |
| TRACE_08 (DIR=+1) |       297 793.000 |    7 381.778 |    385.163 | 177.893 |

## Performance Analysis

### Performance Categories

| Category                    | Ratio Range | Examples                       |
|-----------------------------|-------------|--------------------------------|
| **Extremely High**          | 170-178x    | TRACE_08 procedures            |
| **Baseline**                | 1.0x        | RECALC_08                      |
| **Slightly Above Baseline** | 1.02-1.05x  | IGRF models, SHUETAL_MGNP_08   |
| **Efficient**               | 0.8-1.0x    | Most coordinate transforms     |
| **Most Efficient**          | 0.78-0.80x  | MAGSM_08, GEOGSW_08, GSWGSE_08 |

### Key Observations

- **TRACE_08** remains the most computationally intensive procedure (171-178x baseline)
- **IGRF field models** are slightly more expensive than RECALC_08 (1.03-1.05x)
- **Coordinate transformations** show consistent performance (0.78-0.82x)
- **GEODGEO_08** shows directional performance difference (0.86x vs 0.94x)
- **Magnetopause models**: SHUETAL_MGNP_08 (1.05x) vs T96_MGNP_08 (0.84x)
- **Most efficient**: GEOGSW_08 (J<0) at 0.795 ratio
