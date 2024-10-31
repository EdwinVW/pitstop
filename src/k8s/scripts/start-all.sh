#!/bin/bash

# If started with argument --nomesh, the solution is started without service-mesh.
# If started with argument --istio, the solution is started with the Istio service-mesh.
# If started with argument --linkerd, the solution is started with the Linkerd service-mesh.

MESHPOSTFIX=''

if [ "$1" != "--nomesh" ] && [ "$1" != "--istio" ] && [ "$1" != "--linkerd" ]
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

# Define the secret name and credentials
SECRET_NAME="rabbitmq-secret"
USERNAME="rabbitmquser"
PASSWORD="DEBmbwkSrzy9D1T9cJfa"

# Check if the secret exists
if kubectl get secret $SECRET_NAME -n pitstop >/dev/null 2>&1; then
    echo "rabbitmq-secret already exists."
else
    echo "Creating rabbitmq-secret with provided username and password."
    kubectl create secret generic $SECRET_NAME \
        --from-literal=username=$USERNAME \
        --from-literal=password=$PASSWORD -n pitstop
fi

kubectl apply \
    -f ../pitstop-namespace$MESHPOSTFIX.yaml \
    -f ../rabbitmq-trigger-auth.yaml \
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
    -f ../customersupportapi.yaml \
    -f ../webapp$meshPostfix.yaml \
    -f ../workshopmanagementeventhandler-scaledobject.yaml \
    -f ../invoiceservice-scaledobject.yaml \
    -f ../auditlogservice-scaledobject.yaml \
    -f ../notificationservice-scaledobject.yaml 
