FROM microsoft/dotnet AS build-env
WORKDIR /app

# Copy necessary files and restore as distinct layer
COPY *.csproj ./
COPY nuget /nuget
RUN dotnet restore -s https://nuget.org/api/v2/ -s /nuget

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet
COPY --from=build-env /app/out .

# Start
ENTRYPOINT ["dotnet", "Pitstop.TimeService.dll"]
