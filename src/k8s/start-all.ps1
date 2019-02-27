# If started without argument, the solution is started without service-mesh. 
# If started with argument -mesh, the solution is started with the Istio service-mesh.

param (
    [switch]$mesh = $false
)

$meshPostfix = ''
if ($mesh) {
    $meshPostfix = '-istio'
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