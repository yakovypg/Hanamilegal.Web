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

if [ $# -lt 3 ]; then
  echo "error: path to the docker compose file not specified" >&2
  exit 1
fi

if [ $# -lt 4 ]; then
  echo "error: docker compose service name not specified" >&2
  exit 1
fi

PROJECT_PATH="$1"
CONTEXT_NAME="$2"
DOCKER_COMPOSE_FILE_PATH="$3"
DOCKER_COMPOSE_SERVICE_NAME="$4"

docker compose -f "$DOCKER_COMPOSE_FILE_PATH" exec "$DOCKER_COMPOSE_SERVICE_NAME" \
  dotnet ef database update \
    --project "$PROJECT_PATH" \
    --startup-project "$PROJECT_PATH" \
    --context "$CONTEXT_NAME"
