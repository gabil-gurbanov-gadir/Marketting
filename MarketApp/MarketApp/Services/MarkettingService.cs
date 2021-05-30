using MarketApp.Data.Common;
using MarketApp.Data.Entities;
using MarketApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketApp.Services
{
    public class MarkettingService : IMarketable
    {

        public MarkettingService()
        {
            _sales = new List<Sale>();
            _products = new List<Product>();
            _saleItems = new List<SaleItem>();
        }

        private List<Sale> _sales;
        public List<Sale> Sales => _sales;

        private List<Product> _products;
        public List<Product> Products => _products;

        private List<SaleItem> _saleItems;

        public List<SaleItem> SaleItems => _saleItems;


        #region Product Methods

        public void AddProduct(string name, double price, int count, ProductCategories category)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("nameProduct");
            }

            if (price<=0)
            {
                throw new ArgumentOutOfRangeException("priceProduct");
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("countProduct");
            }

            int productIndex = FindProductIndexByCode(name);

            if (productIndex != -1)
            {
                _products[productIndex].Price = price;
                _products[productIndex].Count = count;
                _products[productIndex].Category = category;
            }
            else
            {
                Product product = new Product
                {
                    Name = name,
                    Price = price,
                    Count = count,
                    Category = category
                };

                _products.Add(product);
            }
        }

        public void DeleteProduct(int code)
        {
            if (code <= 0)
                throw new ArgumentOutOfRangeException();

            int index = FindProductIndexByCode(code);

            if (index == -1)
                throw new KeyNotFoundException();

            _products.RemoveAt(index);

        }

        public void EditProduct(int code, string newName, double newPrice, int newCount, ProductCategories newCategory)
        {
            if (code<=0)
            {
                throw new ArgumentOutOfRangeException("code");
            }

            if (string.IsNullOrEmpty(newName))
            {
                throw new ArgumentNullException("newName");
            }

            if (newPrice <= 0)
            {
                throw new ArgumentOutOfRangeException("newPrice");
            }

            if (newCount <= 0)
            {
                throw new ArgumentOutOfRangeException("newCount");
            }

            int productIndex = FindProductIndexByCode(code);

            if (productIndex == -1)
                throw new KeyNotFoundException();

            _products[productIndex].Name = newName;
            _products[productIndex].Price = newPrice;
            _products[productIndex].Count = newCount;
        }

        public List<Product> ProductsByCategories(ProductCategories category)
        {
            List<Product> products = _products.FindAll(p => p.Category == category);

            return products;
        }

        public List<Product> ProductsByRangeOfPrice(double minPrice, double maxPrice)
        {
            if (minPrice<=0)
            {
                throw new ArgumentOutOfRangeException("minPrice");
            }

            if (maxPrice <= 0)
            {
                throw new ArgumentOutOfRangeException("maxPrice");
            }

            List<Product> products = _products.FindAll(p=> p.Price>=minPrice&&p.Price<=maxPrice);

            return products;
        }

        public List<Product> SearchProductsByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            List<Product> products = _products.FindAll(p=> p.Name.Contains(name));

            return products;
        }
        #endregion


        #region Sale Methods

        public void AddSale()
        {
            Sale sale;

            sale = new Sale
            {
                SaleDate = DateTime.Now
            };

            sale.SaleItems.AddRange(_saleItems);

            double saleTotalPrice = 0;

            foreach (var saleItem in _saleItems)
            {
                saleTotalPrice += saleItem.Product.Price * saleItem.SaleCount;
            }

            sale.TotalPrice = saleTotalPrice;

            _sales.Add(sale);

            _saleItems.Clear();

        }
        public void DeleteSale(int saleNo)
        {
            if (saleNo <= 0)
            {
                throw new ArgumentOutOfRangeException("saleNo");
            }           

            Sale sale = _sales.Find(s => s.No == saleNo);

            if (sale == null)
                throw new ArgumentNullException();

            foreach (var item in sale.SaleItems)
            {
                item.Product.Count += item.SaleCount;
            }

            _sales.Remove(sale);
        }
        public Sale SaleByNo(int saleNo)
        {
            if (saleNo <= 0)
            {
                throw new ArgumentOutOfRangeException("saleNo");
            }

            Sale sale = _sales.Find(s => s.No == saleNo);
            return sale;
        }
        
        public List<Sale> SalesByDate(DateTime date)
        {
            List<Sale> sales = _sales.FindAll(s => s.SaleDate == date);

            return sales;
        }
        public List<Sale> SalesByRangeOfDate(DateTime start, DateTime end)
        {
            List<Sale> sales = _sales.FindAll(s => s.SaleDate >= start&& s.SaleDate<=end);

            return sales;
        }
        public List<Sale> SalesByRangeOfTotalPrice(double min, double max)
        {
            if (min <= 0)
            {
                throw new ArgumentOutOfRangeException("min");
            }

            if (max <= 0)
            {
                throw new ArgumentOutOfRangeException("max");
            }

            List<Sale> sales = _sales.FindAll(s => s.TotalPrice >= min && s.TotalPrice<=max);

            return sales;
        }
        public void ReturnProductFromSale(int saleNo, int codeProduct, int returnCount)
        {
            if (saleNo <= 0)
            {
                throw new ArgumentOutOfRangeException("saleNo");
            }

            if (codeProduct <= 0)
            {
                throw new ArgumentOutOfRangeException("codeProduct");
            }

            if (returnCount <= 0)
            {
                throw new ArgumentOutOfRangeException("returnCount");
            }

            Sale sale = _sales.Find(s => s.No == saleNo);

            if (sale == null)
            {
                throw new ArgumentNullException();
            }

            SaleItem saleItem = sale.SaleItems.Find(sI=> sI.Product.Code == codeProduct);

            if (saleItem == null)
            {
                throw new ArgumentNullException();
            }

            if (saleItem.SaleCount<returnCount)
            {
                throw new ArgumentOutOfRangeException();
            }

            saleItem.SaleCount -= returnCount;
            saleItem.Product.Count += returnCount;

        }

        #endregion


        #region Common 

        public void AddSaleItems(int productCode, int saleCount)
        {

            if (productCode <= 0)
                throw new ArgumentOutOfRangeException("code");
            
            if (saleCount <= 0)
                throw new ArgumentOutOfRangeException("saleCount");

            int productIndex = FindProductIndexByCode(productCode);

            if(productIndex == -1)
                throw new KeyNotFoundException("productCode");

            if ((_products[productIndex].Count - saleCount) >= 0)
            {
                int saleItemIndex = _saleItems.FindIndex(sI => sI.Product.Name == _products[productIndex].Name);

                if (saleItemIndex == -1)
                {
                    SaleItem newSaleItem = new SaleItem
                    {
                        Product = _products[productIndex],
                        SaleCount = saleCount
                    };
                    _saleItems.Add(newSaleItem);
                }
                else
                {
                    _saleItems[saleItemIndex].SaleCount += saleCount;
                }
                _products[productIndex].Count -= saleCount;
            }
            else
            {
                throw new ArgumentOutOfRangeException("-saleCount");
            }
        }

        public int FindProductIndexByCode(int productCode)
        {
            return _products.FindIndex(p => p.Code == productCode);
        }

        public int FindProductIndexByCode(string productName)
        {
            return _products.FindIndex(p => p.Name == productName);
        }

        public int CountOfProductForSale(int index)
        {
            return _products[index].Count;
        }

        #endregion


    }
}
