#!/usr/bin/env bash

set -euo pipefail

DAYS="365"
CN="localhost"

CRT_FILE_NAME="fullchain.pem"
KEY_FILE_NAME="privkey.pem"
BASE64_CRT_FILE_NAME="${CRT_FILE_NAME}.base64"
BASE64_KEY_FILE_NAME="${KEY_FILE_NAME}.base64"

# Without password
openssl req -x509 -nodes -newkey rsa:2048 \
  -days "$DAYS" \
  -subj "/CN=${CN}" \
  -addext "subjectAltName=DNS:${CN},IP:127.0.0.1" \
  -out "$CRT_FILE_NAME" \
  -keyout "$KEY_FILE_NAME"

base64 -w 0 "$CRT_FILE_NAME" > "$BASE64_CRT_FILE_NAME"
base64 -w 0 "$KEY_FILE_NAME" > "$BASE64_KEY_FILE_NAME"
