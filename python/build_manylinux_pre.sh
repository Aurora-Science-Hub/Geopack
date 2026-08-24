#!/usr/bin/env bash
# Runs inside the cibuildwheel manylinux container (CIBW_BEFORE_ALL). Installs
# .NET, builds the NativeAOT shared library and bundles it into the package so
# the subsequent wheel build includes it.
set -euo pipefail

: "${REPO_ROOT:?}"   # set via CIBW_ENVIRONMENT to /host/<workspace>

case "$(uname -m)" in
  x86_64)  RID="linux-x64" ;;
  aarch64) RID="linux-arm64" ;;
  *) echo "unsupported arch: $(uname -m)" >&2; exit 1 ;;
esac

# .NET SDK 10 (glibc >= 2.27; manylinux_2_28 provides 2.28).
curl -sSL https://dot.net/v1/dotnet-install.sh | bash -s -- --channel 10.0
export PATH="$HOME/.dotnet:$PATH"
export DOTNET_CLI_TELEMETRY_OPTOUT=1 DOTNET_NOLOGO=1 DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1

# NativeAOT prerequisites (clang + zlib; the manylinux analogue of the Ubuntu
# runner's clang/zlib1g-dev).
yum install -y clang zlib-devel

# Copy the repo into a writable, container-local path so obj/ and the .so are
# written inside the container (the /host mount may be read-only).
cp -r "$REPO_ROOT" /tmp/repo
rm -rf /tmp/repo/.git

dotnet publish /tmp/repo/src/Geopack.Native/Geopack.Native.csproj \
  -c Release -r "$RID" -o /tmp/native-out

LIB="$(find /tmp/native-out -maxdepth 1 -type f \( -name 'geopack.so' -o -name 'libgeopack.so' \) | head -n1)"
[[ -n "$LIB" ]] || { echo "no native library produced" >&2; ls -la /tmp/native-out; exit 1; }
cp "$LIB" ./geopack/
