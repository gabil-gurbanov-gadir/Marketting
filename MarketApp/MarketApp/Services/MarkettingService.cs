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

            int productIndex = FindProductIndex(name);

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
        public void EditProduct(int code, string newName, double newPrice, int newCount, ProductCategories newCategory)
        {
            if (code <= 0)
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

            int productIndex = FindProductIndex(code);

            if (productIndex == -1)
                throw new KeyNotFoundException();

            _products[productIndex].Name = newName;
            _products[productIndex].Price = newPrice;
            _products[productIndex].Count = newCount;
        }
        public void DeleteProduct(int code)
        {
            if (code <= 0)
                throw new ArgumentOutOfRangeException();

            int index = FindProductIndex(code);

            if (index == -1)
                throw new KeyNotFoundException();

            _products.RemoveAt(index);

        }
        public List<Product> ProductsByCategories(ProductCategories category)
        {
            List<Product> products = _products.FindAll(p => p.Category == category);

            return products;
        }
        public List<Product> ProductsByRangeOfPrice(double minPrice, double maxPrice)
        {
            if (minPrice<0)
                throw new ArgumentOutOfRangeException("minPrice");

            if (maxPrice <= 0)
                throw new ArgumentOutOfRangeException("maxPrice");

            if (minPrice > maxPrice)
                throw new ArgumentOutOfRangeException();

            List<Product> products = _products.FindAll(p=> p.Price>=minPrice&&p.Price<=maxPrice);

            return products;
        }
        public List<Product> SearchProductsByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            
            List<Product> products = _products.FindAll(p=> p.Name.ToUpper().Contains(name.ToUpper()));

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
        public void ReturnProductFromSale(int saleNo, int productCode, int returnCount)
        {
            if (saleNo <= 0)
                throw new ArgumentOutOfRangeException("saleNo");

            if (productCode <= 0)
                throw new ArgumentOutOfRangeException("codeProduct");

            if (returnCount <= 0)
                throw new ArgumentOutOfRangeException("returnCount");

            int saleIndex = FindSaleIndex(saleNo);

            if (saleIndex == -1)
                throw new KeyNotFoundException();

            int saleItemIndex = FindSaleItemIndex(saleIndex, productCode);

            if (saleItemIndex == -1)
                throw new KeyNotFoundException();

            int saleItemSaleCount = GetSaleCountOfSaleItem(saleIndex, saleItemIndex);

            if (saleItemSaleCount < returnCount)
                throw new ArgumentOutOfRangeException();

            if (saleItemSaleCount == returnCount)
                _sales[saleIndex].SaleItems.RemoveAt(saleItemIndex);

            _sales[saleIndex].SaleItems[saleItemIndex].SaleCount -= returnCount;

            _sales[saleIndex].TotalPrice -= _sales[saleIndex].SaleItems[saleItemIndex].Product.Price * returnCount;

            int productIndex = FindProductIndex(productCode);

            if (productIndex != -1)
                _products[productIndex].Count += returnCount;

        }
        public void DeleteSale(int saleNo)
        {
            if (saleNo <= 0)
                throw new ArgumentOutOfRangeException("saleNo");

            int saleIndex = FindSaleIndex(saleNo);

            if (saleIndex == -1)
                throw new KeyNotFoundException();

            int productIndex;

            foreach (var item in _sales[saleIndex].SaleItems)
            {
                productIndex = _products.FindIndex(p => p.Code == item.Product.Code);

                if (productIndex != -1)
                {
                    _products[productIndex].Count += item.SaleCount;
                }
            }

            _sales.RemoveAt(saleIndex);
        }
        public List<Sale> SalesByRangeOfDate(DateTime start, DateTime end)
        {
            if (start > end)
                throw new ArgumentOutOfRangeException();

            List<Sale> sales = _sales.FindAll(s => s.SaleDate >= start && s.SaleDate <= end);

            if (sales.Count == 0)
                throw new ArgumentNullException();

            return sales;
        }
        public List<Sale> SalesByRangeOfTotalPrice(double min, double max)
        {
            if (min <= 0)
                throw new ArgumentOutOfRangeException("min");

            if (max <= 0)
                throw new ArgumentOutOfRangeException("max");

            List<Sale> sales = _sales.FindAll(s => s.TotalPrice >= min && s.TotalPrice <= max);

            if (sales.Count == 0)
                throw new ArgumentNullException();

            return sales;
        }
        public List<Sale> SalesByDate(DateTime date)
        {
            List<Sale> sales = _sales.FindAll(s => s.SaleDate.ToString("dd-MM-yyyy") == date.ToString("dd-MM-yyyy"));

            if (sales.Count == 0)
                throw new ArgumentNullException();

            return sales;
        }
        public Sale SaleByNo(int saleNo)
        {
            if (saleNo <= 0)
                throw new ArgumentOutOfRangeException("saleNo");

            int indexSale = _sales.FindIndex(s=> s.No == saleNo);

            if (indexSale == -1)
                throw new KeyNotFoundException();

            return _sales[indexSale];
        }

        #endregion


        #region Common 

        public bool HasProductInDepot()
        {
            if (_products.Count > 0)
                return true;
            return false;
        }

        public bool HasSaleInDepot()
        {
            if (_sales.Count > 0)
                return true;
            return false;
        }

        public void AddSaleItems(int productCode, int saleCount)
        {

            if (productCode <= 0)
                throw new ArgumentOutOfRangeException("code");
            
            if (saleCount <= 0)
                throw new ArgumentOutOfRangeException("saleCount");

            int productIndex = FindProductIndex(productCode);

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

        public int FindProductIndex(int productCode)
        {
            return _products.FindIndex(p => p.Code == productCode);
        }

        public int FindProductIndex(string productName)
        {
            return _products.FindIndex(p => p.Name == productName);
        }

        public int FindSaleIndex(int saleNo)
        {
            return _sales.FindIndex(s=> s.No == saleNo);
        }

        public int FindSaleItemIndex(int saleIndex, int productCode)
        {
            return _sales[saleIndex].SaleItems.FindIndex(sI => sI.Product.Code == productCode);
        }

        public int GetSaleCountOfSaleItem(int saleIndex, int saleItemIndex)
        {
            return _sales[saleIndex].SaleItems[saleItemIndex].SaleCount;
        }

        public int GetCountOfProductForSale(int index)
        {
            return _products[index].Count;
        }

        public bool HasSalesRangeOfDate(DateTime start, DateTime end)
        {
            List<Sale> sales = _sales.FindAll(s => s.SaleDate >= start && s.SaleDate <= end);
            if (sales.Count != 0)
            {
                return true;
            }
            return false;
        }

        public bool HasSalesByDate(DateTime date)
        {
            List<Sale> sales = _sales.FindAll(s => s.SaleDate.ToString("dd-MM-yyyy") == date.ToString("dd-MM-yyyy"));

            if (sales.Count != 0)
            {
                return true;
            }
            return false;
        }

        public bool HasSalesRangeOfTotalPrice(double min, double max)
        {
            List<Sale> sales = _sales.FindAll(s => s.TotalPrice >= min && s.TotalPrice <= max);

            if (sales.Count != 0)
            {
                return true;
            }
            return false;
        }



        public string GetProductNameByIndex(int index)
        {
            return _products[index].Name;
        }

        #endregion


    }
}
