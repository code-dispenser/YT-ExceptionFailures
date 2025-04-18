using ExceptionFailures.ConsoleClient.Common.Exceptions;
using ExceptionFailures.ConsoleClient.Common.Interceptors;
using ExceptionFailures.Contracts.Areas.Suppliers;
using Flow.Core.Areas.Extensions;
using Flow.Core.Areas.Returns;
using Flow.Core.Common.Models;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using ProtoBuf.Grpc.Client;
using System.Net.Http.Json;

namespace ExceptionFailures.ConsoleClient;

internal class Program
{
    private static readonly Uri  _hostingServer             = new Uri("https://localhost:44316"); // <<<< Choose your poison, this is for IIS Express see launch settings on web server .
    //private static readonly Uri    _hostingServer          = new Uri("https://localhost:7269"); //  <<<< kestrel
    private static readonly string _suppliersUrl            = "/api/suppliers";
    private static readonly string _delegatingClientName    = "DelegatingClient";
    private static readonly string _nonDelegatingClientName = "NonDelegatingClient";
    static async Task Main()
    {
        var _httpClientFactory  = ConfigureServiceProvider(_hostingServer, _delegatingClientName,_nonDelegatingClientName);
        var suppliersUrl         = "/api/suppliers";
        var httpClient           = _httpClientFactory.CreateClient(_nonDelegatingClientName);
        var delegatingHttpClient = _httpClientFactory.CreateClient(_delegatingClientName);

        var channel              = GrpcChannel.ForAddress(_hostingServer);
        var supplierService      = channel.CreateGrpcService<IGrpcSuppliersService>();

        //try
        //{

        //    //var supplierView = await httpClient.GetFromJsonAsync<SupplierView>($"{suppliersUrl}/1");
        //    //Console.WriteLine(supplierView);

        //    //var response         = await httpClient.DeleteFromJsonAsync<None>($"{suppliersUrl}/1");
        //    var delegatingResponse = await delegatingHttpClient.DeleteFromJsonAsync<None>($"{suppliersUrl}/1");

        //}
        //catch (HttpRequestException ex)
        //{
        //    Console.WriteLine($"HttpRequestException: {ex.Message}");
        //}
        //catch (ProblemDetailsException)
        //{
        //    Console.WriteLine($"Back in our delete methods try catch block - now what?");
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine(ex.Message);
        //}

        //Console.ReadLine();

        #region Using a result type like Flow

        var result = await delegatingHttpClient.GetFromJsonAsync<Flow<SupplierView>>($"{suppliersUrl}/getwithflow/1")!
                        .OnFailure(failure => Console.Out.WriteLineAsync(failure.Reason))
                            .OnSuccess(Console.Out.WriteLine)
                                .Finally(failure => new SupplierView(0, "", 0), success => success);


        await httpClient.DeleteFromJsonAsync<Flow<None>>($"{suppliersUrl}/deletewithflow/1")!
                            .OnSuccess(Console.WriteLine)
                                .OnFailure(failure => Console.WriteLine(failure.Reason));


        await supplierService.GetSupplier(new GetSupplier(1))
                                .OnSuccess(success => Console.WriteLine(success))
                                    .OnFailure(failure => Console.WriteLine(failure.Reason));


        await supplierService.DeleteSupplier(new DeleteSupplier(1)).TryCatchGrpcResult()// <<<< This is a custom extension for any unhandled errors not put into a flow. Comment out everything and run without the server.
                                .OnSuccess(success => Console.Out.WriteLineAsync(success.ToString()))
                                    .OnFailure(failure => Console.Out.WriteLineAsync(failure.Reason));

        #endregion

        Console.ReadLine();
    }
    static IHttpClientFactory ConfigureServiceProvider(Uri hostUrl, string delegatingClientName, string nonDelegatingClientName)
    {
        var services = new ServiceCollection();

        services.AddTransient<HttpClientDelegatingHandler>();

        services.AddHttpClient(nonDelegatingClientName, client => client.BaseAddress = hostUrl);

        services.AddHttpClient(delegatingClientName, client => client.BaseAddress = hostUrl)
                 .AddHttpMessageHandler<HttpClientDelegatingHandler>();

        return services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>();
    }
}

public static class GrpcFlowExt
{
    /*
        * For more rpc control you can use client side grpc interceptors that you can register via the AddCodeFirstGrpcClient and the GrpcClientFactoryOptions.
        * 
        * See the Flow demo for more information: https://github.com/code-dispenser/Flow
        * 
        * This extension is just an example/alternate way of handling unexpected grpc issues i.e not in a flow.
    */
    public static async Task<Flow<T>> TryCatchGrpcResult<T>(this Task<Flow<T>> @thisOperation)
    {
        try
        {
            return await @thisOperation;
        }
        catch (RpcException ex)
        {
            return Flow<T>.Failed(new Failure.UnknownFailure("A problem has occurred, please try again in a few minutes. If the problem persists please contact the system administrator.", exception: ex));
        }
    }
}
/*
    * You can also make an extension for json. You then use the non extension types GetAsync DeleteAsync etc so you can attach to Task<HttpResponseMessage>
    * See the Flow demo for more information as it has a TryCatchJsonResult extension: https://github.com/code-dispenser/Flow
 */
