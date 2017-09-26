echo "====================="
echo "====== Volumes ======"
echo "====================="
docker volume create --name=sqlserverdata
docker volume create --name=rabbitmqdata

echo ""
echo "=============================="
echo "====== Auditlog Service ======"
echo "=============================="
cd AuditLogService
./RebuildDockerImage.ps1
cd ..

echo ""
echo "===================================="
echo "====== CustomerManagement API ======"
echo "===================================="
cd CustomerManagementAPI
./RebuildDockerImage.ps1
cd ..

echo ""
echo "==================================="
echo "====== VehicleManagement API ======"
echo "==================================="
cd VehicleManagementAPI
./RebuildDockerImage.ps1
cd ..

echo ""
echo "===================================="
echo "====== WorkshopManagement API ======"
echo "===================================="
cd WorkshopManagementAPI
./RebuildDockerImage.ps1
cd ..

echo ""
echo "============================================="
echo "====== WorkshopManagement EventHandler ======"
echo "============================================="
cd WorkshopManagementEventHandler
./RebuildDockerImage.ps1
cd ..

echo ""
echo "=================================="
echo "====== Notification Service ======"
echo "=================================="
cd NotificationService
./RebuildDockerImage.ps1
cd ..

echo ""
echo "============================="
echo "====== Invoice Service ======"
echo "============================="
cd InvoiceService
./RebuildDockerImage.ps1
cd ..

echo ""
echo "=========================="
echo "====== Time Service ======"
echo "=========================="
cd TimeService
./RebuildDockerImage.ps1
cd ..

echo ""
echo "====================="
echo "====== Web App ======"
echo "====================="
cd WebApp
./RebuildDockerImage.ps1
cd ..
