# Run all containers and publish all APIs available via localhost. 
# Updated something? Then run the RebuildAllDockerImages script to run changes.

docker-compose -f .\docker-compose.yml -f .\docker-compose.local.yml up