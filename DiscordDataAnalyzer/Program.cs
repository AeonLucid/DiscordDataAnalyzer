using System;
using System.Linq;
using System.Threading.Tasks;
using DiscordDataAnalyzer.Parser;
using Serilog;

namespace DiscordDataAnalyzer
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Please run the application with \"DiscordDataAnalyzer.exe <Path to extracted package>\".");
                return;
            }
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            // Parse discord package.
            var package = new DiscordPackage(args[0]);

            Log.Information("Parsing package");
            await package.ParseAsync();
            
            // Statistics.
            Log.Information("Showing statistics");

            var rank = 1;
            
            foreach (var (_, channel) in package.Channels
                .Where(x => x.Value.IsPrivate)
                .OrderByDescending(x => x.Value.EventCount["send_message"])
                .Take(25))
            {
                Log.Information("{Rank,-2} {Channel,27}: {Messages}", rank++, channel, channel.EventCount["send_message"]);
                Log.Information("> First {Start:dd/MM/yyyy}", channel.MessageFirst);
                Log.Information("> Last  {Start:dd/MM/yyyy}", channel.MessageLast);
                Log.Information("> Messages per day {Count}", channel.EventCount["send_message"] / (channel.MessageLast - channel.MessageFirst)?.Days);
                Log.Information("> Popular hours");
                
                foreach (var value in channel.MessageHours
                    .OrderByDescending(x => x.Value)
                    .Take(4)
                    .Select(x => $"{x.Key:00}:00 ({x.Value})"))
                {
                    Log.Information("  {Hour}", value);
                }
            }
        }
    }
}