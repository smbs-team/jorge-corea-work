[CmdletBinding()]
param (
    $DeploymentEnv,
	$DeploymentResourceGroup,
	$DeploymentSubscriptionId
)

$subscriptionId = $DeploymentSubscriptionId
$deployEnv = $DeploymentEnv
$resGroupName = $DeploymentResourceGroupName

function Get-AzCachedAccessToken()
{
    $ErrorActionPreference = 'Stop'
  
    if(-not (Get-Module Az.Accounts)) {
        Import-Module Az.Accounts
    }
    $azProfile = [Microsoft.Azure.Commands.Common.Authentication.Abstractions.AzureRmProfileProvider]::Instance.Profile
    if(-not $azProfile.Accounts.Count) {
        Write-Error "Ensure you have logged in before calling this function."    
    }
  
    $currentAzureContext = Get-AzContext
    $profileClient = New-Object Microsoft.Azure.Commands.ResourceManager.Common.RMProfileClient($azProfile)
    Write-Debug ("Getting access token for tenant" + $currentAzureContext.Tenant.TenantId)
    $token = $profileClient.AcquireAccessToken($currentAzureContext.Tenant.TenantId)
    $token.AccessToken
}

function Get-AzBearerToken()
{
    $ErrorActionPreference = 'Stop'
    ('Bearer {0}' -f (Get-AzCachedAccessToken))
}


# Update for APIM Keys
$servicePrincipalName = "PTAS-$($deployEnv)-MapTileServices"

write-output "-> Updating APIM Keys for $($servicePrincipalName)..."

write-output "--> Getting APIM Key for service..."
$accessToken = Get-AzBearerToken
$listFunctionKeysUrl = "https://management.azure.com/subscriptions/$($subscriptionId)/resourceGroups/$($resGroupName)/providers/Microsoft.Web/sites/$($servicePrincipalName)/host/default/listkeys?api-version=2015-08-01"
$functionKeys = Invoke-RestMethod -Method Post -Uri $listFunctionKeysUrl -Headers @{ Authorization="$accessToken"; "Content-Type"="application/json" }
$apiManagementKey = $functionKeys.functionKeys.'apim-PTAS-$($deployEnv)-ApiManagement'	

write-output "--> Setting APIM Key in APIM..."
$context = New-AzApiManagementContext -ResourceGroupName $resGroupName) -ServiceName PTAS-SBOX-ApiManagement
Set-AzApiManagementProperty -Context $context -PropertyId "$($servicePrincipalName.ToLower())-key" -Value $apiManagementKey -Secret $true