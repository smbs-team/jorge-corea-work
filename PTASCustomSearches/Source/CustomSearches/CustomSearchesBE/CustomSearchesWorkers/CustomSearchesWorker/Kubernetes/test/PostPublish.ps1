$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
Copy-Item "$($scriptPath)\..\..\app.settings.test.json" -Destination "$($scriptPath)\..\..\bin\Release\netcoreapp3.1\publish\app.settings.json"
Remove-Item "$($scriptPath)\..\..\bin\Release\netcoreapp3.1\publish\local.settings.json"
Write-Host "After running this script zip this file in the  [publish] folder with the [CustomSearchesWorker.zip] name."
Write-Host "Copy the [CustomSearchesWorker.zip] file in the [Docker] folder and run the [Deploytest.ps1] script."