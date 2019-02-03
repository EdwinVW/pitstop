param (
    [Parameter(Mandatory=$true)][string]$jobName
)

stop-job $jobName
remove-job $jobName