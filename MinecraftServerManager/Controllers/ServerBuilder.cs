using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MinecraftServerInstaller.Controllers {
  public class ServerBuilder {
    public ServerConfig Config { get; private set; }

    private IList<Thread> ServerBuilderThreads;

    //Creates a ServerBuilder with a ServerConfig and makes it the current instance
    public ServerBuilder(ServerConfig config) {
      Config = config;
      ServerBuilderThreads = new List<Thread>();
    }

    //Creates a ServerBuilder with a config file path to make a ServerConfig with
    //that files, and stores the ServerConfig from that.
    public ServerBuilder(string ConfigFilePath) {
      Config = DeserializeConfig(ConfigFilePath);
      ServerBuilderThreads = new List<Thread>();
    }

    //Takes nothing and builds from the protected config
    public void BuildAll() {
      foreach (Server server in Config.Servers) {
        Thread thread = new Thread(() => {
          Build(server);
        }) {
          Name = $"Build Thread[{server.Name}]"
        };
        Console.WriteLine("Starting " + thread.Name);
        ServerBuilderThreads.Add(thread);
        thread.Start();
      }
    }

    //Takes in a config and builds from that
    public static void BuildAll(ServerConfig config) {
      throw new NotImplementedException();
    }

    //Takes in a server and builds a server with it's config
    public void Build(Server server) {
      Directory.CreateDirectory(Config.ConfigMeta.Path + "/" + server.Name);
      Directory.CreateDirectory(Config.ConfigMeta.BackupsPath + "/" + server.Name);
      Directory.CreateDirectory(Config.ConfigMeta.Path + "/" + server.Name + $"/ManagerLog");
      DownloadServerBinarys(server, Config.ConfigMeta);
      CreateEULA(server, Config.ConfigMeta);
      BuildServerProperties(server, Config.ConfigMeta);
      Console.WriteLine("Done building server " + server.Name);
    }

    //Create the server.properties file
    public static void BuildServerProperties(Server server, ServerConfigMeta meta) {
      Console.WriteLine("Creating server.properties for server {0}", server.Name);
      using (FileStream stream = new FileStream(meta.Path + "/" + server.Name + "/server.properties", FileMode.Create, FileAccess.ReadWrite)) {
        using (StreamWriter writer = new StreamWriter(stream)) {
          Parallel.ForEach(server.Properties.Keys, (key) => {
            writer.WriteLine(ToPropertiesString(key, server.Properties[key]));
          });
        }
      }
    }

    public static string ToPropertiesString(string key, string value) {
      return Regex.Replace(key, @"(?<=[a-z])([A-Z])", @"-$1").ToLower() + "=" + value;
    }

    //Create EULA file and set it to true by default
    public static void CreateEULA(Server server, ServerConfigMeta meta) {
      using (FileStream stream = new FileStream(meta.Path + "/" + server.Name + "/eula.txt", FileMode.Create, FileAccess.ReadWrite)) {
        using (StreamWriter writer = new StreamWriter(stream)) {
          writer.WriteAsync(meta.EULA);
        }
      }
    }

    //Download binarys for server
    public static void DownloadServerBinarys(Server server, ServerConfigMeta meta) {
      using (WebClient client = new WebClient()) {
        Console.WriteLine($"Downloading server binarys for {server.Name}...");
        client.DownloadFile(new Uri(server.MinecraftServerUrl), $"{meta.Path}/{server.Name}/[{server.GameVersion}]server.jar");
        Console.WriteLine($"Finished downloading server binarys for {server.Name}!");
      }
    }

    //Deserializtion
    public static ServerConfig DeserializeConfig(string ConfigFilePath) {
      using (FileStream package = new FileStream(ConfigFilePath, FileMode.Open, FileAccess.Read)) {
        using (StreamReader stream = new StreamReader(package)) {
          return JsonConvert.DeserializeObject<ServerConfig>(stream.ReadToEnd());
        }
      }
    }

    public static async Task<ServerConfig> DeserializeConfigAsync(string ConfigFilePath) {
      using (FileStream package = new FileStream(ConfigFilePath, FileMode.Open, FileAccess.Read)) {
        using (StreamReader stream = new StreamReader(package)) {
          return JsonConvert.DeserializeObject<ServerConfig>(await stream.ReadToEndAsync());
        }
      }
    }

  }
}
