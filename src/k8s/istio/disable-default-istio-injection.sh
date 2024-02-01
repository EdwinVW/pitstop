#!/bin/bash

# In order to use the istio-injection annotation only on specific deployments within
# the pitstop namespace, we need to disable automatic injection and enable it explicitly
# on the namespace (see start-all script).

kubectl get configmap istio-sidecar-injector -o yaml -n istio-system | \
  sed -e '0,/policy: enabled/ s/policy: enabled/policy: disabled/' | \
  kubectl apply -n istio-system -f -

