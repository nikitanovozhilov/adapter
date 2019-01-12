﻿using AdapterLibrary;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace Kompas_3d_Adapter
{
    public partial class AdapterAppForm : Form
    {
        private KompasConnector _kompasConnector = new KompasConnector();

        private AdapterParameters _parameters;

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
            catch (Exception ex)
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
            catch (Exception ex)
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
                _parameters = new AdapterParameters((float) FieldBigDiameter.Value, (float) FieldSmallDiameter.Value,
                    (float) FieldWallThickness.Value, (float) FieldHighAdapter.Value, val,
                    (float) FieldfilletRadius.Value);
                _builder = new AdapterBuilder(_kompasConnector);
                _builder.AdapterBuild(_parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FieldBigDiameter_ValueChanged(object sender, EventArgs e)
        {
            FieldSmallDiameter.Maximum = FieldBigDiameter.Value - 10;
            FieldBigDiameter.Minimum = FieldSmallDiameter.Value + 10;
        }
    }
}