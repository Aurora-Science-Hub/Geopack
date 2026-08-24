#!/usr/bin/env bash
# Runs inside the cibuildwheel manylinux container (CIBW_BEFORE_ALL). Installs
# .NET, builds the NativeAOT shared library and bundles it into the Python
# package so the subsequent wheel build includes it.
#
# cibuildwheel copies the whole workspace into /project and runs from there, so
# the .NET source is /project/src and the package is /project/python/geopack.
set -euo pipefail

case "$(uname -m)" in
  x86_64)  RID="linux-x64" ;;
  aarch64) RID="linux-arm64" ;;
  *) echo "unsupported arch: $(uname -m)" >&2; exit 1 ;;
esac

# .NET SDK 10 (glibc >= 2.27; manylinux_2_28 provides 2.28).
curl -sSL https://dot.net/v1/dotnet-install.sh | bash -s -- --channel 10.0
export PATH="$HOME/.dotnet:$PATH"
export DOTNET_CLI_TELEMETRY_OPTOUT=1 DOTNET_NOLOGO=1 DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1

# NativeAOT prerequisites (clang + zlib + libicu for the .NET SDK).
# manylinux_2_28 is AlmaLinux 8 (dnf); older manylinux images use yum.
if command -v dnf >/dev/null 2>&1; then
  dnf install -y clang zlib-devel libicu
elif command -v yum >/dev/null 2>&1; then
  yum install -y clang zlib-devel libicu
else
  echo "no supported package manager found" >&2
  exit 1
fi

# The .NET SDK needs ICU at startup; run invariant so it doesn't depend on a
# specific ICU version (our library is pure numeric — no culture use).
export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

dotnet publish /project/src/Geopack.Native/Geopack.Native.csproj \
  -c Release -r "$RID" -o /tmp/native-out

LIB="$(find /tmp/native-out -maxdepth 1 -type f \( -name 'geopack.so' -o -name 'libgeopack.so' \) | head -n1)"
[[ -n "$LIB" ]] || { echo "no native library produced" >&2; ls -la /tmp/native-out; exit 1; }
cp "$LIB" /project/python/geopack/
