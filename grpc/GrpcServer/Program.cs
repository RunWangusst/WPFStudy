using Grpc.Core;

namespace GrpcServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //提供服务
            Server server = new Server()
            {
                Services = { GrpcService.FileTransfer.BindService(new FileImpl()) },
                Ports = { new ServerPort("127.0.0.1", 50000, ServerCredentials.Insecure) }
            };
            // 服务开始
            server.Start();

            while (Console.ReadLine().Trim().ToLower() != "exit")
            {

            }

            // 结束服务
            server.ShutdownAsync();
            Console.WriteLine("Hello, World!");
        }
    }
}