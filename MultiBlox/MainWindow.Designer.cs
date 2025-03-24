using System.Text.Json;

namespace MultiBlox
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            panel1 = new Panel();
            label1 = new Label();
            button2 = new Button();
            button1 = new Button();
            AddAccountButton = new Button();
            accountsTable = new TableLayoutPanel();
            RobloxTokenInput = new TextBox();
            label3 = new Label();
            JoinGameButton = new Button();
            PlaceID = new TextBox();
            UserID = new TextBox();
            label2 = new Label();
            label4 = new Label();
            multiRoblox = new CheckBox();
            joinFriend = new CheckBox();
            ServerList = new ComboBox();
            pictureBox1 = new PictureBox();
            instanceClosing = new CheckBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(50, 50, 50);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(801, 62);
            panel1.TabIndex = 0;
            panel1.MouseDown += drag_mouse;
            panel1.MouseMove += drag_mouse_move;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(205, 9);
            label1.Name = "label1";
            label1.Size = new Size(440, 37);
            label1.TabIndex = 2;
            label1.Text = "MultiBlox Account Manager V1.1";
            label1.MouseDown += drag_mouse;
            label1.MouseMove += drag_mouse_move;
            // 
            // button2
            // 
            button2.Location = new Point(746, 3);
            button2.Name = "button2";
            button2.Size = new Size(23, 23);
            button2.TabIndex = 3;
            button2.Text = "-";
            button2.UseVisualStyleBackColor = true;
            button2.Click += minimize;
            // 
            // button1
            // 
            button1.Location = new Point(775, 3);
            button1.Name = "button1";
            button1.Size = new Size(23, 23);
            button1.TabIndex = 2;
            button1.Text = "X";
            button1.UseVisualStyleBackColor = true;
            button1.Click += close;
            // 
            // AddAccountButton
            // 
            AddAccountButton.Location = new Point(12, 404);
            AddAccountButton.Name = "AddAccountButton";
            AddAccountButton.Size = new Size(85, 23);
            AddAccountButton.TabIndex = 1;
            AddAccountButton.Text = "Add Account";
            AddAccountButton.UseVisualStyleBackColor = true;
            AddAccountButton.Click += addAccountButton;
            // 
            // accountsTable
            // 
            accountsTable.BackColor = Color.FromArgb(30, 30, 30);
            accountsTable.ColumnCount = 1;
            accountsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            accountsTable.Location = new Point(12, 68);
            accountsTable.Name = "accountsTable";
            accountsTable.Size = new Size(306, 318);
            accountsTable.TabIndex = 0;
            // 
            // RobloxTokenInput
            // 
            RobloxTokenInput.HideSelection = false;
            RobloxTokenInput.Location = new Point(112, 404);
            RobloxTokenInput.Name = "RobloxTokenInput";
            RobloxTokenInput.Size = new Size(100, 23);
            RobloxTokenInput.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.White;
            label3.Location = new Point(103, 389);
            label3.Name = "label3";
            label3.Size = new Size(117, 15);
            label3.TabIndex = 6;
            label3.Text = "Enter Roblox Cookie!";
            // 
            // JoinGameButton
            // 
            JoinGameButton.Location = new Point(335, 162);
            JoinGameButton.Name = "JoinGameButton";
            JoinGameButton.Size = new Size(75, 23);
            JoinGameButton.TabIndex = 7;
            JoinGameButton.Text = "Join Game!";
            JoinGameButton.UseVisualStyleBackColor = true;
            JoinGameButton.Click += JoinGameButton_Click;
            // 
            // PlaceID
            // 
            PlaceID.Location = new Point(335, 83);
            PlaceID.Name = "PlaceID";
            PlaceID.Size = new Size(100, 23);
            PlaceID.TabIndex = 8;
            // 
            // UserID
            // 
            UserID.Location = new Point(690, 83);
            UserID.Name = "UserID";
            UserID.Size = new Size(100, 23);
            UserID.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.ForeColor = Color.White;
            label2.Location = new Point(361, 65);
            label2.Name = "label2";
            label2.Size = new Size(49, 15);
            label2.TabIndex = 10;
            label2.Text = "Place ID";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.ForeColor = Color.White;
            label4.Location = new Point(728, 65);
            label4.Name = "label4";
            label4.Size = new Size(41, 15);
            label4.TabIndex = 11;
            label4.Text = "UserID";
            // 
            // multiRoblox
            // 
            multiRoblox.AutoSize = true;
            multiRoblox.ForeColor = Color.White;
            multiRoblox.Location = new Point(690, 112);
            multiRoblox.Name = "multiRoblox";
            multiRoblox.Size = new Size(96, 19);
            multiRoblox.TabIndex = 12;
            multiRoblox.Text = "Multi-Roblox";
            multiRoblox.UseVisualStyleBackColor = true;
            multiRoblox.CheckedChanged += multiRoblox_CheckedChanged;
            // 
            // joinFriend
            // 
            joinFriend.AutoSize = true;
            joinFriend.ForeColor = Color.White;
            joinFriend.Location = new Point(690, 135);
            joinFriend.Name = "joinFriend";
            joinFriend.Size = new Size(83, 19);
            joinFriend.TabIndex = 13;
            joinFriend.Text = "Join Friend";
            joinFriend.UseVisualStyleBackColor = true;
            // 
            // ServerList
            // 
            ServerList.BackColor = Color.FromArgb(45, 45, 45);
            ServerList.DropDownStyle = ComboBoxStyle.DropDownList;
            ServerList.FormattingEnabled = true;
            ServerList.Location = new Point(335, 133);
            ServerList.Name = "ServerList";
            ServerList.Size = new Size(241, 23);
            ServerList.TabIndex = 14;
            ServerList.SelectionChangeCommitted += choose_game_dropdown;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(351, 107);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(64, 22);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 15;
            pictureBox1.TabStop = false;
            // 
            // instanceClosing
            // 
            instanceClosing.ForeColor = Color.White;
            instanceClosing.Location = new Point(690, 160);
            instanceClosing.Name = "instanceClosing";
            instanceClosing.Size = new Size(104, 81);
            instanceClosing.TabIndex = 16;
            instanceClosing.Text = "Auto-Close Roblox instances on account launch";
            instanceClosing.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(800, 450);
            Controls.Add(instanceClosing);
            Controls.Add(pictureBox1);
            Controls.Add(ServerList);
            Controls.Add(joinFriend);
            Controls.Add(multiRoblox);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(UserID);
            Controls.Add(PlaceID);
            Controls.Add(JoinGameButton);
            Controls.Add(label3);
            Controls.Add(RobloxTokenInput);
            Controls.Add(accountsTable);
            Controls.Add(AddAccountButton);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "MainWindow";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MultiBlox";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion

        private Panel panel1;
        private Button AddAccountButton;
        private Button button1;
        private Button button2;
        private Label label1;
        private TableLayoutPanel accountsTable;
        private TextBox RobloxTokenInput;
        private Label label3;
        private Button JoinGameButton;
        private TextBox PlaceID;
        private TextBox UserID;
        private Label label2;
        private Label label4;
        private CheckBox multiRoblox;
        private CheckBox joinFriend;
        private ComboBox ServerList;
        private PictureBox pictureBox1;
        private CheckBox instanceClosing;
    }
}
