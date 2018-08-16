using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LibraryForICN
{
    public class Adress
    {
        public string CompileAdress(string index, string region, string city, string street, string house, string flat)
        {
            string adress = "";
            adress += index + ", " + region + ", г. " + city + ", ул. " + street + ", д. " + house + ", кв. " + flat;
            return adress;
        }

        public string CompileAdress(string index, string region, string area, string city, string street, string house, string flat)
        {
            string adress = "";
            adress += index + ", " + region + ", " + area + ", г. " + city + ", ул. " + street + ", д. " + house + ", кв. " + flat;
            return adress;
        }






        public string Street(string s)
        {
            s = "," + s + ",";
            string street = "Не удалось распознать улицу";
            Regex regex1 = new Regex(@",\s*(?:у|У)(?:[а-я]|-)*(?:\.|\s)+((?:[0-9]|[А-я]|-|\s)+)\s*,+.*"); //добавлены цифры
            Regex regex2 = new Regex(@",\s*(?:у|У)(?:[а-я]|-)*\s*([А-Я](?:[А-я]|-|\s)+)\s*,+.*");
            MatchCollection matches = regex1.Matches(s);

            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    street = match.Groups[1].Value;
                }
            }
            else
            {
                MatchCollection matches2 = regex2.Matches(s);
                if (matches2.Count > 0)
                {
                    foreach (Match match in matches2)
                    {
                        street = match.Groups[1].Value;
                    }
                }
                //else
                //    street = "Совпадений не найдено";
            }
            //if (street.IndexOf('-') !=(-1))
            //{
            //    street = street.Replace(" ", "");
            //}
            return street.Trim();
        }

        public string City(string s)
        {
            s = "," + s + ",";
            string city = "Не удалось распознать город";

            Regex regex1 = new Regex(@",\s*(?:г|Г)(?:[а-я]|-)*(?:\.|\s)+((?:[А-я]|-|\s)+)\s*,+.*");
            Regex regex2 = new Regex(@",\s*(?:г|Г)(?:[а-я]|-)*\s*([А-Я](?:[А-я]|-|\s)+)\s*,+.*"); 
            MatchCollection matches = regex1.Matches(s);


            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    city = match.Groups[1].Value;
                }
            }
            else
            {
                MatchCollection matches2 = regex2.Matches(s);
                if (matches2.Count > 0)
                {
                    foreach (Match match in matches2)
                    {
                        city = match.Groups[1].Value;
                    }
                }
                else
                    city = "Пермь";
            }
            return city.Trim();
        }

        public string House(string s)
        {
            s = "," + s + ",";
            string house = "Не удалось распознать дом";

            Regex regex1 = new Regex(@",\s*(?:д|Д)(?:[А-я]|-)*(?:\.|\s)*(\d+(?:/|\s|[А-я])?\d*)\s*,+.*");
            MatchCollection matches = regex1.Matches(s);


            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    house = match.Groups[1].Value;
                }
            }
            return house.Trim();
        }

        public string Flat(string s)
        {
            s = "," + s + ",";
            string flat = "Не удалось распознать квартиру";

            Regex regex1 = new Regex(@",\s*(?:к|К)(?:[А-я]|-)*(?:\.|\s)*(\d+)\s*,+.*");
            MatchCollection matches = regex1.Matches(s);


            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    flat = match.Groups[1].Value;
                }
            }
            return flat.Trim();
        }


        public string Region(string s)
        {
            s = "," + s + ",";
            string region = "Не удалось распознать регион";
            Regex regex1 = new Regex(@",(\s*(?:[А-я]|-)*\s*(?:кр|КР|Кр|кР|о)+[А-я]*\s*(?:[А-я]|-)*)+\s*,+.*");
            Regex regex2 = new Regex(@",\s*(?:кр|Кр|КР|о)(?:[а-я]|-)*\s*([А-Я](?:[А-я]|-|\s)+)\s*,+.*");
            
          MatchCollection matches = regex1.Matches(s);

            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    region = match.Groups[1].Value;
                }
            }
            else
            {
                MatchCollection matches2 = regex2.Matches(s);
                if (matches2.Count > 0)
                {
                    foreach (Match match in matches2)
                    {
                        Console.WriteLine("Вторая ветвь");
                        region = match.Groups[1].Value;
                    }
                }
                else
                    region = "Пермский край";
            }
            return region.Trim();
        }

    }
}
