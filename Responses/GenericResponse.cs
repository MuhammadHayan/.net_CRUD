namespace MongoCrudApi.Responses;

// Generic version when API returns data
public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }
}
