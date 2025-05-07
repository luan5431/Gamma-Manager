namespace Gamma_Manager
{
    partial class Window
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window));
            this.trackBarGamma = new System.Windows.Forms.TrackBar();
            this.comboBoxPresets = new System.Windows.Forms.ComboBox();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.comboBoxMonitors = new System.Windows.Forms.ComboBox();
            this.trackBarBrightness = new System.Windows.Forms.TrackBar();
            this.textBoxGamma = new System.Windows.Forms.TextBox();
            this.textBoxBrightness = new System.Windows.Forms.TextBox();
            this.labelGamma = new System.Windows.Forms.Label();
            this.labelBrightness = new System.Windows.Forms.Label();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonHide = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.picLUTGraph = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGamma)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLUTGraph)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBarGamma
            // 
            this.trackBarGamma.LargeChange = 1;
            this.trackBarGamma.Location = new System.Drawing.Point(61, 4);
            this.trackBarGamma.Margin = new System.Windows.Forms.Padding(2);
            this.trackBarGamma.Maximum = 44;
            this.trackBarGamma.Minimum = 3;
            this.trackBarGamma.Name = "trackBarGamma";
            this.trackBarGamma.Size = new System.Drawing.Size(279, 45);
            this.trackBarGamma.TabIndex = 0;
            this.trackBarGamma.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarGamma.Value = 10;
            this.trackBarGamma.ValueChanged += new System.EventHandler(this.trackBarGamma_ValueChanged);
            // 
            // comboBoxPresets
            // 
            this.comboBoxPresets.FormattingEnabled = true;
            this.comboBoxPresets.Location = new System.Drawing.Point(14, 91);
            this.comboBoxPresets.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxPresets.Name = "comboBoxPresets";
            this.comboBoxPresets.Size = new System.Drawing.Size(180, 21);
            this.comboBoxPresets.TabIndex = 5;
            this.comboBoxPresets.SelectedIndexChanged += new System.EventHandler(this.comboBoxPresets_SelectedIndexChanged);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(282, 60);
            this.buttonReset.Margin = new System.Windows.Forms.Padding(2);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(103, 23);
            this.buttonReset.TabIndex = 22;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(215, 62);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(63, 23);
            this.buttonSave.TabIndex = 7;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // comboBoxMonitors
            // 
            this.comboBoxMonitors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMonitors.FormattingEnabled = true;
            this.comboBoxMonitors.Location = new System.Drawing.Point(14, 62);
            this.comboBoxMonitors.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxMonitors.Name = "comboBoxMonitors";
            this.comboBoxMonitors.Size = new System.Drawing.Size(180, 21);
            this.comboBoxMonitors.TabIndex = 8;
            this.comboBoxMonitors.SelectedIndexChanged += new System.EventHandler(this.comboBoxMonitors_SelectedIndexChanged);
            // 
            // trackBarBrightness
            // 
            this.trackBarBrightness.LargeChange = 1;
            this.trackBarBrightness.Location = new System.Drawing.Point(61, 31);
            this.trackBarBrightness.Margin = new System.Windows.Forms.Padding(2);
            this.trackBarBrightness.Minimum = -10;
            this.trackBarBrightness.Name = "trackBarBrightness";
            this.trackBarBrightness.Size = new System.Drawing.Size(279, 45);
            this.trackBarBrightness.TabIndex = 10;
            this.trackBarBrightness.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarBrightness.ValueChanged += new System.EventHandler(this.trackBarBrightness_ValueChanged);
            // 
            // textBoxGamma
            // 
            this.textBoxGamma.Location = new System.Drawing.Point(352, 4);
            this.textBoxGamma.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxGamma.Name = "textBoxGamma";
            this.textBoxGamma.ReadOnly = true;
            this.textBoxGamma.Size = new System.Drawing.Size(33, 20);
            this.textBoxGamma.TabIndex = 11;
            this.textBoxGamma.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxGamma.TextChanged += new System.EventHandler(this.textBoxGamma_TextChanged);
            // 
            // textBoxBrightness
            // 
            this.textBoxBrightness.Location = new System.Drawing.Point(352, 31);
            this.textBoxBrightness.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxBrightness.Name = "textBoxBrightness";
            this.textBoxBrightness.ReadOnly = true;
            this.textBoxBrightness.Size = new System.Drawing.Size(33, 20);
            this.textBoxBrightness.TabIndex = 13;
            this.textBoxBrightness.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelGamma
            // 
            this.labelGamma.AutoSize = true;
            this.labelGamma.Location = new System.Drawing.Point(12, 8);
            this.labelGamma.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelGamma.Name = "labelGamma";
            this.labelGamma.Size = new System.Drawing.Size(43, 13);
            this.labelGamma.TabIndex = 14;
            this.labelGamma.Text = "Gamma";
            // 
            // labelBrightness
            // 
            this.labelBrightness.AutoSize = true;
            this.labelBrightness.Location = new System.Drawing.Point(6, 33);
            this.labelBrightness.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelBrightness.Name = "labelBrightness";
            this.labelBrightness.Size = new System.Drawing.Size(56, 13);
            this.labelBrightness.TabIndex = 16;
            this.labelBrightness.Text = "Brightness";
            this.labelBrightness.Click += new System.EventHandler(this.labelBrightness_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(215, 89);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(63, 23);
            this.buttonDelete.TabIndex = 17;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonHide
            // 
            this.buttonHide.Location = new System.Drawing.Point(282, 89);
            this.buttonHide.Margin = new System.Windows.Forms.Padding(2);
            this.buttonHide.Name = "buttonHide";
            this.buttonHide.Size = new System.Drawing.Size(103, 23);
            this.buttonHide.TabIndex = 22;
            this.buttonHide.Text = "Hide";
            this.buttonHide.UseVisualStyleBackColor = true;
            this.buttonHide.Click += new System.EventHandler(this.buttonHide_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Gamma Manager";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // contextMenu
            // 
            this.contextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // picLUTGraph
            // 
            this.picLUTGraph.BackColor = System.Drawing.Color.White;
            this.picLUTGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLUTGraph.Location = new System.Drawing.Point(40, 130);
            this.picLUTGraph.Name = "picLUTGraph";
            this.picLUTGraph.Size = new System.Drawing.Size(300, 200);
            this.picLUTGraph.TabIndex = 0;
            this.picLUTGraph.TabStop = false;
            this.picLUTGraph.Click += new System.EventHandler(this.picLUTGraph_Click);
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(393, 342);
            this.Controls.Add(this.picLUTGraph);
            this.Controls.Add(this.buttonHide);
            this.Controls.Add(this.comboBoxMonitors);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.comboBoxPresets);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.labelBrightness);
            this.Controls.Add(this.labelGamma);
            this.Controls.Add(this.textBoxBrightness);
            this.Controls.Add(this.textBoxGamma);
            this.Controls.Add(this.trackBarBrightness);
            this.Controls.Add(this.trackBarGamma);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "Window";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Gamma Manager";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Window_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGamma)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLUTGraph)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBarGamma;
        private System.Windows.Forms.ComboBox comboBoxPresets;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ComboBox comboBoxMonitors;
        private System.Windows.Forms.TrackBar trackBarBrightness;
        private System.Windows.Forms.TextBox textBoxGamma;
        private System.Windows.Forms.TextBox textBoxBrightness;
        private System.Windows.Forms.Label labelGamma;
        private System.Windows.Forms.Label labelBrightness;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonHide;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.PictureBox picLUTGraph;
    }
}

