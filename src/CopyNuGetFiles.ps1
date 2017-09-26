# copy private nuget folder contents to local nuget folder for docker build
New-Item -ItemType Directory -force -Path nuget
Copy-Item "d:\NuGet\PitStop\*.nupkg" ./nuget/ 
