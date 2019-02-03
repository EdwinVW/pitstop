param (
    [Parameter(Mandatory=$true)][string]$jobName
)

$uris = Get-Content -Path loadtest-uris.txt

echo 'Starting load-test for:'
foreach ($uri in $uris) {
    echo $uri
}

start-job -name $jobName -argumentlist (,$uris) -scriptblock {
    param($uris)
    for(;;) {
        foreach ($uri in $uris) {
            Invoke-Webrequest $uri -ErrorAction SilentlyContinue | Out-Null
            Start-Sleep -Milliseconds 25
        } 
    }
}