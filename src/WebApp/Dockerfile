FROM pitstop-dotnet-sdk-base:1.0 AS build-env
WORKDIR /app

# Install node
ENV NODE_VERSION 16.14.0
ENV NODE_DOWNLOAD_SHA 2c69e7b040c208b61ebf9735c63d2e5bcabfed32ef05a9b8dd5823489ea50d6b
RUN curl -SL "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
    && echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c - \
    && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
    && rm nodejs.tar.gz \
    && ln -s /usr/local/bin/node /usr/local/bin/nodejs
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
COPY --from=build-env /app/out .

# Expose ports
EXPOSE 7000/tcp
ENV ASPNETCORE_URLS http://*:7000
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:7000/hc || exit 1

# Start
ENTRYPOINT ["dotnet", "Pitstop.WebApp.dll"]
