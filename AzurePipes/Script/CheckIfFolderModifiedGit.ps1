[CmdletBinding()]
param (
    $folder
)

$modulePath = $Env:BUILD_SOURCESDIRECTORY + '\AzurePipes\Script\PipeScriptHelperModule.psm1'
Import-Module $modulePath 

Write-Host "Build Reason: $($Env:BUILD_REASON)"

if ($Env:BUILD_REASON -eq "PullRequest")
{
    $sourceBranch = "$($Env:SYSTEM_PULLREQUEST_SOURCEBRANCH)"
    $sourceBranch = $sourceBranch -replace "refs/heads", "origin"
    $targetBranch = "$($Env:SYSTEM_PULLREQUEST_TARGETBRANCH)"
    $targetBranch = $targetBranch -replace "refs/heads", "origin"

    Write-Host "Comparing branches: $($targetBranch)...$($sourceBranch)"
    $files = Invoke-Expression "git diff --name-only $($targetBranch)...$($sourceBranch)"
}
else
{
    if (($Env:BUILD_REASON -eq "IndividualCI") -or ($Env:BUILD_REASON -eq "BatchedCI") -or ($Env:BUILD_REASON -eq "CheckInShelveset"))
    {
        Write-Host "Getting files from commit: $($Env:BUILD_SOURCEVERSION)"
        
        $files = $(git log -m -1 --name-only --pretty="format:" $($Env:BUILD_SOURCEVERSION))
    } else
    {
        $files = ""
	}
}

$temp = $files -split ' '
$count = $temp.Length

$FolderModified = $False
For ($i=0; $i -lt $temp.Length; $i++)
{
    $name = $temp[$i]
    Write-Host "Detected modified file: $($name)"
    if ($name -like $folder)
    {
        $FolderModified=$True        
    }
}

Write-Host "$($folder) folder - CheckIfFolderModifiedGit output: $($FolderModified)"
Write-OutuptVariable -variableName "FolderChanged" -variableValue $FolderModified