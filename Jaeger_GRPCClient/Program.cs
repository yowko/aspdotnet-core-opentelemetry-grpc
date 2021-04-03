using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcService;

namespace Jaeger_GRPCClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5001");

            var client = new Greeter.GreeterClient(channel);
            var response = await client.SayHelloAsync(new HelloRequest { Name = "World" });

            Console.WriteLine("Greeting: " + response.Message);

        }
    }
}