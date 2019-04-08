using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyFirstSite.Domain.Abstract;
using MyFirstSite.WebUI.Models;

namespace MyFirstSite.WebUI.Controllers{

    public class NavController : Controller{

        private IProductRepository repository;

        public NavController(IProductRepository repositoryParam){
            repository = repositoryParam;
        }

        //панель категорий
        //метод работает с частичным представлением и выбирает все различающиеся категории
        //paramHorizontalLayout --при сжатии браузера из Layout 
        public PartialViewResult Menu(string category=null, bool horizontalLayout=false){//, bool paramHorizontalLayout=false) 
            //1 вариант---> пример использования ViewBag для подсветки кнопки в MenuHorizontal
            ViewBag.SelectedCategory = category;

            //2 вариант---> подсветка через модель представления в Menu
            SelectedCategoryModel modelCategory = new SelectedCategoryModel(){
                selectedCategory = category,
                categories = repository.Products
                    .Select(x => x.Category)
                    .Distinct()
                    .OrderBy(y => y)
            };
            //return PartialView(modelCategory); 

            //string viewName = horizontalLayout ? "MenuHorizontal" : "Menu";
            //return PartialView(viewName, modelCategory);

            return PartialView("FlexMenu", modelCategory);
        }
    }
}