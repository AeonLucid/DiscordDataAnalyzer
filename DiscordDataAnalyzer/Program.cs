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
            Log.Information("Showing message stats");

            var rank = 1;
            
            foreach (var (_, channel) in package.Channels
                .Where(x => x.Value.IsPrivate)
                .OrderByDescending(x => x.Value.EventCount["send_message"])
                .Take(25))
            {
                Log.Information("{Rank,-2} {Channel,27}: {Messages}", rank++, channel, channel.EventCount["send_message"]);
                Log.Information("> First {Start:dd/MM/yyyy}", channel.MessageFirst);
                Log.Information("> Last  {Start:dd/MM/yyyy}", channel.MessageLast);

                var daysBetween = (channel.MessageLast - channel.MessageFirst)?.Days;
                if (daysBetween > 0)
                {
                    Log.Information("> Messages per day {Count}", channel.EventCount["send_message"] / daysBetween);
                }
                
                // Log.Information("> Popular hours");
                //
                // foreach (var value in channel.MessageHours
                //     .OrderByDescending(x => x.Value)
                //     .Take(4)
                //     .Select(x => $"{x.Key:00}:00 ({x.Value})"))
                // {
                //     Log.Information("  {Hour}", value);
                // }
            }

            var dateMax = package.Devices.Max(x => x.Timestamp);
            var dateMin = dateMax.Subtract(TimeSpan.FromDays(14));

            Log.Information("Showing device stats from {From,-12} - {To,-12}", 
                dateMin.ToString("yyyy/MM/dd"), 
                dateMax.ToString("yyyy/MM/dd"));
            
            foreach (var devices in package.Devices
                         .Where(x => x.Timestamp >= dateMin && x.Timestamp <= dateMax)
                         .OrderBy(x => x.Timestamp)
                         .GroupBy(x => x.Key()))
            {
                var entries = devices.ToList();
                var entry = entries[0];

                var accessFirst = entries.Min(x => x.Timestamp);
                var accessLast = entries.Max(x => x.Timestamp);
                
                Log.Information("{Amount,-8} {Browser,-16} {Device,-32} {Ip,-16} {First,-12} {Last,-12}",
                    entries.Count,
                    entry.Browser,
                    entry.Device,
                    entry.Ip,
                    accessFirst.ToString("yyyy/MM/dd"), 
                    accessLast.ToString("yyyy/MM/dd"));
            }
        }
    }
}