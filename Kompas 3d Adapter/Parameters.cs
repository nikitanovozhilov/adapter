using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        /// <summary>
        /// Создание параметров.
        /// </summary>
        /// <param name="bigDiameter">Большой диаметр.</param>
        /// <param name="highAdapter">Высота муфты.</param>
        /// <param name="smallDiameter">Малый диаметр.</param>
        /// <param name="stepThread">Шаг резьбы.</param>
        /// <param name="wallTheckness">Толщина стенки муфты.</param>
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


       /* private void Validate()
        {
            var errorMessage = new List<String>();

            if (BigDiameter - SmallDiameter = 5 && BigDiameter => 25 && BigDiameter <= 110)
            {
                errorMessage.Add("Большой диаметр переходной муфты должен " +
                                 "быть больше малого диаметра минимум на 5 мм и находиться в диапозоне от 25 мм до 110 мм");
            }

            if (SmallDiameter >= 20 && SmallDiameter <= 105)
            {
                errorMessage.Add("Малый диаметр переходной муфты должен " +
                                 "находиться в диапозоне от 20 мм до 105 мм");
            }


        }
        */
    }
}
