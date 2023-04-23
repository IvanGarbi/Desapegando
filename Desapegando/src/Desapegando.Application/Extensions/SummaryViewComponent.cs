using Microsoft.AspNetCore.Mvc;

namespace Desapegando.Application.Extensions;

public class SummaryViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}