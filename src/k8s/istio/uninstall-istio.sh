#!/bin/bash

ISTIO_VERSION=$(<istio-version.txt)

echo "Uninstalling Istio $ISTIO_VERSION"

kubectl delete -f https://raw.githubusercontent.com/istio/istio/release-$ISTIO_VERSION/samples/addons/prometheus.yaml
kubectl delete -f https://raw.githubusercontent.com/istio/istio/release-$ISTIO_VERSION/samples/addons/grafana.yaml
kubectl delete -f https://raw.githubusercontent.com/istio/istio/release-$ISTIO_VERSION/samples/addons/kiali.yaml
kubectl delete -f https://raw.githubusercontent.com/istio/istio/release-$ISTIO_VERSION/samples/addons/jaeger.yaml

istioctl uninstall --purge -y

kubectl delete namespace istio-system