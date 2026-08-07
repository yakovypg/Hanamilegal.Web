#!/usr/bin/env bash

set -euo pipefail

if [ $# -lt 1 ]; then
  echo "error: domain not specified" >&2
  exit 1
fi

DOMAIN="$1"
RENEW="${2:-false}"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

if ! command -v certbot >/dev/null 2>&1; then
  echo "error: certbot not found" >&2
  exit 2
fi

if [ "$RENEW" = "true" ] || [ "$RENEW" = "1" ]; then
  sudo certbot renew --cert-name "$DOMAIN"
else
  sudo certbot certonly --standalone -d "$DOMAIN"
fi

"$SCRIPT_DIR/cp-certificate.sh" "$DOMAIN"
