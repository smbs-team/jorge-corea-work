[CmdletBinding()]
param (
    $DeploymentEnv,
	$DeploymentSqlEnv,
	$DeploymentResourceGroup,
	$DeploymentKeyVaultName
)

Install-Module -Name PoshRSJob -Confirm:$False -Force

$deployEnv = $DeploymentEnv
$deploySqlEnv = $DeploymentSqlEnv
$resGroupName = $DeploymentResourceGroup
$vaultName = $DeploymentKeyVaultName

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
    $cert = Get-AzKeyVaultSecret -VaultName $keyvaultName -Name $certName
	Write-Host "--> Got cert from keyvault $keyvaultName  ... "	
	
	$secretValueText = $cert.SecretValueText
	#$secretValueText = "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"

    # Convert format to PFX
    Write-Host "--> Converting from base 64 ... "	
	$certBytes = [System.Convert]::FromBase64String($secretValueText)

    $certCollection = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2Collection
    $certCollection.Import($certBytes,$null,[System.Security.Cryptography.X509Certificates.X509KeyStorageFlags]::Exportable)
	Write-Host "--> Converted from base 64 ... "	

    # Write to PFX file
    Write-Host "--> Writing to file $pfxPath... "	
    $protectedCertificateBytes = $certCollection.Export([System.Security.Cryptography.X509Certificates.X509ContentType]::Pkcs12, $password)
    [System.IO.File]::WriteAllBytes($pfxPath, $protectedCertificateBytes)
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
