#!/usr/bin/env bash
# Builds the NativeAOT shared library for the current platform.
#
#   ./deploy/build_native.sh              # auto-detect RID from the host
#   RID=linux-x64 ./deploy/build_native.sh
#
# Output: deploy/out/<rid>/geopack.{dylib,so,dll}
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

# PublishAot/IsAotCompatible come from src/Directory.Build.props,
# NativeLib=Shared from src/Geopack.Native/Geopack.Native.csproj.
dotnet publish "$REPO_ROOT/src/Geopack.Native/Geopack.Native.csproj" \
  -c Release \
  -r "$RID" \
  -o "$OUT_DIR"

echo
echo "Native library ($RID):"
ls -1 "$OUT_DIR"/geopack.* 2>/dev/null || ls -1 "$OUT_DIR"
