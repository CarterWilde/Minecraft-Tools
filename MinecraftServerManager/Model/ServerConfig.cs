using System.Collections.Generic;

namespace MinecraftServerManager.Model {
  public class ServerConfig {
    public ServerConfigMeta ConfigMeta { get; set; }
    public IList<Server> Servers { get; set; }
  }
}