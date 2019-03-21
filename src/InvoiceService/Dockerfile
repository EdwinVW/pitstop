FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /app

# Copy necessary files and restore as distinct layer
COPY *.csproj ./
RUN dotnet restore -s https://api.nuget.org/v3/index.json -s https://www.myget.org/F/pitstop/api/v3/index.json

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.2-runtime
COPY --from=build-env /app/out .

# Start
ENTRYPOINT ["dotnet", "Pitstop.InvoiceService.dll"]
