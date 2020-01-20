using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftClientInstaller.Model {
  class Modpack {
    public string Name { get; set; }
    public IList<string> Authors { get; set; }
    public string Version { get; set; }
    public string GameVersion { get; set; }
    public IList<Mod> Server { get; set; }
    public IList<Mod> Joint { get; set; }
    public IList<Mod> Client { get; set; }
  }
}
