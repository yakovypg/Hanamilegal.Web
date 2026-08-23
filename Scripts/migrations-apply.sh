#!/usr/bin/env bash

set -euo pipefail

if [ $# -lt 1 ]; then
  echo "error: project path not specified" >&2
  exit 1
fi

if [ $# -lt 2 ]; then
  echo "error: context name not specified" >&2
  exit 1
fi

PROJECT_PATH="$1"
CONTEXT_NAME="$2"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/migrations-load-env.sh"

dotnet ef database update \
  --project "$PROJECT_PATH" \
  --startup-project "$PROJECT_PATH" \
  --context "$CONTEXT_NAME"
