#!/usr/bin/env bash

# Required packages:
# sudo apt install inkscape icoutils imagemagick

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

FAVICON_ICO_PATH="$OUTPUT_DIR/favicon.ico"
FAVICON_PNG_PATH="$OUTPUT_DIR/favicon.png"
FAVICON_16x16_PATH="$OUTPUT_DIR/favicon-16x16.png"
FAVICON_32x32_PATH="$OUTPUT_DIR/favicon-32x32.png"
FAVICON_48x48_PATH="$OUTPUT_DIR/favicon-48x48.png"
FAVICON_64x64_PATH="$OUTPUT_DIR/favicon-64x64.png"
FAVICON_96x96_PATH="$OUTPUT_DIR/favicon-96x96.png"
FAVICON_120x120_PATH="$OUTPUT_DIR/favicon-120x120.png"
FAVICON_128x128_PATH="$OUTPUT_DIR/favicon-128x128.png"
FAVICON_256x256_PATH="$OUTPUT_DIR/favicon-256x256.png"
ANDROID_CHROME_192x192_PATH="$OUTPUT_DIR/android-chrome-192x192.png"
ANDROID_CHROME_512x512_PATH="$OUTPUT_DIR/android-chrome-512x512.png"
APPLE_TOUCH_ICON_180x180_PATH="$OUTPUT_DIR/apple-touch-icon-180x180.png"
OG_IMAGE_1200x630_PATH="$OUTPUT_DIR/og-image-1200x630.png"

create_png() {
  local input_path="$1"
  local output_path="$2"
  local size_px="$3"

  # inkscape
  inkscape "$input_path" \
    --export-filename="$output_path" \
    --export-width="$size_px" \
    --export-height="$size_px"
}

create_og_image() {
  local input_path="$1"
  local output_path="$2"
  local background_color="${3:-white}"
  local canvas_size="${4:-1200x630}"

  # imagemagick
  convert \
    -size "$canvas_size" \
    "xc:$background_color" \
    "$input_path" \
    -gravity center \
    -composite \
    "$output_path"
}

create_ico() {
  local output_path="$1"
  shift
  local input_paths=("$@")

  # icoutils
  icotool \
    -c \
    -o "$output_path" \
    "${input_paths[@]}"
}

create_png "$SVG_ICON_PATH" "$FAVICON_16x16_PATH" "16"
create_png "$SVG_ICON_PATH" "$FAVICON_32x32_PATH" "32"
create_png "$SVG_ICON_PATH" "$FAVICON_48x48_PATH" "48"
create_png "$SVG_ICON_PATH" "$FAVICON_64x64_PATH" "64"
create_png "$SVG_ICON_PATH" "$FAVICON_96x96_PATH" "96"
create_png "$SVG_ICON_PATH" "$FAVICON_120x120_PATH" "120"
create_png "$SVG_ICON_PATH" "$FAVICON_128x128_PATH" "128"
create_png "$SVG_ICON_PATH" "$FAVICON_256x256_PATH" "256"
create_png "$SVG_ICON_PATH" "$ANDROID_CHROME_192x192_PATH" "192"
create_png "$SVG_ICON_PATH" "$ANDROID_CHROME_512x512_PATH" "512"
create_png "$SVG_ICON_PATH" "$APPLE_TOUCH_ICON_180x180_PATH" "180"

create_og_image "$ANDROID_CHROME_512x512_PATH" "$OG_IMAGE_1200x630_PATH" white 1200x630

create_ico \
  "$FAVICON_ICO_PATH" \
  "$FAVICON_16x16_PATH" \
  "$FAVICON_32x32_PATH" \
  "$FAVICON_48x48_PATH" \
  "$FAVICON_64x64_PATH" \
  "$FAVICON_96x96_PATH" \
  "$FAVICON_120x120_PATH" \
  "$FAVICON_128x128_PATH" \
  "$FAVICON_256x256_PATH"

cp "$FAVICON_120x120_PATH" "$FAVICON_PNG_PATH"

echo "Icons successfully created in $OUTPUT_DIR"
