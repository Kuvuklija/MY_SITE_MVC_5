using MyFirstSite.Domain.Abstract;
using MyFirstSite.Domain.Entities;
using System.Collections.Generic;

namespace MyFirstSite.Domain.Concrete{

    public class EFProductRepository : IProductRepository{

        private EFDbContext context = new EFDbContext();

        public IEnumerable<Product> Products {
            get { return context.Products;}
        }
    }
}
