using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LibraryForICN
{
    /* Найденные ошибки:
     * посёлок распознаётся корректно, но в результате указывается как "город" - нет признака, посёлок это, деревня или город
     * 
     * фраза "город Край" трактуется и как название края, и как название города
     * 
     * "городской район", "гомельский район" и т.д. распознаётся как город "район" из-за признака "г" в регулярных выражениях.
     * исчезает при установке сначала города, а потом района. Возможное решение для часто встречающихся случаев- смотреть справа налево
     * 
     * Для каждого действия возвращать числовой (например?) код операции, а объект для изменения передавать по ссылке
     * Сделать предкомпилированные регулярные выражения для повышения скорости работы
     */

    public class AddressStructure //Данный класс существует в качестве структуры данных, хранящей адрес, используется для сборки и разборки адреса
    {
        public bool CorrectAddress; //признаки корректности адреса, устанавливается в "false" в случае отстутствия названия улицы или номера дома
        public string index;
        public string region;
        public string area;
        public string city;
        public string street;
        public string house;
        public string flat;
    }

    public class AdressFromLIB
    {
        public string CompileAddress(AddressStructure adress)
        {
            try
            {
                string result = "";
                if (adress.CorrectAddress == true) //если адрес корректный - то улица и дом распознаны, прибавим их
                {

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
            catch (Exception ex)
            {
                return ("При сборке адреса возникло исключение "+ Convert.ToString(ex));
            }
        }

        public AddressStructure ParseAdress(string s) 
        {
            try
            {
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
                    return result;
                }
                else
                    result.flat = Flat(s);   //не вернёт значение по умолчанию, но адрес может существовать и без квартиры
                return result;
            }
            catch (Exception ex)
            {
                AddressStructure faultResult = new AddressStructure(); //создаём новый объект
                faultResult.CorrectAddress = false; //считаем его ошибочным
                faultResult.region = "При разборке адреса возникло исключение "+ Convert.ToString(ex);
                return faultResult;
            }
        } //парсер адреса из строки, получает объект, преобразует в AddressStructure

        public string[] ParseAddress(string s)
        {
            try
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
            }
            catch (Exception ex)
            {
                string[] faultMas = new string[7];
                faultMas[0] = "При попытке разборке адреса в набор элементов возникло исключение " + Convert.ToString(ex);
                return faultMas;
            }
        } //парсер адреса из строки, получает объект, преобразует в массив строк

        public string CompileAddressFromSet(string index, string region, string area, string city, string street, string house, string flat)  //создаёт объект типа Адрес, передаёт в функцию сборки из объекта 
        {
            try
            {
                AddressStructure addressStructure = new AddressStructure();
                Regex regex1 = new Regex(@"([А-я])");
                if ((region == "") || (city == "") || (street == "") || (house == "")|| (MatchWithOneRegex(regex1, index) != "")||(MatchWithOneRegex(regex1, flat) != ""))
                {

                    addressStructure.CorrectAddress = false;
                }
                else
                {
                    addressStructure.CorrectAddress = true;
                    addressStructure.index = (MatchWithOneRegex(regex1, index));
                    addressStructure.region = region;
                    addressStructure.area = area;
                    addressStructure.city = city;
                    addressStructure.street = street;
                    addressStructure.house = house;
                    addressStructure.flat = flat;
                }
                return CompileAddress(addressStructure);
            }
            catch (Exception ex)
            {
                return ("При попытке собрать адрес из отдельных компонент возникло исключение " + Convert.ToString(ex));
            }

        }




        public string Index(string s)
        {
            try
            {

                string index = "";                              //ищем участок следующего вида: 
                Regex regex1 = new Regex(@",(\s*\d+\s*),+.*");  //запятая, любое количество пробелов, минимум одна цифра, любое количетсво пробелов, запятая, последующий текст
                index = MatchWithOneRegex(regex1, s);           //применяем регулярное выражение к строке
                if (index == "") index = "Не удалось распознать индекс";
                return index.Trim();                            //вернём значение, лишённое пробелов с левой и правой стороны
            }
            catch (Exception ex)
            {
                return ("При попытке разобрать индекс возникло исключение " + Convert.ToString(ex));
            }
        }

        public string Region(string s)
        {
            try
            {
                string region = "";                                                             //разделено на 2 части:
                Regex regex1 = new Regex(@",(\s*(?:[А-я]|-|\s)*\s+(?:кр|о|Кр)+[А-я]*\s*),");       //запятая, пробелы, название, признак кр/о, дополненный до конца, пробелы, запятая, окончание
                Regex regex2 = new Regex(@",(\s*(?:кр|о|Кр)+[А-я]*\s+(?:[А-я]|-|\s)*)+\s*,");      //запятая, признак кр/о, дополенный до конца, пробел, название, пробелы, запятая, окончание
                Regex regex3 = new Regex(@",(\s*(?:кр|о|Кр)(?:[а-я]|-)*\s*[А-Я](?:[А-я]|-|\s)+)\s*,"); //запятая, признак кр/о, дополенный до конца из маленьких букв, Большая буква, окончание названия, запятая
                region = MatchWithThreeRegex(regex1, regex2, regex3, s);                        //применяем 3 регулярных выражения
                if (region == "") region = "Пермский край";                                     //если распознать регион не удалось- назначим регион по умолчанию
                return region.Trim();                                                           //вернём значение, лишённое пробелов с левой и правой стороны
            }
            catch (Exception ex)
            {
                return ("При попытке разобрать регион возникло исключение " + Convert.ToString(ex));
            }
        }

        public string Area(string s) //код аналогичен распознаванию региона, признак кр/о (край/область) заменён на "р" от "район"
        {
            try
            {
                string area = "";
                Regex regex1 = new Regex(@",(\s*(?:[А-я]|-|\s)*\s+р[А-я]*\s*),");
                Regex regex2 = new Regex(@",(\s*район[А-я]*\s+(?:[А-я]|-|\s)*)+\s*,");
                Regex regex3 = new Regex(@",(\s*р(?:[а-я]|-)*\s*[А-Я](?:[А-я]|-|\s)+)\s*,");
                area = MatchWithThreeRegex(regex1, regex2, regex3, s);
                return area.Trim();
            }
            catch (Exception ex)
            {
                return ("При попытке разобрать район возникло исключение " + Convert.ToString(ex));
            }
        }

        public string City(string s)
        {
            try
            {
                string city = "";
                Regex regex1 = new Regex(@",\s*(?:г|Г|дер|Дер|п)(?:[а-я]|-)*(?:\.|\s)+((?:[А-я]|-|\s|\d|-)+)\s*,"); //запятая, пробелы, город/деревня/посёлок, разделитель, название, пробелы, запятая
                Regex regex2 = new Regex(@",\s*(?:г|дер|п)(?:[а-я]|-)*\s*([А-Я](?:[А-я]|-|\s|\d|-)+)\s*,"); //тип населённого пункта, маленькими буквами, с большой буквы название 
                city = MatchWithTwoRegex(regex1, regex2, s); //если ошибка- вернётся пустая строка
                if (city == "") city = "Пермь"; //если распознавание ничего не дало- вернём значение по умолчанию для города
                return city.Trim();
            }
            catch (Exception ex)
            {
                return ("При попытке разобрать населённый пункт возникло исключение " + Convert.ToString(ex));
            }
        }

        public string Street(string s)
        {
            try
            {
                string street = "";
                Regex regex1 = new Regex(@",\s*(?:у|У)(?:[а-я]|-)*(?:\.|\s)+((?:[0-9]|[А-я]|-|\s)+)\s*,"); //запятая, признак улицы, разделитель, название может состоять из цифр, букв, пробелов, пробелы, запятая
                Regex regex2 = new Regex(@",\s*(?:у|У)(?:[а-я]|-)*\s*([А-Я](?:[А-я]|-|\s)+)\s*,"); //запятая, признак улицы маленькими буквами, может быть разделитель, большие буквы, пробелы, тире в названии
                street = MatchWithTwoRegex(regex1, regex2, s);
                if (street == "") street = "Не удалось распознать улицу"; //возвращаем значение по умолчанию, ошибк
                return street.Trim();
            }
            catch (Exception ex)
            {
                return ("При попытке разобрать улицу возникло исключение " + Convert.ToString(ex));
            }
        }

        public string House(string s)
        {
            try
            {
                string house;
                Regex regex1 = new Regex(@",\s*(?:д|Д)(?:[А-я]|-)*(?:\.|\s)*(\d+(?:/|\s|[А-я])*\d*)\s*,"); //запятая, признак дома, разделитель, цифры, '/', буквы, снова цифры, пробелы, окончание адреса
                MatchCollection matches = regex1.Matches(s);
                house = MatchWithOneRegex(regex1, s);
                if (house == "") house = "Не удалось распознать дом";
                return house.Trim();
            }
            catch (Exception ex)
            {
                return ("При попытке разобрать дом возникло исключение " + Convert.ToString(ex));
            }
        }

        public string Flat(string s)
        {
            try
            {
                string flat = "";
                Regex regex1 = new Regex(@",\s*(?:к|К)(?:[А-я]|-)*(?:\.|\s)*(\d+)\s*,"); //запятая, признак квартиры, продолжение, разделитель, номер, пробелы, запятая, окончание адреса
                flat = MatchWithOneRegex(regex1, s);
                if (flat == "") flat = "Не удалось распознать квартиру";
                return flat.Trim();
            }
            catch (Exception ex)
            {
                return ("При попытке разобрать квартиру возникло исключение " + Convert.ToString(ex));
            }
        }
        

        public string MatchWithOneRegex(Regex regex1, string s) //вынесенный метод применения одного регулярного выражения
        {
            try
            {
                string result = "";
                MatchCollection matches = regex1.Matches(s);


                if (matches.Count > 0)  //если найден результат применения первого регулярного выражения - запомним его
                {
                    foreach (Match match in matches)
                    {
                        result = match.Groups[1].Value;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return ("Во время применения одного регулярного выражения возникло исключение "+Convert.ToString(ex));
            }
        }

        public string MatchWithTwoRegex(Regex regex1, Regex regex2, string s) //вынесенный метод применения двух регулярных выражений
        {
            try
            {
                string result = "";
                result = MatchWithOneRegex(regex1, s);
                if (result == "")
                    result = MatchWithOneRegex(regex2, s);
                return result;
            }
            catch (Exception ex)
            {
                return ("Во время применения двух регулярных выражений возникло исключение " + Convert.ToString(ex));
            }
        }

        public string MatchWithThreeRegex(Regex regex1, Regex regex2, Regex regex3, string s) //вынесенный метод применения двух регулярных выражений
        {
            try
            {
                string result = "";
                result = MatchWithOneRegex(regex1, s);
                if (result == "")
                    result = MatchWithOneRegex(regex2, s);
                if (result == "")
                    result = MatchWithOneRegex(regex3, s);
                return result;
            }
            catch (Exception ex)
            {
                return ("Во время применения трёх регулярных выражений возникло исключение " + Convert.ToString(ex));
            }
        }

    }
}
