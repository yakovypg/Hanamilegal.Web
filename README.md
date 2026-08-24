<h1 align="center">Hanamilehal.Web</h1>
<p align="center">
  <img alt="ypdf" height="200" src="https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExOXFmc21tZjN4OGY2cXRyaTNqdzQwdHY3ZmRyOXdib240cmY5M2hsZCZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/z5zWcuq8qDfrl9B3Tp/giphy.gif" />
</p>

## Table of Contents

- [Initial Deployment](#initial-deployment)
- [Updating Application](#updating-application)

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

If prompted for credentials, enter your GitLab username and use your GitLab Personal Access Token as the password.

Update the `.env` file if necessary.

Apply any pending database migrations:

```bash
./Scripts/migrations-apply.sh Api/Hanamilegal.Web.AccountsApi AccountsDbContext
./Scripts/migrations-apply.sh Api/Hanamilegal.Web.ApplicationsApi ApplicationsDbContext
```

Finally, rebuild and restart the Docker containers:

```bash
./Scripts/containers-build.sh
./Scripts/containers-up.sh
```
