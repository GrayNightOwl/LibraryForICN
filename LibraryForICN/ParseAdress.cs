using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using LibraryForICN;

/// <summary>
/// Для запуска программы следует сначала собрать проект LibraryForICN, затем собрать оставшиеся два проекта: LibraryForICNTests, WCFServiceAdress
/// Далее следует запустить WCFServiceAdress на отладку/выполнение,подключиться с помощью программы SoapUI к странице проекта на IIS.
///Затем открыть с помощью SoapUI файл "TestUPD2-soapui-project.xml" и провести необходимые тесты.
///Модульные тесты библиотеки следует выполнять с помощью файла UnitTest1.cs в проекте LibraryForICNTest.
/// </summary>




namespace LibraryForICN
{
    /// <summary>
    /// Найденные ошибки и планы:
    /// 
    /// В свойствах класса AddressStructure необходимо добавить следующие проверки:
    /// Свойства, использующиеся для доступа к полям
    /// на содержание в индексе- только цифр
    /// на содержание в регионе, области, городе, улице- хоть каких-то букв
    /// на содержание в доме, квартире- хоть каких-то цифр
    /// 
    /// посёлок распознаётся корректно, но в результате указывается как "город" - нет признака, посёлок это, деревня или город
    /// фраза "город Край" трактуется и как название края, и как название города
    /// 
    /// "городской район", "гомельский район" и т.д. распознаётся как город "район" из-за признака "г" в регулярных выражениях.
    /// исчезает при установке сначала города, а потом района. Возможное решение для часто встречающихся случаев- смотреть справа налево
    /// Также для решения можно смотреть район, затем исключать эту строку из дальнейшего рассмотрения регулярными выражениями
    /// 
    /// </summary>

    public class AddressStructure
    {

        /// <summary>
        /// Конструктор, принимающий на вход строку с нечётким адресом и "разбирающий" её на части. 
        /// Части адреса сразу же записываются в соответствующие поля экземпляра класса
        /// Здесь же содержится обработчик исключений, который следует вынести в отдельную функцию
        /// </summary>
        /// <param name="s">Строка с нечётким адресом</param>
        public AddressStructure(string s)
        {
            try
            {
                this.CorrectAddress = true; //изначально считаем адрес корректным
                s = "," + s + ",";           //"окаймим" строку запятыми для отделения участков по запятым с двух сторон
                this.Index = ParseIndex(s);     //не может вернуть индекс по умолчанию
                this.Region = ParseRegion(s);   //может вернуть регион по умолчанию
                this.Area = ParseArea(s);       //не вернёт район по умолчанию, иначе все адреса будут сельскими
                this.City = ParseCity(s);       //может вернуть город по умолчанию
                this.Street = ParseStreet(s);   //если не указано - вернуть ошибку
                this.House = ParseHouse(s);     //если не указано - вернуть ошибку
                if ((this.Street == StreetError) || (this.House == HouseError))
                {
                    this.CorrectAddress = false; //в случае отсутствия улицы/дома считаем адрес некорректным
                }
                else
                    this.Flat = ParseFlat(s);   //не вернёт значение по умолчанию, но адрес может существовать и без квартиры
            }
            catch (Exception ex)
            {
                this.CorrectAddress = false; //считаем его ошибочным
                this.Error = "При разборке адреса возникло исключение " + Convert.ToString(ex);
            }
        }

        /// <summary>
        /// Метод, собирающий адрес из отдельных полей экземпляра класса AddressStructure. 
        /// Содержит обработчик исключений, который также следует вынести в отдельную функцию
        /// </summary>
        /// <returns>Собранный в строку адрес</returns>
        public string CompileAddress()
        {
            try
            {
                StringBuilder result = new StringBuilder("",150);
                if (this.CorrectAddress == true) //если адрес корректный - то улица и дом распознаны, прибавим их
                {

                    if ((this.Index != IndexError) && (this.Index != ""))
                    {
                        result.Append(this.Index + ", ");  //индекс не назначается по умолчанию, опустим его
                    }

                    if (this.Region == "")
                    {
                        result.Append("Пермский край, ");
                    }
                    else
                    {
                        result.Append( this.Region + ", "); //регион назначается по умолчанию, можно прибавлять и ставить запятую
                    }

                    if (this.Area != "") //данная проверка необходима, чтобы добавлять запятую только в необходимых случаях
                    {                       //значение по умолчанию для района не ставится, нужно иметь городские и сельские адреса
                        result.Append(this.Area + ", ");
                    }

                    if (this.City == "")
                    {
                        result.Append("г. Пермь, "); //город назначается по умолчанию, добавляем
                    }
                    else
                    {
                        result.Append("г. " + this.City + ", "); //город назначается по умолчанию, добавляем
                    }


                    result.Append("ул. " + this.Street + ", ");
                    result.Append("д. " + this.House);
                    if ((this.Flat != FlatError) && (this.Flat != ""))
                    {
                        result.Append(", кв. " + this.Flat);
                    }
                }
                else { result.Append("Некорректный адрес"); }; //иначе вернём ошибку распознавания

                return result.ToString();
            }
            catch (Exception ex)
            {
                return ("При сборке адреса возникло исключение " + Convert.ToString(ex));
            }
        }




        public AddressStructure() { }

        /// <remarks>
        /// Скрытые поля класса
        /// </remarks>
        private bool correctAddress;
        private string index;
        private string region;
        private string area;
        private string city;
        private string street;
        private string house;
        private string flat;
        private string error;

        /// <remarks>
        /// Свойства, использующиеся для доступа к полям
        /// На данный момент не успеваю это сделать, но в описании свойства необходимо добавить проверку на содержание в индексе- только цифр
        /// на содержание в регионе, области, городе, улице- хоть каких-то букв
        /// на содержание в доме, квартире- хоть каких-то цифр
        /// </remarks>
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
        public string Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;
            }
        }

        /// <remarks>
        /// Далее следуют строковые константы, отвечающие за отображение ошибок и исключений при распознавании частей адреса
        /// </remarks>
        const string IndexError = "Не удалось распознать индекс";
        const string StreetError = "Не удалось распознать улицу";
        const string HouseError = "Не удалось распознать дом";
        const string FlatError = "Не удалось распознать квартиру";
        
        const string IndexException = "При попытке разобрать индекс возникло исключение ";
        const string RegionException = "При попытке разобрать регион возникло исключение ";
        const string AreaException = "При попытке разобрать район возникло исключение ";
        const string CityException = "При попытке разобрать населённый пункт возникло исключение ";
        const string StreetException = "При попытке разобрать улицу возникло исключение ";
        const string HouseException = "При попытке разобрать дом возникло исключение ";
        const string FlatException = "При попытке разобрать квартиру возникло исключение ";

        /// <summary>
        /// Метод, используемый для нахождения индекса в строке.
        /// </summary>
        /// <param name="s">Строка с нечётким адресом</param>
        /// <returns>Распознанный индекс или пустая строка в случае, если индекс распознать не удалось</returns>
        private string ParseIndex(string s)
        {
            try
            {
                string index = "";                              //ищем участок следующего вида: 
                Regex regex1 = new Regex(@",(\s*\d{6}\s*),+.*");  //запятая, любое количество пробелов, минимум одна цифра, любое количетсво пробелов, запятая, последующий текст
                index = MatchWithOneRegex(regex1, s);           //применяем регулярное выражение к строке
                if (index == "") index = IndexError;
                return index.Trim();                            //вернём значение, лишённое пробелов с левой и правой стороны
            }
            catch (Exception ex)
            {
                throw new Exception(IndexException + ex.InnerException);
            }
        }

        /// <summary>
        /// Метод, используемый для нахождения названия региона в строке
        /// </summary>
        /// <param name="s">Строка с нечётким адресом</param>
        /// <returns>Название региона или "Пермский край" в случае, если распознать регион не удалось</returns>
        private string ParseRegion(string s)
        {
            try
            {
                string region = "";                                                             //разделено на 2 части:
                Regex regex1 = new Regex(@",(\s*(?:[А-я]|-|\s)*\s+(?:кр|о|Кр)+[А-я]*\s*),");       //запятая, пробелы, название, признак кр/о, дополненный до конца, пробелы, запятая, окончание
                Regex regex2 = new Regex(@",(\s*(?:кр|о|Кр)+[А-я]*\s+(?:[А-я]|-|\s)*)+\s*,");      //запятая, признак кр/о, дополенный до конца, пробел, название, пробелы, запятая, окончание
                Regex regex3 = new Regex(@",(\s*(?:кр|о|Кр)(?:[а-я]|-)*\s*[А-Я](?:[А-я]|-|\s)+)\s*,"); //запятая, признак кр/о, дополенный до конца из маленьких букв, Большая буква, окончание названия, запятая
                region = MatchWithThreeRegex(regex1, regex2, regex3, s);                        //применяем 3 регулярных выражения
                if (region == "") region = "Пермский край";                                     
                return region.Trim();                                                           //вернём значение, лишённое пробелов с левой и правой стороны
            }
            catch (Exception ex)
            {
                throw new Exception(RegionException + Convert.ToString(ex));
            }
        }

        /// <summary>
        /// Метод, используемый для нахождения названия области в строке
        /// Код аналогичен распознаванию региона, признак кр/о (край/область) заменён на "р" от "район"
        /// </summary>
        /// <param name="s">Строка с нечётким адресом</param>
        /// <returns>Название района или пустая строка, если распознать район не удалось</returns>
        private string ParseArea(string s) 
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
                throw new Exception(AreaException + Convert.ToString(ex));
            }
        }

        /// <summary>
        /// Метод, используемый для нахождения названия города в строке
        /// Код аналогичен предыдущему методу с заменой признака поиска на "г" - город, "дер" - деревня, "п"- посёлок
        /// </summary>
        /// <param name="s">Строка с нечётким адресом</param>
        /// <returns>Название города или "Пермь" в случае, если распознать город не удалось</returns>
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
                throw new Exception(CityException + Convert.ToString(ex));
            }
        }

        /// <summary>
        /// Метод, используемый для нахождения названия улицы в строке
        /// Код аналогичен предыдущему с заменой признака поиска на "у" - улица
        /// </summary>
        /// <param name="s">Строка с нечётким адресом</param>
        /// <returns>Название улицы или строка с указанием о том, что улица не распознана</returns>
        private string ParseStreet(string s)
        {
            try
            {
                string street = "";
                Regex regex1 = new Regex(@",\s*(?:у|У)(?:[а-я]|-)*(?:\.|\s)+((?:[0-9]|[А-я]|-|\s)+)\s*,"); //запятая, признак улицы, разделитель, название может состоять из цифр, букв, пробелов, пробелы, запятая
                Regex regex2 = new Regex(@",\s*(?:у|У)(?:[а-я]|-)*\s*([А-Я](?:[А-я]|-|\s)+)\s*,"); //запятая, признак улицы маленькими буквами, может быть разделитель, большие буквы, пробелы, тире в названии
                street = MatchWithTwoRegex(regex1, regex2, s);
                if (street == "") street = StreetError; //возвращаем значение по умолчанию, ошибк
                return street.Trim();
            }
            catch (Exception ex)
            {
                throw new Exception(StreetException + Convert.ToString(ex));
            }
        }

        /// <summary>
        /// Метод, используемый для нахождения дома в строке
        /// Код аналоигчен предыдущему, с заменой признака поиска на "д"- дом
        /// </summary>
        /// <param name="s">Строка с нечётким адресом</param>
        /// <returns>Номер дома или строка с указанием о том, что номер дома не распознан</returns>
        private string ParseHouse(string s)
        {
            try
            {
                string house;
                Regex regex1 = new Regex(@",\s*(?:д|Д)(?:[А-я]|-)*(?:\.|\s)*(\d+(?:/|\s|[А-я])*\d*)\s*,"); //запятая, признак дома, разделитель, цифры, '/', буквы, снова цифры, пробелы, окончание адреса
                MatchCollection matches = regex1.Matches(s);
                house = MatchWithOneRegex(regex1, s);
                if (house == "") house = HouseError;
                return house.Trim();
            }
            catch (Exception ex)
            {
                throw new Exception(HouseException + Convert.ToString(ex));
            }
        }

        /// <summary>
        /// Метод, используемый для нахождения номера квартиры в строке
        /// Код аналогичен предыдущему с заменой признака поиска на "к" - квартира
        /// </summary>
        /// <param name="s">Строка с нечётким адресом</param>
        /// <returns>Номер квартиры или строка с указанием о том, что номер квартиры не распознан</returns>
        private string ParseFlat(string s)
        {
            try
            {
                string flat = "";
                Regex regex1 = new Regex(@",\s*(?:к|К)(?:[А-я]|-)*(?:\.|\s)*(\d+)\s*,"); //запятая, признак квартиры, продолжение, разделитель, номер, пробелы, запятая, окончание адреса
                flat = MatchWithOneRegex(regex1, s);
                if (flat == "") flat = FlatError;
                return flat.Trim();
            }
            catch (Exception ex)
            {
                throw new Exception(FlatException + Convert.ToString(ex));
            }
        }

        /// <summary>
        /// Вынесенный метод для применения одного регулярного выражения
        /// </summary>
        /// <param name="regex1">Регулярное выражение на входе</param>
        /// <param name="s">Строка, к которой следует применить регулярное выражение</param>
        /// <returns>Результат поиска</returns>
        private string MatchWithOneRegex(Regex regex1, string s) 
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

        /// <summary>
        /// Вынесенный метод применения двух регулярных выражений
        /// </summary>
        /// <param name="regex1">Регулярное выражение 1 на входе</param>
        /// <param name="regex2">Регулярное выражение 2 на входе</param>
        /// <param name="s">Строка, к которой следует применить 2 регулярных выражения</param>
        /// <returns>Результат поиска</returns>
        private string MatchWithTwoRegex(Regex regex1, Regex regex2, string s) 
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

        /// <summary>
        /// Вынесенный метод применения двух регулярных выражений
        /// </summary>
        /// <param name="regex1">Регулярное выражение 1 на входе</param>
        /// <param name="regex2">Регулярное выражение 2 на входе</param>
        /// <param name="regex3">Регулярное выражение 3 на входе</param>
        /// <param name="s">Строка, к которой следует применить 3 регулярных выражения</param>
        /// <returns>Результат поиска</returns>
        private string MatchWithThreeRegex(Regex regex1, Regex regex2, Regex regex3, string s) 
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
