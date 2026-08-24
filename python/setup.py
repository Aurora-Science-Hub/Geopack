"""Build shim for the geopack wheel.

The wheel embeds a platform-specific native library (``geopack.dylib`` /
``geopack.so`` / ``geopack.dll``) but ships no compiled Python extension
modules, so it must be tagged ``py3-none-<platform>`` (installable on every
Python 3.8+) rather than ``py3-none-any`` or a CPython-specific tag.
"""

from setuptools import setup
from setuptools.dist import Distribution
from wheel.bdist_wheel import bdist_wheel as _bdist_wheel


class BinaryDistribution(Distribution):
    """Advertise native extensions so the wheel is treated as platform-specific.

    ``has_ext_modules() -> True`` makes the ``install`` command place the
    package in ``platlib`` (and set ``Root-Is-Purelib: false``), which is
    correct for a wheel that bundles a native shared library.
    """

    def has_ext_modules(self):
        return True


class BinaryBdistWheel(_bdist_wheel):
    """Emit ``py3-none-<platform>`` instead of ``cp<ver>-cp<ver>-<platform>``.

    ``has_ext_modules() -> True`` would otherwise make ``wheel`` tag the wheel
    against the running CPython version; we ship only a native shared library,
    so a single ``py3``/``none`` wheel serves every Python 3.8+.
    """

    def get_tag(self):
        _, _, plat = super().get_tag()
        return "py3", "none", plat


setup(distclass=BinaryDistribution, cmdclass={"bdist_wheel": BinaryBdistWheel})
