#!/bin/bash

# Check if KEDA is installed by verifying the existence of the 'keda' namespace
if ! kubectl get ns keda &> /dev/null; then
    echo "KEDA is not installed. Installing KEDA using Helm."
    helm repo add kedacore https://kedacore.github.io/charts
    helm repo update
    helm install keda kedacore/keda --namespace keda --create-namespace
else
    echo "KEDA is already installed."
fi

# Apply configuration for scaled objects
echo "Applying configuration..."
kubectl apply -f rabbitmq-trigger-auth.yaml \
    -f notificationservice-scaledobject.yaml \
    -f invoiceservice-scaledobject.yaml \
    -f auditlogservice-scaledobject.yaml \
    -f workshopmanagementeventhandler-scaledobject.yaml
