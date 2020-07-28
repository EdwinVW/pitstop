# If started without argument, the solution is started without service-mesh. 
# If started with argument -istio, the solution is started with the Istio service-mesh.
# If started with argument -linkerd, the solution is started with the Linkerd service-mesh.

param (
    [switch]$istio = $false,
    [switch]$linkerd = $false
)

if ($istio -and $linkerd) {
    echo "Error: You can specify only 1 mesh implementation."
    return
}

# create namespace
kubectl apply -f ./pitstop-namespace.yaml

$meshPostfix = ''
if ($istio) {
    $meshPostfix = '-istio'

    # configure istio side-car injection
    & "./disable-default-istio-injection.ps1"
    kubectl label --overwrite namespace pitstop istio-injection=enabled
}

if ($linkerd) {
    $meshPostfix = '-linkerd'
}

kubectl apply `
    -f ./rabbitmq.yaml `
    -f ./logserver.yaml `
    -f ./sqlserver.yaml `
    -f ./mailserver.yaml `
    -f ./invoiceservice.yaml `
    -f ./timeservice.yaml `
    -f ./notificationservice.yaml `
    -f ./workshopmanagementeventhandler.yaml `
    -f ./auditlogservice.yaml `
    -f ./customermanagementapi-v1$meshPostfix.yaml `
    -f ./customermanagementapi-svc.yaml `
    -f ./vehiclemanagementapi$meshPostfix.yaml `
    -f ./workshopmanagementapi$meshPostfix.yaml `
    -f ./webapp$meshPostfix.yaml