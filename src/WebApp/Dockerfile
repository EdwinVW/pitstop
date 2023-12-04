FROM adamoctoclose/pitstop-dotnet-sdk-base:1.0 AS build-env
WORKDIR /app

# Install node
RUN apt-get update
RUN apt-get install -y ca-certificates curl gnupg
RUN mkdir -p /etc/apt/keyrings
RUN curl -fsSL https://deb.nodesource.com/gpgkey/nodesource-repo.gpg.key | gpg --dearmor -o /etc/apt/keyrings/nodesource.gpg
ENV NODE_MAJOR=18
RUN echo "deb [signed-by=/etc/apt/keyrings/nodesource.gpg] https://deb.nodesource.com/node_$NODE_MAJOR.x nodistro main" | tee /etc/apt/sources.list.d/nodesource.list
RUN apt-get update
RUN apt-get install nodejs -y

# Install gulp
RUN npm install --global gulp-cli

# Copy necessary files and restore as distinct layer
COPY . .
WORKDIR "/app/src/WebApp"
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN npm install
RUN gulp default
RUN dotnet publish "./WebApp.csproj" -c Release -o /app/publish

# Build runtime image
FROM adamoctoclose/pitstop-dotnet-aspnet-base:1.0
WORKDIR /app
COPY --from=build-env /app/publish .

# Expose ports
EXPOSE 7005/tcp
ENV ASPNETCORE_URLS http://*:7005
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:7005/hc || exit 1

# Start
ENTRYPOINT ["dotnet", "Pitstop.WebApp.dll"]
