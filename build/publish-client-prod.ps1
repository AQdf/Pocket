Set-Location (Split-Path $MyInvocation.InvocationName)
Set-Location -Path ..
$basePath = (pwd).path

Set-Location -Path ./client/sho-pocket-app
Write-Host "1. Building 'prod' Angular application..."
ng build --prod

$source = $basePath + "/client/sho-pocket-app/dist/sho-pocket-app"
$destination = $basePath + "/server/Sho.Pocket.Web/wwwroot"

Write-Host "2. Cleaning up $destination"
Remove-Item –Path $destination -Recurse

Write-Host "3. Copying $source to $destination"
Copy-Item -Path $source -Destination $destination –Recurse

Set-Location (Split-Path $MyInvocation.InvocationName)
Read-Host "Press Enter to exit..."