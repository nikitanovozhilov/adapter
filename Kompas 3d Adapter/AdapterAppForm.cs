using AdapterLibrary;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace Kompas_3d_Adapter
{
    public partial class AdapterAppForm : Form
    {
        /// <summary>
        /// Ссылка на КОМПАС-3D.
        /// </summary>
        private KompasConnector _kompasConnector = new KompasConnector();

        /// <summary>
        /// Параметры детали.
        /// </summary>
        private AdapterParameters _parameters;

        /// <summary>
        /// Ссылка на построитель.
        /// </summary>
        private AdapterBuilder _builder;

        public AdapterAppForm()
        {
            InitializeComponent();
        }

        private void StartKompasButton_Click(object sender, EventArgs e)
        {
            try
            {
                _kompasConnector.ConnectKompas();
                BuildButton.Enabled = true;
                StartKompasButton.Enabled = false;
                CloseKompasButton.Enabled = true;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CloseKompasButton_Click(object sender, EventArgs e)
        {
            try
            {
                _kompasConnector.DisconnectKompas();
                BuildButton.Enabled = false;
                StartKompasButton.Enabled = true;
                CloseKompasButton.Enabled = false;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BuildButton_Click(object sender, EventArgs e)
        {
            try
            {
                var val = 0f;
                float.TryParse(FieldStepThread.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out val);
                _parameters = new AdapterParameters((float) FieldBigDiameter.Value,
                    (float) FieldSmallDiameter.Value, (float) FieldWallThickness.Value,
                    (float) FieldHighAdapter.Value, val,
                    (float) FieldFilletRadius.Value);
                _builder = new AdapterBuilder(_kompasConnector);
                _builder.AdapterBuild(_parameters);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обновление пределов значений.
        /// </summary>
        private void RefreshValues()
        {
            FieldSmallDiameter.Maximum = FieldBigDiameter.Value - (decimal)AdapterParameters.subDiameters;
            FieldBigDiameter.Minimum = FieldSmallDiameter.Value + (decimal)AdapterParameters.subDiameters;
        }

        private void FieldBigDiameter_ValueChanged(object sender, EventArgs e)
        {
            RefreshValues();
        }

        private void FieldSmallDiameter_ValueChanged(object sender, EventArgs e)
        {
            RefreshValues();
        }
    }
}