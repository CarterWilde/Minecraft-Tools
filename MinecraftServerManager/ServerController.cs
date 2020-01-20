using System;
using System.Linq;
using System.Collections;
using MinecraftServerManager.Models.ServerModels;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MinecraftServerManager {
  class ServerController {
    public ServerConfig Config { get; private set; }
    public IDictionary<string, Thread> ServerThreads { get; set; }
    public ServerController(ServerConfig config) {
      Config = config;
      ServerThreads = new Dictionary<string, Thread>();
      foreach(ServerManager server in Config.Servers) {
        server.StartInfo = Models.ModelSerializer.PropertiesToStartInfo(server, Config);
        Thread serverThread = new Thread(async () => {
          await server.Start();
          server.initStreams();
        });
        ServerThreads.Add(server.Name, serverThread);
      }
    }

    public void StopAll() {
      Parallel.ForEach(Config.Servers, async server => {
        await server.Stop();
      });
    }
  }
}
