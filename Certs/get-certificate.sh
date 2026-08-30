#!/usr/bin/env bash

set -euo pipefail

if [ $# -lt 1 ]; then
  echo "error: domains not specified" >&2
  echo "usage: $0 <domain1> [domain2] [domain3] ..." >&2
  exit 1
fi

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

if ! command -v certbot >/dev/null 2>&1; then
  echo "error: certbot not found" >&2
  exit 2
fi

PRIMARY_DOMAIN="$1"
DOMAIN_FLAGS=()

for domain in "$@"; do
  DOMAIN_FLAGS+=(-d "$domain")
done

sudo certbot certonly --standalone "${DOMAIN_FLAGS[@]}"

"$SCRIPT_DIR/cp-certificate.sh" "$PRIMARY_DOMAIN"
