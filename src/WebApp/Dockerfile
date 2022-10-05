FROM pitstop-dotnet-sdk-base:1.0 AS build-env
WORKDIR /app

# Install node
ENV NODE_VERSION 16.x
ENV NODE_DOWNLOAD_SHA 2c69e7b040c208b61ebf9735c63d2e5bcabfed32ef05a9b8dd5823489ea50d6b
RUN curl -fsSL "https://deb.nodesource.com/setup_${NODE_VERSION}" | bash -
RUN apt install -y nodejs && rm -rf /var/lib/apt/lists/*
RUN npm i npm@latest -g
RUN npm install gulp -g

# Copy necessary files and restore as distinct layer
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN npm install
RUN gulp default
RUN dotnet publish -c Release -o out

# Build runtime image
FROM pitstop-dotnet-aspnet-base:1.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose ports
EXPOSE 7005/tcp
ENV ASPNETCORE_URLS http://*:7005
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:7005/hc || exit 1

# Start
ENTRYPOINT ["dotnet", "Pitstop.WebApp.dll"]
