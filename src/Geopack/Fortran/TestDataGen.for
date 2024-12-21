******************************************************************************
C
C ############################################################################
C #    THE MAIN PROGRAMS BELOW GIVE TWO EXAMPLES OF TRACING FIELD LINES      #
C #      USING THE GEOPACK-2008 SOFTWARE  (release of Feb 08, 2008)              #
C ############################################################################
C
      PROGRAM EXAMPLE1
C
C   IN THIS EXAMPLE IT IS ASSUMED THAT WE KNOW GEOGRAPHIC COORDINATES OF A FOOTPOINT
C   OF A FIELD LINE AT THE EARTH'S SURFACE AND TRACE THAT LINE FOR A SPECIFIED
C   MOMENT OF UNIVERSAL TIME, USING A FULL IGRF EXPANSION FOR THE INTERNAL FIELD
C
      PARAMETER (LMAX=500)
C
C  LMAX IS THE UPPER LIMIT ON THE NUMBER OF FIELD LINE POINTS RETURNED BY THE TRACER.
C  IT CAN BE SET ARBITRARILY LARGE, DEPENDING ON THE SPECIFICS OF A PROBLEM UNDER STUDY.
C  IN THIS EXAMPLE, LMAX IS TENTATIVELY SET EQUAL TO 500.
C
      DIMENSION XX(LMAX),YY(LMAX),ZZ(LMAX), PARMOD(10)
c
c    Be sure to include an EXTERNAL statement in your codes, specifying the names
c    of external and internal field model subroutines in the package, as shown below.
c    In this example, the external and internal field models are T96_01 and IGRF_GSW_08,
c    respectively. Any other models can be used, provided they have the same format
c    and the same meaning of the input/output parameters.
c
      EXTERNAL T96_01,IGRF_GSW_08
C
C   DEFINE THE UNIVERSAL TIME AND PREPARE THE COORDINATE TRANSFORMATION PARAMETERS
C   BY INVOKING THE SUBROUTINE RECALC_08: IN THIS PARTICULAR CASE WE TRACE A LINE
C   FOR YEAR=1997, IDAY=350, UT HOUR = 21, MIN = SEC = 0
C
      IYEAR=1997
      IDAY=350
      IHOUR=21
      MIN=0
      ISEC=0
C
C  AT THAT TIME, ACCORDING TO THE OMNI DATABASE, THE SOLAR WIND VELOCITY IN GSE HAD THE
C  COMPONENTS
C
      VGSEX=-304.0
      VGSEY= -16.0
      VGSEZ=   4.0
C
C  NOTE, HOWEVER, THAT THE ABERRATION CORRECTION WAS ALREADY MADE IN THE OMNI SOLAR WIND DATA.
C  THEREFORE, TO CORRECTLY TRANSFORM THE DATA TO GSM COORDINATE SYSTEM, WE HAVE TO RESTORE
C  VGSEY TO ITS ORIGINAL OBSERVED VALUE:
C
      VGSEY=VGSEY+29.78
C
      CALL RECALC_08 (IYEAR,IDAY,IHOUR,MIN,ISEC,VGSEX,VGSEY,VGSEZ)
C
      OPEN(UNIT=1,FILE='LINTEST1.DAT')
C
      WRITE (1,100) IYEAR, IDAY, IHOUR, MIN
 100  FORMAT (' IYEAR=',I4,' IDAY=',I3,' IHOUR=',I2,' MIN=',I2,/)
C
      PARMOD(1)=3.
      PARMOD(2)=-20.
      PARMOD(3)=3.
      PARMOD(4)=-5.

      WRITE (1,110) PARMOD(1)
 110  FORMAT('   SOLAR WIND RAM PRESSURE (NANOPASCALS):',F6.1,/)
C
      WRITE (1,120) PARMOD(2)
 120  FORMAT ('   DST-INDEX: ',F6.0,/)
C
      WRITE (1,130) PARMOD(3),PARMOD(4)
 130  FORMAT ('   IMF By and Bz: ',2F6.1,/)
C
C    THE LINE WILL BE TRACED FROM A GROUND (RE=1.0) FOOTPOINT WITH GEOGRAPHICAL
C     LONGITUDE 45 DEGREES AND LATITUDE 75 DEGREES:
C
      GEOLAT=75.
      GEOLON=45.
      RE=1.0

      PRINT *, '  GEOGRAPHIC (GEOCENTRIC) LATITUDE (degs): ',GEOLAT
      PRINT *, '  GEOGRAPHIC (GEOCENTRIC) LONGITUDE (degs): ',GEOLON

      COLAT=(90.-GEOLAT)*.01745329
      XLON=GEOLON*.01745329
C
C   CONVERT SPHERICAL COORDS INTO CARTESIAN :
C
      CALL SPHCAR_08 (RE,COLAT,XLON,XGEO,YGEO,ZGEO,1)
C
C   TRANSFORM GEOGRAPHICAL GEOCENTRIC COORDS INTO SOLAR WIND MAGNETOSPHERIC ONES:
C
      CALL GEOGSW_08 (XGEO,YGEO,ZGEO,XGSW,YGSW,ZGSW,1)
C
c   SPECIFY TRACING PARAMETERS:
C
      DIR=1.
C            (TRACE THE LINE WITH A FOOTPOINT IN THE NORTHERN HEMISPHERE, THAT IS,
C             ANTIPARALLEL TO THE MAGNETIC FIELD)
C
      DSMAX=1.0
C               (MAXIMAL SPACING BETWEEN THE FIELD LINE POINTS SET EQUAL TO 1 RE)
C
      ERR=0.0001
C                 (PERMISSIBLE STEP ERROR SET AT ERR=0.0001)
      RLIM=60.
C            (LIMIT THE TRACING REGION WITHIN R=60 Re)
C
      R0=1.
C            (LANDING POINT WILL BE CALCULATED ON THE SPHERE R=1,
C                   I.E. ON THE EARTH'S SURFACE)
      IOPT=0
C           (IN THIS EXAMPLE IOPT IS JUST A DUMMY PARAMETER,
C                 WHOSE VALUE DOES NOT MATTER)
C
C   TRACE THE FIELD LINE:
C
      CALL TRACE_08 (XGSW,YGSW,ZGSW,DIR,DSMAX,ERR,RLIM,R0,IOPT,
     * PARMOD,T96_01,IGRF_GSW_08,XF,YF,ZF,XX,YY,ZZ,M,LMAX)
C
C   WRITE THE RESULTS IN THE DATAFILE 'LINTEST1.DAT':
C
        WRITE (1,20)
 20     FORMAT('  THE LINE IN GSW COORDS:',/)
        WRITE (1,21) (XX(L),YY(L),ZZ(L),L=1,M)
 21     FORMAT ((2X,3F6.2))

      CLOSE(UNIT=1)
      END





C
C******************************************************************************
C
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