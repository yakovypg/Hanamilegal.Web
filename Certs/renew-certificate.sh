#!/usr/bin/env bash

set -euo pipefail

if [ $# -lt 1 ]; then
  echo "error: main domain not specified" >&2
  exit 1
fi

DOMAIN="$1"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

if ! command -v certbot >/dev/null 2>&1; then
  echo "error: certbot not found" >&2
  exit 2
fi

sudo certbot renew --cert-name "$DOMAIN"

"$SCRIPT_DIR/cp-certificate.sh" "$DOMAIN"
