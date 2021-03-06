﻿using MinecraftClientInstaller;
using MinecraftServerManager.Models;
using MinecraftServerManager.Models.ServerModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MinecraftServerManager {
  public class ServerBuilder {
    public event EventHandler<EventArgs> BuildComplete;
    private ServerConfig Config { get; }
    public ServerBuilder(ServerConfig config) {
      Config = config;
    }
    public void Build() {
      List<List<ServerManager>> serverGroups = (from server in Config.Servers
                                                group server by server.ServerURL
                                                into newGroup select newGroup)
                                               .Select(grp => grp.ToList()).ToList();
      
      IList<Server> downloadList = new List<Server>();
      foreach(List<ServerManager> group in serverGroups) {
        downloadList.Add(group.First());
      }
      Parallel.ForEach(downloadList, server => {
        Build(server).Wait();
      });

      int i = 0;
      Parallel.ForEach(serverGroups, group => {
        Parallel.ForEach(group, server => {
          if(!server.Name.Equals(downloadList[i].Name)) {
            Build(server, downloadList[i]).Wait();
          }
        });
        i++;
      });
      OnBuildComplete(new EventArgs());
    }

    protected virtual void OnBuildComplete(EventArgs args) {
      BuildComplete?.Invoke(this, args);
    }

    public async Task Build(Server server) {
      string path = Path.Combine(Config.Path, server.Name);
      Directory.CreateDirectory(path);
      await DownloadServerBinarys(server);
      await CreateEULA(server);
      await CreateProperites(server);
      if(server.Modpack != null) {
        DownloadManager manager = new DownloadManager(Path.Combine(path, "mods"), server.GameVersion);
        manager.DownloadMods(server.Modpack.Joint);
        manager.DownloadMods(server.Modpack.Server);
      }
    }

    public async Task Build(Server server, Server downloadServer) {
      Directory.CreateDirectory(Path.Combine(Config.Path, server.Name));
      string from = Path.Combine(Path.Combine(Config.Path, downloadServer.Name), $"[{downloadServer.GameVersion}]server.jar"); ;
      string destination = Path.Combine(Path.Combine(Config.Path, server.Name), $"[{server.GameVersion}]server.jar");
      Console.WriteLine($"Copying server binary for {server.Name} from {downloadServer.Name} [{from}]...");
      File.Copy(from, destination, true);
      Console.WriteLine($"Finished copying server binary for {server.Name} from {downloadServer.Name} [{from}]!");
      await Task.CompletedTask;
    }

    //Download server binary
    public async Task DownloadServerBinarys(Server server) {
      Console.WriteLine($"Downloading server binary for {server.Name}...");
      using(WebClient client = new WebClient()) {
        string DownloadPath = Path.Combine(Path.Combine(Config.Path, server.Name), $"[{server.GameVersion}]server.jar");
        await client.DownloadFileTaskAsync(new Uri(server.ServerURL), DownloadPath);
      }
      Console.WriteLine($"Finished downloading server binarys for {server.Name}!");
    }

    //Create EULA file and set it to true by default
    public async Task CreateEULA(Server server) {
      using(FileStream stream = new FileStream(Config.Path + "/" + server.Name + "/eula.txt", FileMode.Create, FileAccess.ReadWrite)) {
        using(StreamWriter writer = new StreamWriter(stream)) {
          await writer.WriteAsync(Config.EULA);
        }
      }
    }

    //Create server.properties file
    public async Task CreateProperites(Server server) {
      using(FileStream stream = new FileStream(Config.Path + "/" + server.Name + "/server.properties", FileMode.Create, FileAccess.ReadWrite)) {
        using(StreamWriter writer = new StreamWriter(stream)) {
          foreach(string key in server.Properties.Keys) {
            await writer.WriteAsync(ModelSerializer.ToPropertiesString(key, server.Properties[key]) + Environment.NewLine);
          }
        }
      }
    }
  }
}
