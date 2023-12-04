FROM adamoctoclose/pitstop-dotnet-sdk-base:1.0 AS build-env
WORKDIR /app

# Copy necessary files and restore as distinct layer
COPY . .
WORKDIR "/app/src/WorkshopManagementAPI"
RUN dotnet restore

# Copy everything else and build
RUN dotnet publish "./WorkshopManagementAPI.csproj" -c Release -o /app/publish

# Build runtime image
FROM adamoctoclose/pitstop-dotnet-runtime-base:1.0
WORKDIR /app
COPY --from=build-env /app/publish .

# Expose ports
EXPOSE 5200/tcp
ENV ASPNETCORE_URLS http://*:5200
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:5200/hc || exit 1

# Start
ENTRYPOINT ["dotnet", "Pitstop.WorkshopManagementAPI.dll"]
