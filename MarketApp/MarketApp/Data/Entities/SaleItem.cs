using MarketApp.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketApp.Data.Entities
{
    public class SaleItem: BaseEntity
    {
        private static int _count = 0;

        public Product Product { get; set; }

        public int SaleCount { get; set; }

        public SaleItem()
        {
            _count++;

            No = _count;
        }
    }
}
