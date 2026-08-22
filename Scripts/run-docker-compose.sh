#!/usr/bin/env bash

set -euo pipefail

# Examples:
# ./run-docker-compose.sh prod up -d --build
# ./run-docker-compose.sh dev down

if [ $# -lt 1 ]; then
  echo "error: mode not specified" >&2
  exit 1
fi

SPECIFIED_MODE="$1"
MODE="production"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

if [ "$SPECIFIED_MODE" = "dev" ] || [ "$SPECIFIED_MODE" = "development" ]; then
  MODE="development"
fi

docker compose -f "$SCRIPT_DIR/../docker-compose-$MODE.yml" --env-file "$SCRIPT_DIR/../.env" "${@:2}"
