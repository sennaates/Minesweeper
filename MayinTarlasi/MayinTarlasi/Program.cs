using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayinTarlasi
{
    class Program
    {

    }
    class MayinTarlasiOyunu
    {
        static int alanBoyutu = 20; // Oyun alanı boyutu
        static int mayinSayisi = 40; // Mayın sayısı
        static char[,] tahta = new char[alanBoyutu, alanBoyutu];
        static bool[,] acildiMi = new bool[alanBoyutu, alanBoyutu];
        static bool[,] mayinlar = new bool[alanBoyutu, alanBoyutu];

        static void Main()
        {
            TahtayiBaslat();
            MayinlariYerlestir();
            OyunuOyna();
        }

        static void TahtayiBaslat()
        {
            for (int i = 0; i < alanBoyutu; i++)
            {
                for (int j = 0; j < alanBoyutu; j++)
                {
                    tahta[i, j] = '#'; // Kapalı hücreyi temsil eder
                    acildiMi[i, j] = false;
                    mayinlar[i, j] = false;
                }
            }
        }

        static void MayinlariYerlestir()
        {
            Random rastgele = new Random();
            int yerlestirilenMayinSayisi = 0;

            while (yerlestirilenMayinSayisi < mayinSayisi)
            {
                int x = rastgele.Next(alanBoyutu);
                int y = rastgele.Next(alanBoyutu);

                if (!mayinlar[x, y])
                {
                    mayinlar[x, y] = true;
                    yerlestirilenMayinSayisi++;
                }
            }
        }

        static void OyunuOyna()
        {
            bool oyunDevamEdiyor = true;

            while (oyunDevamEdiyor)
            {
                Console.Clear();
                TahtayiGoster();
                Console.WriteLine("\nBir hücre seçin (satır ve sütun): ");
                Console.Write("Satır: ");
                int satir = int.Parse(Console.ReadLine());
                Console.Write("Sütun: ");
                int sutun = int.Parse(Console.ReadLine());

                if (satir < 0 || satir >= alanBoyutu || sutun < 0 || sutun >= alanBoyutu)
                {
                    Console.WriteLine("Geçersiz giriş! Tekrar deneyin.");
                    continue;
                }

                if (mayinlar[satir, sutun])
                {
                    Console.Clear();
                    Console.WriteLine("Mayına bastınız! Oyun bitti.");
                    MayinlariGoster();
                    oyunDevamEdiyor = false;
                }
                else
                {
                    HucreyiAc(satir, sutun);
                    if (OyunKazanildiMi())
                    {
                        Console.Clear();
                        Console.WriteLine("Tebrikler! Tüm mayınları buldunuz.");
                        MayinlariGoster();
                        oyunDevamEdiyor = false;
                    }
                }
            }
        }

        static void TahtayiGoster()
        {
            Console.Write("   "); // İlk boşluk
            for (int i = 0; i < alanBoyutu; i++)
            {
                Console.Write($"{i:D2} "); // Sütun numaraları
            }
            Console.WriteLine();

            for (int i = 0; i < alanBoyutu; i++)
            {
                Console.Write($"{i:D2} "); // Satır numarası
                for (int j = 0; j < alanBoyutu; j++)
                {
                    if (acildiMi[i, j])
                    {
                        Console.Write(tahta[i, j] + "  ");
                    }
                    else
                    {
                        Console.Write("#  ");
                    }
                }
                Console.WriteLine();
            }
        }

        static void MayinlariGoster()
        {
            Console.Write("   "); // İlk boşluk
            for (int i = 0; i < alanBoyutu; i++)
            {
                Console.Write($"{i:D2} "); // Sütun numaraları
            }
            Console.WriteLine();

            for (int i = 0; i < alanBoyutu; i++)
            {
                Console.Write($"{i:D2} "); // Satır numarası
                for (int j = 0; j < alanBoyutu; j++)
                {
                    if (mayinlar[i, j])
                    {
                        Console.Write("*  ");
                    }
                    else
                    {
                        Console.Write(tahta[i, j] + "  ");
                    }
                }
                Console.WriteLine();
            }
        }

        static void HucreyiAc(int satir, int sutun)
        {
            if (satir < 0 || satir >= alanBoyutu || sutun < 0 || sutun >= alanBoyutu || acildiMi[satir, sutun])
            {
                return;
            }

            acildiMi[satir, sutun] = true;

            int cevredekiMayinSayisi = CevredekiMayinlariSay(satir, sutun);

            if (cevredekiMayinSayisi == 0)
            {
                tahta[satir, sutun] = ' ';
                CevredekiHucreleriAc(satir, sutun);
            }
            else
            {
                tahta[satir, sutun] = (char)(cevredekiMayinSayisi + '0');
            }
        }

        static void CevredekiHucreleriAc(int satir, int sutun)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    HucreyiAc(satir + i, sutun + j);
                }
            }
        }

        static int CevredekiMayinlariSay(int satir, int sutun)
        {
            int sayi = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int yeniSatir = satir + i;
                    int yeniSutun = sutun + j;

                    if (yeniSatir >= 0 && yeniSatir < alanBoyutu && yeniSutun >= 0 && yeniSutun < alanBoyutu && mayinlar[yeniSatir, yeniSutun])
                    {
                        sayi++;
                    }
                }
            }

            return sayi;
        }

        static bool OyunKazanildiMi()
        {
            for (int i = 0; i < alanBoyutu; i++)
            {
                for (int j = 0; j < alanBoyutu; j++)
                {
                    if (!mayinlar[i, j] && !acildiMi[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}


  