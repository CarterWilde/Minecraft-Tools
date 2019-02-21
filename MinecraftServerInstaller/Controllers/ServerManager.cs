using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

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
      process.StartInfo.ArgumentList.Add($"-Xmx{server.VMProperties.Xmx}M");
      process.StartInfo.ArgumentList.Add($"-Xms{server.VMProperties.Xms}M");
      process.StartInfo.ArgumentList.Add("-jar");
      process.StartInfo.WorkingDirectory = $"{Config.ConfigMeta.Path}/{server.Name}";
      process.StartInfo.ArgumentList.Add($"[{server.GameVersion}]server.jar");
      //process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.RedirectStandardInput = true;
      if (server.VMProperties.NoGui) {
        process.StartInfo.ArgumentList.Add($"nogui");
      }
      Thread thread = new Thread(() => {
        if (process.Start()) {
          //ServerOutputStreams.Add(server.Name, process.StandardOutput);
          ServerInputStreams.Add(server.Name, process.StandardInput);
        }
      }) {
        Name = server.Name
      };
      Console.WriteLine("Starting Thread[{0}]", thread.Name);
      ServerThreads.Add(thread.Name, thread);
      thread.Start();
    }

    public void KillAll() {
      foreach (Server server in Config.Servers) {
        Thread thread = new Thread(() => {
          Kill(server);
        }) {
          Name = $"Killer Thread[{server.Name}]"
        };
        Console.WriteLine("Starting killer thread " + thread.Name);
      }
    }

    public void Kill(Server server) {
      Console.WriteLine($"Killing thread[{server.Name}]");
      ServerInputStreams[server.Name].WriteLine("stop");
    }
  }
}
