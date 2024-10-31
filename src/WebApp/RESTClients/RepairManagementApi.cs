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

    public async Task<List<RepairOrder>> GetAllRepairOrders()
    {
        return await _restClient.GetAllRepairOrders();
    }

    public async Task<RepairOrder> GetRepairOrderById(string id)
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

    public async Task SendRepairOrder(SendRepairOrder command)
    {
        await _restClient.SendRepairOrder(command);
    }

    public async Task ApproveRepairOrder(string repairOrderId, ApproveRepairOrder command) 
    {
        await _restClient.ApproveRepairOrder(repairOrderId, command); 
    }

    public async Task RejectRepairOrder(string repairOrderId, RejectRepairOrder command)
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
