FROM pitstop-dotnet-sdk-base:1.0 AS build-env
WORKDIR /app

# Copy necessary files and restore as distinct layer
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM pitstop-dotnet-runtime-base:1.0
WORKDIR /app
COPY --from=build-env /app/out .

# Start
ENTRYPOINT ["dotnet", "Pitstop.AuditlogService.dll"]
