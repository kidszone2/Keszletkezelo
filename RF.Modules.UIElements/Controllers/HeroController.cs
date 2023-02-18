using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using RF.Modules.UIElements.RF.Modules.UIElements.Models;
using System.Web.Mvc;

namespace RF.Modules.UIElements.RF.Modules.UIElements.Controllers
{
    [DnnHandleError]
    public class HeroController : DnnController
    {
        // GET: Hero
        [ModuleAction(ControlKey = "UIElementHero", TitleKey = "UIElementHero")]
        public ActionResult Index() 
            => View(HeroSettings.Fetch(ModuleContext));
    }
}