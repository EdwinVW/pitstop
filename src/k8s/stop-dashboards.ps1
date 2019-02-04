$K8SDashboardUp = get-job K8SDashboard -ErrorAction SilentlyContinue
if ($K8SDashboardUp) {
    stop-job K8SDashboard
    remove-job K8SDashboard
    echo 'K8S dashboard stopped'
} else {
    echo 'K8S dashboard is not running'
}

$IstioGrafanaDashboardUp = get-job IstioGrafanaDashboard -ErrorAction SilentlyContinue
if ($IstioGrafanaDashboardUp) {
    stop-job IstioGrafanaDashboard
    remove-job IstioGrafanaDashboard
    echo 'Istio Grafana dashboard stopped'
} else {
    echo 'Istio Grafana dashboard is not running'
}

$IstioKialiDashboardUp = get-job IstioKialiDashboard -ErrorAction SilentlyContinue
if ($IstioKialiDashboardUp) {
    stop-job IstioKialiDashboard
    remove-job IstioKialiDashboard
    echo 'Istio Kiali dashboard stopped'
} else {
    echo 'Istio Kiali dashboard is not running'
}