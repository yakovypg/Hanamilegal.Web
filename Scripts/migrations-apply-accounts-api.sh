#!/usr/bin/env bash

set -euo pipefail

MODE="${1:-production}"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

if [ "$MODE" != "prod" ] && \
   [ "$MODE" != "production" ] && \
   [ "$MODE" != "dev" ] && \
   [ "$MODE" != "development" ]; then
  echo "error: mode is unknown" >&2
  exit 1
fi

if [ "$MODE" = "dev" ] || [ "$MODE" = "development" ]; then
  MODE="development"
fi

"$SCRIPT_DIR/migrations-apply.sh" \
    "Api/Hanamilegal.Web.AccountsApi" \
    "AccountsDbContext" \
    "$SCRIPT_DIR/../docker-compose-$MODE.yml" \
    "accounts-api"
