      PROGRAM TRACE_08_TESTDATA

      IMPLICIT REAL*8 (A-H,O-Z)

      PARAMETER (LMAX=500)
      DIMENSION XX(LMAX),YY(LMAX),ZZ(LMAX), PARMOD(10)

C     Параметры для измерений
      INTEGER NUM_RUNS, I
      PARAMETER (NUM_RUNS=1000)
      REAL*8 START_TIME, END_TIME, TOTAL_TIME1, TOTAL_TIME2
      REAL*8 AVG_TIME1, AVG_TIME2, RATIO
      REAL*8 SUM_SQ1, SUM_SQ2, STD_DEV1, STD_DEV2
      REAL*8 ERROR1, ERROR2, TIME1, TIME2
C     Константа для 99.9% доверительного интервала (t-распределение, n=1000)
      REAL*8 T_VALUE
      PARAMETER (T_VALUE=3.300D0)  ! t_{0.9995, 999} ≈ 3.300

      EXTERNAL T89D_DP,IGRF_GSW_08

      IYEAR=1997
      IDAY=350
      IHOUR=21
      MIN=0
      ISEC=0

      VGSEX=-400.0D0
      VGSEY= 0.0D0
      VGSEZ= 0.0D0

      CALL RECALC_08 (IYEAR,IDAY,IHOUR,MIN,ISEC,VGSEX,VGSEY,VGSEZ)

C     Первый вызов TRACE_08
      DIR1=-1.D0
      DSMAX=0.1D0
      ERR=0.0001D0
      RLIM=60.D0
      R0=1.D0
      IOPT=1
      XGSW1=-0.1059965956329907D0
      YGSW1=0.41975266827470664D0
      ZGSW1=-0.9014246640527153D0

C     Второй вызов TRACE_08 (обратное направление)
      DIR2=1.D0
      XGSW2=-0.45455707401565865D0
      YGSW2=0.4737969930623606D0
      ZGSW2=0.7542497890011055D0

C     Выполнение бенчмарка
      TOTAL_TIME1 = 0.0D0
      TOTAL_TIME2 = 0.0D0
      SUM_SQ1 = 0.0D0
      SUM_SQ2 = 0.0D0

      DO I = 1, NUM_RUNS
C         Первый вызов
          CALL CPU_TIME(START_TIME)
          CALL TRACE_08 (XGSW1,YGSW1,ZGSW1,DIR1,DSMAX,ERR,RLIM,R0,IOPT,
     *     PARMOD,T89D_DP,IGRF_GSW_08,XF,YF,ZF,XX,YY,ZZ,M,LMAX)
          CALL CPU_TIME(END_TIME)
          TIME1 = END_TIME - START_TIME
          TOTAL_TIME1 = TOTAL_TIME1 + TIME1
          SUM_SQ1 = SUM_SQ1 + TIME1*TIME1

C         Второй вызов
          CALL CPU_TIME(START_TIME)
          CALL TRACE_08 (XGSW2,YGSW2,ZGSW2,DIR2,DSMAX,ERR,RLIM,R0,IOPT,
     *     PARMOD,T89D_DP,IGRF_GSW_08,XF,YF,ZF,XX,YY,ZZ,M,LMAX)
          CALL CPU_TIME(END_TIME)
          TIME2 = END_TIME - START_TIME
          TOTAL_TIME2 = TOTAL_TIME2 + TIME2
          SUM_SQ2 = SUM_SQ2 + TIME2*TIME2
      END DO

C     Расчет среднего времени
      AVG_TIME1 = TOTAL_TIME1 / DBLE(NUM_RUNS)
      AVG_TIME2 = TOTAL_TIME2 / DBLE(NUM_RUNS)
      RATIO1 = AVG_TIME1 / AVG_TIME2
      RATIO2 = AVG_TIME2 / AVG_TIME2

C     Расчет стандартного отклонения
      STD_DEV1 =
     *SQRT((SUM_SQ1 - TOTAL_TIME1*AVG_TIME1)/DBLE(NUM_RUNS))
      STD_DEV2 =
     *SQRT((SUM_SQ2 - TOTAL_TIME2*AVG_TIME2)/DBLE(NUM_RUNS))

C     Расчет ошибки как половины 99.9% доверительного интервала
      ERROR1 = (T_VALUE * STD_DEV1 / SQRT(DBLE(NUM_RUNS))) / 2.0D0
      ERROR2 = (T_VALUE * STD_DEV2 / SQRT(DBLE(NUM_RUNS))) / 2.0D0

C     Вывод результатов
      WRITE (*,*) 'BENCHMARK RESULTS:'
      WRITE (*,*) '=================='
      WRITE (*,'(A,I6)') 'NUMBER OF RUNS: ', NUM_RUNS
      WRITE (*,*) ''
      WRITE (*,*) 'DIRECTION    AVERAGE TIME (MKS)    STD DEV    ERROR
     *    RATIO'
      WRITE (*,*) '---------    ------------------    -------    -----
     *    -----'
           WRITE (*,'(A,F15.8,A,F15.8,A,F15.8,A,F6.2)') 'DIR=+1    ',
     * AVG_TIME2 * 1000000.0D0, '    ', STD_DEV2 * 1000000.0D0, '    ',
     * ERROR2 * 1000000.0D0, '    ', RATIO2
      WRITE (*,'(A,F15.8,A,F15.8,A,F15.8,A,F6.2)') 'DIR=-1    ',
     * AVG_TIME1 * 1000000.0D0, '    ', STD_DEV1 * 1000000.0D0, '    ',
     * ERROR1 * 1000000.0D0, '    ', RATIO1

      END
