using Newtonsoft.Json;

namespace MinecraftServerInstaller {
  public class JVMProperties {
    //in terms of m
    public int Xmx { get; set; }
    //in terms of m
    public int Xms { get; set; }
    public bool NoGui { get; set; }
  }
}