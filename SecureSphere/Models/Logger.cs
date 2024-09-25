namespace SecureSphere.Models
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    // Include your DbContext and SystemLog model

    public static class Logger
    {
        private static readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "system_logs.txt");

        static Logger()
        {
            // Create log directory if it doesn't exist
            var logDirectory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        // Method to log to both file and database
        public static async Task LogAsync(string message, ApplicationDbContext dbContext)
        {
            try
            {
                // Log to file
                LogToFile(message);

                // Log to database
                await LogToDatabaseAsync(message, dbContext);
            }
            catch (Exception ex)
            {
                // Handle logging error (write to fallback or notify system admin)
                Console.WriteLine($"Logging failed: {ex.Message}");
            }
        }

        // Method to log to file
        private static void LogToFile(string message)
        {
            using (StreamWriter sw = new StreamWriter(logFilePath, true))
            {
                sw.WriteLine($"{DateTime.Now}: {message}");
            }
        }

        // Method to log to database
        private static async Task LogToDatabaseAsync(string message, ApplicationDbContext dbContext)
        {
            var logEntry = new SystemLog
            {
                Details = message
            };

            dbContext.SystemLogs.Add(logEntry);
            await dbContext.SaveChangesAsync();
        }
    }

}
