start-job -name K8SDB -scriptblock {
    kubectl proxy
}

start-job -name IstioGrafanaDB -scriptblock {
    kubectl -n istio-system port-forward svc/grafana 3000
}

start-job -name IstioKialiDB -scriptblock {
    kubectl -n istio-system port-forward svc/kiali 20001
}

start-job -name IstioJaegerDB -scriptblock {
    $podName = kubectl get pods -n istio-system --selector=app=jaeger -o=jsonpath='{.items..metadata.name}'
    kubectl -n istio-system port-forward pod/$podName 16686
}