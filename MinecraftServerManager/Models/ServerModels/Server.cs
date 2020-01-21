using MinecraftServerManager.Models.PlayerModels;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using MinecraftClientInstaller.Model;

namespace MinecraftServerManager.Models {
  public class Server {
    public string Name { get; set; }
    public string GameVersion { get; set; }
    public string ServerURL { get; set; }
    public Dictionary<string, string> VMProperties { get; set; }
    public Dictionary<string, string> Properties { get; set; }
    public IList<string> BannedIPS { get; set; }
    public IList<Ban> Bans { get; set; }
    public IList<Op> Ops { get; set; }
    public Modpack Modpack { get; set; }
  }
}