[CmdletBinding()]
param (
    $DeploymentEnv,
	$DeploymentEnvShort,
	$DeploymentResourceGroup,
	$DeploymentSslCertName
)

$deployEnv = $DeploymentEnv
$deployEnvShort = $DeploymentEnvShort
$resGroupName = $DeploymentResourceGroup
$sslCertName = $DeploymentSslCertName

function ApplyCertSsl()
{
	param($resourceName, $resourceType, $resourceGroupName, $certName, $password, $customDomain)

    $certPath = ".\$certName.pfx"
    Write-Host "-> Applying SSL certificate $certName to $resourceName in $resourceGroupName..."

	Write-Host "--> Upload SSL binding..."

	#Apply the cert to the resource
	New-AzWebAppSSLBinding -WebAppName $resourceName -ResourceGroupName $resourceGroupName -Name $customDomain -CertificateFilePath $certPath -CertificatePassword $password -SslState SniEnabled

	Write-Host "--> Deleting certificate file $certName."
    # Delete the certificate
    Remove-Item -path $certPath

	Write-Host "-> SSL Certificate $certName applied."
}


$certPassword = "j.O-J*qa0Cit"

# --- Install SSL certificate for seniors portal
if ($deployEnvShort.ToLower() -eq "prod")
{
	$customDomain =  "senior-exemption.kingcounty.gov"
} 
else
{
	$customDomain =  "senior-exemption-$($deployEnv).kingcounty.gov"
}

ApplyCertSsl -resourceName "PTAS-$($deployEnvShort)-SeniorsPortal" -ResourceGroupName $resGroupName -certName $sslCertName -password $certPassword -customDomain $customDomain