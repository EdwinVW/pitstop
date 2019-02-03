param (
    [Parameter(Mandatory=$true)][string]$jobName
)

$uris = Get-Content loadtest-uris.json | ConvertFrom-Json

echo 'Starting load-test for:'
foreach ($uri in $uris) {
    echo $uri
}

start-job -name $jobName -arg $uris -scriptblock {
    param($uris)
    for(;;) {
        foreach ($uri in $uris) {
            echo $uri
            Invoke-Webrequest $uri | Out-Null
            Start-Sleep -Milliseconds 25
        } 
    }
} 