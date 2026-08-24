#!/usr/bin/env bash

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

DAYS="365"
CRT_FILE_PATH="${SCRIPT_DIR}/fullchain.pem"
KEY_FILE_PATH="${SCRIPT_DIR}/privkey.pem"
BASE64_CRT_FILE_PATH="${CRT_FILE_PATH}.base64"
BASE64_KEY_FILE_PATH="${KEY_FILE_PATH}.base64"

DNS_NAMES=(
  "localhost"
  "accounts-api"
  "applications-api"
)

IP_ADDRESSES=(
  "127.0.0.1"
)

SAN_ENTRIES=()

for dns_name in "${DNS_NAMES[@]}"; do
  SAN_ENTRIES+=("DNS:${dns_name}")
done

for ip_address in "${IP_ADDRESSES[@]}"; do
  SAN_ENTRIES+=("IP:${ip_address}")
done

CN="${DNS_NAMES[0]}"
SUBJECT_ALT_NAME=$(IFS=,; echo "${SAN_ENTRIES[*]}")

# Without password
openssl req -x509 -nodes -newkey rsa:2048 \
  -days "$DAYS" \
  -subj "/CN=${CN}" \
  -addext "subjectAltName=${SUBJECT_ALT_NAME}" \
  -out "$CRT_FILE_PATH" \
  -keyout "$KEY_FILE_PATH"

base64 -w 0 "$CRT_FILE_PATH" > "$BASE64_CRT_FILE_PATH"
base64 -w 0 "$KEY_FILE_PATH" > "$BASE64_KEY_FILE_PATH"

chmod 644 "$CRT_FILE_PATH"
chmod 600 "$KEY_FILE_PATH"
chmod 644 "$BASE64_CRT_FILE_PATH"
chmod 600 "$BASE64_KEY_FILE_PATH"
