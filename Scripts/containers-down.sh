#!/usr/bin/env bash

set -euo pipefail

SPECIFIED_MODE="${1:-}"
MODE="production"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

if [ "$SPECIFIED_MODE" = "dev" ] || [ "$SPECIFIED_MODE" = "development" ]; then
  MODE="development"
fi

"$SCRIPT_DIR/run-docker-compose.sh" "$MODE" down
