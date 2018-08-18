using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFServiceForAdress
{
    [ServiceContract(Namespace = "AddressNS")]
    public interface IntAddress
    {

        [OperationContract]
        string CompileAddressFromSet(string index, string region, string area, string city, string street, string house, string flat);

        [OperationContract]
        string[] ParseAddress(string s);

    }

    [DataContract]
    public class AddressStructure
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

}
