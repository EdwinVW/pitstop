docker volume create --name=sqlserverdata
docker volume create --name=rabbitmqdata

# Build the .NET SDK base image that contains the Directory.Packages.props file so it is used when restoring the NuGet packages
docker build -t pitstop-dotnet-sdk-base:1.0 . -f dotnet-sdk-base-dockerfile

# Build the .NET runtime base image
docker build -t pitstop-dotnet-runtime-base:1.0 . -f dotnet-runtime-base-dockerfile

# Build the .NET ASP.NET base image
docker build -t pitstop-dotnet-aspnet-base:1.0 . -f dotnet-aspnet-base-dockerfile

# Rebuild all the services that have changes
# If you want to (re)build only a specific service, go to the src folder and execute `docker compose build <servicename-lowercase>`
docker compose build --force-rm
