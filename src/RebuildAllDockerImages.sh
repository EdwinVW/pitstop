#!/bin/bash
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
./RebuildDockerImage.sh
cd ..

echo ""
echo "===================================="
echo "====== CustomerManagement API ======"
echo "===================================="
cd CustomerManagementAPI
./RebuildDockerImage.sh
cd ..

echo ""
echo "==================================="
echo "====== VehicleManagement API ======"
echo "==================================="
cd VehicleManagementAPI
./RebuildDockerImage.sh
cd ..

echo ""
echo "===================================="
echo "====== WorkshopManagement API ======"
echo "===================================="
cd WorkshopManagementAPI
./RebuildDockerImage.sh
cd ..

echo ""
echo "============================================="
echo "====== WorkshopManagement EventHandler ======"
echo "============================================="
cd WorkshopManagementEventHandler
./RebuildDockerImage.sh
cd ..

echo ""
echo "=================================="
echo "====== Notification Service ======"
echo "=================================="
cd NotificationService
./RebuildDockerImage.sh
cd ..

echo ""
echo "============================="
echo "====== Invoice Service ======"
echo "============================="
cd InvoiceService
./RebuildDockerImage.sh
cd ..

echo ""
echo "=========================="
echo "====== Time Service ======"
echo "=========================="
cd TimeService
./RebuildDockerImage.sh
cd ..

echo ""
echo "====================="
echo "====== Web App ======"
echo "====================="
cd WebApp
./RebuildDockerImage.sh
cd ..
