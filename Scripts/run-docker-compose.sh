#!/usr/bin/env bash

set -euo pipefail

# Examples:
# ./run-docker-compose.sh prod up -d --build
# ./run-docker-compose.sh dev down

if [ $# -lt 1 ]; then
  echo "error: mode not specified" >&2
  exit 1
fi

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

docker compose -f "$SCRIPT_DIR/../docker-compose-$MODE.yml" --env-file "$SCRIPT_DIR/../.env" "${@:2}"
