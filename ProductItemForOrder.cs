using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MurzayanovChudoObuv
{
    public class ProductItemForOrder
    {
        public ProductItems productItem { get; set; }
        public int itemCount { get; set; }

        public ProductItemForOrder(ProductItems ProductItem, int Count)
        {
            productItem = ProductItem;
            itemCount = Count;
        }
    }
}
