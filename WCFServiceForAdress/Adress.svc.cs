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
        /// <summary>
        /// Метод, использующийся для сборки адреса из отдельных частей
        /// Использует метод преобразования к библиотечному классу, затем использует библиотечный метод.
        /// </summary>
        /// <param name="address">Адресс, принятый с помощью WCF, объект класса AddressTransfer</param>
        /// <returns>Строка с собранным адресом</returns>
        public string CompileAddress(AddressTransfer address)
        {
            AddressStructure temp = address.ConvertToAddressStructure();
            return temp.CompileAddress();
        }

        /// <summary>
        /// Метод, использующийся для разборки адреса
        /// Создаётся экземпляр библиотечного класса, с помощью конструктора разбирается адрес
        /// Создаётся экземпляр класса AddressTranfser, пригодного для передачи с помощью WCF
        /// Если адрес корректный- распознанные значения полей библиотечного класса присваиваются полям класса для передачи
        /// 
        /// В будущем необходимо переписать с использованием конструтора
        /// </summary>
        /// <param name="s">Строка с нечётким адресом</param>
        /// <returns>Объект, пригодный для передачи с помощью WCF с распознанным адресом или флагом и сообщением об ошибки</returns>
        public AddressTransfer ParseAddress(string s)
        {
            AddressStructure addr = new AddressStructure(s);
            AddressTransfer ATresult = new AddressTransfer();
            if (addr.CorrectAddress == true)
            {
                ATresult.region = addr.Region;
                ATresult.index = addr.Index;
                ATresult.area = addr.Area;
                ATresult.city = addr.City;
                ATresult.street = addr.Street;
                ATresult.house = addr.House;
                ATresult.flat = addr.Flat;
                ATresult.error = addr.Error; //на случай, если флаг корректности установлен, но есть ошибка
            }
            else
            {
                ATresult.CorrectAddress = false;
                ATresult.error = addr.Error;  //передача ошибки пользователю
            }
            return ATresult;
        }
        
    }
}
