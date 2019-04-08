using System.Collections.Generic;
using System.Linq;

namespace MyFirstSite.Domain.Entities{

    public class CartLine {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

    public class Cart{
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Product product, int quantity) {
            CartLine line = lineCollection
                .Where(p => p.Product.ProductID == product.ProductID)
                .FirstOrDefault();

            if (line == null) {
                lineCollection.Add(new CartLine { Product=product, Quantity=quantity});
            } else {
                line.Quantity +=quantity;
            }
        }

        public void RemoveItem(Product product) {
            lineCollection.RemoveAll(p => p.Product.ProductID == product.ProductID);
        }

        public decimal ComputeTotalValue() {
            return lineCollection.Sum(p => p.Quantity*p.Product.Price);
        }

        public void Clear() {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines{
            get { return lineCollection; } 
        }
    }
}
