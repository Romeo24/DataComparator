﻿namespace DataComparator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_Import = new System.Windows.Forms.Button();
            this.btn_Export = new System.Windows.Forms.Button();
            this.btn_Select = new System.Windows.Forms.Button();
            this.tbx_FilePath = new System.Windows.Forms.TextBox();
            this.chbx_DataFromDC = new System.Windows.Forms.CheckBox();
            this.chbx_SalesFromBAT = new System.Windows.Forms.CheckBox();
            this.chbx_DebtFromBAT = new System.Windows.Forms.CheckBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.cmbbx_dc_list = new System.Windows.Forms.ComboBox();
            this.chbx_ArchivedStockstFromBAT = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Import
            // 
            this.btn_Import.Location = new System.Drawing.Point(91, 41);
            this.btn_Import.Name = "btn_Import";
            this.btn_Import.Size = new System.Drawing.Size(72, 23);
            this.btn_Import.TabIndex = 0;
            this.btn_Import.Text = "Імпорт";
            this.btn_Import.UseVisualStyleBackColor = true;
            this.btn_Import.Click += new System.EventHandler(this.btn_Import_Click);
            // 
            // btn_Export
            // 
            this.btn_Export.Location = new System.Drawing.Point(261, 41);
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.Size = new System.Drawing.Size(72, 23);
            this.btn_Export.TabIndex = 1;
            this.btn_Export.Text = "Експорт";
            this.btn_Export.UseVisualStyleBackColor = true;
            this.btn_Export.Click += new System.EventHandler(this.btn_Export_Click);
            // 
            // btn_Select
            // 
            this.btn_Select.Location = new System.Drawing.Point(13, 41);
            this.btn_Select.Name = "btn_Select";
            this.btn_Select.Size = new System.Drawing.Size(72, 23);
            this.btn_Select.TabIndex = 2;
            this.btn_Select.Text = "Вибір";
            this.btn_Select.UseVisualStyleBackColor = true;
            this.btn_Select.Click += new System.EventHandler(this.btn_Select_Click);
            // 
            // tbx_FilePath
            // 
            this.tbx_FilePath.Location = new System.Drawing.Point(13, 15);
            this.tbx_FilePath.Name = "tbx_FilePath";
            this.tbx_FilePath.Size = new System.Drawing.Size(524, 20);
            this.tbx_FilePath.TabIndex = 3;
            // 
            // chbx_DataFromDC
            // 
            this.chbx_DataFromDC.AutoSize = true;
            this.chbx_DataFromDC.Location = new System.Drawing.Point(0, 19);
            this.chbx_DataFromDC.Name = "chbx_DataFromDC";
            this.chbx_DataFromDC.Size = new System.Drawing.Size(240, 17);
            this.chbx_DataFromDC.TabIndex = 4;
            this.chbx_DataFromDC.Text = "Продажі, залишки і дебіторка з Додатка-2";
            this.chbx_DataFromDC.UseVisualStyleBackColor = true;
            // 
            // chbx_SalesFromBAT
            // 
            this.chbx_SalesFromBAT.AutoSize = true;
            this.chbx_SalesFromBAT.Location = new System.Drawing.Point(0, 36);
            this.chbx_SalesFromBAT.Name = "chbx_SalesFromBAT";
            this.chbx_SalesFromBAT.Size = new System.Drawing.Size(101, 17);
            this.chbx_SalesFromBAT.TabIndex = 5;
            this.chbx_SalesFromBAT.Text = "Продажі з BAT";
            this.chbx_SalesFromBAT.UseVisualStyleBackColor = true;
            // 
            // chbx_DebtFromBAT
            // 
            this.chbx_DebtFromBAT.AutoSize = true;
            this.chbx_DebtFromBAT.Location = new System.Drawing.Point(0, 53);
            this.chbx_DebtFromBAT.Name = "chbx_DebtFromBAT";
            this.chbx_DebtFromBAT.Size = new System.Drawing.Size(208, 17);
            this.chbx_DebtFromBAT.TabIndex = 6;
            this.chbx_DebtFromBAT.Text = "Дебіторка КЕГ та ГРН з ПДВ з BAT";
            this.chbx_DebtFromBAT.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(6, 46);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(91, 20);
            this.dateTimePicker1.TabIndex = 7;
            this.dateTimePicker1.Value = new System.DateTime(2018, 11, 30, 0, 0, 0, 0);
            // 
            // cmbbx_dc_list
            // 
            this.cmbbx_dc_list.FormattingEnabled = true;
            this.cmbbx_dc_list.Location = new System.Drawing.Point(6, 19);
            this.cmbbx_dc_list.Name = "cmbbx_dc_list";
            this.cmbbx_dc_list.Size = new System.Drawing.Size(264, 21);
            this.cmbbx_dc_list.TabIndex = 8;
            // 
            // chbx_ArchivedStockstFromBAT
            // 
            this.chbx_ArchivedStockstFromBAT.AutoSize = true;
            this.chbx_ArchivedStockstFromBAT.Location = new System.Drawing.Point(0, 70);
            this.chbx_ArchivedStockstFromBAT.Name = "chbx_ArchivedStockstFromBAT";
            this.chbx_ArchivedStockstFromBAT.Size = new System.Drawing.Size(140, 17);
            this.chbx_ArchivedStockstFromBAT.TabIndex = 9;
            this.chbx_ArchivedStockstFromBAT.Text = "Архівні залишки з BAT";
            this.chbx_ArchivedStockstFromBAT.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbx_DataFromDC);
            this.groupBox1.Controls.Add(this.chbx_ArchivedStockstFromBAT);
            this.groupBox1.Controls.Add(this.chbx_SalesFromBAT);
            this.groupBox1.Controls.Add(this.chbx_DebtFromBAT);
            this.groupBox1.Location = new System.Drawing.Point(13, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(242, 100);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dateTimePicker1);
            this.groupBox2.Controls.Add(this.cmbbx_dc_list);
            this.groupBox2.Location = new System.Drawing.Point(261, 70);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(276, 100);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Export";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 182);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbx_FilePath);
            this.Controls.Add(this.btn_Select);
            this.Controls.Add(this.btn_Export);
            this.Controls.Add(this.btn_Import);
            this.Name = "MainForm";
            this.Text = "Data Comparator Tool";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Import;
        private System.Windows.Forms.Button btn_Export;
        private System.Windows.Forms.Button btn_Select;
        private System.Windows.Forms.TextBox tbx_FilePath;
        private System.Windows.Forms.CheckBox chbx_DataFromDC;
        private System.Windows.Forms.CheckBox chbx_SalesFromBAT;
        private System.Windows.Forms.CheckBox chbx_DebtFromBAT;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ComboBox cmbbx_dc_list;
        private System.Windows.Forms.CheckBox chbx_ArchivedStockstFromBAT;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

