using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using LibraryForICN;

namespace WCFServiceForAdress
{
    [ServiceContract(Namespace = "AdressNamespace")]
    public interface IntAdress
    {
        [OperationContract]
        string CompileAdress(AdressStructure adress);
        [OperationContract]
        AdressStructure ParseAdress(string s);
    }

    // Используйте контракт данных, как показано в примере ниже, чтобы добавить составные типы к операциям служб.
    [DataContract]
    public class AdressStructure
    {
        public bool CorrectAdress; //признаки корректности адреса, устанавливается в "false" в случае отстутствия названия улицы или номера дома
        public string index;
        public string region;
        public string area;
        public string city;
        public string street;
        public string house;
        public string flat;
    }

}
