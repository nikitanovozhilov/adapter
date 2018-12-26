﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdapterLibrary;

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


        private void StartKompasButton_Click_1(object sender, EventArgs e)
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

        private void CloseKompasButton_Click_1(object sender, EventArgs e)
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
            var val = 0f;
            float.TryParse(FieldStepThread.Text, out val);
            _parameters = new AdapterParameters((float)FieldBigDiameter.Value, (float)FieldSmallDiameter.Value, 
                                         (float)FieldWallThickness.Value, (float)FieldHighAdapter.Value, val);
            _builder = new AdapterBuilder(_kompasConnector);
            _builder.AdapterBuild(_parameters);
        }

        private void FieldBigDiameter_ValueChanged(object sender, EventArgs e)
        {
            FieldSmallDiameter.Maximum = FieldBigDiameter.Value - 10;
        }
    }
}