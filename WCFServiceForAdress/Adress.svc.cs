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
        
        private string Index(string s)
        {
            return addr.Index(s);
        }
        
        private string Region(string s)
        {
            return addr.Region(s);
        }
        
        private string Area(string s) //код аналогичен распознаванию региона, признак кр/о (край/область) заменён на "р" от "район"
        {
            return addr.Area(s);
        }

        private string City(string s)
        {
            return addr.City(s);
        }

        private string Street(string s)
        {
            return addr.Street(s);
        }

        private string House(string s)
        {
            return addr.House(s);
        }
        
        private string Flat(string s)
        {
            return addr.Flat(s);
        }


        private string MatchWithOneRegex(Regex regex1, string s) //вынесенный метод применения одного регулярного выражения
        {
            return addr.MatchWithOneRegex(regex1, s);
        }

        private string MatchWithTwoRegex(Regex regex1, Regex regex2, string s) //вынесенный метод применения двух регулярных выражений
        {
            return addr.MatchWithTwoRegex(regex1,regex2, s);
        }

        private string MatchWithThreeRegex(Regex regex1, Regex regex2, Regex regex3, string s) //вынесенный метод применения двух регулярных выражений
        {
            return addr.MatchWithThreeRegex(regex1, regex2,regex3, s);
        }
    }
}
