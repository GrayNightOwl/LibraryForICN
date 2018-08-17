using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LibraryForICN
{

    public class AdressForCompile //Данный класс существует в качестве структуры данных, хранящей адрес, используется для сборки и разборки адреса
    {
        public bool CorrectAdress; //признаки корректности адреса, устанавливается в "false" в случае отстутствия названия улицы или номера дома
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

            result = result + adress.index + ", "; //индекс и регион назначаются по умолчанию, поэтому их можно присоединять к строке
            result = result + adress.region + ", "; // без опасений в нулевых значениях, ставить после них запятые

            if (adress.area != "") //данная проверка необходима, чтобы добавлять запятую только в необходимых случаях
            {                       //значение по умолчанию для района не ставится, нужно иметь городские и сельские адреса
                result = result + adress.area + ", "; 
            }
            result = result + "г. " + adress.city + ", "; //город назначается по умолчанию, добавляем без раздумий
            if (adress.CorrectAdress == true) //если адрес корректный - то улица и дом распознаны, прибавим их
            {
                result = result + "ул. " + adress.street + ", ";
                result = result + "д. " + adress.house;
                if (adress.flat != "Не удалось распознать квартиру")
                {
                    result = result + ", кв. " + adress.flat;
                }
            }
            else { result = "Некорректный адрес"; }; //иначе вернём ошибку распознавания

            return result;
        }

        public AdressForCompile ParseAdress(string s) 
        {
            AdressForCompile result = new AdressForCompile();
            result.CorrectAdress = true; //изначально считаем адрес корректным
            s = "," + s + ",";           //"окаймим" строку запятыми для отделения участков по запятым с двух сторон
            result.index = Index(s);     //может вернуть индекс по умолчанию
            result.region = Region(s);   //может вернуть регион по умолчанию
            result.area = Area(s);       //не вернёт район по умолчанию, иначе все адреса будут сельскими
            result.city = City(s);       //может вернуть город по умолчанию
            result.street = Street(s);   //если не указано - вернуть ошибку
            result.house = House(s);     //если не указано - вернуть ошибку
            if ((result.street == "Не удалось распознать улицу") || (result.house == "Не удалось распознать дом"))
            {
                result.CorrectAdress = false; //в случае отсутствия улицы/дома считаем адрес некорректным
                return result;
            }
            else
                result.flat = Flat(s);   //не вернёт значение по умолчанию, но адрес может существовать и без квартиры
            return result;
        }







        private string Index(string s) 
        {
            string index = "";                              //ищем участок следующего вида: 
            Regex regex1 = new Regex(@",(\s*\d+\s*),+.*");  //запятая, любое количество пробелов, минимум одна цифра, любое количетсво пробелов, запятая, последующий текст
            index = MatchWithOneRegex(regex1, s);           //применяем регулярное выражение к строке
            if (index == "") index = "Не удалось распознать индекс";
            return index.Trim();                            //вернём значение, лишённое пробелов с левой и правой стороны
        }
        

        private string Region(string s)
        {
            string region = "";                                                             //разделено на 2 части:
            Regex regex1 = new Regex(@",(\s*(?:[А-я]|-)*\s+(?:кр|о|Кр)+[А-я]*\s*),+.*");       //запятая, пробелы, название, признак кр/о, дополненный до конца, пробелы, запятая, окончание
            Regex regex2 = new Regex(@",(\s*(?:кр|о|Кр)+[А-я]*\s+(?:[А-я]|-)*)+\s*,+.*");      //запятая, признак кр/о, дополенный до конца, пробел, название, пробелы, запятая, окончание
            Regex regex3 = new Regex(@",(\s*(?:кр|о|Кр)(?:[а-я]|-)*\s*[А-Я](?:[А-я]|-|\s)+)\s*,+.*"); //запятая, признак кр/о, дополенный до конца из маленьких букв, Большая буква, окончание названия, запятая
            region = MatchWithTwoRegex(regex1, regex2, s);  //применяем 2 регулярных выражения
            if (region == "")                               //если поиск по пробелу ничего не дал  - применим поиск по большой букве
            {
                region = MatchWithOneRegex(regex3, s);
            }
            if (region == "") region = "Пермский край";     //если распознать регион не удалось- назначим регион по умолчанию
            return region.Trim();                           //вернём значение, лишённое пробелов с левой и правой стороны
        }



        private string Area(string s) //код аналогичен распознаванию региона, признак кр/о (край/область) заменён на "р" от "район"
        {
            string area = "";
            Regex regex1 = new Regex(@",(\s*(?:[А-я]|-)*\s+р[А-я]*\s*),+.*");
            Regex regex2 = new Regex(@",(\s*район[А-я]*\s+(?:[А-я]|-)*)+\s*,+.*");
            Regex regex3 = new Regex(@",(\s*р(?:[а-я]|-)*\s*[А-Я](?:[А-я]|-|\s)+)\s*,+.*");
            area = MatchWithTwoRegex(regex1, regex2, s);
            if (area == "")
            {
                area = MatchWithOneRegex(regex3, s);
            }
            return area.Trim();
        }

       private string City(string s)
        {
            string city = "";
            Regex regex1 = new Regex(@",\s*(?:г|Г|дер|Дер|п)(?:[а-я]|-)*(?:\.|\s)+((?:[А-я]|-|\s)+)\s*,+.*");
            Regex regex2 = new Regex(@",\s*(?:г|Г|дер|Дер|п)(?:[а-я]|-)*\s*([А-Я](?:[А-я]|-|\s)+)\s*,+.*");
            city = MatchWithTwoRegex(regex1, regex2, s); //если ошибка- вернётся пустая строка
            if (city == "") city = "Пермь"; //если распознавание ничего не дало- вернём значение по умолчанию для города
            return city.Trim();
        }


        private string Street(string s)
        {
            string street = "";
            Regex regex1 = new Regex(@",\s*(?:у|У)(?:[а-я]|-)*(?:\.|\s)+((?:[0-9]|[А-я]|-|\s)+)\s*,+.*"); //добавлены цифры
            Regex regex2 = new Regex(@",\s*(?:у|У)(?:[а-я]|-)*\s*([А-Я](?:[А-я]|-|\s)+)\s*,+.*");
            street = MatchWithTwoRegex(regex1, regex2, s);
            if (street == "") street = "Не удалось распознать улицу";
            return street.Trim();
        }

        private string House(string s)
        {
            string house;
            Regex regex1 = new Regex(@",\s*(?:д|Д)(?:[А-я]|-)*(?:\.|\s)*(\d+(?:/|\s|[А-я])?\d*)\s*,+.*");
            MatchCollection matches = regex1.Matches(s);
            house = MatchWithOneRegex(regex1, s);
            if (house == "") house = "Не удалось распознать дом";
            return house.Trim();
        }

        private string Flat(string s)
        {
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
