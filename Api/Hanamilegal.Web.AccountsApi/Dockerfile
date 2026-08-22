FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app/src

# Copy .csproj files
COPY ./Shared/Hanamilegal.Web.ApiConfiguration/Hanamilegal.Web.ApiConfiguration.csproj ./Shared/Hanamilegal.Web.ApiConfiguration/
COPY ./Shared/Hanamilegal.Web.Auth/Hanamilegal.Web.Auth.csproj ./Shared/Hanamilegal.Web.Auth/
COPY ./Api/Hanamilegal.Web.AccountsApi/Hanamilegal.Web.AccountsApi.csproj ./Api/Hanamilegal.Web.AccountsApi/

# Restore dependencies
RUN dotnet restore ./Api/Hanamilegal.Web.AccountsApi/Hanamilegal.Web.AccountsApi.csproj

# Copy sources
COPY ./Shared/Hanamilegal.Web.ApiConfiguration/. ./Shared/Hanamilegal.Web.ApiConfiguration/
COPY ./Shared/Hanamilegal.Web.Auth/. ./Shared/Hanamilegal.Web.Auth/
COPY ./Api/Hanamilegal.Web.AccountsApi/. ./Api/Hanamilegal.Web.AccountsApi/

# Publish application
RUN dotnet publish \
    -c Release \
    -o /app/publish \
    ./Api/Hanamilegal.Web.AccountsApi/Hanamilegal.Web.AccountsApi.csproj

# Use runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime

# Create final image
WORKDIR /app
COPY --from=build /app/publish ./

# Run application
ENTRYPOINT ["dotnet", "Hanamilegal.Web.AccountsApi.dll"]
