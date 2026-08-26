#!/usr/bin/env bash

# Examples:
# - ./migrations-add.sh InitialCreate Api/Hanamilegal.Web.AccountsApi AccountsDbContext
# - ./migrations-add.sh InitialCreate Api/Hanamilegal.Web.ApplicationsApi ApplicationsDbContext

set -euo pipefail

if [ $# -lt 1 ]; then
  echo "error: migration name not specified" >&2
  exit 1
fi

if [ $# -lt 2 ]; then
  echo "error: project path not specified" >&2
  exit 1
fi

if [ $# -lt 3 ]; then
  echo "error: context name not specified" >&2
  exit 1
fi

MIGRATION_NAME="$1"
PROJECT_PATH="$2"
CONTEXT_NAME="$3"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

# Load ACCOUNTS_DB_CONNECTION_STRING and APPLICATIONS_DB_CONNECTION_STRING
source "$SCRIPT_DIR/migrations-load-env.sh"

dotnet ef migrations add "$MIGRATION_NAME" \
  --project "$PROJECT_PATH" \
  --startup-project "$PROJECT_PATH" \
  --context "$CONTEXT_NAME"
