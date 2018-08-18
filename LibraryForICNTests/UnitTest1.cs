using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryForICN;

namespace LibraryForICNTests
{
    [TestClass]
    public class AdressFromLIBTests
    {


        [TestMethod]
        public void Street_inputString1_resultString() //как это оформить на русском?
        {
            //вход
            string s = "  Приморский край ,  625000   , г.  Край ,  кв4 , дом 15,   улица 1-я Красноармейская   , ";
            string expected = "625000, Приморский край, г. Край, ул. 1-я Красноармейская, д. 15, кв. 4";


            //получаем значения
            AdressFromLIB adress = new AdressFromLIB();
            AddressStructure res = adress.ParseAdress(s);
            string actual = adress.CompileAddress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Street_inputString2_resultString() //как это оформить на русском?
        {
            //вход
            string s = " д. 70Б,ул Гагарина бульвар, край Пермский ,  614000   , гор  Пермь , ";
            string expected = "614000, край Пермский, г. Пермь, ул. Гагарина бульвар, д. 70Б";
            //получаем значения
            AdressFromLIB adress = new AdressFromLIB();
            AddressStructure res = adress.ParseAdress(s);
            string actual = adress.CompileAddress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Street_inputString3_resultString() //как это оформить на русском?
        {
            //вход
            string s = " д. 70/2, улСоловьева, кв. 45, край Пермский ,  614285   , г  Пермь , ";
            string expected = "614285, край Пермский, г. Пермь, ул. Соловьева, д. 70/2, кв. 45";
            //получаем значения
            AdressFromLIB adress = new AdressFromLIB();
            AddressStructure res = adress.ParseAdress(s);
            string actual = adress.CompileAddress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputString4_resultString() //как это оформить на русском?
        {
            //вход
            string s = " д. 70/2, улСоловьева, кв. 45, край Пермский ,  614285   , г. Край , ";
            string expected = "614285, край Пермский, г. Край, ул. Соловьева, д. 70/2, кв. 45";
            //получаем значения
            AdressFromLIB adress = new AdressFromLIB();
            AddressStructure res = adress.ParseAdress(s);
            string actual = adress.CompileAddress(res);

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
            AdressFromLIB adress = new AdressFromLIB();
            AddressStructure res = adress.ParseAdress(s);
            string actual = adress.CompileAddress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);

        }


        [TestMethod]
        public void Street_inputString6_resultString() //как это оформить на русском?
        {
            //вход
            string s = " дом 116Б, ул. Растеряева, кв-ра45, область Ростовская ,  548788   , город  Распушил ,район Ивановский ,";
            string expected = "548788, область Ростовская, район Ивановский, г. Распушил, ул. Растеряева, д. 116Б, кв. 45";
            //получаем значения
            AdressFromLIB adress = new AdressFromLIB();
            AddressStructure res = adress.ParseAdress(s);
            string actual = adress.CompileAddress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Street_inputString7_resultString() //как это оформить на русском?
        {
            //вход
            string s = " дом 116Б, ул. Растеряева, кв-ра45, область Ростовская ,  548788   , город  Распушил ,";
            string expected = "548788, область Ростовская, г. Распушил, ул. Растеряева, д. 116Б, кв. 45";
            //получаем значения
            AdressFromLIB adress = new AdressFromLIB();
            AddressStructure res = adress.ParseAdress(s);
            string actual = adress.CompileAddress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputString8_resultString() //как это оформить на русском?
        {
            //вход
            string s = "дом 116Б, ул. Растеряева, кв-ра45, область Ростовская ,  548788   , Приморский Край , г Окраина";
            string expected = "548788, Приморский Край, г. Окраина, ул. Растеряева, д. 116Б, кв. 45";
            //получаем значения
            AdressFromLIB adress = new AdressFromLIB();
            AddressStructure res = adress.ParseAdress(s);
            string actual = adress.CompileAddress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Street_inputString9_resultString() //посёлок определяется верно, однако на последнем этапе он приводится к типу "город"
        {
            //вход
            string s = " пос Юрчимск, дом 116Б, ул. Растеряева, кв-ра45, область Ростовская ,  548788   , Алтайский Край ,";
            string expected = "548788, Алтайский Край, пос. Юрчимск, ул. Растеряева, д. 116Б, кв. 45";
            //фактический результат: "548788, Алтайский Край, г. Юрчимск, ул. Растеряева, д. 116Б, кв. 45"

            //получаем значения
            AdressFromLIB adress = new AdressFromLIB();
            AddressStructure res = adress.ParseAdress(s);
            string actual = adress.CompileAddress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }



        [TestMethod]
        public void Street_inputString10_resultString() //тест не проходит из-за того, что "город Край" распознается как город Край и как "край под названием "город""
        {
            //вход
            string s = "дом 116Б, ул. Растеряева, кв-ра45, область Ростовская ,  548788  , город Край";
            string expected = "548788, Приморский Край, г. Край, ул. Растеряева, д. 116Б, кв. 45";
            //получаем значения
            AdressFromLIB adress = new AdressFromLIB();
            AddressStructure res = adress.ParseAdress(s);
            string actual = adress.CompileAddress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputString11_resultString() //без индекса
        {
            //вход
            string s = "дом 116Б, ул. Растеряева, кв-ра45,  Приморский Край , г Край";
            string expected = "Приморский Край, г. Край, ул. Растеряева, д. 116Б, кв. 45";
            //получаем значения
            AdressFromLIB adress = new AdressFromLIB();
            AddressStructure res = adress.ParseAdress(s);
            string actual = adress.CompileAddress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputString12_resultString() //минимально допустимый адрес
        {
            //вход
            string s = "дом 116Б, ул. Растеряева";
            string expected = "Пермский край, г. Пермь, ул. Растеряева, д. 116Б";
            //получаем значения
            AdressFromLIB adress = new AdressFromLIB();
            AddressStructure res = adress.ParseAdress(s);
            string actual = adress.CompileAddress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputString13_resultString() //наиболее сложный адрес 
        {
            //вход
            //Еврейская Автономная область, Биробиджанский район, город Биробиджан, улица Степана Разина, дом 115/1, квартира 12
            string s = " 614825 , Еврейская Автономная область, Спасо Преображенский район, город Биробиджан , улица Степана Разина вторая , дом 115/1, квартира 12 ";
            string expected = "614825, Еврейская Автономная область, Спасо Преображенский район, г. Биробиджан, ул. Степана Разина вторая, д. 115/1, кв. 12";
            //получаем значения
            AdressFromLIB adress = new AdressFromLIB();
            AddressStructure res = adress.ParseAdress(s);
            string actual = adress.CompileAddress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Street_inputString14_resultString() //тест с названием города с цифрами
        {
            //вход
            //Еврейская Автономная область, Биробиджанский район, город Биробиджан, улица Степана Разина, дом 115/1, квартира 12
            string s = " 614825 , Еврейская Автономная область, Спасо Преображенский район, город 13-й километр , улица Степана Разина вторая , дом 115Б/1, квартира 12 ";
            string expected = "614825, Еврейская Автономная область, Спасо Преображенский район, г. 13-й километр, ул. Степана Разина вторая, д. 115Б/1, кв. 12";
            //получаем значения
            AdressFromLIB adress = new AdressFromLIB();
            AddressStructure res = adress.ParseAdress(s);
            string actual = adress.CompileAddress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputString15_resultString() //тест не пройден, т.к. "городской" район содержит слово "город", дальнейшее трактуется как название города
        {
            //вход
            //Еврейская Автономная область, Биробиджанский район, город Биробиджан, улица Степана Разина, дом 115/1, квартира 12
            string s = " 614825 , Еврейская Автономная область, городской район, город 13-й километр , улица Степана Разина вторая , дом 115Б/1, квартира 12 ";
            string expected = "614825, Еврейская Автономная область, городской район, г. 13-й километр, ул. Степана Разина вторая, д. 115Б/1, кв. 12";
            //получаем значения
            AdressFromLIB adress = new AdressFromLIB();
            AddressStructure res = adress.ParseAdress(s);
            string actual = adress.CompileAddress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }
    }
}
