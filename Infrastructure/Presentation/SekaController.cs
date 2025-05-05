using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Presentation;

public class SekaController : Controller
{
    private readonly ILogger<SekaController> _logger;

    public SekaController(ILogger<SekaController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return Content("dskhflkshdflkhsdlkf");
    }

}
