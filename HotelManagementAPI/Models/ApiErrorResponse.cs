namespace HotelManagementAPI.Models;

public class ApiErrorResponse
{
    public ApiErrorResponse()
    {
    }

    public ApiErrorResponse(string errorMessage)
    {
        Error = errorMessage;
    }
    public string Error { get; set; }
}