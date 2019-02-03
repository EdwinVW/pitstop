start-job -name K8SDashboard -scriptblock {
    kubectl proxy
}

start-job -name IstioGrafanaDashboard -scriptblock {
    kubectl -n istio-system port-forward svc/grafana 3000
}
