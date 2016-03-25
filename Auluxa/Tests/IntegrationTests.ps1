function assertAreEqual($expected, $actual) {
	if($expected -eq $actual) {
		Write-Host "PASSED" -foreground "green"
	} else {
		Write-Host ("expected: {0}, actual: {1}" -f $expected, $actual) -foreground "red"
	}
}


#************************
# ZONES
#************************
Invoke-RestMethod -Method Get -Uri http://localhost:57776/api/zones
Invoke-RestMethod -Method Get -Uri http://localhost:57776/api/zones?ids="3,1"

$zone ='
{
  "Name": "Kitchen",
}
'
Invoke-RestMethod -Method Post -Uri http://localhost:57776/api/zones/ -Body $zone -ContentType "application/json"
Invoke-RestMethod -Method Delete -Uri http://localhost:57776/api/zones/3 -ContentType "application/json"

#************************
# SETTINGS
#************************
Invoke-RestMethod -Method Get -Uri http://localhost:57776/api/settings

$settingUpdate ='
{
  "hoursFormat": "hh:mm:ss",
  "dateFormat": "yyyyMMdd",
  "timeZoneName": "Hong Kong",
  "timeZoneUtcOffset": 8,
}
'
$updatedSettings = Invoke-RestMethod -Method Patch -Uri http://localhost:57776/api/settings/ -Body $settingUpdate -ContentType "application/json"

#************************
# KRANIUM
#************************
Invoke-RestMethod -Method Get -Uri http://localhost:57776/api/kranium

$kraniumUpdate ='
{
  "name": "Two Kranium",
  "version": "2.0",
  "macAddress": "aa:bb:cc:dd:ee:ff",
  "iPAddress": "127.0.0.2",
  "zigBeePanId": "7",
  "zigBeeChannel": "8",
  "zigBeeMacAddress": "11:22:33:44:55:66",
}
'
$updatedKranium = Invoke-RestMethod -Method Patch -Uri http://localhost:57776/api/kranium/ -Body $kraniumUpdate -ContentType "application/json"

#************************
# APPLIANCES
#************************
function testAppliances() {
	$allAppliances = Invoke-RestMethod -Method Get -Uri http://localhost:57776/api/appliances
	$someAppliances = Invoke-RestMethod -Method Get -Uri http://localhost:57776/api/appliances?ids="1,2"
	
	$newAppliance = '
	{
		"Name": "Lockpad",
		"UserName": "TestUser"
	}
	'
	$newAppliance = Invoke-RestMethod -Method Post -Uri http://localhost:57776/api/appliances/ -Body $newAppliance -ContentType "application/json"

	$updatedAppliance = '
	{
		"ApplianceId": "5",
		"Name": "Lockpad-Modified",
		"UserName": "NewTestUser"
	}
	'
	$noDisplay = Invoke-RestMethod -Method Post -Uri http://localhost:57776/api/appliances/ -Body $updatedAppliance -ContentType "application/json"
	$updatedAppliance = Invoke-RestMethod -Method Get -Uri http://localhost:57776/api/appliances?ids="5"
	$noDisplay = Invoke-RestMethod -Method Delete -Uri http://localhost:57776/api/appliances/5

	$appliancesAfterDelete = Invoke-RestMethod -Method Get -Uri http://localhost:57776/api/appliances
	
	assertAreEqual 4 $allAppliances.count
	assertAreEqual 2 $someAppliances.count
	assertAreEqual 5 $newAppliance.applianceId
	assertAreEqual "Lockpad" $newAppliance.name
	assertAreEqual "Lockpad-Modified" $updatedAppliance.name
	assertAreEqual "NewTestUser" $updatedAppliance.UserName
	assertAreEqual 4 $appliancesAfterDelete.count
}testAppliances

