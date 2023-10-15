namespace Main
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
            pictureBox1 = new PictureBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            trackBar1 = new TrackBar();
            label1 = new Label();
            trackBar2 = new TrackBar();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar2).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1103, 683);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.Location = new Point(1138, 12);
            button1.Name = "button1";
            button1.Size = new Size(294, 62);
            button1.TabIndex = 1;
            button1.Text = "Открыть BMP-файл";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(1138, 80);
            button2.Name = "button2";
            button2.Size = new Size(294, 62);
            button2.TabIndex = 2;
            button2.Text = "Сохранить BMP-файл";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(1138, 633);
            button3.Name = "button3";
            button3.Size = new Size(294, 62);
            button3.TabIndex = 3;
            button3.Text = "Увеличить изображение";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(1138, 171);
            trackBar1.Maximum = 4;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(294, 56);
            trackBar1.TabIndex = 4;
            trackBar1.Value = 2;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1258, 207);
            label1.Name = "label1";
            label1.Size = new Size(54, 20);
            label1.TabIndex = 5;
            label1.Text = "Гамма";
            // 
            // trackBar2
            // 
            trackBar2.LargeChange = 51;
            trackBar2.Location = new Point(1138, 280);
            trackBar2.Maximum = 255;
            trackBar2.Name = "trackBar2";
            trackBar2.Size = new Size(294, 56);
            trackBar2.SmallChange = 51;
            trackBar2.TabIndex = 6;
            trackBar2.Value = 255;
            trackBar2.Scroll += trackBar2_Scroll;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(1249, 316);
            label2.Name = "label2";
            label2.Size = new Size(72, 20);
            label2.TabIndex = 7;
            label2.Text = "Контраст";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1444, 707);
            Controls.Add(label2);
            Controls.Add(trackBar2);
            Controls.Add(label1);
            Controls.Add(trackBar1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            KeyPreview = true;
            Name = "Form1";
            Text = "Form1";
            KeyDown += Form1_KeyDown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button button1;
        private Button button2;
        private Button button3;
        private TrackBar trackBar1;
        private Label label1;
        private TrackBar trackBar2;
        private Label label2;
    }
}