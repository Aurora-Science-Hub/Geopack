      PROGRAM BCARSPH_08_TESTS

      IMPLICIT REAL*8 (A-H,O-Z)

      x=0.D0
      Y=0.D0
      Z=0.D0

      BX=1.D0
      BY=1.D0
      BZ=1.D0

      CALL BCARSP_08 (X, Y, Z, BX, BY, BZ, BR, BTHETA, BPHI)

      write(*, 10) BR, BTHETA, BPHI
10    FORMAT(3F23.18)

      END
