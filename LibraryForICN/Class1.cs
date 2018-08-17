using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LibraryForICN
{

    public class AdressForCompile
    {
        public bool CorrectAdress;
        public string index;
        public string region;
        public string area;
        public string city;
        public string street;
        public string house;
        public string flat;
    }

    public class Adress
    {
        public string CompileAdress(AdressForCompile adress)
        {
            string result = "";

            result = result + adress.index + ", ";
            result = result + adress.region + ", ";
            if (adress.area != "")
            {
                result = result + adress.area + ", ";
            }
            result = result + "г. " + adress.city + ", ";
            if (adress.CorrectAdress == true) //если адрес корректный - то улица и дом распознаны
            {
                result = result + "ул. " + adress.street + ", ";
                result = result + "д. " + adress.house;
                if (adress.flat != "Не удалось распознать квартиру")
                {
                    result = result + ", кв. " + adress.flat;
                }
            }
            else { result = "Некорректный адрес"; };

            return result;
        }

        public AdressForCompile ParseAdress(string s)
        {
            AdressForCompile result = new AdressForCompile();
            result.CorrectAdress = true;
            result.index = Index(s);
            result.region = Region(s); //может вернуть регион по умолчанию
            result.area = Area(s);         // result.area = Area(s); //может вернуть район по умолчанию
            result.city = City(s); //может вернуть город по умолчанию
            result.street = Street(s); //если не указано - вернуть ошибку
            result.house = House(s); //если не указано - вернуть ошибку
            if ((result.street == "Не удалось распознать улицу") || (result.house == "Не удалось распознать дом"))
            {
                result.CorrectAdress = false;
                return result;
            }
            else
                result.flat = Flat(s); //
            return result;
        }







        private string Index(string s)
        {
            s = "," + s + ",";
            string index = "";
            Regex regex1 = new Regex(@",(\s*\d+\s*),+.*");
            index = MatchWithOneRegex(regex1, s);
            if (index == "") index = "Не удалось распознать индекс";
            return index.Trim();
        }

        //private string Region(string s)
        //{
        //    s = "," + s + ",";
        //    string region = "";
        //    // string s = " д. 70/2, улСоловьева, кв. 45, край Пермский ,  614285   , город  Пермь , ";
        //    Regex regex1 = new Regex(@",(\s*(?:[А-я]|-)*\s*(?:кр|КР|Кр|кР|о)+[А-я]*\s*(?:[А-я]|-)*)+\s*,+.*");
        //    Regex regex2 = new Regex(@",(\s*(?:кр|Кр|КР|о)(?:[а-я]|-)*\s*[А-Я](?:[А-я]|-|\s)+)\s*,+.*");
        //    region = MatchWithTwoRegex(regex1, regex2, s);
        //    if (region == "") region = "Пермский край";
        //    return region.Trim();
        //}

        private string Region(string s)
        {
            s = "," + s + ",";
            string region = "";
            // string s = " д. 70/2, улСоловьева, кв. 45, край Пермский ,  614285   , город  Пермь , ";
            Regex regex1 = new Regex(@",(\s*(?:[А-я]|-)*\s+(?:кр|о)+[А-я]*\s*),+.*");
            Regex regex2 = new Regex(@",(\s*(?:кр|о)+[А-я]*\s+(?:[А-я]|-)*)+\s*,+.*");
            Regex regex3 = new Regex(@",(\s*(?:кр|о)(?:[а-я]|-)*\s*[А-Я](?:[А-я]|-|\s)+)\s*,+.*");
            region = MatchWithTwoRegex(regex1, regex2, s);
            if (region == "")
            {
                region = MatchWithOneRegex(regex3, s);
            }
            if (region == "") region = "Пермский край";
            return region.Trim();
        }



        private string Area(string s)
        {
            s = "," + s + ",";
            string area = "";
            // string s = " д. 70/2, улСоловьева, кв. 45, край Пермский ,  614285   , город  Пермь , ";
            Regex regex1 = new Regex(@",(\s*(?:[А-я]|-)*\s+р[А-я]*\s*),+.*");
            Regex regex2 = new Regex(@",(\s*район[А-я]*\s+(?:[А-я]|-)*)+\s*,+.*");
            Regex regex3 = new Regex(@",(\s*р(?:[а-я]|-)*\s*[А-Я](?:[А-я]|-|\s)+)\s*,+.*");
            area = MatchWithTwoRegex(regex1, regex2, s);
            if (area == "")
            {
                area = MatchWithOneRegex(regex3, s);
            }
            //if (area == "") area = "Пермский район"; 
            return area.Trim();
        }


        //private string Area(string s)
        //{
        //    s = ", " + s + " ,";
        //    string area = "";
        //    // string s = "614000, кв4 , дом 15,   улица. 1-я Красноармейская   , г. Москва,   край Пермский,район Пермский ,"
        //    // string s = "614000, кв4 , дом 15,   улица. 1-я Красноармейская   , г. Москва,   край Пермский, Пермский район , "
        //    //Regex regex1 = new Regex(@"(,\s*(?:[А-я]|-)*(?:,р|\s+р)[А-я]*\s*(?:[А-я]|-)*\s*),+.*")
        //    Regex regex1 = new Regex(@",\s*");
        //    Regex regex2 = new Regex(@",\s*р(?:[а-я]|-)*\s+([А-Я](?:[А-я]|-|\s)+)\s*,+.*");
        //    area = MatchWithTwoRegex(regex1, regex2, s);
        //    area.Replace(",", "");
        //    return area.Trim();
        //}

        private string City(string s)
        {
            s = "," + s + ",";
            string city = "";

            Regex regex1 = new Regex(@",\s*(?:г|Г)(?:[а-я]|-)*(?:\.|\s)+((?:[А-я]|-|\s)+)\s*,+.*");
            Regex regex2 = new Regex(@",\s*(?:г|Г)(?:[а-я]|-)*\s*([А-Я](?:[А-я]|-|\s)+)\s*,+.*");
            city = MatchWithTwoRegex(regex1, regex2, s); //если ошибка- вернётся пустая строка
            if (city == "") city = "Пермь"; //если распознавание ничего не дало- вернём значение по умолчанию для города
            return city.Trim();
        }


        private string Street(string s)
        {
            s = "," + s + ",";
            string street = "";
            Regex regex1 = new Regex(@",\s*(?:у|У)(?:[а-я]|-)*(?:\.|\s)+((?:[0-9]|[А-я]|-|\s)+)\s*,+.*"); //добавлены цифры
            Regex regex2 = new Regex(@",\s*(?:у|У)(?:[а-я]|-)*\s*([А-Я](?:[А-я]|-|\s)+)\s*,+.*");
            street = MatchWithTwoRegex(regex1, regex2, s);
            if (street == "") street = "Не удалось распознать улицу";
            return street.Trim();
        }

        private string House(string s)
        {
            s = "," + s + ",";
            string house;

            Regex regex1 = new Regex(@",\s*(?:д|Д)(?:[А-я]|-)*(?:\.|\s)*(\d+(?:/|\s|[А-я])?\d*)\s*,+.*");
            MatchCollection matches = regex1.Matches(s);
            house = MatchWithOneRegex(regex1, s);
            if (house == "") house = "Не удалось распознать дом";
            return house.Trim();
        }

        private string Flat(string s)
        {
            s = "," + s + ",";
            string flat = "";
            Regex regex1 = new Regex(@",\s*(?:к|К)(?:[А-я]|-)*(?:\.|\s)*(\d+)\s*,+.*");
            flat = MatchWithOneRegex(regex1, s);
            if (flat == "") flat = "Не удалось распознать квартиру";
            return flat.Trim();
        }



        private string MatchWithTwoRegex(Regex regex1, Regex regex2, string s)
        {
            string result = "";
            MatchCollection matches = regex1.Matches(s);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    result = match.Groups[1].Value;
                }
            }
            else
            {
                MatchCollection matches2 = regex2.Matches(s);
                if (matches2.Count > 0)
                {
                    foreach (Match match in matches2)
                    {
                        result = match.Groups[1].Value;
                    }
                }
            }
            return result;
        }

        private string MatchWithOneRegex(Regex regex1, string s)
        {
            string result = "";
            MatchCollection matches = regex1.Matches(s);


            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    result = match.Groups[1].Value;
                }
            }
            return result;
        }


    }
}
