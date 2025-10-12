Possible issues in original Geopack-2008dp. Should be addressed to N. A. Tsyganenko.

1. Possible fraction loss in `SUN_08` procedure. Should it work

**Example**: IY=2000 IDAY=1 IHOUR=12 MIN=0 ISEC=0

**Original line**: DJ=365*(IYEAR-1900)+(IYEAR-1901)/`4`+IDAY-0.5D0+FDAY (==`36525`)

**Expected line**: DJ=365*(IYEAR-1900)+(IYEAR-1901)/`4.D0`+IDAY-0.5D0+FDAY (==`36525.75`)

2. Missing D0 notations in T89DP model: line 121 and 122 of original T89DP
3. Awful GEOPACK1 COMMON bloc manipulation at line 1831 where: DD=DIR masks DS3 variable at 13th position. DD == DS3.
