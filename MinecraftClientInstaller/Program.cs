using MinecraftClientInstaller.Model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace MinecraftClientInstaller{
  class Program {
    public static bool InDebug;
    public static void Main(string[] args) {
      InDebug = false;
#if DEBUG
      InDebug = true;
#endif
      Modpack modpack = DeserializeModpack(args[0]);
      string ModpackPath = $"{Environment.GetEnvironmentVariable("AppData") + "\\.minecraft\\mods"}\\{modpack.Meta.GameVersion}";
      if (InDebug) {
        ModpackPath = $".\\{modpack.Meta.GameVersion}";
      }
      DownloadManager manager = new DownloadManager(ModpackPath, modpack.Meta.GameVersion);
      Console.WriteLine("Downloading to " + ModpackPath);
      Console.WriteLine("Starting Downloads! Some of these mods might be large, so it could be awhile.");
      manager.DownloadMods(modpack.Joint);
      manager.DownloadMods(modpack.Client);
    }

    static Modpack DeserializeModpack(string FileName) {
      using (FileStream package = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
        using (StreamReader stream = new StreamReader(package)) {
          return JsonConvert.DeserializeObject<Modpack>(stream.ReadToEnd());
        }
      }
    }
  }
}
