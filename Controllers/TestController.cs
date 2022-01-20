using Microsoft.AspNetCore.Mvc;

namespace StreamWriterConfusion.Controllers;

[Route("api/[Controller]/[Action]")]
public class TestController : ControllerBase
{
    // http://localhost:5105/api/test/noDispose
    // This case works
    [HttpGet]
    [Produces("text/plain")]
    public async Task NoDispose()
    {
        var sw = new StreamWriter(Response.Body, leaveOpen: true);

        await sw.WriteAsync("Some normal text");
        await sw.FlushAsync();
    }

    // http://localhost:5105/api/test/normalDispose
    // THIS IS THE CASE THAT DOESN'T WORK AS EXPECTED
    [HttpGet]
    [Produces("text/plain")]
    public async Task NormalDispose()
    {
        var sw = new StreamWriter(Response.Body, leaveOpen: true);

        await sw.WriteAsync("Some normal text");
        await sw.FlushAsync();

        // Equivalent to `using var sw = ...`
        sw.Dispose();
    }

    // http://localhost:5105/api/test/disposeAsync
    // This case works
    [HttpGet]
    [Produces("text/plain")]
    public async Task AsyncDispose()
    {
        var sw = new StreamWriter(Response.Body, leaveOpen: true);

        await sw.WriteAsync("Some normal text");
        await sw.FlushAsync();

        // Equivalent to `await using var sw = ...`
        await sw.DisposeAsync();
    }
}
