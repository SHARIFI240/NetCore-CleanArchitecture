using Microsoft.AspNetCore.Mvc;

namespace KarAfarin.Controllers
{
    public class BaseController : Controller
    {
        protected int GetCurrentUserId()
        {
            try
            {
                return int.Parse(User.Claims
                    .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?
                    .Value!);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}
