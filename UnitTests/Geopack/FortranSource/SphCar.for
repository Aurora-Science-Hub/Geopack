      PROGRAM SPHCAR_08_TESTS

      IMPLICIT REAL*8 (A-H,O-Z)

      R=-1.D0
      THETA=-1.D0
      PHI=-1.D0

c       X=0.D0
c       Y=-1.D0
c       Z=0.D0

      CALL SPHCAR_08 (R, THETA, PHI, X, Y, Z, 1)

      END
