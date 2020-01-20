FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Install node
ENV NODE_VERSION 12.14.1
ENV NODE_DOWNLOAD_SHA e21be7dd07cc143c480695d6214f40873a7791f437c1ca12fc94d45f539a47d9
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
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
COPY --from=build-env /app/out .

# Expose ports
EXPOSE 7000/tcp
ENV ASPNETCORE_URLS http://*:7000
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:7000/hc || exit 1

# Start
ENTRYPOINT ["dotnet", "WebApp.dll"]
