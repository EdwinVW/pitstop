param (
    [Parameter(Mandatory=$true)][int]$jobId
)

stop-job $jobId
remove-job $jobId