using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MinecraftServerManager.Models;

namespace MinecraftServerManagerWebAPI.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class ServersController : ControllerBase {
    private readonly ILogger<ServersController> _logger;

    public ServersController(ILogger<ServersController> logger) {
      _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Server> Get() {
      Response.Headers.Add("Access-Control-Allow-Origin", "*");
      return Startup.controller.Config.Servers;
    }
    [HttpPost]
    [Route("/CreateServer")]
    public IActionResult PostNewServer(Server server) {
      Response.Headers.Add("Access-Control-Allow-Origin", "*");
      server.Name = server.Name.Replace("\"", "");
      server.GameVersion = server.GameVersion.Replace("\"", "");
      server.ServerURL = server.ServerURL.Replace("\"", "");
      server.OutputFile = server.OutputFile.Replace("\"", "");
      Dictionary<string, string> vmprops = new Dictionary<string, string>(server.VMProperties);
      foreach(string s in vmprops.Keys) {
        server.VMProperties[s] = server.VMProperties[s].Replace("\"", "");
      }
      Dictionary<string, string> props = new Dictionary<string, string>(server.Properties);
      foreach(string s in props.Keys) {
        server.Properties[s] = server.Properties[s].Replace("\"", "");
      }
      if(!ModelState.IsValid) return BadRequest("Invalid Data");
      Startup.controller.CreateServer(server);
      return Ok();
    }
  }
}
