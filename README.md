# Hanamilehal.Web

Hanamilegal software suite.

## Table of Contents

- [Architecture](#architecture)
- [Initial Deployment](#initial-deployment)
- [Updating Application](#updating-application)

## Architecture

The architecture of the Hanamilegal software suite is illustrated below:

![Hanamilegal software suite architecture](/Docs/Hanamilegal.Web.Diagram.svg)

The diagram includes the following components:

- **Proxy** — manages DNS and routes incoming requests.
- **LegalSupportApp** — a web application for legal support processes.
- **TechApp** — a web application for software development and technical operations.
- **InternalApp** — an internal web application for reviewing and managing applications and related data.
- **AccountsApi** — an API for managing users, roles, and related account data.
- **ApplicationsApi** — an API for managing applications and their related data.
- **AccountsDb** — a database for storing user, role, and account-related data.
- **ApplicationsDb** — a database for storing application data.
- **ConsentsDb** — a database for storing consent records, including consent for the processing of personal data.

## Initial Deployment

First, create a `.env` file in the repository root using `.env.template` as a template.

Next, create a TLS certificate and place `fullchain.pem` and `privkey.pem` in the `Certs` directory. You can use the `Certs/get-certificate.sh` and `Certs/cp-certificate.sh` scripts, or generate a development certificate with `Certs/generate-dev-cert.sh`.

Build and start the Docker containers:

```bash
./Scripts/containers-build.sh
./Scripts/containers-up.sh
```

Next, install the .NET SDK and dotnet-ef if they are not already installed:

```bash
sudo add-apt-repository ppa:dotnet/backports
sudo apt install dotnet-sdk-10.0
dotnet tool install --global dotnet-ef
```

Build the applications locally. This generates files such as `Api/Hanamilegal.Web.AccountsApi/obj/project.assets.json`, which are required to apply the migrations:

```bash
dotnet build -c Release
```

Finally, apply the database migrations:

```bash
./Scripts/migrations-apply.sh Api/Hanamilegal.Web.AccountsApi AccountsDbContext
./Scripts/migrations-apply.sh Api/Hanamilegal.Web.ApplicationsApi ApplicationsDbContext
```

## Updating Application

First, pull the latest changes from the main branch:

```bash
git pull origin main
```

Update the `.env` file if necessary.

Apply any pending database migrations:

```bash
./Scripts/migrations-apply-accounts-api.sh
./Scripts/migrations-apply-applications-api.sh
```

Finally, rebuild and restart the Docker containers:

```bash
./Scripts/containers-build.sh
./Scripts/containers-up.sh
```
