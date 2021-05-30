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
            var table = new ConsoleTable("Kod", "Ad", "Kateqoriya", "Say", "Qiymet");

            foreach (var product in markettingService.Products)
            {
                string productCategory ="";
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

        public static void DisplaySales()
        {
            var table = new ConsoleTable("Nomre", "Mebleg", "Mehsul sayi", "Tarix");

            foreach (var sale in markettingService.Sales)
            {
                table.AddRow(sale.No.ToString(), sale.TotalPrice.ToString("0.00"), sale.SaleItems.Count.ToString(), sale.SaleDate.ToString("dd.mm.yyyy HH:mm"));
            }
            table.Write();
            Console.WriteLine();
        }

        #endregion

        #region Add Menus

        public static void AddProductMenu()
        {
            string name="";
            do
            {
                try
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Mehsulun adini daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); name = Console.ReadLine();
                    if (string.IsNullOrEmpty(name)||string.IsNullOrWhiteSpace(name))
                    {
                        throw new ArgumentNullException();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine("  Mehsulun adi bos ola bilmez!");
                }
                
            } while (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name));
            

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
                        
            int count = 0;
            do
            {
                try
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Mehsulun sayini daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); int.TryParse(Console.ReadLine(), out count);
                    if (count<=0)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                catch(ArgumentOutOfRangeException)
                {
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine("  Mehsulun sayi herf, 0 ve ya menfi eded ola bilmez!");
                }

            } while (count <= 0);

            double price = 0;
            do
            {
                try
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Mehsulun qiymetini daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); double.TryParse(Console.ReadLine(), out price);
                    if (price <= 0)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine("  Mehsulun qiymeti herf, 0 ve ya menfi eded ola bilmez!");
                }

            } while (price <= 0);

            try
            {
                markettingService.AddProduct(name, price, count, (ProductCategories)categorySelection);
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine($"  {name} adli mehsul daxil edildi ve ya deyisdirildi");
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
            int selection = 0;

            do
            {
                int saleParametrsWhile;
                do
                {
                    int codeForSale = 0;
                    do
                    {
                        try
                        {
                            Console.WriteLine("================================================================");
                            Console.WriteLine("  Satilacaq mehsulun kodunu daxil edin: ");
                            Console.WriteLine("----------------------------------------------------------------");
                            Console.Write(" "); int.TryParse(Console.ReadLine(), out codeForSale);
                            if (markettingService.FindProductIndexByCode(codeForSale) == -1)
                            {
                                codeForSale = 0;
                                throw new KeyNotFoundException();
                            }
                        }
                        catch (KeyNotFoundException)
                        {
                            Console.WriteLine("----------------------------------------------------------------");
                            Console.WriteLine("  Mehsul tapilmadi, kod duzgun deyil!");
                        }

                    } while (codeForSale <= 0);

                    int indexProduct = markettingService.FindProductIndexByCode(codeForSale);
                    int countProduct = markettingService.CountOfProductForSale(indexProduct);

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
                            Console.WriteLine($"  Anbarda {countProduct} eded mehsul qalib");
                            saleCount = 0;
                        }

                    } while (saleCount <= 0);

                    try
                    {
                        markettingService.AddSaleItems(codeForSale, saleCount);
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Satis elave olundu ve ya deyisdirildi");
                        Console.WriteLine("----------------------------------------------------------------");
                        saleParametrsWhile = 1;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Satis parametrlerini duzgun daxil edin!");
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine(e.Message);
                        saleParametrsWhile = 0;
                    }
                } while (saleParametrsWhile == 0);

                int result = 0; ;
                Console.WriteLine("================================================================");
                Console.WriteLine("  Movcud satis uzre emeliyyatlardan birini secin");
                do
                {
                    try
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine(" 1. Mehsul elave et");
                        Console.WriteLine(" 2. Satis bitdi");
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.Write(" "); int.TryParse(Console.ReadLine(), out result);
                        if (result <= 0 || result>2)
                        {
                            throw new ArgumentOutOfRangeException();
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
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("----------------------------------------------------------------");
                        Console.WriteLine("  Duzgun secim edilmedi!");
                    }
                    
                } while (result <= 0 || result > 2);

            } while (selection==1);

            markettingService.AddSale();
        }

        #endregion

        #region Delete Menus

        public static void DeleteProductMenu()
        {
            int codeForDelete = 0;
            do
            {
                try
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Silinecek mehsulun kodunu daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); int.TryParse(Console.ReadLine(), out codeForDelete);
                    if (markettingService.FindProductIndexByCode(codeForDelete) == -1)
                    {
                        codeForDelete = 0;
                        throw new KeyNotFoundException();
                    }
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine("  Mehsul tapilmadi, kod duzgun deyil!");
                }

            } while (codeForDelete <= 0);

            markettingService.DeleteProduct(codeForDelete);
        }

        #endregion

        #region Edit Menus

        public static void EditProduct()
        {
            int codeForFind = 0;
            do
            {
                try
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Deyisdirilecek mehsulun kodunu daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); int.TryParse(Console.ReadLine(), out codeForFind);
                    if (markettingService.FindProductIndexByCode(codeForFind) == -1)
                    {
                        codeForFind = 0;
                        throw new KeyNotFoundException();
                    }
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine("  Mehsul tapilmadi, kod duzgun deyil!");
                }

            } while (codeForFind <= 0);

            string newName = "";
            do
            {
                try
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Mehsulun yeni adini daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); newName = Console.ReadLine();
                    if (string.IsNullOrEmpty(newName) || string.IsNullOrWhiteSpace(newName))
                    {
                        throw new ArgumentNullException();
                    }
                }
                catch (Exception)
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

            int newCount = 0;
            do
            {
                try
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Mehsulun yeni sayini daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); int.TryParse(Console.ReadLine(), out newCount);
                    if (newCount <= 0)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine("  Mehsulun sayi herf, 0 ve ya menfi eded ola bilmez!");
                }

            } while (newCount <= 0);

            double newPrice = 0;
            do
            {
                try
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("  Mehsulun yeni qiymetini daxil edin: ");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(" "); double.TryParse(Console.ReadLine(), out newPrice);
                    if (newPrice <= 0)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine("  Mehsulun qiymeti herf, 0 ve ya menfi eded ola bilmez!");
                }

            } while (newPrice <= 0);

            try
            {
                markettingService.EditProduct(codeForFind, newName, newPrice, newCount, (ProductCategories)newCategorySelection);
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine($"  Mehsul yenilendi");
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

        #endregion

    }
}
