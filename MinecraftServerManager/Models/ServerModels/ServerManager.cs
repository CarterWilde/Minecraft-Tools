using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MinecraftServerManager.Models.ServerModels {
  public class ServerManager : Server{
    [JsonIgnore]
    public StreamReader OutputStream { get; }
    [JsonIgnore]
    public StreamWriter InputStream { get; }
    [JsonIgnore]
    public ProcessStartInfo StartInfo { get; set; }
    public ServerManager() {}
    public ServerManager(ProcessStartInfo startInfo) {
      StartInfo = startInfo;
    }
    public async Task Start() {
      await Task.CompletedTask;
    }
    public async Task Stop() {
      await Task.CompletedTask;
    }
    public static string ToArgString(string key, string value) {
      if(key == "nogui") {
        return String.Empty;
      } else {
        bool boolValue = Boolean.Parse(value);
        if(!boolValue) {
          return String.Empty;
        }
        else if(boolValue) {
          return "-" + key;
        } else {
          return "-" + key + value;
        }
      }
    }
  }
}
