using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MinecraftServerManager.Models;

namespace MinecraftServerManager {
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

    public ServerManager(Server server, string path) : base(server.Name
                                              ,server.GameVersion
                                              ,server.ServerURL
                                              ,server.OutputFile
                                              ,server.VMProperties
                                              ,server.Properties
                                              ,server.BannedIPS
                                              ,server.Bans
                                              ,server.Ops
                                              ,server.Modpack){
      Start(path);
    }

    public void UpdateProps(Server server) {
      Name = server.Name;
      GameVersion = server.GameVersion;
      ServerURL = server.ServerURL;
      OutputFile = server.OutputFile;
      VMProperties = server.VMProperties;
      Properties = server.Properties;
      BannedIPS = server.BannedIPS;
      Bans = server.Bans;
      Ops = server.Ops;
      Modpack = server.Modpack;
    }

    public async Task StartAsync(string path) {
      string folder = Path.Combine(path, Name);
      FullOutputFilePath = Path.Combine(folder, OutputFile);
      File = new FileStream(FullOutputFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
      FileWriter = new StreamWriter(File);
      FileWriter.AutoFlush = true;
      FileReader = new StreamReader(File);
      Process = new Process();
      Process.StartInfo = StartInfo;
      Process.StartInfo.RedirectStandardInput = true;
      Process.StartInfo.RedirectStandardOutput = true;
      Process.StartInfo.WorkingDirectory = folder;
      Process.OutputDataReceived += Process_OutputDataReceived;
      Process.Start();
      Process.BeginOutputReadLine();
      await Task.CompletedTask;
    }
    public void Start(string path) {
      FullOutputFilePath = Path.Combine(Path.Combine(Name, path), OutputFile);
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
    }

    private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e) {
      FileWriter.WriteLine(e.Data);
    }

    public string GetLog() {
      FileReader.BaseStream.Seek(0, SeekOrigin.Begin);
      return FileReader.ReadToEnd();
    }

    public async Task<string> GetLogAsync() {
      FileReader.BaseStream.Seek(0, SeekOrigin.Begin);
      return await FileReader.ReadToEndAsync();
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
