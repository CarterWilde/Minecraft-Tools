
using MinecraftServerInstaller.Controllers;
using System;

namespace MinecraftServerInstaller {
  class Program {
    static void Main(string[] args) {
      Console.SetOut(new ConsoleWirter());
      ServerBuilder builder = new ServerBuilder(ServerBuilder.DeserializeConfig(args[0]));
      ServerManager manager = new ServerManager(builder.Config);
      builder.BuildAll();
      Console.WriteLine("Press enter to run servers once they are built");
      Console.ReadKey();
      manager.RunAll();
    }
  }
}
