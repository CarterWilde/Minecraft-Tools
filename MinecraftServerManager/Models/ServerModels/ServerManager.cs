using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MinecraftServerManager.Models.ServerModels {
  public class ServerManager : Server{
    [JsonIgnore]
    public Process Process { get; private set; }
    [JsonIgnore]
    public StreamReader OutputStream { get; private set; }
    [JsonIgnore]
    public StreamWriter InputStream { get; private set; }
    [JsonIgnore]
    public ProcessStartInfo StartInfo { get; set; }
    public ServerManager() {}
    public ServerManager(ProcessStartInfo startInfo) {
      StartInfo = startInfo;
    }
    public async Task Start() {
      Process = new Process();
      Process.StartInfo = StartInfo;
      Process.StartInfo.RedirectStandardInput = true;
      Process.StartInfo.RedirectStandardOutput = true;
      Process.StartInfo.WorkingDirectory = $"Servers/{Name}";
      Process.Start();
      OutputStream = Process.StandardOutput;
      InputStream = Process.StandardInput;
      await Task.CompletedTask;
    }
    public async Task Stop() {
      Process.Kill();
      await Task.CompletedTask;
    }
  }
}
