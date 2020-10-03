#!/bin/bash
kubectl proxy > /dev/null &
kubectl -n istio-system port-forward svc/grafana 3000 > /dev/null &
kubectl -n istio-system port-forward svc/kiali 20001 > /dev/null &