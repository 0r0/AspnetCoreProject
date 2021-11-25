using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace AspnetCoreProject.ViewComponents
{   [ViewComponent(Name ="MyFresh")]
    public class MyFreshViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int a, int b,string op)
        {
            int result = 0;
            if (op == "add")
            {
                result = a + b;
            }
            else if (op == "substract")
            {
                result = a - b;
            }
            ViewBag.a = a;
            ViewBag.b = b;
            ViewBag.op = op;
            ViewBag.result = result;
            return View();
        }
    }
}
