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
    //
    // You may have to try multiple times for it to fail. It seems to work 100% of the
    // time when making the request from Postman, but usually fails (with no clear 
    // error message) when visiting the URL through Chrome on Windows.
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

    // http://localhost:5105/api/test/asyncDispose
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
