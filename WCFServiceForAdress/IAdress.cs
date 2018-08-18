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
}
