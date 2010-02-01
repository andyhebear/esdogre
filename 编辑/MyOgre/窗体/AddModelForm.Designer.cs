namespace MyOgre
{
    partial class AddModelForm
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
            this.name_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.remark_textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.picture_textBox = new System.Windows.Forms.TextBox();
            this.picture_button = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.vido_textBox = new System.Windows.Forms.TextBox();
            this.vido_button = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "名称:";
            // 
            // name_textBox
            // 
            this.name_textBox.Location = new System.Drawing.Point(85, 6);
            this.name_textBox.Name = "name_textBox";
            this.name_textBox.Size = new System.Drawing.Size(215, 21);
            this.name_textBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "介绍:";
            // 
            // remark_textBox
            // 
            this.remark_textBox.Location = new System.Drawing.Point(85, 35);
            this.remark_textBox.Multiline = true;
            this.remark_textBox.Name = "remark_textBox";
            this.remark_textBox.Size = new System.Drawing.Size(215, 95);
            this.remark_textBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "图片:";
            // 
            // picture_textBox
            // 
            this.picture_textBox.Location = new System.Drawing.Point(85, 136);
            this.picture_textBox.Name = "picture_textBox";
            this.picture_textBox.Size = new System.Drawing.Size(215, 21);
            this.picture_textBox.TabIndex = 5;
            this.picture_textBox.TextChanged += new System.EventHandler(this.picture_textBox_TextChanged);
            // 
            // picture_button
            // 
            this.picture_button.Location = new System.Drawing.Point(306, 136);
            this.picture_button.Name = "picture_button";
            this.picture_button.Size = new System.Drawing.Size(43, 23);
            this.picture_button.TabIndex = 6;
            this.picture_button.Text = "浏览";
            this.picture_button.UseVisualStyleBackColor = true;
            this.picture_button.Click += new System.EventHandler(this.picture_button_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "视频:";
            // 
            // vido_textBox
            // 
            this.vido_textBox.Location = new System.Drawing.Point(85, 163);
            this.vido_textBox.Name = "vido_textBox";
            this.vido_textBox.Size = new System.Drawing.Size(215, 21);
            this.vido_textBox.TabIndex = 8;
            this.vido_textBox.TextChanged += new System.EventHandler(this.vido_textBox_TextChanged);
            // 
            // vido_button
            // 
            this.vido_button.Location = new System.Drawing.Point(306, 162);
            this.vido_button.Name = "vido_button";
            this.vido_button.Size = new System.Drawing.Size(43, 23);
            this.vido_button.TabIndex = 6;
            this.vido_button.Text = "浏览";
            this.vido_button.UseVisualStyleBackColor = true;
            this.vido_button.Click += new System.EventHandler(this.vido_button_Click);
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button3.Location = new System.Drawing.Point(144, 202);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "确定";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button4.Location = new System.Drawing.Point(225, 202);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 10;
            this.button4.Text = "取消";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // AddModelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 240);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.vido_textBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.vido_button);
            this.Controls.Add(this.picture_button);
            this.Controls.Add(this.picture_textBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.remark_textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.name_textBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AddModelForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "模型属性";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button picture_button;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button vido_button;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        public System.Windows.Forms.TextBox name_textBox;
        public System.Windows.Forms.TextBox remark_textBox;
        public System.Windows.Forms.TextBox picture_textBox;
        public System.Windows.Forms.TextBox vido_textBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}