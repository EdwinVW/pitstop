#!/bin/bash

mesh=$1

kubectl apply \
    -f ./pitstop-namespace.yaml \
    -f ./rabbitmq.yaml \
    -f ./logserver.yaml \
    -f ./sqlserver.yaml \
    -f ./mailserver.yaml \
    -f ./invoiceservice.yaml \
    -f ./timeservice.yaml \
    -f ./notificationservice.yaml \
    -f ./workshopmanagementeventhandler.yaml \
    -f ./auditlogservice.yaml \
    -f ./customermanagementapi$mesh.yaml \
    -f ./vehiclemanagementapi$mesh.yaml \
    -f ./workshopmanagementapi$mesh.yaml \
    -f ./webapp$mesh.yaml