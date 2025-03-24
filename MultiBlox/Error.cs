using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiBlox
{
    public partial class Error : Form
    {
        Point lastPoint = new Point();
        public Error(string message)
        {
            InitializeComponent();
            CheckAndDisableForms();
            ErrorMessage.Text = message;
        }


        public void DisableAllFormsExcept<T>() where T : Form
        {
            foreach (Form form in Application.OpenForms)
            {
                if (!(form is T))
                {
                    form.Enabled = false;
                }
            }
        }

        public void CheckAndDisableForms()
        {
            DisableAllFormsExcept<Error>();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                form.Enabled = true;
            }
            this.Close();
        }


        private void drag_mouse(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void drag_mouse_move(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }
    }
}
