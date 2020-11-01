FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY SoftwareArchitect.Service.Users/*.csproj ./SoftwareArchitect.Service.Users/
COPY SoftwareArchitect.Job/*.csproj ./SoftwareArchitect.Job/
RUN dotnet restore ./SoftwareArchitect.Api/SoftwareArchitect.Service.Users.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish ./SoftwareArchitect.Service.Users -c Release -o out
RUN dotnet publish ./SoftwareArchitect.Job -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "SoftwareArchitect.Service.Users.dll"]