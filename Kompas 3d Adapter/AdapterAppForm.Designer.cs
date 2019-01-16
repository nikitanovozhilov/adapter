namespace Kompas_3d_Adapter
{
    partial class AdapterAppForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdapterAppForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CloseKompasButton = new System.Windows.Forms.Button();
            this.StartKompasButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.FieldFilletRadius = new System.Windows.Forms.NumericUpDown();
            this.FieldHighAdapter = new System.Windows.Forms.NumericUpDown();
            this.FieldWallThickness = new System.Windows.Forms.NumericUpDown();
            this.FieldSmallDiameter = new System.Windows.Forms.NumericUpDown();
            this.FieldBigDiameter = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.BuildButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.FieldStepThread = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FieldFilletRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FieldHighAdapter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FieldWallThickness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FieldSmallDiameter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FieldBigDiameter)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CloseKompasButton);
            this.groupBox1.Controls.Add(this.StartKompasButton);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(215, 59);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "KOMPAS 3D";
            // 
            // CloseKompasButton
            // 
            this.CloseKompasButton.Enabled = false;
            this.CloseKompasButton.Location = new System.Drawing.Point(130, 20);
            this.CloseKompasButton.Name = "CloseKompasButton";
            this.CloseKompasButton.Size = new System.Drawing.Size(75, 23);
            this.CloseKompasButton.TabIndex = 1;
            this.CloseKompasButton.Text = "Close";
            this.CloseKompasButton.UseVisualStyleBackColor = true;
            this.CloseKompasButton.Click += new System.EventHandler(this.CloseKompasButton_Click);
            // 
            // StartKompasButton
            // 
            this.StartKompasButton.Location = new System.Drawing.Point(7, 20);
            this.StartKompasButton.Name = "StartKompasButton";
            this.StartKompasButton.Size = new System.Drawing.Size(75, 23);
            this.StartKompasButton.TabIndex = 0;
            this.StartKompasButton.Text = "Start";
            this.StartKompasButton.UseVisualStyleBackColor = true;
            this.StartKompasButton.Click += new System.EventHandler(this.StartKompasButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.FieldFilletRadius);
            this.groupBox2.Controls.Add(this.FieldHighAdapter);
            this.groupBox2.Controls.Add(this.FieldWallThickness);
            this.groupBox2.Controls.Add(this.FieldSmallDiameter);
            this.groupBox2.Controls.Add(this.FieldBigDiameter);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.BuildButton);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.FieldStepThread);
            this.groupBox2.Location = new System.Drawing.Point(13, 78);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(215, 207);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parameters";
            // 
            // FieldFilletRadius
            // 
            this.FieldFilletRadius.DecimalPlaces = 2;
            this.FieldFilletRadius.Location = new System.Drawing.Point(102, 150);
            this.FieldFilletRadius.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.FieldFilletRadius.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FieldFilletRadius.Name = "FieldFilletRadius";
            this.FieldFilletRadius.Size = new System.Drawing.Size(101, 20);
            this.FieldFilletRadius.TabIndex = 15;
            this.FieldFilletRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.FieldFilletRadius.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // FieldHighAdapter
            // 
            this.FieldHighAdapter.DecimalPlaces = 2;
            this.FieldHighAdapter.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.FieldHighAdapter.Location = new System.Drawing.Point(102, 97);
            this.FieldHighAdapter.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.FieldHighAdapter.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.FieldHighAdapter.Name = "FieldHighAdapter";
            this.FieldHighAdapter.Size = new System.Drawing.Size(103, 20);
            this.FieldHighAdapter.TabIndex = 14;
            this.FieldHighAdapter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.FieldHighAdapter.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // FieldWallThickness
            // 
            this.FieldWallThickness.DecimalPlaces = 2;
            this.FieldWallThickness.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.FieldWallThickness.Location = new System.Drawing.Point(102, 72);
            this.FieldWallThickness.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.FieldWallThickness.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.FieldWallThickness.Name = "FieldWallThickness";
            this.FieldWallThickness.Size = new System.Drawing.Size(103, 20);
            this.FieldWallThickness.TabIndex = 13;
            this.FieldWallThickness.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.FieldWallThickness.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // FieldSmallDiameter
            // 
            this.FieldSmallDiameter.DecimalPlaces = 2;
            this.FieldSmallDiameter.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.FieldSmallDiameter.Location = new System.Drawing.Point(102, 46);
            this.FieldSmallDiameter.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.FieldSmallDiameter.Name = "FieldSmallDiameter";
            this.FieldSmallDiameter.Size = new System.Drawing.Size(103, 20);
            this.FieldSmallDiameter.TabIndex = 12;
            this.FieldSmallDiameter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.FieldSmallDiameter.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // FieldBigDiameter
            // 
            this.FieldBigDiameter.DecimalPlaces = 2;
            this.FieldBigDiameter.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.FieldBigDiameter.Location = new System.Drawing.Point(102, 20);
            this.FieldBigDiameter.Maximum = new decimal(new int[] {
            110,
            0,
            0,
            0});
            this.FieldBigDiameter.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.FieldBigDiameter.Name = "FieldBigDiameter";
            this.FieldBigDiameter.Size = new System.Drawing.Size(103, 20);
            this.FieldBigDiameter.TabIndex = 11;
            this.FieldBigDiameter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.FieldBigDiameter.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.FieldBigDiameter.ValueChanged += new System.EventHandler(this.FieldBigDiameter_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Wall Thickness";
            // 
            // BuildButton
            // 
            this.BuildButton.Enabled = false;
            this.BuildButton.Location = new System.Drawing.Point(7, 173);
            this.BuildButton.Name = "BuildButton";
            this.BuildButton.Size = new System.Drawing.Size(196, 23);
            this.BuildButton.TabIndex = 8;
            this.BuildButton.Text = "Build";
            this.BuildButton.UseVisualStyleBackColor = true;
            this.BuildButton.Click += new System.EventHandler(this.BuildButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "High Adapter";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Step Thread";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Small Diameter";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Big Diameter";
            // 
            // FieldStepThread
            // 
            this.FieldStepThread.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FieldStepThread.FormattingEnabled = true;
            this.FieldStepThread.Items.AddRange(new object[] {
            "0.75",
            "1",
            "1.5",
            "2",
            "3"});
            this.FieldStepThread.Location = new System.Drawing.Point(102, 123);
            this.FieldStepThread.Name = "FieldStepThread";
            this.FieldStepThread.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.FieldStepThread.Size = new System.Drawing.Size(103, 21);
            this.FieldStepThread.Sorted = true;
            this.FieldStepThread.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 152);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Fillet Radius";
            // 
            // AdapterAppForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 297);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(256, 336);
            this.Name = "AdapterAppForm";
            this.Text = "Adapter";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FieldFilletRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FieldHighAdapter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FieldWallThickness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FieldSmallDiameter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FieldBigDiameter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button CloseKompasButton;
        private System.Windows.Forms.Button StartKompasButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button BuildButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox FieldStepThread;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown FieldWallThickness;
        private System.Windows.Forms.NumericUpDown FieldSmallDiameter;
        private System.Windows.Forms.NumericUpDown FieldBigDiameter;
        private System.Windows.Forms.NumericUpDown FieldHighAdapter;
        private System.Windows.Forms.NumericUpDown FieldFilletRadius;
        private System.Windows.Forms.Label label6;
    }
}

