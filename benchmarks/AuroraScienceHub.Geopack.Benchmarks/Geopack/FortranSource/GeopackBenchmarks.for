      PROGRAM COMPREHENSIVE_GEOPACK_BENCHMARK
      IMPLICIT REAL*8 (A-H,O-Z)

      PARAMETER (LMAX=500)
      DIMENSION XX(LMAX),YY(LMAX),ZZ(LMAX), PARMOD(10)
      DIMENSION XTRACE(LMAX), YTRACE(LMAX), ZTRACE(LMAX)

      INTEGER NUM_RUNS, I
      PARAMETER (NUM_RUNS=1000)
      REAL*8 START_TIME, END_TIME
      REAL*8 T_VALUE
      PARAMETER (T_VALUE=3.300D0)

C     Общие переменные для хранения времени
      REAL*8 TOTAL_TIME, AVG_TIME, SUM_SQ, STD_DEV, ERROR_VAL
      REAL*8 AVG_TIME_RECALC

C     Переменные для выходных параметров
      REAL*8 HXGSW, HYGSW, HZGSW, BR, BTHETA, BPHI
      REAL*8 BXGSW, BYGSW, BZGSW, GST, SLONG, SRASN, SDEC
      REAL*8 X, Y, Z, R, THETA, PHI, BX, BY, BZ
      REAL*8 XGEO, YGEO, ZGEO, XMAG, YMAG, ZMAG
      REAL*8 XGEI, YGEI, ZGEI, XSM, YSM, ZSM
      REAL*8 XGSE, YGSE, ZGSE, H, XMU, RR, THETAR
      REAL*8 XMGNP, YMGNP, ZMGNP, DIST
      REAL*8 XF, YF, ZF
      INTEGER ID, M, L

C     Внешние процедуры
      EXTERNAL T89D_DP, IGRF_GSW_08, DIP_08

C     Общие параметры для RECALC_08
      IYEAR = 1997
      IDAY = 345
      IHOUR = 10
      MIN = 10
      ISEC = 0
      VGSEX = -304.0D0
      VGSEY = 14.78D0
      VGSEZ = 4.0D0

C     Инициализация PARMOD
      DO I = 1, 10
          PARMOD(I) = 0.0D0
      END DO

      WRITE (*,*) 'COMPREHENSIVE GEOPACK-2008 BENCHMARK'
      WRITE (*,*) '==================================='
      WRITE (*,'(A,I6)') 'NUMBER OF RUNS: ', NUM_RUNS
      WRITE (*,*) ''

C     Единый вызов RECALC_08 для всех последующих тестов
      CALL RECALC_08(IYEAR, IDAY, IHOUR, MIN, ISEC, VGSEX, VGSEY, VGSEZ)

C     Параметры для тестов (согласованы с C# бенчмарком)
      XGSW = -1.02D0
      YGSW = 1.02D0
      ZGSW = 1.02D0

      R = 1.02D0
      THETA = 0.7D0
      PHI = 1.4D0

      BX = 1.0D0
      BY = 1.0D0
      BZ = 1.0D0

      BR = 1.0D0
      BTHETA = 1.0D0
      BPHI = 1.0D0

      H = 100.0D0
      XMU = 1.04719D0
      RR = 6462.13176D0
      THETAR = 0.526464D0

      XN_PD = 5.0D0
      VEL = -350.0D0
      BZIMF = 5.0D0

C     Параметры для TRACE тестов
      DIR1 = -1.D0
      DIR2 = 1.D0
      DSMAX_TRACE = 0.1D0
      ERR = 0.0001D0
      RLIM = 60.D0
      R0 = 1.D0
      IOPT = 1
      XGSW1 = -0.1059965956329907D0
      YGSW1 = 0.41975266827470664D0
      ZGSW1 = -0.9014246640527153D0
      XGSW2 = -0.45455707401565865D0
      YGSW2 = 0.4737969930623606D0
      ZGSW2 = 0.7542497890011055D0

      WRITE (*,*) 'FUNCTION                AVERAGE TIME (nS)    ',
     *            'STD DEV (nS)    ERROR (nS)    RATIO'
      WRITE (*,*) '-------------------    -----------------    ',
     *            '-------------    ----------    -----'

C     Бенчмарк 0: RECALC_08 (базовый)
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL RECALC_08(IYEAR, IDAY, IHOUR, MIN, ISEC,
     *                   VGSEX, VGSEY, VGSEZ)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *        AVG_TIME_RECALC, STD_DEV_RECALC, ERROR_VAL_RECALC)
      PRINT *, 'RECALC_08 completed'

C     Бенчмарк 1: IGRF_GSW_08
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL IGRF_GSW_08(XGSW, YGSW, ZGSW, HXGSW, HYGSW, HZGSW)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'IGRF_GSW_08 output:', HXGSW, HYGSW, HZGSW
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'IGRF_GSW_08         ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 2: IGRF_GEO_08
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL IGRF_GEO_08(R, THETA, PHI, BR, BTHETA, BPHI)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'IGRF_GEO_08 output:', BR, BTHETA, BPHI
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'IGRF_GEO_08         ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 3: DIP_08
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL DIP_08(XGSW, YGSW, ZGSW, BXGSW, BYGSW, BZGSW)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'DIP_08 output:', BXGSW, BYGSW, BZGSW
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'DIP_08             ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 4: SUN_08
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL SUN_08(IYEAR, IDAY, IHOUR, MIN, ISEC, GST, SLONG,
     *                SRASN, SDEC)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'SUN_08 output:', GST, SLONG, SRASN, SDEC
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'SUN_08             ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 5: SPHCAR_08 (J>0)
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL SPHCAR_08(R, THETA, PHI, X, Y, Z, 1)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'SPHCAR_08 (J>0) output:', X, Y, Z
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'SPHCAR_08 (J>0)    ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 6: SPHCAR_08 (J<0)
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL SPHCAR_08(R, THETA, PHI, X, Y, Z, -1)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'SPHCAR_08 (J<0) output:', R, THETA, PHI
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'SPHCAR_08 (J<0)    ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 7: BSPCAR_08
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL BSPCAR_08(THETA, PHI, BR, BTHETA, BPHI, BX, BY, BZ)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'BSPCAR_08 output:', BX, BY, BZ
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'BSPCAR_08          ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 8: BCARSP_08
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL BCARSP_08(X, Y, Z, BX, BY, BZ, BR, BTHETA, BPHI)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'BCARSP_08 output:', BR, BTHETA, BPHI
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'BCARSP_08          ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 9: GEOMAG_08 (J>0)
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      XGEO = XGSW
      YGEO = YGSW
      ZGEO = ZGSW
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL GEOMAG_08(XGEO, YGEO, ZGEO, XMAG, YMAG, ZMAG, 1)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'GEOMAG_08 (J>0) output:', XMAG, YMAG, ZMAG
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'GEOMAG_08 (J>0)    ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 10: GEOMAG_08 (J<0)
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL GEOMAG_08(XGEO, YGEO, ZGEO, XMAG, YMAG, ZMAG, -1)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'GEOMAG_08 (J<0) output:', XGEO, YGEO, ZGEO
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'GEOMAG_08 (J<0)    ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 11: GEIGEO_08 (J>0)
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      XGEI = XGSW
      YGEI = YGSW
      ZGEI = ZGSW
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL GEIGEO_08(XGEI, YGEI, ZGEI, XGEO, YGEO, ZGEO, 1)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'GEIGEO_08 (J>0) output:', XGEO, YGEO, ZGEO
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'GEIGEO_08 (J>0)    ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 12: GEIGEO_08 (J<0)
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL GEIGEO_08(XGEI, YGEI, ZGEI, XGEO, YGEO, ZGEO, -1)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'GEIGEO_08 (J<0) output:', XGEI, YGEI, ZGEI
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'GEIGEO_08 (J<0)    ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 13: MAGSM_08 (J>0)
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      XMAG = XGSW
      YMAG = YGSW
      ZMAG = ZGSW
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL MAGSM_08(XMAG, YMAG, ZMAG, XSM, YSM, ZSM, 1)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'MAGSM_08 (J>0) output:', XSM, YSM, ZSM
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'MAGSM_08 (J>0)     ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 14: MAGSM_08 (J<0)
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL MAGSM_08(XMAG, YMAG, ZMAG, XSM, YSM, ZSM, -1)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'MAGSM_08 (J<0) output:', XMAG, YMAG, ZMAG
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'MAGSM_08 (J<0)     ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 15: SMGSW_08 (J>0)
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      XSM = XGSW
      YSM = YGSW
      ZSM = ZGSW
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL SMGSW_08(XSM, YSM, ZSM, XGSW, YGSW, ZGSW, 1)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'SMGSW_08 (J>0) output:', XGSW, YGSW, ZGSW
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'SMGSW_08 (J>0)     ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 16: SMGSW_08 (J<0)
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL SMGSW_08(XSM, YSM, ZSM, XGSW, YGSW, ZGSW, -1)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'SMGSW_08 (J<0) output:', XSM, YSM, ZSM
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'SMGSW_08 (J<0)     ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 17: GEOGSW_08 (J>0)
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      XGEO = XGSW
      YGEO = YGSW
      ZGEO = ZGSW
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL GEOGSW_08(XGEO, YGEO, ZGEO, XGSW, YGSW, ZGSW, 1)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'GEOGSW_08 (J>0) output:', XGSW, YGSW, ZGSW
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'GEOGSW_08 (J>0)    ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 18: GEOGSW_08 (J<0)
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL GEOGSW_08(XGEO, YGEO, ZGEO, XGSW, YGSW, ZGSW, -1)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'GEOGSW_08 (J<0) output:', XGEO, YGEO, ZGEO
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'GEOGSW_08 (J<0)    ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 19: GEODGEO_08 (J>0)
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL GEODGEO_08(H, XMU, R, THETA, 1)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'GEODGEO_08 (J>0) output:', R, THETA
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'GEODGEO_08 (J>0)   ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 20: GEODGEO_08 (J<0)
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL GEODGEO_08(H, XMU, R, THETA, -1)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'GEODGEO_08 (J<0) output:', H, XMU
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'GEODGEO_08 (J<0)   ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 21: GSWGSE_08 (J>0)
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL GSWGSE_08(XGSW, YGSW, ZGSW, XGSE, YGSE, ZGSE, 1)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'GSWGSE_08 (J>0) output:', XGSE, YGSE, ZGSE
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'GSWGSE_08 (J>0)    ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 22: GSWGSE_08 (J<0)
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL GSWGSE_08(XGSW, YGSW, ZGSW, XGSE, YGSE, ZGSE, -1)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'GSWGSE_08 (J<0) output:', XGSW, YGSW, ZGSW
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'GSWGSE_08 (J<0)    ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 23: SHUETAL_MGNP_08
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL SHUETAL_MGNP_08(XN_PD, VEL, BZIMF, XGSW, YGSW, ZGSW,
     *        XMGNP, YMGNP, ZMGNP, DIST, ID)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'SHUETAL_MGNP_08 output:', XMGNP, YMGNP, ZMGNP, DIST, ID
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'SHUETAL_MGNP_08    ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 24: T96_MGNP_08
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL T96_MGNP_08(XN_PD, VEL, XGSW, YGSW, ZGSW,
     *        XMGNP, YMGNP, ZMGNP, DIST, ID)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'T96_MGNP_08 output:', XMGNP, YMGNP, ZMGNP, DIST, ID
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'T96_MGNP_08        ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 25: TRACE_08 (DIR=-1)
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL TRACE_08(XGSW1, YGSW1, ZGSW1, DIR1, DSMAX_TRACE, ERR,
     *        RLIM, R0, IOPT, PARMOD, T89D_DP, IGRF_GSW_08,
     *        XF, YF, ZF, XX, YY, ZZ, M, LMAX)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'TRACE_08 (DIR=-1) output:', XF, YF, ZF, M
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'TRACE_08 (DIR=-1)  ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 26: TRACE_08 (DIR=+1)
      TOTAL_TIME = 0.0D0
      SUM_SQ = 0.0D0
      DO I = 1, NUM_RUNS
          CALL CPU_TIME(START_TIME)
          CALL TRACE_08(XGSW2, YGSW2, ZGSW2, DIR2, DSMAX_TRACE, ERR,
     *        RLIM, R0, IOPT, PARMOD, T89D_DP, IGRF_GSW_08,
     *        XF, YF, ZF, XX, YY, ZZ, M, LMAX)
          CALL CPU_TIME(END_TIME)
          TOTAL_TIME = TOTAL_TIME + (END_TIME - START_TIME)
          SUM_SQ = SUM_SQ + (END_TIME - START_TIME)**2
      END DO
      CALL COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                   AVG_TIME, STD_DEV, ERROR_VAL)
      PRINT *, 'TRACE_08 (DIR=+1) output:', XF, YF, ZF, M
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'TRACE_08 (DIR=+1)  ',
     *     AVG_TIME * 1D9, STD_DEV * 1D9, ERROR_VAL * 1D9,
     *     AVG_TIME/AVG_TIME_RECALC

C     Бенчмарк 27: RECALC_08 (отдельно выводим в таблицу)
      WRITE (*,'(A, F15.3, 2F15.3, F10.3)') 'RECALC_08          ',
     *     AVG_TIME_RECALC * 1D9, STD_DEV_RECALC * 1D9,
     *     ERROR_VAL_RECALC * 1D9, 1.0D0

      WRITE (*,*) ''
      WRITE (*,*) 'BENCHMARK COMPLETED SUCCESSFULLY'
      WRITE (*,*) 'Note: Ratios are relative to RECALC_08'

      END

C     Вспомогательная подпрограмма для вычисления статистики
      SUBROUTINE COMPUTE_STATS(TOTAL_TIME, SUM_SQ, NUM_RUNS, T_VALUE,
     *                         AVG_TIME, STD_DEV, ERROR_VAL)
      IMPLICIT REAL*8 (A-H,O-Z)
      INTEGER NUM_RUNS
      REAL*8 TOTAL_TIME, SUM_SQ, T_VALUE, AVG_TIME, STD_DEV, ERROR_VAL

      AVG_TIME = TOTAL_TIME / DBLE(NUM_RUNS)
      STD_DEV = SQRT((SUM_SQ - TOTAL_TIME * AVG_TIME)
     *         / DBLE(NUM_RUNS - 1))
      ERROR_VAL = (T_VALUE * STD_DEV / SQRT(DBLE(NUM_RUNS))) / 2.0D0

      RETURN
      END
