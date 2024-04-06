param([string]$SolutionDir,  [string]$ProjectName,  [string]$ProjectDir,   [string]$TargetDir,  [string]$TargetPath, [string]$ConfigurationName) ;

Write-Host '* - PowerShell Script: UFCW.CMS.CustomerService.ps1'

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

$SolutionDirPackage =  Join-Path -Path $SolutionDirRoot  -ChildPath "VB.NET.Packages.Git"
Write-Host '$SolutionDirPackage: ' $SolutionDirPackage


$ProjectDirParent =  (get-item $ProjectDir ).parent.FullName  
Write-Host '$ProjectDirParent: ' $ProjectDirParent
Write-Host 

#Get-ADPrincipalGroupMembership $env:USERNAME | select name,groupscope

if ($ConfigurationName -ne "Publish")
{
	#These are in the full build order.

	$IncludeProjectDir  = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.CMS.AlertManager\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
	$IncludeProjectDir  = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.CMS.HistoryViewerControl\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
	$IncludeProjectDir  = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.CMS.AnnotationControl\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
	$IncludeProjectDir = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.CMS.AccumulatorControl\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
	$IncludeProjectDir  = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.CMS.ClaimDocumentHistory\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
	$IncludeProjectDir  = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.CMS.HRAControl\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
	$IncludeProjectDir  = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.CMS.PremiumsControl\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
	$IncludeProjectDir  = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.CMS.UFCWDocsControl\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
	$IncludeProjectDir  = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.CMS.MemberSearchControl\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
	$IncludeProjectDir  = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.CMS.COBControl\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
	$IncludeProjectDir  = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.CMS.ELIGIBILITYControl\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
	$IncludeProjectDir  = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.CMS.FREETEXTControl\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
	$IncludeProjectDir  = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.CMS.HoursControl\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
	$IncludeProjectDir  = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.CMS.DentalControl\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
	$IncludeProjectDir  = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.CMS.PrescriptionsControl\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
	$IncludeProjectDir  = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.CMS.PROVIDERCONTROL\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
	$IncludeProjectDir = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.REGMASTER.HistoryControl\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
	$IncludeProjectDir = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.REGMASTER.CoverageHistoryControl\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
	$IncludeProjectDir = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.REGMASTER.EligMaintenance\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
	$IncludeProjectDir = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.REGMASTER.RemarksControl\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO
		$IncludeProjectDir  = Join-Path -Path $SolutionDirPackage -ChildPath "UFCW.CMS.CUSTOMERSERVICEControl\Bin\$ConfigurationName\"
	Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
	ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO

}

if ($ConfigurationName -eq "Release")
{
	$DependenciesDir = Join-Path -Path $SolutionDir -ChildPath "Dependencies\"
	Write-Host '$DependenciesDir: ' $DependenciesDir 
	ROBOCOPY  ""$TargetDir\"" ""$DependenciesDir\"" '*rid.xml' /XO
}

if ($ConfigurationName -eq "Publish")
{
	$DependenciesDir = Join-Path -Path $SolutionDir -ChildPath "Dependencies\"
	Write-Host '$DependenciesDir: ' $DependenciesDir 
	ROBOCOPY  ""$DependenciesDir\"" ""$TargetDir\"" '*rid.xml' /XO
}
