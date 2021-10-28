[CmdletBinding()]
param (
    $DeploymentEnv,
	$DeploymentSqlEnv,
	$DeploymentResourceGroup,
	$DeployKeyVaultName,
	$DeployPipeAppRegistration
)

$deployEnv = $DeploymentEnv
$deploySqlEnv = $DeploymentSqlEnv
$resGroupName = $DeploymentResourceGroup
$vaultName = $DeployKeyVaultName
$appRegistrationName = $DeployPipeAppRegistration

function AssignRole()
{
	param( $principalName, $roleName, $resourceName, $resourceType, $resourceGroupName, $continueOnError)

	try
	{
		Write-Host "-> Reading $principalName Service Principal"	

		$servicePrincipal = Get-AzADServicePrincipal -SearchString $principalName | Where ServicePrincipalNames -match "https://identity.azure.net/"

		Write-Host "-> Adding $roleName role for $resourceName ..."

		$currentRole = Get-AzRoleAssignment -ObjectId $servicePrincipal.Id -RoleDefinitionName $roleName -ResourceName $resourceName -ResourceType $resourceType -ResourceGroupName $resourceGroupName

		if ($currentRole -eq $null)
		{
		   New-AzRoleAssignment -ObjectId $servicePrincipal.Id -RoleDefinitionName $roleName -ResourceName $resourceName -ResourceType $resourceType -ResourceGroupName $resourceGroupName
		}

		Write-Host "-> Role assigned."
	}
	catch
	{
		Write-Host "--> Error caught during role assignment.  StatusCode: " $_.Exception.Response.StatusCode.value__ "StatusDescription: " $_.Exception.Response.StatusDescription

		if (-not $continueOnError)
		{					
			throw "-> Error while assigning role. Execution stopped."
		}

		Write-Host "-> Failed to assign role to the $roleName to $principalName.  Access should be manually granted or the rest of the pipe will fail."
	}
}

function AddVaultAccess()
{	
	param( $principalName, $keyvaultName, $continueOnError)

	try
	{

		Write-Host "-> Reading $principalName Service Principal"
	
		$servicePrincipal = Get-AzADServicePrincipal -SearchString $principalName | Where ServicePrincipalNames -match "https://identity.azure.net/"

		Write-Host "-> Adding KeyVault access policy for $principalName"
	
		Set-AzKeyVaultAccessPolicy -VaultName  $keyvaultName -ObjectId $servicePrincipal.Id -PermissionsToSecrets Get

		Write-Host "-> Vault access granted."
	}
	catch
	{
		Write-Host "--> Error caught while trying to assign vault access.  StatusCode: " $_.Exception.Response.StatusCode.value__ "StatusDescription: " $_.Exception.Response.StatusDescription

		if (-not $continueOnError)
		{					
			throw "-> Error while trying to assign vault access. Execution stopped."
		}

		Write-Host "-> Failed to assign vault access to $principalName.  Access should be manually granted or the rest of the pipe will fail."
	}
}

function AddVaultAccessAppRegistration()
{	
	param( $appRegistrationDisplayName, $keyvaultName, $continueOnError)

	try
	{
		Write-Host "-> Reading $appRegistrationDisplayName App Registration"
	
		$appRegistration = Get-AzADApplication -DisplayName $appRegistrationDisplayName

		Write-Host "-> Adding KeyVault access policy for $appRegistrationDisplayName"
	
		Set-AzKeyVaultAccessPolicy -VaultName  $keyvaultName -ObjectId $appRegistration.ObjectId -PermissionsToSecrets Get

		Write-Host "-> Vault access granted."
	}
	catch
	{
		Write-Host "--> Error caught while trying to assign vault access.  StatusCode: " $_.Exception.Response.StatusCode.value__ "StatusDescription: " $_.Exception.Response.StatusDescription

		if (-not $continueOnError)
		{					
			throw "-> Error while trying to assign vault access. Execution stopped."
		}

		Write-Host "-> Failed to assign vault access to $appRegistrationDisplayName.  Access should be manually granted or the rest of the pipe will fail."
	}
}

function AddSqlServerReadAccess()
{
	param( $principalName, $env, $continueOnError)

	try
	{
		Write-Host "-> Reading $principalName Service Principal"	

		$servicePrincipal = Get-AzADServicePrincipal -SearchString $principalName | Where ServicePrincipalNames -match "https://identity.azure.net/"

		Write-Host "-> Adding SQL Server datareader role for  $principalName Service Principal..."

		$readOnlyAdGroup = Get-AzADGroup -SearchString "PTAS$($env.ToLower())AZ-DBreaders"

		if (Get-AzADGroupMember -GroupObjectId $readOnlyAdGroup.Id | Where-Object {$_.Id -eq $servicePrincipal.Id})
		{        
			Write-Host "-> User already has access to group: $($readOnlyAdGroup.Id)"
		} else
		{
			Add-AzADGroupMember -MemberObjectId  $servicePrincipal.Id -TargetGroupObjectId $readOnlyAdGroup.Id

			Write-Host "-> SQL Server read access granted."
		}	
	}
	catch
	{
		Write-Host "--> Error caught during SQL Server read access assignment.  StatusCode: " $_.Exception.Response.StatusCode.value__ "StatusDescription: " $_.Exception.Response.StatusDescription

		if (-not $continueOnError)
		{					
			throw "-> Error caught during SQL Server read access assignment. Execution stopped."
		}

		Write-Host "-> Failed to assign SQL Server read access to $principalName.  Access should be manually granted or the rest of the pipe will fail."
	}
}

function LeaseBlobContainer()
{   
	param( $accountName, $containerName, $storageKey, $continueOnError)

	Write-Host "-> Creating container blob lease for $accountName-$containerName..."		

	$url = "https://$accountName.blob.core.windows.net/$($containerName)?restype=container&comp=lease"

	$leaseId = [guid]::newguid().ToString()

	Write-Host "--> Invoking storage REST Url: $url"	

	$GMTTime = (Get-Date).ToUniversalTime().toString('R')
	$canonicalizedHeaders = "x-ms-date:$GMTTime`nx-ms-lease-action:acquire`nx-ms-lease-duration:-1`nx-ms-proposed-lease-id:$leaseId`nx-ms-version:2012-02-12"

	$stringToSign = "PUT`n`n`n`n$canonicalizedHeaders`n/$accountName/$($containerName)?comp=lease"

	$hmacsha = New-Object System.Security.Cryptography.HMACSHA256
    $hmacsha.key = [Convert]::FromBase64String($storageKey)
    $signature = $hmacsha.ComputeHash([Text.Encoding]::UTF8.GetBytes($stringToSign))
    $signature = [Convert]::ToBase64String($signature)
	$authHeaderContent = "SharedKeyLite $($accountName):$signature"	

	try
	{
		$pipeline = Invoke-RestMethod -Method 'Put' -Uri $url -Headers @{		
			'x-ms-date' = $GMTTime;
			'x-ms-lease-action' = 'acquire';
			'x-ms-lease-duration' = '-1';
			'x-ms-proposed-lease-id' = $leaseId;
			'x-ms-version' = '2012-02-12';		
			'Authorization' = $authHeaderContent
		}

		Write-Host "-> Finished creating lease."	
	} catch
	{
		Write-Host "--> Error caught during blob storage lease creation.  StatusCode: " $_.Exception.Response.StatusCode.value__ "StatusDescription: " $_.Exception.Response.StatusDescription
		if ($_.Exception.Response.StatusCode.value__ -ne 409)
		{			
			if (-not $continueOnError)
			{					
				throw "-> Error trying to create lease for $accountName-$containerName."
			}

			Write-Host "-> Failed to assign lease for $containerName.  Lease should be manually created."			
		}
	}
}

# --- Calculate whether the script should continue even if permissions can't be granted ---
$isProdEnvironment = ($deployEnv.ToLower() -eq 'prod') -or ($deployEnv.ToLower() -eq 'prdm')

# --- vsts-dev-reg role assignment ---

#$appRegistrationName = "vsts-$($deployEnv)-reg"
AddVaultAccessAppRegistration -appRegistrationDisplayName $appRegistrationName -keyvaultName $vaultName -continueOnError $isProdEnvironment

# --- PTAS Custom Searches role assignment ---
# This needs to be in a try catch while PTAS Custom Searches is not on all the environments.
try
{
	if (-not $isProdEnvironment)
	{
		$servicePrincipalName = "ptas-sbox-customsearchesfunctions"
		AddSqlServerReadAccess -principalName $servicePrincipalName -env $deploySqlEnv -continueOnError $isProdEnvironment
	}
} catch
{
    Write-Host "-> Couldn't assign roles to PTAS Custom Searches SBOX."
}

# --- PTAS Custom Searches role assignment ---
# This needs to be in a try catch while PTAS Custom Searches is not on all the environments.
try
{
	$servicePrincipalName = "PTAS-$($deployEnv)-CustomSearchesWebApp"
	AddVaultAccess -principalName $servicePrincipalName -keyvaultName $vaultName -continueOnError $isProdEnvironment
	AddSqlServerReadAccess -principalName $servicePrincipalName -env $deploySqlEnv -continueOnError $isProdEnvironment
} catch
{
    Write-Host "-> Couldn't assign roles to PTAS Custom Searches."
}

# --- PTAS OData Services role assignment ---
# This needs to be in a try catch while PTAS OData Services is not on all the environments.
try
{
	$servicePrincipalName = "PTAS-$($deployEnv)-ODataServices"
	AddVaultAccess -principalName $servicePrincipalName -keyvaultName $vaultName -continueOnError $isProdEnvironment
	AddSqlServerReadAccess -principalName $servicePrincipalName -env $deploySqlEnv -continueOnError $isProdEnvironment
} catch
{
    Write-Host "-> Couldn't assign roles to PTAS OData Services."
}

# --- PTAS SEMA Signalr role assignment ---
# This needs to be in a try catch while PTAS SEMA is not on all the environments.
try
{
	$servicePrincipalName = "PTAS-$($deployEnv)-SemaSignalR"
	AddVaultAccess -principalName $servicePrincipalName -keyvaultName $vaultName -continueOnError $isProdEnvironment
} catch
{
    Write-Host "-> Couldn't assign roles to PTAS SEMA."
}

# --- Magic Link Services role assignment ---
$servicePrincipalName = "PTAS-$($deployEnv)-MagicLinkService"
AddVaultAccess -principalName $servicePrincipalName -keyvaultName $vaultName -continueOnError $isProdEnvironment

# --- Health Services role assignment ---
$servicePrincipalName = "PTAS-$($deployEnv)-HealthServices"
AssignRole -principalName $servicePrincipalName -roleName "Storage Blob Data Contributor" -resourceName "ptas$($deployEnv.ToLower())storagepremium" -resourceType "Microsoft.Storage/storageAccounts" -resourceGroupName $resGroupName -continueOnError $isProdEnvironment
AddVaultAccess -principalName $servicePrincipalName -keyvaultName $vaultName -continueOnError $isProdEnvironment
AddSqlServerReadAccess -principalName $servicePrincipalName -env $deploySqlEnv -continueOnError $isProdEnvironment

# --- Map Tile Services role assignment ---
$servicePrincipalName = "PTAS-$($deployEnv)-MapTileServices"
AssignRole -principalName $servicePrincipalName -roleName "Storage Blob Data Contributor" -resourceName "ptas$($deployEnv.ToLower())storagepremium" -resourceType "Microsoft.Storage/storageAccounts" -resourceGroupName $resGroupName -continueOnError $isProdEnvironment
AddVaultAccess -principalName $servicePrincipalName -keyvaultName $vaultName -continueOnError $isProdEnvironment
AddSqlServerReadAccess -principalName $servicePrincipalName -env $deploySqlEnv -continueOnError $isProdEnvironment
# --- Media Info role assignment ---
$servicePrincipalName = "PTAS-$($deployEnv)-MediaInfo"
AssignRole -principalName $servicePrincipalName -roleName "Storage Blob Data Contributor" -resourceName "ptas$($deployEnv.ToLower())storage" -resourceType "Microsoft.Storage/storageAccounts" -resourceGroupName $resGroupName -continueOnError $isProdEnvironment
AddVaultAccess -principalName $servicePrincipalName -keyvaultName $vaultName -continueOnError $isProdEnvironment
AddSqlServerReadAccess -principalName $servicePrincipalName -env $deploySqlEnv -continueOnError $isProdEnvironment

# --- Data Services role assignment ---
$servicePrincipalName = "PTAS-$($deployEnv)-DataServices"
AssignRole -principalName $servicePrincipalName -roleName "Storage Blob Data Contributor" -resourceName "ptas$($deployEnv.ToLower())storage" -resourceType "Microsoft.Storage/storageAccounts" -resourceGroupName $resGroupName -continueOnError $isProdEnvironment
AddVaultAccess -principalName $servicePrincipalName -keyvaultName $vaultName -continueOnError $isProdEnvironment

# --- Document Storage Services role assignment ---
$servicePrincipalName = "PTAS-$($deployEnv)-DocumentStorageServices"
AssignRole -principalName $servicePrincipalName -roleName "Storage Blob Data Contributor" -resourceName "ptas$($deployEnv.ToLower())storage" -resourceType "Microsoft.Storage/storageAccounts" -resourceGroupName $resGroupName -continueOnError $isProdEnvironment
AssignRole -principalName $servicePrincipalName -roleName "Cognitive Services User" -resourceName "PTAS-$($deployEnv)-ComputerVision" -resourceType "Microsoft.CognitiveServices/accounts" -resourceGroupName $resGroupName -continueOnError $isProdEnvironment
AddVaultAccess -principalName $servicePrincipalName -keyvaultName $vaultName -continueOnError $isProdEnvironment

#--- Lease blob containers ---
$lowcaseEnv = $deployEnv.ToLower()
$blobAccountName = "ptas$($lowcaseEnv)storage"

Write-Host "-> Getting storage key from vault..."	
$storageConnectionStringSecret = Get-AzKeyVaultSecret -VaultName $vaultName -Name 'ptas-storage-connectionstring'
Write-Host "-> Got storage key from vault"	

$hashString = $storageConnectionStringSecret.SecretValueText.Replace(";","`n")
$hash = ConvertFrom-StringData $hashString
$storageKey = $hash.AccountKey

Write-Host "-> Storage key retrieved"	

LeaseBlobContainer -accountName $blobAccountName -containerName 'ilinx' -storageKey $storageKey -continueOnError $isProdEnvironment
LeaseBlobContainer -accountName $blobAccountName -containerName 'media' -storageKey $storageKey -continueOnError $isProdEnvironment
LeaseBlobContainer -accountName $blobAccountName -containerName 'tilesource' -storageKey $storageKey -continueOnError $isProdEnvironment
LeaseBlobContainer -accountName $blobAccountName -containerName 'json-store' -storageKey $storageKey -continueOnError $isProdEnvironment
LeaseBlobContainer -accountName $blobAccountName -containerName 'json-store-processed' -storageKey $storageKey -continueOnError $isProdEnvironment
LeaseBlobContainer -accountName $blobAccountName -containerName 'sketch' -storageKey $storageKey -continueOnError $isProdEnvironment