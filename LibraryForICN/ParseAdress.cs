using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using LibraryForICN;

/*
 Для запуска программы следует сначала собрать проект LibraryForICN, затем собрать оставшиеся два проекта: LibraryForICNTests, WCFServiceAdress
Далее следует запустить WCFServiceAdress на отладку/выполнение,подключиться с помощью программы SoapUI к странице проекта на IIS. 
Затем открыть с помощью SoapUI файл "TestForLibrary-soapui-project.xml" и провести необходимые тесты.
Модульные тесты библиотеки следует выполнять с помощью файла UnitTest1.cs в проекте LibraryForICNTest.
 */



namespace LibraryForICN
{
    /* Найденные ошибки и планы:
     * 
     * решить проблему с подключением библиотеки, возможно использовать относительный адрес и динамическое подключение библиотек?
     * 
     * посёлок распознаётся корректно, но в результате указывается как "город" - нет признака, посёлок это, деревня или город
     * 
     * фраза "город Край" трактуется и как название края, и как название города
     * 
     * "городской район", "гомельский район" и т.д. распознаётся как город "район" из-за признака "г" в регулярных выражениях.
     * исчезает при установке сначала города, а потом района. Возможное решение для часто встречающихся случаев- смотреть справа налево
     * Также для решения можно смотреть район, затем исключать эту строку из дальнейшего рассмотрения регулярными выражениями
     * 
     * Для каждого действия возвращать числовой (например?) код операции, а объект для изменения передавать по ссылке
     * 
     * Сделать предкомпилированные регулярные выражения для повышения скорости работы
     * 
     * Сделать логирование ошибок сервиса или действий пользователя вообще
     */

    public class AddressStructure
    { 
        public AddressStructure(string s)
        {
            try
            {
                this.CorrectAddress = true; //изначально считаем адрес корректным
                s = "," + s + ",";           //"окаймим" строку запятыми для отделения участков по запятым с двух сторон
                this.Index = ParseIndex(s);     //может вернуть индекс по умолчанию
                this.Region = ParseRegion(s);   //может вернуть регион по умолчанию
                this.Area = ParseArea(s);       //не вернёт район по умолчанию, иначе все адреса будут сельскими
                this.City = ParseCity(s);       //может вернуть город по умолчанию
                this.Street = ParseStreet(s);   //если не указано - вернуть ошибку
                this.House = ParseHouse(s);     //если не указано - вернуть ошибку
                if ((this.Street == "Не удалось распознать улицу") || (this.House == "Не удалось распознать дом"))
                {
                    this.CorrectAddress = false; //в случае отсутствия улицы/дома считаем адрес некорректным
                }
                else
                    this.Flat = ParseFlat(s);   //не вернёт значение по умолчанию, но адрес может существовать и без квартиры
            }
            catch (Exception ex)
            {
                this.CorrectAddress = false; //считаем его ошибочным
                this.Region = "При разборке адреса возникло исключение " + Convert.ToString(ex);
            }
        }

        public AddressStructure() { }

        public string CompileAddress()
        {
            try
            {
                string result = "";
                if (this.CorrectAddress == true) //если адрес корректный - то улица и дом распознаны, прибавим их
                {

                    if ((this.Index != "Не удалось распознать индекс") || (this.Index != ""))
                    {
                        result = result + this.Index + ", ";  //индекс не назначается по умолчанию, опустим его
                    }

                    if (this.Region == "")
                    {
                        result = result + "Пермский край, ";
                    }
                    else
                    {
                        result = result + this.Region + ", "; //регион назначается по умолчанию, можно прибавлять и ставить запятую
                    }

                    if (this.Area != "") //данная проверка необходима, чтобы добавлять запятую только в необходимых случаях
                    {                       //значение по умолчанию для района не ставится, нужно иметь городские и сельские адреса
                        result = result + this.Area + ", ";
                    }

                    if (this.City == "")
                    {
                        result = result + "г. Пермь, "; //город назначается по умолчанию, добавляем
                    }
                    else
                    {
                        result = result + "г. " + this.City + ", "; //город назначается по умолчанию, добавляем
                    }


                    result = result + "ул. " + this.Street + ", ";
                    result = result + "д. " + this.House;
                    if ((this.Flat != "Не удалось распознать квартиру") && (this.Flat != ""))
                    {
                        result = result + ", кв. " + this.Flat;
                    }
                }
                else { result = "Некорректный адрес"; }; //иначе вернём ошибку распознавания

                return result;
            }
            catch (Exception ex)
            {
                return ("При сборке адреса возникло исключение " + Convert.ToString(ex));
            }
        }


        public bool CorrectAddress
        {
            get
            {
                return correctAddress;
            }
            set
            {
                correctAddress = value;
            }
        }

        public string Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
            }
        }

        public string Region
        {
            get
            {
                return region;
            }
            set
            {
                region = value;
            }
        }

        public string Area
        {
            get
            {
                return area;
            }
            set
            {
                area = value;
            }
        }

        public string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
            }
        }

        public string Street
        {
            get
            {
                return street;
            }
            set
            {
                street = value;
            }
        }

        public string House
        {
            get
            {
                return house;
            }
            set
            {
                house = value;
            }
        }

        public string Flat
        {
            get
            {
                return flat;
            }
            set
            {
                flat = value;
            }
        }



        private bool correctAddress;
        private string index;
        private string region;
        private string area;
        private string city;
        private string street;
        private string house;
        private string flat;

        private string ParseIndex(string s)
        {
            try
            {

                string index = "";                              //ищем участок следующего вида: 
                Regex regex1 = new Regex(@",(\s*\d{6}\s*),+.*");  //запятая, любое количество пробелов, минимум одна цифра, любое количетсво пробелов, запятая, последующий текст
                index = MatchWithOneRegex(regex1, s);           //применяем регулярное выражение к строке
                if (index == "") index = "Не удалось распознать индекс";
                return index.Trim();                            //вернём значение, лишённое пробелов с левой и правой стороны
            }
            catch (Exception ex)
            {
                return ("При попытке разобрать индекс возникло исключение " + Convert.ToString(ex));
            }
        }

        private string ParseRegion(string s)
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

        private string ParseArea(string s) //код аналогичен распознаванию региона, признак кр/о (край/область) заменён на "р" от "район"
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

        private string ParseCity(string s)
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

        private string ParseStreet(string s)
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

        private string ParseHouse(string s)
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

        private string ParseFlat(string s)
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


        private string MatchWithOneRegex(Regex regex1, string s) //вынесенный метод применения одного регулярного выражения
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
                return ("Во время применения одного регулярного выражения возникло исключение " + Convert.ToString(ex));
            }
        }

        private string MatchWithTwoRegex(Regex regex1, Regex regex2, string s) //вынесенный метод применения двух регулярных выражений
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

        private string MatchWithThreeRegex(Regex regex1, Regex regex2, Regex regex3, string s) //вынесенный метод применения двух регулярных выражений
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
