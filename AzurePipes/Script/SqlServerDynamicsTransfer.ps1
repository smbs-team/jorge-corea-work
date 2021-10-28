[CmdletBinding()]
param (
    $DeploymentEnv,
	$DatabaseServerName,
	$DatabaseName,
	$KeyVaultName,
	$DynamicsUrl,
	[Parameter(Mandatory=$false)]
	$SpecificTable
)

function DeployDataFromDynamics()
{
	param( $connectionString, $dynamicsUrl, $deploymentEnv, $keyVaultName, $specificTable )

	$appIdSecretName = "ptas-$deploymentEnv-app-clientid".ToLower()
	Write-Host "Getting $($appIdSecretName) from KeyVault $($keyVaultName) ..."	
	$appId = $(az keyvault secret show --name "$($appIdSecretName)" --vault-name "$($keyVaultName)" --query '{SecretValue:value}' -o tsv)
	Write-Host "Client Id: $($appId)"

	$appSecretSecretName = "ptas-$deploymentEnv-app-secret".ToLower()
	Write-Host "Getting $($appSecretSecretName) from KeyVault $($keyVaultName)..."
	$appSecret = $(az keyvault secret show --name "$($appSecretSecretName)" --vault-name "$($keyVaultName)" --query '{SecretValue:value}' -o tsv)

	$trasnferToolPath = 'PTASDynamicsTransfer\PTASDynamicsTranfer.exe'

	Write-Host "-> Copying data from Dynamics..."

	$authenticationUrl = "https://login.windows.net/KC1.onmicrosoft.com"

	$manifestPath = $Env:BUILD_SOURCESDIRECTORY + '\AzurePipes\SqlDeploymentScripts\EntityManifest.txt'

	Write-Host "-> Looking for items in $($manifestPath)..."	
	
	if ($specificTable -eq $null)
	{
		$entities = Get-Content -Path $manifestPath 
	} else
	{
		$entities = @($specificTable)
	}
	
	foreach ($entityName in $entities)
	{
		Write-Host "--> Copying data from Dynamics table $entityName ..."

		Write-Host "Acquiring SQL Server bearer token..."
		$token = $(az account get-access-token --resource=https://database.windows.net/ --query 'accessToken' -o tsv)
		Write-Host "SQL Server bearer token acquired."

		$output = [string] (& $trasnferToolPath '-e' $entityName '-s' 2000 '-c' "$($connectionString)" '-d' "$($dynamicsUrl)" '-b' "$($token)" '--registrationid' "$($appId)" '--registrationsecret' "$($appSecret)" '-a' "$($authenticationUrl)" '-u' 1)

		while ($output -like '*Token is expired*')
		{
			$token = $(az account get-access-token --resource=https://database.windows.net/ --query 'accessToken' -o tsv)
			$output = [string] (& $trasnferToolPath '-e' $entityName '-s' 2000 '-c' "$($connectionString)" '-d' "$($dynamicsUrl)" '-b' "$($token)" '--registrationid' "$($appId)" '--registrationsecret' "$($appSecret)" '-a' "$($authenticationUrl)" '-u' 1)
		}

		Write-Host "--> Successfully copied data from dynamics table $entityName."
		Write-Host "--> Tool Output: $($output )"
	}

	Write-Host "-> Successfully copied data from dynamics."
}

$modulePath = $Env:BUILD_SOURCESDIRECTORY + '\AzurePipes\Script\PipeScriptHelperModule.psm1'
Import-Module $modulePath 

Write-Host "Downloading pipe tools..."
Download-BlobItems "https://ptasdevpipetools.blob.core.windows.net/pipetools?sv=2019-10-10&ss=b&srt=sco&sp=rl&se=2100-05-02T05:09:19Z&st=2020-05-01T21:09:19Z&spr=https&sig=P3GqPxayXEfSPecIEGqRjlHN1FibGYZQL3ISTOGBqcQ%3D"
Write-Host "Pipe tools downloaded."

$ConnectionString = "Server=tcp:$($DatabaseServerName),1433;Initial Catalog=$($DatabaseName);Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=900;"

DeployDataFromDynamics -connectionString $ConnectionString -dynamicsUrl $DynamicsUrl -deploymentEnv $DeploymentEnv -keyVaultName $KeyVaultName -specificTable $SpecificTable  