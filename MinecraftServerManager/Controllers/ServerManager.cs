using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MinecraftServerInstaller.Controllers {
  class ServerManager {
    public ServerConfig Config { get; private set; }
    //The key is the servers ThreadName the threadname should be the server name 
    //and the thread is the thread of that server
    private IDictionary<string, Thread> ServerThreads { get; set; }
    //The key is the servers ThreadName the thread name should be the server name 
    //and the thread is the Output of the server
    private IDictionary<string, StreamReader> ServerOutputStreams { get; set; }
    //The key is the servers ThreadName the threadname should be the server name 
    //and the thread is the Input of the server
    private IDictionary<string, StreamWriter> ServerInputStreams { get; set; }

    //Creates a ServerManager with a ServerConfig and makes it the current instance
    public ServerManager(ServerConfig config) {
      Config = config;
      ServerThreads = new Dictionary<string, Thread>();
      ServerOutputStreams = new Dictionary<string, StreamReader>();
      ServerInputStreams = new Dictionary<string, StreamWriter>();
    }

    //Creates a ServerManager with a config file path to make a ServerConfig with
    //that files, and stores the ServerConfig from that.
    public ServerManager(string ConfigFilePath) {
      Config = ServerBuilder.DeserializeConfig(ConfigFilePath);
      ServerThreads = new Dictionary<string, Thread>();
      ServerOutputStreams = new Dictionary<string, StreamReader>();
      ServerInputStreams = new Dictionary<string, StreamWriter>();
    }

    public void RunAll() {
      foreach (Server server in Config.Servers) {
        Run(server);
      }
    }

    public void Run(Server server) {
      Process process = new Process();
      process.StartInfo.FileName = $"java";
      bool isGui = true;
      Parallel.ForEach(server.VMProperties.Keys, (key) => {
        if (key == "nogui") {
          isGui = true;
          return;
        }
        process.StartInfo.ArgumentList.Add(ToArgString(key, server.VMProperties[key]));
      });
      process.StartInfo.WorkingDirectory = $"{Config.ConfigMeta.Path}/{server.Name}";
      process.StartInfo.ArgumentList.Add($"[{server.GameVersion}]server.jar");
      if (isGui) {
        process.StartInfo.ArgumentList.Add("nogui");
      }
      //process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.RedirectStandardInput = true;
      Thread thread = new Thread(() => {
        if (process.Start()) {
          //ServerOutputStreams.Add(server.Name, process.StandardOutput);
          ServerInputStreams.Add(server.Name, process.StandardInput);
        }
      }) {
        Name = server.Name
      };
      Console.WriteLine("StartingThread[{0}]", thread.Name);
      ServerThreads.Add(thread.Name, thread);
      thread.Start();
    }

    public static string ToArgString(string key, string value) {
      if (key == "nogui") {
        return String.Empty;
      } else if (value == "true" || value == "false") {
        if (value == "false") {
          return String.Empty;
        }
        if (value == "true") {
          return "-" + key;
        }
      }
      return "-" + key + value;
    }

    public void KillAll() {
      foreach (Server server in Config.Servers) {
        Thread thread = new Thread(() => {
          Kill(server);
        }) {
          Name = $"KillerThread[{server.Name}]"
        };
        Console.WriteLine("Starting Killer Thread " + thread.Name);
      }
    }

    public void Kill(Server server) {
      Console.WriteLine($"KillingThread[{server.Name}]");
      ServerInputStreams[server.Name].WriteLine("stop");
    }
  }
}
