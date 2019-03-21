#!/bin/bash

# If started without argument, the solution is started without service-mesh. 
# If started with argument --mesh, the solution is started with the Istio service-mesh.

MESHPOSTFIX=''

if [ "$1" = "--mesh" ]
 then
    MESHPOSTFIX='-istio'
fi

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
    -f ./customermanagementapi$MESHPOSTFIX.yaml \
    -f ./vehiclemanagementapi$MESHPOSTFIX.yaml \
    -f ./workshopmanagementapi$MESHPOSTFIX.yaml \
    -f ./webapp$MESHPOSTFIX.yaml