******************************************************************************
C
C ############################################################################
C #    THE MAIN PROGRAMS BELOW GIVE TWO EXAMPLES OF TRACING FIELD LINES      #
C #      USING THE GEOPACK-2008 SOFTWARE  (release of Feb 08, 2008)          #
C ############################################################################
C
      PROGRAM TestDataGenerator
C
C   IN THIS EXAMPLE IT IS ASSUMED THAT WE KNOW GEOGRAPHIC COORDINATES OF A FOOTPOINT
C   OF A FIELD LINE AT THE EARTH'S SURFACE AND TRACE THAT LINE FOR A SPECIFIED
C   MOMENT OF UNIVERSAL TIME, USING A FULL IGRF EXPANSION FOR THE INTERNAL FIELD
      IMPLICIT REAL*8 (A-H,O-Z)
C
      PARAMETER (LMAX=500)
      DIMENSION XX(LMAX),YY(LMAX),ZZ(LMAX), PARMOD(10)

      EXTERNAL T89D_DP,IGRF_GSW_08

      IYEAR=1997
      IDAY=350
      IHOUR=21
      MIN=0
      ISEC=0

      VGSEX=-304.0D0
      VGSEY= -16.0D0
      VGSEZ=   4.0D0

      VGSEY=VGSEY+29.78D0
C
      CALL RECALC_08 (IYEAR,IDAY,IHOUR,MIN,ISEC,VGSEX,VGSEY,VGSEZ)
C
      OPEN(UNIT=1,FILE='TraceResult.dat')
C
      DIR=1.D0
C            (TRACE THE LINE WITH A FOOTPOINT IN THE NORTHERN HEMISPHERE, THAT IS,
C             ANTIPARALLEL TO THE MAGNETIC FIELD)
C
      DSMAX=0.1D0
C               (MAXIMAL SPACING BETWEEN THE FIELD LINE POINTS SET EQUAL TO 1 RE)
C
      ERR=0.0001D0
C                 (PERMISSIBLE STEP ERROR SET AT ERR=0.0001)
      RLIM=60.D0
C            (LIMIT THE TRACING REGION WITHIN R=60 Re)
C
      R0=1.D0
C            (LANDING POINT WILL BE CALCULATED ON THE SPHERE R=1,
C                   I.E. ON THE EARTH'S SURFACE)
      IOPT=1
C           (IN THIS EXAMPLE IOPT IS JUST A DUMMY PARAMETER,
C                 WHOSE VALUE DOES NOT MATTER)
      XGSW=-1.02D0
      YGSW=0.8D0
      ZGSW=0.9D0
C
C   TRACE THE FIELD LINE:
C
       CALL TRACE_08 (XGSW,YGSW,ZGSW,DIR,DSMAX,ERR,RLIM,R0,IOPT,
     * PARMOD,T89D_DP,IGRF_GSW_08,XF,YF,ZF,XX,YY,ZZ,M,LMAX)

      print *, XF,YF,ZF

C
C   WRITE THE RESULTS IN THE DATAFILE 'LINTEST1.DAT':
C
        WRITE (1,21) (XX(L),YY(L),ZZ(L),L=1,M)
 21     FORMAT ((2X,3F32.23))
c
      CLOSE(UNIT=1)
      END





C
C******************************************************************************
C
c       PROGRAM EXAMPLE2
c C
c C  THIS IS ANOTHER EXAMPLE OF USING THE GEOPACK SUBROUTINE "TRACE_08". UNLIKE IN
c C  THE EXAMPLE1, HERE WE ASSUME A PURELY DIPOLAR APPROXIMATION FOR THE EARTH'S
c C  INTERNAL FIELD.
c C  IN THIS CASE WE ALSO EXPLICITLY SPECIFY THE TILT ANGLE OF THE GEODIPOLE,
c C  INSTEAD OF CALCULATING IT FROM THE DATE/TIME.
c C
c       PARAMETER (LMAX=500)
c C
c C  LMAX IS THE UPPER LIMIT ON THE NUMBER OF FIELD LINE POINTS RETURNED BY THE TRACER.
c C  IT CAN BE SET ARBITRARILY LARGE, DEPENDING ON THE SPECIFICS OF A PROBLEM UNDER STUDY.
c C  IN THIS EXAMPLE, LMAX IS TENTATIVELY SET EQUAL TO 500.
c C
c       DIMENSION XX(LMAX),YY(LMAX),ZZ(LMAX), PARMOD(10)
c c
c C  Unlike in the EXAMPLE1, here we "manually" specify the tilt angle and its sine/cosine.
c c  To forward them to the coordinate transformation subroutines, we need to explicitly
c c  include the common block /GEOPACK1/:
c
c C
c       COMMON /GEOPACK1/ AA(10),SPS,CPS,BB(3),PSI,CC(18)
c C
c c be sure to include an EXTERNAL statement with the names of (i) a magnetospheric
c c external field model and (ii) Earth's internal field model.
c c
c       EXTERNAL T96_01, DIP_08
c C
c C   First, call RECALC_08, to define the main field coefficients and, hence, the magnetic
c C      moment of the geodipole for IYEAR=1997 and IDAY=350.
c C   The universal time and solar wind direction does not matter in this example,
c C   because here we explicitly specify the tilt angle (hence, the orientation of
c C   dipole in the GSW coordinates), so we arbitrarily set IHOUR=MIN=ISEC=0 and
c C   VGSEX=-400.0, VGSEY=VGSEZ=0 (any other values would be equally OK):
c C
c       CALL RECALC_08 (1997,350,0,0,0,-400.0,0.0,0.0)
c c
c c   Enter input parameters for T96_01:
c c
c       PRINT *, '   ENTER SOLAR WIND RAM PRESSURE IN NANOPASCALS'
c       READ *, PARMOD(1)
c C
c       PRINT *, '   ENTER DST '
c       READ *, PARMOD(2)
c C
c       PRINT *, '   ENTER IMF BY AND BZ'
c       READ *, PARMOD(3),PARMOD(4)
c C
c
c c  Define the latitude (XLAT) and longitude (XLON) of the field line footpoint
c c   in the GSW coordinate system:
c c
c       XLAT=75.
c       XLON=180.
c C
c C  Specify the dipole tilt angle PS, its sine SPS and cosine CPS, entering
c c    in the common block /GEOPACK1/:
c C
c        PSI=0.
c        SPS=SIN(PSI)
c        CPS=COS(PSI)
c c
c c   Calculate Cartesian coordinates of the starting footpoint:
c c
c       T=(90.-XLAT)*.01745329
c       XL=XLON*.01745329
c       XGSW=SIN(T)*COS(XL)
c       YGSW=SIN(T)*SIN(XL)
c       ZGSW=COS(T)
c C
c c   SPECIFY TRACING PARAMETERS:
c C
c       DIR=1.
c C            (TRACE THE LINE WITH A FOOTPOINT IN THE NORTHERN HEMISPHERE, THAT IS,
c C             ANTIPARALLEL TO THE MAGNETIC FIELD)
c C
c       DSMAX=1.0
c C                (SETS THE MAXIMAL SPACING BETWEEN CONSECUTIVE POINTS ON THE LINE)
c       ERR=0.0001
c C                 (PERMISSIBLE STEP ERROR SET AT ERR=0.0001)
c       RLIM=60.
c C            (LIMIT THE TRACING REGION WITHIN R=60 Re)
c C
c       R0=1.
c C            (LANDING POINT WILL BE CALCULATED ON THE SPHERE R=1,
c C                   I.E. ON THE EARTH'S SURFACE)
c c
c       IOPT=0
c C           (IN THIS EXAMPLE IOPT IS JUST A DUMMY PARAMETER,
c C                 WHOSE VALUE DOES NOT MATTER)
c c
c c  Trace the field line:
c c
c       CALL TRACE_08 (XGSW,YGSW,ZGSW,DIR,DSMAX,ERR,RLIM,R0,IOPT,
c      * PARMOD,T96_01,DIP_08,XF,YF,ZF,XX,YY,ZZ,M,LMAX)
c C
c C   Write the result in the output file 'LINTEST2.DAT':
c C
c        OPEN(UNIT=1, FILE='LINTEST2.DAT')
c   1   WRITE (1,20) (XX(L),YY(L),ZZ(L),L=1,M)
c  20   FORMAT((2X,3F6.2))
c
c       CLOSE(UNIT=1)
c       END

c@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
