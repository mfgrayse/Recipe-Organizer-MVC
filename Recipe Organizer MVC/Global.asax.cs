//GitHub repo info: https://github.com/mfgrayse/Recipe-Organizer-MVC
//CSS examples: https://www.bettycrocker.com/recipes/ultimate-chocolate-chip-cookies/77c14e03-d8b0-4844-846d-f19304f61c57
// and http://www.delish.com/cooking/recipe-ideas/recipes/a49067/chicken-noodle-soup-casserole-recipe/
//browser size: https://www.webpagefx.com/tools/whats-my-browser-size/
//vert phone HxW: ~560x445
//horiz phone HxW: ~380x680
//preferred desktop HxW: ~835x1400 
//estimated tablet HxW: ~580x1000
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Recipe_Organizer_MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
