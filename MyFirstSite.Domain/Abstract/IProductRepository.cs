using MyFirstSite.Domain.Entities;
using System.Collections.Generic;

namespace MyFirstSite.Domain.Abstract{

    public interface IProductRepository{

        IEnumerable<Product> Products { get; }
    }
}
