#!/usr/bin/env bash
# Builds the NativeAOT shared library for the current platform.
#
#   ./python/build_native.sh              # auto-detect RID from the host
#   RID=linux-x64 ./python/build_native.sh
#
# Output: python/out/<rid>/geopack.{dylib,so,dll}
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"

case "$(uname -s)-$(uname -m)" in
  Darwin-arm64)  RID="${RID:-osx-arm64}" ;;
  Darwin-x86_64) RID="${RID:-osx-x64}" ;;
  Linux-x86_64)  RID="${RID:-linux-x64}" ;;
  Linux-aarch64) RID="${RID:-linux-arm64}" ;;
  *)
    if [[ -z "${RID:-}" ]]; then
      echo "Unsupported platform: $(uname -s)-$(uname -m). Set RID explicitly." >&2
      exit 1
    fi
    ;;
esac

OUT_DIR="$SCRIPT_DIR/out/$RID"
mkdir -p "$OUT_DIR"

# NativeAOT may emit either geopack.{dylib,so,dll} or libgeopack.{dylib,so}.
# Remove a leftover file with the other name so it cannot shadow the fresh
# build in the listing below or in build_wheel.sh's candidate search.
rm -f "$OUT_DIR"/geopack.dylib "$OUT_DIR"/libgeopack.dylib \
      "$OUT_DIR"/geopack.so "$OUT_DIR"/libgeopack.so \
      "$OUT_DIR"/geopack.dll

# PublishAot/IsAotCompatible come from src/Directory.Build.props,
# NativeLib=Shared from src/Geopack.Native/Geopack.Native.csproj.
dotnet publish "$REPO_ROOT/src/Geopack.Native/Geopack.Native.csproj" \
  -c Release \
  -r "$RID" \
  -o "$OUT_DIR"

echo
echo "Native library ($RID):"
library="$(find "$OUT_DIR" -maxdepth 1 -type f \
  \( -name 'geopack.dylib' -o -name 'libgeopack.dylib' \
     -o -name 'geopack.so' -o -name 'libgeopack.so' \
     -o -name 'geopack.dll' \) -print -quit)"
if [[ -z "$library" ]]; then
  echo "No native library found in $OUT_DIR" >&2
  exit 1
fi
echo "$library"
