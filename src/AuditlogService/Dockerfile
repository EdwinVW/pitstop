FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy necessary files and restore as distinct layer
COPY *.csproj ./
RUN dotnet restore


# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1
COPY --from=build-env /app/out .

# Start
ENTRYPOINT ["dotnet", "Pitstop.AuditlogService.dll"]
