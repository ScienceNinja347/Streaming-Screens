using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DesktopSetupV2
{
    public partial class FormMessageBox : Form
    {
        public FormMessageBox()
        {
            InitializeComponent();
        }

        public void AddString(string message, string caption)
        {
            if (InvokeRequired)
                Invoke(new Action(() => AddString(message, caption)));
            else
            {
                labelMessage.Text += message;
                Text = caption;
                Show();
            }
        }

        private void FormMessageBox_Load(object sender, EventArgs e)
        {
            CenterToParent();
        }

        private void btnOk_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
