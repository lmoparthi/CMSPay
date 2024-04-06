param([string]$SolutionDir,  [string]$ProjectName,  [string]$ProjectDir,   [string]$TargetDir,  [string]$TargetPath, [string]$ConfigurationName) ;
Write-Host 'PowerShell Script: CustomerServicePlugInPostBuild.ps1'

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

$IncludeProjectDir = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.AccumulatorControl\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO

$IncludeProjectDir  = Join-Path -Path $SolutionDirParent -ChildPath "UFCW.CMS.CUSTOMERSERVICEControl\Bin\$ConfigurationName\"
Write-Host '$IncludeProjectDir: ' $IncludeProjectDir 
ROBOCOPY  ""$IncludeProjectDir\"" ""$TargetDir\"" '*rid.xml' /XO



