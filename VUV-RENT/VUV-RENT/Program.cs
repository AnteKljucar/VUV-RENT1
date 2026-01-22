
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VUV_RENT
{
    public struct Vozila
    {
        public int id; //id vozila krece od 0
        public string tipVozila;
        public string marka; //proizvodajc
        public string model; //model auta
        public string motor; //motor
        public int snaga; //snaga motora izrazena u kW
        public int KM; //kilometraza
        public int godina; //godina proizvodnje
        public DateTime registracija; //registracija datum
        public int cjena; //cjena po danu
        public bool dostupnost; //dostupnost da ne, odnosi se na provjeru da li je vozilo trenutno rentano
        public Vozila(int a, string k, string b, string c, string d, int e, int f, int g, DateTime h, int i, bool j)
        {
            id = a;
            tipVozila = k;
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


        static Korisnik GetTrenutniKorisnik(List<Korisnik> korisnici, int trenutniId)
        {
            foreach (Korisnik k in korisnici)
            {
                if (k.id == trenutniId)
                    return k;
            }
            return new Korisnik();
        }

        
        

        static void statistika() { 
        
        
        }
        static void ponudaVozila() {
            string json = "";
            StreamReader sr = new StreamReader("C:\\Users\\AK\\source\\repos\\VUV-RENT1-main\\VUV-RENT1-main\\VUV-RENT\\VUV-RENT\\vozila.json"); //StreamReader sr se dodaje putanja do korisnik.json
            using (sr)
            {
                json = sr.ReadToEnd();
            }

            List<Vozila> lVozila = new List<Vozila>();
            lVozila = JsonConvert.DeserializeObject<List<Vozila>>(json);

            foreach (Vozila k in lVozila) {
                Console.WriteLine(k.id);
                Console.WriteLine(k.tipVozila);
                Console.WriteLine(k.marka);
                Console.WriteLine(k.model);
                Console.WriteLine(k.motor);
                Console.WriteLine(k.snaga + " kw");
                Console.WriteLine(k.KM);
                Console.WriteLine(k.godina);
                Console.WriteLine(k.registracija);
                Console.WriteLine(k.cjena);
                Console.WriteLine(k.dostupnost);

            }
            Console.WriteLine("Pritisnite ENTER za povrat na izbornik");
            Console.ReadLine();
            Console.Clear();

        }
        static void pretrazivanjeVozila() {

            string json = "";
            StreamReader sr = new StreamReader("C:\\Users\\AK\\source\\repos\\VUV-RENT1-main\\VUV-RENT1-main\\VUV-RENT\\VUV-RENT\\vozila.json"); //StreamReader sr se dodaje putanja do korisnik.json
            using (sr)
            {
                json = sr.ReadToEnd();
            }

            List<Vozila> lVozila = new List<Vozila>();
            lVozila = JsonConvert.DeserializeObject<List<Vozila>>(json);
            bool izbornikPretAktivan = true;
            while (izbornikPretAktivan)
            {
                Console.WriteLine("\n--- Izbornik za pretrazivanje ---");
                Console.WriteLine("Odaberite kriterij pretrazivanja");
                Console.WriteLine("1 - Tipu vozila");
                Console.WriteLine("2 - Marki vozila");
                Console.WriteLine("3 - Modelu vozila");
                Console.WriteLine("4 - Motoru vozila");
                Console.WriteLine("5 - Snaga vozila");
                Console.WriteLine("6 - Kilometrazi");
                Console.WriteLine("7 - Godini proizvodnje");
                Console.WriteLine("8 - Cjeni rente");
                Console.WriteLine("9 - Povrat");


                Console.Write("\nOdabir: ");
                int izbor = Convert.ToInt32(Console.ReadLine());

                switch (izbor)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("1 - Tipu vozila");
                        bool izbornikTipVozilaAktivan = true;
                        while (izbornikTipVozilaAktivan) {
                            Console.Clear();
                            Console.WriteLine("--- Izbornik tipa vozila ---");
                            Console.WriteLine("1 - Auto");
                            Console.WriteLine("2 - Motor");
                            Console.WriteLine("3 - Bicikl");
                            Console.WriteLine("4 - Povratak");
                            Console.Write("\nOdabir: ");
                            int izborTipa = Convert.ToInt32(Console.ReadLine());

                            switch (izborTipa) {
                                case 1:
                                    Console.Clear();
                                    Console.WriteLine("1. Auti");
                                    int rb = 1;
                                    foreach (Vozila k in lVozila)
                                    {
                                        if (k.tipVozila == "Auto")
                                        {
                                            Console.WriteLine(rb);
                                            Console.WriteLine(k.marka);
                                            Console.WriteLine(k.model);
                                            Console.WriteLine(k.motor);
                                            Console.WriteLine(k.snaga + " kw");
                                            Console.WriteLine(k.KM);
                                            Console.WriteLine(k.godina);
                                            Console.WriteLine(k.registracija);
                                            Console.WriteLine(k.cjena);
                                            Console.WriteLine(k.dostupnost);
                                            rb++;
                                        }

                                    }
                                    Console.ReadLine();
                                    break;
                                case 2:
                                    Console.Clear();
                                    Console.WriteLine("2. Motori");
                                    int rb2 = 1;
                                    foreach (Vozila k in lVozila)
                                    {
                                        if (k.tipVozila == "Motor")
                                        {
                                            Console.WriteLine(rb2);
                                            Console.WriteLine(k.marka);
                                            Console.WriteLine(k.model);
                                            Console.WriteLine(k.motor);
                                            Console.WriteLine(k.snaga + " kw");
                                            Console.WriteLine(k.KM);
                                            Console.WriteLine(k.godina);
                                            Console.WriteLine(k.registracija);
                                            Console.WriteLine(k.cjena);
                                            Console.WriteLine(k.dostupnost);
                                            rb2++;
                                        }
                                    }
                                    Console.ReadLine();
                                    break;
                                case 3:
                                    Console.Clear();
                                    Console.WriteLine("3. Bicikli");
                                    int rb3 = 1;
                                    foreach (Vozila k in lVozila)
                                    {
                                        if (k.tipVozila == "Bicikl")
                                        {
                                            Console.WriteLine(rb3);
                                            Console.WriteLine(k.marka);
                                            Console.WriteLine(k.model);
                                            Console.WriteLine(k.motor);
                                            Console.WriteLine(k.snaga + " kw");
                                            Console.WriteLine(k.KM);
                                            Console.WriteLine(k.godina);
                                            Console.WriteLine(k.registracija);
                                            Console.WriteLine(k.cjena);
                                            Console.WriteLine(k.dostupnost);
                                            rb3++;
                                        }
                                    }
                                    Console.ReadLine();
                                    break;
                                case 4:
                                    Console.Clear();
                                    izbornikTipVozilaAktivan = false;
                                    break;

                                default:
                                    Console.WriteLine("Neispravan odabir.");
                                    break;

                            }


                        }
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("2 - Marki vozila");
                        bool izbornikMarkaVozilaAktivan = true;
                        while (izbornikMarkaVozilaAktivan) {
                            Console.WriteLine("--- Pretraga po marci vozila ---");
                            Console.WriteLine("Upišite marku vozila koja vas zanima.(Upišite 1 za povratak na predhodni izbornik)");
                            string markaPretrage = Console.ReadLine();
                            int rb = 1;
                            if(markaPretrage != "1") { 
                            foreach(Vozila k in lVozila)
                            {
                                if (markaPretrage == k.marka) {
                                    Console.WriteLine(rb);
                                    Console.WriteLine(k.marka);
                                    Console.WriteLine(k.model);
                                    Console.WriteLine(k.motor);
                                    Console.WriteLine(k.snaga + " kw");
                                    Console.WriteLine(k.KM);
                                    Console.WriteLine(k.godina);
                                    Console.WriteLine(k.registracija);
                                    Console.WriteLine(k.cjena);
                                    Console.WriteLine(k.dostupnost);
                                    rb++;

                                }
                            }
                                Console.WriteLine("Pritisnite enter za povratak");
                                Console.ReadLine();
                                izbornikMarkaVozilaAktivan = false;
                                Console.Clear();
                            }
                            else {
                                Console.Clear();
                                izbornikMarkaVozilaAktivan = false; }
                        }
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("3 - Modelu vozilaa");
                        bool izbornikModelaVozila = true;
                        while (izbornikModelaVozila)
                        {
                            Console.WriteLine("--- Pretraga po modelu vozila ---");
                            Console.WriteLine("Upišite model vozila koja vas zanima. (Upišite 1 za povratak na predhodni izbornik)");
                            string modelPretrage = Console.ReadLine();
                            int rb = 1;
                            if (modelPretrage != "1")
                            {
                                foreach (Vozila k in lVozila)
                                {
                                    if (modelPretrage == k.model)
                                    {
                                        Console.WriteLine(rb);
                                        Console.WriteLine(k.marka);
                                        Console.WriteLine(k.model);
                                        Console.WriteLine(k.motor);
                                        Console.WriteLine(k.snaga + " kw");
                                        Console.WriteLine(k.KM);
                                        Console.WriteLine(k.godina);
                                        Console.WriteLine(k.registracija);
                                        Console.WriteLine(k.cjena);
                                        Console.WriteLine(k.dostupnost);
                                        rb++;

                                    }
                                }
                                Console.WriteLine("Pritisnite enter za povratak");
                                Console.ReadLine();
                                izbornikModelaVozila = false;
                                Console.Clear();
                            }
                            else
                            {
                                Console.Clear();
                                izbornikModelaVozila = false;
                            }
                        }
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("4 - Motoru vozila");
                        bool izbornikMotoraVozila = true;
                        while (izbornikMotoraVozila)
                        {
                            Console.WriteLine("--- Pretraga po motoru vozila ---");
                            Console.WriteLine("Upišite motoru vozila koja vas zanima. (Upišite 1 za povratak na predhodni izbornik)");
                            string motorPretrage = Console.ReadLine();
                            int rb = 1;
                            if (motorPretrage != "1")
                            {
                                foreach (Vozila k in lVozila)
                                {
                                    if (motorPretrage == k.motor)
                                    {
                                        Console.WriteLine(rb);
                                        Console.WriteLine(k.marka);
                                        Console.WriteLine(k.model);
                                        Console.WriteLine(k.motor);
                                        Console.WriteLine(k.snaga + " kw");
                                        Console.WriteLine(k.KM);
                                        Console.WriteLine(k.godina);
                                        Console.WriteLine(k.registracija);
                                        Console.WriteLine(k.cjena);
                                        Console.WriteLine(k.dostupnost);
                                        rb++;

                                    }
                                }
                                Console.WriteLine("Pritisnite enter za povratak");
                                Console.ReadLine();
                                izbornikMotoraVozila = false;
                                Console.Clear();
                            }
                            else
                            {
                                Console.Clear();
                                izbornikMotoraVozila = false;
                            }
                        }
                        break;
                    case 5:
                        Console.Clear();
                        Console.WriteLine("5 - Snaga vozila");               
                        bool izbornikSnageVozila = true;
                        while (izbornikSnageVozila)
                        {
                            Console.WriteLine("--- Pretraga po modelu vozila ---");
                            Console.WriteLine("Upišite od koje do koje snage vozila želite, snaga izražena u KW.");
                            Console.WriteLine("Povratak? Da/Ne");
                            string povratakODG = Console.ReadLine();
                            Console.WriteLine("OD: ");
                            int odSnage = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("DO: ");
                            int doSnage = Convert.ToInt32(Console.ReadLine());

                            int rb = 1;
                            if (povratakODG == "Ne" || povratakODG == "ne" || povratakODG =="NE" || povratakODG =="nE")
                            {
                                foreach (Vozila k in lVozila)
                                {
                                    if (k.snaga>=odSnage && k.snaga<=doSnage)
                                    {
                                        Console.WriteLine(rb);
                                        Console.WriteLine(k.marka);
                                        Console.WriteLine(k.model);
                                        Console.WriteLine(k.motor);
                                        Console.WriteLine(k.snaga + " kw");
                                        Console.WriteLine(k.KM);
                                        Console.WriteLine(k.godina);
                                        Console.WriteLine(k.registracija);
                                        Console.WriteLine(k.cjena);
                                        Console.WriteLine(k.dostupnost);
                                        rb++;

                                    }
                                }
                                Console.WriteLine("Pritisnite enter za povratak");
                                Console.ReadLine();
                                izbornikSnageVozila = false;
                                Console.Clear();
                            }
                            else
                            {
                                Console.Clear();
                                izbornikSnageVozila = false;
                            }
                        }
                        break;
                    case 6:
                        Console.Clear();
                        Console.WriteLine("6 - Kilometrazi");
                        bool izbornikKilometrazeVozila = true;
                        while (izbornikKilometrazeVozila)
                        {
                            Console.WriteLine("--- Pretraga po modelu vozila ---");
                            Console.WriteLine("Upišite od koje do koje kilometraza vozila želite");
                            Console.WriteLine("Povratak? Da/Ne");
                            string povratakODG = Console.ReadLine();
                            Console.WriteLine("OD: ");
                            int odKM = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("DO: ");
                            int doKM = Convert.ToInt32(Console.ReadLine());

                            int rb = 1;
                            if (povratakODG == "Ne" || povratakODG == "ne" || povratakODG == "NE" || povratakODG == "nE")
                            {
                                foreach (Vozila k in lVozila)
                                {
                                    if (k.KM >= odKM && k.KM <= doKM)
                                    {
                                        Console.WriteLine(rb);
                                        Console.WriteLine(k.marka);
                                        Console.WriteLine(k.model);
                                        Console.WriteLine(k.motor);
                                        Console.WriteLine(k.snaga + " kw");
                                        Console.WriteLine(k.KM);
                                        Console.WriteLine(k.godina);
                                        Console.WriteLine(k.registracija);
                                        Console.WriteLine(k.cjena);
                                        Console.WriteLine(k.dostupnost);
                                        rb++;

                                    }
                                }
                                Console.WriteLine("Pritisnite enter za povratak");
                                Console.ReadLine();
                                izbornikKilometrazeVozila = false;
                                Console.Clear();
                            }
                            else
                            {
                                Console.Clear();
                                izbornikKilometrazeVozila = false;
                            }
                        }
                        break;
                        
                    case 7:
                        Console.Clear();
                        Console.WriteLine("7 - Godini proizvodnje");
                        bool izbornikGodinaVozila = true;
                        while (izbornikGodinaVozila)
                        {
                            Console.WriteLine("--- Pretraga po modelu vozila ---");
                            Console.WriteLine("Upišite od koje do koje godine proizvodnje vozila želite");
                            Console.WriteLine("Povratak? Da/Ne");
                            string povratakODG = Console.ReadLine();
                            Console.WriteLine("OD: ");
                            int odGodine = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("DO: ");
                            int doGodine = Convert.ToInt32(Console.ReadLine());

                            int rb = 1;
                            if (povratakODG == "Ne" || povratakODG == "ne" || povratakODG == "NE" || povratakODG == "nE")
                            {
                                foreach (Vozila k in lVozila)
                                {
                                    if (k.godina >= odGodine && k.godina <= doGodine)
                                    {
                                        Console.WriteLine(rb);
                                        Console.WriteLine(k.marka);
                                        Console.WriteLine(k.model);
                                        Console.WriteLine(k.motor);
                                        Console.WriteLine(k.snaga + " kw");
                                        Console.WriteLine(k.KM);
                                        Console.WriteLine(k.godina);
                                        Console.WriteLine(k.registracija);
                                        Console.WriteLine(k.cjena);
                                        Console.WriteLine(k.dostupnost);
                                        rb++;

                                    }
                                }
                                Console.WriteLine("Pritisnite enter za povratak");
                                Console.ReadLine();
                                izbornikGodinaVozila = false;
                                Console.Clear();
                            }
                            else
                            {
                                Console.Clear();
                                izbornikGodinaVozila = false;
                            }
                        }
                        break;
                        
                    case 8:
                        Console.Clear();
                        Console.WriteLine("8 - Cjeni rente");
                        bool izbornikCjeneVozila = true;
                        while (izbornikCjeneVozila)
                        {
                            Console.WriteLine("--- Pretraga po modelu vozila ---");
                            Console.WriteLine("Upišite od koje do koje godine proizvodnje vozila želite");
                            Console.WriteLine("Povratak? Da/Ne");
                            string povratakODG = Console.ReadLine();
                            Console.WriteLine("OD: ");
                            int odCijene = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("DO: ");
                            int doCijene = Convert.ToInt32(Console.ReadLine());

                            int rb = 1;
                            if (povratakODG == "Ne" || povratakODG == "ne" || povratakODG == "NE" || povratakODG == "nE")
                            {
                                foreach (Vozila k in lVozila)
                                {
                                    if (k.cjena >= odCijene && k.cjena <= doCijene)
                                    {
                                        Console.WriteLine(rb);
                                        Console.WriteLine(k.marka);
                                        Console.WriteLine(k.model);
                                        Console.WriteLine(k.motor);
                                        Console.WriteLine(k.snaga + " kw");
                                        Console.WriteLine(k.KM);
                                        Console.WriteLine(k.godina);
                                        Console.WriteLine(k.registracija);
                                        Console.WriteLine(k.cjena);
                                        Console.WriteLine(k.dostupnost);
                                        rb++;

                                    }
                                }
                                Console.WriteLine("Pritisnite enter za povratak");
                                Console.ReadLine();
                                izbornikCjeneVozila = false;
                                Console.Clear();
                            }
                            else
                            {
                                Console.Clear();
                                izbornikCjeneVozila = false;
                            }
                        }
                        break;
               
                    case 9:
                        Console.Clear();
                        izbornikPretAktivan = false;
                        break;


                    default:
                        Console.WriteLine("Neispravan odabir ili nemate ovlasti.");
                        break;
                }
            }
        }



        static void Main(string[] args)
        {
            int idTrenutnogKorisnika=-1;
            logAfReg:
            string json = "";
            StreamReader sr = new StreamReader("C:\\Users\\AK\\source\\repos\\VUV-RENT1-main\\VUV-RENT1-main\\VUV-RENT\\VUV-RENT\\korisnik.json"); //StreamReader sr se dodaje putanja do korisnik.json
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
                Console.WriteLine("1 - Pregled ponude vozila");
                Console.WriteLine("2 - Pretrazivanje vozila");
                Console.WriteLine("3 - Iznajmi vozilo");
                Console.WriteLine("4 - Odjava");


                if (trenutniKorisnik.Admin)
                {
                    Console.WriteLine("\n--- ADMIN OPCIJE ---");
                    Console.WriteLine("5 - Povrat vozila");
                    Console.WriteLine("6 - Dodavanje/azuriranje/brisanje vozila");
                    Console.WriteLine("7 - Statistika");

                }

                Console.Write("\nOdabir: ");
                int izbor = Convert.ToInt32(Console.ReadLine());

                switch (izbor)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("1 - Pregled ponude vozila");
                        ponudaVozila();
                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("2 - Pretrazivanje vozila");
                        pretrazivanjeVozila();
                        break;

                    case 3:
                        Console.Clear();
                        Console.WriteLine("3 - Iznajmi vozilo");
                        break;

                    case 4:
                        Console.WriteLine("Odjava uspješna.");
                        izbornikAktivan = false;
                        break;

                    case 5 when trenutniKorisnik.Admin:
                        Console.Clear();
                        Console.WriteLine("5 - Povrat vozila");
                        break;

                    case 6 when trenutniKorisnik.Admin:
                        Console.Clear();
                        Console.WriteLine("6 - Dodavanje/azuriranje/brisanje vozila");
                        break;

                    case 7 when trenutniKorisnik.Admin:
                        Console.Clear();
                        Console.WriteLine("7 - Statistika");
                        statistika();
                        break;


                    default:
                        Console.WriteLine("Neispravan odabir ili nemate ovlasti.");
                        break;
                }
            }
            
            Console.ReadLine();


        }


    }
}


