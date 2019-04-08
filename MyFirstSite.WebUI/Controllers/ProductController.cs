using System.Linq;
using System.Web.Mvc;
using MyFirstSite.Domain.Abstract;
using MyFirstSite.Domain.Entities;
using MyFirstSite.WebUI.Models;

namespace MyFirstSite.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        //to break on the page
        public int PageSize = 4;

        public ProductController(IProductRepository productRepository) {
            repository = productRepository;
        }

        public ViewResult List(string category, int page = 1){
            //return View(repository.Products //передаем IEnumerable в представление(констуктор MVC в представлении указал на модель Product (it needs to be corrected))
            //    .OrderBy(p => p.ProductID)
            //    .Skip((page - 1) * PageSize) //skip all what there is before current page
            //    .Take(PageSize));            //view 4 pages

            //теперь вместо IEnumerable передаем в представление экземпляр ProductListViewModel -- это нужно учесть в тестах

            ProductListViewModel model = new ProductListViewModel(){
                Products = repository.Products
                          .Where(p => category == null || p.Category == category) //category==null ---> выбрали все товары, без фильтрации
                          .OrderBy(p => p.ProductID)
                          .Skip((page - 1) * PageSize) //skip all what there is before current page
                          .Take(PageSize),             //select n-pages
                PagingInfo = new PagingInfo(){
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    //TotalItems = repository.Products.Count()
                    TotalItems= category==null ?
                                          repository.Products.Count()
                                          :repository.Products.Where(p=>p.Category==category).Count()
                },
                CurrentCategory = category
            };

            return View(model);
        }
    }
}
