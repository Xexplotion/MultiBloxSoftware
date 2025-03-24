namespace MultiBlox
{
    partial class Error
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Error));
            ErrorMessage = new Label();
            pictureBox1 = new PictureBox();
            button1 = new Button();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // ErrorMessage
            // 
            ErrorMessage.AutoSize = true;
            ErrorMessage.BackColor = Color.FromArgb(40, 40, 40);
            ErrorMessage.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            ErrorMessage.ForeColor = Color.White;
            ErrorMessage.Location = new Point(128, 81);
            ErrorMessage.MaximumSize = new Size(275, 160);
            ErrorMessage.Name = "ErrorMessage";
            ErrorMessage.Size = new Size(43, 19);
            ErrorMessage.TabIndex = 0;
            ErrorMessage.Text = "Error";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 58);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(100, 100);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.Location = new Point(12, 258);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 4;
            button1.Text = "OK";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.BackColor = Color.FromArgb(25, 25, 25);
            label1.Font = new Font("Segoe UI", 30F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(396, 54);
            label1.TabIndex = 5;
            label1.Text = "Error";
            label1.MouseDown += drag_mouse;
            label1.MouseMove += drag_mouse_move;
            // 
            // Error
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 40, 40);
            ClientSize = new Size(395, 293);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            Controls.Add(ErrorMessage);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Error";
            Padding = new Padding(10);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "A";
            TopMost = true;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label ErrorMessage;
        private PictureBox pictureBox1;
        private Button button1;
        private Label label1;
    }
}