"""
Geopack-2008 for Python: Example Usage

This script demonstrates how to use the `geopack` library to:
1. Initialize a computation context for a specific date and time.
2. Calculate the position of the Sun.
3. Perform coordinate transformations (e.g., GEO to GSW).
4. Calculate Earth's magnetic field using IGRF and Dipole models.
5. Evaluate magnetopause boundary models (Shue et al. 1998, Tsyganenko 1996).
"""

from datetime import datetime
import geopack

def main():
    # 1. Date and Time
    # You can use a datetime object or pass year, month, day, etc. individually.
    dt = datetime(2023, 10, 31, 12, 0, 0)
    print(f"--- Geopack Example for {dt} ---")

    # 2. Sun Position
    # Calculate the position of the Sun for the given time. Returns a NamedTuple.
    sun_pos = geopack.sun(dt)
    print(f"\nSun Position:")
    print(f"  Greenwich mean sidereal time (GST): {sun_pos.gst:.4f} rad")
    print(f"  Ecliptic longitude (SLONG):         {sun_pos.slong:.4f} rad")
    print(f"  Right ascension (SRASN):            {sun_pos.srasn:.4f} rad")
    print(f"  Declination (SDEC):                 {sun_pos.sdec:.4f} rad")

    # 3. Computation Context & Magnetic Fields
    # The Context is required for coordinate transforms and field models because
    # they depend on the dipole tilt and Earth's orientation at a specific time.
    # We use a context manager (with block) to ensure the C-handle is released.
    print("\nMagnetic Field Models (Input: x=1, y=1, z=1 in GSW):")
    with geopack.recalc(dt, vx=-400, vy=0, vz=0) as ctx:
        # Calculate IGRF field in GSW (outputs nT)
        igrf = ctx.igrf_gsw(1.0, 1.0, 1.0)
        print(f"  IGRF (GSW):   Bx={igrf.x:8.2f}, By={igrf.y:8.2f}, Bz={igrf.z:8.2f} nT")
        
        # Calculate Dipole field in GSW (outputs nT)
        dip = ctx.dip(1.0, 1.0, 1.0)
        print(f"  Dipole (GSW): Bx={dip.x:8.2f}, By={dip.y:8.2f}, Bz={dip.z:8.2f} nT")

        # 4. Coordinate Transformations
        # All transforms take (x, y, z) in Earth radii and return (x, y, z).
        print("\nCoordinate Transformations:")
        gsw = (1.0, 0.0, 0.0)
        print(f"  Input GSW: {gsw}")
        geo = ctx.gsw_to_geo(*gsw)
        print(f"  -> GEO:    ({geo.x:.4f}, {geo.y:.4f}, {geo.z:.4f})")
        gse = ctx.gsw_to_gse(*gsw)
        print(f"  -> GSE:    ({gse.x:.4f}, {gse.y:.4f}, {gse.z:.4f})")
        sm = ctx.gsw_to_sm(*gsw)
        print(f"  -> SM:     ({sm.x:.4f}, {sm.y:.4f}, {sm.z:.4f})")

    # 5. Magnetopause Models
    # Magnetopause models do not require a context because they depend on 
    # solar wind parameters passed directly to the function.
    print("\nMagnetopause Models (Input: x=10, y=0, z=0 in GSW):")
    
    # Shue et al. (1998)
    # xn_pd: dynamic pressure (nPa), vel: solar wind velocity (km/s), bz_imf: IMF Bz (nT)
    shu = geopack.shu_mgnp(xn_pd=2.0, vel=400.0, bz_imf=-2.5, x=10.0, y=0.0, z=0.0)
    print(f"  Shue et al. (1998):")
    print(f"    Boundary Point: ({shu.boundary.x:.2f}, {shu.boundary.y:.2f}, {shu.boundary.z:.2f}) Re")
    print(f"    Distance:       {shu.dist:.2f} Re")
    print(f"    Position:       {shu.position.name}")

    # Tsyganenko (1996)
    t96 = geopack.t96_mgnp(xn_pd=2.0, vel=400.0, x=10.0, y=0.0, z=0.0)
    print(f"  Tsyganenko (1996):")
    print(f"    Boundary Point: ({t96.boundary.x:.2f}, {t96.boundary.y:.2f}, {t96.boundary.z:.2f}) Re")
    print(f"    Distance:       {t96.dist:.2f} Re")
    print(f"    Position:       {t96.position.name}")

if __name__ == "__main__":
    main()