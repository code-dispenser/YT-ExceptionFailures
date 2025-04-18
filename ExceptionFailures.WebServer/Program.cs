using Autofac;
using Autofac.Extensions.DependencyInjection;
using ExceptionFailures.Application.Configuration;
using ExceptionFailures.Infrastructure.Configuration;
using ExceptionFailures.WebServer.GrpcServices;
using ProtoBuf.Grpc.Server;
using System.Reflection.Emit;

namespace ExceptionFailures.WebServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(Container => {
            
                Container.RegisterModule<ApplicationAutofacModule>();
                Container.RegisterModule<InfrastructureAutofacModule>();
            });

            // Add services to the container.
            builder.Services.AddControllers();
           
            builder.Services.AddCodeFirstGrpc();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.MapGrpcService<GrpcSuppliersService>();

            app.Run();
        }
    }
}
