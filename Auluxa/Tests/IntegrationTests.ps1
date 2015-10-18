function assertAreEqual($expected, $actual) {
	if($expected -eq $actual) {
		Write-Host "PASSED" -foreground "green"
	} else {
		Write-Host ("expected: {0}, actual: {1}" -f $expected, $actual) -foreground "red"
	}
}

Invoke-RestMethod -Method Get -Uri http://localhost:9000/api/zones
Invoke-RestMethod -Method Get -Uri http://localhost:9000/api/zones?ids="3,1"

$zone ='
{
  "Name": "Kitchen",
}
'
Invoke-RestMethod -Method Post -Uri http://localhost:9000/api/zones/ -Body $zone -ContentType "application/json"
Invoke-RestMethod -Method Delete -Uri http://localhost:9000/api/zones/3 -ContentType "application/json"

Invoke-RestMethod -Method Put -Uri http://localhost:9000/api/zones/1/Appliances/1 -ContentType "application/json"
Invoke-RestMethod -Method Put -Uri http://localhost:9000/api/zones/1/Appliances/1,2 -ContentType "application/json"


#************************
function testAppliances() {
	$allAppliances = Invoke-RestMethod -Method Get -Uri http://localhost:9000/api/appliances
	$someAppliances = Invoke-RestMethod -Method Get -Uri http://localhost:9000/api/appliances?ids="1,2"
	
	$newAppliance = '
	{
		"Name": "Lockpad"
	}
	'
	$newAppliance = Invoke-RestMethod -Method Post -Uri http://localhost:9000/api/appliances/ -Body $newAppliance -ContentType "application/json"

	$updatedAppliance = '
	{
		"ApplianceId": "5",
		"Name": "Lockpad-Modified"
	}
	'
	$noDisplay = Invoke-RestMethod -Method Post -Uri http://localhost:9000/api/appliances/ -Body $updatedAppliance -ContentType "application/json"
	$updatedAppliance = Invoke-RestMethod -Method Get -Uri http://localhost:9000/api/appliances?ids="5"
	$noDisplay = Invoke-RestMethod -Method Delete -Uri http://localhost:9000/api/appliances/5

	$appliancesAfterDelete = Invoke-RestMethod -Method Get -Uri http://localhost:9000/api/appliances
	
	assertAreEqual 4 $allAppliances.count
	assertAreEqual 2 $someAppliances.count
	assertAreEqual 5 $newAppliance.applianceId
	assertAreEqual "Lockpad" $newAppliance.name
	assertAreEqual "Lockpad-Modified" $updatedAppliance.name
	assertAreEqual 4 $appliancesAfterDelete.count
}testAppliances

