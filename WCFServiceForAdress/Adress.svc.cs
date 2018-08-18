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

        //public string CompileAddressFromSet(string index, string region, string area, string city, string street, string house, string flat)
        //{   //создаёт объект типа Адрес, передаёт в функцию сборки из объекта 

        //    AddressStructure addressStructure = new AddressStructure();
        //    addressStructure.CorrectAddress = true;
        //    addressStructure.index = index;
        //    addressStructure.region = region;
        //    addressStructure.area = area;
        //    addressStructure.city = city;
        //    addressStructure.street = street;
        //    addressStructure.house = house;
        //    addressStructure.flat = flat;

        //    return addr.CompileAddress(addressStructure);

        //}

        public string CompileAddressFromSet(string index, string region, string area, string city, string street, string house, string flat)
        {   //создаёт объект типа Адрес, передаёт в функцию сборки из объекта 
            return addr.CompileAddressFromSet(index, region, area, city, street, house, flat);

        }

        public string[] ParseAddress(string s)
        {
            return addr.ParseAddress(s);
        } //парсер адреса из строки, получает объект, преобразует в строку с помощью CompileAdress

        private string CompileAddress(AddressStructure adress)
        {
            return addr.CompileAddress(adress);
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
