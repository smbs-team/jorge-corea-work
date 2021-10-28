[CmdletBinding()]
param (
	$DeploymentSslKeyVaultName,
	$DeploymentSslCertName
)

$sslVaultName = $DeploymentSslKeyVaultName
$sslCertName = $DeploymentSslCertName

function DownloadCert()
{
	param( $certName, $keyvaultName, $password )

	Write-Host "-> Downloading Cert $certName to file system... "	

    $pfxPath = ".\$certName.pfx"

    # Download certificate
    Write-Host "--> Getting cert $certName from keyvault $keyvaultName  ... "	
    $cert = Get-AzKeyVaultSecret -VaultName $keyvaultName -Name $certName
	Write-Host "--> Got cert from keyvault $keyvaultName  ... "	

    # Convert format to PFX
    Write-Host "--> Converting from base 64 ... "	
    $certBytes = [System.Convert]::FromBase64String($cert.SecretValueText)
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

$certPassword = "j.O-J*qa0Cit"

# --- Install SSL certificate for seniors portal
DownloadCert -certName $sslCertName -keyvaultName $sslVaultName -password $certPassword
