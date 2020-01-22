using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MinecraftServerManager.Models.ServerModels {
  public class ServerManager : Server {
    [JsonIgnore]
    private Process Process { get; set; }
    [JsonIgnore]
    private StreamWriter OutputStream { get; set; }
    [JsonIgnore]
    private FileStream File { get; set; }
    [JsonIgnore]
    private StreamWriter FileWriter { get; set; }
    [JsonIgnore]
    private StreamReader FileReader { get; set; }
    [JsonIgnore]
    public ProcessStartInfo StartInfo { get; set; }
    [JsonIgnore]
    public string FullOutputFilePath { get; private set; }
    public ServerManager() {}
    public ServerManager(ProcessStartInfo startInfo) {
      StartInfo = startInfo;
    }
    public async Task Start(String folder) {
      FullOutputFilePath = Path.Combine(folder, OutputFile);
      File = new FileStream(FullOutputFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
      FileWriter = new StreamWriter(File);
      FileWriter.AutoFlush = true;
      FileReader = new StreamReader(File);
      Process = new Process();
      Process.StartInfo = StartInfo;
      Process.StartInfo.RedirectStandardInput = true;
      Process.StartInfo.RedirectStandardOutput = true;
      Process.StartInfo.WorkingDirectory = $"Servers/{Name}";
      Process.OutputDataReceived += Process_OutputDataReceived;
      Process.Start();
      Process.BeginOutputReadLine();
      await Task.CompletedTask;
    }

    private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e) {
      FileWriter.WriteLine(e.Data);
    }

    public string GetLog() {
      return FileReader.ReadToEnd();
    }

    public void SendCommand(string command) {
      Process.StandardInput.WriteLineAsync(command);
    }

    public async Task Stop() {
      Process.Kill();
      await Task.CompletedTask;
    }
  }
}
