kubectl apply `
    -f ../monitoring-namespace.yaml `
    -f ../prometheus/clusterRole.yaml `
    -f ../prometheus/config-map.yaml `
    -f ../prometheus/prometheus-deployment.yaml `
    -f ../prometheus/prometheus-service.yaml `
    -f ../alertmanager/config-map.yaml `
    -f ../alertmanager/template-config-map.yaml `
    -f ../alertmanager/alertmanager-deployment.yaml `
    -f ../alertmanager/alertmanager-service.yaml