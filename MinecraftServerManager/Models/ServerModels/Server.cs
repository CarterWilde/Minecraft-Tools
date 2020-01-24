using MinecraftServerManager.Models.PlayerModels;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using MinecraftClientInstaller.Model;

namespace MinecraftServerManager.Models {
  public class Server {
    public Server() {}

    public Server(string name, string gameVersion, string serverURL, string outputFile, Dictionary<string, string> vMProperties, Dictionary<string, string> properties, IList<string> bannedIPS, IList<Ban> bans, IList<Op> ops, Modpack modpack) {
      Name = name;
      GameVersion = gameVersion;
      ServerURL = serverURL;
      OutputFile = outputFile;
      VMProperties = vMProperties;
      Properties = properties;
      BannedIPS = bannedIPS;
      Bans = bans;
      Ops = ops;
      Modpack = modpack;
    }

    public string Name { get; set; }
    public string GameVersion { get; set; }
    public string ServerURL { get; set; }
    public string OutputFile { get; set; }
    public Dictionary<string, string> VMProperties { get; set; }
    public Dictionary<string, string> Properties { get; set; }
    public IList<string> BannedIPS { get; set; }
    public IList<Ban> Bans { get; set; }
    public IList<Op> Ops { get; set; }
    public Modpack Modpack { get; set; }
  }
}