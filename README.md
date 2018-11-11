# Simple Mysql Manager
A very simple manager for MySQL databases. It's a bare-bones solution, with as few frills as possible

## How to use
You can either manually enter data into the textboxes or you can place a json-file called "servers.json", next to the 



### servers.json
This is how the configuration file "servers.json" must be formatted, (you do not need multiple items in the array)
```json
{
	"servers": [
		{
			"address": "mysql01.somedomain.ms",
			"friendlyname": "My personal database",
			"database": "jdhomedb",
			"username": "joannadoe",
			"password": "qwerty12345"
		},
		{
			"address": "mysql02.somedomain.ms",
			"friendlyname": "Accounting Firm LLC",
			"database": "accounfirm",
			"username": "joannadoe",
			"password": "qwerty12345"
		},
		{
			"address": "mysql03.somedomain.ms",
			"friendlyname": "Testing Database",
			"database": "devdb",
			"username": "joannadoe",
			"password": "qwerty12345"
		}
	]
}
```

## TODO
- Query-field and execution button, to write to server
- Functionality to edit database from UI
- Detailed error-handlig
- Clean code
- Comment and document code
- Add 