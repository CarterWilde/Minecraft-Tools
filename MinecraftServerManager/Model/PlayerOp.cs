﻿using Newtonsoft.Json;

namespace MinecraftServerManager.Model {
  public class PlayerOp {
    [JsonExtensionData]
    public Player Player { get; set; }
    [JsonProperty("level")]
    public OpLevels Level { get; set; }
  }
  public enum OpLevels {
    Low = 1,
    Medium = 2,
    High = 3,
    Full = 4
  }
}