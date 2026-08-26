#!/usr/bin/env bash

# Script be sourced: source migrations-load-env.sh
if [[ "${BASH_SOURCE[0]}" == "$0" ]]; then
    echo "error: script must be sourced (source $0)" >&2
    exit 1
fi

if [[ -n ${BASH_VERSION:-} ]]; then
    SOURCE_FILE="${BASH_SOURCE[0]}"
elif [[ -n ${ZSH_VERSION:-} ]]; then
    SOURCE_FILE="${(%):-%N}"
else
    echo "error: shell not supported" >&2
    return 1 2>/dev/null || exit 1
fi

SCRIPT_DIR="$(cd -- "$(dirname -- "$SOURCE_FILE")" && pwd)"
ENV_FILE="$SCRIPT_DIR/../.env"

if [[ ! -f "$ENV_FILE" ]]; then
    echo "error: .env file not found in '$ENV_FILE'" >&2
    return 1 2>/dev/null || exit 1
fi

set -a
source "$ENV_FILE"
set +a

export ACCOUNTS_DB_CONNECTION_STRING="Host=accounts-db;Port=12001;Database=${ACCOUNTS_DB_NAME};Username=${ACCOUNTS_DB_USER};Password=${ACCOUNTS_DB_PASSWORD}"
export APPLICATIONS_DB_CONNECTION_STRING="Host=applications-db;Port=12002;Database=${APPLICATIONS_DB_NAME};Username=${APPLICATIONS_DB_USER};Password=${APPLICATIONS_DB_PASSWORD}"
