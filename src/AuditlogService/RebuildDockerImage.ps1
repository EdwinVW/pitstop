# copy nuget folder contents and config file to local nuget folder 
..\CopyNuGetFiles.ps1

# build image
docker build --force-rm -t pitstop/auditlogservice:latest .