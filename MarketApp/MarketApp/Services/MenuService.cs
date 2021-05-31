using ConsoleTables;
using MarketApp.Data.Entities;
using MarketApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketApp.Services
{
    public class MenuService
    {
        static MarkettingService markettingService = new MarkettingService();

        #region Display Tables

        public static void DisplayProducts()
        {
            if (markettingService.HasProductInDepot())
            {
                var table = new ConsoleTable("Kod", "Ad", "Kateqoriya", "Say", "Qiymet");

                foreach (var product in markettingService.Products)
                {
                    string productCategory = "";
                    switch (product.Category)
                    {
                        case ProductCategories.Meat:
                            productCategory = "Et mehsullari";
                            break;
                        case ProductCategories.Bakery:
                            productCategory = "Corek mehsullari";
                            break;
                        case ProductCategories.FreshFood:
                            productCategory = "Meyve-terevez";
                            break;
                        case ProductCategories.CleaningProduct:
                            productCategory = "Temizlik mehsullari";
                            break;
                        case ProductCategories.DairyProducts:
                            productCategory = "Sud mehsullari";
                            break;
                        default:
                            break;
                    }
                    table.AddRow(product.Code, product.Name, productCategory, product.Count, product.Price.ToString("0.00"));
                }
                table.Write();
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("  Anbarda mehsul yoxdur!");
            }
        }

        public static void DisplaySales()
        {

            if (markettingService.HasSaleInDepot())
            {
                var table = new ConsoleTable("Nomre", "Mebleg", "Mehsul sayi", "Tarix");

                foreach (var sale in markettingService.Sales)
                {
                    table.AddRow(sale.No.ToString(), sale.TotalPrice.ToString("0.00"), sale.SaleItems.Count.ToString(), sale.SaleDate.ToString("dd.mm.yyyy HH:mm"));
                }
                table.Write();
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("  Satis yoxdur!");
            }
        }

        public static void DisplayProductsByCategory()
        {
            if (markettingService.HasProductInDepot())
            {
                int categorySelection;
                do
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Kateqoriyalardan birini secin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine(" 1. Et mehsullari");
                    Console.WriteLine(" 2. Corek mehsullari");
                    Console.WriteLine(" 3. Meyve-terevez");
                    Console.WriteLine(" 4. Temizlik mehsullari");
                    Console.WriteLine(" 5. Sud mehsullari");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); int.TryParse(Console.ReadLine(), out categorySelection);
                    if (!(categorySelection > 0 && categorySelection <= 5))
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Duzgun secim edilmedi!");
                    }

                } while (!(categorySelection > 0 && categorySelection <= 5));

                var table = new ConsoleTable("Kod", "Ad", "Kateqoriya", "Say", "Qiymet");

                foreach (var product in markettingService.ProductsByCategories((ProductCategories)categorySelection))
                {
                    string productCategory = "";
                    switch (product.Category)
                    {
                        case ProductCategories.Meat:
                            productCategory = "Et mehsullari";
                            break;
                        case ProductCategories.Bakery:
                            productCategory = "Corek mehsullari";
                            break;
                        case ProductCategories.FreshFood:
                            productCategory = "Meyve-terevez";
                            break;
                        case ProductCategories.CleaningProduct:
                            productCategory = "Temizlik mehsullari";
                            break;
                        case ProductCategories.DairyProducts:
                            productCategory = "Sud mehsullari";
                            break;
                        default:
                            break;
                    }
                    table.AddRow(product.Code, product.Name, productCategory, product.Count, product.Price.ToString("0.00"));
                }
                table.Write();
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("  Anbarda mehsul yoxdur!");
            }
        }

        public static void DisplayProductsByRangeOfPrice()
        {            
            if (markettingService.HasProductInDepot())
            {
                bool result;
                double minPrice;
                double maxPrice;

                do
                {
                    do
                    {
                        Console.WriteLine("================================================================");
                        Console.WriteLine("  Mehsulun minimum qiymetini daxil edin: ");
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.Write(" "); result = double.TryParse(Console.ReadLine(), out minPrice);


                        if (!result)
                        {
                            Console.WriteLine("----------------------------------------------------------------");
                            Console.WriteLine("  Mehsulun minimum qiymeti herf ve ya bosluq ola bilmez!");
                        }
                        else if (minPrice < 0)
                        {
                            Console.WriteLine("----------------------------------------------------------------");
                            Console.WriteLine("  Mehsulun minimum qiymeti menfi eded ola bilmez!");
                        }

                    } while (!result || minPrice < 0);


                    do
                    {
                        Console.WriteLine("================================================================");
                        Console.WriteLine("  Mehsulun maksimum qiymetini daxil edin: ");
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.Write(" "); double.TryParse(Console.ReadLine(), out maxPrice);

                        if (maxPrice <= 0)
                        {
                            Console.WriteLine("----------------------------------------------------------------");
                            Console.WriteLine("  Mehsulun maksimum qiymeti herf, 0 ve ya menfi eded ola bilmez!");
                        }

                    } while (maxPrice <= 0);

                    if (minPrice > maxPrice)
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Minimum qiymet maksimumdan boyuk ola bilmez!");
                    }

                } while (minPrice > maxPrice);

                var table = new ConsoleTable("Kod", "Ad", "Kateqoriya", "Say", "Qiymet");

                foreach (var product in markettingService.ProductsByRangeOfPrice(minPrice, maxPrice))
                {
                    string productCategory = "";
                    switch (product.Category)
                    {
                        case ProductCategories.Meat:
                            productCategory = "Et mehsullari";
                            break;
                        case ProductCategories.Bakery:
                            productCategory = "Corek mehsullari";
                            break;
                        case ProductCategories.FreshFood:
                            productCategory = "Meyve-terevez";
                            break;
                        case ProductCategories.CleaningProduct:
                            productCategory = "Temizlik mehsullari";
                            break;
                        case ProductCategories.DairyProducts:
                            productCategory = "Sud mehsullari";
                            break;
                        default:
                            break;
                    }
                    table.AddRow(product.Code, product.Name, productCategory, product.Count, product.Price.ToString("0.00"));
                }
                table.Write();
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("  Anbarda mehsul yoxdur!");
            }            
        }

        public static void DisplaySearchProductsByName()
        {
            if (markettingService.HasProductInDepot())
            {
                string searchStr;
                do
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Mehsulun adini daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); searchStr = Console.ReadLine();

                     if (string.IsNullOrEmpty(searchStr) || string.IsNullOrWhiteSpace(searchStr))
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Mehsulun adi bos ola bilmez!");
                    }

                } while ( string.IsNullOrEmpty(searchStr) || string.IsNullOrWhiteSpace(searchStr));

                var table = new ConsoleTable("Kod", "Ad", "Kateqoriya", "Say", "Qiymet");

                foreach (var product in markettingService.SearchProductsByName(searchStr))
                {
                    string productCategory = "";
                    switch (product.Category)
                    {
                        case ProductCategories.Meat:
                            productCategory = "Et mehsullari";
                            break;
                        case ProductCategories.Bakery:
                            productCategory = "Corek mehsullari";
                            break;
                        case ProductCategories.FreshFood:
                            productCategory = "Meyve-terevez";
                            break;
                        case ProductCategories.CleaningProduct:
                            productCategory = "Temizlik mehsullari";
                            break;
                        case ProductCategories.DairyProducts:
                            productCategory = "Sud mehsullari";
                            break;
                        default:
                            break;
                    }
                    table.AddRow(product.Code, product.Name, productCategory, product.Count, product.Price.ToString("0.00"));
                }
                table.Write();
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("  Anbarda mehsul yoxdur!");
            }
        }

        public static void DisplaySalesByRangeOfDate()
        {
            if (markettingService.HasSaleInDepot())
            {
                DateTime startDate;
                bool startResult;
                DateTime endDate;
                bool endResult;

                do
                {
                    do
                    {
                        Console.WriteLine("================================================================");
                        Console.WriteLine("  Baslama tarixini gun-ay-il seklinde daxil edin: ");
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.Write(" "); startResult = DateTime.TryParse(Console.ReadLine(), out startDate);

                        if (!startResult)
                        {
                            Console.WriteLine("----------------------------------------------------------------");
                            Console.WriteLine("  Tarix duzgun qeyd edilmeyib!");
                        }

                    } while (!startResult);


                    do
                    {
                        Console.WriteLine("================================================================");
                        Console.WriteLine("  Sonlama tarixini gun-ay-il seklinde daxil edin: ");
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.Write(" "); endResult = DateTime.TryParse(Console.ReadLine(), out endDate);

                        if (!endResult)
                        {
                            Console.WriteLine("----------------------------------------------------------------");
                            Console.WriteLine("  Tarix duzgun qeyd edilmeyib!");
                        }

                    } while (!endResult);

                    if (startDate > endDate)
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Baslama tarixi sonlanma tarixinden boyuk ola bilmez!");
                    }
                } while (startDate > endDate);

                if (markettingService.HasSalesRangeOfDate(startDate, endDate))
                {
                    var table = new ConsoleTable("Nomre", "Mebleg", "Mehsul sayi", "Tarix");

                    foreach (var sale in markettingService.SalesByRangeOfDate(startDate,endDate))
                    {
                        table.AddRow(sale.No.ToString(), sale.TotalPrice.ToString("0.00"), sale.SaleItems.Count.ToString(), sale.SaleDate.ToString("dd.mm.yyyy HH:mm"));
                    }
                    table.Write();
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine($"  Bu tarix araliginda satis tapilmadi");
                    Console.WriteLine("----------------------------------------------------------------");
                }
                
            }
            else
            {
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("  Satis yoxdur!");
            }
        }

        public static void DisplaySalesByRangeOfTotalPrice()
        {
            if (markettingService.HasSaleInDepot())
            {
                bool result;
                double minTotalPrice;
                double maxTotalPrice;

                do
                {
                    do
                    {
                        Console.WriteLine("================================================================");
                        Console.WriteLine("  Umumi meblegin minimum qiymetini daxil edin: ");
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.Write(" "); result = double.TryParse(Console.ReadLine(), out minTotalPrice);


                        if (!result)
                        {
                            Console.WriteLine("----------------------------------------------------------------");
                            Console.WriteLine("  Meblegin minimum qiymeti herf ve ya bosluq ola bilmez!");
                        }
                        else if (minTotalPrice < 0)
                        {
                            Console.WriteLine("----------------------------------------------------------------");
                            Console.WriteLine("  Meblegin minimum qiymeti menfi eded ola bilmez!");
                        }

                    } while (!result || minTotalPrice < 0);


                    do
                    {
                        Console.WriteLine("================================================================");
                        Console.WriteLine("  Umumi meblegin maksimum qiymetini daxil edin: ");
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.Write(" "); double.TryParse(Console.ReadLine(), out maxTotalPrice);

                        if (maxTotalPrice <= 0)
                        {
                            Console.WriteLine("----------------------------------------------------------------");
                            Console.WriteLine("  Meblegin maksimum qiymeti herf, 0 ve ya menfi eded ola bilmez!");
                        }

                    } while (maxTotalPrice <= 0);

                    if (minTotalPrice > maxTotalPrice)
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Minimum qiymet maksimumdan boyuk ola bilmez!");
                    }

                } while (minTotalPrice > maxTotalPrice);

                if (markettingService.HasSalesRangeOfTotalPrice(minTotalPrice,maxTotalPrice))
                {
                    Console.WriteLine(markettingService.HasSalesRangeOfTotalPrice(minTotalPrice, maxTotalPrice));
                    var table = new ConsoleTable("Nomre", "Mebleg", "Mehsul sayi", "Tarix");

                    foreach (var sale in markettingService.SalesByRangeOfTotalPrice(minTotalPrice, maxTotalPrice))
                    {
                        table.AddRow(sale.No.ToString(), sale.TotalPrice.ToString("0.00"), sale.SaleItems.Count.ToString(), sale.SaleDate.ToString("dd.mm.yyyy HH:mm"));
                    }
                    table.Write();
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine($"  Bu mebleg araliginda satis tapilmadi");
                    Console.WriteLine("----------------------------------------------------------------");
                }
            }
            else
            {
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("  Satis yoxdur!");
            }
        }

        public static void DisplaySalesByDate()
        {
            if (markettingService.HasSaleInDepot())
            {
                DateTime date;
                bool result;
                do
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Axtarilan tarixi gun-ay-il seklinde daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); result = DateTime.TryParse(Console.ReadLine(), out date);

                    if (!result)
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Tarix duzgun qeyd edilmeyib!");
                    }

                } while (!result);

                if (markettingService.HasSalesByDate(date))
                {
                    var table = new ConsoleTable("Nomre", "Mebleg", "Mehsul sayi", "Tarix");

                    foreach (var sale in markettingService.SalesByDate(date))
                    {
                        table.AddRow(sale.No.ToString(), sale.TotalPrice.ToString("0.00"), sale.SaleItems.Count.ToString(), sale.SaleDate.ToString("dd.mm.yyyy HH:mm"));
                    }
                    table.Write();
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine($"  Verilmis tarixde satis tapilmadi");
                    Console.WriteLine("----------------------------------------------------------------");
                }
            }
            else
            {
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("  Satis yoxdur!");
            }
        }

        public static void DisplaySaleByNo()
        {
            if (markettingService.HasSaleInDepot())
            {
                int saleNo;
                int saleIndex;

                do
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Gosterilecek satisin nomresini daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); int.TryParse(Console.ReadLine(), out saleNo);
                    saleIndex = markettingService.FindSaleIndex(saleNo);
                    if (saleIndex == -1)
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Bu nomreli satis tapilmadi ve ya duzgun yazilmayib!");
                        saleNo = 0;
                    }

                } while (saleNo <= 0);

                Sale sale = markettingService.SaleByNo(saleNo);

                var tableSale = new ConsoleTable("Nomre", "Mebleg", "Mehsul sayi", "Tarix");
                tableSale.AddRow(sale.No.ToString(), sale.TotalPrice.ToString("0.00"), sale.SaleItems.Count.ToString(), sale.SaleDate.ToString("dd.mm.yyyy HH:mm"));
                tableSale.Write();
                Console.WriteLine();
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("--------------------     Mehsullar    --------------------------");
                Console.WriteLine("----------------------------------------------------------------");
                var tableSaleItem = new ConsoleTable("Nomre", "Mehsulun adi", "Mehsulun miqdari");
                foreach (var saleI in sale.SaleItems)
                {
                    tableSaleItem.AddRow(saleI.No.ToString(), saleI.Product.Name, saleI.SaleCount.ToString());
                }
                tableSaleItem.Write();
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("  Satis yoxdur!");
            }
        }

        #endregion

        #region Add Menus

        public static void AddProductMenu()
        {
            string name;
            int indexProduct;
            do
            {
                Console.WriteLine("================================================================");
                Console.WriteLine("  Mehsulun adini daxil edin: ");
                Console.WriteLine("----------------------------------------------------------------");
                Console.Write(" "); name = Console.ReadLine();


                indexProduct = markettingService.FindProductIndex(name);

                if (indexProduct != -1)
                {
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine($"  {name} adli mehsul movcuddur!");
                }else if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine("  Mehsulun adi bos ola bilmez!");
                }
                
            } while (indexProduct != -1 || string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name));
            

            int categorySelection;
            do
            {
                Console.WriteLine("================================================================");
                Console.WriteLine("  Kateqoriyalardan birini secin: ");
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine(" 1. Et mehsullari");
                Console.WriteLine(" 2. Corek mehsullari");
                Console.WriteLine(" 3. Meyve-terevez");
                Console.WriteLine(" 4. Temizlik mehsullari");
                Console.WriteLine(" 5. Sud mehsullari");
                Console.WriteLine("----------------------------------------------------------------");
                Console.Write(" "); int.TryParse(Console.ReadLine(), out categorySelection);
                if (!(categorySelection > 0 && categorySelection <= 5))
                {
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine("  Duzgun secim edilmedi!");
                }

            } while (!(categorySelection>0&&categorySelection<=5));

            int count;
            do
            {
                Console.WriteLine("================================================================");
                Console.WriteLine("  Mehsulun sayini daxil edin: ");
                Console.WriteLine("----------------------------------------------------------------");
                Console.Write(" "); int.TryParse(Console.ReadLine(), out count);

                if (count <= 0)
                {
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine("  Mehsulun sayi herf, 0 ve ya menfi eded ola bilmez!");
                }

            } while (count <= 0);

            double price;
            do
            {
                Console.WriteLine("================================================================");
                Console.WriteLine("  Mehsulun qiymetini daxil edin: ");
                Console.WriteLine("----------------------------------------------------------------");
                Console.Write(" "); double.TryParse(Console.ReadLine(), out price);

                if (price <= 0)
                {
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine("  Mehsulun qiymeti herf, 0 ve ya menfi eded ola bilmez!");
                }

            } while (price <= 0);

            try
            {
                markettingService.AddProduct(name, price, count, (ProductCategories)categorySelection);
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine($"  {name} adli mehsul daxil edildi");
                Console.WriteLine("----------------------------------------------------------------");
            }
            catch (Exception e)
            {
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("  Yeniden daxil edin!");
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine(e.Message);
            }
        }

        public static void AddSaleMenu()
        {
            if (markettingService.HasProductInDepot())
            {
                int selection = 0;

                do
                {
                    int codeForSale;
                    int indexProduct;
                    do
                    {
                        Console.WriteLine("================================================================");
                        Console.WriteLine("  Satilacaq mehsulun kodunu daxil edin: ");
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.Write(" "); int.TryParse(Console.ReadLine(), out codeForSale);
                        indexProduct = markettingService.FindProductIndex(codeForSale);
                        if (indexProduct == -1)
                        {
                            Console.WriteLine("----------------------------------------------------------------");
                            Console.WriteLine("  Mehsul tapilmadi, kod duzgun deyil!");
                            codeForSale = 0;
                        }

                    } while (codeForSale <= 0);

                    int countProduct = markettingService.GetCountOfProductForSale(indexProduct);

                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine($"  {markettingService.GetProductNameByIndex(indexProduct)} adli mehsuldan anbarda {countProduct} eded var");

                    int saleCount;
                    do
                    {
                        Console.WriteLine("================================================================");
                        Console.WriteLine("  Satis miqdarini daxil edin: ");
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.Write(" "); int.TryParse(Console.ReadLine(), out saleCount);
                        if (saleCount <= 0)
                        {
                            Console.WriteLine("----------------------------------------------------------------");
                            Console.WriteLine("  Satis miqdari herf, 0 ve ya menfi eded ola bilmez!");
                        }
                        else if (!(countProduct - saleCount >= 0))
                        {
                            Console.WriteLine("----------------------------------------------------------------");
                            Console.WriteLine($"  Miqdar {countProduct} eded ve ya bundan az ola biler");
                            saleCount = 0;
                        }

                    } while (saleCount <= 0);

                    markettingService.AddSaleItems(codeForSale, saleCount);
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine("  Satis elave olundu ve ya deyisdirildi");
                    Console.WriteLine("----------------------------------------------------------------");

                    int result = 0; ;
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Movcud satis uzre emeliyyatlardan birini secin");
                    do
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine(" 1. Mehsul elave et");
                        Console.WriteLine(" 2. Satis bitdi");
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.Write(" "); int.TryParse(Console.ReadLine(), out result);
                        if (result <= 0 || result > 2)
                        {
                            Console.WriteLine("----------------------------------------------------------------");
                            Console.WriteLine("  Duzgun secim edilmedi!");
                        }
                        else
                        {
                            switch (result)
                            {
                                case 1:
                                    selection = 1;
                                    break;
                                case 2:
                                    selection = 0;
                                    break;
                                default:
                                    break;
                            }
                        }

                    } while (result <= 0 || result > 2);

                } while (selection == 1);

                markettingService.AddSale();
            }
            else
            {
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("  Anbarda mehsul yoxdur!");
            }
        }

        #endregion

        #region Delete Menus

        public static void DeleteProductMenu()
        {
            if (markettingService.HasProductInDepot())
            {
                int codeForDelete;
                do
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Silinecek mehsulun kodunu daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); int.TryParse(Console.ReadLine(), out codeForDelete);

                    if (markettingService.FindProductIndex(codeForDelete) == -1)
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Mehsul tapilmadi, kod duzgun deyil!");
                        codeForDelete = 0;
                    }

                } while (codeForDelete <= 0);

                markettingService.DeleteProduct(codeForDelete);
            }
            else
            {
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("  Anbarda mehsul yoxdur!");
            }
        }

        public static void DeleteSaleMenu()
        {
            if (markettingService.HasSaleInDepot())
            {
                int saleNo;
                int saleIndex;

                do
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Silinecek satisin nomresini daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); int.TryParse(Console.ReadLine(), out saleNo);
                    saleIndex = markettingService.FindSaleIndex(saleNo);
                    if (saleIndex == -1)
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Bu nomreli satis tapilmadi ve ya duzgun yazilmayib!");
                        saleNo = 0;
                    }

                } while (saleNo <= 0);

                markettingService.DeleteSale(saleNo);

                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine($"  Satis silindi");
                Console.WriteLine("----------------------------------------------------------------");
            }
            else
            {
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("  Satis yoxdur!");
            }
        }

        #endregion

        #region Edit Menus

        public static void EditProduct()
        {
            if (markettingService.HasProductInDepot())
            {
                int codeForFind;
                do
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Deyisdirilecek mehsulun kodunu daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); int.TryParse(Console.ReadLine(), out codeForFind);
                    if (markettingService.FindProductIndex(codeForFind) == -1)
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Mehsul tapilmadi, kod duzgun deyil!");
                        codeForFind = 0;
                    }

                } while (codeForFind <= 0);

                string newName;
                do
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Mehsulun yeni adini daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); newName = Console.ReadLine();
                    if (string.IsNullOrEmpty(newName) || string.IsNullOrWhiteSpace(newName))
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Mehsulun adi bos ola bilmez!");
                    }

                } while (string.IsNullOrEmpty(newName) || string.IsNullOrWhiteSpace(newName));

                int newCategorySelection;
                do
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Kateqoriyalardan birini secin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine(" 1. Et mehsullari");
                    Console.WriteLine(" 2. Corek mehsullari");
                    Console.WriteLine(" 3. Meyve-terevez");
                    Console.WriteLine(" 4. Temizlik mehsullari");
                    Console.WriteLine(" 5. Sud mehsullari");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); int.TryParse(Console.ReadLine(), out newCategorySelection);
                    if (!(newCategorySelection > 0 && newCategorySelection <= 5))
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Duzgun secim edilmedi!");
                    }

                } while (!(newCategorySelection > 0 && newCategorySelection <= 5));

                int newCount;
                do
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Mehsulun yeni sayini daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); int.TryParse(Console.ReadLine(), out newCount);
                    if (newCount <= 0)
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Mehsulun sayi herf, 0 ve ya menfi eded ola bilmez!");
                    }

                } while (newCount <= 0);

                double newPrice;
                do
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Mehsulun yeni qiymetini daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); double.TryParse(Console.ReadLine(), out newPrice);
                    if (newPrice <= 0)
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Mehsulun qiymeti herf, 0 ve ya menfi eded ola bilmez!");
                    }

                } while (newPrice <= 0);

                markettingService.EditProduct(codeForFind, newName, newPrice, newCount, (ProductCategories)newCategorySelection);
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine($"  Mehsul yenilendi");
                Console.WriteLine("----------------------------------------------------------------");
            }
            else
            {
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("  Anbarda mehsul yoxdur!");
            }
        }

        public static void ReturnProductFromSale()
        {
            if (markettingService.HasSaleInDepot())
            {
                int saleNo;
                int saleIndex;
                int saleItemIndex;

                do
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Deyisdirilecek satisin nomresini daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); int.TryParse(Console.ReadLine(), out saleNo);
                    saleIndex = markettingService.FindSaleIndex(saleNo);
                    if (saleIndex == -1)
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Bu nomreli satis tapilmadi ve ya duzgun yazilmayib!");
                        saleNo = 0;
                    }

                } while (saleNo <= 0);

                int productCode;
                do
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Qaytarilacaq mehsulun kodunu daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); int.TryParse(Console.ReadLine(), out productCode);
                    saleItemIndex = markettingService.FindSaleItemIndex(saleIndex, productCode);
                    if (productCode <= 0)
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Mehsulun kodu herf, 0 ve ya menfi eded ola bilmez!");
                    }else if (saleItemIndex == -1)
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Bu kodda mehsul satisda yoxdur");
                        return;
                    }

                } while (productCode <= 0);

                int saleCountOfSaleItem = markettingService.GetSaleCountOfSaleItem(saleIndex, saleItemIndex);
                int returnCount;
                do
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Qaytarilacaq mehsulun sayini daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); int.TryParse(Console.ReadLine(), out returnCount);
                    if (returnCount <= 0)
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Mehsulun sayi herf, 0 ve ya menfi eded ola bilmez!");
                    }
                    else if (saleCountOfSaleItem < returnCount)
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Qaytarilmaya daxil edilen miqdar satisdaki miqdardan coxdur!");
                        returnCount = 0;
                    }

                } while (returnCount <= 0);

                markettingService.ReturnProductFromSale(saleNo, productCode, returnCount);

                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine($"  Mehsul satisdan geri qaytarildi");
                Console.WriteLine("----------------------------------------------------------------");

            }
            else
            {
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("  Satis yoxdu!");
            }
        }

        #endregion

    }
}
