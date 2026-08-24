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

# .NET SDK 10 (glibc >= 2.27; manylinux_2_28 provides 2.28). Invariant
# globalization avoids an ICU dependency (our library is pure numeric).
curl -sSL https://dot.net/v1/dotnet-install.sh | bash -s -- --channel 10.0
export PATH="$HOME/.dotnet:$PATH"
export DOTNET_CLI_TELEMETRY_OPTOUT=1 DOTNET_NOLOGO=1 DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

# NativeAOT needs clang; the manylinux image ships gcc but not clang. Resolve
# the package manager by absolute path — cibuildwheel's PATH may not include
# /usr/bin, so `command -v` can miss dnf/yum.
if ! command -v clang >/dev/null 2>&1; then
  PKG=""
  for cand in /usr/bin/dnf /usr/bin/yum /bin/dnf /bin/yum; do
    if [ -x "$cand" ]; then PKG="$cand"; break; fi
  done
  [ -z "$PKG" ] && PKG="$(command -v dnf 2>/dev/null || command -v yum 2>/dev/null || true)"
  if [ -z "$PKG" ]; then
    echo "no package manager found to install clang" >&2
    exit 1
  fi
  "$PKG" install -y clang
fi

dotnet publish /project/src/Geopack.Native/Geopack.Native.csproj \
  -c Release -r "$RID" -o /tmp/native-out

LIB="$(find /tmp/native-out -maxdepth 1 -type f \( -name 'geopack.so' -o -name 'libgeopack.so' \) | head -n1)"
[[ -n "$LIB" ]] || { echo "no native library produced" >&2; ls -la /tmp/native-out; exit 1; }
cp "$LIB" /project/python/geopack/
