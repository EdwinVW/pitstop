# Run all containers in the background, and directly afterwards tail the logs files (like"
# running in the foreground. When pressing crtl-c, youre containers will continue to run."
# Updated something? Then run the RebuildAllDockerImages script to run changes."

docker-compose up -d --scale customermanagementapi=2 --scale vehiclemanagementapi=2 --scale workshopmanagementapi=3; docker-compose logs -f
