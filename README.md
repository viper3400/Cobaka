# Cobaka

Cobaka "monitors" your dogs noise (especially the barking) when you're not at home. It's a simple noise detection tool for Windows. It uses the **standard audio capture device** to listen to room.

If a special treshold is reached it starts a new recording (wav) for some seconds and saves it to a file. You could adjust the **treshold** (0.01 - 0.99), the **duration of each record** (in seconds) and the **destination directory**.

Choose **Continue Record When Over Treshold** ceckbox to record a continuous file while noise level is over the treshold. Record will stop when level is under the treshold for the time specified for duration.

Use "Start/Stop" buttons to enable/disable listening mode.

![AppWindow](https://github.com/viper3400/Cobaka/blob/master/CobakaMainWindow.png)

## Download

https://github.com/viper3400/Cobaka/releases/latest

## Settings

Settings are stored in "%LOCALAPPDATA%\Cobaka\NoiseDetectorConfig.json" whenever they are changed. Use "Reset Defaults" button to delete the settings file and reset to default values.

### Troubleshooting after Update to Latest Version

After update Cobaka to a newer version it may be necessary to delete "%LOCALAPPDATA%\Cobaka\NoiseDetectorConfig.json" as there is no update mechanism for settings in place. Exceptions may occur if settings properties are new, changed or deleted in a new Cobaka version.

## Build
* This is a .NET Core 3 app (starting with tag 1.5.0)
* Clone solution and cd into directory
* run ```dotnet restore```
* run ```dotnet build```
* to publish as self-contained and trimmed Windows 10 application for run ```dotnet publish -c Release -r win10-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true /p:Version=x.x.x.x``` (x => version)

## 3rd Party Credits
* https://github.com/naudio/NAudio
* https://www.newtonsoft.com/json
