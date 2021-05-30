
using MarketApp.Data.Enums;

namespace MarketApp.Data.Entities
{
    public class Product
    {
        private static int _count = 0;

        public int Code { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int Count { get; set; }

        public ProductCategories Category { get; set; }

        public Product()
        {
            _count++;

            Code = _count;
        }
    }
}
