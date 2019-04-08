using System.Collections.Generic;
using MyFirstSite.Domain.Entities;

namespace MyFirstSite.WebUI.Models{

    public class ProductListViewModel{
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}