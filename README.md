# Simple Mysql Manager
A very simple manager for MySQL databases. It's a bare-bones solution, with as few frills as possible

## Latest release
[Latest release](https://github.com/frankhaugen/simple-mysql-manager/releases/latest)

## NOTICE
This is early in development, and so this has very limited functionality, but the goal here is to have very simple functionality

## How to use
You can either manually enter data into the textboxes or you can place a json-file called "servers.json", next to the executable

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

## Screenshots
![Welcome notice](/Screenshots/screenshot_01.png)
![Full functionality](/Screenshots/screenshot_03.png)

## TODO
- Find how to handle databases with high number of tables
- Query-field and execution button, to write to server
- Functionality to edit database from UI
- Detailed error-handlig
- Clean code
- Comment and document code
- Add better error-handling