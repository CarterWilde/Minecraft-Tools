using MinecraftClientInstaller.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace MinecraftClientInstaller {
  class DownloadManager {
    private readonly string ModpackPath;
    private readonly string GameVersion;

    public DownloadManager(string modpackpath, string gameVersion) {
      GameVersion = gameVersion;
      ModpackPath = modpackpath;
      Directory.CreateDirectory(ModpackPath);
    }

    public void DownloadMods(IList<Mod> mods) {
      if (mods != null) {
        Parallel.ForEach(mods, (mod)=> {
          WebClient Client = new WebClient();
          Console.WriteLine($"Downloading dependencie '{mod.Name}'...");
          Client.DownloadFile(new Uri(mod.Uri), $"{ModpackPath}/[{GameVersion}]{mod.Name}-{mod.Version}.jar");
          Console.WriteLine($"Done downloading dependencie '{mod.Name}'...");
          if (mod != null) {
            DownloadMods(mod.Dependencies);
          } else {
            return;
          }
        });
      }
    }
  }
}
