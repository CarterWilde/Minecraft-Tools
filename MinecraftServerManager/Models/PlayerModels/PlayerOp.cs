using System.Text.Json.Serialization;

namespace MinecraftServerManager.Models.PlayerModels {
  public class Op : Player {
    [JsonPropertyName("level")]
    public OpLevel Level { get; set; }
  }
}