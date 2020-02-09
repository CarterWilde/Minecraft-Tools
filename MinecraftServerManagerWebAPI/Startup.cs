using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MinecraftServerManager;
using MinecraftServerManager.Models;
using MinecraftServerManager.Models.ServerModels;

namespace MinecraftServerManagerWebAPI {
  public class Startup {
    public static ServerController controller;

    public Startup(IConfiguration configuration) {
      Configuration = configuration;
      FileStream fileStream = new FileStream("./servers.config.json", FileMode.Open, FileAccess.Read);
      ServerConfig config = ModelSerializer.ServerConfig(fileStream);
      fileStream.Close();
      ServerBuilder builder = new ServerBuilder(config);
      controller = new ServerController(config);
      builder.Build();
      controller.StartAll();
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.AddCors(options => {
        options.AddPolicy("AllowOrigin", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().Build());
      });
      services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if(env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseCors("AllowOrigin");

      app.UseAuthorization();

      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
    }
  }
}
