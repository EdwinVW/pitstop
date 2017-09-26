FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

# Copy necessary files and restore as distinct layer
COPY *.csproj ./
COPY nuget /nuget
RUN dotnet restore -s https://nuget.org/api/v2/ -s /nuget

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/aspnetcore:2.0
COPY --from=build-env /app/out .

# Expose ports
EXPOSE 6000/tcp
ENV ASPNETCORE_URLS http://*:6000

# Start
ENTRYPOINT ["dotnet", "WebApp.dll"]
