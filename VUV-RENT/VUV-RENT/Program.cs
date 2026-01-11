
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VUV_RENT
{
    public struct unosVozilaAuto
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
        public unosVozilaAuto(int a, string b, string c, string d, int e, int f, int g, DateTime h, int i, bool j)
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
    public struct unosVozilaMotor
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
        public unosVozilaMotor(int a, string b, string c, string d, int e, int f, int g, DateTime h, int i, bool j)
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
    public struct korisnik
    {
        public int id; //id korisnika
        public string ime; //ime korisnika
        public string prezime; //prezime korisnika
        public string korisnickoIme; //korisnicko ime ili username
        public string password; //password profila 
        public string OIB; //oib
        public bool Admin; //provjera da li je korisnik administrator ili ne, odkljucav mogucnost modificiranje/dodavannje/brisanje podataka iz liste vozila

        public korisnik(int a, string b, string c, string d, string e, string f, bool g)
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

        static korisnik register(int b)
        {
            korisnik noviUnos = new korisnik();
            noviUnos.id = b; 
            Console.WriteLine("unesite svoje ime:");
            noviUnos.ime = Console.ReadLine();
            Console.WriteLine("Unesite svoje prezime:");
            noviUnos.prezime = Console.ReadLine();
            Console.WriteLine("Unesite svoj username ili korisnicko ime: ");
            noviUnos.korisnickoIme = Console.ReadLine();
            passReset:
            Console.WriteLine("Unesite lozinku: ");
            string password = Console.ReadLine();
            Console.WriteLine("Potvrdite lozinku: ");
            string confPassword = Console.ReadLine();
            if (password != confPassword)
            {
                Console.WriteLine("Ponovo unesite lozinku, potvrda lozinke nije uspjela.");
                goto passReset;
            }
            else
            {
                noviUnos.password = password;
            }
            oibReset:
            Console.WriteLine("Unesite vas OIB:");
            noviUnos.OIB = Console.ReadLine();
            if (noviUnos.OIB.Length != 11)
            {
                Console.WriteLine("Unos OIB-a je neispravan.");
                goto oibReset;
            }
            noviUnos.Admin = false;

            return noviUnos;
        }





        static void SpremiKorisnike(List<korisnik> korisnici, string path)
        {
            string noviJson = JsonConvert.SerializeObject(korisnici, Formatting.Indented);
            File.WriteAllText(path, noviJson);
        }







        static void ZabiljeziLog(string opisDogađaja)
        {
            string logPath = "C:\\Users\\Akljucar\\source\\repos\\VUV-RENT\\VUV-RENT\\logBook.txt";
            string vrijeme = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string zapis = $"{vrijeme} - {opisDogađaja}";

            
            File.AppendAllText(logPath, zapis + Environment.NewLine);
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

            List<korisnik> lkorisnik = new List<korisnik>();
            lkorisnik = JsonConvert.DeserializeObject<List<korisnik>>(json);

            


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
                        foreach (korisnik user in lkorisnik)
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
            ponoviOdabirIzDva:
            Console.WriteLine("Izbornik:");
            Console.WriteLine("1. Renta vozila\t2. Unos novog vozila\t3. Manualni unos novog korisnika\t4. Azuriranje postojeceg vozila\t5. Azuriranje postojece rente\t6. Azuracija podatka korisnika");
            int izDvaOdb = Convert.ToInt32(Console.ReadLine());

            switch (izDvaOdb) {
                case 1:
                    break;

                case 2:

                    break;
                case 3:
                    foreach (korisnik users in lkorisnik)
                    {
                        if (users.id == idTrenutnogKorisnika)
                        {
                            if (users.Admin == true) {
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nema ovlasti za ovu mogucnost!!!");
                            goto ponoviOdabirIzDva;
                        }
                    }
                    break;
                case 5:
                    foreach (korisnik users in lkorisnik)
                    {
                        if (users.id == idTrenutnogKorisnika)
                        {
                            if (users.Admin == true)
                            {
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nema ovlasti za ovu mogucnost!!!");
                            goto ponoviOdabirIzDva;

                        }
                    }

                    break;
                case 6:
                    foreach (korisnik users in lkorisnik)
                    {
                        if (users.id == idTrenutnogKorisnika)
                        {
                            if (users.Admin == true)
                            {
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nema ovlasti za ovu mogucnost!!!");
                            goto ponoviOdabirIzDva;
                        }
                    }
                    break;

                default:
                    Console.WriteLine("Odabrali ste nešto što nije ponudjeno!!!");
                    goto ponoviOdabirIzDva;         
            }

            foreach(korisnik users in lkorisnik)
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


