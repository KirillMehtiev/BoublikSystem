using System.Web;
using System.Web.Mvc;

namespace BoublikSystem
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AuthorizeAttribute()); // Для того что бы все страници требовали авторизации
            filters.Add(new HandleErrorAttribute());
        }
    }
}
