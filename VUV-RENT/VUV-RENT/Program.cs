
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using ConsoleTableExt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;


namespace VUV_RENT
{

    public struct Vozila
    {
        public int id; 
        public string tipVozila;
        public string marka; 
        public string model; 
        public string motor; 
        public int kategorija;
        public int snaga; 
        public int KM; 
        public int godina; 
        public DateTime registracija; 
        public int cjena; 
        public bool dostupnost; 
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
        public int id; 
        public string ime; 
        public string prezime; 
        public string korisnickoIme; 
        public string password; 
        public string OIB; 
        public bool Admin; 

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
        public int id;
        public string kategorija; 
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
        public bool povrat;
        public Najam(int a, int b, int c, DateTime d, DateTime e, int f, bool g)
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
            StreamReader sr = new StreamReader(korisnikPutanja); 
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

        
        

        static void ponudaVozila(string ponudaVozila, string putanjaKategorija) {
            string json = "";
            StreamReader sr = new StreamReader(ponudaVozila); 
            using (sr)
            {
                json = sr.ReadToEnd();
            }


            List<Vozila> lVozila = new List<Vozila>();
            lVozila = JsonConvert.DeserializeObject<List<Vozila>>(json);

            string json2 = "";
            StreamReader sr2 = new StreamReader(putanjaKategorija);
            using (sr2)
            {
                json2 = sr2.ReadToEnd();
            }


            List<Kategorija> lKategorija = new List<Kategorija>();
            lKategorija = JsonConvert.DeserializeObject<List<Kategorija>>(json2);

            var tableData = new List<List<object>>();

            foreach (Vozila v in lVozila)
            {
                if (v.dostupnost)
                {
                    tableData.Add(new List<object>()
                    {v.id, v.tipVozila, v.marka, v.model, v.motor, v.snaga, lKategorija.First(k => k.id == v.kategorija).kategorija, v.KM, v.registracija, v.cjena});
                }
            }
            ConsoleTableBuilder
                .From(tableData)
                .WithTitle("Dostupna Vozila")
                .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cjena")
                .ExportAndWriteLine();


            Console.WriteLine("Pritisnite ENTER za povrat na izbornik");
            Console.ReadLine();
            Console.Clear();

        }















        static void pretrazivanjeVozila(string putanjaVozila, string putanjaKategorija) {

            string json = "";
            StreamReader sr = new StreamReader(putanjaVozila); 
            using (sr)
            {
                json = sr.ReadToEnd();
            }

            List<Vozila> lVozila = new List<Vozila>();
            lVozila = JsonConvert.DeserializeObject<List<Vozila>>(json);
            string json2 = "";
            StreamReader sr2 = new StreamReader(putanjaKategorija); 
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
                int izbor = -1;
                while (izbor == -1)
                {
                    Console.WriteLine("Odabir");
                    string input = Console.ReadLine();
                    if (!int.TryParse(input, out izbor))
                    {
                        Console.WriteLine("Unos mora biti cijeli broj. Pokušaj ponovno.");
                        izbor = -1;
                    }
                }

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

                            int izborTipa = -1;
                            while (izborTipa == -1)
                            {
                                Console.WriteLine("Odabir");
                                string input = Console.ReadLine();
                                if (!int.TryParse(input, out izborTipa))
                                {
                                    Console.WriteLine("Unos mora biti cijeli broj. Pokušaj ponovno.");
                                    izborTipa = -1;
                                }
                            }

                            switch (izborTipa) {
                                case 1:
                                    Console.Clear();
                                    Console.WriteLine("1. Auti");
                                    var tableData = new List<List<object>>();
                                    foreach (Vozila v in lVozila)
                                    {
                                        if(v.tipVozila == "Auto") { }
                                        if (v.dostupnost)
                                        {
                                            tableData.Add(new List<object>()
                                                {v.id, v.tipVozila, v.marka, v.model, v.motor, v.snaga+"KW", lKategorija.First(k => k.id == v.kategorija).kategorija, v.KM, v.registracija, v.cjena});
                                        }
                                    }
                                    ConsoleTableBuilder
                                        .From(tableData)
                                        .WithTitle("Dostupna Vozila")
                                        .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cjena")
                                        .ExportAndWriteLine();

                                    
                                    Console.ReadLine();
                                    break;
                                case 2:
                                    Console.Clear();
                                    Console.WriteLine("2. Motori");
                                    var tableData2 = new List<List<object>>();
                                    foreach (Vozila v in lVozila)
                                    {
                                        if (v.tipVozila == "Motor")
                                        {
                                            if (v.dostupnost)
                                            {
                                                tableData2.Add(new List<object>()
                                                {v.id, v.tipVozila, v.marka, v.model, v.motor, v.snaga, lKategorija.First(k => k.id == v.kategorija).kategorija, v.KM, v.registracija, v.cjena});
                                            }
                                        }
                                    }
                                    ConsoleTableBuilder
                                        .From(tableData2)
                                        .WithTitle("Dostupna Vozila")
                                        .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cjena")
                                        .ExportAndWriteLine();

                                    Console.ReadLine();
                                    break;
                                case 3:
                                    Console.Clear();
                                    Console.WriteLine("3. Bicikli");
                                    var tableData3 = new List<List<object>>();
                                    foreach (Vozila v in lVozila)
                                    {
                                        if (v.tipVozila == "Bicikl")
                                        {
                                            
                                            if (v.dostupnost)
                                            {
                                                tableData3.Add(new List<object>()
                                                {v.id, v.tipVozila, v.marka, v.model, v.motor, v.snaga, lKategorija.First(k => k.id == v.kategorija).kategorija, v.KM, v.registracija, v.cjena});
                                            }
                                        }
                                    }
                                    ConsoleTableBuilder
                                        .From(tableData3)
                                        .WithTitle("Dostupna Vozila")
                                        .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cjena")
                                        .ExportAndWriteLine();
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
                            var tableData4 = new List<List<object>>();
                            if (markaPretrage != "1") { 

                            foreach(Vozila v in lVozila)
                            {
                                if (markaPretrage == v.marka) {
                                        
                                        if (v.dostupnost)
                                        {
                                            tableData4.Add(new List<object>()
                                                {v.id, v.tipVozila, v.marka, v.model, v.motor, v.snaga, lKategorija.First(k => k.id == v.kategorija).kategorija, v.KM, v.registracija, v.cjena});
                                        }
                                    }
                                }
                                ConsoleTableBuilder
                                    .From(tableData4)
                                    .WithTitle("Dostupna Vozila")
                                    .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cjena")
                                    .ExportAndWriteLine();
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
                            var tableData5 = new List<List<object>>();
                            if (modelPretrage != "1")
                            {
                                Console.Clear();
                                foreach (Vozila v in lVozila)
                                {
                                    if (modelPretrage == v.model)
                                    {
                                        if (v.dostupnost)
                                        {
                                            tableData5.Add(new List<object>()
                                                {v.id, v.tipVozila, v.marka, v.model, v.motor, v.snaga, lKategorija.First(k => k.id == v.kategorija).kategorija, v.KM, v.registracija, v.cjena});
                                        }
                                    }
                                }
                                ConsoleTableBuilder
                                    .From(tableData5)
                                    .WithTitle("Dostupna Vozila")
                                    .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cjena")
                                    .ExportAndWriteLine();
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
                            var tableData6 = new List<List<object>>();
                            if (motorPretrage != "1")
                            {
                                Console.Clear();
                                foreach (Vozila v in lVozila)
                                {
                                    if (motorPretrage == v.motor)
                                    {
                                        if (v.dostupnost)
                                        {
                                            tableData6.Add(new List<object>()
                                                {v.id, v.tipVozila, v.marka, v.model, v.motor, v.snaga, lKategorija.First(k => k.id == v.kategorija).kategorija, v.KM, v.registracija, v.cjena});
                                        }
                                    }
                                }
                                ConsoleTableBuilder
                                    .From(tableData6)
                                    .WithTitle("Dostupna Vozila")
                                    .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cjena")
                                    .ExportAndWriteLine();
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
                            Console.WriteLine("Povrat? Upiši Ne za nastavak.");
                            string povratakODG = Console.ReadLine();
                            if (povratakODG != "Ne" || povratakODG != "ne" || povratakODG != "NE" || povratakODG != "nE")
                            {
                                Console.Clear();
                                izbornikSnageVozila = false;
                            }
                            else {
                                int odSnage = -1;
                                while (odSnage == -1)
                                {
                                    Console.WriteLine("OD: ");
                                    string input = Console.ReadLine();
                                    if (!int.TryParse(input, out odSnage))
                                    {
                                        Console.WriteLine("Unos mora biti cijeli broj i jednak ili veci od 0. Pokušaj ponovno.");
                                        odSnage = -1;
                                    }
                                }

                                int doSnage = -1;
                                while (doSnage == -1)
                                {
                                    Console.WriteLine("OD: ");
                                    string input = Console.ReadLine();
                                    if (!int.TryParse(input, out doSnage))
                                    {
                                        Console.WriteLine("Unos mora biti cijeli broj i jednak ili veci od 0. Pokušaj ponovno.");
                                        doSnage = -1;
                                    }
                                }


                                var tableData7 = new List<List<object>>();
                                foreach (Vozila v in lVozila)
                                {
                                    if (v.snaga >= odSnage && v.snaga <= doSnage)
                                    {
                                        if (v.dostupnost)
                                        {
                                            tableData7.Add(new List<object>()
                                                {v.id, v.tipVozila, v.marka, v.model, v.motor, v.snaga, lKategorija.First(k => k.id == v.kategorija).kategorija, v.KM, v.registracija, v.cjena});
                                        }
                                    }
                                }
                                ConsoleTableBuilder
                                    .From(tableData7)
                                    .WithTitle("Dostupna Vozila")
                                    .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cjena")
                                    .ExportAndWriteLine();
                                Console.WriteLine("Pritisnite enter za povratak");
                                Console.ReadLine();
                                izbornikSnageVozila = false;
                                Console.Clear();
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
                            Console.WriteLine("Povrat? Upiši Ne za nastavak.");
                            string povratakODG = Console.ReadLine();
                            if (povratakODG != "Ne" || povratakODG != "ne" || povratakODG != "NE" || povratakODG != "nE")
                            {
                                Console.Clear();
                                izbornikKilometrazeVozila = false;
                            }
                            else
                            {
                                int odKM = -1;
                                while (odKM == -1)
                                {
                                    Console.WriteLine("OD: ");
                                    string input = Console.ReadLine();
                                    if (!int.TryParse(input, out odKM))
                                    {
                                        Console.WriteLine("Unos mora biti cijeli broj i jednak ili veci od 0. Pokušaj ponovno.");
                                        odKM = -1;
                                    }
                                }

                                int doKM = -1;
                                while (doKM == -1)
                                {
                                    Console.WriteLine("DO: ");
                                    string input = Console.ReadLine();
                                    if (!int.TryParse(input, out doKM))
                                    {
                                        Console.WriteLine("Unos mora biti cijeli broj i jednak ili veci od 0. Pokušaj ponovno.");
                                        doKM = -1;
                                    }
                                }


                                if (povratakODG == "Ne" || povratakODG == "ne" || povratakODG == "NE" || povratakODG == "nE")
                                {
                                    var tableData8 = new List<List<object>>();
                                    foreach (Vozila v in lVozila)
                                    {
                                        if (v.KM >= odKM && v.KM <= doKM)
                                        {
                                            if (v.dostupnost)
                                            {
                                                tableData8.Add(new List<object>()
                                                {v.id, v.tipVozila, v.marka, v.model, v.motor, v.snaga, lKategorija.First(k => k.id == v.kategorija).kategorija, v.KM, v.registracija, v.cjena});
                                            }
                                        }
                                    }
                                    ConsoleTableBuilder
                                        .From(tableData8)
                                        .WithTitle("Dostupna Vozila")
                                        .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cjena")
                                        .ExportAndWriteLine();
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
                            Console.WriteLine("Povrat? Upiši Ne za nastavak.");
                            string povratakODG = Console.ReadLine();
                            if (povratakODG != "Ne" || povratakODG != "ne" || povratakODG != "NE" || povratakODG != "nE")
                            {
                                Console.Clear();
                                izbornikGodinaVozila = false;
                                break;
                            }
                            else {
                                int odGodine = -1;
                                while (odGodine == -1)
                                {
                                    Console.WriteLine("OD: ");
                                    string input = Console.ReadLine();
                                    if (!int.TryParse(input, out odGodine))
                                    {
                                        Console.WriteLine("Unos mora biti cijeli broj. Pokušaj ponovno.");
                                        odGodine = -1;
                                    }
                                }
                                int doGodine = -1;
                                while (doGodine == -1)
                                {
                                    Console.WriteLine("OD: ");
                                    string input = Console.ReadLine();
                                    if (!int.TryParse(input, out doGodine))
                                    {
                                        Console.WriteLine("Unos mora biti cijeli broj. Pokušaj ponovno.");
                                        doGodine = -1;
                                    }
                                }


                                var tableData9 = new List<List<object>>();
                                foreach (Vozila v in lVozila)
                                {
                                    if (v.godina >= odGodine && v.godina <= doGodine)
                                    {
                                        if (v.dostupnost)
                                        {
                                            tableData9.Add(new List<object>()
                                                {v.id, v.tipVozila, v.marka, v.model, v.motor, v.snaga, lKategorija.First(k => k.id == v.kategorija).kategorija, v.KM, v.registracija, v.cjena});
                                        }
                                    }
                                }
                                ConsoleTableBuilder
                                    .From(tableData9)
                                    .WithTitle("Dostupna Vozila")
                                    .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cjena")
                                    .ExportAndWriteLine();
                                Console.WriteLine("Pritisnite enter za povratak");
                                Console.ReadLine();
                                izbornikGodinaVozila = false;
                                Console.Clear();
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
                            Console.WriteLine("Povrat? Upiši Ne za nastavak.");
                            string povratakODG = Console.ReadLine();
                            if (povratakODG != "Ne" || povratakODG != "ne" || povratakODG != "NE" || povratakODG != "nE")
                            {
                                Console.Clear();
                                izbornikCjeneVozila = false;
                            }
                            else
                            {
                                int odCijene = -1;
                                while (odCijene == -1)
                                {
                                    Console.WriteLine("OD: ");
                                    string input = Console.ReadLine();
                                    if (!int.TryParse(input, out odCijene))
                                    {
                                        Console.WriteLine("Unos mora biti cijeli broj. Pokušaj ponovno.");
                                        odCijene = -1;
                                    }
                                }

                                int doCijene = -1;
                                while (doCijene == -1)
                                {
                                    Console.WriteLine("DO: ");
                                    string input = Console.ReadLine();
                                    if (!int.TryParse(input, out doCijene))
                                    {
                                        Console.WriteLine("Unos mora biti cijeli broj. Pokušaj ponovno.");
                                        doCijene = -1;
                                    }
                                }
                                var tableData10 = new List<List<object>>();
                                foreach (Vozila v in lVozila)
                                {
                                    if (v.cjena >= odCijene && v.cjena <= doCijene && v.dostupnost != false)
                                    {
                                        if (v.dostupnost)
                                        {
                                            tableData10.Add(new List<object>()
                                                {v.id, v.tipVozila, v.marka, v.model, v.motor, v.snaga, lKategorija.First(k => k.id == v.kategorija).kategorija, v.KM, v.registracija, v.cjena});
                                        }
                                    }
                                }
                                ConsoleTableBuilder
                                    .From(tableData10)
                                    .WithTitle("Dostupna Vozila")
                                    .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cjena")
                                    .ExportAndWriteLine();
                                Console.WriteLine("Pritisnite enter za povratak");
                                Console.ReadLine();
                                izbornikCjeneVozila = false;
                                Console.Clear();

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

            string json4 = File.ReadAllText(putanjaKategorija);
            List<Kategorija> lKategorija = JsonConvert.DeserializeObject<List<Kategorija>>(json4);


            DateTime odDatuma = DateTime.MinValue;
            while (odDatuma == DateTime.MinValue)
            {
                Console.WriteLine("Unesi pocetni datum rente (d-m-g):");
                if (!DateTime.TryParse(Console.ReadLine(), out odDatuma))
                {
                    Console.WriteLine("Neispravan format datuma. Pokušaj ponovno.");
                    odDatuma = DateTime.MinValue;
                }
            }


            DateTime doDatuma = DateTime.MinValue;
            while (doDatuma == DateTime.MinValue)
            {
                Console.WriteLine("Unesi zavrsni datum rente (d-m-g):");
                if (!DateTime.TryParse(Console.ReadLine(), out doDatuma))
                {
                    Console.WriteLine("Neispravan format datuma. Pokušaj ponovno.");
                    doDatuma = DateTime.MinValue;
                }
            }

            Console.WriteLine("\nSlobodna vozila u odabranom periodu:\n");
            var tableData11 = new List<List<object>>();
            foreach (Vozila v in lVozila)
            {
                bool zauzeto = false;

                foreach (Najam renta in lRenta)
                {
                    if (renta.idVozila == v.id && v.dostupnost != false)
                    {
                        DateTime rentaOd = renta.odDatuma;
                        DateTime rentaDo = renta.doDatuma;

                        if (rentaOd <= doDatuma && rentaDo >= odDatuma && renta.povrat)
                        {
                            zauzeto = true;
                            break;
                        }


                    }
                }

                if (!zauzeto && v.dostupnost != false)
                {

                    if (v.dostupnost)
                    {
                        tableData11.Add(new List<object>()
                        {v.id, v.tipVozila, v.marka, v.model, v.motor, v.snaga, lKategorija.First(k => k.id == v.kategorija).kategorija, v.KM, v.registracija, v.cjena});
                    }
                }
            }
            ConsoleTableBuilder
                .From(tableData11)
                .WithTitle("Dostupna Vozila")
                .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cjena")
                .ExportAndWriteLine();


            int rentVoziloID = -1;
            while (rentVoziloID == -1)
            {
                Console.WriteLine("Unesi ID vozila koje telite iznajmit: ");
                string unosOdabira = Console.ReadLine();
                if(!int.TryParse(unosOdabira, out rentVoziloID))
                {
                    Console.WriteLine("Unos nije pronadjen ili je pogresan.");
                    rentVoziloID = -1;
                }
            }
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
                povrat = false
            }) ;

            File.WriteAllText(
                rentaPutanja,
                JsonConvert.SerializeObject(lRenta, Formatting.Indented)
            );
        }



















        static void povratVozila(string najamPutanja, string vozilaPutanja, string korisnikPutanja)
        {

            string jsonNajam = File.ReadAllText(najamPutanja);
            List<Najam> lNajmova = JsonConvert.DeserializeObject<List<Najam>>(jsonNajam);

            string jsonVozila = File.ReadAllText(vozilaPutanja);
            List<Vozila> lVozila = JsonConvert.DeserializeObject<List<Vozila>>(jsonVozila);

            string jsonKorisnik = File.ReadAllText(korisnikPutanja);
            List<Korisnik> lKorisnika = JsonConvert.DeserializeObject<List<Korisnik>>(jsonKorisnik);


            Console.Clear();

            var tableData = new List<List<object>>();

            foreach (Najam n in lNajmova)
            {
                if(n.povrat) { 
                var korisnik = lKorisnika.FirstOrDefault(k => k.id == n.idKorisnika);
                var vozilo = lVozila.FirstOrDefault(v => v.id == n.idVozila);


                tableData.Add(new List<object>(){ n.id,korisnik.ime,korisnik.prezime,n.odDatuma,n.doDatuma,vozilo.marka,vozilo.model,n.ukupniIznosTroskovaRente});
                }
            }

            ConsoleTableBuilder
                .From(tableData)
                .WithTitle("Najmovi")
                .WithColumn("ID najma", "Ime korisnika", "Prezime korisnika", "Od datuma", "Do datuma", "Marka vozila", "Model vozila", "Cijena rente")
                .ExportAndWriteLine();

            int trazeniId = -1;
            while (trazeniId == -1)
            {
                Console.WriteLine("Unesi ID najma za povrat: ");
                string unosOdabira = Console.ReadLine();
                if (!int.TryParse(unosOdabira, out trazeniId))
                {
                    Console.WriteLine("Unos nije pronadjen ili je pogresan.");
                    trazeniId = -1;
                }
            }


            for (int i = 0; i < lNajmova.Count; i++)
            {
                if (lNajmova[i].id == trazeniId)
                {
                    var tempNajam = lNajmova[i];

                    if (tempNajam.povrat)
                    {
                        Console.WriteLine("Vozilo je već vraćeno.");
                        Console.ReadLine();
                        return;
                    }

                    tempNajam.povrat = true;
                    lNajmova[i] = tempNajam;

                    for (int j = 0; j < lVozila.Count; j++)
                    {
                        if (lVozila[j].id == tempNajam.idVozila)
                        {
                            var tempVozilo = lVozila[j]; 
                            tempVozilo.dostupnost = true;
                            lVozila[j] = tempVozilo;    
                            break;
                        }
                    }

                    File.WriteAllText(najamPutanja, JsonConvert.SerializeObject(lNajmova, Formatting.Indented));
                    File.WriteAllText(vozilaPutanja, JsonConvert.SerializeObject(lVozila, Formatting.Indented));

                    Console.WriteLine("Povrat uspješan.");
                    Console.ReadLine();
                    return;
                }
            }

            Console.WriteLine("Najam s tim ID-em ne postoji.");
            Console.ReadLine();
        }


















        static Vozila dodavanje(string vozilaPutanja)
        {
            Vozila novoVozilo = new Vozila();
            string json = "";
            StreamReader sr = new StreamReader(vozilaPutanja); 
            using (sr)
            {
                json = sr.ReadToEnd();
            }

            List<Vozila> lVozila = new List<Vozila>();
            lVozila = JsonConvert.DeserializeObject<List<Vozila>>(json);
            novoVozilo.id = lVozila[lVozila.Count-1].id+1;
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
                    Console.WriteLine("1 - Avant");
                    Console.WriteLine("2 - Limuzina");
                    Console.WriteLine("3 - Sportback");
                    Console.WriteLine("4 - Hatchback");
                    Console.WriteLine("5 - Coupe");
                    Console.WriteLine("6 - SUV");
                    Console.WriteLine("7 - Sportak");
                    Console.WriteLine("8 - Skuter");
                    Console.WriteLine("9 - Naked");
                    Console.WriteLine("10 - Bicikl");
                    Console.WriteLine("Odabir: ");
                    int izbor = -1;
                    while (izbor == -1)
                    {
                        string input = Console.ReadLine();
                        if (!int.TryParse(input, out izbor))
                        {
                            Console.WriteLine("Unesi nešto od ponudjenog ili unos nije bio cjeli broj.");
                            izbor = -1;
                        }
                    }
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

            List<Kategorija> lKategorija = JsonConvert.DeserializeObject<List<Kategorija>>(json2);
            Console.Clear();
            var tableData = new List<List<object>>();
            foreach (Vozila v in lVozila)
            {
                    tableData.Add(new List<object>()
                    {v.id, v.tipVozila, v.marka, v.model, v.motor, v.snaga, lKategorija.First(k => k.id == v.kategorija).kategorija, v.KM, v.registracija, v.cjena, v.dostupnost});
            }
            ConsoleTableBuilder
            .From(tableData)
            .WithTitle("Dostupna Vozila")
            .ExportAndWriteLine();

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
                        Console.Clear();
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
                        Console.WriteLine("12 - Povratak");

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

                                        int izborKategorije = -1;
                                        while (izborKategorije == -1)
                                        {
                                            Console.WriteLine("Odabir: ");
                                            string unosOdabira = Console.ReadLine();
                                            if (!int.TryParse(unosOdabira, out izborKategorije))
                                            {
                                                Console.WriteLine("Unos nije pronadjen ili je pogresan.");
                                                izborKategorije = -1;
                                            }
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

















        static void brisanje(string vozilaPutanja, string kategorijaPutanja)
        {
            string json;
            using (StreamReader sr = new StreamReader(vozilaPutanja))
            {
                json = sr.ReadToEnd();
            }

            List<Vozila> lVozila = JsonConvert.DeserializeObject<List<Vozila>>(json);

            string json2;
            using (StreamReader srk = new StreamReader(kategorijaPutanja))
            {
                json2 = srk.ReadToEnd();
            }

            List<Kategorija> lKategorija = JsonConvert.DeserializeObject<List<Kategorija>>(json2);

            var tableData = new List<List<object>>();
            foreach (Vozila v in lVozila)
            {
                if (v.dostupnost)
                {
                    tableData.Add(new List<object>()
                    {v.id, v.tipVozila, v.marka, v.model, v.motor, v.snaga, lKategorija.First(k => k.id == v.kategorija).kategorija, v.KM, v.registracija, v.cjena});
                }
            }
            ConsoleTableBuilder
            .From(tableData)
            .WithTitle("Dostupna Vozila")
            .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cjena")
            .ExportAndWriteLine();

            int idZaBrisanje;
            while (true)
            {
                Console.WriteLine("Unesi ID vozila koje želiš obrisati: ");
                if (int.TryParse(Console.ReadLine(), out idZaBrisanje))
                    break;

                Console.WriteLine("Unos mora biti cijeli broj.");
            }

            bool pronadeno = false;

            for (int i = 0; i < lVozila.Count; i++)
            {
                if (lVozila[i].id == idZaBrisanje && lVozila[i].dostupnost)
                {
                    Vozila vozilo = lVozila[i];
                    vozilo.dostupnost = false;
                    lVozila[i] = vozilo;

                    spremiVozilo(lVozila, vozilaPutanja);
                    Console.WriteLine("Vozilo je uspješno obrisano.");
                    pronadeno = true;
                    break;
                }
            }

            if (!pronadeno)
            {
                Console.WriteLine("Vozilo s tim ID-em ne postoji ili je već obrisano.");
            }

            Console.ReadLine();
        }



        static void statistika(string vozilaPutanja,string kategorijePutanja ,string korisnikPutanja,string rentaPutanja)
        {
            string jsonVozila = File.ReadAllText(vozilaPutanja);
            List<Vozila> lVozila = JsonConvert.DeserializeObject<List<Vozila>>(jsonVozila);

            string jsonKategorija = File.ReadAllText(kategorijePutanja);
            List<Kategorija> lKategorija = JsonConvert.DeserializeObject<List<Kategorija>>(jsonKategorija);

            string jsonKorisnici = File.ReadAllText(korisnikPutanja);
            List<Korisnik> lKorisnika = JsonConvert.DeserializeObject<List<Korisnik>>(jsonKorisnici);

            string jsonNajma = File.ReadAllText(rentaPutanja);
            List<Najam> lNajma = JsonConvert.DeserializeObject<List<Najam>>(jsonNajma);

            int odabir = -1;
            bool izbornik = true;
            while (izbornik) {
                while (odabir == -1)
                {
                    Console.WriteLine("Unesi odabir: 1 - ispis statistike, 2 - pretragu statistike, 3 - povratak");
                    string input = Console.ReadLine();

                    if (!int.TryParse(input, out odabir))
                    {
                        Console.WriteLine("Unos mora biti cijeli broj.");
                        odabir = -1;
                    }
                }

                switch (odabir)
                {
                    case 1:

                        int ukupnaSumaAutomobila = 0;
                        int ukupnaSumaMotocikala = 0;
                        int ukupnaSumaBicikala = 0;
                        int ukupniPrihodi;
                        var tableData = new List<List<object>>();
                        foreach (Najam n in lNajma)
                        {
                            if (n.povrat)
                            {
                                foreach (Vozila v in lVozila)
                                {
                                    if (n.idVozila == v.id)
                                    {
                                        if (v.tipVozila == "Auto")
                                        {
                                            ukupnaSumaAutomobila += n.ukupniIznosTroskovaRente;
                                        }
                                        else if (v.tipVozila == "Motor")
                                        {
                                            ukupnaSumaMotocikala += n.ukupniIznosTroskovaRente;
                                        }

                                        else if (v.tipVozila == "Bicikl")
                                        {
                                            ukupnaSumaBicikala += n.ukupniIznosTroskovaRente;
                                        }

                                    }
                                }
                            }
                        }
                        ukupniPrihodi = ukupnaSumaAutomobila + ukupnaSumaMotocikala + ukupnaSumaBicikala;
                        tableData.Add(new List<object>() { ukupnaSumaAutomobila + "€", ukupnaSumaMotocikala + "€", ukupnaSumaBicikala + "€", ukupniPrihodi + "€" });

                        ConsoleTableBuilder
                            .From(tableData)
                            .WithTitle("Prihodi po tipu vozila")
                            .WithColumn("Ukupni prihodi od najma automobila", "Ukupni prihodi od najma motocikala", "Ukupni prihodi od najma bicikala", "Ukupni prihodi svih najmova zajedno")
                            .ExportAndWriteLine();


                        int[] sumaPoKategorijama = new int[lKategorija.Count];
                        for (int i = 0; i < sumaPoKategorijama.Length; i++)
                        {
                            sumaPoKategorijama[i] = 0;
                        }

                        foreach (Najam n in lNajma)
                        {
                            if (n.povrat)
                            {
                                foreach (Vozila v in lVozila)
                                {
                                    if (n.idVozila == v.id)
                                    {
                                        foreach (Kategorija k in lKategorija)
                                        {

                                            if (v.kategorija == k.id)
                                            {
                                                sumaPoKategorijama[k.id] += n.ukupniIznosTroskovaRente;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        var tableDataSumaPoKat = new List<List<object>>();
                        List<object> poKat = new List<object>();

                        foreach (var element in sumaPoKategorijama)
                        {
                            poKat.Add(element);
                        }

                        tableDataSumaPoKat.Add(poKat);

                        var headers = lKategorija
                            .OrderBy(k => k.id)
                            .Select(k => k.kategorija)
                            .ToList();
                        ConsoleTableBuilder.From(tableDataSumaPoKat).WithTitle("Prihodi po tipu vozila").WithColumn(headers).ExportAndWriteLine();
                        izbornik = false;
                        break;
                    case 2:
                        string[] searchVozila = new string[lVozila.Count];
                        var tableDataSearch = new List<List<object>>();
                        var tableDataSearchNajmovi = new List<List<object>>();

                        for (int i = 0; i < lVozila.Count; i++)
                        {
                            foreach(Vozila v in lVozila) {
                                if(i == v.id) {
                                    searchVozila[i] = (
                                    lVozila[i].marka + " " +
                                    lVozila[i].model + " " +
                                    lVozila[i].motor + " " +
                                    lVozila[i].godina
                                    ).ToLower();
                                }

                            }

                        }
                        Console.WriteLine("Unesi unos za pretragu");
                        string unosPRetraga = Console.ReadLine();
                        unosPRetraga = unosPRetraga.ToLower();

                        for(int i=0; i < searchVozila.Length; i++) { 

                            if(searchVozila[i].Contains(unosPRetraga)){
                                foreach(Vozila v in lVozila)
                                {
                                    if (i == v.id)
                                    {
                                        tableDataSearch.Add(new List<object>()
                                                {v.id, v.tipVozila, v.marka, v.model, v.motor, v.snaga+"KW", lKategorija.First(k => k.id == v.kategorija).kategorija, v.KM, v.registracija, v.cjena+"€"});

                                        foreach(Najam n in lNajma)
                                        {
                                            if(n.idVozila == v.id)
                                            {
                                                var korisnik = lKorisnika.FirstOrDefault(k => k.id == n.idKorisnika);

                                                tableDataSearchNajmovi.Add(new List<object>()
                                                { n.id,korisnik.ime,korisnik.prezime,n.odDatuma,n.doDatuma,v.marka,v.model,n.ukupniIznosTroskovaRente});
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        ConsoleTableBuilder.From(tableDataSearch).WithTitle("Rezultat").WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cjena").ExportAndWriteLine();
                        ConsoleTableBuilder.From(tableDataSearchNajmovi).WithTitle("NNajmovi pretrazivanog vozila").WithColumn("ID najma", "Ime korisnika", "Prezime korisnika", "Od datuma", "Do datuma", "Marka vozila", "Model vozila", "Cijena rente").ExportAndWriteLine();

                        izbornik = false;
                        break;
                    case 3:
                        izbornik = false;
                        break;
                    default:
                        Console.WriteLine("Odabir netocan");
                        izbornik = true;
                        break;
                }
            }

            Console.WriteLine("ENTER za povratak");
            Console.ReadLine();
        }








        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_MAXIMIZE = 3; 



        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;


            string kategorijePutanja = "C:\\Users\\Akljucar\\source\\repos\\VUV-RENT\\VUV-RENT\\kategorija.json";
            string vozilaPutanja = "C:\\Users\\Akljucar\\source\\repos\\VUV-RENT\\VUV-RENT\\vozila.json";
            string korisnikPutanja = "C:\\Users\\Akljucar\\source\\repos\\VUV-RENT\\VUV-RENT\\korisnik.json";
            string rentaPutanja = "C:\\Users\\Akljucar\\source\\repos\\VUV-RENT\\VUV-RENT\\najam.json";

            IntPtr handle = GetConsoleWindow();

            ShowWindow(handle, SW_MAXIMIZE);

            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            int idTrenutnogKorisnika =-1;
            logAfReg:
            string json = "";
            StreamReader sr = new StreamReader(korisnikPutanja); 
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
                        povratVozila(rentaPutanja, vozilaPutanja, korisnikPutanja);
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
                                StreamReader srV = new StreamReader(vozilaPutanja); 
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
                                azuriranje(vozilaPutanja, kategorijePutanja);
                                break;
                            case "3":

                                Console.Clear();
                                brisanje(vozilaPutanja, kategorijePutanja);
                                break;
                            default:
                                Console.WriteLine("Neispravan odabir ili nemate ovlasti.");
                                break;
                        }
                        break;
                    case "7" when trenutniKorisnik.Admin:
                        Console.Clear();
                        Console.WriteLine("7 - Statistika");
                        statistika(vozilaPutanja, kategorijePutanja , korisnikPutanja, rentaPutanja);
                        
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


