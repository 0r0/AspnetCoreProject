using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspnetCoreProject.Models;
namespace AspnetCoreProject.ViewComponents
{
    [ViewComponent(Name ="MySimple")]
    public class MySimpleViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int total,string title)
        {
            var items = GetItems(total);
            ViewBag.CompTitle = title;
            return View(items);
        }

        private List<Book> GetItems(int total)
        {
            var List = new List<Book>();
            for(int i = 0; i < total; i++)
            {
                Book o = new Book();
                o.Id = i+1;
                o.Author = string.Format("Author {0}", i + 1);
                o.Title = string.Format("title {0}",Guid.NewGuid().ToString());

                List.Add(o);

            }
            return List;
        }

    }
}
