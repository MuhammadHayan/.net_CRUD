namespace MongoCrudApi.Responses;

// This class defines the STANDARD response format for ALL APIs
public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
