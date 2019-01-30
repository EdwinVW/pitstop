#!/bin/bash
kubectl create \
    -f ./rabbitmq-deployment.yaml \
    -f ./logserver-deployment.yaml \
    -f ./sqlserver-deployment.yaml \
    -f ./mailserver-deployment.yaml

kubectl create \
    -f ./rabbitmq-service.yaml \
    -f ./logserver-service.yaml \
    -f ./sqlserver-service.yaml \
    -f ./mailserver-service.yaml