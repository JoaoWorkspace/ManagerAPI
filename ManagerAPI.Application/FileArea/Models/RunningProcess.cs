using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerAPI.Application.FileArea.Models
{
    public class RunningProcess
    {
        public bool HasExited { get; set; }
        public List<ProcessThread> Threads { get; set; }
        public ProcessModule MainModule { get; set; } 
        public ProcessModuleCollection Modules { get; set; }
        public string MainWindowTitle { get; set; }
        public TimeSpan PrivilegedProcessorTime { get; set; }
        public string ProcessName { get; set; }
        public bool Responding { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan TotalProcessorTime { get; set; }
        public TimeSpan UserProcessorTime { get; set; }
        public long VirtualMemorySize { get; set; }
        public long VirtualMemorySize64 { get; set; }
    }
}
