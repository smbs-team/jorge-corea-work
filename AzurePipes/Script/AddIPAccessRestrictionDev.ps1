[CmdletBinding()]

$resGroupName = "ptas-dev-resourcegroup"
$restrictions = @(
    @{
        ipAddress = "52.156.70.17/32"; 
        action = "Allow";
        priority = "65000";
    },
    @{
        ipAddress = "146.129.239.4/32"; 
        action = "Allow";
        priority = "65000";
    },
    @{
        ipAddress = "198.49.222.20/32"; 
        action = "Allow";
        priority = "65000";
    },
    @{
        ipAddress = "146.129.49.0/32"; 
        action = "Allow";
        priority = "65000";
    }
)

function AddIPRestrictionOnDev()
{
    param($resourceGroupName, $appServiceName, [Hashtable[]] $newIpRules)

    write-host "Grab the latest available api version"
    $APIVersion = ((Get-AzResourceProvider -ProviderNamespace Microsoft.Web).ResourceTypes | Where-Object ResourceTypeName -eq sites).ApiVersions[0]

    $WebAppConfig = Get-AzResource -ResourceName $appServiceName -ResourceType Microsoft.Web/sites/config -ResourceGroupName $resourceGroupName -ApiVersion $APIVersion

    write-host "Clean previous IPs from $appServiceName"
    $WebAppConfig.Properties.ipSecurityRestrictions = @()

    write-host "Adding new IP rules to $appServiceName"
    foreach ($NewIpRule in $NewIpRules) {
        $WebAppConfig.Properties.ipSecurityRestrictions += $NewIpRule
    }

    Set-AzResource -ResourceId $WebAppConfig.ResourceId -Properties $WebAppConfig.Properties -ApiVersion $APIVersion -Force
}

write-host "Start process to add IP rules on $resGroupName"
$subscriptionId = Get-AzSubscription -SubscriptionName "Dev"
Select-AzSubscription -SubscriptionId $subscriptionId.Id

$appServName = "ptas-dev-dataservices"
AddIPRestrictionOnDev -resourceGroupName $resGroupName -appServiceName $appServName -newIpRules $restrictions

$appServName = "ptas-dev-documentstorageservices"
AddIPRestrictionOnDev -resourceGroupName $resGroupName -appServiceName $appServName -newIpRules $restrictions

$appServName = "ptas-dev-mediainfo"
AddIPRestrictionOnDev -resourceGroupName $resGroupName -appServiceName $appServName -newIpRules $restrictions

$appServName = "ptas-dev-sharepointfunctions"
AddIPRestrictionOnDev -resourceGroupName $resGroupName -appServiceName $appServName -newIpRules $restrictions