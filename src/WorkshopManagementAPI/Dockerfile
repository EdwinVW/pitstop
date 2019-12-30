FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy necessary files and restore as distinct layer
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
COPY --from=build-env /app/out .

# Expose ports
EXPOSE 5200/tcp
ENV ASPNETCORE_URLS http://*:5200
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:5200/hc || exit 1

# Start
ENTRYPOINT ["dotnet", "Pitstop.WorkshopManagementAPI.dll"]
