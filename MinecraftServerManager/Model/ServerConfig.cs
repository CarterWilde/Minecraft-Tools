using System.Collections.Generic;

namespace MinecraftServerInstaller {
  public class ServerConfig {
    public ServerConfigMeta ConfigMeta { get; set; }
    public IList<Server> Servers { get; set; }
  }
}