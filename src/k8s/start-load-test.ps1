param (
    [Parameter(Mandatory=$true)][string]$jobName,
    [Parameter(Mandatory=$true)][string]$uri
)

start-job -name $jobName -arg $uri -scriptblock {
    param($uri)
    for (;;) {
        Invoke-Webrequest $uri | Out-Null
        Start-Sleep -Milliseconds 50
    } 
} 