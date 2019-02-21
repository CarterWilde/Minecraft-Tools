using MinecraftServerInstaller.Model;
using Newtonsoft.Json;
using System;

namespace MinecraftServerInstaller {
  public class PlayerBan {
    [JsonExtensionData]
    public Player Player { get; set; }
    [JsonProperty("created")]
    public DateTime Created { get; set; }
    [JsonProperty("source")]
    public string Source { get; set; }
    [JsonProperty("expires")]
    public string Expires { get; set; }
    [JsonProperty("reason")]
    public string Reason { get; set; }
  }
}