[CmdletBinding()]
param (
	$DatabaseServerName,
	$DatabaseName
)

function ExecuteScriptsInPath()
{
	param( $path, $connectionString, $bearerToken )

	$pipesToolPath = 'AzurePipesTools\AzurePipesTools.exe'

	Write-Host "-> Executing scripts in path: $path..."

	$output = [string] (& $pipesToolPath '-deployscript' $path $connectionString $bearerToken)

	Write-Host "-> Executed scripts in path: $path"
	Write-Host "--> Tool Output: $($output )"
}

$modulePath = $Env:BUILD_SOURCESDIRECTORY + '\AzurePipes\Script\PipeScriptHelperModule.psm1'
Import-Module $modulePath 

Write-Host "Downloading pipe tools..."
Download-BlobItems "https://ptasdevpipetools.blob.core.windows.net/pipetools?sv=2019-10-10&ss=b&srt=sco&sp=rl&se=2100-05-02T05:09:19Z&st=2020-05-01T21:09:19Z&spr=https&sig=P3GqPxayXEfSPecIEGqRjlHN1FibGYZQL3ISTOGBqcQ%3D"
Write-Host "Pipe tools downloaded."

Write-Host "Acquiring SQL Server bearer token..."
$token = $(az account get-access-token --resource=https://database.windows.net/ --query 'accessToken' -o tsv)
Write-Host "SQL Server bearer token acquired."

$ConnectionString = "Server=tcp:$($DatabaseServerName),1433;Initial Catalog=$($DatabaseName);Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

$schemaScriptPath = $Env:BUILD_SOURCESDIRECTORY + '\AzurePipes\SqlDeploymentScripts\Schema'
ExecuteScriptsInPath -path $schemaScriptPath -connectionString $ConnectionString -bearerToken $token

$schemaScriptPath = $Env:BUILD_SOURCESDIRECTORY + '\AzurePipes\SqlDeploymentScripts\Patches'
ExecuteScriptsInPath -path $schemaScriptPath -connectionString $ConnectionString -bearerToken $token  