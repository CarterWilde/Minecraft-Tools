using System;
using System.Collections.Generic;

namespace MinecraftServerManager.Models {
  public class ServerConfig : Server {
    public readonly string EULA = $"#By changing the setting below to TRUE you are indicating your agreement to our EULA (https://account.mojang.com/documents/minecraft_eula).{Environment.NewLine}#{DateTime.Now}{Environment.NewLine}eula=true";
    public string Version { get; set; }
    private string path;
    public string Path {
      get {
        return path;
      }
      set {
        path = value;
      }
    }

    public IList<Server> Servers { get; set; }
  }
}