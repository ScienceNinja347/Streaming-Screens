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
    public partial class DialogBox : Form
    {
        #pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public bool Closed { get; set; } = true;
        #pragma warning restore CS0108 // Member hides inherited member; missing new keyword

        public List<string> LogLines { get { return listBoxLiveLogging.Items.Cast<string>().ToList(); } set { } }
        public DialogBox()
        {
            InitializeComponent();
        }

        delegate void delvoidstring(string s);
        public void AppendText(string text)
        {
            if(listBoxLiveLogging.InvokeRequired)
            {
                delvoidstring dvi = new delvoidstring(AppendText);

                try
                {
                    listBoxLiveLogging.Invoke(dvi, text);
                }
                catch (Exception) { }
                
                return;
            }
            listBoxLiveLogging.Items.Add(text);
        }

        private void DialogBox_Load(object sender, EventArgs e)
        {

        }
    }
}
