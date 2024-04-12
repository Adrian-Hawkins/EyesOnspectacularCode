# C# levelup - Eyes On The Spectacular Code
### Eyes On The Spectacular Code is a set of developer tools which is accesible through our Discord chatbot or our web app.
### We combine external tools all into one convenient place for devs to access

## Some Features

###  Curl Conversion
- **Description:** Converts curl commands to a programming language of your choice.

### Date/Time Conversion
- **Description:** Converts date/times to from one format to another.

### YAML to JSON Converter
- **Description:** Converts YAML to JSON.

### HTML/JSON/XML Formatters
- **Description:** Formats unformatted HTML/JSON/XML files.

#### And much more...


## [Invite Discord Bot to your server](https://discord.com/oauth2/authorize?client_id=1223255852711153704&permissions=8&scope=bot)

## [Visit our website today](http://34.241.148.6/)


## To run the API
```cmd
cd EOSC.API
dtonet run
```

## To run the Bot
```cmd
cd EOSC.Bot
dtonet run
```

## To run the Web
```cmd
cd EOSC.Web
dtonet run
```
## [Alternatively make usage of visual studio 2022](https://visualstudio.microsoft.com/vs/community/)
## Secrets.json for EOSC.Common
```json
{
  "api": {
    "endpoint": "<YOUR_HOSTED_API>"
  },
  "database": {
    "connection": "<YOUR_DB_CONNECTION>"
  },
  "gpt": {
    "key": "<YOUR_GPT_KEY>"
  },
  "bot": {
    "token": "<YOUR_BOT_TOKEN>"
  }
}
```

## Secrets.json for EOSC.Bot
```json
{
  "Discord": {
    "Token": "<YOUR_DISCORD_TOKEN>"
  }
}
```

## Alternatively make use of Enviroment variables
```cmd
api:endpoint = "<YOUR_HOSTED_API>"
database:connection = "<YOUR_DB_CONNECTION>"
gpt:key = "<YOUR_GPT_KEY>"
bot:token = "<YOUR_BOT_TOKEN>"
Discord:Token = "<YOUR_DISCORD_TOKEN>"
```

## ERD
![image](https://github.com/Adrian-Hawkins/EyesOnspectacularCode/assets/159035044/0536a251-3cfb-449a-9965-feee92ca82c0)

