#!/bin/bash
kubectl apply \
    -f ../pitstop-namespace.yaml \
    -f ../rabbitmq.yaml \
    -f ../logserver.yaml \
    -f ../sqlserver.yaml \
    -f ../mailserver.yaml