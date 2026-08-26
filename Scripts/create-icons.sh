#!/usr/bin/env bash

# Required packages:
# sudo apt install inkscape icoutils

set -euo pipefail

if [[ $# -lt 1 ]]; then
  echo "Usage: $0 SVG_ICON_PATH [OUTPUT_DIR]" >&2
  exit 1
fi

SCRIPT_DIR="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)"

SVG_ICON_PATH="$1"
OUTPUT_DIR="${2:-$SCRIPT_DIR}"

if [[ ! -f "$SVG_ICON_PATH" ]]; then
  echo "error: SVG file not found: $SVG_ICON_PATH" >&2
  exit 1
fi

if [[ ! -d "$OUTPUT_DIR" ]]; then
  mkdir -p "$OUTPUT_DIR"
fi

FAVICON_PATH="$OUTPUT_DIR/favicon.ico"
FAVICON_16x16_PATH="$OUTPUT_DIR/favicon-16x16.png"
FAVICON_32x32_PATH="$OUTPUT_DIR/favicon-32x32.png"
FAVICON_48x48_PATH="$OUTPUT_DIR/favicon-48x48.png"
FAVICON_64x64_PATH="$OUTPUT_DIR/favicon-64x64.png"
FAVICON_128x128_PATH="$OUTPUT_DIR/favicon-128x128.png"
FAVICON_256x256_PATH="$OUTPUT_DIR/favicon-256x256.png"
ANDROID_CHROME_192x192_PATH="$OUTPUT_DIR/android-chrome-192x192.png"
ANDROID_CHROME_512x512_PATH="$OUTPUT_DIR/android-chrome-512x512.png"
APPLE_TOUCH_ICON_180x180_PATH="$OUTPUT_DIR/apple-touch-icon-180x180.png"

create_png() {
  local input_path="$1"
  local output_path="$2"
  local size_px="$3"

  inkscape "$input_path" \
    --export-filename="$output_path" \
    --export-width="$size_px" \
    --export-height="$size_px"
}

create_ico() {
  local output_path="$1"
  shift
  local input_paths=("$@")

  icotool \
    -c \
    -o "$output_path" \
    "${input_paths[@]}"
}

create_png "$SVG_ICON_PATH" "$FAVICON_16x16_PATH" "16"
create_png "$SVG_ICON_PATH" "$FAVICON_32x32_PATH" "32"
create_png "$SVG_ICON_PATH" "$FAVICON_48x48_PATH" "48"
create_png "$SVG_ICON_PATH" "$FAVICON_64x64_PATH" "64"
create_png "$SVG_ICON_PATH" "$FAVICON_128x128_PATH" "128"
create_png "$SVG_ICON_PATH" "$FAVICON_256x256_PATH" "256"
create_png "$SVG_ICON_PATH" "$ANDROID_CHROME_192x192_PATH" "192"
create_png "$SVG_ICON_PATH" "$ANDROID_CHROME_512x512_PATH" "512"
create_png "$SVG_ICON_PATH" "$APPLE_TOUCH_ICON_180x180_PATH" "180"

create_ico \
  "$FAVICON_PATH" \
  "$FAVICON_16x16_PATH" \
  "$FAVICON_32x32_PATH" \
  "$FAVICON_48x48_PATH" \
  "$FAVICON_64x64_PATH" \
  "$FAVICON_128x128_PATH" \
  "$FAVICON_256x256_PATH"

echo "Icons successfully created in $OUTPUT_DIR"
