Invoke-RestMethod -Method Get -Uri http://localhost:9000/api/zones
Invoke-RestMethod -Method Get -Uri http://localhost:9000/api/zones?ids=3,1

$zone ='
{
  "Name": "Kitchen",
}
'
Invoke-RestMethod -Method Post -Uri http://localhost:9000/api/zones/ -Body $zone -ContentType "application/json"

Invoke-RestMethod -Method Delete -Uri http://localhost:9000/api/zones/3 -ContentType "application/json"

Invoke-RestMethod -Method Put -Uri http://localhost:9000/api/zones/3/Appliances/1 -ContentType "application/json"
