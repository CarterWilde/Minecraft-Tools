using MinecraftServerManager.Models.PlayerModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using MinecraftServerManager.Models.ServerModels;

namespace MinecraftServerManager.Models {
  class ModelSerializer {
    private string Path { get; }
    public ModelSerializer(string path) {
      Path = path;
    }
    public async Task<Server> Server() {
      using FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read);
      return await Server(stream);
    }
    public static async Task<Server> Server(Stream content) {
      return await JsonSerializer.DeserializeAsync<Server>(content);
    }
    public async Task<ServerConfig> ServerConfig() {
      FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read);
      return await ServerConfig(stream);
    }
    public static async Task<ServerConfig> ServerConfig(Stream content) {
      return await JsonSerializer.DeserializeAsync<ServerConfig>(content);
    }
    public static string ToPropertiesString(string key, string value) {
      return Regex.Replace(key, @"(?<=[a-z])([A-Z])", @"-$1").ToLower() + "=" + value;
    }

    //Create EULA file and set it to true by default
    public async Task CreateEULA(ServerConfig config, Server server) {
      using(FileStream stream = new FileStream(config.Path + "/" + server.Name + "/eula.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
        using(StreamWriter writer = new StreamWriter(stream)) {
          await writer.WriteAsync(config.EULA);
        }
      }
    }
  }
}
