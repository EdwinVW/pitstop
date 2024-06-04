# If started with argument -nomesh, the solution is started without service-mesh.
# If started with argument -istio, the solution is started with the Istio service-mesh.
# If started with argument -linkerd, the solution is started with the Linkerd service-mesh.

param (
    [switch]$nomesh = $false,
    [switch]$istio = $false,
    [switch]$linkerd = $false
)

if (-not $nomesh -and -not $istio -and -not $linkerd)
{
    echo "Error: You must specify how to start Pitstop:"
    echo "  start-all.ps1 < -nomesh | -istio | -linkerd >."
    return
}

$meshPostfix = ''
if (-not $nomesh)
{
    if ($istio -and $linkerd) {
        echo "Error: You can specify only 1 mesh implementation."
        return
    }

    if ($istio) {
        $meshPostfix = '-istio'
        echo "Starting Pitstop with Istio service mesh."

        # disable global istio side-car injection (only for annotated pods)
        & "../istio/disable-default-istio-injection.ps1"
    }

    if ($linkerd) {
        $meshPostfix = '-linkerd'
        echo "Starting Pitstop with Linkerd service mesh."
    }
}
else
{
    echo "Starting Pitstop without service mesh."
}

kubectl apply `
    -f ../pitstop-namespace$meshPostfix.yaml `
    -f ../rabbitmq.yaml `
    -f ../logserver.yaml `
    -f ../sqlserver$meshPostfix.yaml `
    -f ../mailserver.yaml `
    -f ../invoiceservice.yaml `
    -f ../timeservice.yaml `
    -f ../notificationservice.yaml `
    -f ../workshopmanagementeventhandler.yaml `
    -f ../auditlogservice.yaml `
    -f ../customermanagementapi-v1$meshPostfix.yaml `
    -f ../customermanagementapi-svc.yaml `
    -f ../vehiclemanagementapi$meshPostfix.yaml `
    -f ../workshopmanagementapi$meshPostfix.yaml `
    -f ../webapp$meshPostfix.yaml