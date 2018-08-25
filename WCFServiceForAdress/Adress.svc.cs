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

        public string CompileAddress(AddressTransfer address)
        {
            AddressStructure temp = address.ConvertToAddressStructure();
            return temp.CompileAddress();
        }

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
                ATresult.error = addr.Error;
            }
            else
                ATresult.CorrectAddress = false; //как это передаётся пользователю?
            return ATresult;
        }
        
    }
}
