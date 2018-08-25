using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using LibraryForICN;
using System.ServiceModel.Web;
using System.Text.RegularExpressions;
using System.Text;

namespace WCFServiceForAdress
{
    [ServiceContract(Namespace = "AddressNS")]
    public interface IntAddress
    {
        /// <summary>
        /// Описание метода, используемого для сборки адреса из объекта, полученного с помощью WCF
        /// </summary>
        /// <param name="address">Объект типа AddressTransfer, полученный с помощью WCF</param>
        /// <returns>Собранная строка</returns>
        [OperationContract]
        string CompileAddress(AddressTransfer address);

        /// <summary>
        /// Описание метода, используемого для разборки нечётко заданного адреса
        /// </summary>
        /// <param name="s">Строка с нечётко заданным адресом</param>
        /// <returns>Объект типа AddressTransfer, пригодный для передачи с помощью WCF</returns>
        [OperationContract]
        AddressTransfer ParseAddress(string s);


    }


    /// <summary>
    /// Соглашение о классе AddressTranfser
    /// </summary>
    [DataContract]
    public class AddressTransfer
    {
        /// <summary>
        /// Метод, преобразующий класс AddressTransfer (передача WCF) в класс AdressStructure, работа с которым идёт в библиотеке
        /// Содержит проверку на пустоту квартиры и улицы, так как именно этот набор является минимально допустимым
        /// </summary>
        /// <returns>объект типа AddressStructure, работа с которым потом пойдёт методами из библиотеки</returns>
        public AddressStructure ConvertToAddressStructure()
        {
            AddressStructure temp = new AddressStructure();
            if ((street == "") || (house == ""))
            {
                temp.CorrectAddress = false;
            }
            else
            {
                temp.CorrectAddress = true; //искуственно выставляем флаг корректности, так как необходимо 
                temp.Index = index;
                temp.Region = region;
                temp.Area = area;
                temp.City = city;
                temp.Street = street;
                temp.House = house;
                temp.Flat = flat;
            }
            return temp;
        }


        /// <summary>
        /// Далее следуют соглашения о полях, передаваемых с помощью WCF
        /// </summary>
        [DataMember]
        public string index;

        [DataMember]
        public string region;

        [DataMember]
        public string area;

        [DataMember]
        public string city;

        [DataMember]
        public string street;

        [DataMember]
        public string house;

        [DataMember]
        public string flat;

        [DataMember]
        public string error;

        /// <summary>
        /// Метод, который не передаётся при взаимодействии с помощью WCF
        /// </summary>
        public bool CorrectAddress;

    }
}
