using System;
using System.IO;
using MinecraftServerManager.Models.ServerModels;
using MinecraftServerManager.Models;
using MinecraftServerManager;
using System.Net;
using System.Threading;
using System.Diagnostics;

namespace MinecraftServerManagerConsoleInterface {
  class Program {
    private static ServerController controller;
    public static bool hasExited = false;
    static void Main(string[] args) {
      FileStream fileStream = new FileStream("./servers.config.json", FileMode.Open, FileAccess.Read);
      ServerConfig config = ModelSerializer.ServerConfig(fileStream);
      ServerBuilder builder = new ServerBuilder(config);
      controller = new ServerController(config);
      foreach(ServerManager server in controller.Config.Servers) {
        server.OutputData_Recived += Server_OutputData_Recived;
      }
      builder.BuildComplete += Builder_BuildComplete;
      builder.Build();
      while(!hasExited) {
        Console.ReadLine();
      }
    }

    private static void Server_OutputData_Recived(object sender, CustomDataReceivedEventArgs e) {
      Console.WriteLine("" + e.Data);
    }

    private static void Builder_BuildComplete(object sender, EventArgs e) {
      controller.StartAll();
    }
  }
}
