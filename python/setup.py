"""Build shim for the geopack wheel.

The wheel embeds a platform-specific native library (``geopack.dylib`` /
``geopack.so`` / ``geopack.dll``), so it must NOT be tagged ``py3-none-any``.
Forcing ``has_ext_modules()`` to return True makes the wheel builder emit a
platform tag (``cp38-abi3-macosx_11_0_arm64``, etc.) derived from the host,
even though we ship no compiled Python extension modules.
"""

from setuptools import setup
from setuptools.dist import Distribution


class BinaryDistribution(Distribution):
    """Advertise native extensions so the wheel is tagged platform-specific."""

    def has_ext_modules(self):
        return True


setup(distclass=BinaryDistribution)
