using System;
namespace KoperasiTentera.Application.DTOs;

public class GenericResponse<T>
{
    public GenericResponse(string message, T t)
    {
        ResponseMessage = message;
        ResposneObj = t;
    }
    public T ResposneObj { get; set; }
    public string ResponseMessage { get; set; }
}

