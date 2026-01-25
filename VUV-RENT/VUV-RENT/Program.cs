
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
        public int kategorija;
        public int snaga; //snaga motora izrazena u kW
        public int KM; //kilometraza
        public int godina; //godina proizvodnje
        public DateTime registracija; //registracija datum
        public int cjena; //cjena po danu
        public bool dostupnost; //dostupnost da ne, odnosi se na provjeru da li je vozilo trenutno rentano
        public Vozila(int a, string k, int l , string b, string c, string d, int e, int f, int g, DateTime h, int i, bool j)
        {
            id = a;
            tipVozila = k;
            marka = b;
            model = c;
            motor = d;
            kategorija = l;
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
            id = a;
            ime = b;
            prezime = c;
            korisnickoIme = d;
            password = e;
            OIB = f;
            Admin = g;
        }
    }
    public struct Kategorija
    {
        public int id; //id korisnika
        public string kategorija; //ime korisnika
        public Kategorija(int a, string b)
        {
            id = a;
            kategorija = b;
        }
    }
    public struct Najam
    {
        public int id;
        public int idKorisnika;
        public int idVozila;
        public DateTime odDatuma;
        public DateTime doDatuma;
        public int ukupniIznosTroskovaRente;
        public string povrat;
        public Najam(int a, int b, int c, DateTime d, DateTime e, int f, string g)
        {
            id = a;
            idKorisnika = b;
            idVozila = c;
            odDatuma = d;
            doDatuma = e;
            ukupniIznosTroskovaRente = f;
            povrat = g;
        }
    }

    class Program
    {

        static Korisnik register(int b, string korisnikPutanja)
        {
            Korisnik noviUnos = new Korisnik();
            string json = "";
            StreamReader sr = new StreamReader(korisnikPutanja); //StreamReader sr se dodaje putanja do korisnik.json
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


        static void ponudaVozila(string ponudaVozila, string putanjaKategorija) {
            string json = "";
            StreamReader sr = new StreamReader(ponudaVozila); //StreamReader sr se dodaje putanja do korisnik.json
            using (sr)
            {
                json = sr.ReadToEnd();
            }


            List<Vozila> lVozila = new List<Vozila>();
            lVozila = JsonConvert.DeserializeObject<List<Vozila>>(json);

            string json2 = "";
            StreamReader sr2 = new StreamReader(putanjaKategorija); //StreamReader sr se dodaje putanja do korisnik.json
            using (sr2)
            {
                json2 = sr2.ReadToEnd();
            }


            List<Kategorija> lKategorija = new List<Kategorija>();
            lKategorija = JsonConvert.DeserializeObject<List<Kategorija>>(json2);

            foreach (Vozila k in lVozila) {
                Console.WriteLine(k.id);
                Console.WriteLine(k.tipVozila);
                Console.WriteLine(k.marka);
                Console.WriteLine(k.model);
                Console.WriteLine(k.motor);
                foreach(Kategorija l in lKategorija)
                {
                    if(l.id == k.kategorija)
                    {
                        Console.WriteLine(l.kategorija);
                    }

                }
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
        static void pretrazivanjeVozila(string putanjaVozila, string putanjaKategorija) {

            string json = "";
            StreamReader sr = new StreamReader(putanjaVozila); //StreamReader sr se dodaje putanja do korisnik.json
            using (sr)
            {
                json = sr.ReadToEnd();
            }

            List<Vozila> lVozila = new List<Vozila>();
            lVozila = JsonConvert.DeserializeObject<List<Vozila>>(json);
            string json2 = "";
            StreamReader sr2 = new StreamReader(putanjaKategorija); //StreamReader sr se dodaje putanja do korisnik.json
            using (sr2)
            {
                json2 = sr2.ReadToEnd();
            }


            List<Kategorija> lKategorija = new List<Kategorija>();
            lKategorija = JsonConvert.DeserializeObject<List<Kategorija>>(json2);
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
                                            foreach (Kategorija l in lKategorija)
                                            {
                                                if (l.id == k.kategorija)
                                                {
                                                    Console.WriteLine(l.kategorija);
                                                }

                                            }
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
                                            foreach (Kategorija l in lKategorija)
                                            {
                                                if (l.id == k.kategorija)
                                                {
                                                    Console.WriteLine(l.kategorija);
                                                }

                                            }
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
                                            foreach (Kategorija l in lKategorija)
                                            {
                                                if (l.id == k.kategorija)
                                                {
                                                    Console.WriteLine(l.kategorija);
                                                }

                                            }
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
                                        foreach (Kategorija l in lKategorija)
                                        {
                                            if (l.id == k.kategorija)
                                            {
                                                Console.WriteLine(l.kategorija);
                                            }

                                        }
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
                                        foreach (Kategorija l in lKategorija)
                                        {
                                            if (l.id == k.kategorija)
                                            {
                                                Console.WriteLine(l.kategorija);
                                            }

                                        }
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
                                        foreach (Kategorija l in lKategorija)
                                        {
                                            if (l.id == k.kategorija)
                                            {
                                                Console.WriteLine(l.kategorija);
                                            }

                                        }
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
                                        foreach (Kategorija l in lKategorija)
                                        {
                                            if (l.id == k.kategorija)
                                            {
                                                Console.WriteLine(l.kategorija);
                                            }

                                        }
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
                                        foreach (Kategorija l in lKategorija)
                                        {
                                            if (l.id == k.kategorija)
                                            {
                                                Console.WriteLine(l.kategorija);
                                            }

                                        }
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
                                        foreach (Kategorija l in lKategorija)
                                        {
                                            if (l.id == k.kategorija)
                                            {
                                                Console.WriteLine(l.kategorija);
                                            }

                                        }
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
                                        foreach (Kategorija l in lKategorija)
                                        {
                                            if (l.id == k.kategorija)
                                            {
                                                Console.WriteLine(l.kategorija);
                                            }

                                        }
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



        static void RentajVozilo(string vozilaPutanja, string rentaPutanja, int trenutniKorisnik, string korisnikPutanja, string putanjaKategorija)
        {
            string json = File.ReadAllText(vozilaPutanja);
            List<Vozila> lVozila = JsonConvert.DeserializeObject<List<Vozila>>(json);

            string json2 = File.ReadAllText(rentaPutanja);
            List<Najam> lRenta = JsonConvert.DeserializeObject<List<Najam>>(json2);

            string json3 = File.ReadAllText(korisnikPutanja);
            List<Korisnik> lkorisnik = JsonConvert.DeserializeObject<List<Korisnik>>(json3); 

            string json4 = File.ReadAllText(putanjaKategorija);
            List<Kategorija> lKategorija = JsonConvert.DeserializeObject<List<Kategorija>>(json4);

            Console.WriteLine("Dobrodošli u rentu.");
            Console.WriteLine("Odaberite datum od kojeg zelite zapoceti rentu:");
            DateTime odDatuma = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Odaberite datum do kojeg zelite zapoceti rentu:");
            DateTime doDatuma = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("\nSlobodna vozila u odabranom periodu:\n");

            foreach (var vozilo in lVozila)
            {
                bool zauzeto = false;

                foreach (Najam renta in lRenta)
                {
                    if (renta.idVozila == vozilo.id)
                    {
                        DateTime rentaOd = renta.odDatuma;
                        DateTime rentaDo = renta.doDatuma;

                        // Ako postoji preklapanje perioda, vozilo je zauzeto
                        if (rentaOd <= doDatuma && rentaDo >= odDatuma)
                        {
                            zauzeto = true;
                            break;
                        }
                    }
                }

                if (!zauzeto)
                {
                    Console.WriteLine(vozilo.id);
                    Console.WriteLine(vozilo.marka);
                    Console.WriteLine(vozilo.model);
                    Console.WriteLine(vozilo.motor);

                    foreach (Kategorija l in lKategorija)
                    {
                        if (l.id == vozilo.kategorija)
                        {
                            Console.WriteLine(l.kategorija);
                            break;
                        }
                    }

                    Console.WriteLine(vozilo.snaga + " kw");
                    Console.WriteLine(vozilo.KM);
                    Console.WriteLine(vozilo.godina);
                    Console.WriteLine(vozilo.registracija);
                    Console.WriteLine(vozilo.cjena);
                    Console.WriteLine(vozilo.dostupnost);
                }

            }
            Console.WriteLine("Unesi ID vozila koje telite iznajmit: ");
            int rentVoziloID = Convert.ToInt32(Console.ReadLine());
            int ukupnaCjena = 0;
            int brojDana = Math.Abs((odDatuma - doDatuma).Days);
            foreach (Vozila l in lVozila)
            {
                if (l.id == rentVoziloID)
                {
                    ukupnaCjena = brojDana * l.cjena;
                }
            }


            zapisUNajam(rentaPutanja, rentVoziloID, trenutniKorisnik, odDatuma, doDatuma, ukupnaCjena);
            Console.Clear();
            Console.WriteLine("Rent uspjesan!!!");
            Console.WriteLine("Pritisnite enter za povratak");
            Console.ReadLine();

        }
        static void zapisUNajam(string rentaPutanja, int rentVoziloID, int trenutniKorisnik, DateTime odDatuma, DateTime doDatuma, int ukupnaCjena)
        {
            string json2 = File.ReadAllText(rentaPutanja);
            List<Najam> lRenta = JsonConvert.DeserializeObject<List<Najam>>(json2);

            int noviId = lRenta.Count;


            lRenta.Add(new Najam
            {
                id = noviId,
                idKorisnika = trenutniKorisnik,
                idVozila = rentVoziloID,
                odDatuma = odDatuma,
                doDatuma = doDatuma,
                ukupniIznosTroskovaRente = ukupnaCjena,
                povrat = "Ne"
            }) ;

            File.WriteAllText(
                rentaPutanja,
                JsonConvert.SerializeObject(lRenta, Formatting.Indented)
            );
        }

        static void povratVozila(string najamPutanja, string vozilaPutanja)
        {
            // učitaj najam
            string jsonNajam = File.ReadAllText(najamPutanja);
            List<Najam> najmovi = JsonConvert.DeserializeObject<List<Najam>>(jsonNajam);

            // učitaj vozila
            string jsonVozila = File.ReadAllText(vozilaPutanja);
            List<Vozila> vozila = JsonConvert.DeserializeObject<List<Vozila>>(jsonVozila);

            Console.Clear();
            Console.WriteLine("=== NAJMOVI ===\n");

            // ISPIŠI SAMO OSNOVNO
            for (int i = 0; i < najmovi.Count; i++)
            {
                Console.WriteLine("ID najma: " + najmovi[i].id);
                Console.WriteLine("Vozilo ID: " + najmovi[i].idVozila);
                Console.WriteLine("Povrat: " + najmovi[i].povrat);
                Console.WriteLine("------------------");
            }

            Console.Write("Unesi ID najma za povrat: ");
            int trazeniId = Convert.ToInt32(Console.ReadLine());

            // NAĐI NAJAM
            for (int i = 0; i < najmovi.Count; i++)
            {
                if (najmovi[i].id == trazeniId)
                {
                    // koristimo kopiju jer je struct
                    var tempNajam = najmovi[i];

                    if (tempNajam.povrat == "Da")
                    {
                        Console.WriteLine("Vozilo je već vraćeno.");
                        Console.ReadLine();
                        return;
                    }

                    tempNajam.povrat = "Da";
                    najmovi[i] = tempNajam; // vraćamo kopiju nazad u listu

                    // vrati vozilo kao dostupno
                    for (int j = 0; j < vozila.Count; j++)
                    {
                        if (vozila[j].id == tempNajam.idVozila)
                        {
                            var tempVozilo = vozila[j]; // kopija structa
                            tempVozilo.dostupnost = true;
                            vozila[j] = tempVozilo;     // vraćamo kopiju nazad u listu
                            break;
                        }
                    }

                    // spremi promjene
                    File.WriteAllText(najamPutanja, JsonConvert.SerializeObject(najmovi, Formatting.Indented));
                    File.WriteAllText(vozilaPutanja, JsonConvert.SerializeObject(vozila, Formatting.Indented));

                    Console.WriteLine("✅ Povrat uspješan.");
                    Console.ReadLine();
                    return;
                }
            }

            Console.WriteLine("❌ Najam s tim ID-em ne postoji.");
            Console.ReadLine();
        }


        static Vozila dodavanje(string vozilaPutanja)
        {
            Vozila novoVozilo = new Vozila();
            string json = "";
            StreamReader sr = new StreamReader(vozilaPutanja); //StreamReader sr se dodaje putanja do korisnik.json
            using (sr)
            {
                json = sr.ReadToEnd();
            }

            List<Vozila> lVozila = new List<Vozila>();
            lVozila = JsonConvert.DeserializeObject<List<Vozila>>(json);
            novoVozilo.id = lVozila.Count;
            novoVozilo.tipVozila = "";
            novoVozilo.marka = "";
            novoVozilo.model = "";
            novoVozilo.motor = "";
            novoVozilo.kategorija = -1;
            novoVozilo.snaga = -1;
            novoVozilo.KM = -1;
            novoVozilo.godina = -1;
            novoVozilo.cjena = -1;
            novoVozilo.dostupnost = true;


            while (novoVozilo.tipVozila == "")
            {
                Console.WriteLine("Unesi tip vozila (Auto, Motor, Bicikl):");
                novoVozilo.tipVozila = Console.ReadLine();
            }
            while (novoVozilo.marka == "")
            {
                Console.WriteLine("Unesi marku vozila:");
                novoVozilo.marka = Console.ReadLine();
            }
            while (novoVozilo.model == "")
            {
                Console.WriteLine("Unesi model vozila: ");
                novoVozilo.model = Console.ReadLine();
            }
            while (novoVozilo.motor == "")
            {
                Console.WriteLine("Unesi motor: ");
                novoVozilo.motor = Console.ReadLine();
            }

            while (novoVozilo.kategorija == -1)
            {
                bool izbornikKategorije = true;
                while (izbornikKategorije) {
                    Console.WriteLine("Odaberi kategoriju vozila:");
                    Console.WriteLine("Avant");
                    Console.WriteLine("Limuzina");
                    Console.WriteLine("Sportback");
                    Console.WriteLine("Hatchback");
                    Console.WriteLine("Coupe");
                    Console.WriteLine("SUV");

                    Console.WriteLine("Sportak");
                    Console.WriteLine("Skuter");
                    Console.WriteLine("Naked");

                    Console.WriteLine("Bicikl");

                    Console.WriteLine("Odabir: ");
                    int izbor = Convert.ToInt32(Console.ReadLine());
                    switch (izbor)
                {
                    case 1 when novoVozilo.tipVozila == "Auto":
                            novoVozilo.kategorija = izbor - 1;
                            izbornikKategorije = false;
                            break;
                    case 2 when novoVozilo.tipVozila == "Auto":
                            novoVozilo.kategorija = izbor - 1;
                            izbornikKategorije = false;
                            break;
                    case 3 when novoVozilo.tipVozila == "Auto":
                            novoVozilo.kategorija = izbor - 1;
                            izbornikKategorije = false;
                            break;

                    case 4 when novoVozilo.tipVozila == "Auto":
                            novoVozilo.kategorija = izbor - 1;
                            izbornikKategorije = false;
                            break;
                    case 5 when novoVozilo.tipVozila == "Auto":
                            novoVozilo.kategorija = izbor - 1;
                            izbornikKategorije = false;
                            break;

                    case 6 when novoVozilo.tipVozila == "Auto":
                            novoVozilo.kategorija = izbor - 1;
                            izbornikKategorije = false;
                            break;
                    case 7 when novoVozilo.tipVozila == "Motor":
                            novoVozilo.kategorija = izbor - 1;
                            izbornikKategorije = false;
                            break;
                    case 8 when novoVozilo.tipVozila == "Motor":
                            novoVozilo.kategorija = izbor - 1;
                            izbornikKategorije = false;
                            break;

                    case 9 when novoVozilo.tipVozila == "Motor":
                            novoVozilo.kategorija = izbor - 1;
                            izbornikKategorije = false;
                            break;
                    case 10 when novoVozilo.tipVozila == "Bicikl":
                            novoVozilo.kategorija = izbor - 1;
                            izbornikKategorije=false;
                        break;
                    default:
                            Console.Write("Odabrano nešto što nije ponudjeno.");
                        break;

                }}
            }
            while (novoVozilo.snaga == -1)
            {
                Console.WriteLine("Unesi snagu vozila (izraženu u kW): ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out novoVozilo.snaga))
                {
                    Console.WriteLine("Unos mora biti cijeli broj. Pokušaj ponovno.");
                    novoVozilo.snaga = -1;
                }
            }

            while (novoVozilo.KM == -1)
            {
                Console.WriteLine("Unesi kilometražu vozila: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out novoVozilo.KM))
                {
                    Console.WriteLine("Unos mora biti cijeli broj. Pokušaj ponovno.");
                    novoVozilo.KM = -1;
                }
            }
            while (novoVozilo.godina == -1)
            {
                Console.WriteLine("Unesi godinu proizvodnje vozila: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out novoVozilo.godina))
                {
                    Console.WriteLine("Unos mora biti cijeli broj. Pokušaj ponovno.");
                    novoVozilo.godina = -1;
                }
            }
            while (novoVozilo.cjena == -1)
            {
                Console.WriteLine("Unesi cijenu dana rente vozila: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out novoVozilo.cjena))
                {
                    Console.WriteLine("Unos mora biti cijeli broj. Pokušaj ponovno.");
                    novoVozilo.cjena = -1;
                }
            }
            DateTime date = DateTime.Now.AddYears(1).Date;        
            novoVozilo.registracija = date;

            return novoVozilo;
        }

        static void spremiVozilo(List<Vozila> lVozila, string path)
        {
            string noviJson = JsonConvert.SerializeObject(lVozila, Formatting.Indented);
            File.WriteAllText(path, noviJson);
        }






        static void azuriranje(string vozilaPutanja, string rentaPutanja)
        {
            string json = "";
            using (StreamReader sr = new StreamReader(vozilaPutanja))
            {
                json = sr.ReadToEnd();
            }

            List<Vozila> lVozila = JsonConvert.DeserializeObject<List<Vozila>>(json);

            string json2 = "";
            using (StreamReader srk = new StreamReader(rentaPutanja))
            {
                json2 = srk.ReadToEnd();
            }

            List<Kategorija> lKategorijas = JsonConvert.DeserializeObject<List<Kategorija>>(json2);

            foreach (Vozila v in lVozila)
            {
                Console.WriteLine(v.id);
                Console.WriteLine(v.tipVozila);
                Console.WriteLine(v.marka);
                Console.WriteLine(v.model);
                Console.WriteLine(v.motor);
                Console.WriteLine(v.snaga);
                foreach (Kategorija k in lKategorijas)
                {
                    if (k.id == v.kategorija)
                    {
                        Console.WriteLine(k.kategorija);
                    }
                }
                Console.WriteLine(v.KM);
                Console.WriteLine(v.registracija);
                Console.WriteLine(v.cjena);
                Console.WriteLine(v.dostupnost);
            }

            int idZaAzuriranje = -1;

            while (idZaAzuriranje == -1)
            {
                Console.WriteLine("Unesi ID vozila koje želiš ažurirati: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out idZaAzuriranje))
                {
                    Console.WriteLine("Unos mora biti cijeli broj.");
                    idZaAzuriranje = -1;
                }
            }
            Console.Clear();
            for (int i = 0; i < lVozila.Count; i++)
            {
                if (lVozila[i].id == idZaAzuriranje)
                {
                    bool izbornikAzuriranja = true;

                    while (izbornikAzuriranja)
                    {
                        Console.WriteLine("Što želiš ažurirati?");
                        Console.WriteLine("1 - Tip vozila");
                        Console.WriteLine("2 - Marka");
                        Console.WriteLine("3 - Model");
                        Console.WriteLine("4 - Motor");
                        Console.WriteLine("5 - Kategorija");
                        Console.WriteLine("6 - Snaga (kW)");
                        Console.WriteLine("7 - Kilometraža");
                        Console.WriteLine("8 - Godina");
                        Console.WriteLine("9 - Registracija (datum)");
                        Console.WriteLine("10 - Cijena");
                        Console.WriteLine("11 - Dostupnost");
                        Console.WriteLine("11 - Azuriranje i povratak");

                        int izbor = -1;
                        while (izbor == -1)
                        {
                            Console.WriteLine("Odabir: ");
                            string unos = Console.ReadLine();

                            if (!int.TryParse(unos, out izbor))
                            {
                                Console.WriteLine("Unos mora biti broj. Pokušaj ponovno.");
                                izbor = -1;
                            }
                        }

                        Vozila vozilo = lVozila[i];

                        switch (izbor)
                        {
                            case 1:
                                Console.Clear();
                                string noviTip = "";
                                while (noviTip == "")
                                {
                                    Console.WriteLine("Unesi novi tip vozila (Auto, Motor, Bicikl):");
                                    noviTip = Console.ReadLine();
                                }
                                vozilo.tipVozila = noviTip;
                                break;

                            case 2:
                                Console.Clear();
                                string novaMarka = "";
                                while (novaMarka == "")
                                {
                                    Console.WriteLine("Unesi novu marku vozila:");
                                    novaMarka = Console.ReadLine();
                                }
                                vozilo.marka = novaMarka;
                                break;

                            case 3:
                                Console.Clear();
                                string noviModel = "";
                                while (noviModel == "")
                                {
                                    Console.WriteLine("Unesi novi model vozila:");
                                    noviModel = Console.ReadLine();
                                }
                                vozilo.model = noviModel;
                                break;

                            case 4:
                                Console.Clear();
                                string noviMotor = "";
                                while (noviMotor == "")
                                {
                                    Console.WriteLine("Unesi novi motor:");
                                    noviMotor = Console.ReadLine();
                                }
                                vozilo.motor = noviMotor;  
                                break;

                            case 5:
                                Console.Clear();
                                int novaKategorija = -1;
                                while (novaKategorija == -1)
                                {
                                    bool izbornikKategorije = true;
                                    while (izbornikKategorije)
                                    {
                                        Console.WriteLine("Odaberi kategoriju vozila:");
                                        Console.WriteLine("1. Avant");
                                        Console.WriteLine("2. Limuzina");
                                        Console.WriteLine("3. Sportback");
                                        Console.WriteLine("4. Hatchback");
                                        Console.WriteLine("5. Coupe");
                                        Console.WriteLine("6. SUV");
                                        Console.WriteLine("7. Sportak");
                                        Console.WriteLine("8. Skuter");
                                        Console.WriteLine("9. Naked");
                                        Console.WriteLine("10. Bicikl");
                                        Console.Write("Odabir: ");

                                        int izborKategorije;
                                        if (!int.TryParse(Console.ReadLine(), out izborKategorije))
                                        {
                                            Console.WriteLine("Unos mora biti broj. Pokušaj ponovno.");
                                            continue;
                                        }

                                        switch (izborKategorije)
                                        {
                                            case 1 when vozilo.tipVozila == "Auto":
                                            case 2 when vozilo.tipVozila == "Auto":
                                            case 3 when vozilo.tipVozila == "Auto":
                                            case 4 when vozilo.tipVozila == "Auto":
                                            case 5 when vozilo.tipVozila == "Auto":
                                            case 6 when vozilo.tipVozila == "Auto":
                                            case 7 when vozilo.tipVozila == "Motor":
                                            case 8 when vozilo.tipVozila == "Motor":
                                            case 9 when vozilo.tipVozila == "Motor":
                                            case 10 when vozilo.tipVozila == "Bicikl":
                                                novaKategorija = izborKategorije - 1;
                                                izbornikKategorije = false;
                                                break;

                                            default:
                                                Console.WriteLine("Odabrano nešto što nije ponuđeno. Pokušaj ponovno.");
                                                break;
                                        }
                                    }
                                }
                                vozilo.kategorija = novaKategorija;
                                break;

                            case 6:
                                Console.Clear();
                                int novaSnaga = -1;
                                while (novaSnaga == -1)
                                {
                                    Console.WriteLine("Unesi novu snagu (kW):");
                                    if (!int.TryParse(Console.ReadLine(), out novaSnaga))
                                    {
                                        Console.WriteLine("Unos mora biti cijeli broj. Pokušaj ponovno.");
                                        novaSnaga = -1;
                                    }
                                }
                                vozilo.snaga = novaSnaga;
                                break;

                            case 7:
                                Console.Clear();
                                int noviKM = -1;
                                while (noviKM == -1)
                                {
                                    Console.WriteLine("Unesi novu kilometražu:");
                                    if (!int.TryParse(Console.ReadLine(), out noviKM))
                                    {
                                        Console.WriteLine("Unos mora biti cijeli broj. Pokušaj ponovno.");
                                        noviKM = -1;
                                    }
                                }
                                vozilo.KM = noviKM;
                                break;

                            case 8:
                                Console.Clear();
                                int novaGodina = -1;
                                while (novaGodina == -1)
                                {
                                    Console.WriteLine("Unesi novu godinu:");
                                    if (!int.TryParse(Console.ReadLine(), out novaGodina))
                                    {
                                        Console.WriteLine("Unos mora biti cijeli broj. Pokušaj ponovno.");
                                        novaGodina = -1;
                                    }
                                }
                                vozilo.godina = novaGodina;
                                break;

                            case 9:
                                Console.Clear();
                                DateTime novaRegistracija = DateTime.MinValue;
                                while (novaRegistracija == DateTime.MinValue)
                                {
                                    Console.WriteLine("Unesi novu registraciju (format: yyyy-MM-ddTHH:mm:ss):");
                                    if (!DateTime.TryParse(Console.ReadLine(), out novaRegistracija))
                                    {
                                        Console.WriteLine("Neispravan format datuma. Pokušaj ponovno.");
                                        novaRegistracija = DateTime.MinValue;
                                    }
                                }
                                vozilo.registracija = novaRegistracija;
                                break;

                            case 10:
                                Console.Clear();
                                int novaCijena = -1;
                                while (novaCijena == -1)
                                {
                                    Console.WriteLine("Unesi novu cijenu:");
                                    if (!int.TryParse(Console.ReadLine(), out novaCijena))
                                    {
                                        Console.WriteLine("Unos mora biti cijeli broj. Pokušaj ponovno.");
                                        novaCijena = -1;
                                    }
                                }
                                Console.Clear();
                                vozilo.cjena = novaCijena;
                                break;

                            case 11:
                                Console.Clear();
                                bool novaDostupnost = false;
                                bool valid = false;
                                while (!valid)
                                {
                                    Console.WriteLine("Je li vozilo dostupno? (true/false):");
                                    if (bool.TryParse(Console.ReadLine(), out novaDostupnost))
                                    {
                                        valid = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Unos mora biti true ili false. Pokušaj ponovno.");
                                    }
                                }
                                vozilo.dostupnost = novaDostupnost;
                                break;
                            case 12:
                                izbornikAzuriranja = false;
                                break;

                            default:
                                Console.Clear();
                                Console.WriteLine("Odabrano nešto što nije ponuđeno. Pokušaj ponovno.");
                                break;
                        }

                        lVozila[i] = vozilo;
                    }

                    spremiVozilo(lVozila, vozilaPutanja);
                    Console.WriteLine("Vozilo je uspješno ažurirano.");
                    return;
                }
            }

            Console.WriteLine("Vozilo s tim ID-em nije pronađeno.");
            Console.ReadLine();
        }









        static void brisanje(string vozilaPutanja, string rentaPutanja)
        {
            string json = "";
            StreamReader sr = new StreamReader(vozilaPutanja); //StreamReader sr se dodaje putanja do korisnik.json
            using (sr)
            {
                json = sr.ReadToEnd();
            }

            List<Vozila> lVozila = new List<Vozila>();
            lVozila = JsonConvert.DeserializeObject<List<Vozila>>(json);
            string json2 = "";
            StreamReader srk = new StreamReader(rentaPutanja); //StreamReader sr se dodaje putanja do korisnik.json
            using (sr)
            {
                json2 = srk.ReadToEnd();
            }

            List<Kategorija> lKategorijas = new List<Kategorija>();
            lKategorijas = JsonConvert.DeserializeObject<List<Kategorija>>(json2);

            foreach (Vozila v in lVozila)
            {
                Console.WriteLine(v.id);
                Console.WriteLine(v.tipVozila);
                Console.WriteLine(v.marka);
                Console.WriteLine(v.model);
                Console.WriteLine(v.motor);
                Console.WriteLine(v.snaga);
                foreach (Kategorija k in lKategorijas)
                {
                    if (k.id == v.kategorija)
                    {
                        Console.WriteLine(k.kategorija);
                    }
                }
                Console.WriteLine(v.KM);
                Console.WriteLine(v.registracija);
                Console.WriteLine(v.cjena);
                Console.WriteLine(v.dostupnost);

            }

            int idZaBrisanje = -1;

            // Unos ID-a (sigurno, samo int)
            while (idZaBrisanje == -1)
            {
                Console.WriteLine("Unesi ID vozila koje želiš obrisati: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out idZaBrisanje))
                {
                    Console.WriteLine("Unos mora biti cijeli broj.");
                    idZaBrisanje = -1;
                }
            }
            for (int i = 0; i < lVozila.Count; i++)
            {
                if (lVozila[i].id == idZaBrisanje)
                {
                    lVozila.RemoveAt(i);

                    spremiVozilo(lVozila, vozilaPutanja);

                    Console.WriteLine("Vozilo je uspješno obrisano.");
                    return;
                }
            }

            Console.WriteLine("Vozilo s tim ID-em nije pronađeno.");
            Console.ReadLine();

        }


        static void Main(string[] args)
        {
            string kategorijePutanja = "C:\\Users\\Akljucar\\source\\repos\\VUV-RENT\\VUV-RENT\\kategorija.json";
            string vozilaPutanja = "C:\\Users\\Akljucar\\source\\repos\\VUV-RENT\\VUV-RENT\\vozila.json";
            string korisnikPutanja = "C:\\Users\\Akljucar\\source\\repos\\VUV-RENT\\VUV-RENT\\korisnik.json";
            string rentaPutanja = "C:\\Users\\Akljucar\\source\\repos\\VUV-RENT\\VUV-RENT\\najam.json";

            int idTrenutnogKorisnika =-1;
            logAfReg:
            string json = "";
            StreamReader sr = new StreamReader(korisnikPutanja); //StreamReader sr se dodaje putanja do korisnik.json
            using (sr)
            {
                json = sr.ReadToEnd();
            }

            List<Korisnik> lkorisnik = new List<Korisnik>();
            lkorisnik = JsonConvert.DeserializeObject<List<Korisnik>>(json);


            int sel1;

            while (true)
            {
                Console.WriteLine("Login ili register? 1 - log in, 2 - register");

                if (int.TryParse(Console.ReadLine(), out sel1) && (sel1 == 1 || sel1 == 2))
                    break;

                Console.WriteLine("Neispravan unos! Unesite 1 ili 2.\n");
            }

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
                    lkorisnik.Add(register(c, korisnikPutanja));

                    SpremiKorisnike(lkorisnik , korisnikPutanja);
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
                Console.Clear();
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
                string izbor =Console.ReadLine();
                
                switch (izbor)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("1 - Pregled ponude vozila");
                        ponudaVozila(vozilaPutanja, kategorijePutanja);
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("2 - Pretrazivanje vozila");
                        pretrazivanjeVozila(vozilaPutanja, kategorijePutanja);
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("3 - Iznajmi vozilo");
                        RentajVozilo(vozilaPutanja, rentaPutanja, trenutniKorisnik.id, korisnikPutanja, kategorijePutanja);
                        break;

                    case "4":

                        Console.Clear();
                        Console.WriteLine("Odjava uspješna.");
                        izbornikAktivan = false;
                        break;

                    case "5" when trenutniKorisnik.Admin:
                        Console.Clear();
                        Console.WriteLine("5 - Povrat vozila");
                        povratVozila(rentaPutanja, vozilaPutanja);
                        break;

                    case "6" when trenutniKorisnik.Admin:
                        Console.Clear();
                        Console.WriteLine("6 - Dodavanje/azuriranje/brisanje vozila");
                        Console.WriteLine("Odaberi što zeliš napraviti: ");
                        Console.WriteLine("1 - Dodavanje");
                        Console.WriteLine("2 - azuriranje");
                        Console.WriteLine("3 - brisanje");

                        Console.Write("\nOdabir: ");
                        string izbor2 =Console.ReadLine();

                        switch(izbor2)
                        {
                            case "1":
                                StreamReader srV = new StreamReader(vozilaPutanja); //StreamReader sr se dodaje putanja do korisnik.json
                                using (srV)
                                {
                                    json = srV.ReadToEnd();
                                }

                                List<Vozila> lVozila = new List<Vozila>();
                                lVozila = JsonConvert.DeserializeObject<List<Vozila>>(json);
                                Console.Clear();
                                lVozila.Add(dodavanje(vozilaPutanja));
                                spremiVozilo(lVozila, vozilaPutanja);

                                break;
                            case "2":

                                Console.Clear();
                                azuriranje(vozilaPutanja, rentaPutanja);
                                break;
                            case "3":

                                Console.Clear();
                                brisanje(vozilaPutanja, rentaPutanja);
                                break;
                            default:
                                Console.WriteLine("Neispravan odabir ili nemate ovlasti.");
                                break;
                        }

                        break;

                    case "7" when trenutniKorisnik.Admin:
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


