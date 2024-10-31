# Check if KEDA is installed
$kedaExists = & kubectl get ns keda 2>$null

if (-not $kedaExists) {
    Write-Output "KEDA is not installed. Installing KEDA using Helm."
    & helm repo add kedacore https://kedacore.github.io/charts
    & helm repo update
    & helm install keda kedacore/keda --namespace keda --create-namespace --set webhooks.enabled=false
} else {
    Write-Output "KEDA is already installed."
}

Write-Output "Applying configuration..."
& kubectl apply `
    -f rabbitmq-trigger-auth.yaml `
    -f notificationservice-scaledobject.yaml `
    -f invoiceservice-scaledobject.yaml `
    -f auditlogservice-scaledobject.yaml `
    -f workshopmanagementeventhandler-scaledobject.yaml
