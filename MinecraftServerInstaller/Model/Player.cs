using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftServerInstaller.Model {
  public class Player {
    [JsonProperty("uuid")]
    public string Uuid { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
  }
}
