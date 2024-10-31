# Define the RabbitMQ connection details
$username = "rabbitmquser"
$password = "DEBmbwkSrzy9D1T9cJfa"
$url = "http://<node-ip>:30001/api/exchanges/%2F/Pitstop/publish"

# Define the message body template
$bodyTemplate = @{
    properties = @{}
    routing_key = "Notifications"
    payload = "Test message from PowerShell"
    payload_encoding = "string"
}

# Convert the body template to JSON
$bodyJson = $bodyTemplate | ConvertTo-Json

# Send 1,000 messages
for ($i = 1; $i -le 1000; $i++) {
    # Update the payload with the message number
    $bodyTemplate.payload = "Test message $i from PowerShell"
    $bodyJson = $bodyTemplate | ConvertTo-Json

    # Send the POST request to RabbitMQ API
    Invoke-RestMethod -Uri $url -Method Post -Body $bodyJson -ContentType "application/json" -Credential (New-Object System.Management.Automation.PSCredential($username, (ConvertTo-SecureString $password -AsPlainText -Force)))

    # Print progress
    Write-Output "Sent message $i"
}