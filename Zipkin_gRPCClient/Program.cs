﻿using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcService;
using Microsoft.Extensions.Configuration;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Zipkin_gRPCClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional:true,reloadOnChange: true)
                .Build();
            
            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(config.GetValue<string>("Zipkin:ServiceName")))
                .AddGrpcClientInstrumentation()
                .AddZipkinExporter(zipkinOptions =>
                {
                    zipkinOptions.Endpoint = new Uri(config.GetValue<string>("Zipkin:Endpoint"));
                })
                .Build(); 
            
            var channel = GrpcChannel.ForAddress("http://localhost:5001");

            var client = new Greeter.GreeterClient(channel);
            var response = await client.SayHelloAsync(new HelloRequest { Name = "World" });

            Console.WriteLine("Greeting: " + response.Message);
        }
    }
}