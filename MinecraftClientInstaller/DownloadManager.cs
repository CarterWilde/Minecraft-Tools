using MinecraftClientInstaller.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace MinecraftClientInstaller {
  class DownloadManager {
    private WebClient Client { get; }
    private string ModpackPath { get; }
    private readonly string GameVersion;

    public DownloadManager(string modpackpath, string gameVersion) {
      GameVersion = gameVersion;
      ModpackPath = modpackpath;
      Client = new WebClient();
      Directory.CreateDirectory(ModpackPath);
    }

    public void DownloadMods(IList<Mod> mods) {
      if (mods != null) {
        foreach (Mod mod in mods) {
          Console.WriteLine($"Downloading dependencie '{mod.Name}'...");
          Client.DownloadFile(new Uri(mod.Uri), $"{ModpackPath}/[{GameVersion}]{mod.Name}-{mod.Version}.jar");
          Console.WriteLine($"Done downloading dependencie '{mod.Name}'...");
          if (mod != null) {
            DownloadMods(mod.Dependencies);
          } else {
            break;
          }
        }
      }
    }
  }
}
