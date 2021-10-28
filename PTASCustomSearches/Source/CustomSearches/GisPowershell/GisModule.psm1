function Get-ServiceUrl
{
    param (
        $Env,
        $MethodName
    )
    if ($Env -eq "localhost")
    {
        return "http://localhost:7071/v2/API/GIS/" + $MethodName
    }
    else
    {
        return "https://ptas-" + $Env + "-customsearchesfunctions.azurewebsites.net/v2/API/GIS/" + $MethodName
	}
}

function Get-JobsServiceUrl
{
    param (
        $Env,
        $MethodName
    )
    if ($Env -eq "localhost")
    {
        return "http://localhost:7071/v2/API/JOBS/" + $MethodName
    }
    else
    {
        return "https://ptas-" + $Env + "-customsearchesfunctions.azurewebsites.net/v2/API/JOBS/" + $MethodName
	}
}

function Import-LayerSource
{
<#
    .SYNOPSIS
        Imports or updates a custom search.

    .DESCRIPTION
        Imports or updates a layer source.  LayerSourceName field is the key that will determine whether the LayerSource will be Imported  or Updated.
		This assumes that the GIS file or table has already been provisioned, and will setup a mapserver map file that allows access to the layer in
		the GIS renderers.
		
    .PARAMETER Env
        Environment where the change will be applied.  Valid choices:  localhost/sbox/dev/test/uat/stage/prdm/prod.

    .PARAMETER Token
        Bearer token.

    .INPUTS
        The script accepts the LayerSource JSon definition as a value from the pipeline.

    .EXAMPLE
        Get-Content -path .\ParcelLayerSource.json -raw | Import-LayerSource -Env sbox -Token $token

    .LINK
      #>
        
    [CmdletBinding()]
    param (
        $Env,
        $Token,
        [Parameter(ValueFromPipeline)]
        $LayerSourceJson
    )

    $url = Get-ServiceUrl -Env $Env -MethodName "ImportLayerSource"

    Write-Host Importing LayerSource with JSon: $LayerSourceJson
    $response = Invoke-WebRequest -Uri $url -Method POST -Body $LayerSourceJson -Headers @{'Authorization' = "Bearer $($Token)"}
    Write-Host $response.ToString()
}

function Start-AsyncJob
{
<#
    .SYNOPSIS
        Starts an asynchronous job.

    .DESCRIPTION
        Starts an asynchronous job.  Returns the id of the started job.  Get-JobStatus can be used to query for the job status.
		
    .PARAMETER Env
        Environment where the change will be applied.  Valid choices:  localhost/sbox/dev/test/uat/stage/prdm/prod.

    .PARAMETER Token
        Bearer token.
		
    .PARAMETER QueueName
        QueueName where the job will be queued.
		
    .PARAMETER JobType
        Type of the job.

    .INPUTS
        The script accepts the Job Payload JSon as a value from the pipeline.

    .EXAMPLE
        Get-Content -path .\JobPayload.json -raw | Start-AsyncJob -Env sbox -Token $token -QueueName 'LayerConversion' -JobType 'LayerConversionProcessor'

    .LINK
      #>
        
    [CmdletBinding()]
    param (
        $Env,
        $Token,
		$QueueName,
		$JobType,
        [Parameter(ValueFromPipeline)]
        $JobPayloadJson
    )

    $url = Get-JobsServiceUrl -Env $Env -MethodName "StartJob"
	$url = $url + '/' + $QueueName + '/' + $JobType

    Write-Host Start job with JSon: $JobPayloadJson
    $response = Invoke-WebRequest -Uri $url -Method POST -Body $JobPayloadJson -Headers @{'Authorization' = "Bearer $($Token)"}
    Write-Host $response.ToString()
}

function Get-AsyncJobStatus
{
<#
    .SYNOPSIS
        Gets the status for an async job.

    .DESCRIPTION
        Gets the status for an async job.  Returns the status and result.
		
    .PARAMETER Env
        Environment where the change will be applied.  Valid choices:  localhost/sbox/dev/test/uat/stage/prdm/prod.

    .PARAMETER Token
        Bearer token.
		
    .PARAMETER JobId
        The Id of the job.
		
    .EXAMPLE
        Get-AsyncJobStatus -Env sbox -Token $token -JobId 1245

    .LINK
      #>
        
    [CmdletBinding()]
    param (
        $Env,
        $Token,
		$JobId
    )

    $url = Get-JobsServiceUrl -Env $Env -MethodName "GetJobStatus"
	$url = $url + "/$JobId"

    Write-Host Get Job Status: $JobId
    $response = Invoke-WebRequest -Uri $url -Method GET -Headers @{'Authorization' = "Bearer $($Token)"}
    Write-Host $response.ToString()
}

Export-ModuleMember -Function Import-LayerSource
Export-ModuleMember -Function Start-AsyncJob

Export-ModuleMember -Function Get-AsyncJobStatus
