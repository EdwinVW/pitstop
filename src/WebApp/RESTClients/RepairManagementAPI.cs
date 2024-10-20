namespace Pitstop.WebApp.RESTClients;

public class RepairManagementAPI : IRepairManagementAPI
{
    private IRepairManagementAPI _restClient;

    public RepairManagementAPI(IConfiguration config, HttpClient httpClient)
    {
        string apiHostAndPort = config.GetSection("APIServiceLocations").GetValue<string>("RepairManagementAPI");
        
        
        httpClient.BaseAddress = new Uri($"http://{apiHostAndPort}/api");
        _restClient = RestService.For<IRepairManagementAPI>(
            httpClient,
            new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer()
            });
    }

    public async Task<List<RepairOrders>> GetAllRepairOrders()
    {
        return await _restClient.GetAllRepairOrders();
    }

    public async Task<RepairOrders> GetRepairOrderById(Guid id)
    {
        try
        {
            return await _restClient.GetRepairOrderById(id);
        }
        catch (ApiException ex)
        {
            if (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                throw;
            }
        }
    }

    public async Task CreateRepairOrder(CreateRepairOrder command)
    {
        await _restClient.CreateRepairOrder(command);
    }
    
    public async Task ApproveRepairOrder(int repairOrderId, ApproveRepairOrder command)
    {
        await _restClient.ApproveRepairOrder(repairOrderId, command);
    }
    
    public async Task RejectRepairOrder(int repairOrderId, RejectRepairOrder command)
    {
        await _restClient.RejectRepairOrder(repairOrderId, command);
    }
}