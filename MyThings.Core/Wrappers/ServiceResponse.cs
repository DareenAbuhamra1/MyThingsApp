namespace MyThings.Core.Wrappers;

public class ServiceResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message {get;set;} = string.Empty;

    public static ServiceResponse<T> Ok(T data) => 
        new ServiceResponse<T> { Data = data , StatusCode = 200, Success =true};

    public static ServiceResponse<T> Failure(string message, int statusCode = 400) => 
        new ServiceResponse<T> { Success = false, Message = message, StatusCode = statusCode };

}