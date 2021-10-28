[CmdletBinding()]
param (
    $DeploymentResourceGroup,
    $StorageAccountName,
    $FilePath,
    $Container
)

#Get the storage account context that defines the storage account you want to use.
write-host "Getting Storage Account: $DeploymentResourceGroup - $StorageAccountName"
$storageAccount = Get-AzStorageAccount -ResourceGroupName "$DeploymentResourceGroup" -AccountName "$StorageAccountName"
$ctx = $storageAccount.Context

write-host "Coping files to container $Container"
Get-ChildItem "$FilePath" | 
Foreach-Object {
    $mimeType = [System.Web.MimeMapping]::GetMimeMapping("$FilePath\$_")
    set-AzStorageblobcontent -File "$FilePath\$_" `
    -Container $Container `
    -Blob "$_" `
    -Context $ctx `
    -Force `
    -Properties @{"ContentType" = "$mimeType"}
}
write-host "Files Copied"