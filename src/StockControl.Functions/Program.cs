using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        // Aqui depois podemos registrar services próprios
        // usados pelas Azure Functions.
    })
    .Build();

host.Run();
