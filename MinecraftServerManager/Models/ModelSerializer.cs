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
using System.Diagnostics;

namespace MinecraftServerManager.Models {
  public class ModelSerializer {
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
    public async Task<ServerConfig> ServerConfigAsync() {
      FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read);
      return await ServerConfigAsync(stream);
    }
    public static ServerConfig ServerConfig(Stream content) {
      StreamReader reader = new StreamReader(content);
      return JsonSerializer.Deserialize<ServerConfig>(reader.ReadToEnd());
    }
    public static async Task<ServerConfig> ServerConfigAsync(Stream content) {
      return await JsonSerializer.DeserializeAsync<ServerConfig>(content);
    }
    public static string ToPropertiesString(string key, string value) {
      return Regex.Replace(key, @"(?<=[a-z])([A-Z])", @"-$1").ToLower() + "=" + value;
    }

    public static ProcessStartInfo VMPropertiesToStartInfo(Server server, ServerConfig config) {
      ProcessStartInfo info = new ProcessStartInfo("java");
      info.WorkingDirectory = $"{config.Path}/{server.Name}";
      bool nogui = false;
      foreach(string key in server.VMProperties.Keys) {
        bool flag = false;
        try {
          flag = Boolean.Parse(server.VMProperties[key]);
        }
        catch(FormatException e) {
          flag = false;
        }
        if(key.Equals("nogui")) {
          if(flag) {
            nogui = true;
          }
        } else if(!key.Equals("jar") && !flag) {
          info.ArgumentList.Add($"-{key}{server.VMProperties[key]}");
        } else {
          info.ArgumentList.Add($"-{key}");
        }
      }
      info.ArgumentList.Add($"[{server.GameVersion}]server.jar");
      if(nogui) {
        info.ArgumentList.Add("nogui");
      }
      return info;
    }
  }
}