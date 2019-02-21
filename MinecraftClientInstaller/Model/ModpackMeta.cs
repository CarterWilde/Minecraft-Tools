using System;
using System.Collections.Generic;

namespace MinecraftClientInstaller.Model {
  public class ModpackMeta {
    public string Name { get; set; }
    public IList<string> Authors { get; set; }
    public string Version { get; set; }
    public string GameVersion { get; set; }
  }
}