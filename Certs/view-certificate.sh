#!/usr/bin/env bash

CERTIFICATE_PATH="${1:-fullchain.pem}"

set -euo pipefail
openssl x509 -in "$CERTIFICATE_PATH" -noout -text
