using MinecraftServerManager.Models;
using MinecraftServerManager.Models.ServerModels;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MinecraftServerManager {
  class Program {
    static async Task Main(string[] args) {
      FileStream fileStream = new FileStream("./servers.config.json", FileMode.Open, FileAccess.Read);
      ServerConfig config = await ModelSerializer.ServerConfig(fileStream);
      ServerBuilder builder = new ServerBuilder(config);
      builder.Build();
    }
  }
}
