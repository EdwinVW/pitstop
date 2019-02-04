[string]$user="kialiuser"
[string]$pass="kialipassword"

$userb64=[Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes($user))
$passb64=[Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes($pass))

Write-Host $userb64
Write-Host $passb64