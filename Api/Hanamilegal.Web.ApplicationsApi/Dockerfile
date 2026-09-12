FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app/src

# Copy configuration files
COPY Directory.Build.props ./
COPY Directory.Packages.props ./
COPY stylecop.json ./
COPY .ruleset ./
COPY .editorconfig ./

# Copy solution and .csproj files
COPY Hanamilegal.Web.slnx ./
COPY Api/Hanamilegal.Web.ApplicationsApi/Hanamilegal.Web.ApplicationsApi.csproj Api/Hanamilegal.Web.ApplicationsApi/
COPY Shared/Hanamilegal.Web.ApiCommon/Hanamilegal.Web.ApiCommon.csproj Shared/Hanamilegal.Web.ApiCommon/
COPY Shared/Hanamilegal.Web.ApiConfiguration/Hanamilegal.Web.ApiConfiguration.csproj Shared/Hanamilegal.Web.ApiConfiguration/
COPY Shared/Hanamilegal.Web.Auth/Hanamilegal.Web.Auth.csproj Shared/Hanamilegal.Web.Auth/
COPY Shared/Hanamilegal.Web.Contracts/Hanamilegal.Web.Contracts.csproj Shared/Hanamilegal.Web.Contracts/

# Restore dependencies
RUN dotnet restore Api/Hanamilegal.Web.ApplicationsApi/Hanamilegal.Web.ApplicationsApi.csproj

# Copy sources
COPY Api/Hanamilegal.Web.ApplicationsApi/. Api/Hanamilegal.Web.ApplicationsApi/
COPY Shared/Hanamilegal.Web.ApiCommon/. Shared/Hanamilegal.Web.ApiCommon/
COPY Shared/Hanamilegal.Web.ApiConfiguration/. Shared/Hanamilegal.Web.ApiConfiguration/
COPY Shared/Hanamilegal.Web.Auth/. Shared/Hanamilegal.Web.Auth/
COPY Shared/Hanamilegal.Web.Contracts/. Shared/Hanamilegal.Web.Contracts/

# Publish application
RUN dotnet publish \
    Api/Hanamilegal.Web.ApplicationsApi/Hanamilegal.Web.ApplicationsApi.csproj \
    -c Release \
    -o /app/publish

# Use runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

# Create final image
WORKDIR /app
COPY --from=build /app/publish ./

# Run application
ENTRYPOINT ["dotnet", "Hanamilegal.Web.ApplicationsApi.dll"]
