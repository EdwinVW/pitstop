$K8SDashboardUp = get-job K8SDB -ErrorAction SilentlyContinue
if ($K8SDashboardUp) {
    stop-job K8SDB
    remove-job K8SDB
    echo 'K8S dashboard stopped'
} else {
    echo 'K8S dashboard is not running'
}

$K8SDashboardUp = get-job IstioJaegerDB -ErrorAction SilentlyContinue
if ($K8SDashboardUp) {
    stop-job IstioJaegerDB
    remove-job IstioJaegerDB
    echo 'Istio Jaeger dashboard stopped'
} else {
    echo 'Istio Jaeger dashboard is not running'
}

$IstioGrafanaDashboardUp = get-job IstioGrafanaDB -ErrorAction SilentlyContinue
if ($IstioGrafanaDashboardUp) {
    stop-job IstioGrafanaDB
    remove-job IstioGrafanaDB
    echo 'Istio Grafana dashboard stopped'
} else {
    echo 'Istio Grafana dashboard is not running'
}

$IstioKialiDashboardUp = get-job IstioKialiDB -ErrorAction SilentlyContinue
if ($IstioKialiDashboardUp) {
    stop-job IstioKialiDB
    remove-job IstioKialiDB
    echo 'Istio Kiali dashboard stopped'
} else {
    echo 'Istio Kiali dashboard is not running'
}