# Cobaka

Cobaka "monitors" your dogs noise (especially the barking) when you're not at home. It's a simple noise detection tool for Windows. It uses the **standard audio capture device** to listen to room.

If a special treshold is reached it starts a new recording (wav) for some seconds and saves it to a file. You could adjust the **treshold** (0.01 - 0.99), the **duration of each record** (in seconds) and the **destination directory**.

Choose **Continue Record When Over Treshold** ceckbox to record a continuous file while noise level is over the treshold. Record will stop when level is under the treshold for the time specified for duration.

Use "Start/Stop" buttons to enable/disable listening mode.

![AppWindow](https://github.com/viper3400/Cobaka/blob/master/CobakaMainWindow.png)

## Settings

Settings are stored in "%LOCALAPPDATA%\Cobaka\NoiseDetectorConfig.json" whenever they are changed. Use "Reset Defaults" button to delete the settings file and reset to default values.
