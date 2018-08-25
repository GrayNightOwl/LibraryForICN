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
        
        [OperationContract]
        string CompileAddress(AddressTransfer address);

        [OperationContract]
        AddressTransfer ParseAddress(string s);


    }



    [DataContract]
    public class AddressTransfer
    {
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

        [DataMember]
        public string index; //сделать их свойствами?

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



        // This is not serialized because the DataMemberAttribute 
        // has not been applied.
        public bool CorrectAddress;

    }
}
