start-job -name K8SDB -scriptblock {
    kubectl proxy
}

start-job -name IstioJaegerDB -scriptblock {
    kubectl -n istio-system port-forward svc/jaeger-query 16686
}

start-job -name IstioGrafanaDB -scriptblock {
    kubectl -n istio-system port-forward svc/grafana 3000
}

start-job -name IstioKialiDB -scriptblock {
    kubectl -n istio-system port-forward svc/kiali 20001:20001
}