using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

public class LogService : BackgroundService
{
    private static readonly string logFilePath = "Logs/log-.log";
    private static Timer _timer;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Periodicno preuzimanje logova svakih 10 minuta
        _timer = new Timer(AsyncProcessLogs, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));

        // Zaustavljanje kada je potrebno
        stoppingToken.Register(() => _timer.Dispose());

        return Task.CompletedTask;
    }

    private static async void AsyncProcessLogs(object state)
    {
        try
        {
            // Preuzimanje novih logova iz fajla (mozete koristiti FileSystemWatcher za real-time pracenje)
            string[] logs = File.ReadAllLines(logFilePath)
                                 .Where(log => log.Contains("ERROR") || log.Contains("WARNING"))  // Po potrebi filtrirajte logove
                                 .ToArray();

            if (logs.Any())
            {
                // Slanje logova na Elasticsearch
                foreach (var log in logs)
                {
                    // Logovanje u Elasticsearch
                    Log.Information(log);
                    Console.WriteLine($"Sent log to Elasticsearch: {log}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing logs: {ex.Message}");
        }
    }
}