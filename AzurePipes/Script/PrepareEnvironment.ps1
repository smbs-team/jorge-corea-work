[CmdletBinding()]
param (
    $DeploymentEnv,
	$DeploymentSqlEnv,
	$DeploymentResourceGroup,
	$DeploymentKeyVaultName,
	$DeploymentLocation
)

Install-Module -Name PoshRSJob -Confirm:$False -Force

$deployEnv = $DeploymentEnv
$deploySqlEnv = $DeploymentSqlEnv
$resGroupName = $DeploymentResourceGroup
$vaultName = $DeploymentKeyVaultName
$deployLocation = $DeploymentLocation

function DeleteOldDeployments()
{
	param( $resourceGroupName)

	Write-Host "-> Deleting old deployments..."	

	$deployments = Get-AzResourceGroupDeployment -ResourceGroupName $resourceGroupName | Where-Object Timestamp -lt ((Get-Date).AddDays(-3))

	if ($deployments)
	{
		Write-Host "--> Old deployments found."	

		foreach ($deployment in $deployments) {
			$deploymentName = $deployment.DeploymentName			
			Start-RSJob -Name $deploymentName -Throttle 15 -ScriptBlock {
				Write-Host "--> Starting job to delete $using:deploymentName in $using:resourceGroupName ..."	
				Remove-AzResourceGroupDeployment -ResourceGroupName $using:resourceGroupName  -Name $using:deploymentName
				Write-Host "--> $using:deploymentName deleted"
			}
		}
	}

	Get-RSJob | Receive-RSJob

	Write-Host "-> Old deployments deleted."
}

function DownloadCert()
{
	param( $certName, $keyvaultName, $password )

	Write-Host "-> Downloading Cert $certName to file system... "	

    $pfxPath = ".\$certName.pfx"

    # Download certificate
    Write-Host "--> Getting cert $certName from keyvault $keyvaultName  ... "	
    $secret = Get-AzKeyVaultSecret -VaultName $keyvaultName -Name $certName -AsPlainText
	Write-Host "--> Got cert from keyvault $keyvaultName  ... "	

    # Convert format to PFX
    Write-Host "--> Converting from base 64 ... Key length $($secret.length)"	
    $secretByte = [Convert]::FromBase64String($secret)
    Write-Host "--> Converted from base 64 ... "	

    Write-Host "--> Generating exportable cert ... "	
    $x509Cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2($secretByte, "", "Exportable,PersistKeySet")
    $type = [System.Security.Cryptography.X509Certificates.X509ContentType]::Pfx
    Write-Host "--> Generated exportable cert ... "	
	
    Write-Host "--> Exporting cert btyes ... "	
    $pfxFileByte = $x509Cert.Export($type, $password)
    Write-Host "--> Exported cert btyes ... "	

    # Write to PFX file
    Write-Host "--> Writing to file $pfxPath... "	
    [System.IO.File]::WriteAllBytes($pfxPath, $pfxFileByte)
	Write-Host "--> Writing to file $pfxPath finished."	

	Write-Host "-> Cert $certName downloaded to file system. "	
}

function ApplyCertNonSsl()
{
	param($resourceName, $resourceType, $resourceGroupName, $certName, $password)

    $certPath = ".\$certName.pfx"
    Write-Host "-> Applying Non SSL certificate $certName to $resourceName in $resourceGroupName..."

	Write-Host "--> Upload fake SSL binding..."
	try 
	{
		#Apply the cert to the resource
		New-AzWebAppSSLBinding -WebAppName $resourceName -ResourceGroupName $resourceGroupName -Name 'www.contoso.com' -CertificateFilePath $certPath -CertificatePassword $password
	} 
	catch 
	{
	    Write-Host "--> Expected error uploading certificate..."
	}

	Write-Host "--> Deleting certificate file $certName."
    # Delete the certificate
    Remove-Item -path $certPath

	Write-Host "-> Non SSL Certificate $certName applied."
}

# --- Remove old deployments
DeleteOldDeployments -resourceGroupName $resGroupName

$certPassword = [System.guid]::NewGuid().toString()

# --- Install magic link certificate
DownloadCert -certName 'ptasselfsignedcertificate' -keyvaultName $vaultName -password $certPassword
ApplyCertNonSsl -resourceName "PTAS-$($deployEnv)-MagicLinkService" -ResourceGroupName $resGroupName -certName 'ptasselfsignedcertificate' -password $certPassword

# --- Create a Service Bus Queue and the Queue
####New-AzServiceBusNamespace -ResourceGroupName $resGroupName -NamespaceName 'ptas-$($deployEnv)-dynamics-to-sql-bus' -Location $deployLocation
####New-AzServiceBusQueue -ResourceGroupName $resGroupName -NamespaceName 'ptas-$($deployEnv)-dynamics-to-sql-bus' -Name 'dynamicsqueue' -EnablePartitioning $False