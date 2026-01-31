
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
        static Korisnik Register(int b, string korisnikPutanja)
        {
            Korisnik noviUnos = new Korisnik();
            string jsonKorisnik = File.ReadAllText(korisnikPutanja);
            List<Korisnik> lKorisnika = JsonConvert.DeserializeObject<List<Korisnik>>(jsonKorisnik);

            noviUnos.id = lKorisnika[lKorisnika.Count - 1].id+1;
            noviUnos.ime = "";
            noviUnos.korisnickoIme = "";
            noviUnos.prezime = "";
            noviUnos.password = "";
            noviUnos.OIB = "";
            while (noviUnos.ime == "") {
                Console.WriteLine("Ime:");
                noviUnos.ime = Console.ReadLine();
            }
            while (noviUnos.prezime == "") {
                Console.WriteLine("Prezime:");
                noviUnos.prezime = Console.ReadLine();
            }
            while (noviUnos.korisnickoIme == "") {
                Console.WriteLine("Username ili korisničko ime: ");
                noviUnos.korisnickoIme = Console.ReadLine();
            }
            while (noviUnos.password == "") {
                Console.WriteLine("Lozinka: ");
                string password = Console.ReadLine();
                Console.WriteLine("Potvrda lozinke: ");
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
                Console.WriteLine("Unesite vaš OIB:");
                noviUnos.OIB = Console.ReadLine();

                if (noviUnos.OIB.Length != 11)
                {
                    Console.WriteLine("OIB mora sadržavati točno 11 znamenki.");
                    noviUnos.OIB = "";
                    continue;
                }

                long oibBroj;
                if (!long.TryParse(noviUnos.OIB, out oibBroj))
                {
                    Console.WriteLine("OIB smije sadržavati samo brojeve.");
                    noviUnos.OIB = "";
                    continue;
                }

                bool postoji = false;
                foreach (Korisnik k in lKorisnika)
                {
                    if (k.OIB == noviUnos.OIB)
                    {
                        postoji = true;
                        break;
                    }
                }

                if (postoji)
                {
                    Console.WriteLine("Korisnik s tim OIB-om već postoji.");
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

        
        

        static void PonudaVozila(string ponudaVozila, string putanjaKategorija) {
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
                .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cijena")
                .ExportAndWriteLine();


            Console.WriteLine("Pritisnite ENTER za povrat");
            Console.ReadLine();
            Console.Clear();

        }















        static void PretrazivanjeVozila(string putanjaVozila, string putanjaKategorija) {

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

            Console.Clear();
            while (izbornikPretAktivan)
            {
                Console.Clear();
                Console.WriteLine("\n--- Izbornik ---");
                Console.WriteLine("\n1 - Pretraga");
                Console.WriteLine("\n--- Filteri ---");
                Console.WriteLine("\n2- Kategorija");
                Console.WriteLine("3 - Snaga vozila");
                Console.WriteLine("4 - Kilometrazi");
                Console.WriteLine("5 - Godini proizvodnje");
                Console.WriteLine("6 - Cjeni rente");
                Console.WriteLine("7 - Povrat");
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
                        string[] searchVozila = new string[lVozila.Count];
                        var tableDataSearch = new List<List<object>>();
                        var tableDataSearchNajmovi = new List<List<object>>();

                        for (int i = 0; i < lVozila.Count; i++)
                        {
                            foreach (Vozila v in lVozila)
                            {
                                if (i == v.id)
                                {
                                    searchVozila[i] = (
                                    lVozila[i].marka + " " +
                                    lVozila[i].model + " " +
                                    lVozila[i].motor + " " + 
                                    lKategorija.First(k => k.id == v.kategorija).kategorija
                                    ).ToLower();
                                }

                            }

                        }

                        Console.WriteLine("Unesi unos za pretragu (Moguca pretrga po: Marka, Model, Motor, Kategorija): ");
                        string unosPRetraga = Console.ReadLine();
                        unosPRetraga = unosPRetraga.ToLower();

                        for (int i = 0; i < searchVozila.Length; i++)
                        {

                            if (searchVozila[i].Contains(unosPRetraga))
                            {
                                foreach (Vozila v in lVozila)
                                {
                                    if (i == v.id)
                                    {
                                        tableDataSearch.Add(new List<object>(){v.id, v.tipVozila, v.marka, v.model, v.motor, v.snaga+"KW", lKategorija.First(k => k.id == v.kategorija).kategorija, v.KM, v.registracija, v.cjena+"€"});                                      
                                    }
                                }
                            }
                        }
                        ConsoleTableBuilder.From(tableDataSearch).WithTitle("Rezultat").WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cijena").ExportAndWriteLine();
                        Console.WriteLine("Pritisni ENTER za povratak.");
                        Console.ReadLine();
                        break;
                    case 2:

                    case 3:
                        Console.Clear();          
                        bool izbornikSnageVozila = true;
                        while (izbornikSnageVozila)
                        {
                            Console.WriteLine("--- Pretraga po snazi vozila ---");
                            Console.WriteLine("Povrat? Da -> povrat");
                            string povratakODG = Console.ReadLine();
                            if (povratakODG.ToLower() == "da")
                            {
                                Console.Clear();
                                izbornikSnageVozila = false;
                            }
                            else {
                                Console.WriteLine("Upišite od koje do koje snage vozila želite, snaga izražena u KW.");
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
                                    if (odSnage < 0)
                                    {
                                        Console.WriteLine("Unos mora biti pozitivan. Pokušaj ponovno.");
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
                                    if (doSnage < 0)
                                    {
                                        Console.WriteLine("Unos mora biti pozitivan. Pokušaj ponovno.");
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
                                    .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cijena")
                                    .ExportAndWriteLine();
                                Console.WriteLine("Pritisnite enter za povratak");
                                Console.ReadLine();
                                izbornikSnageVozila = false;
                                Console.Clear();
                            }
                        }


                            
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("6 - Kilometrazi");
                        bool izbornikKilometrazeVozila = true;
                        while (izbornikKilometrazeVozila)
                        {
                            Console.WriteLine("--- Filtriranje po kilometraži vozila ---");
                            Console.WriteLine("Povrat? Da -> povrat");
                            string povratakODG = Console.ReadLine();
                            if (povratakODG.ToLower() == "da")
                            {
                                Console.Clear();
                                izbornikKilometrazeVozila = false;
                            }
                            else
                            {
                                Console.WriteLine("Upišite od koje do koje kilometraža vozila želite");
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
                                    if (odKM < 0)
                                    {
                                        Console.WriteLine("Unos mora biti pozitivan. Pokušaj ponovno.");
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
                                    if (doKM < 0)
                                    {
                                        Console.WriteLine("Unos mora biti pozitivan. Pokušaj ponovno.");
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
                                        .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cijena")
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
                        
                    case 5:
                        Console.Clear();
                        bool izbornikGodinaVozila = true;
                        while (izbornikGodinaVozila)
                        {
                            Console.WriteLine("--- Filtriranje po godini proizvodnje ---");
                            Console.WriteLine("Povrat? Da -> povrat");
                            string povratakODG = Console.ReadLine();
                            if (povratakODG.ToLower() == "da")
                            {
                                Console.Clear();
                                izbornikGodinaVozila = false;
                                break;
                            }
                            else {
                                Console.WriteLine("Upišite od koje do koje godine proizvodnje vozila želite");
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
                                    if (odGodine<0)
                                    {
                                        Console.WriteLine("Unos mora biti pozitivan. Pokušaj ponovno.");
                                        odGodine = -1;
                                    }
                                }
                                int doGodine = -1;
                                while (doGodine == -1)
                                {
                                    Console.WriteLine("DO: ");
                                    string input = Console.ReadLine();
                                    if (!int.TryParse(input, out doGodine))
                                    {
                                        Console.WriteLine("Unos mora biti cijeli broj. Pokušaj ponovno.");
                                        doGodine = -1;
                                    }
                                    if (doGodine<0)
                                    {
                                        Console.WriteLine("Unos mora biti pozitivan. Pokušaj ponovno.");
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
                                    .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cijena")
                                    .ExportAndWriteLine();
                                Console.WriteLine("Pritisnite enter za povratak");
                                Console.ReadLine();
                                izbornikGodinaVozila = false;
                                Console.Clear();
                            }
                        }
                        break;
                    case 6:
                        Console.Clear();
                        bool izbornikCjeneVozila = true;
                        while (izbornikCjeneVozila)
                        {
                            Console.WriteLine("--- Filtriranje po cijeni najma ---");
                            Console.WriteLine("Povrat? Da -> povrat");
                            string povratakODG = Console.ReadLine();
                            if (povratakODG.ToLower() == "da")
                            {
                                Console.Clear();
                                izbornikCjeneVozila = false;
                            }
                            else
                            {
                                Console.WriteLine("Upišite od koje do koje cijene najma želite filtrirati");
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
                                    .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cijena")
                                    .ExportAndWriteLine();
                                Console.WriteLine("Pritisnite enter za povratak");
                                Console.ReadLine();
                                izbornikCjeneVozila = false;
                                Console.Clear();

                            }
                        }
                        break;
               
                    case 7:
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
                Console.WriteLine("OD ovog datuma počine renta (d-m-g):");
                if (!DateTime.TryParse(Console.ReadLine(), out odDatuma))
                {
                    Console.WriteLine("Neispravan format datuma. Pokušaj ponovno.");
                    odDatuma = DateTime.MinValue;
                }
                if (odDatuma < DateTime.Now)
                {
                    Console.WriteLine("Nije moguće rentati vozilo u prošlosti");
                    odDatuma = DateTime.MinValue;
                }

            }


            DateTime doDatuma = DateTime.MinValue;
            while (doDatuma == DateTime.MinValue)
            {
                Console.WriteLine("Ovim datumom ističe vaša renta (d-m-g):");
                if (!DateTime.TryParse(Console.ReadLine(), out doDatuma))
                {
                    Console.WriteLine("Neispravan format datuma. Pokušaj ponovno.");
                    doDatuma = DateTime.MinValue;
                }
                if (doDatuma < DateTime.Now)
                {
                    Console.WriteLine("Nije moguće rentati vozilo u prošlosti");
                    doDatuma = DateTime.MinValue;
                }
                if(doDatuma < odDatuma)
                {
                    Console.WriteLine("Datum “DO” ne može biti raniji od datuma “OD”. Molimo odaberite ispravan raspon datuma.");
                    doDatuma = DateTime.MinValue;
                }
            }


            var tableData11 = new List<List<object>>();
            var slobodniIdVozila = new List<int>();

            foreach (Vozila v in lVozila)
            {
                if (!v.dostupnost)
                    continue;

                bool zauzeto = false;

                foreach (Najam renta in lRenta)
                {
                    if (renta.idVozila == v.id)
                    {
                        DateTime rentaOd = renta.odDatuma;
                        DateTime rentaDo = renta.doDatuma;

                        if (rentaOd <= doDatuma && rentaDo >= odDatuma)
                        {
                            zauzeto = true;
                            break;
                        }
                    }
                }
                if (!zauzeto)
                {
                    tableData11.Add(new List<object>{v.id,v.tipVozila,v.marka,v.model,v.motor,v.snaga,lKategorija.First(k => k.id == v.kategorija).kategorija,v.KM,v.registracija,v.cjena});

                    slobodniIdVozila.Add(v.id);
                }
            }
            ConsoleTableBuilder
                .From(tableData11)
                .WithTitle("Dostupna vozila unutar odabranog perioda")
                .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cijena")
                .ExportAndWriteLine();

            int rentVoziloID;

            while (true)
            {
                Console.Write("Unesi ID vozila koje želite iznajmiti: ");
                string unos = Console.ReadLine();

                if (!int.TryParse(unos, out rentVoziloID) || rentVoziloID <= 0)
                {
                    Console.WriteLine("Unos mora biti pozitivan cijeli broj.");
                    continue;
                }

                if (!slobodniIdVozila.Contains(rentVoziloID))
                {
                    Console.WriteLine("Uneseni ID nije među dostupnim vozilima. Molimo odaberite ID iz tablice.");
                    continue;
                }
                break; 
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


            ZapisUNajam(rentaPutanja, rentVoziloID, trenutniKorisnik, odDatuma, doDatuma, ukupnaCjena);
            Console.Clear();
            Console.WriteLine("Rent uspjesan!!!");
            Console.WriteLine("Pritisnite enter za povratak");
            Console.ReadLine();

        }
















        static void ZapisUNajam(string rentaPutanja, int rentVoziloID, int trenutniKorisnik, DateTime odDatuma, DateTime doDatuma, int ukupnaCjena)
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



















        static void PovratVozila(string najamPutanja, string vozilaPutanja, string korisnikPutanja)
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
                if(!n.povrat) { 
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
            Console.WriteLine("ENTER za povratak");
            Console.ReadLine();
        }


















        static Vozila Dodavanje(string vozilaPutanja)
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
                Console.WriteLine("1 - Auto");
                Console.WriteLine("2 - Motor");
                Console.WriteLine("3 - Bicikl");

                int tipVozila = -1;
                while (tipVozila == -1)
                {
                    Console.WriteLine("Odabir (Unos mora biti broj):");
                    string unosOdabira = Console.ReadLine();
                    if (!int.TryParse(unosOdabira, out tipVozila))
                    {
                        Console.WriteLine("Unos mora biti cjeli broj.");
                        tipVozila = -1;
                    }
                    if (tipVozila < -1 || tipVozila>3)
                    {
                        Console.WriteLine("Odabrali ste nešto što nije ponuđeno.");
                        tipVozila = -1;
                    }                
                }

                if(tipVozila == 1)
                {
                    novoVozilo.tipVozila = "Auto";
                }else if (tipVozila == 2)
                {
                    novoVozilo.tipVozila = "Motor";
                }else if (tipVozila == 3)
                {
                    novoVozilo.tipVozila = "Bicikl";
                }

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
                    if (novoVozilo.tipVozila == "Auto")
                    {
                        Console.WriteLine("1 - Avant");
                        Console.WriteLine("2 - Limuzina");
                        Console.WriteLine("3 - Sportback");
                        Console.WriteLine("4 - Hatchback");
                        Console.WriteLine("5 - Coupe");
                        Console.WriteLine("6 - SUV");
                    }
                    else if(novoVozilo.tipVozila == "Motor")
                    {
                        Console.WriteLine("7 - Sportak");
                        Console.WriteLine("8 - Skuter");
                        Console.WriteLine("9 - Naked");
                    }else if(novoVozilo.tipVozila == "Bicikl") {
                        Console.WriteLine("10 - Bicikl");
                    }

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
            if(novoVozilo.tipVozila == "Auto" || novoVozilo.tipVozila == "Motor") {
                while (novoVozilo.snaga == -1)
                {
                    Console.WriteLine("Unesi snagu vozila (izraženu u kW): ");
                    string input = Console.ReadLine();

                    if (!int.TryParse(input, out novoVozilo.snaga))
                    {
                        Console.WriteLine("Unos mora biti cijeli broj. Pokušaj ponovno.");
                        novoVozilo.snaga = -1;
                    }
                    if (novoVozilo.snaga < -1)
                    {
                        Console.WriteLine("Unos mora biti pozitivan cjeli broj. Pokušaj ponovno.");
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
                    if (novoVozilo.KM < -1)
                    {
                        Console.WriteLine("Unos mora biti pozitivan cjeli broj Pokušaj ponovno.");
                        novoVozilo.KM = -1;
                    }
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
                if (novoVozilo.godina < -1)
                {
                    Console.WriteLine("Unos mora biti pozitivan cjeli broj. Pokušaj ponovno.");
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
                if (novoVozilo.cjena < -1)
                {
                    Console.WriteLine("Unos mora biti pozitivan cjeli broj. Pokušaj ponovno.");
                    novoVozilo.cjena = -1;
                }
            }
            DateTime date = DateTime.Now.AddYears(1).Date; 
            novoVozilo.registracija = date;

            return novoVozilo;
        }













        static void SpremiVozilo(List<Vozila> lVozila, string path)
        {
            string noviJson = JsonConvert.SerializeObject(lVozila, Formatting.Indented);
            File.WriteAllText(path, noviJson);
        }















        static void Azuriranje(string vozilaPutanja, string rentaPutanja)
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
                                    Console.WriteLine("Unesi novu registraciju (dd-mm-yyyy):");
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

                    SpremiVozilo(lVozila, vozilaPutanja);
                    Console.WriteLine("Vozilo je uspješno ažurirano.");
                    return;
                }
            }

            Console.WriteLine("Vozilo s tim ID-em nije pronađeno.");
            Console.ReadLine();
        }

















        static void Brisanje(string vozilaPutanja, string kategorijaPutanja)
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
            .WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cijena")
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

                    SpremiVozilo(lVozila, vozilaPutanja);
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



        static void Statistika(string vozilaPutanja,string kategorijePutanja ,string korisnikPutanja,string rentaPutanja)
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
                        Console.WriteLine("Unesi unos za pretragu (Moguca pretrga po: Marci, Modelu, Motoru, Godini proizvodnje): ");
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
                        ConsoleTableBuilder.From(tableDataSearch).WithTitle("Rezultat").WithColumn("ID", "Tip vozila", "Marka", "Model", "Motor", "Snaga", "Kategorija", "KM", "Registracija", "Cijena").ExportAndWriteLine();
                        ConsoleTableBuilder.From(tableDataSearchNajmovi).WithTitle("Najmovi pretrazivanog vozila").WithColumn("ID najma", "Ime korisnika", "Prezime korisnika", "Od datuma", "Do datuma", "Marka vozila", "Model vozila", "Cijena rente").ExportAndWriteLine();

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
            Console.Clear();
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
                    lkorisnik.Add(Register(c, korisnikPutanja));

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
                        PonudaVozila(vozilaPutanja, kategorijePutanja);
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("2 - Pretrazivanje vozila");
                        PretrazivanjeVozila(vozilaPutanja, kategorijePutanja);
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
                        PovratVozila(rentaPutanja, vozilaPutanja, korisnikPutanja);
                        break;

                    case "6" when trenutniKorisnik.Admin:
                        Console.Clear();
                        Console.WriteLine("6 - Dodavanje/azuriranje/brisanje vozila");
                        Console.WriteLine("Odaberi što zeliš napraviti: ");
                        Console.WriteLine("1 - Dodavanje");
                        Console.WriteLine("2 - Azuriranje");
                        Console.WriteLine("3 - Brisanje");

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
                                lVozila.Add(Dodavanje(vozilaPutanja));
                                SpremiVozilo(lVozila, vozilaPutanja);

                                break;
                            case "2":

                                Console.Clear();
                                Azuriranje(vozilaPutanja, kategorijePutanja);
                                break;
                            case "3":

                                Console.Clear();
                                Brisanje(vozilaPutanja, kategorijePutanja);
                                break;
                            default:
                                Console.WriteLine("Neispravan odabir ili nemate ovlasti.");
                                break;
                        }
                        break;
                    case "7" when trenutniKorisnik.Admin:
                        Console.Clear();
                        Console.WriteLine("7 - Statistika");
                        Statistika(vozilaPutanja, kategorijePutanja , korisnikPutanja, rentaPutanja);
                        
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


