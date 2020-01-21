using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MinecraftServerManager.Models.PlayerModels {
  public class Player {
    [JsonPropertyName("uuid")]
    public string Uuid { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
  }
}
