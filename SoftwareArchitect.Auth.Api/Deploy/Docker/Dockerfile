FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY SoftwareArchitect.Api/*.csproj ./SoftwareArchitect.Api/
COPY SoftwareArchitect.Common.Models/*.csproj ./SoftwareArchitect.Common.Models/
COPY SoftwareArchitect.Storages/*.csproj ./SoftwareArchitect.Storages/
COPY SoftwareArchitect.Job/*.csproj ./SoftwareArchitect.Job/
RUN dotnet restore ./SoftwareArchitect.Api/SoftwareArchitect.Api.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish ./SoftwareArchitect.Api -c Release -o out
RUN dotnet publish ./SoftwareArchitect.Job -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "SoftwareArchitect.Api.dll"]