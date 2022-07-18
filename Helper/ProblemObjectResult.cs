using Microsoft.AspNetCore.Mvc;

namespace IBAM.API.Helper
{

public class ProblemObjectResult : ObjectResult
{
    public ProblemObjectResult(ProblemDetails value) : base(value)
    {
        StatusCode = (int)value.Status;
    }
}
}

