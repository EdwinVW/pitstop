namespace WebApp.RESTClients;

public class CustomerSupportAPI : ICustomerSupportAPI
{
    private ICustomerSupportAPI _restClient;
    
    public CustomerSupportAPI(IConfiguration config, HttpClient httpClient)
    {
        var apiHostAndPort = config.GetSection("APIServiceLocations").GetValue<string>("CustomerSupportAPI");
        httpClient.BaseAddress = new Uri($"http://{apiHostAndPort}/api");
        _restClient = RestService.For<ICustomerSupportAPI>(
            httpClient,
            new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer()
            });
    }

    public Task<List<Rejection>> GetRejections()
    {
        return _restClient.GetRejections();
    }
}