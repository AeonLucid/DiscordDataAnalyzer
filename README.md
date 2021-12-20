# DiscordDataAnalyzer [![GitHub Workflow Status](https://img.shields.io/github/workflow/status/AeonLucid/DiscordDataAnalyzer/Build%20and%20Publish)](https://github.com/AeonLucid/DiscordDataAnalyzer/actions)

> This is a small application I wrote for myself, but made public in case others found it interesting.  
> Which means this repository will not be actively maintained.

Small C# console application that parses the Discord data package. 

For more information about the package, view the Discord help articles:
- [Your Discord Data Package](https://support.discord.com/hc/en-us/articles/360004957991-Your-Discord-Data-Package)
- [Requesting a Copy of your Data](https://support.discord.com/hc/en-us/articles/360004027692-Requesting-a-Copy-of-your-Data)

## What does it do?

Currently it parses the Discord event reporting data, which includes mostly messages you sent in a channel. Which can be in a server or in private messages. This data also includes events for messages which are manually removed, but not their content.

It shows:

- Top 25 most messaged users
  - Total amount of messages
  - Timestamp of first message
  - Timestamp of last message
  - Messages per day
  - The 4 most popular hours at which messages were sent (Disabled by default)
- Top devices active in the last 14 days

If you know C#, you can easily tweak the above to your liking in the [Program.cs](DiscordDataAnalyzer/Program.cs) file.

## Usage

This is written with [.NET 6](https://dotnet.microsoft.com/), which means it works on Windows, Mac OSX and Linux.  
Run with one argument specifying the location of the extracted Discord data package.

```
DiscordDataAnalyzer.exe <Path to extracted package>
```
