using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

public class LogService : BackgroundService
{
    private static long lastReadPosition = 0; // offset
    private static readonly string logsDirectoryPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Logs");
    private static Timer _timer;
    private static readonly HttpClient _httpClient = new HttpClient();

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(AsyncProcessLogs, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

        stoppingToken.Register(() => _timer.Dispose());

        return Task.CompletedTask;
    }

    private static string FindLogsDirectory()
    {
        if (!Directory.Exists(logsDirectoryPath))
        {
            Console.WriteLine($"Logs directory not found: {logsDirectoryPath}");
            return null;
        }

        return logsDirectoryPath;
    }

    private static string GetLatestLogFilePath()
    {
        string logsDirectory = FindLogsDirectory();
        if (logsDirectory == null)
        {
            return null;
        }

        var latestLogFile = Directory.GetFiles(logsDirectory, "log-*.log")
                                     .OrderByDescending(f => f)
                                     .FirstOrDefault();

        if (latestLogFile == null)
        {
            Console.WriteLine("No log files found.");
            return null;
        }

        return latestLogFile;
    }

    private static async void AsyncProcessLogs(object state)
    {
        try
        {
            var logFilePath = GetLatestLogFilePath();
            if (logFilePath == null)
            {
                return;
            }

            using (var stream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                stream.Seek(lastReadPosition, SeekOrigin.Begin);

                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        await SendLogToElasticsearch(line);
                    }

                    lastReadPosition = stream.Position;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing logs: {ex.Message}");
        }
    }

    private static async Task SendLogToElasticsearch(string logLine)
    {
        try
        {
            var logEntry = ParseLogLine(logLine);

            if (logEntry == null)
            {
                Console.WriteLine($"Skipping invalid log line: {logLine}");
                return;
            }

            var (timestamp, level, message) = logEntry.Value;

            var jsonLog = new
            {
                timestamp = timestamp.ToString("o"), // ISO8601 date format
                level = level,
                message = message
            };

            var jsonContent = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(jsonLog),
                Encoding.UTF8,
                "application/json"
            );

            // Elasticsearch endpoint
            var elasticsearchUri = "http://localhost:9200/logstash-logs/_doc";

            var response = await _httpClient.PostAsync(elasticsearchUri, jsonContent);

            if (!response.IsSuccessStatusCode)
            { 
                Console.WriteLine($"Failed to send log to Elasticsearch. Status code: {response.StatusCode}");
                var responseBody = await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send log to Elasticsearch: {ex.Message}");
        }
    }

    private static (DateTime Timestamp, string Level, string Message)? ParseLogLine(string logLine)
    {
        try
        {
            var parts = logLine.Split(new[] { ' ' }, 5, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 5)
                return null;

            var timestamp = DateTime.Parse($"{parts[0]} {parts[1]}");

            var level = parts[3].Trim('[', ']');

            var message = parts[4];

            return (timestamp, level, message);
        }
        catch
        {
            return null;
        }
    }
}