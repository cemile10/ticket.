using System;
using System.Collections.Generic;
using System.IO;
using ConsoleApp28.ticket.models;

namespace ConsoleApp28.ticket.models
{
    public class UserModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
    }

    public class BiletModel
    {
        public string Qalxis { get; set; }
        public string Enis { get; set; }
        public int say { get; set; }
        public decimal nomre { get; set; }
    }
    public class Program
    {
        public static void Main()
        {
            var users = File.ReadAllLines("users.csv");
            List<UserModel> userList = new List<UserModel>();

            foreach (var line in users)
            {
                string[] values = line.Split(',');

                if (values.Length >= 4)
                {
                    UserModel user = new UserModel
                    {
                        Name = values[0],
                        Surname = values[1],
                        Password = values[2],
                        Balance = decimal.Parse(values[3])
                    };

                    userList.Add(user);
                }
            }

            var datas = File.ReadAllLines("bilet.csv");
            List<BiletModel> biletList = new List<BiletModel>();
            foreach (var line in datas)
            {
                string[] deyerler = line.Split(',');

                if (deyerler.Length >= 4)
                {
                    BiletModel bilet = new BiletModel
                    {
                        Qalxis = deyerler[0],
                        Enis = deyerler[1],
                        say = Convert.ToInt32(deyerler[2]),
                        nomre = Convert.ToDecimal(deyerler[3]),
                    };

                    biletList.Add(bilet);
                }

            }
            User istifadeci = new User();
            istifadeci.KodYoxlanis = true;
            User.AdminPaneli(userList, biletList);


        }
    }
}
public class User
{

    private static bool SuperAdmin;
    private string adminKod = "superadmin123";


    public bool KodYoxlanis
    {
        get
        {
            return SuperAdmin;

        }
        set
        {

            Console.WriteLine("Kodu daxil edin:");
            string kod = Console.ReadLine();
            if (kod == adminKod)
            {
                Console.WriteLine("Admin olaraq giris edildi");
                SuperAdmin = true;
            }
            else
            {
                SuperAdmin = false;
            }
        }
    }

    public static void AdminPaneli(List<UserModel> userList, List<BiletModel> biletList)
    {
        if (SuperAdmin)
        {
            Console.WriteLine("Balans emeliyyatlari ucun 0, Bilet emeliyyatlari ucun 1 yazin");
            int number = Convert.ToInt32(Console.ReadLine());
            if (number == 0)
            {
                Console.WriteLine("Balansinda deyisiklik etmek istediyiniz istifadecinin adini qeyd edin");
                string userName = Console.ReadLine();
                var user = userList.Find(u => u.Name == userName);
                if (user != null)
                {
                    Console.WriteLine("Artiracaginiz miqdari yazin (azaldacagsinizsa - ile)");
                    decimal amount = Convert.ToDecimal(Console.ReadLine());
                    user.Balance += amount;
                    using (StreamWriter writer = new StreamWriter("users.csv"))
                    {
                        writer.WriteLine($"{user.Name},{user.Surname},{user.Password},{user.Balance}");
                    }
                    Console.WriteLine($" {userName}'nin balansi {amount} manat artirildi. Yeni balans: {user.Balance}");
                }
                else
                {
                    Console.WriteLine($" {userName} adda istifadeci tapilmadi");
                }
            }

            else if (number == 1)
            {
                Console.WriteLine("Uçuş numarasını girin: ");
                string ucusNumarasi = Console.ReadLine();

                BiletModel seciliBilet = biletList.FirstOrDefault(b => b.nomre == Convert.ToDecimal(ucusNumarasi));

                if (seciliBilet == null)
                {
                    Console.WriteLine("Bele bir ucus tapilmadi.");
                }
                else
                {
                    Console.WriteLine($"Movcud bilet sayısı: {seciliBilet.say}");
                    Console.WriteLine("Bilet sayısını artırmaq veya azaltmagi secin:");
                    Console.WriteLine("1. Bilet sayısını artır");
                    Console.WriteLine("2. Bilet sayısını azalt");
                    int secim = Convert.ToInt32(Console.ReadLine());

                    if (secim == 1)
                    {
                        Console.WriteLine("Artırlaca bilet sayini daxil edin: ");
                        int artis = Convert.ToInt32(Console.ReadLine());
                        seciliBilet.say += artis;
                    }
                    else if (secim == 2)
                    {
                        Console.WriteLine("Azaltilacaq bilet sayini daxil edin: ");
                        int azalis = Convert.ToInt32(Console.ReadLine());
                        if (azalis > seciliBilet.say)
                        {
                            Console.WriteLine("XETA. azaldilaca bilet sayi artirilacadan coxdur.");
                        }
                        else
                        {
                            seciliBilet.say -= azalis;
                        }
                    }

                    File.WriteAllLines("bilet.csv", biletList.Select(b => $"{b.Qalxis},{b.Enis},{b.say},{b.nomre}"));

                }
            }


        }
        else
        {
            Console.WriteLine("bele bir emeliyyat yoxdur");
        }

    }
}


