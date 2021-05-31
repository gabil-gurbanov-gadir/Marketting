using MarketApp.Data.Entities;
using MarketApp.Data.Enums;
using System;
using System.Collections.Generic;

namespace MarketApp.Data.Common
{
    interface IMarketable
    {
        List<Sale> Sales { get; }
        List<Product> Products { get; }

        #region ProductMethods

        void AddProduct(string name, double price, int count, ProductCategories category);

        void EditProduct(int code, string newName, double newPrice, int newCount, ProductCategories newCategory);

        void DeleteProduct(int code);

        List<Product> ProductsByCategories(ProductCategories category);

        List<Product> ProductsByRangeOfPrice(double minPrice, double maxPrice);

        List<Product> SearchProductsByName(string name);

        #endregion

        #region SaleMethods

        void AddSale();

        void ReturnProductFromSale(int saleNo, int productCode, int returnCount);

        void DeleteSale(int saleNo);

        List<Sale> SalesByRangeOfDate(DateTime start, DateTime end);

        List<Sale> SalesByRangeOfTotalPrice(double min, double max);

        List<Sale> SalesByDate(DateTime date);

        Sale SaleByNo(int saleNo);

        #endregion

    }
}
