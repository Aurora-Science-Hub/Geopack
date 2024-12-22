      PROGRAM EXAMPLE2
C
C  THIS IS ANOTHER EXAMPLE OF USING THE GEOPACK SUBROUTINE "TRACE_08". UNLIKE IN
C  THE EXAMPLE1, HERE WE ASSUME A PURELY DIPOLAR APPROXIMATION FOR THE EARTH'S
C  INTERNAL FIELD.
C  IN THIS CASE WE ALSO EXPLICITLY SPECIFY THE TILT ANGLE OF THE GEODIPOLE,
C  INSTEAD OF CALCULATING IT FROM THE DATE/TIME.
C
      PARAMETER (LMAX=500)
C
C  LMAX IS THE UPPER LIMIT ON THE NUMBER OF FIELD LINE POINTS RETURNED BY THE TRACER.
C  IT CAN BE SET ARBITRARILY LARGE, DEPENDING ON THE SPECIFICS OF A PROBLEM UNDER STUDY.
C  IN THIS EXAMPLE, LMAX IS TENTATIVELY SET EQUAL TO 500.
C
      DIMENSION XX(LMAX),YY(LMAX),ZZ(LMAX), PARMOD(10)
c
C  Unlike in the EXAMPLE1, here we "manually" specify the tilt angle and its sine/cosine.
c  To forward them to the coordinate transformation subroutines, we need to explicitly
c  include the common block /GEOPACK1/:

C
      COMMON /GEOPACK1/ AA(10),SPS,CPS,BB(3),PSI,CC(18)
C
c be sure to include an EXTERNAL statement with the names of (i) a magnetospheric
c external field model and (ii) Earth's internal field model.
c
      EXTERNAL T96_01, DIP_08
C
C   First, call RECALC_08, to define the main field coefficients and, hence, the magnetic
C      moment of the geodipole for IYEAR=1997 and IDAY=350.
C   The universal time and solar wind direction does not matter in this example,
C   because here we explicitly specify the tilt angle (hence, the orientation of
C   dipole in the GSW coordinates), so we arbitrarily set IHOUR=MIN=ISEC=0 and
C   VGSEX=-400.0, VGSEY=VGSEZ=0 (any other values would be equally OK):
C
      CALL RECALC_08 (1997,350,0,0,0,-400.0,0.0,0.0)
c
c   Enter input parameters for T96_01:
c
      PRINT *, '   ENTER SOLAR WIND RAM PRESSURE IN NANOPASCALS'
      READ *, PARMOD(1)
C
      PRINT *, '   ENTER DST '
      READ *, PARMOD(2)
C
      PRINT *, '   ENTER IMF BY AND BZ'
      READ *, PARMOD(3),PARMOD(4)
C

c  Define the latitude (XLAT) and longitude (XLON) of the field line footpoint
c   in the GSW coordinate system:
c
      XLAT=75.
      XLON=180.
C
C  Specify the dipole tilt angle PS, its sine SPS and cosine CPS, entering
c    in the common block /GEOPACK1/:
C
       PSI=0.
       SPS=SIN(PSI)
       CPS=COS(PSI)
c
c   Calculate Cartesian coordinates of the starting footpoint:
c
      T=(90.-XLAT)*.01745329
      XL=XLON*.01745329
      XGSW=SIN(T)*COS(XL)
      YGSW=SIN(T)*SIN(XL)
      ZGSW=COS(T)
C
c   SPECIFY TRACING PARAMETERS:
C
      DIR=1.
C            (TRACE THE LINE WITH A FOOTPOINT IN THE NORTHERN HEMISPHERE, THAT IS,
C             ANTIPARALLEL TO THE MAGNETIC FIELD)
C
      DSMAX=1.0
C                (SETS THE MAXIMAL SPACING BETWEEN CONSECUTIVE POINTS ON THE LINE)
      ERR=0.0001
C                 (PERMISSIBLE STEP ERROR SET AT ERR=0.0001)
      RLIM=60.
C            (LIMIT THE TRACING REGION WITHIN R=60 Re)
C
      R0=1.
C            (LANDING POINT WILL BE CALCULATED ON THE SPHERE R=1,
C                   I.E. ON THE EARTH'S SURFACE)
c
      IOPT=0
C           (IN THIS EXAMPLE IOPT IS JUST A DUMMY PARAMETER,
C                 WHOSE VALUE DOES NOT MATTER)
c
c  Trace the field line:
c
      CALL TRACE_08 (XGSW,YGSW,ZGSW,DIR,DSMAX,ERR,RLIM,R0,IOPT,
     * PARMOD,T96_01,DIP_08,XF,YF,ZF,XX,YY,ZZ,M,LMAX)
C
C   Write the result in the output file 'LINTEST2.DAT':
C
       OPEN(UNIT=1, FILE='LINTEST2.DAT')
  1   WRITE (1,20) (XX(L),YY(L),ZZ(L),L=1,M)
 20   FORMAT((2X,3F6.2))

      CLOSE(UNIT=1)
      END

c@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
