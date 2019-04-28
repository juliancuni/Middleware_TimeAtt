using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Middleware_TimeAtt
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
            {
                x.Service<TCPServer>(s =>
                {
                    s.ConstructUsing(tcpServer => new TCPServer());
                    s.WhenStarted(tcpServer => tcpServer.StartServer());
                    s.WhenStopped(tcpServer => tcpServer.StopServer());
                });
                //x.RunAs("administrator", "newSnew2013.");
                x.RunAsLocalSystem();
                //x.RunAsLocalService();

                x.SetServiceName("Middleware_TimeAtt");
                x.SetDisplayName("TimeAtt Middleware");
                x.SetDescription("Middleware midis Backend Nodejs server dhe ZKTeco i360. TCP server per te degjuar komandat nga Nodejs, TCP client per tu lidhur me ZKTeco dhe per ti nisur komanda qe vijne nga WEBClient. Gjithashtu dhe REST Api client per te regjistruar ne databaze informacione mbi Attendances");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
