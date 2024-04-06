param([string]$SolutionDir,  [string]$ProjectName,  [string]$ProjectDir,   [string]$TargetDir,  [string]$TargetPath, [string]$ConfigurationName) ;
Write-Host 'PowerShell Script: SetConfig2DB2P.ps1'

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


if ($ConfigurationName -eq "Publish")
	{
	$path = -join($ProjectDir.Trim("") ,"app.config")

	write-host 'Path: ' $path

	$xml = (Select-Xml -Path $path -XPath / ).Node
	$element = $xml.SelectSingleNode("*/dataConfiguration")
	Write-Host 'Original defaultDatabase: ' $element.defaultDatabase

	if ($element.defaultDatabase -ne "DDTek P Connection String")
	{
		Get-Item -Path $path | Set-ItemProperty -Name IsReadOnly -Value $false
		$element.defaultDatabase = 'DDTek P Connection String'
		$xml.save($path)
		$element = $xml.SelectSingleNode("*/dataConfiguration")
	}

	Write-Host 'Final defaultDatabase: ' $element.defaultDatabase

}