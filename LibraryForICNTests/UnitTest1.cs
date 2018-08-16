using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryForICN;

namespace LibraryForICNTests
{
    [TestClass]
    public class AdressTests
    {
        [TestMethod]
        public void Street_inputString2_resultString() //как это оформить на русском?
        {
            //вход

            string s = "ул Синяя, г Пермь,кв4 , дом 15";
            string expected = "Синяя";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Street_inputString3_resultString() //как это оформить на русском?
        {
            //вход
            string s = "ул. Синяя, г Пермь,кв4 , дом 15";
            string expected = "Синяя";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Street_inputString4_resultString() //как это оформить на русском?
        {
            //вход
            string s = "ул.Синяя, г Пермь,кв4 , дом 15";
            string expected = "Синяя";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputString5_resultString() //как это оформить на русском?
        {
            //вход
            string s = "улСиняя, г Пермь,кв4 , дом 15";
            string expected = "Синяя";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputString6_resultString() //как это оформить на русском?
        {
            //вход
            string s = "УлСиняя, г Пермь,кв4 , дом 15";
            string expected = "Синяя";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputString7_resultString() //как это оформить на русском?
        {
            //вход
            string s = "Ул Синяя, г Пермь,кв4 , дом 15";
            string expected = "Синяя";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputString8_resultString() //как это оформить на русском?
        {
            //вход
            string s = "Ул. Синяя, г Пермь,кв4 , дом 15";
            string expected = "Синяя";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputStringWithTwoWords1_resultString() //как это оформить на русском?
        {
            //вход
            string s = "ул. Генерала Лизюкова, г Пермь,кв4 , дом 15";
            string expected = "Генерала Лизюкова";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Street_inputStringWithTwoWords2_resultString() //как это оформить на русском?
        {
            //вход
            string s = "ул Генерала Лизюкова, г Пермь,кв4 , дом 15";
            string expected = "Генерала Лизюкова";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputStringWithTwoWords3_resultString() //как это оформить на русском?
        {
            //вход
            string s = "ул. Генерала Лизюкова, г Пермь,кв4 , дом 15";
            string expected = "Генерала Лизюкова";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Street_inputStringWithTwoWords4_resultString() //как это оформить на русском?
        {
            //вход
            string s = "ул.Генерала Лизюкова, г Пермь,кв4 , дом 15";
            string expected = "Генерала Лизюкова";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Street_inputStringWithTwoWords5_resultString() //как это оформить на русском?
        {
            //вход
            string s = "улГенерала Лизюкова, г Пермь,кв4 , дом 15";
            string expected = "Генерала Лизюкова";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Street_inputStringWithTwoWords6_resultString() //как это оформить на русском?
        {
            //вход
            string s = "УлГенерала Лизюкова, г Пермь,кв4 , дом 15";
            string expected = "Генерала Лизюкова";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Street_inputStringWithTwoWords7_resultString() //как это оформить на русском?
        {
            //вход
            string s = "Ул.Генерала Лизюкова, г Пермь,кв4 , дом 15";
            string expected = "Генерала Лизюкова";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputStringWithTwoWords8_resultString() //как это оформить на русском?
        {
            //вход
            string s = "Ул Генерала Лизюкова, г Пермь,кв4 , дом 15";
            string expected = "Генерала Лизюкова";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Street_inputStringWithTwoWords9_resultString() //как это оформить на русском?
        {
            //вход
            string s = "Ул. Генерала Лизюкова, г Пермь,кв4 , дом 15";
            string expected = "Генерала Лизюкова";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }



        [TestMethod]
        public void Street_inputStringWithTwoWordsWithADash1_resultString() //как это оформить на русском?
        {
            //вход
            string s = "Ул.Генерала-Лизюкова, г Пермь,кв4 , дом 15";
            string expected = "Генерала-Лизюкова";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Street_inputStringWithTwoWordsWithADash2_resultString() //как это оформить на русском?
        {
            //вход
            string s = "Ул Генерала-Лизюкова, г Пермь,кв4 , дом 15";
            string expected = "Генерала-Лизюкова";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputStringWithTwoWordsWithADash3_resultString() //как это оформить на русском?
        {
            //вход
            string s = "Ул. Генерала-Лизюкова, г Пермь,кв4 , дом 15";
            string expected = "Генерала-Лизюкова";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Street_inputStringWithTwoWordsWithADash4_resultString() //как это оформить на русском?
        {
            //вход
            string s = "УлГенерала-Лизюкова, г Пермь,кв4 , дом 15";
            string expected = "Генерала-Лизюкова";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputStringWithTwoWordsWithADash5_resultString() //как это оформить на русском?
        {
            //вход
            string s = "ул.Генерала-Лизюкова, г Пермь,кв4 , дом 15";
            string expected = "Генерала-Лизюкова";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Street_inputStringWithTwoWordsWithADash6_resultString() //как это оформить на русском?
        {
            //вход
            string s = "ул Генерала-Лизюкова, г Пермь,кв4 , дом 15";
            string expected = "Генерала-Лизюкова";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputStringWithTwoWordsWithADash7_resultString() //как это оформить на русском?
        {
            //вход
            string s = "ул. Генерала-Лизюкова, г Пермь,кв4 , дом 15";
            string expected = "Генерала-Лизюкова";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputStringWithTwoWordsWithADash8_resultString() //как это оформить на русском?
        {
            //вход
            string s = "    улГенерала - Лизюкова    , г Пермь,кв4 , дом 15";
            string expected = "Генерала - Лизюкова";

            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }

        
        [TestMethod]
        public void Street_inputStringWithSpaces_resultString() //как это оформить на русском?
        { 
            //вход
            string s = "улГенерала - Лизюкова, г Пермь,кв4 , дом 15";
            string expected = "Генерала - Лизюкова";
            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Street_inputStringDifficult_resultString() //как это оформить на русском?
        {
            //вход
            string s = "ул. 1-я Красноармейская, г Пермь,кв4 , дом 15";
            string expected = "1-я Красноармейская";
            //получаем значения
            Adress adress = new Adress();
            string actual = adress.Street(s);

            //сравнение результатов
            Assert.AreEqual(expected, actual);
        }
    }
}
