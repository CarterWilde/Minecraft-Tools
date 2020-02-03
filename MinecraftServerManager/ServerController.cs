using System;
using System.Linq;
using System.Collections;
using MinecraftServerManager.Models.ServerModels;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using MinecraftServerManager.Models;

namespace MinecraftServerManager {
  public class ServerController {
    public event EventHandler<EventArgs> ServersStarted;

    public ServerConfig Config { get; private set; }
    private IDictionary<string, Thread> ServerThreads { get; set; }
    private ServerBuilder Builder { get; set; }
    public ServerController(ServerConfig config) {
      Config = config;
      Builder = new ServerBuilder(Config);
      ServerThreads = new Dictionary<string, Thread>();
      foreach(ServerManager server in Config.Servers) {
        server.StartInfo = Models.ModelSerializer.VMPropertiesToStartInfo(server, Config);
      }
    }

    public async void CreateServer(Server server) {
      await Builder.Build(server);
      Config.Servers.Add(new ServerManager(server, Config.Path, Config));
    }

    public ServerManager ReadServer(string name) {
      return (from server in Config.Servers where server.Name == name select server).First();
    }

    public void UpdateServer(string name, Server server) {
      ServerManager selected = (from s in Config.Servers where s.Name == name select s).First();
      selected.UpdateProps(server);
    }

    public async Task<Server> DeleteServer(string name) {
      ServerManager server = (from s in Config.Servers where s.Name == name select s).First();
      await server.Stop();
      Config.Servers.Remove(server);
      ServerThreads[server.Name].Abort();
      ServerThreads.Remove(server.Name);
      return server;
    }

    public async Task<string> ServerLog(string name) {
      ServerManager server = (from s in Config.Servers where s.Name == name select s).First();
      return await server.GetLogAsync();
    }

    public void StartAll() {
      Console.WriteLine("Starting servers!");
      foreach(ServerManager server in Config.Servers) {
        Thread serverThread = new Thread(async () => {
          await server.StartAsync(Config.Path, Config);
        });
        serverThread.Start();
        ServerThreads.Add(server.Name, serverThread);
      }
      OnServersStarted(new EventArgs());
    }

    protected virtual void OnServersStarted(EventArgs eventArgs) {
      ServersStarted?.Invoke(this, eventArgs);
    }

    public void StopAll() {
      Parallel.ForEach(Config.Servers, async server => {
        await server.Stop();
      });
      Parallel.ForEach(ServerThreads, thread => {
        thread.Value.Abort();
        ServerThreads.Remove(thread.Key);
      });
    }
  }
}
