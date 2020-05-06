using System;
using System.IO;

namespace Kros.Services
{
    public class ApplicationStorage : IApplicationStorage
    {
        public static ApplicationStorage Instance { get; } = new ApplicationStorage("GG", "Kros");
        
        private ApplicationStorage(string group, string app)
        {
            #if DEBUG
            app += "-dev";
            #endif
            
            BaseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), group, app);
            Directory.CreateDirectory(BaseDirectory);

            LogDirectory = Path.Combine(BaseDirectory, "logs");
            Directory.CreateDirectory(LogDirectory);
        }

        public string BaseDirectory { get; }
        
        public string LogDirectory { get; }
    }
}