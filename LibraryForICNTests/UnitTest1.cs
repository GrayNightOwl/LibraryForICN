using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryForICN;

namespace LibraryForICNTests
{
    [TestClass]
    public class AdressTests
    {

        /* Найденные ошибки:
         * посёлок распознаётся корректно, но в результате указывается как "город" - нет признака, посёлок это, деревня или город
         * фраза "город Край" трактуется и как название края, и как название города
         * можно ли уйти от окончаний в регулярных выражениях?
         * 2-ное и 3-ное регулярное выражение, поправить
         * область может состоять из двух слов
         * в названии города/деревни добавить цифры и тире
         * может быть дом "115Б/1"
         * подумать над передачей ошибки распознавания- по сути, там может быть любая строка
         * код ошибки?
        */


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
            string s = " д. 70Б,ул Гагарина бульвар, край Пермский ,  614000   , гор  Пермь , ";
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
            string s = " д. 70/2, улСоловьева, кв. 45, край Пермский ,  614285   , г  Пермь , ";
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
            string s = " д. 70/2, улСоловьева, кв. 45, край Пермский ,  614285   , г. Край , ";
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


        [TestMethod]
        public void Street_inputString6_resultString() //как это оформить на русском?
        {
            //вход
            string s = " дом 116Б, ул. Растеряева, кв-ра45, область Ростовская ,  548788   , город  Распушил ,район Ивановский ,";
            string expected = "548788, область Ростовская, район Ивановский, г. Распушил, ул. Растеряева, д. 116Б, кв. 45";
            //получаем значения
            Adress adress = new Adress();
            AdressForCompile res = adress.ParseAdress(s);
            string actual = adress.CompileAdress(res);

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
            Adress adress = new Adress();
            AdressForCompile res = adress.ParseAdress(s);
            string actual = adress.CompileAdress(res);

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
            Adress adress = new Adress();
            AdressForCompile res = adress.ParseAdress(s);
            string actual = adress.CompileAdress(res);

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
            Adress adress = new Adress();
            AdressForCompile res = adress.ParseAdress(s);
            string actual = adress.CompileAdress(res);

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
            Adress adress = new Adress();
            AdressForCompile res = adress.ParseAdress(s);
            string actual = adress.CompileAdress(res);

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
            Adress adress = new Adress();
            AdressForCompile res = adress.ParseAdress(s);
            string actual = adress.CompileAdress(res);

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
            Adress adress = new Adress();
            AdressForCompile res = adress.ParseAdress(s);
            string actual = adress.CompileAdress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputString13_resultString() //наиболее сложный адрес 
        {
            //вход
            //Еврейская Автономная область, Биробиджанский район, город Биробиджан, улица Степана Разина, дом 115/1, квартира 12
            string s = " 614825 , Еврейская Автономная область, Биробиджанский район, город Биробиджан , улица Степана Разина , дом 115/1, квартира 12 ";
            string expected = "614825, Пермский край, Биробиджанский район, г. Биробиджан, ул. Степана Разина, д. 115/1, кв. 12";
            //получаем значения
            Adress adress = new Adress();
            AdressForCompile res = adress.ParseAdress(s);
            string actual = adress.CompileAdress(res);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }
    }
}
