      module MagneticFieldLineTraceBenchmarks
      use, intrinsic :: iso_fortran_env, only: dp => real64
      implicit none

      ! Константы, аналогичные C#
      integer, parameter :: Lmax = 500
      integer, parameter :: Iopt = 1
      real(dp), parameter :: Dsmax = 0.1_dp
      real(dp), parameter :: Err = 0.0001_dp
      real(dp), parameter :: Rlim = 60.0_dp
      real(dp), parameter :: R0 = 1.0_dp
      real(dp), parameter :: Vgsex = -304.0_dp
      real(dp), parameter :: Vgsey = 14.78_dp
      real(dp), parameter :: Vgsez = 4.0_dp

      ! Направления трассировки
      real(dp), parameter :: DirNs = -1.0_dp  ! AntiParallel
      real(dp), parameter :: DirSn = 1.0_dp   ! Parallel

      ! Тестовые точки
      real(dp), parameter :: X_ns = -1.02_dp,
     *Y_ns = 0.0_dp, Z_ns = 0.9_dp
      real(dp), parameter :: X_sn = -1.02_dp,
     *Y_sn = 0.0_dp, Z_sn = -0.9_dp

      ! Параметры модели
      real(dp) :: parmod(10) = 0.0_dp

      ! Глобальные переменные для состояния Geopack
      logical :: geopack_initialized = .false.

      ! Переменные для подсчёта памяти
      integer, parameter :: bytes_per_double = 8
      integer :: total_memory_ns = 0
      integer :: total_memory_sn = 0

      contains

      ! Аналог GlobalSetup в C#
      subroutine setup_benchmark()
          implicit none

          integer :: test_year, test_month, test_day, test_hour,
     *test_min, test_sec

          if (.not. geopack_initialized) then
              ! 18 октября 2023, 00:00:00
              test_year = 2023
              test_month = 10
              test_day = 18
              test_hour = 0
              test_min = 0
              test_sec = 0

              call RECALC_08(test_year, test_month, test_day,
     *test_hour, test_min, test_sec, Vgsex, Vgsey, Vgsez)

              geopack_initialized = .true.
          end if
      end subroutine setup_benchmark

      ! Функция для подсчёта памяти, используемой в вызове TRACE_08
      integer function calculate_trace_memory_usage()
          implicit none
          ! Память для массивов xx, yy, zz (каждый размером Lmax)
          calculate_trace_memory_usage = 3 * Lmax * bytes_per_double
          ! Память для скаляров xf, yf, zf
          calculate_trace_memory_usage = calculate_trace_memory_usage
     * + 3 * bytes_per_double
          ! Память для целочисленной переменной m
          calculate_trace_memory_usage = calculate_trace_memory_usage
     * + 4
          ! Память для входных параметров (x, y, z, dir, dsmax, err, rlim, r0)
          calculate_trace_memory_usage = calculate_trace_memory_usage
     * + 8 * bytes_per_double
          ! Память для массива parmod
          calculate_trace_memory_usage = calculate_trace_memory_usage
     * + 10 * bytes_per_double
      end function calculate_trace_memory_usage

      ! Бенчмарк трассировки от северного к южному полушарию (Baseline)
      subroutine benchmark_trace_ns()
          implicit none
          EXTERNAL T89D_DP,IGRF_GSW_08

          real(dp) :: xx(Lmax), yy(Lmax), zz(Lmax), xf, yf, zf
          integer :: m

          call setup_benchmark()

          ! Подсчёт памяти для этого вызова
          total_memory_ns = total_memory_ns
     * + calculate_trace_memory_usage()

          call TRACE_08(X_ns, Y_ns, Z_ns, DirNs, Dsmax,
     *Err, Rlim, R0, Iopt,
     *parmod, T89D_DP, IGRF_GSW_08,
     *xf, yf, zf, xx, yy, zz, m, Lmax)

      end subroutine benchmark_trace_ns

      ! Бенчмарк трассировки от южного к северному полушарию
      subroutine benchmark_trace_sn()
          implicit none
          EXTERNAL T89D_DP,IGRF_GSW_08

          real(dp) :: xx(Lmax), yy(Lmax), zz(Lmax), xf, yf, zf
          integer :: m

          call setup_benchmark()

          ! Подсчёт памяти для этого вызова
          total_memory_sn = total_memory_sn
     * + calculate_trace_memory_usage()

          call TRACE_08(X_sn, Y_sn, Z_sn, DirSn, Dsmax,
     *Err, Rlim, R0, Iopt,
     *parmod, T89D_DP, IGRF_GSW_08,
     *xf, yf, zf, xx, yy, zz, m, Lmax)

      end subroutine benchmark_trace_sn

      end module MagneticFieldLineTraceBenchmarks

      ! Основная программа для запуска бенчмарков
      program run_benchmarks
      use MagneticFieldLineTraceBenchmarks
      use, intrinsic :: iso_fortran_env, only: dp => real64
      implicit none

      integer :: i, num_runs
      real(dp) :: start_time, end_time, total_time_ns, total_time_sn
      real(dp) :: avg_time_ns, avg_time_sn
      integer :: count_start, count_end, count_rate
      real(dp) :: avg_memory_ns, avg_memory_sn

      ! Количество запусков для усреднения
      num_runs = 1000

      write(*,*) 'Starting Fortran benchmarks...'
      write(*,*) 'Number of runs per benchmark:', num_runs
      write(*,*) ''

      ! Инициализация системного таймера
      call system_clock(count_rate=count_rate)

      ! Обнуление счетчиков памяти
      total_memory_ns = 0
      total_memory_sn = 0

      ! Бенчмарк North->South (Baseline)
      total_time_ns = 0.0_dp
      do i = 1, num_runs
          call system_clock(count_start)
          call benchmark_trace_ns()
          call system_clock(count_end)
          total_time_ns = total_time_ns
     * + real(count_end - count_start, dp) / real(count_rate, dp)
      end do
      avg_time_ns = total_time_ns / real(num_runs, dp)
      avg_memory_ns = real(total_memory_ns, dp) / real(num_runs, dp)

      ! Бенчмарк South->North
      total_time_sn = 0.0_dp
      do i = 1, num_runs
          call system_clock(count_start)
          call benchmark_trace_sn()
          call system_clock(count_end)
          total_time_sn = total_time_sn
     * + real(count_end - count_start, dp) / real(count_rate, dp)
      end do
      avg_time_sn = total_time_sn / real(num_runs, dp)
      avg_memory_sn = real(total_memory_sn, dp) / real(num_runs, dp)

      ! Вывод результатов
      write(*,*) 'Benchmark Results:'
      write(*,*) '=================='
      write(*,*) 'Trace North->South (Baseline):'
      write(*,'(A,F12.8,A)') '  Average time: ',
     *avg_time_ns * 1000000.0_dp, ' mks'
      write(*,'(A,F12.2,A)') '  Average memory: ',
     *avg_memory_ns / 1024.0_dp, ' KB'
      write(*,*) 'Trace South->North:'
      write(*,'(A,F12.8,A)') '  Average time: ',
     *avg_time_sn * 1000000.0_dp, ' mks'
      write(*,'(A,F12.2,A)') '  Average memory: ',
     *avg_memory_sn / 1024.0_dp, ' KB'
      write(*,'(A,F8.4)') 'Time Ratio SN/NS: ',
     *avg_time_sn / avg_time_ns
      write(*,'(A,F8.4)') 'Memory Ratio SN/NS: ',
     *avg_memory_sn / avg_memory_ns

      ! Детальная информация об использовании памяти
      write(*,*) ''
      write(*,*) 'Memory Usage Details:'
      write(*,*) '====================='
      write(*,'(A,I8,A)') 'Memory per TRACE_08 call: ',
     *calculate_trace_memory_usage(), ' bytes'
      write(*,'(A,I8,A)') 'Arrays (xx,yy,zz) size: ',
     *3 * Lmax * bytes_per_double, ' bytes'
      write(*,'(A,I8,A)') 'Parmod array size: ',
     *10 * bytes_per_double, ' bytes'

      end program run_benchmarks
