using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TodoBackend.Controllers
{
    public class TodoControllerBase : ControllerBase
    {
        public string Username => HttpContext.User.Identity?.Name ?? "guest";
    }
}
