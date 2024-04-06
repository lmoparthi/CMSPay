param([string]$SolutionDir,  [string]$ProjectName,  [string]$ProjectDir,   [string]$TargetDir,  [string]$TargetPath, [string]$ConfigurationName) ;
Write-Host 'PowerShell Script: UFCW.CMS.CustomerServiceControl.ps1'

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
Write-Host '$SolutionDirParent: ' $SolutionDirParent
$SolutionDirRoot =  (get-item $SolutionDir ).parent.parent.FullName  
Write-Host '$SolutionDirRoot: ' $SolutionDirRoot
$ProjectDirParent =  (get-item $ProjectDir ).parent.FullName  
Write-Host '$ProjectDirParent: ' $ProjectDirParent

#Get-ADPrincipalGroupMembership $env:USERNAME | select name,groupscope

if ($ConfigurationName -ne "Publish")
{
	#These are in the full build order.

$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.AlertManager\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.HistoryViewerControl\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.AnnotationControl\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
$IncludeProjectDir = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.AccumulatorControl\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.ClaimDocumentHistory\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.HRA\UFCW.CMS.HRAControl\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.PremiumsControl\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.UFCWDocsControl\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.MemberSearchControl\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.COBControl\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.ELIGIBILITYControl\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.FREETEXTControl\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.HoursControl\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.DentalControl\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.PrescriptionsControl\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.PROVIDER\PROVIDERCONTROL\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO

$IncludeProjectRoot = Join-Path -Path $SolutionDirRoot -ChildPath "UFCW.RegMaster\"
Write-Host '$IncludeProjectRoot: ' $IncludeProjectRoot 

$IncludeProjectDir = Join-Path -Path $IncludeProjectRoot -ChildPath "UFCW.REGMASTER.HistoryControl\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
$IncludeProjectDir = Join-Path -Path $IncludeProjectRoot -ChildPath "UFCW.REGMASTER.CoverageHistoryControl\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
$IncludeProjectDir = Join-Path -Path $IncludeProjectRoot -ChildPath "UFCW.REGMASTER.EligMaintenance\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
$IncludeProjectDir = Join-Path -Path $IncludeProjectRoot -ChildPath "UFCW.REGMASTER.RemarksControl\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO

}

#if ($ConfigurationName -eq "Release")
#{
#	$DependenciesDir = Join-Path -Path $SolutionDir -ChildPath "Dependencies\"
#	Write-Host '$DependenciesDir: ' $DependenciesDir 
#	ROBOCOPY  ""$TargetDir\"" ""$DependenciesDir\"" '*rid.xml' /XO
#}


