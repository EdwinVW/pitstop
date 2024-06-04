param (
    [Parameter(Mandatory=$true)][string]$jobName
)

$uris = Get-Content -Path loadtest-uris.txt

echo 'Starting load-test for:'
echo $uris

foreach ($uri in $uris) {
    start-job -name $jobName -argumentlist $uri -scriptblock {
        param($uri)
        
        for(;;) {
            # execute request
            echo "Execute request: $uri"
            $progressPreference = 'SilentlyContinue' 
            Invoke-Webrequest -Headers @{"Cache-Control"="no-cache"} $uri -ErrorAction SilentlyContinue | Out-Null
            $progressPreference = 'Continue' 

            # random delay
            $delay = Get-Random -Minimum 100 -Maximum 2000
            echo "Wait $delay ms. ..."
            Start-Sleep -Milliseconds $delay
        } 
    }
}