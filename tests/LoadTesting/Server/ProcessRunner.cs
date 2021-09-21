using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace LoadTesting.Server
{
    class ProcessRunner : IDisposable
    {
        private readonly Process _process;
        private bool disposedValue;

        public ProcessRunner(string command, string argsuments)
        {
            _process = Process.Start(new ProcessStartInfo()
            {
                WorkingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                FileName = command,
                Arguments = argsuments,
            });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _process.Kill();
                    _process.WaitForExit();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}