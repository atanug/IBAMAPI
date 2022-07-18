using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IBAM.API.Helper{

public class ErrorResponse
{
    public static ObjectResult CreateResponse(
        HttpStatusCode statusCode,
        string type,
        string title = "",
        string detail = "",
        string instance = "")
    {
        var problem = new ProblemDetails()
        {
            Status = (int)statusCode,
            Type = type,
            Title = title,
            Instance = instance,
            Detail = detail
        };
 
        return new ProblemObjectResult(problem);
    }
 
    public static ObjectResult BadRequest(
        string type,
        string title = "",
        string detail = "",
        string instance = "")
    {
        return CreateResponse(HttpStatusCode.BadRequest, type, title, detail, instance);
    }
 
    public static ObjectResult InternalServerError(
        string type = "unexpected-error",
        string title = "",
        string detail = "",
        string instance = "")
    {
        return CreateResponse(HttpStatusCode.InternalServerError, type, title, detail, instance);
    }
 
    public static ObjectResult NotFound(
        string type,
        string title = "",
        string detail = "",
        string instance = "")
    {
        return CreateResponse(HttpStatusCode.NotFound, type, title, detail, instance);
    }
}
}