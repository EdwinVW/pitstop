FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

# Copy necessary files and restore as distinct layer
COPY *.csproj ./
RUN dotnet restore -s https://api.nuget.org/v3/index.json -s https://www.myget.org/F/pitstop/api/v3/index.json

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/aspnetcore:2.0
COPY --from=build-env /app/out .

# Expose ports
EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000

# Configure environment variables
ENV WEBSITE_SITE_NAME VehicleManagementAPI

# Start
ENTRYPOINT ["dotnet", "Pitstop.VehicleManagement.dll"]
