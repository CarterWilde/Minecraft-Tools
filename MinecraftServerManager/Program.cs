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
      ServerController controller = new ServerController(config);
      builder.Build();
      //bool isQutting = false;
      //do {
      //  Console.WriteLine("!ls: for list of servers \n" +
      //                    "!ss: to select server\n" +
      //                    "!q: to quit");
      //  string userInput = Console.ReadLine();
      //  switch(userInput.ToLower()) {
      //    case "!ls": {
      //        foreach(Server server in controller.Config.Servers) {
      //          Console.WriteLine(server.Name);
      //        }
      //        break;
      //      }
      //    case "!ss": {
      //        Console.WriteLine("Input server");
      //        string serverChoice = Console.ReadLine();
      //        foreach(ServerManager server in controller.Config.Servers) {
      //          if(serverChoice == server.Name) {
      //            server.OutputStream.ReadToEnd();
      //          }
      //        }
      //        break;
      //      }
      //    case "!q": {
      //        controller.StopAll();
      //        isQutting = true;
      //        break;
      //      }
      //  }
      //} while(!isQutting);
    }
  }
}
