#!/usr/bin/env bash

set -euo pipefail

MODE="${1:-production}"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$SCRIPT_DIR/.."
ENV_FILE="$PROJECT_ROOT/.env"

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

MODE="${MODE^}"

# Load ACCOUNTS_DB_CONNECTION_STRING and APPLICATIONS_DB_CONNECTION_STRING
source "$SCRIPT_DIR/migrations-load-env.sh"

IMAGE_NAME="hanamilegal-applications-api-migrations:latest"
NETWORK_NAME="hanamilegal-web_hanamilegal-web"

docker build \
  --file "$PROJECT_ROOT/Api/Hanamilegal.Web.ApplicationsApi/Dockerfile.migrations" \
  --build-arg "APPLICATIONS_DB_CONNECTION_STRING=$APPLICATIONS_DB_CONNECTION_STRING" \
  --tag "$IMAGE_NAME" \
  "$PROJECT_ROOT"

docker run --rm \
  --name "ApplicationsApiMigrations" \
  --network "$NETWORK_NAME" \
  -e "ASPNETCORE_ENVIRONMENT=$MODE" \
  -e "APPLICATIONS_DB_CONNECTION_STRING=$APPLICATIONS_DB_CONNECTION_STRING" \
  "$IMAGE_NAME"
