using System;

namespace MinecraftServerInstaller {
  public class ServerConfigMeta {
    public readonly string EULA = $"#By changing the setting below to TRUE you are indicating your agreement to our EULA (https://account.mojang.com/documents/minecraft_eula).{Environment.NewLine}#{DateTime.Now}{Environment.NewLine}eula=true";

    public string Name { get; set; }
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
    public string BackupsPath { get; set; }
  }
}