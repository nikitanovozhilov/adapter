using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOMPAS_3D_Adapter
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
        private float _wallTheckness;

        public Parameters (float bigDiameter, float highAdapter, float smallDiameter, float stepThread, float wallTheckness)
        {
            BigDiameter = bigDiameter;
            HighAdapter = highAdapter;
            SmallDiameter = smallDiameter;
            StepThread = stepThread;
            WallTheckness = wallTheckness;
        }

        /// <summary>
        /// Большой диаметр.
        /// </summary>
        public float BigDiameter { get; private set; }

        /// <summary>
        /// Высота муфты.
        /// </summary>
        public float HighAdapter { get; private set; }

        /// <summary>
        /// Малый диаметр.
        /// </summary>
        public float SmallDiameter { get; private set; }

        /// <summary>
        /// Шаг резьбы.
        /// </summary>
        public float StepThread { get; private set; }

        /// <summary>
        /// Толщина стенки муфты.
        /// </summary>
        public float WallTheckness { get; private set; }
    }
}
