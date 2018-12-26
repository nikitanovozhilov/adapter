﻿using System;

namespace AdapterLibrary
{
    /// <summary>
    /// Параметры для построения.
    /// </summary>
    public class AdapterParameters
    {
        private float _bigDiameter;
        private float _highAdapter;
        private float _smallDiameter;
        private float _stepThread;
        private float _wallThickness;
        private bool _outerThread;

        /// <summary>
        /// Создание параметров.
        /// </summary>
        /// <param name="bigDiameter">Большой диаметр.</param>
        /// <param name="highAdapter">Высота муфты.</param>
        /// <param name="smallDiameter">Малый диаметр.</param>
        /// <param name="stepThread">Шаг резьбы.</param>
        /// <param name="wallThickness">Толщина стенки муфты.</param>
        /// <param name="outerThread">Внешняя резьба.</param>
        public AdapterParameters(float bigDiameter, float smallDiameter, float wallThickness,
                          float highAdapter, float stepThread, bool outerThread)
        {
            BigDiameter = bigDiameter;
            SmallDiameter = smallDiameter;
            HighAdapter = highAdapter;
            WallThickness = wallThickness;
            StepThread = stepThread;
            OuterThread = outerThread;

            Validate();
        }

        /// <summary>
        /// Большой диаметр.
        /// </summary>
        public float BigDiameter
        {
            get => _bigDiameter;
            private set => _bigDiameter = value;
        }

        /// <summary>
        /// Большой диаметр.
        /// </summary>
        public float SmallDiameter
        {
            get => _smallDiameter;
            private set => _smallDiameter = value;
        }


        /// <summary>
        /// Высота муфты.
        /// </summary>
        public float HighAdapter
        {
            get => _highAdapter;
            private set => _highAdapter = value;
        }

        /// <summary>
        /// Шаг резьбы.
        /// </summary>
        public float StepThread
        {
            get => _stepThread;
            private set => _stepThread = value;
        }

        /// <summary>
        /// Толщина стенки муфты.
        /// </summary>
        public float WallThickness
        {
            get => _wallThickness;
            private set => _wallThickness = value;
        }

        public bool OuterThread
        {
            get => _outerThread;
            private set => _outerThread = value;
        }

        //Валидация данных по значению.
        private void Validate()
        {
            if (BigDiameter - SmallDiameter < 10)
            {
                throw new ArgumentException("Разница переходных диаметров должна быть не менее 10 мм");
            }

            if (HighAdapter > 120 || HighAdapter < 60 || float.IsNaN(HighAdapter) || float.IsInfinity(HighAdapter))
            {
                throw  new ArgumentException("Высота муфты должна находиться в диапозоне от 60 мм до 120 мм");
            }

            if (StepThread == 0f || float.IsNaN(StepThread) || float.IsInfinity(StepThread))
            {
                throw new ArgumentException("Не введено значение шага резьбы.");
            }

            if (WallThickness < 3 || WallThickness > 10 || float.IsNaN(WallThickness) || float.IsInfinity(WallThickness)) 
            {
                throw new ArgumentException("Толщина стенки муфты не может быть меньше 3 мм и больше 10 мм");
            }

            if (BigDiameter < 30 || BigDiameter > 110 || float.IsNaN(BigDiameter) || float.IsInfinity(BigDiameter))
            {
                throw new ArgumentException("Большой диаметр должен находиться в диапозоне от 30 до 110 мм");
            }

            if (SmallDiameter < 20 || SmallDiameter > 100 || float.IsNaN(SmallDiameter) || float.IsInfinity(SmallDiameter))
            {
                throw new ArgumentException("Малый диаметр должен находиться в диапозоне от 20 до 110 мм");
            }
        }
    }
}

