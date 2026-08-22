#!/usr/bin/env bash
# Builds the native library, bundles it into the Python package, and produces a
# platform-specific wheel in python/dist/.
#
#   ./python/build_wheel.sh
#   pip install python/dist/geopack_2008-*.whl
#
# Requires: .NET SDK 10, a C toolchain (for NativeAOT), and Python 3.8+.
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PY_DIR="$SCRIPT_DIR"
PKG_DIR="$PY_DIR/geopack"
DIST_DIR="$SCRIPT_DIR/dist"

# 1. Native library ---------------------------------------------------------
"$SCRIPT_DIR/build_native.sh"

# build_native.sh picks its own default RID; mirror the same detection here so
# we can find the artifact.
case "$(uname -s)-$(uname -m)" in
  Darwin-arm64)  RID="${RID:-osx-arm64}" ;;
  Darwin-x86_64) RID="${RID:-osx-x64}" ;;
  Linux-x86_64)  RID="${RID:-linux-x64}" ;;
  Linux-aarch64) RID="${RID:-linux-arm64}" ;;
esac

OUT_DIR="$SCRIPT_DIR/out/$RID"
LIB=""
# Accept both the plain name and the lib-prefixed name NativeAOT may produce on Unix.
for _candidate in \
  "$OUT_DIR"/geopack.dylib "$OUT_DIR"/libgeopack.dylib \
  "$OUT_DIR"/geopack.so "$OUT_DIR"/libgeopack.so \
  "$OUT_DIR"/geopack.dll; do
  if [[ -f "$_candidate" ]]; then
    LIB="$_candidate"
    break
  fi
done
if [[ -z "$LIB" ]]; then
  echo "No native library found in $OUT_DIR (expected geopack.dylib/.so/.dll)" >&2
  exit 1
fi

# 2. Bundle into the package ------------------------------------------------
cp "$LIB" "$PKG_DIR/"

# 3. Build the wheel --------------------------------------------------------
rm -rf "$PY_DIR/build" "$DIST_DIR" "$PY_DIR"/*.egg-info
mkdir -p "$DIST_DIR"

if python3 -m build --version &>/dev/null; then
  python3 -m build --wheel --outdir "$DIST_DIR" "$PY_DIR"
else
  # Fallback: pip wheel (PEP 517) with no runtime deps.
  python3 -m pip wheel --no-deps --wheel-dir "$DIST_DIR" "$PY_DIR"
fi

echo "Wheel:"
ls -1 "$DIST_DIR"/*.whl
