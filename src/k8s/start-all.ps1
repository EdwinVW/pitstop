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

$meshPostfix = ''
if ($istio) {
    $meshPostfix = '-istio'
}
if ($linkerd) {
    $meshPostfix = '-linkerd'
}

kubectl apply `
    -f ./pitstop-namespace.yaml `
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