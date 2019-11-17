using Newtonsoft.Json;
using System;

namespace MinecraftServerManager.Models.PlayerModels {
  public class Ban : Player {
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