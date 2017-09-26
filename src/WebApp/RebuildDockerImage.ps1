# minification and bundling
npm install
gulp default

# copy nuget folder contents and config file to local nuget folder 
..\CopyNuGetFiles.ps1

# build docker image
docker build --force-rm -t pitstop/webapp:latest .