#!/usr/bin/env bash

set -euo pipefail

if [ $# -lt 1 ]; then
  echo "error: domain not specified" >&2
  exit 1
fi

DOMAIN=$1

CERTBOT_CERTS_DIR="/etc/letsencrypt/live/$DOMAIN"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

CERTIFICATE_FILE_NAME="fullchain.pem"
PRIVATE_KEY_FILE_NAME="privkey.pem"

if [ ! -d "$CERTBOT_CERTS_DIR" ]; then
  echo "error: certificate directory not found: $CERTBOT_CERTS_DIR" >&2
  exit 2
fi

sudo cp "$CERTBOT_CERTS_DIR/$CERTIFICATE_FILE_NAME" "$SCRIPT_DIR/"
sudo cp "$CERTBOT_CERTS_DIR/$PRIVATE_KEY_FILE_NAME" "$SCRIPT_DIR/"

sudo chown \
  --reference="$SCRIPT_DIR" \
  "$SCRIPT_DIR/$CERTIFICATE_FILE_NAME" \
  "$SCRIPT_DIR/$PRIVATE_KEY_FILE_NAME" \
  || true

sudo chmod 644 "$SCRIPT_DIR/$CERTIFICATE_FILE_NAME"
sudo chmod 600 "$SCRIPT_DIR/$PRIVATE_KEY_FILE_NAME"
