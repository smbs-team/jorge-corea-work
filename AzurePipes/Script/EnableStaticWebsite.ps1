[CmdletBinding()]
param (
    $DeploymentResourceGroup,
    $StorageAccountName,
    $ErrorDocumentName,
    $IndexDocumentName
)

#Get the storage account context that defines the storage account you want to use.
write-host "Getting Storage Account: $DeploymentResourceGroup - $StorageAccountName"
$storageAccount = Get-AzStorageAccount -ResourceGroupName "$DeploymentResourceGroup" -AccountName "$StorageAccountName"
$ctx = $storageAccount.Context

#Enable static website hosting.
write-host "Enabling StaticWebsite on context $ctx"
if (($ErrorDocumentName -eq $null) -and ($IndexDocumentName -eq $null))
{
    Enable-AzStorageStaticWebsite -Context $ctx
}
elseif ($ErrorDocumentName -eq $null)
{
    Enable-AzStorageStaticWebsite -Context $ctx -IndexDocument $IndexDocumentName
}
elseif ($IndexDocumentName -eq $null)
{
    Enable-AzStorageStaticWebsite -Context $ctx -ErrorDocument404Path $ErrorDocumentName
}
else
{
    Enable-AzStorageStaticWebsite -Context $ctx -IndexDocument $IndexDocumentName -ErrorDocument404Path $ErrorDocumentName
}
write-host "StaticWebsite Enabled"