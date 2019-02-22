﻿using System.Collections.Generic;

namespace MinecraftServerManager.Model {
  public class Server {
    public string Name { get; set; }
    public string GameVersion { get; set; }
    public string MinecraftServerUrl { get; set; }
    public string ForgeServerUrl { get; set; }
    public Dictionary<string, string> VMProperties { get; set; }
    public Dictionary<string, string> Properties { get; set; }
    public IList<string> BannedIPS { get; set; }
    public IList<PlayerBan> BannedPlayers { get; set; }
    public IList<PlayerOp> Ops { get; set; }
  }
}