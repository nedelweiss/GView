## Task Description
The main idea is if the user take a screenshot of any chose by own Game/Application, taken screenshot will send to Discord channel.

If you don't use GUI you should have to set ```BOT_TOKEN```, ```CHANNEL_ID```, ```GAME_TITLE``` and ```SERVER_ID``` environment variables.
- Where can you get the ```BotToken```? Follow the next link:
```https://docs.discordnet.dev/guides/getting_started/first-bot.html```
- Take ```ServerId``` and ```ChannelId``` from your Discord Server.
- Take the game title from the top left corner of the game window

But if you use GUI you should have to enter Game Title, Channel Id, Server Id to TextBoxes. 
The program will automatically update the data inside after typing.
Also, you need to set BOT_TOKEN to windows envs. See 3 options above.

## Tech stack
- C# 12.0
- .Net 8.0
- WPF
- Discord API, Discord.Net
- FileSystemWatcher
- Nito.AsyncEx (A helper library for the Task-Based Asynchronous Pattern (TAP)); ```https://github.com/StephenCleary/AsyncEx```

## Build, Run & Exe file creation
JetBrains Rider IDE was used for developing so, just ```open``` a Solution (GView.sln) and press ```Run```.
To build exe file you need to click right on your Solution & select ```Publish``` option.
Click it, choose ```Local Folder``` and then select a target runtime as ```win-x64```, deployment mode as ```Self-Contained(?)```
and there are checkboxes:
1. produce single file
2. include native libraries for self extract(?)
3. enable ReadyToRun compilation(?)

(check these options) and click run.

## C# Events
- ```https://stackoverflow.com/questions/623451/how-can-i-make-my-own-event-in-c```
- ```https://habr.com/ru/companies/fuse8/articles/809467/```

## Useful links
- ```https://discord.com/developers/docs/resources/application```
- ```https://discord.com/developers/applications```
- ```https://learn.microsoft.com/en-us/archive/blogs/toub/low-level-keyboard-hook-in-c```
- ```https://stackoverflow.com/questions/39702704/connecting-uwp-apps-hosted-by-applicationframehost-to-their-real-processes```
- ```https://stackoverflow.com/questions/21103669/how-do-i-create-an-event-loop-message-pipe-in-a-console-application```
- ```https://stackoverflow.com/questions/604410/global-keyboard-capture-in-c-sharp-application```
- ```https://stackoverflow.com/questions/7268302/get-the-titles-of-all-open-windows```
- ```https://stackoverflow.com/questions/277085/how-do-i-getmodulefilename-if-i-only-have-a-window-handle-hwnd```
- ```https://stackoverflow.com/questions/76481119/how-to-build-an-exe-in-jetbrains-rider```
- ```https://stackoverflow.com/questions/21447223/selecting-random-nodes-from-xml-file```
- ```https://learn.microsoft.com/en-us/dotnet/standard/threading/timers```