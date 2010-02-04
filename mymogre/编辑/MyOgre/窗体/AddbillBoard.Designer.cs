namespace MyOgre
{
    partial class AddbillBoard
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.setfont_button = new System.Windows.Forms.Button();
            this.setcolor_button = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 9);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(318, 129);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // setfont_button
            // 
            this.setfont_button.Location = new System.Drawing.Point(12, 144);
            this.setfont_button.Name = "setfont_button";
            this.setfont_button.Size = new System.Drawing.Size(75, 23);
            this.setfont_button.TabIndex = 1;
            this.setfont_button.Text = "设置字体";
            this.setfont_button.UseVisualStyleBackColor = true;
            this.setfont_button.Click += new System.EventHandler(this.setfont_button_Click);
            // 
            // setcolor_button
            // 
            this.setcolor_button.Location = new System.Drawing.Point(93, 144);
            this.setcolor_button.Name = "setcolor_button";
            this.setcolor_button.Size = new System.Drawing.Size(75, 23);
            this.setcolor_button.TabIndex = 2;
            this.setcolor_button.Text = "设置颜色";
            this.setcolor_button.UseVisualStyleBackColor = true;
            this.setcolor_button.Click += new System.EventHandler(this.setcolor_button_Click);
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button3.Location = new System.Drawing.Point(174, 144);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "确定";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button4.Location = new System.Drawing.Point(255, 144);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "取消";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // AddbillBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 181);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.setcolor_button);
            this.Controls.Add(this.setfont_button);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AddbillBoard";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "增加注记";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button setfont_button;
        private System.Windows.Forms.Button setcolor_button;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}