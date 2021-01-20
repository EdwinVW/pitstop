#!/bin/bash

# K8S dashboard
kubectl proxy > /dev/null &

#grafana
kubectl -n istio-system port-forward svc/grafana 3000 > /dev/null &

#kiali
kubectl -n istio-system port-forward svc/kiali 20001 > /dev/null &

#jaeger
podName=$(kubectl get pods -n istio-system --selector=app=jaeger -o=jsonpath='{.items..metadata.name}')
kubectl -n istio-system port-forward pod/$podName 16686 > /dev/null &