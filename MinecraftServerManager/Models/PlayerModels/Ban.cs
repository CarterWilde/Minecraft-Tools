using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MinecraftServerManager.Models.PlayerModels {
  public class Ban : Player {
    [JsonPropertyName("created")]
    public DateTime Created { get; set; }
    [JsonPropertyName("source")]
    public string Source { get; set; }
    [JsonPropertyName("expires")]
    public string Expires { get; set; }
    [JsonPropertyName("reason")]
    public string Reason { get; set; }
  }
}