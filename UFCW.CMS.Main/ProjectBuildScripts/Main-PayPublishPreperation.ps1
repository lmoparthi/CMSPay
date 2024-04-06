param([string]$SolutionDir,  [string]$ProjectName,  [string]$ProjectDir,   [string]$TargetDir,  [string]$TargetPath, [string]$ConfigurationName) ;
Write-Host 'PowerShell Script: Main-PayPublishPreperation.ps1'

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

	# Load the XML content of the app.config
	[xml]$configXml = Get-Content $path 

	# Find the 'Mode' element using XPath
	$modeNode = $configXml.configuration.appSettings.add | Where-Object { $_.key -eq "Mode" }

	if ($modeNode -ne $null) { 
		# Change the 'value' attribute
		Write-Host 'Original Mode: ' $modeNode.value
		$modeNode.value = 'Pay'
		# Save the changes back to the app.config
		$configXml.Save($path)
	}

	Write-Host 'Final Mode: ' $modeNode.value

	# Find the 'MailToProd' element using XPath
	$MailToProdNode = $configXml.configuration.appSettings.add | Where-Object { $_.key -eq "MailToProd" }

	if ($MailToProdNode -ne $null) { 
		# Change the 'value' attribute
		Write-Host 'Original MailToProd: ' $MailToProdNode.value
		$MailToProdNode.value = 'MSTONE@scufcwfunds.com;LMoparthi@scufcwfunds.com'
		# Save the changes back to the app.config
		$configXml.Save($path)
	}

	Write-Host 'Final MailToProd: ' $MailToProdNode.value

	# Find the 'EnableDDTEKLogging' element using XPath
	$EnableDDTEKLoggingNode = $configXml.configuration.appSettings.add | Where-Object { $_.key -eq "EnableDDTEKLogging" }

	if ($EnableDDTEKLoggingNode -ne $null) { 
		# Change the 'value' attribute
		Write-Host 'Original EnableDDTEKLogging: ' $EnableDDTEKLoggingNode.value
		$EnableDDTEKLoggingNode.value = '0'
		# Save the changes back to the app.config
		$configXml.Save($path)
	}

	Write-Host 'Final EnableDDTEKLogging: ' $EnableDDTEKLoggingNode.value

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