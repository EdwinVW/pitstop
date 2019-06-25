#!/bin/bash

# If started without argument, the solution is started without service-mesh. 
# If started with argument --istio, the solution is started with the Istio service-mesh.
# If started with argument --linkerd, the solution is started with the Linkerd service-mesh.

MESHPOSTFIX=''

if [ "$1" = "--istio" ]
then
    MESHPOSTFIX='-istio'
fi

if [ "$1" = "--linkerd" ]
then
    MESHPOSTFIX='-linkerd'
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
    -f ./customermanagementapi-v1$MESHPOSTFIX.yaml \
    -f ./customermanagementapi-svc.yaml \
    -f ./vehiclemanagementapi$MESHPOSTFIX.yaml \
    -f ./workshopmanagementapi$MESHPOSTFIX.yaml \
    -f ./webapp$MESHPOSTFIX.yaml