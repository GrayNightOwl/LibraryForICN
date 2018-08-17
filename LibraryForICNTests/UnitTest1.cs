using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryForICN;

namespace LibraryForICNTests
{
    [TestClass]
    public class AdressTests
    {
        [TestMethod]
        public void Street_inputString1_resultString() //как это оформить на русском?
        {
            //вход
            string s = "  Приморский край ,  625000   , г.  Край ,  кв4 , дом 15,   улица 1-я Красноармейская   , ";
            string expected = "625000, Приморский край, г. Край, ул. 1-я Красноармейская, д. 15, кв. 4";
            //получаем значения
            Adress adress = new Adress();
            AdressForCompile res = adress.ParseAdress(s);
            string actual = adress.CompileAdress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Street_inputString2_resultString() //как это оформить на русском?
        {
            //вход
            string s = " д. 70Б,ул Гагарина бульвар, край Пермский ,  614000   , город  Пермь , ";
            string expected = "614000, край Пермский, г. Пермь, ул. Гагарина бульвар, д. 70Б";
            //получаем значения
            Adress adress = new Adress();
            AdressForCompile res = adress.ParseAdress(s);
            string actual = adress.CompileAdress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Street_inputString3_resultString() //как это оформить на русском?
        {
            //вход
            string s = " д. 70/2, улСоловьева, кв. 45, край Пермский ,  614285   , город  Пермь , ";
            string expected = "614285, край Пермский, г. Пермь, ул. Соловьева, д. 70/2, кв. 45";
            //получаем значения
            Adress adress = new Adress();
            AdressForCompile res = adress.ParseAdress(s);
            string actual = adress.CompileAdress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputString4_resultString() //как это оформить на русском?
        {
            //вход
            string s = " д. 70/2, улСоловьева, кв. 45, край Пермский ,  614285   , город  Край , ";
            string expected = "614285, край Пермский, г. Край, ул. Соловьева, д. 70/2, кв. 45";
            //получаем значения
            Adress adress = new Adress();
            AdressForCompile res = adress.ParseAdress(s);
            string actual = adress.CompileAdress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputString5_resultString() //как это оформить на русском?
        {
            //вход
            string s = " дом 116Б, улицаСоловьева, кв-ра45, область Магаданская ,  548788   , город  Магадан , ";
            string expected = "548788, область Магаданская, г. Магадан, ул. Соловьева, д. 116Б, кв. 45";
            //получаем значения
            Adress adress = new Adress();
            AdressForCompile res = adress.ParseAdress(s);
            string actual = adress.CompileAdress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);

        }
    }
}
