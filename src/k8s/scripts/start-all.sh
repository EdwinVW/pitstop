#!/bin/bash

# If started with argument --nomesh, the solution is started without service-mesh.
# If started with argument --istio, the solution is started with the Istio service-mesh.
# If started with argument --linkerd, the solution is started with the Linkerd service-mesh.

MESHPOSTFIX=''

if [ "$1" != "--nomesh" and  "$1" != "--istio" and "$1" != "--linkerd" ]
then
    echo "Error: You must specify how to start Pitstop:"
    echo "  start-all.ps1 < --nomesh | --istio | --linkerd >."
    exit 1
fi

if [ "$1" = "--nomesh" ]
then
    echo "Starting Pitstop without service mesh."
fi

if [ "$1" = "--istio" ]
then
    MESHPOSTFIX='-istio'

    echo "Starting Pitstop with Istio service mesh."

    # disable global istio side-car injection (only for annotated pods)
    ../istio/disable-default-istio-injection.sh
fi

if [ "$1" = "--linkerd" ]
then
    MESHPOSTFIX='-linkerd'

    echo "Starting Pitstop with Linkerd service mesh."
fi

kubectl apply \
    -f ../pitstop-namespace$MESHPOSTFIX.yaml \
    -f ../rabbitmq.yaml \
    -f ../logserver.yaml \
    -f ../sqlserver$MESHPOSTFIX.yaml \
    -f ../mailserver.yaml \
    -f ../invoiceservice.yaml \
    -f ../timeservice.yaml \
    -f ../notificationservice.yaml \
    -f ../workshopmanagementeventhandler.yaml \
    -f ../auditlogservice.yaml \
    -f ../customermanagementapi-v1$MESHPOSTFIX.yaml \
    -f ../customermanagementapi-svc.yaml \
    -f ../vehiclemanagementapi$MESHPOSTFIX.yaml \
    -f ../workshopmanagementapi$MESHPOSTFIX.yaml \
    -f ../webapp$MESHPOSTFIX.yaml
