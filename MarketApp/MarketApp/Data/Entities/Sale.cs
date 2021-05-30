using MarketApp.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketApp.Data.Entities
{
    public class Sale : BaseEntity
    {
        private static int _count = 0;

        public double TotalPrice { get; set; }

        public List<SaleItem> SaleItems { get; set; }

        public DateTime SaleDate { get; set; }

        public Sale()
        {
            _count++;

            No = _count;
            SaleItems = new List<SaleItem>();
        }
    }
}
