# Build application with dotnet core sdk
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/ ./
RUN dotnet restore DrTrendify.Api/DrTrendify.Api.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish DrTrendify.Api/DrTrendify.Api.csproj --configuration Release --output out

# Build runtime image with dotnet core runtime
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "DrTrendify.Api.dll" ]
