using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdapterLibrary;
using NUnit.Framework;

namespace AdapterUnitTest
{
    [TestFixture]
    public class UnitTests
    {
        private AdapterParameters _parameters;

        [SetUp]
        public void Test()
        {
            _parameters = new AdapterParameters(30, 20, 5, 60, 1, false);
        }

        [Test(Description = "Позитивный тест конструктора класса ")]
        public void TestParameters_CorrectValue()
        {
            var expectedParameters = new AdapterParameters(30, 20, 5, 60, 1, false);
            var actual = _parameters;

            Assert.AreEqual
                (expectedParameters.BigDiameter, actual.BigDiameter,
                "Некорректное значение BigDiameter");
            Assert.AreEqual
                (expectedParameters.SmallDiameter, actual.SmallDiameter,
                "Некорректное значение SmallDiameter");
            Assert.AreEqual
                (expectedParameters.WallThickness, actual.WallThickness,
                "Некорректное значение WallThickness");
            Assert.AreEqual
                (expectedParameters.HighAdapter, actual.HighAdapter,
                "Некорректное значение HighAdapter");
            Assert.AreEqual
                (expectedParameters.StepThread, actual.StepThread,
                "Некорректное значение StepThread");
            Assert.AreEqual
                (expectedParameters.OuterThread, actual.OuterThread,
                "Некорректное значение OuterThread");
        }

        [TestCase(float.NegativeInfinity, 20, 5, 60, 1, false, "BigDiameter",
            TestName = "Негативный тест на infinity поля BigDiameter")]
        [TestCase(30, float.NegativeInfinity, 5, 60, 1, false, "SmallDiameter",
            TestName = "Негативный тест на infinity поля SmallDiameter")]
        [TestCase(30, 20, float.NegativeInfinity, 60, 1, false, "WallThickness",
            TestName = "Негативный тест на infinity поля WallThickness")]
        [TestCase(30, 20, 5, float.NegativeInfinity, 1, false, "HighAdapter",
            TestName = "Негативный тест на infinity поля HighAdapter")]
        [TestCase(30, 20, 5, 60, float.NegativeInfinity, false, "StepThread",
            TestName = "Негативный тест на infinity поля StepThread")]

        [TestCase(float.NaN, 20, 5, 60, 1, false, "BigDiameter",
            TestName = "Негативный тест на NaN поля BigDiameter")]
        [TestCase(30, float.NaN, 5, 60, 1, false, "SmallDiameter",
            TestName = "Негативный тест на NaN поля SmallDiameter")]
        [TestCase(30, 20, float.NaN, 60, 1, false, "WallThickness",
            TestName = "Негативный тест на NaN поля WallThickness")]
        [TestCase(30, 20, 5, float.NaN, 1, false, "HighAdapter",
            TestName = "Негативный тест на NaN поля HighAdapter")]
        [TestCase(30, 20, 5, 60, float.NaN, false, "StepThread",
            TestName = "Негативный тест на NaN поля StepThread")]

        [TestCase(1000, 20, 5, 60, 1, false, "BigDiameter",
            TestName = "Негативный тест поля BigDiameter если > 110")]
        [TestCase(10, 20, 5, 60, 1, false, "BigDiameter",
            TestName = "Негативный тест поля BigDiameter если < 30")]
        [TestCase(30, 40, 5, 60, 1, false, "SmallDiameter",
            TestName = "Негативный тест поля SmallDiameter если > BigDiameter")]
        [TestCase(30, 25, 5, 60, 1, false, "SmallDiameter",
            TestName = "Негативный тест поля SmallDiameter если SmallDiameter - BigDiameter < 10")]
        [TestCase(30, 10, 5, 60, 1, false, "SmallDiameter",
            TestName = "Негативный тест поля SmallDiameter если < 10")]
        [TestCase(30, 110, 5, 60, 1, false, "SmallDiameter",
            TestName = "Негативный тест поля SmallDiameter если > 100")]
        [TestCase(30, 20, 12, 60, 1, false, "WallThickness",
            TestName = "Негативный тест поля WallThickness если > 12")]
        [TestCase(30, 20, 2, 60, 1, false, "WallThickness",
            TestName = "Негативный тест поля WallThickness если < 3")]
        [TestCase(30, 20, 5, 500, 1, false, "HighAdapter",
            TestName = "Негативный тест поля HighAdapter если > 120")]
        [TestCase(30, 20, 5, 30, 1, false, "HighAdapter",
            TestName = "Негативный тест поля HighAdapter если < 60")]
        [TestCase(30, 20, 5, 30, 1, false, "HighAdapter",
            TestName = "Негативный тест поля HighAdapter если < 60")]
        [TestCase(30, 20, 5, 30, 1, false, "HighAdapter",
            TestName = "Негативный тест поля HighAdapter если < 60")]

        public void TestParameters_ArgumentValue
            (float bigDiameter, float smallDiameter, float wallThickness,
            float highAdapter, float stepThread, bool outerThread, string attr)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var parameters = new AdapterParameters
                    (bigDiameter, smallDiameter, wallThickness, highAdapter, stepThread, outerThread);
            }, "Должно возникнуть исключение если значение поля "
               + attr + "выходит за диапозон допустимых значений");
        }
    }
}
