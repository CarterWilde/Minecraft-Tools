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
      server.Name.Replace("\"", "");
      server.GameVersion.Replace("\"", "");
      server.ServerURL.Replace("\"", "");
      server.OutputFile.Replace("\"", "");
      foreach(string s in server.VMProperties.Keys) {
        server.VMProperties[s].Replace("\"", "");
      }
      foreach(string s in server.Properties.Keys) {
        server.Properties[s].Replace("\"", "");
      }
      if(!ModelState.IsValid) return BadRequest("Invalid Data");
      Startup.controller.CreateServer(server);
      return Ok();
    }
  }
}
