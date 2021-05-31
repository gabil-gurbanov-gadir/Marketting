using MarketApp.Services;
using System;

namespace MarketApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int selection;
            Console.WriteLine("================================================================");
            Console.WriteLine("======================   Market Tedbiqi   ======================");

            do
            {
                Console.WriteLine("================================================================");
                Console.WriteLine("  Asagidaki emeliyyatlardan birini secin: ");
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("  1. Mehsullar uzerinde emeliyyat aparmaq");
                Console.WriteLine("  2. Satislar uzerinde emeliyyat aparmaq");
                Console.WriteLine("  3. Sistemden cixmaq");
                Console.WriteLine("----------------------------------------------------------------");
                Console.Write(" "); int.TryParse(Console.ReadLine(), out selection);

                switch (selection)
                {
                    case 1:
                        int productSelection;
                        do
                        {                            
                            Console.WriteLine("================================================================");
                            Console.WriteLine("     Mehsullar uzerinde emeliyyatlardan birini secin: ");
                            Console.WriteLine("----------------------------------------------------------------");
                            Console.WriteLine("  1. Yeni mehsul elave et");
                            Console.WriteLine("  2. Mehsul uzerinde duzelis et");
                            Console.WriteLine("  3. Mehsulu sil");
                            Console.WriteLine("  4. Butun mehsullari goster");
                            Console.WriteLine("  5. Kateqoriyasina gore mehsullari goster");
                            Console.WriteLine("  6. Qiymet araligina gore mehsullari goster");
                            Console.WriteLine("  7. Mehsullar arasinda ada gore axtaris et");
                            Console.WriteLine("  8. Esas menuya qayit");
                            Console.WriteLine("  9. Sistemden cix");
                            Console.WriteLine("----------------------------------------------------------------");

                            Console.Write(" "); int.TryParse(Console.ReadLine(), out productSelection);

                            switch (productSelection)
                            {
                                case 1:
                                    MenuService.AddProductMenu();
                                    break;
                                case 2:
                                    MenuService.EditProduct();
                                    break;
                                case 3:
                                    MenuService.DeleteProductMenu();
                                    break;
                                case 4:
                                    MenuService.DisplayProducts();
                                    break;
                                case 5:
                                    MenuService.DisplayProductsByCategory();
                                    break;
                                case 6:
                                    MenuService.DisplayProductsByRangeOfPrice();
                                    break;
                                case 7:
                                    MenuService.DisplaySearchProductsByName();
                                    break;
                                case 9:
                                    selection = 3;
                                    break;
                                default:
                                    break;
                            }

                        } while (productSelection != 8 && selection!=3);
                        selection = 0;
                        break;
                    case 2:
                        int saleSelection;
                        do
                        {
                            Console.WriteLine("================================================================");
                            Console.WriteLine("     Satislar uzerinde emeliyyatlardan birini secin: ");
                            Console.WriteLine("----------------------------------------------------------------");
                            Console.WriteLine("  1. Yeni satis elave et");
                            Console.WriteLine("  2. Satisdaki hansisa mehsulun geri qaytarilmasi");
                            Console.WriteLine("  3. Satisi sil");
                            Console.WriteLine("  4. Butun satislari ekrana cixar");
                            Console.WriteLine("  5. Verilen tarix araligina gore satislari goster");
                            Console.WriteLine("  6. Verilen mebleg araligina gore satislari goster");
                            Console.WriteLine("  7. Verilmis bir tarixde olan satislari goster");
                            Console.WriteLine("  8. Verilmis nomreye esasen hemin nomreli satisin melumatlarini goster");
                            Console.WriteLine("  9. Esas menuya qayit");
                            Console.WriteLine("  10. Sistemden cix");
                            Console.WriteLine("----------------------------------------------------------------");

                            Console.Write(" "); int.TryParse(Console.ReadLine(), out saleSelection);

                            switch (saleSelection)
                            {
                                case 1:
                                    MenuService.AddSaleMenu();
                                    break;
                                case 2:
                                    MenuService.ReturnProductFromSale();
                                    break;
                                case 3:
                                    MenuService.DeleteSaleMenu();
                                    break;
                                case 4:
                                    MenuService.DisplaySales();
                                    break;
                                case 5:
                                    MenuService.DisplaySalesByRangeOfDate();
                                    break;
                                case 6:
                                    MenuService.DisplaySalesByRangeOfTotalPrice();
                                    break;
                                case 7:
                                    MenuService.DisplaySalesByDate();
                                    break;
                                case 8:
                                    MenuService.DisplaySaleByNo();
                                    break;
                                case 10:
                                    selection = 3;
                                    break;                                    
                                default:
                                    break;
                            }

                        } while (saleSelection != 9 && selection != 3);
                        break;
                    default:
                        break;
                }

            } while (selection != 3);
        }
    }
}
