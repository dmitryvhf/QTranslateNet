using System.Drawing;
using System.Windows.Forms;

namespace QTranslateNet
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            comboBoxFrom = new ComboBox();
            labelFrom = new Label();
            labelTo = new Label();
            comboBoxTo = new ComboBox();
            textBoxFrom = new TextBox();
            textBoxTo = new TextBox();
            btnTranslate = new Button();
            toolTip1 = new ToolTip(components);
            btnClear = new Button();
            btnSwapLang = new Button();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // comboBoxFrom
            // 
            comboBoxFrom.FormattingEnabled = true;
            comboBoxFrom.Location = new Point(86, 178);
            comboBoxFrom.Name = "comboBoxFrom";
            comboBoxFrom.Size = new Size(121, 23);
            comboBoxFrom.TabIndex = 0;
            // 
            // labelFrom
            // 
            labelFrom.AutoSize = true;
            labelFrom.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            labelFrom.Location = new Point(41, 182);
            labelFrom.Name = "labelFrom";
            labelFrom.Size = new Size(39, 15);
            labelFrom.TabIndex = 1;
            labelFrom.Text = "From:";
            // 
            // labelTo
            // 
            labelTo.AutoSize = true;
            labelTo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            labelTo.Location = new Point(241, 182);
            labelTo.Name = "labelTo";
            labelTo.Size = new Size(23, 15);
            labelTo.TabIndex = 3;
            labelTo.Text = "To:";
            // 
            // comboBoxTo
            // 
            comboBoxTo.FormattingEnabled = true;
            comboBoxTo.Location = new Point(270, 179);
            comboBoxTo.Name = "comboBoxTo";
            comboBoxTo.Size = new Size(121, 23);
            comboBoxTo.TabIndex = 2;
            // 
            // textBoxFrom
            // 
            textBoxFrom.Location = new Point(12, 12);
            textBoxFrom.Multiline = true;
            textBoxFrom.Name = "textBoxFrom";
            textBoxFrom.ScrollBars = ScrollBars.Vertical;
            textBoxFrom.Size = new Size(465, 160);
            textBoxFrom.TabIndex = 4;
            // 
            // textBoxTo
            // 
            textBoxTo.Location = new Point(12, 208);
            textBoxTo.Multiline = true;
            textBoxTo.Name = "textBoxTo";
            textBoxTo.ScrollBars = ScrollBars.Vertical;
            textBoxTo.Size = new Size(465, 160);
            textBoxTo.TabIndex = 5;
            // 
            // btnTranslate
            // 
            btnTranslate.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btnTranslate.Location = new Point(395, 179);
            btnTranslate.Name = "btnTranslate";
            btnTranslate.Size = new Size(82, 23);
            btnTranslate.TabIndex = 6;
            btnTranslate.Text = "Перевести";
            btnTranslate.UseVisualStyleBackColor = true;
            btnTranslate.Click += BtnTranslate_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(12, 178);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(23, 23);
            btnClear.TabIndex = 10;
            btnClear.Text = "✕";
            toolTip1.SetToolTip(btnClear, "Очистить текущий перевод");
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += BtnClear_Click;
            // 
            // btnSwapLang
            // 
            btnSwapLang.Location = new Point(213, 177);
            btnSwapLang.Name = "btnSwapLang";
            btnSwapLang.Size = new Size(23, 23);
            btnSwapLang.TabIndex = 11;
            btnSwapLang.Text = "⇄";
            toolTip1.SetToolTip(btnSwapLang, "Поменять местами языки перевода");
            btnSwapLang.UseVisualStyleBackColor = true;
            btnSwapLang.Click += BtnSwapLang_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 450);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(486, 22);
            statusStrip1.TabIndex = 9;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(0, 17);
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Location = new Point(12, 374);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(465, 68);
            flowLayoutPanel1.TabIndex = 12;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(486, 472);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(btnSwapLang);
            Controls.Add(btnClear);
            Controls.Add(statusStrip1);
            Controls.Add(btnTranslate);
            Controls.Add(textBoxTo);
            Controls.Add(textBoxFrom);
            Controls.Add(labelTo);
            Controls.Add(comboBoxTo);
            Controls.Add(labelFrom);
            Controls.Add(comboBoxFrom);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "QTranslate.NET";
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBoxFrom;
        private Label labelFrom;
        private Label labelTo;
        private ComboBox comboBoxTo;
        private TextBox textBoxFrom;
        private TextBox textBoxTo;
        private Button btnTranslate;
        private ToolTip toolTip1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Button btnClear;
        private Button btnSwapLang;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}
