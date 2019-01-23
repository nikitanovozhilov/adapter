using System;

namespace AdapterLibrary
{
    /// <summary>
    /// Параметры для построения.
    /// </summary>
    public class AdapterParameters
    {

        /// <summary>
        /// Создание параметров.
        /// </summary>
        /// <param name="bigDiameter">Большой диаметр.</param>
        /// <param name="smallDiameter">Малый диаметр.</param>
        /// <param name="wallThickness">Ширина стенки.</param>
        /// <param name="highAdapter">Высота муфты.</param>
        /// <param name="stepThread">Шаг резьбы.</param>
        /// <param name="filletRadius">Радиус скругления.</param>
        public AdapterParameters(float bigDiameter, float smallDiameter,
            float wallThickness, float highAdapter, float stepThread, float filletRadius)
        {
            BigDiameter = bigDiameter;
            SmallDiameter = smallDiameter;
            HighAdapter = highAdapter;
            WallThickness = wallThickness;
            StepThread = stepThread;
            FilletRadius = filletRadius;

            Validate();
            CheckValue(bigDiameter, "Некорректное значение большого диаметра.");
            CheckValue(smallDiameter, "Некорректное значение малого диаметра.");
            CheckValue(highAdapter, "Некорректное значение высоты муфты.");
            CheckValue(wallThickness, "Некорректное значение толщины стенки.");
            CheckValue(stepThread, "Некорректное значение шага резьбы.");
            CheckValue(filletRadius, "Некорректное значение радиуса скругления.");
        }

        /// <summary>
        /// Большой диаметр.
        /// </summary>
        public float BigDiameter { get; private set; }

        /// <summary>
        /// Малый диаметр.
        /// </summary>
        public float SmallDiameter { get; private set; }


        /// <summary>
        /// Высота муфты.
        /// </summary>
        public float HighAdapter { get; private set; }

        /// <summary>
        /// Шаг резьбы.
        /// </summary>
        public float StepThread { get; private set; }

        /// <summary>
        /// Толщина стенки муфты.
        /// </summary>
        public float WallThickness { get; private set; }

        /// <summary>
        /// Радиус скругления.
        /// </summary>
        public float FilletRadius { get; private set; }

        /// <summary>
        /// Проверка на NaN и Infinity поля.
        /// </summary>
        /// <param name="value">Значение параметра.</param>
        /// <param name="message">Текст сообщения.</param>
        private void CheckValue(float value, string message)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                throw new ArgumentException(message);
            }
        }

        /// <summary>
        /// Валидация данных по значению.
        /// </summary>
        private void Validate()
        {
            if (BigDiameter - SmallDiameter < 10)
            {
                throw new ArgumentException("Разница переходных диаметров " +
                                            "должна быть не менее 10 мм");
            }

            if (HighAdapter > 120 || HighAdapter < 60)
            {
                throw new ArgumentException("Высота муфты должна находиться" +
                                            " в диапозоне от 60 мм до 120 мм");
            }
            
            if (StepThread == 0f)
            {
                throw new ArgumentException("Не введено значение шага резьбы.");
            }

            if (WallThickness < 3 || WallThickness > 10) 
            {
                throw new ArgumentException("Толщина стенки муфты не может" +
                                            " быть меньше 3 мм и больше 10 мм");
            }

            if (BigDiameter < 30 || BigDiameter > 110)
            {
                throw new ArgumentException("Большой диаметр должен находиться" +
                                            " в диапозоне от 30 до 110 мм");
            }

            if (SmallDiameter < 20 || SmallDiameter > 100)
            {
                throw new ArgumentException("Малый диаметр должен находиться" +
                                            " в диапозоне от 20 до 110 мм");
            }

            if (FilletRadius < 1 || FilletRadius > 5)
            {
                throw new ArgumentException("Радиус скругления должен находиться" +
                                            " в диапозоне от 1 до 5");
            }
        }
    }
}

