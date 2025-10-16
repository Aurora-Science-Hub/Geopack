      PROGRAM SPHCAR_08_TESTDATA

      IMPLICIT REAL*8 (A-H,O-Z)

      DIR=1

      R=-1.D0
      THETA=-1.D0
      PHI=-1.D0

c       X=0.D0
c       Y=-1.D0
c       Z=0.D0

      CALL SPHCAR_08 (R, THETA, PHI, X, Y, Z, DIR)

      write(*, 10) X, Y, Z
С      write(*, 10) R, THETA, PHI

10    FORMAT(3F32.18)

      END
