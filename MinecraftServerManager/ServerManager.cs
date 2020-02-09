using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MinecraftServerManager.Models;
using MinecraftServerManager.Models.ServerModels;

namespace MinecraftServerManager {
  public class ServerManager : Server, INotifyPropertyChanged{
    public event EventHandler<CustomDataReceivedEventArgs> OutputData_Recived;
    public event PropertyChangedEventHandler PropertyChanged;

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
    [JsonIgnore]
    public ServerConfig Config { get; set; }
    public ServerManager() {}

    public ServerManager(Server server, string path, ServerConfig config) : base(
                                               server.Name
                                              ,server.GameVersion
                                              ,server.ServerURL
                                              ,server.OutputFile
                                              ,server.VMProperties
                                              ,server.Properties
                                              ,server.BannedIPS
                                              ,server.Bans
                                              ,server.Ops
                                              ,server.Modpack){
      Config = config;
      PropertyChanged += ServerManager_PropertyChanged;
      Start(path);
    }

    private void ServerManager_PropertyChanged(object sender, PropertyChangedEventArgs e) {
      ModelSerializer.UpdateConfig(Config);
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
      Start(path);
      PropertyChanged += ServerManager_PropertyChanged;
      await Task.CompletedTask;
    }
    public void Start(string path) {
      StartInfo = ModelSerializer.VMPropertiesToStartInfo(this, Config);
      string folder = Path.Combine(path, Name);
      FullOutputFilePath = Path.Combine(folder, OutputFile);
      File = new FileStream(FullOutputFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
      FileWriter = new StreamWriter(File) {
        AutoFlush = true
      };
      FileReader = new StreamReader(File);
      Process = new Process {
        StartInfo = StartInfo
      };
      Process.StartInfo.RedirectStandardInput = true;
      Process.StartInfo.RedirectStandardOutput = true;
      Process.StartInfo.WorkingDirectory = folder;
      Process.OutputDataReceived += Process_OutputDataReceived;
      PropertyChanged += ServerManager_PropertyChanged;
      Console.WriteLine($"[{Name}]Has Started at {Process.StartInfo.WorkingDirectory}");
      Process.Start();
      Process.BeginOutputReadLine();
    }

    private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e) {
      FileWriter.BaseStream.Seek(0, SeekOrigin.End);
      FileWriter.WriteLine(e.Data);
      OnOutputDataReceived(new CustomDataReceivedEventArgs(e.Data, Name));
    }

    protected virtual void OnOutputDataReceived(CustomDataReceivedEventArgs e) {
      OutputData_Recived?.Invoke(this, e);
    }

    public string GetLog() {
      if(FileReader != null) {
        FileReader.BaseStream.Seek(0, SeekOrigin.Begin);
        return FileReader.ReadToEnd();
      } else {
        return "Server Wasn't Ready!";
      }
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

  public class CustomDataReceivedEventArgs{
    public string Data { get; set; }
    public string Name { get; set; }
    public CustomDataReceivedEventArgs(string data, string name) {
      Name = name;
      Data = $"[{Name}]" + data;
    }
  }
}
