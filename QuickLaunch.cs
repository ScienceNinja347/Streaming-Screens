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
    public partial class QuickLaunch : Form
    {
        public QuickLaunch()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;

                textBoxAP.Text = file;
            }
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            // Check if there is a valid AP and Argument
            if (textBoxAP.Text.ToString() != "")
            {
                if (textBoxArgument.Text.ToString() != null)
                {
                    String commandLine = textBoxAP.Text.ToString();
                    String argument = textBoxArgument.Text.ToString();

                    //AddtoGDVIQL(commandLine, argument);
                }
                else
                {
                    String message = "No argument entered.";
                    String caption = "Error detected in Argument TextBox: ";

                    MessageBox.Show(message, caption);
                }
            }
            else
            {
                String message = "No Command Line entered.";
                String caption = "Error detected in Command Line TextBox: ";

                MessageBox.Show(message, caption);
            }
        }
        //private void AddtoGDVIQL(String commandLine, String argument)
        //{
        //    String LogMessage = "Entering \"private void AddtoGDVIQL(String commandLine, String argument)\"";
        //    LogFile(LogMessage, 10);

        //    GridDataViewItems newItem;

        //    newItem = new GridDataViewItems()
        //    {
        //        commandLine = textBoxAP.Text.ToString(),
        //        arguments = textBoxArgument.Text.ToString(),
        //        width = (int)nUDWidth.Value,
        //        height = (int)nUDHeight.Value,
        //        xLoc = (int)nUDxLoc.Value,
        //        yLoc = (int)nUDyLoc.Value,
        //        moveAfter = (int)nUDMoveAfter.Value,
        //        defaultSize = checkBoxDefaultSize.Checked,
        //        id = _GDVI.Count()
        //    };

        //    _GDVI.Add(newItem);
        //    AddtoGui(newItem, false);

        //    newItem.ProcessPointer = StartProcess(newItem);
        //    if (newItem.ProcessPointer != null)
        //        ResizeWindow(newItem);
        //    else
        //    {
        //        _GDVI.Remove(_GDVI[newItem.id]);
        //        dataGridView.Rows.RemoveAt(newItem.id);
        //        FixIDs();
        //    }

        //    textBoxAP.Text = "";
        //    textBoxArgument.Text = "";
        //    nUDWidth.Value = 0;
        //    nUDHeight.Value = 0;
        //    nUDMoveAfter.Value = 1;
        //    nUDxLoc.Value = 0;
        //    nUDyLoc.Value = 0;
        //    checkBoxDefaultSize.Checked = false;

        //    LogMessage = "Leaving  \"private void AddtoGDVIQL(String commandLine, String argument)\"";
        //    LogFile(LogMessage, 10);
        //}
    }
}
