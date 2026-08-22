FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app/src

# Copy .csproj files
COPY ./Shared/Hanamilegal.Web.ApiConfiguration/Hanamilegal.Web.ApiConfiguration.csproj ./Shared/Hanamilegal.Web.ApiConfiguration/
COPY ./Shared/Hanamilegal.Web.Auth/Hanamilegal.Web.Auth.csproj ./Shared/Hanamilegal.Web.Auth/
COPY ./Api/Hanamilegal.Web.ApplicationsApi/Hanamilegal.Web.ApplicationsApi.csproj ./Api/Hanamilegal.Web.ApplicationsApi/

# Restore dependencies
RUN dotnet restore ./Api/Hanamilegal.Web.ApplicationsApi/Hanamilegal.Web.ApplicationsApi.csproj

# Copy sources
COPY ./Shared/Hanamilegal.Web.ApiConfiguration/. ./Shared/Hanamilegal.Web.ApiConfiguration/
COPY ./Shared/Hanamilegal.Web.Auth/. ./Shared/Hanamilegal.Web.Auth/
COPY ./Api/Hanamilegal.Web.ApplicationsApi/. ./Api/Hanamilegal.Web.ApplicationsApi/

# Publish application
RUN dotnet publish \
    -c Release \
    -o /app/publish \
    ./Api/Hanamilegal.Web.ApplicationsApi/Hanamilegal.Web.ApplicationsApi.csproj

# Use runtime
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime

# Create final image
WORKDIR /app
COPY --from=build /app/publish ./

# Run application
ENTRYPOINT ["dotnet", "Hanamilegal.Web.ApplicationsApi.dll"]
