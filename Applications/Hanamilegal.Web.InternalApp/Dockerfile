FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app/src

# Copy .csproj files
COPY ./Shared/Hanamilegal.Web.ApiConfiguration/Hanamilegal.Web.ApiConfiguration.csproj ./Shared/Hanamilegal.Web.ApiConfiguration/
COPY ./Shared/Hanamilegal.Web.Auth/Hanamilegal.Web.Auth.csproj ./Shared/Hanamilegal.Web.Auth/
COPY ./Applications/Hanamilegal.Web.InternalApp/Hanamilegal.Web.InternalApp.csproj ./Applications/Hanamilegal.Web.InternalApp/

# Restore dependencies
RUN dotnet restore ./Applications/Hanamilegal.Web.InternalApp/Hanamilegal.Web.InternalApp.csproj

# Copy sources
COPY ./Shared/Hanamilegal.Web.ApiConfiguration/. ./Shared/Hanamilegal.Web.ApiConfiguration/
COPY ./Shared/Hanamilegal.Web.Auth/. ./Shared/Hanamilegal.Web.Auth/
COPY ./Applications/Hanamilegal.Web.InternalApp/. ./Applications/Hanamilegal.Web.InternalApp/

# Publish application
RUN dotnet publish \
    -c Release \
    -o /app/publish \
    ./Applications/Hanamilegal.Web.InternalApp/Hanamilegal.Web.InternalApp.csproj

# Use runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime

# Create final image
WORKDIR /app
COPY --from=build /app/publish ./

# Run application
ENTRYPOINT ["dotnet", "Hanamilegal.Web.InternalApp.dll"]
