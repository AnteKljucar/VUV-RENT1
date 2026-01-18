
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VUV_RENT
{
    public struct UnosVozilaAuto
    {
        public int id; //id vozila krece od 0
        public string marka; //proizvodajc
        public string model; //model auta
        public string motor; //motor
        public int snaga; //snaga motora izrazena u kW
        public int KM; //kilometraza
        public int godina; //godina proizvodnje
        public DateTime registracija; //registracija datum
        public int cjena; //cjena po danu
        public bool dostupnost; //dostupnost da ne, odnosi se na provjeru da li je vozilo trenutno rentano
        public UnosVozilaAuto(int a, string b, string c, string d, int e, int f, int g, DateTime h, int i, bool j)
        {
            id = a;
            marka = b;
            model = c;
            motor = d;
            snaga = e;
            KM = f;
            godina = g;
            registracija = h;
            cjena = i;
            dostupnost = j;
        }
    }
    public struct UnosVozilaMotor
    {
        public int id; //id vozila krece od 0
        public string marka; //proizvodajc
        public string model; //model auta
        public string motor; //motor
        public int snaga; //snaga motora izrazena u kW
        public int KM; //kilometraza
        public int godina; //godina proizvodnje
        public DateTime registracija; //registracija datum
        public int cjena; //cjena po danu
        public bool dostupnost; //dostupnost da ne, odnosi se na provjeru da li je vozilo trenutno rentano
        public UnosVozilaMotor(int a, string b, string c, string d, int e, int f, int g, DateTime h, int i, bool j)
        {
            id = a;
            marka = b;
            model = c;
            motor = d;
            snaga = e;
            KM = f;
            godina = g;
            registracija = h;
            cjena = i;
            dostupnost = j;
        }
    }
    public struct Korisnik
    {
        public int id; //id korisnika
        public string ime; //ime korisnika
        public string prezime; //prezime korisnika
        public string korisnickoIme; //korisnicko ime ili username
        public string password; //password profila 
        public string OIB; //oib
        public bool Admin; //provjera da li je korisnik administrator ili ne, odkljucav mogucnost modificiranje/dodavannje/brisanje podataka iz liste vozila

        public Korisnik(int a, string b, string c, string d, string e, string f, bool g)
        {
            id = 1;
            ime = b;
            prezime = c;
            korisnickoIme = d;
            password = e;
            OIB = f;
            Admin = g;
        }
    }

    class Program
    {

        static Korisnik register(int b)
        {
            Korisnik noviUnos = new Korisnik();
            string json = "";
            StreamReader sr = new StreamReader("C:\\Users\\Akljucar\\source\\repos\\VUV-RENT\\VUV-RENT\\korisnik.json"); //StreamReader sr se dodaje putanja do korisnik.json
            using (sr)
            {
                json = sr.ReadToEnd();
            }

            List<Korisnik> lkorisnik = new List<Korisnik>();
            lkorisnik = JsonConvert.DeserializeObject<List<Korisnik>>(json);
            noviUnos.id = b;
            noviUnos.ime = "";
            noviUnos.korisnickoIme = "";
            noviUnos.prezime = "";
            noviUnos.password = "";
            noviUnos.OIB = "";
            while (noviUnos.ime == "") {
                Console.WriteLine("unesite svoje ime:");
                noviUnos.ime = Console.ReadLine();
            }
            while (noviUnos.prezime == "") {
                Console.WriteLine("Unesite svoje prezime:");
                noviUnos.prezime = Console.ReadLine();
            }
            while (noviUnos.korisnickoIme == "") {
                Console.WriteLine("Unesite svoj username ili korisnicko ime: ");
                noviUnos.korisnickoIme = Console.ReadLine();
            }
            while (noviUnos.password == "") {
                Console.WriteLine("Unesite lozinku: ");
                string password = Console.ReadLine();
                Console.WriteLine("Potvrdite lozinku: ");
                string confPassword = Console.ReadLine();
                if (password != confPassword)
                {
                    Console.WriteLine("Ponovo unesite lozinku, potvrda lozinke nije uspjela.");
                    noviUnos.password = "";
                }
                else
                {
                    noviUnos.password = password;
                }
            }

            while (noviUnos.OIB == "")
            {
                Console.WriteLine("Unesite vas OIB:");
                noviUnos.OIB = Console.ReadLine();

                if (noviUnos.OIB.Length != 11)
                {
                    Console.WriteLine("Unos OIB-a je neispravan.");
                    noviUnos.OIB = "";
                    continue;
                }
   
                bool postoji = false;
                foreach (Korisnik k in lkorisnik)
                {
                    if (k.OIB == noviUnos.OIB)
                    {
                        postoji = true;
                        break;
                    }
                }

                if (postoji)
                {
                    Console.WriteLine("Korisnik s tim OIB-om vec postoji.");
                    noviUnos.OIB = "";
                }
        }
        noviUnos.Admin = false;

            return noviUnos;
        }





        static void SpremiKorisnike(List<Korisnik> korisnici, string path)
        {
            string noviJson = JsonConvert.SerializeObject(korisnici, Formatting.Indented);
            File.WriteAllText(path, noviJson);
        }




        static Korisnik RentACar() { }
        static Korisnik RentABike() { }
        static Korisnik RentABicikl() { }
        static Korisnik unosRente() { }
        static Korisnik unosRente() { }
        static Korisnik unosRente() { }



        static void ZabiljeziLog(string opisDogađaja)
        {
            string logPath = "C:\\Users\\Akljucar\\source\\repos\\VUV-RENT\\VUV-RENT\\logBook.txt";
            string vrijeme = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string zapis = $"{vrijeme} - {opisDogađaja}";

            
            File.AppendAllText(logPath, zapis + Environment.NewLine);
        }



        static Korisnik GetTrenutniKorisnik(List<Korisnik> korisnici, int trenutniId)
        {
            foreach (Korisnik k in korisnici)
            {
                if (k.id == trenutniId)
                    return k;
            }
            return new Korisnik();
        }

        static void RemoveVehicleMenu()
        {
            Console.WriteLine("\n--- REMOVE VEHICLE ---");
            Console.WriteLine("1 - Remove bike");
            Console.WriteLine("2 - Remove car");
            Console.WriteLine("3 - Remove bicikl");
            Console.Write("Odabir: ");

            int izbor = Convert.ToInt32(Console.ReadLine());

            switch (izbor)
            {
                case 1:
                    Console.WriteLine("Brisanje BIKE vozila");
                    break;

                case 2:
                    Console.WriteLine("Brisanje CAR vozila");
                    break;

                case 3:
                    Console.WriteLine("Brisanje BICIKL vozila");
                    break;

                default:
                    Console.WriteLine("Pogrešan unos.");
                    break;
            }
        }




        static void Main(string[] args)
        {
            int idTrenutnogKorisnika=-1;
            logAfReg:
            string json = "";
            StreamReader sr = new StreamReader("C:\\Users\\Akljucar\\source\\repos\\VUV-RENT\\VUV-RENT\\korisnik.json"); //StreamReader sr se dodaje putanja do korisnik.json
            using (sr)
            {
                json = sr.ReadToEnd();
            }

            List<Korisnik> lkorisnik = new List<Korisnik>();
            lkorisnik = JsonConvert.DeserializeObject<List<Korisnik>>(json);

            


            Console.WriteLine("Login ili register? 1 - log in, 2 - register");
            int sel1 = Convert.ToInt32(Console.ReadLine());
            bool logged = false;
            while (!logged) { 
            switch (sel1)
            {
                    case 1:
                        Console.WriteLine("Unesite korisnicko ime:");
                        string korImeCh = Console.ReadLine();

                        Console.WriteLine("Unesite vašu lozinku:");
                        string korLozCh = Console.ReadLine();

                        bool found = false;
                        foreach (Korisnik user in lkorisnik)
                        {
                            if (user.korisnickoIme == korImeCh &&
                                user.password == korLozCh)
                            {
                                idTrenutnogKorisnika = user.id;
                                Console.WriteLine("Uspješno ste se prijavili!");
                                logged = true;
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            Console.WriteLine("Uneseno korisnicko ime ili lozinka su neispravni.");
                        }

                        break;
                    case 2:
                    int c = lkorisnik.Count;
                    lkorisnik.Add(register(c));

                    string path = "C:\\Users\\Akljucar\\source\\repos\\VUV-RENT\\VUV-RENT\\korisnik.json";
                    SpremiKorisnike(lkorisnik, path);
                    goto logAfReg;
                    default:
                    Console.WriteLine("Pogrešan unos.");
                    break;

            }
            }
            Korisnik trenutniKorisnik = GetTrenutniKorisnik(lkorisnik, idTrenutnogKorisnika);
            Console.Clear();
            bool izbornikAktivan = true;
            while (izbornikAktivan)
            {
                Console.WriteLine("\n--- DOBRO DOŠLI ---");
                Console.WriteLine("\n--- GLAVNI IZBORNIK ---");
                Console.WriteLine("1 - Rent-a-car");
                Console.WriteLine("2 - Rent-a-bike");
                Console.WriteLine("3 - Rent-a-bicikl");
                Console.WriteLine("4 - Log out");

                if (trenutniKorisnik.Admin)
                {
                    Console.WriteLine("\n--- ADMIN OPCIJE ---");
                    Console.WriteLine("5 - Edit user");
                    Console.WriteLine("6 - Edit bike");
                    Console.WriteLine("7 - Edit car");
                    Console.WriteLine("8 - Edit bicikl");
                    Console.WriteLine("9 - Edit rent");
                    Console.WriteLine("10 - Remove user");
                    Console.WriteLine("11 - Remove vehicle");
                }

                Console.Write("\nOdabir: ");
                int izbor = Convert.ToInt32(Console.ReadLine());

                switch (izbor)
                {
                    case 1:
                        Console.WriteLine("Rent-a-car odabrano");
                        break;

                    case 2:
                        Console.WriteLine("Rent-a-bike odabrano");
                        break;

                    case 3:
                        Console.WriteLine("Rent-a-bicikl odabrano");
                        break;

                    case 4:
                        Console.WriteLine("Odjava uspješna.");
                        izbornikAktivan = false;
                        break;

                    case 5 when trenutniKorisnik.Admin:
                        Console.WriteLine("Edit user");
                        break;

                    case 6 when trenutniKorisnik.Admin:
                        Console.WriteLine("Edit bike");
                        break;

                    case 7 when trenutniKorisnik.Admin:
                        Console.WriteLine("Edit car");
                        break;

                    case 8 when trenutniKorisnik.Admin:
                        Console.WriteLine("Edit bicikl");
                        break;

                    case 9 when trenutniKorisnik.Admin:
                        Console.WriteLine("Edit rent");
                        break;

                    case 10 when trenutniKorisnik.Admin:
                        Console.WriteLine("Remove user");
                        break;

                    case 11 when trenutniKorisnik.Admin:
                        RemoveVehicleMenu();
                        break;

                    default:
                        Console.WriteLine("Neispravan odabir ili nemate ovlasti.");
                        break;
                }
            }

            foreach (Korisnik users in lkorisnik)
            {
                if(users.id == idTrenutnogKorisnika)
                {
                    Console.WriteLine("ID: " + users.id);
                    Console.WriteLine("Ime: " + users.ime);
                    Console.WriteLine("Prezime: " + users.prezime);
                    Console.WriteLine("OIB: " + users.OIB);
                    Console.WriteLine("Korisničko ime: " + users.korisnickoIme);
                    Console.WriteLine("Password: " + users.password);
                }
            }

            

            Console.ReadLine();


        }


    }
}


