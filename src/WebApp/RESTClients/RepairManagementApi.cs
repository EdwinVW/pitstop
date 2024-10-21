namespace Pitstop.WebApp.RESTClients;

public class RepairManagementApi : IRepairManagementAPI
{
    private IRepairManagementAPI _restClient;

    public RepairManagementApi(IConfiguration config, HttpClient httpClient)
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
                Console.WriteLine(ex);
                return null;
            }

            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task CreateRepairOrder(CreateRepairOrder command)
    {
        await _restClient.CreateRepairOrder(command);
    }
    
    public async Task ApproveRepairOrder(Guid repairOrderId, ApproveRepairOrder command)
    {
        await _restClient.ApproveRepairOrder(repairOrderId, command);
    }

    public async Task RejectRepairOrder(Guid repairOrderId, RejectRepairOrder command)
    {
        await _restClient.RejectRepairOrder(repairOrderId, command);
    }

    public async Task<List<VehicleParts>> GetVehicleParts()
    {
        try
        {
            var result = await _restClient.GetVehicleParts();
            return result;
        }
        catch (ApiException ex)
        {
            if (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            throw;
        }
    }
}