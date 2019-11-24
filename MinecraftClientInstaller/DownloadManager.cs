using MinecraftClientInstaller.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace MinecraftClientInstaller {
  class DownloadManager {
    private string ModpackPath { get; }
    private readonly string GameVersion;

    public DownloadManager(string modpackpath, string gameVersion) {
      GameVersion = gameVersion;
      ModpackPath = modpackpath;
      Directory.CreateDirectory(ModpackPath);
    }

    public void DownloadMods(IList<Mod> mods) {
      if(mods != null) {
        Parallel.ForEach(mods, (mod, state) => {
          if(mod != null) {
            DownloadMods(mod.Dependencies);
          }
          WebClient client = new WebClient();
          Console.WriteLine($"Downloading dependencie '{mod.Name}'...");
          try {
            client.DownloadFile(new Uri(mod.Uri), $"{ModpackPath}/[{GameVersion}]{mod.Name}-{mod.Version}.jar");
          }
          catch(WebException we) {
            Console.WriteLine($"The mod {mod.Name} failed to install. \n{we.Message}");
          }
          Console.WriteLine($"Done downloading dependencie '{mod.Name}'...");
          if(mod == null) {
            state.Break();
          }
        });
      }
    }
  }
}
