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

$certPassword = "j.O-J*qa0Cit"

# --- Install SSL certificate for seniors portal
DownloadCert -certName $sslCertName -keyvaultName $sslVaultName -password $certPassword
