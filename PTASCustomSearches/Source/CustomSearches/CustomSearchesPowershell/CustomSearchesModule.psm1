function Get-ServiceUrl
{
    param (
        $Env,
        $MethodName
    )
    if ($Env -eq "localhost")
    {
        return "http://localhost:7071/v2/API/CustomSearches/" + $MethodName
    }
    elseif ($Env.ToLower() -eq "sbox")
    {
        return "https://ptas-" + $Env + "-customsearchesfunctions.azurewebsites.net/v2/API/CustomSearches/" + $MethodName
	}
    else
    {
        return "https://api-test.kingcounty.gov/ptas-" + $Env + "-customsearchesfunctions/v2r4/API/CustomSearches/" + $MethodName
	}
}

function Get-SharedServiceUrl
{
    param (
        $Env,
        $MethodName
    )
    if ($Env -eq "localhost")
    {
        return "http://localhost:7071/v2/API/Shared/" + $MethodName
    }
    elseif ($Env.ToLower() -eq "sbox")
    {
        return "https://ptas-" + $Env + "-customsearchesfunctions.azurewebsites.net/v2/API/Shared/" + $MethodName
	}
    else
    {
        return "https://api-test.kingcounty.gov/ptas-" + $Env + "-customsearchesfunctions/v2r4/API/Shared/" + $MethodName
	}
}

function Import-CustomSearch 
{
<#
    .SYNOPSIS
        Imports or updates a custom search.

    .DESCRIPTION
        Imports or updates a custom search.  Name field is the key that will determine whether the Custom Search will be Imported  or Updated.
        If this is an update and the fields returned by the stored procedures diverge from the original, existing datasets
        might be broken.  Update operations should be done with extreme care.

    .PARAMETER Env
        Environment where the change will be applied.  Valid choices:  localhost/sbox/dev/test/uat/stage/prdm/prod.

    .PARAMETER Token
        Bearer token.

    .PARAMETER StoredProcedureOutputChanged
        Indicates whether the stored procedure output has changed.  This forces a new version of the Custom Search to be created.

    .INPUTS
        The script accepts the Custom Search JSon definition as a value from the pipeline.

    .EXAMPLE
        Get-Content -path .\SalesCustomSearchDefinition.json -raw | Import-CustomSearch -Env sbox -Token $token -StoredProcedureOutputChanged $false

    .LINK
        https://kc1.sharepoint.com/teams/IT/PMCOE/PTAS/_layouts/OneNote.aspx?id=%2Fteams%2FIT%2FPMCOE%2FPTAS%2FShared%20Documents%2FPTAS%20Program%20and%20planning%2FPTAS%20Dev%2FConnectors%2C%20Mobile%20and%20Web%20Controls%20General%2FPTASDevOneNote&wd=target%28Custom%20Searches.one%7CFEA597FF-6D0B-44A6-ABF8-80E8A4F97B4F%2FImport-RScriptModel%7CDA01D057-AA18-4D38-B318-F99D4E322DEE%2F%29
    #>

    [CmdletBinding()]
    param (
        $Env,
        $Token,
        $StoredProcedureOutputChanged=$false,
        [Parameter(ValueFromPipeline)]
        $CustomSearchJson
    )

    $url = Get-ServiceUrl -Env $Env -MethodName "ImportCustomSearch"

    if ($StoredProcedureOutputChanged)
    {
        $url = $url + "?sPOutputChanged=true"
	}

    Write-Host Importing CustomSearch with JSon: $CustomSearchJson
    Write-Host Server URL: $url
    $response = Invoke-WebRequest -Uri $url -Method POST -Body $CustomSearchJson -Headers @{'Authorization' = "Bearer $($Token)"}
    Write-Host $response.ToString()
}

function Import-RScriptModel
{
<#
    .SYNOPSIS
        Imports or updates a RScriptModel.

    .DESCRIPTION
        Imports or updates a RScriptModel.  RscriptModelName field is the key that will determine whether the RScriptModel will be Imported  or Updated.

    .PARAMETER Env
        Environment where the change will be applied.  Valid choices:  localhost/sbox/dev/test/uat/stage/prdm/prod.

    .PARAMETER Token
        Bearer token.

    .INPUTS
        The script accepts the RScriptModel JSon definition as a value from the pipeline.

    .EXAMPLE
        Get-Content -path .\LinearRegressionRScriptModel.json -raw | Import-RScriptModel -Env sbox -Token $token

    .LINK
        https://kc1.sharepoint.com/teams/IT/PMCOE/PTAS/_layouts/OneNote.aspx?id=%2Fteams%2FIT%2FPMCOE%2FPTAS%2FShared%20Documents%2FPTAS%20Program%20and%20planning%2FPTAS%20Dev%2FConnectors%2C%20Mobile%20and%20Web%20Controls%20General%2FPTASDevOneNote&wd=target%28Custom%20Searches.one%7CFEA597FF-6D0B-44A6-ABF8-80E8A4F97B4F%2FImport-CustomSearch%7CDA01D057-AA18-4D38-B318-F99D4E322DEE%2F%29
    #>

    [CmdletBinding()]
    param (
        $Env,
        $Token,
        [Parameter(ValueFromPipeline)]
        $RScriptModelJson
    )

    $url = Get-ServiceUrl -Env $Env -MethodName "ImportRScriptModel"

    Write-Host Importing RScriptModel with JSon: $RScriptModelJson
    $response = Invoke-WebRequest -Uri $url -Method POST -Body $RScriptModelJson -Headers @{'Authorization' = "Bearer $($Token)"}
    Write-Host $response.ToString()
}

function Upload-RScriptFile
{
<#
    .SYNOPSIS
        Uploads a file asset into the blob storage folder for an RScriptModel.

    .DESCRIPTION
        Uploads a file asset from a folder into the blob storage folder for an RScriptModel.  This method can be used to upload RScript code
        or other dependencies needed by the code.

    .PARAMETER RScriptModelId
        Id of the RScriptModel

    .PARAMETER FilePath
        Path of the file to upload.

    .PARAMETER Env
        Environment where the change will be applied.  Valid choices:  localhost/sbox/dev/test/uat/stage/prdm/prod.

    .PARAMETER Token
        Bearer token.


    .EXAMPLE
        Upload-RScriptFile -RScriptModelId 45 -FilePath 'C:/RScript/LinearRegression/LinearRegression.r' -Env sbox -Token $token

    .LINK
        https://kc1.sharepoint.com/teams/IT/PMCOE/PTAS/_layouts/OneNote.aspx?id=%2Fteams%2FIT%2FPMCOE%2FPTAS%2FShared%20Documents%2FPTAS%20Program%20and%20planning%2FPTAS%20Dev%2FConnectors%2C%20Mobile%20and%20Web%20Controls%20General%2FPTASDevOneNote&wd=target%28Custom%20Searches.one%7CFEA597FF-6D0B-44A6-ABF8-80E8A4F97B4F%2FUpload-RScriptFile%7CAD8FEE5E-693A-41B5-AF2D-2443171EBAF0%2F%29
    #>

    [CmdletBinding()]
    param (
        $RScriptModelId,
        $FilePath,
        $Env,
        $Token
    )

    if (Test-Path $FilePath -PathType leaf)
    {
        $url = Get-ServiceUrl -Env $Env -MethodName "UploadRScriptFile"
        $url = $url + "/$RScriptModelId"
        $fileContent = Get-Content -Path $FilePath -Encoding Byte -Raw
        $file = Get-Item -Path $FilePath
    
        Write-Host Uploading file: $FilePath
        $response = Invoke-WebRequest -Uri $url -Method POST -Body $fileContent -Headers @{ 'Authorization' = "Bearer $($Token)"; 'FileName' = $file.Name }
        Write-Host $response.ToString()
    } 
    else
    {
        Write-Host File $FilePath not found.
	}
}

function Upload-RScriptFiles
{
<#
    .SYNOPSIS
        Imports file assets from a folder into the blob storage folder for an RScriptModel.

    .DESCRIPTION
        Imports file assets from a folder into the blob storage folder for an RScriptModel.  This method can be used to upload RScript code
        or other dependencies needed by the code.  This command does not traverse the folder recursively.

    .PARAMETER RScriptModelId
        Id of the RScriptModel

    .PARAMETER FolderPath
        Folder where the file assets to upload are located.

    .PARAMETER Env
        Environment where the change will be applied.  Valid choices:  localhost/sbox/dev/test/uat/stage/prdm/prod.

    .PARAMETER Token
        Bearer token.

    .EXAMPLE
        Upload-RScriptFiles -RScriptModelId 45 -FolderPath 'C:/RScript/LinearRegression' -Env sbox -Token $token

    .LINK
        https://kc1.sharepoint.com/teams/IT/PMCOE/PTAS/_layouts/OneNote.aspx?id=%2Fteams%2FIT%2FPMCOE%2FPTAS%2FShared%20Documents%2FPTAS%20Program%20and%20planning%2FPTAS%20Dev%2FConnectors%2C%20Mobile%20and%20Web%20Controls%20General%2FPTASDevOneNote&wd=target%28Custom%20Searches.one%7CFEA597FF-6D0B-44A6-ABF8-80E8A4F97B4F%2FUpload-RScriptFiles%7C14C8AEAF-D94B-41DE-916A-A6DD65521176%2F%29
#>

    [CmdletBinding()]
    param (
        $RScriptModelId,
        $FolderPath,
        $Env,
        $Token
    )

    if (Test-Path $FolderPath -PathType container)
    {
        Write-Host Importing Uploading files in folder: $FolderPath

        Get-ChildItem "$FolderPath" | 
            Foreach-Object {
                $fileName = "$_" | split-path -leaf
                $filePath = Join-Path -Path  $FolderPath -ChildPath $fileName 
                Write-Host "Calling Upload-RScriptFile for $filePath "                
                Upload-RScriptFile -RScriptModelId $RScriptModelId -FilePath $filePath -Env $Env -Token $Token
            }
        
        Write-Host Finished uploading files in folder: $FolderPath
    }
    else
    {
        Write-Host Folder $FolderPath not found.
	}
}

function Delete-RScriptModel
{
<#
    .SYNOPSIS
        Deletes a RScriptModel.

    .DESCRIPTION
        Deletes a RScriptModel.

    .PARAMETER RScriptModelId
        Id of the RScriptModel to delete.

    .PARAMETER Env
        Environment where the change will be applied.  Valid choices:  localhost/sbox/dev/test/uat/stage/prdm/prod.

    .PARAMETER Token
        Bearer token.

    .EXAMPLE
        Delete-RScriptModel -RScriptModelId 45 -Env sbox -Token $token

    .LINK
        https://kc1.sharepoint.com/teams/IT/PMCOE/PTAS/_layouts/OneNote.aspx?id=%2Fteams%2FIT%2FPMCOE%2FPTAS%2FShared%20Documents%2FPTAS%20Program%20and%20planning%2FPTAS%20Dev%2FConnectors%2C%20Mobile%20and%20Web%20Controls%20General%2FPTASDevOneNote&wd=target%28Custom%20Searches.one%7CFEA597FF-6D0B-44A6-ABF8-80E8A4F97B4F%2FDelete-RScritpModel%7C049D506C-5D4C-48E6-8D2E-966A1B78C21D%2F%29
#>

    [CmdletBinding()]
    param (
        $RScriptModelId,
        $Env,
        $Token
    )

    $url = Get-ServiceUrl -Env $Env -MethodName "DeleteRScriptModel"
    $url = $url + "/$RScriptModelId"

    Write-Host Deleting RScriptModel: $RScriptModelId

    $response = Invoke-WebRequest -Uri $url -Method POST -Headers @{ 'Authorization' = "Bearer $($Token)" }
        
    Write-Host Finished deleting RScriptModel: $RScriptModelId
}

function Get-RScriptModels
{
<#
    .SYNOPSIS
        Gets the list of RScriptModels.

    .DESCRIPTION
        Gets the list of RScriptModels.

    .PARAMETER Env
        Environment where the change will be applied.  Valid choices:  localhost/sbox/dev/test/uat/stage/prdm/prod.

    .PARAMETER Token
        Bearer token.

    .EXAMPLE
        Get-RScriptModel -Env sbox -Token $token

    .LINK
        https://kc1.sharepoint.com/teams/IT/PMCOE/PTAS/_layouts/OneNote.aspx?id=%2Fteams%2FIT%2FPMCOE%2FPTAS%2FShared%20Documents%2FPTAS%20Program%20and%20planning%2FPTAS%20Dev%2FConnectors%2C%20Mobile%20and%20Web%20Controls%20General%2FPTASDevOneNote&wd=target%28Custom%20Searches.one%7CFEA597FF-6D0B-44A6-ABF8-80E8A4F97B4F%2FGet-RScriptModels%7C237D888B-3A6F-4843-91F9-4BB7FA776D0F%2F%29
    #>

    [CmdletBinding()]
    param (
        $Env,
        $Token
    )

    $url = Get-ServiceUrl -Env $Env -MethodName "GetRScriptModels"

    Write-Host Getting RScriptModels
    $response = Invoke-WebRequest -Uri $url -Method GET -Headers @{'Authorization' = "Bearer $($Token)"}
    Write-Host $response.ToString()
}

function Delete-CustomSearch
{
<#
    .SYNOPSIS
        Deletes a Custom Search.

    .DESCRIPTION
        Deletes a Custom Search.

    .PARAMETER RScriptModelId
        Id of the Custom Search to delete.

    .PARAMETER Env
        Environment where the change will be applied.  Valid choices:  localhost/sbox/dev/test/uat/stage/prdm/prod.

    .PARAMETER Token
        Bearer token.

    .EXAMPLE
        Delete-CustomSearch -CustomSearchId 45 -Env sbox -Token $token

    .LINK
        https://kc1.sharepoint.com/teams/IT/PMCOE/PTAS/_layouts/OneNote.aspx?id=%2Fteams%2FIT%2FPMCOE%2FPTAS%2FShared%20Documents%2FPTAS%20Program%20and%20planning%2FPTAS%20Dev%2FConnectors%2C%20Mobile%20and%20Web%20Controls%20General%2FPTASDevOneNote&wd=target%28Custom%20Searches.one%7CFEA597FF-6D0B-44A6-ABF8-80E8A4F97B4F%2FDelete-RScritpModel%7C049D506C-5D4C-48E6-8D2E-966A1B78C21D%2F%29
#>

    [CmdletBinding()]
    param (
        $CustomSearchId,
        $Env,
        $Token
    )

    $url = Get-ServiceUrl -Env $Env -MethodName "DeleteCustomSearch"
    $url = $url + "/$CustomSearchId"

    Write-Host Deleting Custom Search: $CustomSearchId

    $response = Invoke-WebRequest -Uri $url -Method POST -Headers @{ 'Authorization' = "Bearer $($Token)" }
        
    Write-Host Finished deleting the Custom Search: $CustomSearchId
}

function Unlock-LockedDataset
{
<#
    .SYNOPSIS
        Unlocks a locked dataset.

    .DESCRIPTION
        Unlocks a locked dataset.

    .PARAMETER RScriptModelId
        Id of the dataset to unlock.

    .PARAMETER Env
        Environment where the change will be applied.  Valid choices:  localhost/sbox/dev/test/uat/stage/prdm/prod.

    .PARAMETER Token
        Bearer token.

    .EXAMPLE
        Unlock-LockedDataset -DatasetId '9C3EA2F8-41E8-4B9A-ACE6-001F83535871' -Env sbox -Token $token

    .LINK
        https://kc1.sharepoint.com/teams/IT/PMCOE/PTAS/_layouts/OneNote.aspx?id=%2Fteams%2FIT%2FPMCOE%2FPTAS%2FShared%20Documents%2FPTAS%20Program%20and%20planning%2FPTAS%20Dev%2FConnectors%2C%20Mobile%20and%20Web%20Controls%20General%2FPTASDevOneNote&wd=target%28Custom%20Searches.one%7CFEA597FF-6D0B-44A6-ABF8-80E8A4F97B4F%2FDelete-RScritpModel%7C049D506C-5D4C-48E6-8D2E-966A1B78C21D%2F%29
#>

    [CmdletBinding()]
    param (
        $DatasetId,
        $Env,
        $Token
    )

    $url = Get-ServiceUrl -Env $Env -MethodName "UnlockLockedDataset"
    $url = $url + "/$DatasetId"

    Write-Host Unlocking a locked dataset: $CustomSearchId

    $response = Invoke-WebRequest -Uri $url -Method POST -Headers @{ 'Authorization' = "Bearer $($Token)" }
        
    Write-Host Finished unlocking a locked dataset: $CustomSearchId
}

function Import-ChartTemplate
{
<#
    .SYNOPSIS
        Imports or updates a Chart Template.

    .DESCRIPTION
        Imports or updates a Chart Template.  ChartTitle field is the key that will determine whether the Chart Template will be Imported  or Updated.

    .PARAMETER Env
        Environment where the change will be applied.  Valid choices:  localhost/sbox/dev/test/uat/stage/prdm/prod.

    .PARAMETER Token
        Bearer token.

    .INPUTS
        The script accepts the Chart Template JSon definition as a value from the pipeline.

    .EXAMPLE
        Get-Content -path .\ChartTemplate1.json -raw | Import-ChartTemplate -Env sbox -Token $token

    .LINK
        https://kc1.sharepoint.com/teams/IT/PMCOE/PTAS/_layouts/OneNote.aspx?id=%2Fteams%2FIT%2FPMCOE%2FPTAS%2FShared%20Documents%2FPTAS%20Program%20and%20planning%2FPTAS%20Dev%2FConnectors%2C%20Mobile%20and%20Web%20Controls%20General%2FPTASDevOneNote&wd=target%28Custom%20Searches.one%7CFEA597FF-6D0B-44A6-ABF8-80E8A4F97B4F%2FImport-CustomSearch%7CDA01D057-AA18-4D38-B318-F99D4E322DEE%2F%29
    #>

    [CmdletBinding()]
    param (
        $Env,
        $Token,
        [Parameter(ValueFromPipeline)]
        $ChartTemplateJson
    )

    $url = Get-ServiceUrl -Env $Env -MethodName "ImportChartTemplate"

    Write-Host Importing Chart Template with JSon: $ChartTemplateJson
    $response = Invoke-WebRequest -Uri $url -Method POST -Body $ChartTemplateJson -Headers @{'Authorization' = "Bearer $($Token)"}
    Write-Host $response.ToString()
}

function Import-ProjectType
{
<#
    .SYNOPSIS
        Imports or updates a Project Type.

    .DESCRIPTION
        Imports or updates a Project Type.  ProjectTypeName field is the key that will determine whether the Project Type will be Imported or Updated.

    .PARAMETER Env
        Environment where the change will be applied.  Valid choices:  localhost/sbox/dev/test/uat/stage/prdm/prod.

    .PARAMETER Token
        Bearer token.

    .INPUTS
        The script accepts the Project Type JSon definition as a value from the pipeline.

    .EXAMPLE
        Get-Content -path .\PhysicalInspection.json -raw | Import-ProjectType -Env sbox -Token $token

    .LINK
        https://kc1.sharepoint.com/teams/IT/PMCOE/PTAS/_layouts/OneNote.aspx?id=%2Fteams%2FIT%2FPMCOE%2FPTAS%2FShared%20Documents%2FPTAS%20Program%20and%20planning%2FPTAS%20Dev%2FConnectors%2C%20Mobile%20and%20Web%20Controls%20General%2FPTASDevOneNote&wd=target%28Custom%20Searches.one%7CFEA597FF-6D0B-44A6-ABF8-80E8A4F97B4F%2FImport-CustomSearch%7CDA01D057-AA18-4D38-B318-F99D4E322DEE%2F%29
    #>

    [CmdletBinding()]
    param (
        $Env,
        $Token,
        [Parameter(ValueFromPipeline)]
        $ProjectTypeJson
    )

    $url = Get-ServiceUrl -Env $Env -MethodName "ImportProjectType"

    Write-Host Importing ProjectType with JSon: $ProjectTypeJson
    $response = Invoke-WebRequest -Uri $url -Method POST -Body $ProjectTypeJson -Headers @{'Authorization' = "Bearer $($Token)"}
    Write-Host $response.ToString()
}

function Import-MetadataStoreItems
{
<#
    .SYNOPSIS
        Imports or updates one or more Metadata Store Items.

    .DESCRIPTION
        Imports or updates one or more Metadata Store Items.  StoreType/ItemName/Version fields are the key that will determine whether the Metadata Store Item will be Imported or Updated.

    .PARAMETER Env
        Environment where the change will be applied.  Valid choices:  localhost/sbox/dev/test/uat/stage/prdm/prod.

    .PARAMETER Token
        Bearer token.

    .INPUTS
        The script accepts the Metadata Store Item JSon definition as a value from the pipeline.

    .EXAMPLE
        Get-Content -path .\StoreItems.json -raw | Import-MetadataStoreItems -Env sbox -Token $token

    .LINK
        https://kc1.sharepoint.com/teams/IT/PMCOE/PTAS/_layouts/OneNote.aspx?id=%2Fteams%2FIT%2FPMCOE%2FPTAS%2FShared%20Documents%2FPTAS%20Program%20and%20planning%2FPTAS%20Dev%2FConnectors%2C%20Mobile%20and%20Web%20Controls%20General%2FPTASDevOneNote&wd=target%28Custom%20Searches.one%7CFEA597FF-6D0B-44A6-ABF8-80E8A4F97B4F%2FImport-CustomSearch%7CDA01D057-AA18-4D38-B318-F99D4E322DEE%2F%29
    #>

    [CmdletBinding()]
    param (
        $Env,
        $Token,
        [Parameter(ValueFromPipeline)]
        $MetadataStoreItemJson
    )

    $url = Get-SharedServiceUrl -Env $Env -MethodName "ImportMetadataStoreItems"

    Write-Host Importing Metadata Store Items with JSon: $MetadataStoreItemJson
    $response = Invoke-WebRequest -Uri $url -Method POST -Body $MetadataStoreItemJson -Headers @{'Authorization' = "Bearer $($Token)"}
    Write-Host Finished importing Metadata Store Items
}


function Get-MetadataStoreItems
{
<#
    .SYNOPSIS
        Gets the list of MetadataStoreItems.

    .DESCRIPTION
        Gets the list of MetadataStoreItems filtered by StoreType

    .PARAMETER Env
        Environment where the change will be applied.  Valid choices:  localhost/sbox/dev/test/uat/stage/prdm/prod.

    .PARAMETER Token
        Bearer token.

    .PARAMETER StoreType
        Store type.

    .PARAMETER LatestVersion
        Latest version.

    .EXAMPLE
        Get-MetadataStoreItems -Env sbox -Token $token -StoreType "GlobalConstant" -LatestVersion True

    .LINK
        https://kc1.sharepoint.com/teams/IT/PMCOE/PTAS/_layouts/OneNote.aspx?id=%2Fteams%2FIT%2FPMCOE%2FPTAS%2FShared%20Documents%2FPTAS%20Program%20and%20planning%2FPTAS%20Dev%2FConnectors%2C%20Mobile%20and%20Web%20Controls%20General%2FPTASDevOneNote&wd=target%28Custom%20Searches.one%7CFEA597FF-6D0B-44A6-ABF8-80E8A4F97B4F%2FGet-RScriptModels%7C237D888B-3A6F-4843-91F9-4BB7FA776D0F%2F%29
    #>

    [CmdletBinding()]
    param (
        $Env,
        $Token,
        $StoreType,
        $LatestVersion
    )

    $url = Get-SharedServiceUrl -Env $Env -MethodName "GetMetadataStoreItems"
    $url += "/$StoreType"
    $url += "?latestVersion=$($LatestVersion)"

    Write-Host Getting Metadata Store Items
    $response = Invoke-WebRequest -Uri $url -Method GET -Headers @{'Authorization' = "Bearer $($Token)"}
    Write-Host $response.ToString()
}

function Delete-MetadataStoreItem
{
<#
    .SYNOPSIS
        Deletes a Metadata Store Item.

    .DESCRIPTION
        Deletes a Metadata Store Item.

    .PARAMETER RScriptModelId
        Id of the Metadata Store Item to delete.

    .PARAMETER Env
        Environment where the change will be applied.  Valid choices:  localhost/sbox/dev/test/uat/stage/prdm/prod.

    .PARAMETER Token
        Bearer token.

    .EXAMPLE
        Delete-MetadataStoreItem -MetadataStoreItem 45 -Env sbox -Token $token

    .LINK
        https://kc1.sharepoint.com/teams/IT/PMCOE/PTAS/_layouts/OneNote.aspx?id=%2Fteams%2FIT%2FPMCOE%2FPTAS%2FShared%20Documents%2FPTAS%20Program%20and%20planning%2FPTAS%20Dev%2FConnectors%2C%20Mobile%20and%20Web%20Controls%20General%2FPTASDevOneNote&wd=target%28Custom%20Searches.one%7CFEA597FF-6D0B-44A6-ABF8-80E8A4F97B4F%2FDelete-RScritpModel%7C049D506C-5D4C-48E6-8D2E-966A1B78C21D%2F%29
#>

    [CmdletBinding()]
    param (
        $MetadataStoreItemId,
        $Env,
        $Token
    )

    $url = Get-SharedServiceUrl -Env $Env -MethodName "DeleteMetadataStoreItem"
    $url = $url + "/$MetadataStoreItemId"

    Write-Host Deleting Metadata Store Item: $MetadataStoreItemId

    $response = Invoke-WebRequest -Uri $url -Method POST -Headers @{ 'Authorization' = "Bearer $($Token)" }
        
    Write-Host Finished deleting the Metadata Store Item: $MetadataStoreItemId
}

function Download-PostProcessDump
{
<#
    .SYNOPSIS
        Downloads the dump files from a post-process.

    .DESCRIPTION
        When a (RScript) post-process fails, all files related to the post-process execution are dumped into the blob storage.  This command
        allows to retrieve them and write them in a file folder.

    .PARAMETER PostProcessId
        Id of the PostProcess

    .PARAMETER FolderPath
        Folder where the file assets will be downloaded.

    .PARAMETER Env
        Environment where the change will be applied.  Valid choices:  localhost/sbox/dev/test/uat/stage/prdm/prod.

    .PARAMETER Token
        Bearer token.

    .EXAMPLE
       Download-PostProcessDump -PostProcessId 1678 -FolderPath 'C:/PostProcessDump' -Env sbox -Token $token

    .LINK
        https://kc1.sharepoint.com/teams/IT/PMCOE/PTAS/_layouts/OneNote.aspx?id=%2Fteams%2FIT%2FPMCOE%2FPTAS%2FShared%20Documents%2FPTAS%20Program%20and%20planning%2FPTAS%20Dev%2FConnectors%2C%20Mobile%20and%20Web%20Controls%20General%2FPTASDevOneNote&wd=target%28Custom%20Searches.one%7CFEA597FF-6D0B-44A6-ABF8-80E8A4F97B4F%2FUpload-RScriptFiles%7C14C8AEAF-D94B-41DE-916A-A6DD65521176%2F%29
#>

    [CmdletBinding()]
    param (
        $PostProcessId,
        $FolderPath,
        $Env,
        $Token
    )

    if (Test-Path $FolderPath -PathType container)
    {
        
        Write-Host Getting Dump File List...
        $url = Get-ServiceUrl -Env $Env -MethodName "GetPostProcessDumpFiles"
        $url = $url + "/$PostProcessId"

        Write-Host Getting dump file manifest... $url
        $response = Invoke-WebRequest -Uri $url -Method GET -Headers @{ 'Authorization' = "Bearer $($Token)" }
        Write-Host Server Response: $response

        $responseObject = ConvertFrom-Json –InputObject $response                

        $responseObject.files |  
            Foreach-Object {
                $filename = "$_" | split-path -leaf

                $url = Get-ServiceUrl -Env $Env -MethodName "GetDatasetFile"
                $url = $url + "/$PostProcessId/$($filename)?checkFileInResults=false"
                $targetFilePath = "$FolderPath\$($filename)"

                Write-Host "Downloading file from url: $url"
                $WebClient = New-Object System.Net.WebClient
                $webClient.Headers.add('Authorization', "Bearer $($Token)")
                $WebClient.DownloadFile($url ,$targetFilePath)
            }
        
        Write-Host Finished downloading files to folder: $FolderPath
    }
    else
    {
        Write-Host Folder $FolderPath not found.
	}
}

Export-ModuleMember -Function Import-CustomSearch
Export-ModuleMember -Function Import-RScriptModel
Export-ModuleMember -Function Upload-RScriptFile
Export-ModuleMember -Function Upload-RScriptFiles
Export-ModuleMember -Function Get-ServiceUrl
Export-ModuleMember -Function Get-RScriptModels
Export-ModuleMember -Function Delete-RScriptModel
Export-ModuleMember -Function Delete-CustomSearch
Export-ModuleMember -Function Unlock-LockedDataset
Export-ModuleMember -Function Import-ChartTemplate
Export-ModuleMember -Function Import-ProjectType
Export-ModuleMember -Function Import-MetadataStoreItems
Export-ModuleMember -Function Get-MetadataStoreItems
Export-ModuleMember -Function Delete-MetadataStoreItem
Export-ModuleMember -Function Download-PostProcessDump