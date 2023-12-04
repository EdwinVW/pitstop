FROM adamoctoclose/pitstop-dotnet-sdk-base:1.0 AS build-env
WORKDIR /app

# Copy necessary files and restore as distinct layer
COPY . .
WORKDIR "/app/src/WorkshopManagementEventHandler"
RUN dotnet restore

# Copy everything else and build
RUN dotnet publish "./WorkshopManagementEventHandler.csproj" -c Release -o /app/publish

# Build runtime image
FROM adamoctoclose/pitstop-dotnet-runtime-base:1.0
WORKDIR /app
COPY --from=build-env /app/publish .

# Start
ENTRYPOINT ["dotnet", "Pitstop.WorkshopManagementEventHandler.dll"]
