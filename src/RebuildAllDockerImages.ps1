echo "====================="
echo "====== Volumes ======"
echo "====================="
docker volume create --name=sqlserverdata
docker volume create --name=rabbitmqdata

# Rebuild all the services that have changes
# If you want to (re)build only a specific service, go to the src folder and execute `docker-compose build <servicename-lowercase>`
docker-compose build --force-rm
