param([string]$SolutionDir,  [string]$ProjectName,  [string]$ProjectDir,   [string]$TargetDir,  [string]$TargetPath, [string]$ConfigurationName) ;
Write-Host 'PowerShell Script: PreBuild.ps1'

Write-Host 'SolutionDir: ' $SolutionDir 
Write-Host 'ProjectName: ' $ProjectName
Write-Host 'ProjectDir: ' $ProjectDir 
Write-Host 'TargetDir: ' $TargetDir
Write-Host 'TargetPath: ' $TargetPath
Write-Host 'ConfigurationName: ' $ConfigurationName

$defaultAssemblyInfoPath = Join-Path $ProjectDir "AssemblyInfo.vb"
$alternativeAssemblyInfoPath = Join-Path ($ProjectDir + "\My Project") "AssemblyInfo.vb"

# Check if the AssemblyInfo.vb file exists in the default location
if (Test-Path $defaultAssemblyInfoPath) {
    $assemblyInfoFile = $defaultAssemblyInfoPath
} 
# If not found in the default location, check the alternative location
elseif (Test-Path $alternativeAssemblyInfoPath) {
    $assemblyInfoFile = $alternativeAssemblyInfoPath
} 
# If not found in either location, set to $null (or handle in a different way if needed)
else {
    $assemblyInfoFile = $null
    Write-Warning "AssemblyInfo.vb not found in either location!"
}

Write-Host 'assemblyInfoFile: ' $assemblyInfoFile

if ($ConfigurationName -eq "Release")
{
    # (?<!\x27.*) instructs the regex to ignore commented out lines using the character ' aka x27
    $pattern = '(?<!\x27.*)AssemblyVersion\("(.*)"\)'
    $content = Get-Content $assemblyInfoFile -Raw

    if ($content -match $pattern) {
        $version = $matches[1]
        $versionParts = $version.Split(".")
        Write-Host 'Original Version: ' $version
        $versionParts[1] = ([int]$versionParts[1] + 1).ToString()
        $version = [string]::Join(".", $versionParts)
        Write-Host 'New Version: ' $version
        $content = $content -replace $pattern, "AssemblyVersion(`"$version`")"
        Set-Content $assemblyInfoFile -Value $content
    
        # Commit and push changes to Git
        #git add $assemblyInfoFile
        #git commit -m "Increment assembly version to $version"
        #git push
    
    }
}
