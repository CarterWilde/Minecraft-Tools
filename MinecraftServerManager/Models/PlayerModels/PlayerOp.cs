using Newtonsoft.Json;

namespace MinecraftServerManager.Models.PlayerModels {
  public class Op : Player {
    [JsonProperty("level")]
    public OpLevel Level { get; set; }
  }
}