#!/bin/bash
kubectl proxy > /dev/null &
kubectl -n istio-system port-forward svc/grafana 3000 > /dev/null &
