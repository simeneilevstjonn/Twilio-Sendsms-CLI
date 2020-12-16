# Twilio-Sendsms-CLI
A command line interface for Twilio's Programmable SMS API.

A quick and easy way to send SMS from the command line using wilio's Programmable SMS API.

## Installation

1. Build the program for your os using the `dotnet build` command.
2. Place the compiled program where you want it, I reccomend adding it to PATH.
3. Create the config file. The program needs your Twilio API credentials in order to send messages. These are stored in a JSON file. If you use Linux, it will should be at `/etc/sendsms/config.json`, for Windows `C:\Program Files\sendsms\config.json`. The user which is to run the program, needs read access to this file. The file should be in the following format:
```json
{
    "AccountSID": "INSERT Account SID",
    "AuthToken": "INSERT AUTH TOKEN",
    "ServiceSID": "INSERT MESSAGING SERVICE SID"
}
```

## Usage

You can use this command in two ways.

Either as one command:
`sendsms RECIPIENT SENDER MESSAGE`
Such as
`sendsms +441134960000 Robot 'Hello world'`

The other way is to just write `sendsms`, and fill in the information as the program asks for it.
```
sendsms
To: +441134960000
From: Robot
Message: Hello, World
```

If you want to schedule SMS', you should use the `at` command in unix.
For example:
`echo "sendsms +441134960000 Robot 'Hello world'" | at 1200 jan 1`

The program will print the status code from Twilio's API, if the message was successfully sent, it will print `Status: accepted`.


