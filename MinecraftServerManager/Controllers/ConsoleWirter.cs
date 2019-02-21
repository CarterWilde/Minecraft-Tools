using System;
using System.IO;
using System.Text;

namespace MinecraftServerInstaller.Controllers {
  class ConsoleWirter : TextWriter {
    readonly ConsoleColor DefaultBackColor;
    readonly ConsoleColor DefaultForeColor;
    private TextWriter originalOut;

    public ConsoleWirter() {
      originalOut = Console.Out;
      DefaultBackColor = Console.BackgroundColor;
      DefaultForeColor = Console.ForegroundColor;
    }

    public override Encoding Encoding {
      get { return new System.Text.UTF8Encoding(); }
    }
    public override void WriteLine(string message) {
      originalOut.WriteLine(String.Format("[{0}] {1}", DateTime.Now, message));
    }
    public override void Write(string message) {
      originalOut.Write(String.Format("[{0}] {1}", DateTime.Now, message));
    }
  }
}
