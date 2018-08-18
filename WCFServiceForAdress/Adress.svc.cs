using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using LibraryForICN;
using System.Text.RegularExpressions;

namespace WCFServiceForAdress
{
    public class Service1 : IntAddress
    {
        AdressFromLIB addr = new AdressFromLIB();

        public string CompileAddressFromSet(string index, string region, string area, string city, string street, string house, string flat)
        {   //создаёт объект типа Адрес, передаёт в функцию сборки из объекта 

            AddressStructure addressStructure = new AddressStructure();
            addressStructure.CorrectAddress = true;
            addressStructure.index = index;
            addressStructure.region = region;
            addressStructure.area = area;
            addressStructure.city = city;
            addressStructure.street = street;
            addressStructure.house = house;
            addressStructure.flat = flat;

            return addr.CompileAddress(addressStructure);

        }

        public string[] ParseAddress(string s)
        {

            string[] mas = new string[7];
            AddressStructure result = new AddressStructure();
            result.CorrectAddress = true; //изначально считаем адрес корректным
            s = "," + s + ",";           //"окаймим" строку запятыми для отделения участков по запятым с двух сторон
            result.index = Index(s);     //может вернуть индекс по умолчанию
            result.region = Region(s);   //может вернуть регион по умолчанию
            result.area = Area(s);       //не вернёт район по умолчанию, иначе все адреса будут сельскими
            result.city = City(s);       //может вернуть город по умолчанию
            result.street = Street(s);   //если не указано - вернуть ошибку
            result.house = House(s);     //если не указано - вернуть ошибку
            if ((result.street == "Не удалось распознать улицу") || (result.house == "Не удалось распознать дом"))
            {
                result.CorrectAddress = false; //в случае отсутствия улицы/дома считаем адрес некорректным
                                               //return CompileAddress(result);
            }
            else

            {
                result.flat = Flat(s);   //не вернёт значение по умолчанию, но адрес может существовать и без квартиры
                mas[6] = result.flat;
            }
            //return CompileAddress(result);
            mas[0] = result.index;
            mas[1] = result.region;
            mas[2] = result.area;
            mas[3] = result.city;
            mas[4] = result.street;
            mas[5] = result.house;
            return mas;
        } //парсер адреса из строки, получает объект, преобразует в строку с помощью CompileAdress

        private string CompileAddress(AddressStructure adress)
        {
            string result = "";

            if (adress.index != "Не удалось распознать индекс")
            {
                result = result + adress.index + ", ";  //индекс не назначается по умолчанию, опустим его
            }

            result = result + adress.region + ", "; //регион назначается по умолчанию, можно прибавлять и ставить запятую

            if (adress.area != "") //данная проверка необходима, чтобы добавлять запятую только в необходимых случаях
            {                       //значение по умолчанию для района не ставится, нужно иметь городские и сельские адреса
                result = result + adress.area + ", ";
            }
            result = result + "г. " + adress.city + ", "; //город назначается по умолчанию, добавляем без раздумий
            if (adress.CorrectAddress == true) //если адрес корректный - то улица и дом распознаны, прибавим их
            {
                result = result + "ул. " + adress.street + ", ";
                result = result + "д. " + adress.house;
                if ((adress.flat != "Не удалось распознать квартиру") && (adress.flat != ""))
                {
                    result = result + ", кв. " + adress.flat;
                }
            }
            else { result = "Некорректный адрес"; }; //иначе вернём ошибку распознавания

            return result;
        }

        //private string Index(string s)
        //{
        //    string index = "";                              //ищем участок следующего вида: 
        //    Regex regex1 = new Regex(@",(\s*\d+\s*),+.*");  //запятая, любое количество пробелов, минимум одна цифра, любое количетсво пробелов, запятая, последующий текст
        //    index = MatchWithOneRegex(regex1, s);           //применяем регулярное выражение к строке
        //    if (index == "") index = "Не удалось распознать индекс";
        //    return index.Trim();                            //вернём значение, лишённое пробелов с левой и правой стороны
        //}

        private string Index(string s)
        {
            return addr.Index(s);
        }

        //private string Region(string s)
        //{
        //    string region = "";                                                             //разделено на 2 части:
        //    Regex regex1 = new Regex(@",(\s*(?:[А-я]|-|\s)*\s+(?:кр|о|Кр)+[А-я]*\s*),");       //запятая, пробелы, название, признак кр/о, дополненный до конца, пробелы, запятая, окончание
        //    Regex regex2 = new Regex(@",(\s*(?:кр|о|Кр)+[А-я]*\s+(?:[А-я]|-|\s)*)+\s*,");      //запятая, признак кр/о, дополенный до конца, пробел, название, пробелы, запятая, окончание
        //    Regex regex3 = new Regex(@",(\s*(?:кр|о|Кр)(?:[а-я]|-)*\s*[А-Я](?:[А-я]|-|\s)+)\s*,"); //запятая, признак кр/о, дополенный до конца из маленьких букв, Большая буква, окончание названия, запятая
        //    region = MatchWithThreeRegex(regex1, regex2, regex3, s);                        //применяем 3 регулярных выражения
        //    if (region == "") region = "Пермский край";                                     //если распознать регион не удалось- назначим регион по умолчанию
        //    return region.Trim();                                                           //вернём значение, лишённое пробелов с левой и правой стороны
        //}

        private string Region(string s)
        {
            return addr.Region(s);
        }

        //private string Area(string s) //код аналогичен распознаванию региона, признак кр/о (край/область) заменён на "р" от "район"
        //{
        //    string area = "";
        //    Regex regex1 = new Regex(@",(\s*(?:[А-я]|-|\s)*\s+р[А-я]*\s*),");
        //    Regex regex2 = new Regex(@",(\s*район[А-я]*\s+(?:[А-я]|-|\s)*)+\s*,");
        //    Regex regex3 = new Regex(@",(\s*р(?:[а-я]|-)*\s*[А-Я](?:[А-я]|-|\s)+)\s*,");
        //    area = MatchWithThreeRegex(regex1, regex2, regex3, s);
        //    return area.Trim();
        //}


        private string Area(string s) //код аналогичен распознаванию региона, признак кр/о (край/область) заменён на "р" от "район"
        {
            return addr.Area(s);
        }

        //private string City(string s)
        //{
        //    string city = "";
        //    Regex regex1 = new Regex(@",\s*(?:г|Г|дер|Дер|п)(?:[а-я]|-)*(?:\.|\s)+((?:[А-я]|-|\s|\d|-)+)\s*,"); //запятая, пробелы, город/деревня/посёлок, разделитель, название, пробелы, запятая
        //    Regex regex2 = new Regex(@",\s*(?:г|дер|п)(?:[а-я]|-)*\s*([А-Я](?:[А-я]|-|\s|\d|-)+)\s*,"); //тип населённого пункта, маленькими буквами, с большой буквы название 
        //    city = MatchWithTwoRegex(regex1, regex2, s); //если ошибка- вернётся пустая строка
        //    if (city == "") city = "Пермь"; //если распознавание ничего не дало- вернём значение по умолчанию для города
        //    return city.Trim();
        //}

        private string City(string s)
        {
            return addr.City(s);
        }

        //private string Street(string s)
        //{
        //    string street = "";
        //    Regex regex1 = new Regex(@",\s*(?:у|У)(?:[а-я]|-)*(?:\.|\s)+((?:[0-9]|[А-я]|-|\s)+)\s*,"); //запятая, признак улицы, разделитель, название может состоять из цифр, букв, пробелов, пробелы, запятая
        //    Regex regex2 = new Regex(@",\s*(?:у|У)(?:[а-я]|-)*\s*([А-Я](?:[А-я]|-|\s)+)\s*,"); //запятая, признак улицы маленькими буквами, может быть разделитель, большие буквы, пробелы, тире в названии
        //    street = MatchWithTwoRegex(regex1, regex2, s);
        //    if (street == "") street = "Не удалось распознать улицу"; //возвращаем значение по умолчанию, ошибк
        //    return street.Trim();
        //}

        private string Street(string s)
        {
            return addr.Street(s);
        }

        //private string House(string s)
        //{
        //    string house;
        //    Regex regex1 = new Regex(@",\s*(?:д|Д)(?:[А-я]|-)*(?:\.|\s)*(\d+(?:/|\s|[А-я])*\d*)\s*,"); //запятая, признак дома, разделитель, цифры, '/', буквы, снова цифры, пробелы, окончание адреса
        //    MatchCollection matches = regex1.Matches(s);
        //    house = MatchWithOneRegex(regex1, s);
        //    if (house == "") house = "Не удалось распознать дом";
        //    return house.Trim();
        //}


        private string House(string s)
        {
            return addr.House(s);
        }

        //private string Flat(string s)
        //{
        //    string flat = "";
        //    Regex regex1 = new Regex(@",\s*(?:к|К)(?:[А-я]|-)*(?:\.|\s)*(\d+)\s*,"); //запятая, признак квартиры, продолжение, разделитель, номер, пробелы, запятая, окончание адреса
        //    flat = MatchWithOneRegex(regex1, s);
        //    if (flat == "") flat = "Не удалось распознать квартиру";
        //    return flat.Trim();
        //}

        private string Flat(string s)
        {
            return addr.Flat(s);
        }


        //private string MatchWithOneRegex(Regex regex1, string s) //вынесенный метод применения одного регулярного выражения
        //{
        //    string result = "";
        //    MatchCollection matches = regex1.Matches(s);


        //    if (matches.Count > 0)  //если найден результат применения первого регулярного выражения - запомним его
        //    {
        //        foreach (Match match in matches)
        //        {
        //            result = match.Groups[1].Value;
        //        }
        //    }
        //    return result;
        //}


        private string MatchWithOneRegex(Regex regex1, string s) //вынесенный метод применения одного регулярного выражения
        {
            return addr.MatchWithOneRegex(regex1, s);
        }

        //private string MatchWithTwoRegex(Regex regex1, Regex regex2, string s) //вынесенный метод применения двух регулярных выражений
        //{
        //    string result = "";
        //    result = MatchWithOneRegex(regex1, s);
        //    if (result == "")
        //        result = MatchWithOneRegex(regex2, s);
        //    return result;
        //}

        private string MatchWithTwoRegex(Regex regex1, Regex regex2, string s) //вынесенный метод применения двух регулярных выражений
        {
            return addr.MatchWithTwoRegex(regex1,regex2, s);
        }

        //private string MatchWithThreeRegex(Regex regex1, Regex regex2, Regex regex3, string s) //вынесенный метод применения двух регулярных выражений
        //{
        //    string result = "";
        //    result = MatchWithOneRegex(regex1, s);
        //    if (result == "")
        //        result = MatchWithOneRegex(regex2, s);
        //    if (result == "")
        //        result = MatchWithOneRegex(regex3, s);
        //    return result;
        //}

        private string MatchWithThreeRegex(Regex regex1, Regex regex2, Regex regex3, string s) //вынесенный метод применения двух регулярных выражений
        {
            return addr.MatchWithThreeRegex(regex1, regex2,regex3, s);
        }


    }
}
