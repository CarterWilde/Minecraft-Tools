using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftClientInstaller.Model {
  public class Mod {
    public string Name { get; set; }
    public string Version { get; set; }
    public string Uri { get; set; }
    public IList<Mod> Dependencies { get; set; }
  }
}
