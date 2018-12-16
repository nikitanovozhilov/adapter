using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterLibrary
{
    /// <summary>
    /// Параметры для построения.
    /// </summary>
    public class Parameters
    {
        private float _bigDiameter;
        private float _highAdapter;
        private float _smallDiameter;
        private float _stepThread;
        private float _wallThickness;

        /// <summary>
        /// Создание параметров.
        /// </summary>
        /// <param name="bigDiameter">Большой диаметр.</param>
        /// <param name="highAdapter">Высота муфты.</param>
        /// <param name="smallDiameter">Малый диаметр.</param>
        /// <param name="stepThread">Шаг резьбы.</param>
        /// <param name="wallThickness">Толщина стенки муфты.</param>
        public Parameters(float bigDiameter, float highAdapter, float smallDiameter, float stepThread,
            float wallThickness)
        {
            BigDiameter = bigDiameter;
            HighAdapter = highAdapter;
            SmallDiameter = smallDiameter;
            StepThread = stepThread;
            WallThickness = wallThickness;
            
            Validate();
        }

        /// <summary>
        /// Большой диаметр.
        /// </summary>
        public float BigDiameter
        {
            get => _bigDiameter;
            private set { _bigDiameter = value; }
        }

        /// <summary>
        /// Большой диаметр.
        /// </summary>
        public float SmallDiameter
        {
            get => _smallDiameter;
            private set { _smallDiameter = value; }
        }


        /// <summary>
        /// Высота муфты.
        /// </summary>
        public float HighAdapter
        {
            get => _highAdapter;
            private set { _highAdapter = value; }
        }

        /// <summary>
        /// Шаг резьбы.
        /// </summary>
        public float StepThread
        {
            get => _stepThread;
            private set { _stepThread = value; }
        }

        /// <summary>
        /// Толщина стенки муфты.
        /// </summary>
        public float WallThickness
        {
            get => _wallThickness;
            private set { _wallThickness = value; }
        }

        //Валидация данных по значению.
        private void Validate()
        {
            if (BigDiameter - SmallDiameter <= 10)
            {
                throw new ArgumentException("Разница переходных диаметров должна быть не менее 10 мм");
            }

            if (HighAdapter > 120 || HighAdapter < 60 || float.IsNaN(HighAdapter))
            {
                throw  new ArgumentException("Высота муфты должна находиться в диапозоне от 60 мм до 120 мм");
            }

            if (float.IsNaN(StepThread))
            {
                throw new ArgumentException("Не введено значение шага резьбы.");
            }

            if (WallThickness < 3 || WallThickness > 10 || float.IsNaN(WallThickness)) 
            {
                throw new ArgumentException("Толщина стенки муфты не может быть меньше 3 мм и больше 10 мм");
            }

            if (BigDiameter < 30 || BigDiameter > 110 || float.IsNaN(BigDiameter))
            {
                throw new ArgumentException("Большой диаметр должен находиться в диапозоне от 30 до 110 мм");
            }

            if (SmallDiameter < 20 || SmallDiameter > 100 || float.IsNaN(SmallDiameter))
            {
                throw new ArgumentException("Малый диаметр должен находиться в диапозоне от 30 до 110 мм");
            }
        }
    }
}

