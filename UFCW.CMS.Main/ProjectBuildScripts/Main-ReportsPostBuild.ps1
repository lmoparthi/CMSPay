param([string]$SolutionDir,  [string]$ProjectName,  [string]$ProjectDir,   [string]$TargetDir,  [string]$TargetPath, [string]$ConfigurationName) ;
Write-Host 'PowerShell Script: Main-ReportsPostBuild.ps1'

Write-Host 'SolutionDir: ' $SolutionDir 
Write-Host 'ProjectName: ' $ProjectName
Write-Host 'ProjectDir: ' $ProjectDir 
Write-Host 'TargetDir: ' $TargetDir
Write-Host 'TargetPath: ' $TargetPath
Write-Host 'ConfigurationName: ' $ConfigurationName

$drive = Get-PSDrive -Name (get-location).Drive.Name
$Parent = if($drive.DisplayParent -ne $null){$drive.DisplayParent} else {$drive.Parent}

Write-Host 'Parent: ' $Parent
Write-Host 'drive: ' $drive
Write-Host 'drive.Parent: ' $drive.Parent
Write-Host 'drive.CurrentLocation: ' $drive.CurrentLocation

$SolutionDirParent =  (get-item $SolutionDir ).parent.FullName  
Write-Host 'SolutionDirParent: ' $SolutionDirParent

$ProjectDirParent =  (get-item $ProjectDir ).parent.FullName  
Write-Host 'ProjectDirParent: ' $ProjectDirParent

#Root is same as parent for this solution
$SolutionDirRoot =  (get-item $SolutionDir ).parent.parent.FullName  
Write-Host '$SolutionDirRoot: ' $SolutionDirRoot

$SolutionDirPackage =  Join-Path -Path $SolutionDirRoot  -ChildPath "VB.NET.Packages.Git"
Write-Host '$SolutionDirPackage: ' $SolutionDirPackage

if ($ConfigurationName -ne "Publish")
{

	$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.CUSTOMERSERVICE\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\""  '*rid.xml' /XO

	$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.MedicalReports.Plugin\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO

	$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.DentalReports.Plugin\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO

	$IncludeProjectDir  = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.CMS.AccumulatorOverride.Plugin\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
}

if ($ConfigurationName -eq "Release")
{
	$DependenciesDir = Join-Path -Path $SolutionDir -ChildPath "Dependencies\"
	Write-Host '$DependenciesDir: ' $DependenciesDir 

	$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.CUSTOMERSERVICE\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$DependenciesDir\"" 'UFCW.CMS.CUSTOMERSERVICE.EXE.CONFIG' '*rid.xml' /XO 

	ROBOCOPY  ""$TargetDir\"" ""$DependenciesDir\"" '*rid.xml' /XO
}

if ($ConfigurationName -eq "Publish")
{
	$DependenciesDir = Join-Path -Path $SolutionDir -ChildPath "Dependencies\"
	Write-Host '$DependenciesDir: ' $DependenciesDir 
	ROBOCOPY  ""$DependenciesDir\"" ""$TargetDir\"" '*rid.xml' /XO
}