using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServerManager.Models.ServerModels {
  class ServerManager {
    public StreamReader OutputStream { get; }
    public StreamWriter InputStream { get; }
    public ProcessStartInfo StartInfo { get; }
    public Server Server;
    public ServerManager(Server server, ProcessStartInfo startInfo) {
      Server = server;
      StartInfo = startInfo;
    }
    public async Task Start() {
      await Task.CompletedTask;
    }
    public async Task Stop() {
      await Task.CompletedTask;
    }
  }
}
