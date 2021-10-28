[CmdletBinding()]
param (
    $path,
    $file
)

$modulePath = $Env:BUILD_SOURCESDIRECTORY + '\AzurePipes\Script\PipeScriptHelperModule.psm1'
Import-Module $modulePath 


Write-Host "Checking if file $($file) exists in path $($path)"
$files = Get-ChildItem -PATH "$($path)" "$($file)" -recurse

if ($files)
{
    Write-Host "File exists!"
    Write-OutuptVariable -variableName "FileFound" -variableValue 'True'
}
else
{
    Write-Host "File doesn't exist!"
    Write-OutuptVariable -variableName "FileFound" -variableValue 'False'
}
