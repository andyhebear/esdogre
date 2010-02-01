namespace MyOgre
{
    partial class SetRoadStyleForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.road_button = new System.Windows.Forms.Button();
            this.line_button = new System.Windows.Forms.Button();
            this.road_textBox = new System.Windows.Forms.TextBox();
            this.line_textBox = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "路底色:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(173, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "斑马线:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "路宽:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(173, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "斑马线宽:";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(14, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(294, 100);
            this.panel1.TabIndex = 4;
            // 
            // road_button
            // 
            this.road_button.Location = new System.Drawing.Point(69, 4);
            this.road_button.Name = "road_button";
            this.road_button.Size = new System.Drawing.Size(75, 23);
            this.road_button.TabIndex = 5;
            this.road_button.UseVisualStyleBackColor = true;
            this.road_button.Click += new System.EventHandler(this.road_button_Click);
            // 
            // line_button
            // 
            this.line_button.Location = new System.Drawing.Point(233, 4);
            this.line_button.Name = "line_button";
            this.line_button.Size = new System.Drawing.Size(75, 23);
            this.line_button.TabIndex = 6;
            this.line_button.UseVisualStyleBackColor = true;
            this.line_button.Click += new System.EventHandler(this.line_button_Click);
            // 
            // road_textBox
            // 
            this.road_textBox.Location = new System.Drawing.Point(69, 33);
            this.road_textBox.Name = "road_textBox";
            this.road_textBox.Size = new System.Drawing.Size(75, 21);
            this.road_textBox.TabIndex = 7;
            this.road_textBox.Text = "0";
            this.road_textBox.TextChanged += new System.EventHandler(this.road_textBox_TextChanged);
            // 
            // line_textBox
            // 
            this.line_textBox.Location = new System.Drawing.Point(233, 35);
            this.line_textBox.Name = "line_textBox";
            this.line_textBox.Size = new System.Drawing.Size(75, 21);
            this.line_textBox.TabIndex = 8;
            this.line_textBox.Text = "0";
            this.line_textBox.TextChanged += new System.EventHandler(this.line_textBox_TextChanged);
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button3.Location = new System.Drawing.Point(139, 168);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "确定";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button4.Location = new System.Drawing.Point(230, 168);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 10;
            this.button4.Text = "取消";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 168);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "复默认值";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SetRoadStyleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 206);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.line_textBox);
            this.Controls.Add(this.road_textBox);
            this.Controls.Add(this.line_button);
            this.Controls.Add(this.road_button);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SetRoadStyleForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置路样式";
            this.Load += new System.EventHandler(this.SetRoadStyleForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SetRoadStyleForm_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        public System.Windows.Forms.Button road_button;
        public System.Windows.Forms.Button line_button;
        public System.Windows.Forms.TextBox road_textBox;
        public System.Windows.Forms.TextBox line_textBox;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button1;
    }
}