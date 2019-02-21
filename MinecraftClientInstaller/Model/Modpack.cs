using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftClientInstaller.Model {
  class Modpack {
    public ModpackMeta Meta { get; set; }
    public IList<Mod> Server { get; set; }
    public IList<Mod> Joint { get; set; }
    public IList<Mod> Client { get; set; }
  }
}
