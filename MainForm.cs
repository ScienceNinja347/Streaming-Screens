using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DesktopSetupV2
{
    public partial class MainForm : Form
    {

        private List<GridDataViewItems> _GDVI = new List<GridDataViewItems>(); // GridDataViewList
        private List<string> IPs = new List<string> { };
        private Dictionary<string, Thread> _threads = new Dictionary<string, Thread>();
        private String[] IniLines;

        private int DefaultPort = 0, DefaultWidth = 0, DefaultHeight = 0, DefaultMoveAfter = 1, DefaultXLoc = 0, DefaultYLoc = 0;
        private String DefaultCommandLine = "", DefaultArguments = "", DefaultIp = "", DefaultDescription = "", DefaultRemoteFolder = "";
        private new bool DefaultSize = false;

        //private const int _port = 5900;

        private String LogFilePath = @"Log Files\" + DateTime.Now.ToString("yyyy-MM-dd -- HH-mm-ss") + ".log";
        private String OpenedFile;
        private String IniFile;
        private DialogBox LiveLog;

        private bool doneStartProcess = false;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int Width, int Height, bool Repaint);

        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, out RECT rect);

        //-------------------------------

        public MainForm()
        {
            InitializeComponent();

            OpenLiveLogging();

            AddTemplates();

            Log("Entering Form1()", LoggingLevel.mid);

            String[] args = Environment.GetCommandLineArgs();

            if (args.Count() == 2)
            {
                if (args[1] != null && !args[1].Trim(' ').Equals(""))
                {
                    CSVReadWithIni(args[1]);
                    ThreadManager();
                    OptimizeColumns();
                    OptimizeRows();
                }
            }
            else if (args.Count() > 2)
            {
                String caption = "Error on Initialize.";
                String message = "Error on initialize, too many arguments. First two arguments are used ignored arguments as follows:\n";
                for (int i = 2; i < args.Count(); i++)
                {
                    message += args[i] + "\n";
                }
                message += "\nTo use DesktopSetupV2 correctly run as: \"DesktopSetupV2 FileName\"";

                MessageBox(message, caption);
            }
            Log("Leaving  \"Form1()\"", LoggingLevel.mid);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //dataGridView.Rows[0].Cells["ClId"].Value = 1;
        }


        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            Log("Entering Closing Form", LoggingLevel.mid);

            String LogMessage = "Entering \"private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)\"";
            Log(LogMessage, LoggingLevel.mid);

            foreach (GridDataViewItems item in _GDVI)
                if (item.ProcessPointer != null && !item.ProcessPointer.HasExited)
                    item.ProcessPointer.CloseMainWindow();

            LiveLog.Close();

            LogMessage = "Leaving  \"private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)\"";
            Log(LogMessage, LoggingLevel.mid);

            Log("Leaving  Closing Form", LoggingLevel.mid);
        }


        private void synchronizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("Entering \"synchronizeToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
            Sync();
            Log("Leaving  \"synchronizeToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);            
        }


        private void launchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("Entering \"private void launchToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);

            if (dataGridView.SelectedRows.Count > 0) //There is a selected record
            {
                try
                {
                    int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["CLId"].Value) - 1;

                    GridDataViewItems currItem = _GDVI[id];

                    if (currItem.ProcessPointer == null || currItem.ProcessPointer.HasExited)
                    {
                        currItem.ProcessPointer = StartProcess(currItem);

                        while (!doneStartProcess)
                            Thread.Sleep(100);
                        doneStartProcess = false;
                        ThreadManager();
                        //ResizeWindow(currItem);
                    }
                }
                catch (Exception)
                {
                    String message = "Add record to Launch window";
                    String caption = "Record Not Added";

                    MessageBox(message, caption);
                }
            }
            else //No selected record
            {
                String message = "Select row to Launch.";
                String caption = "No Row Selected";

                MessageBox(message, caption);
            }
            Log("Leavinng  \"private void launchToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("Entering \"private void saveToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
            CSVWrite(OpenedFile);
            Log("Leaving  \"private void saveToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("Entering \"private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
            CSVWrite(null);
            Log("Leaving  \"private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        /// <summary>
        /// Opens, reads, and restores the windows from a file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("Entering \"openToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
            ClearAll();
            CSVReadWithIni(null);
            ThreadManager();
            OptimizeColumns();
            OptimizeRows();
            Log("Leaving  \"openToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("Entering \"deleteToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
            DeleteRow();           
            Log("Leaving  \"deleteToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
        }

        
        /// <summary>
        /// Key down event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            Log("Entering \"dataGridView_KeyDown(object sender, KeyEventArgs e)\"", LoggingLevel.mid);
            if (e.KeyCode == Keys.Delete)
            {
                DeleteRow();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                SaveRecord();
                ThreadManager();
            }
            else if(e.Modifiers == Keys.Control && e.KeyCode == Keys.Home)
            {
                Process.Start(Directory.GetCurrentDirectory());
            }
            Log("Leaving  \"dataGridView_KeyDown(object sender, KeyEventArgs e)\"", LoggingLevel.mid);
        }


        private void addRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("Entering \"\"", LoggingLevel.mid);
            SaveRecord();
            ThreadManager();
            OptimizeRows();
            Log("Leaving  ", LoggingLevel.mid);
        }


        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("Entering \"restoreToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
            SaveRecord();
            ThreadManager();
            RestoreWindows();
            Log("Leaving  \"restoreToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        private void addRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("Entering \"addRowToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
            AddRow();
            OptimizeRows();
            Log("Leaving  \"addRowToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        private void btnLiveLogging_Click(object sender, EventArgs e)
        {
            Log("Entering \"private void btnLiveLogging_Click(object sender, EventArgs e)\"", LoggingLevel.mid);

            if (LiveLog != null && LiveLog.IsDisposed)
                LiveLog = null;

            OpenLiveLogging();

            LiveLog.Show();

            Log("Leaving  \"private void btnLiveLogging_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        private void myProcess_Exited(object sender, System.EventArgs e)
        {
            Log("Entering \"myProcess_Exited(object sender, System.EventArgs e)\"", LoggingLevel.mid);

            String LogMessage = "Entering \"private void myProcess_Exited(object sender, System.EventArgs e)\"";
            Log(LogMessage, LoggingLevel.mid);

            Log("myProcess_Exited Sleep(2000)", LoggingLevel.mid);
            Thread.Sleep(2000);

            LogMessage = "Leaving  \"private void myProcess_Exited(object sender, System.EventArgs e)\"";
            Log(LogMessage, LoggingLevel.mid);

            Log("Leaving  \"myProcess_Exited(object sender, System.EventArgs e)\"", LoggingLevel.mid);
        }


        private void buttonCreateFile_Click(object sender, EventArgs e)
        {
            Log("Entering \"buttonCreateFile_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
            CreateCommandFile();
            Log("Leaving  \"buttonCreateFile_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("Entering \"startToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
            StartThread();
            Log("Leaving  \"startToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("Entering \"stopToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
            StopThread();
            Log("Leaving  \"stopToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        private void optimizeColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("Entering \"optimizeColumnsToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
            OptimizeColumns();
            Log("Leaving  \"optimizeColumnsToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        private void makeColumnsVisibleToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Log("Entering \"makeColumnsVisibleToolStripMenuItem_Click_1(object sender, EventArgs e)\"", LoggingLevel.mid);
            MakeVisible();
            Log("Leaving  \"makeColumnsVisibleToolStripMenuItem_Click_1(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        private void makeColumnsVisibleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Log("Entering \"makeColumnsVisibleToolStripMenuItem1_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
            MakeVisible();
            Log("Leaving  \"makeColumnsVisibleToolStripMenuItem1_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        private void addRowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Log("Entering \"private void addRowToolStripMenuItem1_Click(object sender, EventArgs e)\" <--", LoggingLevel.mid);
            AddRow();
            OptimizeRows();
            Log("Leaving  \"private void addRowToolStripMenuItem1_Click(object sender, EventArgs e)\" <--", LoggingLevel.mid);
        }


        private void startToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Log("Entering \"startToolStripMenuItem1_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
            StartThread();
            Log("Leaving  \"startToolStripMenuItem1_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        private void stopToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Log("Entering \"stopToolStripMenuItem1_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
            StopThread();
            Log("Leaving  \"stopToolStripMenuItem1_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        private void removeColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("Entering \"removeColumnsToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
            OptimizeColumns();
            Log("Leaving  \"removeColumnsToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        private void ButtonRestore_Click(object sender, EventArgs e)
        {
            Log("Entering \"private void ButtonRestore_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
            SaveRecord();
            ThreadManager();
            RestoreWindows();
            Log("Leaving  \"private void ButtonRestore_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        private void listBoxTemplates_DoubleClick(object sender, EventArgs e)
        {
            Log("Entering \"private void listBoxTemplates_DoubleClick(object sender, EventArgs e)\"", LoggingLevel.mid);
            
            if (listBoxTemplates.SelectedItems.Count > 0)
            {
                String file = "Templates\\" + listBoxTemplates.SelectedItem.ToString();
                Process.Start("notepad.exe", file);
            }
            Log("Leaving  \"private void listBoxTemplates_DoubleClick(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        private void btnOpenCommandFolder_Click(object sender, EventArgs e)
        {
            Log("Entering \"private void btnOpenCommandFolder_Click(object sender, EventArgs e)\"", LoggingLevel.mid);

            if (!String.IsNullOrEmpty(DefaultRemoteFolder))
            {
                if (Directory.Exists(DefaultRemoteFolder))
                    Process.Start(DefaultRemoteFolder);
                else
                    MessageBox(DefaultRemoteFolder + " directory does not exist.", "Error on Opening Command Folder");
            }
            else
                MessageBox("Problem: The Command Folder Directory does not exist.\n" +
                           "\nTo fix this problem either: Add a record to your .ini file called: " +
                           "\"Remote folder\" followed by \"=\" on the same line, followed by your desired file path to store command files on the same line" +
                           ". To ensure your desired path is correct, try opening the folder through your Windows File Explorer." +
                           "\n\nOr: Open a readable data file which will use your corresponding .ini file.\n\n\n"
                           , "Error on Opening Command Folder"); // Caption

            Log("Leaving  \"private void btnOpenCommandFolder_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        private void killCommanderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log("Entering \"private void killCommanderToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);

            List<DataGridViewRow> SelectedRows = new List<DataGridViewRow>();

            //Adds selected dataGridViewRows to the SelectedRows SelectedRows list
            foreach (DataGridViewRow row in dataGridView.SelectedRows)
            {
                if (!SelectedRows.Contains(row) && !String.IsNullOrEmpty(row.Cells["ClIp"].Value as string))
                    SelectedRows.Add(row);
            }

            //Adds the selected dataGridViewRows that contain selected dataGridViewCells to the SelectedRows list
            foreach (DataGridViewCell Cell in dataGridView.SelectedCells)
            {
                if (!SelectedRows.Contains(Cell.OwningRow) && !String.IsNullOrEmpty(Cell.OwningRow.Cells["ClIp"].Value as string))
                    SelectedRows.Add(Cell.OwningRow);
            }

            if (SelectedRows.Count == 0)
                MessageBox("Select rows or cells to use \"Kill Commander\".", "\"Kill Commander\" Error: No Rows or Cells Selected");

            int SuccessCount = 0, FailureCount = 0;
            String SuccessComputers = "", FailureComputers = "";
            foreach (DataGridViewRow CurrentRow in SelectedRows)
            {
                String FileToDelete = "\\\\" + _GDVI[CurrentRow.Index].ip + "\\c$\\Program Files\\Commander\\Commander.ini";

                if (File.Exists(FileToDelete))
                {
                    File.Delete(FileToDelete);
                }

                if (CurrentRow.Index < _GDVI.Count)
                {
                    try
                    {
                        Process.Start("taskkill", "/s " + _GDVI[CurrentRow.Index].ip + " /f /im \"Commander Service.exe\"");
                        SuccessCount++;
                        SuccessComputers += _GDVI[CurrentRow.Index].ip + "\n";
                    }
                    catch (Exception)
                    {
                        FailureCount++;
                        FailureComputers += _GDVI[CurrentRow.Index].ip + "\n";
                    }                   
                }
            }
            SuccessComputers += "\n\n";
            FailureComputers += "\n\n";

            if (SuccessCount > 0 || FailureCount > 0)
            {
                if (SuccessCount == SelectedRows.Count)
                {
                    MessageBox("Desktop Setup V2 successfully ended the Command Service " + SuccessCount + " time(s), on computers:\n" + SuccessComputers, "Desktop Setup V2 - Kill Commander");
                }
                else
                {
                    if (SuccessCount > 0)
                    {
                        if (FailureCount > 0)
                        {
                            MessageBox("Desktop Setup V2 successfully ended the Command Service " + SuccessCount + " time(s), on computers:\n" + SuccessComputers +
                                       "\nAnd failed " + FailureCount + " time(s) on computers:\n" + FailureComputers, "Desktop Setup V2 - Kill Commander");
                        }
                        else
                        {
                            MessageBox("Desktop Setup V2 successfully ended the Command Service " + SuccessCount + " time(s), on computers:\n" + SuccessComputers, "Desktop Setup V2 - Kill Commander");
                        }
                    }
                    else
                    {
                        if (FailureCount > 0)
                        {
                            MessageBox("Desktop Setup V2 failed to end the Command Service " + FailureCount + " time(s), on computers:\n" + FailureComputers, "Desktop Setup V2 - Kill Commander");
                        }
                    }
                }
            }
            
            Log("Leaving  \"private void killCommanderToolStripMenuItem_Click(object sender, EventArgs e)\"", LoggingLevel.mid);
        }


        public enum LoggingLevel
        {
            low = 0,
            mid = 10,
            high = 20
        }


        //-------------------------------------------------------------
        //-------------------------------------------------------------


        /// <summary>
        /// Opens custom message box in the center of the parent form
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        private void MessageBox(string message, string caption)
        {
            //Initialises custom message box form called formMessageBox
            using (FormMessageBox formMessageBox = new FormMessageBox())
            {
                formMessageBox.AddString(message, caption);
                formMessageBox.Visible = false;
                formMessageBox.ShowDialog();
            }
        }


        /// <summary>
        /// Writes log messages to log file
        /// </summary>
        /// <param name="message">Message to write to log file</param>
        /// <param name="LogLevel">Log level: 0, 10, 20</param>
        private object Log_Lock = new object();
        /// <summary>
        /// Writes strings to the log file and the log form 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="LogLevel"></param>
        public void Log(String message, LoggingLevel LogLevel)
        {
            lock (Log_Lock)
            {
                //Gets the logging level from the user
                LoggingLevel LoggingLevel = LoggingLevel.low;
                
                if (rBNoLog.Checked)
                    LoggingLevel = LoggingLevel.low;
                
                else if (rBMain.Checked)
                    LoggingLevel = LoggingLevel.mid;
                
                else if (rBAll.Checked)
                    LoggingLevel = LoggingLevel.high;
                
                try
                {
                    //Checks if the Log Files folder exists
                    if (!Directory.Exists(@"Log Files\"))
                    {
                        Directory.CreateDirectory(@"Log Files\");
                    }

                    //Writes to log file and LiveLogging form
                    using (FileStream stream = new FileStream(LogFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (StreamWriter file = new StreamWriter(stream))
                        {
                            if (LogLevel <= LoggingLevel)
                            {
                                //Time stamps the Log
                                file.Write(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                                file.WriteLine("\t" + Thread.CurrentThread.ManagedThreadId + "\t" + message);

                                //Adds the same log to the Live Log Form
                                LiveLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "      " + Thread.CurrentThread.ManagedThreadId + "      " + message);
                            }
                        }
                    }
                }
                catch (Exception ErrMessage)
                {
                    MessageBox(ErrMessage.Message, "Error Opening File");
                }
            }
        }


        /// <summary>
        /// Adds items from records to main datGridViewList
        /// </summary>
        private void SaveRecord()
        {
            Log("Entering \"private void SaveRecord()\"", LoggingLevel.mid);
            Log("Entering \"private void SaveRecord()\" for loop", LoggingLevel.high);

            if(!AssignDefaults())
            {
                ClearAll();
                return;
            }

            //Iterates through dataGrid Rows and saves changed data
            for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
            {
                //Checks non visible column CLAdded
                //Checks if the row's contents have not been added to the main GridDataView List
                if (dataGridView.Rows[i].Cells["ClAdded"].FormattedValue is bool cell_checked && !cell_checked)
                {                   
                    GridDataViewItems newItem = new GridDataViewItems();
                    UpdateGridDataViewItem(newItem, dataGridView.Rows[i]);
                    
                    //Set "added" checkbox to true
                    dataGridView.Rows[i].Cells["ClAdded"].Value = true;
                    _GDVI.Add(newItem);
                }
                else
                {
                    UpdateGridDataViewItem(_GDVI[i], dataGridView.Rows[i]);
                }
            }
            Log("Leaving  \"private void SaveRecord()\"", LoggingLevel.mid);
        }


        /// <summary>
        /// Updates a GridDataViewItems with the contents of the DataGridViewRow
        /// </summary>
        /// <param name="CurrentComputer"></param>
        /// <param name="row"></param>
        private void UpdateGridDataViewItem(GridDataViewItems CurrentComputer, DataGridViewRow row)
        {
            //Updates current item with contents of the dataGridView
            int number;

            if (row.Cells["ClDescription"].Value != null)
            {
                CurrentComputer.description = row.Cells["ClDescription"].Value.ToString();

                if(CurrentComputer.ProcessPointer.MainWindowHandle != IntPtr.Zero)
                    RenameWindow(CurrentComputer);
            }

            String IpCompare = row.Cells["ClIp"].Value.ToString().Trim(' ');
            if (!String.IsNullOrEmpty(IpCompare))
            {
                if (int.TryParse(IpCompare, out _))
                    CurrentComputer.ip = DefaultIp.Replace("###", IpCompare);
                else
                    CurrentComputer.ip = IpCompare;
            }
            else
            {
                //Needs IP Address.
                MessageBox("Insert IP Address on line: " + row.Index, "Missing Field on line: " + row.Index);
                return;
            }

            if (!String.IsNullOrEmpty(row.Cells["ClPort"].Value as string))
                CurrentComputer.port = Convert.ToInt32(row.Cells["ClPort"].Value.ToString().Trim(' '));

            else
            {
                //Uses DefaultPort if cell is null or empty
                CurrentComputer.port = DefaultPort;
            }

            if (!String.IsNullOrEmpty(row.Cells["ClCommandLine"].Value as string))
                CurrentComputer.commandLine = row.Cells["ClCommandLine"].Value.ToString().Trim(' ');

            else
            {
                //Uses DefaultCommandLine if cell is null or empty
                CurrentComputer.commandLine = DefaultCommandLine;
            }

            //if the arguments cell is null, empty, or equal to the ip, we use the default arguments as our arguments
            if (String.IsNullOrEmpty(row.Cells["ClArguments"].Value as string) ||
                row.Cells["ClArguments"].Value.ToString().Equals(CurrentComputer.ip.ToString()))
            {
                if (!String.IsNullOrEmpty(CurrentComputer.ip))
                {
                    string[] SplitIp = CurrentComputer.ip.Split('.');
                    string StringIp = SplitIp[SplitIp.Length - 1];

                    if (int.TryParse(StringIp, out int IntIp))
                    {
                        //Formats IP so it can find the correct file specific for the IP
                        if (IntIp < 1000)
                        {
                            if (IntIp < 10)
                            {
                                StringIp = "00" + StringIp;
                            }
                            else if (IntIp < 100)
                            {
                                StringIp = "0" + StringIp;
                            }

                        }
                    }
                    CurrentComputer.arguments = DefaultArguments.Replace("###", StringIp);
                    CurrentComputer.argumentsIni = true;
                }
            }
            else
                //Uses argument from the dataGridView
                CurrentComputer.arguments = row.Cells["ClArguments"].Value.ToString();


            if (int.TryParse(row.Cells["ClMoveAfter"].Value as string, out int result))
                CurrentComputer.moveAfter = result;
            else
            {
                //Uses DefaultMoveAfter if cell is null or empty
                CurrentComputer.moveAfter = DefaultMoveAfter;
            }

            String xLocationCompare = row.Cells["ClXLoc"].Value.ToString().Trim(' ');
            String yLocationCompare = row.Cells["ClYLoc"].Value.ToString().Trim(' ');

            if (!String.IsNullOrEmpty(xLocationCompare) && int.TryParse(xLocationCompare, out number))
                CurrentComputer.xLoc = number;
            else
                //Uses DefaultXLoc if cell is null or empty
                CurrentComputer.xLoc = DefaultXLoc;

            if (!String.IsNullOrEmpty(yLocationCompare) && int.TryParse(yLocationCompare, out number))
                CurrentComputer.yLoc = number;
            else
                //Uses DefaultYLoc if cell is null or empty
                CurrentComputer.yLoc = DefaultYLoc;

            if (row.Cells["ClDefaultSize"].FormattedValue is bool cell_checked2 && !cell_checked2)
            {
                if (int.TryParse(row.Cells["ClWidth"].Value as string, out number))
                    CurrentComputer.width = number;
                else
                {
                    //Uses DefaultWidth if cell is null or empty
                    CurrentComputer.width = DefaultWidth;
                }

                if (int.TryParse(row.Cells["ClHeight"].Value as string, out number))
                    CurrentComputer.height = number;
                else
                {
                    //Uses DefaultHeight if cell is null or empty
                    CurrentComputer.height = DefaultHeight;
                }
            }
        }


        /// <summary>
        /// Gets all process windows' size and location, updates the item in the main GridDataViewItems list
        /// </summary>
        private void Sync()
        {
            Log("Entering \"private void Sync()\"", LoggingLevel.mid);

            if (_GDVI.Count == 0)
                return;

            Log("Entering \"private void Sync()\" foreach loop", LoggingLevel.high);

            //For each GridDataViewItem, grabs current GridDataViewItems windows' size and location
            //and updates the corresponding GridDataViewItem
            foreach (GridDataViewItems CurrentComputer in _GDVI)
            {
                if (CurrentComputer.ProcessPointer != null && !CurrentComputer.ProcessPointer.HasExited)
                {
                    IntPtr p = (IntPtr)CurrentComputer.ProcessPointer.MainWindowHandle;

                    //if the processpointer's main window is found
                    if (GetWindowRect(p, out RECT ProcessRec) == 1)
                    {
                        //Gets the windows current width, height, x location, and y location
                        CurrentComputer.width = Math.Abs(ProcessRec.Right - ProcessRec.Left);
                        CurrentComputer.height = Math.Abs(ProcessRec.Bottom - ProcessRec.Top);

                        CurrentComputer.xLoc = Math.Abs(ProcessRec.Left);
                        CurrentComputer.yLoc = Math.Abs(ProcessRec.Top);

                        //Updates the GUI with the current computers new values
                        AddtoGui(CurrentComputer);
                    }
                }
            }
            FixIDs();

            Log("Leaving  \"private void Sync()\"", LoggingLevel.mid);
        }


        private readonly object StartProcess_lock = new object();
        /// <summary>
        /// Starts the process of a GridDataViewItem
        /// </summary>
        /// <param name="currItem"></param>
        private Process StartProcess(GridDataViewItems CurrentComputer)
        {
            lock (StartProcess_lock)
            {
                Log("Entering \"private Process StartProcess(GridDataViewItems currItem)\"", LoggingLevel.mid);

                Process ProcessCreated = new Process();

                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

                try
                {
                    //Gets and sets file name and arguments from the CurrentComputer
                    startInfo.FileName = CurrentComputer.commandLine;
                    startInfo.Arguments = CurrentComputer.arguments;

                    ProcessCreated.StartInfo = startInfo;

                    ProcessCreated.Exited += myProcess_Exited;

                    ProcessCreated.EnableRaisingEvents = true;

                    ProcessCreated.Start();

                    //Sleep is nessessary so that the process is not lost
                    Thread.Sleep(CurrentComputer.moveAfter * 1000);
                }
                catch (Exception e)
                {
                    String message = e.Message;
                    String caption = "Finding File Error:";

                    ProcessCreated = null;

                    Log("Exception caught \"private Process StartProcess(GridDataViewItems currItem)\" --> " + e.Message, LoggingLevel.mid);

                    MessageBox(message, caption);
                }
                Log("Leaving  \"private Process StartProcess(GridDataViewItems currItem)\"", LoggingLevel.mid);

                doneStartProcess = true;

                return ProcessCreated;
            }
        }


        /// <summary>
        /// Manages the creation of threads
        /// </summary>       
        private void ThreadManager()
        {
            Log("Entering \"private void ThreadManager()\"", LoggingLevel.mid);
            Log("Entering \"private void ThreadManager()\" foreach loop", LoggingLevel.high);

            ///For each GridDataViewItem, checks if 
            ///the GridDataViewItem is in _threads, if
            ///not it adds the GridDataViewItem and
            ///starts the thread
            foreach (var currComputer in _GDVI)
            {
                AddThread(currComputer);
            }
            Log("Leaving  \"private void ThreadManager()\" foreach loop", LoggingLevel.high);
            Log("Leaving  \"private void ThreadManager()\"", LoggingLevel.mid);
        }


        /// <summary>
        /// Adds a single GridDataViewItem to the threads' list
        /// </summary>
        /// <param name="currComputer"></param>
        private void AddThread(GridDataViewItems currComputer)
        {
            Log("Entering \"private void AddThread(GridDataViewItems currComputer)\" Current Computer --> " + currComputer.ip, LoggingLevel.mid);
            //If the current computer does not have an Ip address set
            if (String.IsNullOrEmpty(currComputer.ip))
                return;

            //If the current computer is not in the main _threads list
            if (!_threads.ContainsKey(currComputer.ip))
            {
                Thread thread = new Thread(() =>
                {
                    PortPing(currComputer);
                });

                thread.IsBackground = true;

                _threads.Add(currComputer.ip, thread); // Adding to global collection of threads
                _threads[currComputer.ip].Start();
            }
            //If the current computer is in the main _threads list
            //If the thread is not alive, start it back up
            else if (!_threads[currComputer.ip].IsAlive)         
            {
                _threads[currComputer.ip] = 
                new Thread(() =>
                {
                    PortPing(currComputer);
                })
                {
                    IsBackground = true
                };

                _threads[currComputer.ip].Start();
            }
            Log("Leaving  \"private void AddThread(GridDataViewItems currComputer)\"", LoggingLevel.mid);
        }


        /// <summary>
        /// Creates, starts, and adds all selected dataGridViewRows and dataGridViewCells to the main _threads
        /// list if not already present
        /// </summary>
        private void StartThread()
        {
            Log("Entering \"private void StartThread()\"", LoggingLevel.mid);

            //Parses through all selected rows
            foreach (DataGridViewRow row in dataGridView.SelectedRows)
            {
                if(row.Index < _GDVI.Count)
                {
                    AddThread(_GDVI[row.Index]);
                }                 
            }           

            //Parses through all selected cells
            foreach (DataGridViewCell cell in dataGridView.SelectedCells)
            {
                if (cell.RowIndex < _GDVI.Count)
                {
                    AddThread(_GDVI[cell.RowIndex]);
                }
            }          

            Log("Leaving  \"private void StartThread()\"", LoggingLevel.mid);
        }


        /// <summary>
        /// Stops all selected threads
        /// </summary>
        private void StopThread()
        {
            Log("Entering \"private void StopThread()\"", LoggingLevel.mid);

            GridDataViewItems currThread;

            //Parses through all selected rows
            foreach (DataGridViewRow row in dataGridView.SelectedRows)
            {
                if (row.Index < _GDVI.Count)
                {
                    currThread = _GDVI[row.Index];
                    if (_threads[currThread.ip].IsAlive)
                    {
                        _threads[currThread.ip].Abort();
                        row.Cells["ClElapsed"].Value = "STOPPED";
                    }
                }
            }
            
            //Parses through all selected cells
            foreach (DataGridViewCell cell in dataGridView.SelectedCells)
            {
                if (cell.RowIndex < _GDVI.Count)
                {
                    currThread = _GDVI[cell.RowIndex];
                    if (_threads[currThread.ip].IsAlive)
                    {
                        _threads[currThread.ip].Abort();
                        dataGridView.Rows[cell.RowIndex].Cells["ClElapsed"].Value = "STOPPED";
                    }
                }
            }
            
            Log("Leaving  \"private void StopThread()\"", LoggingLevel.mid);
        }


        /// <summary>
        /// Makes the size of columns 0 when all the columns in the dataGridView are empty
        /// </summary>
        private void OptimizeColumns()
        {
            Log("Entering \"private void OptimizeColumns()\"", LoggingLevel.mid);

            //Sets variables to keep track of, if an entire column is empty
            bool NoDescription = true,
                 NoCommand = true,
                 NoArg = true,
                 NoIp = true,
                 NoPort = true,
                 NoWidth = true,
                 NoHeight = true,
                 NoMoveAfter = true,
                 NoDefaultSize = true;

            //Parses through all the rows to search for empty columns
            //if a column's cell is not empty it sets the corresponding value to true.
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Index < dataGridView.Rows.Count - 1)
                {
                    if (!String.IsNullOrEmpty(row.Cells["ClDescription"].Value as string))
                    {
                        NoDescription = false;
                    }

                    if (!String.IsNullOrEmpty(row.Cells["ClCommandLine"].Value as string))
                    {
                        NoCommand = false;
                    }

                    if (!String.IsNullOrEmpty(row.Cells["ClArguments"].Value as string))
                    {
                        NoArg = false;
                    }

                    if (!String.IsNullOrEmpty(row.Cells["ClIp"].Value as string))
                    {
                        NoIp = false;
                    }

                    if (!String.IsNullOrEmpty(row.Cells["ClPort"].Value as string))
                    {
                        NoPort = false;
                    }

                    if (!String.IsNullOrEmpty(row.Cells["ClWidth"].Value as string))
                    {
                        NoWidth = false;
                    }

                    if (!String.IsNullOrEmpty(row.Cells["ClHeight"].Value as string))
                    {
                        NoHeight = false;
                    }

                    if (!String.IsNullOrEmpty(row.Cells["ClMoveAfter"].Value as string))
                    {
                        NoMoveAfter = false;
                    }

                    if(row.Cells["ClDefaultSize"].FormattedValue is bool cell_checked2 && cell_checked2)
                    {
                        NoDefaultSize = false;
                    }
                }
            }

            int ColumnIndex = 1;
            if (NoDescription)
                dataGridView.Columns[ColumnIndex].Width = 0;

            //Incremented by 3 to avoid the Last Checked and Time Elapsed Columns
            ColumnIndex++;
            ColumnIndex++;
            ColumnIndex++;

            if (NoCommand)
                dataGridView.Columns[ColumnIndex].Visible = false;

            ColumnIndex++;

            if (NoArg)
                dataGridView.Columns[ColumnIndex].Visible = false;

            ColumnIndex++;

            if (NoIp)
                dataGridView.Columns[ColumnIndex].Visible = false;

            ColumnIndex++;

            if (NoPort)
                dataGridView.Columns[ColumnIndex].Visible = false;

            ColumnIndex++;

            if (NoWidth)
                dataGridView.Columns[ColumnIndex].Visible = false;

            ColumnIndex++;

            if (NoHeight)
                dataGridView.Columns[ColumnIndex].Visible = false;

            //Incremented by 3 to avoid the xLocation and yLocation Columns
            ColumnIndex++;
            ColumnIndex++;
            ColumnIndex++;

            if (NoMoveAfter)
                dataGridView.Columns[ColumnIndex].Visible = false;

            ColumnIndex++;

            if(NoDefaultSize)
                dataGridView.Columns[ColumnIndex].Visible = false;

            Log("Leaving  \"private void OptimizeColumns()\"", LoggingLevel.mid);
        }


        /// <summary>
        /// Resizes the dataGridView to fit all the rows
        /// </summary>
        private void OptimizeRows()
        {
            Log("Entering \"private void OptimizeRows()\"", LoggingLevel.mid);
            this.Height = this.MinimumSize.Height + (23 * (dataGridView.Rows.Count));
            Log("Leaving  \"private void OptimizeRows()\"", LoggingLevel.mid);
        }


        /// <summary>
        /// Makes Columns Visible
        /// </summary>
        private void MakeVisible()
        {
            Log("Entering \"private void MakeVisible()\"", LoggingLevel.mid);

            //Sets all columns to visible except for "ClAdded"
            foreach (DataGridViewColumn dataGridViewColumn in dataGridView.Columns)
            {
                switch(dataGridViewColumn.Name)
                {
                    //Goes into the default case if the Name of the Column is not names "ClAdded"
                    default:
                        dataGridViewColumn.Visible = true;
                        break;
                    
                    case "ClAdded":
                        break;
                }
            }
            Log("Leaving  \"private void MakeVisible()\"", LoggingLevel.mid);
        }


        /// <summary>
        /// Removes individual thread from the main _threads list
        /// </summary>
        /// <param name="currComputer"></param>
        private void RemoveThread(GridDataViewItems currComputer)
        {
            Log("Entering \"private void RemoveThread(GridDataViewItems currItem)\"", LoggingLevel.mid);

            //If the current computer is in the main _threads list, stop the thread and remove it from the list
            if (_threads.ContainsKey(currComputer.ip))
            {
                try
                {
                    _threads[currComputer.ip].Abort();
                    _threads.Remove(currComputer.ip);
                }
                catch (Exception error)
                {
                    Log("Exception Caught \"private void RemoveThread(GridDataViewItems currItem)\" --> " + error.Message, LoggingLevel.high);
                }               
            }

            Log("Leaving  \"private void RemoveThread(GridDataViewItems currItem)\"", LoggingLevel.mid);
        }


        [DllImport("user32.dll")]
        static extern int SetWindowText(IntPtr hWnd, string text);

        private void RenameWindow(GridDataViewItems CurrentComputer)
        {
            Thread.Sleep(100);
            try
            {
                if(!CurrentComputer.ProcessPointer.HasExited)
                    if(CurrentComputer.ProcessPointer.MainWindowHandle != IntPtr.Zero)
                        SetWindowText(CurrentComputer.ProcessPointer.MainWindowHandle, CurrentComputer.description);
            }
            catch (Exception) { }
            
        }

        private readonly object PortPing_lock = new object();
        /// <summary>
        /// Port pings computers to see if they are active, then reopens the window
        /// </summary>
        private void PortPing(GridDataViewItems CurrentComputer)
        {
            try
            {
                Log("Entering \"private void PortPing(String[] Argument, GridDataViewItems currItem)\"", LoggingLevel.mid);
                Log("IP Address = \"" + CurrentComputer.ip + "\"", LoggingLevel.mid);
                Log("Port = \"" + CurrentComputer.port + "\"", LoggingLevel.mid);

                //Stopwatch for the Time Elapsed column
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                TimeSpan TimeElapsed;

                int WaitTime = 1000;               

                Log("Entering While loop", LoggingLevel.mid);

                //Runs indefinitly
                while (true)
                {
                    TcpClient tcpClient = new TcpClient();
                    Boolean PortSuccess = false;
                    try
                    {
                        tcpClient.Connect(CurrentComputer.ip, CurrentComputer.port);
                        if (tcpClient.Connected)
                            PortSuccess = true;
                        tcpClient.Close();
                    }
                    catch (Exception)
                    {
                        //If there is a problem connecting to the computer:
                        PortSuccess = false;
                    }
                    //Sucessful ping
                    if (PortSuccess)
                    {
                        CurrentComputer.FailureCount = 0;

                        try
                        {
                            dataGridView.Rows[CurrentComputer.id].Cells["ClTime"].Value = DateTime.Now.ToString("HH:mm:ss");
                        }
                        catch (Exception error)
                        {
                            Log("Exception Caught \"private void PortPing(GridDataViewItems currComputer)\" --> " + error.Message, LoggingLevel.high);
                            //Catches if the currComputer is closed before the try body is finished
                        }

                        lock (PortPing_lock)
                        {
                            //If the current computer does not have a pointer or the pointers window is closed
                            if (CurrentComputer.ProcessPointer == null || CurrentComputer.ProcessPointer.HasExited)
                            {
                                RestoreWindow(CurrentComputer);
                            }
                            dataGridView.Rows[CurrentComputer.id].Cells["ClElapsed"].Value = "";
                            stopwatch.Reset();
                        }
                        Thread.Sleep(5000);
                    }
                    //Failed connection
                    else
                    {
                        tcpClient.Close();
                        CurrentComputer.FailureCount++;

                        //Logs how many times a thread has failed
                        Log("IP for currComputer: " + CurrentComputer.ip + ". Failure count: " + CurrentComputer.FailureCount, LoggingLevel.mid);

                        //If the current computer has failed its connection more than twice:
                        if (CurrentComputer.FailureCount > 2)
                        {
                            //Checks if the processPointer for the current computer is not already null
                            if (CurrentComputer.ProcessPointer != null)
                            {
                                //Close the window and set processPointer to null
                                if (!CurrentComputer.ProcessPointer.HasExited)
                                    CurrentComputer.ProcessPointer.CloseMainWindow();
                                CurrentComputer.ProcessPointer = null;
                            }
                        }
                        try
                        {
                            //Updates the Last Time checked and Time Elapsed Cells
                            dataGridView.Rows[CurrentComputer.id].Cells["ClTime"].Value = DateTime.Now.ToString("HH:mm:ss");

                            stopwatch.Stop();
                            TimeElapsed = stopwatch.Elapsed;
                            dataGridView.Rows[CurrentComputer.id].Cells["ClElapsed"].Value = String.Format("{0:00}:{1:00}:{2:00}",
                                                                                          TimeElapsed.Hours, TimeElapsed.Minutes, TimeElapsed.Seconds);
                            stopwatch.Start();
                        }
                        catch (Exception error) 
                        {
                            Log("Exception Caught \"private void PortPing(GridDataViewItems currComputer)\" --> " + error.Message, LoggingLevel.high);
                        }
                        Thread.Sleep(WaitTime);
                    }
                    Thread.Sleep(WaitTime);
                }
            }
            catch (ThreadAbortException)
            {
                return;
            }

        }


        /// <summary>
        /// Adds the newItem GridDataViewItem to the GridDataView
        /// </summary>
        /// <param name="CurrentComputer"></param>
        private void AddtoGui(GridDataViewItems CurrentComputer)
        {
            Log("Entering \"private void AddtoGui(GridDataViewItems newItem, bool editLine)\"", LoggingLevel.mid);

            //Adds the contents of the CurrentComputer to the GUI and if a value in
            //the CurrentComputer is null, it makes the corresponding cell null

            int index = CurrentComputer.id;

            dataGridView.Rows[index].Cells["ClDescription"].Value = CurrentComputer.description;

            if (CurrentComputer.commandLine.Equals(DefaultCommandLine))
                dataGridView.Rows[index].Cells["ClCommandLine"].Value = null;
            else
                dataGridView.Rows[index].Cells["ClCommandLine"].Value = CurrentComputer.commandLine;

            if (CurrentComputer.arguments.ToString().Contains(CurrentComputer.ip.ToString()))
                dataGridView.Rows[index].Cells["ClArguments"].Value = CurrentComputer.ip;
            else
                dataGridView.Rows[index].Cells["ClArguments"].Value = CurrentComputer.arguments;

            dataGridView.Rows[index].Cells["ClIp"].Value = CurrentComputer.ip;

            if (CurrentComputer.port == DefaultPort)
                dataGridView.Rows[index].Cells["ClPort"].Value = null;
            else
                dataGridView.Rows[index].Cells["ClPort"].Value = CurrentComputer.port;

            if (CurrentComputer.width == DefaultWidth)
                dataGridView.Rows[index].Cells["ClWidth"].Value = null;
            else
                dataGridView.Rows[index].Cells["ClWidth"].Value = CurrentComputer.width;

            if (CurrentComputer.height == DefaultHeight)
                dataGridView.Rows[index].Cells["ClHeight"].Value = null;
            else
                dataGridView.Rows[index].Cells["ClHeight"].Value = CurrentComputer.height;

            dataGridView.Rows[index].Cells["ClXLoc"].Value = CurrentComputer.xLoc;
            dataGridView.Rows[index].Cells["ClYLoc"].Value = CurrentComputer.yLoc;

            if (CurrentComputer.moveAfter == DefaultMoveAfter)
                dataGridView.Rows[index].Cells["ClMoveAfter"].Value = null;
            else
                dataGridView.Rows[index].Cells["ClMoveAfter"].Value = CurrentComputer.moveAfter;

            dataGridView.Rows[index].Cells["ClDefaultSize"].Value = CurrentComputer.defaultSize;

            dataGridView.Rows[dataGridView.Rows.Count - 1].Cells["ClId"].Value = dataGridView.Rows.Count;

            OptimizeRows();

            Log("Leaving  \"private void AddtoGui(GridDataViewItems newItem, bool editLine)\"", LoggingLevel.mid);
        }
        

        /// <summary>
        /// Writes to the opened .csv file
        /// </summary>
        /// <param name="DestinationFile">The path for the opened .csv file</param>
        private void CSVWrite(String DestinationFile)
        {
            Log("Entering \"private void CSVWrite()\"", LoggingLevel.mid);

            try
            {
                //If the destination file is null or empty, find a file to save to
                if(String.IsNullOrEmpty(DestinationFile))
                {
                    #region
                    //SaveFileDialog saveFileDialog = new SaveFileDialog();
                    //saveFileDialog.Filter = "csv Files|*.csv";
                    //saveFileDialog.Title = "Save As";

                    //if(saveFileDialog.ShowDialog() == DialogResult.OK)
                    //    DestinationFile = saveFileDialog.FileName;
                    #endregion
                    DestinationFile = GetFile("Save as");
                }             
                
                //Double checks that the destinationFile is not null or empty
                if (!String.IsNullOrEmpty(DestinationFile))
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(DestinationFile, false))
                    {
                        //Writes the header to the file
                        file.WriteLine("Command Line" + ',' +
                                       "Arguments" + ',' +
                                       "IP Address" + ',' +
                                       "Port" + ',' +
                                       "Width" + ',' +
                                       "Height" + ',' +
                                       "X Location" + ',' +
                                       "Y Location" + ',' +
                                       "Move After" + ',' +
                                       "Default Size" + ',' +
                                       "Description");

                        Log("Entering \"private void CSVWrite()\" foreach loop", LoggingLevel.high);

                        //Iterates through the rows of the dataGridView and if a cell if null or empty, we write "" in its place in the .csv file
                        foreach (var currComputer in _GDVI)
                        {
                            String isDefaultStr = "False";

                            if (currComputer.defaultSize == true)
                                isDefaultStr = "True";

                            //Sets variable used to write to the file to: "" if the cell that is being checked is null or empty.
                            string commandLine = "", arguments = "", ip = "", port = "", width = "", height = "", xLoc = "", yLoc = "", moveAfter = "", description = "";

                            if (!string.IsNullOrEmpty(dataGridView.Rows[currComputer.id].Cells["ClCommandLine"].Value as string))
                                commandLine = dataGridView.Rows[currComputer.id].Cells["ClCommandLine"].Value.ToString().Trim(' ');

                            if (!string.IsNullOrEmpty(dataGridView.Rows[currComputer.id].Cells["ClArguments"].Value as string))
                                arguments = dataGridView.Rows[currComputer.id].Cells["ClArguments"].Value.ToString().Trim(' ');

                            if (!string.IsNullOrEmpty(dataGridView.Rows[currComputer.id].Cells["ClIp"].Value as string))
                                ip = dataGridView.Rows[currComputer.id].Cells["ClIp"].Value.ToString().Trim(' ');

                            if (!string.IsNullOrEmpty(dataGridView.Rows[currComputer.id].Cells["ClPort"].Value as string))
                                port = dataGridView.Rows[currComputer.id].Cells["ClPort"].Value.ToString().Trim(' ');

                            if (!string.IsNullOrEmpty(dataGridView.Rows[currComputer.id].Cells["ClWidth"].Value as string))
                                width = dataGridView.Rows[currComputer.id].Cells["ClWidth"].Value.ToString().Trim(' ');

                            if (!string.IsNullOrEmpty(dataGridView.Rows[currComputer.id].Cells["ClHeight"].Value as string))
                                height = dataGridView.Rows[currComputer.id].Cells["ClHeight"].Value.ToString().Trim(' ');

                            if (!string.IsNullOrEmpty(dataGridView.Rows[currComputer.id].Cells["ClXLoc"].Value as string))
                                xLoc = dataGridView.Rows[currComputer.id].Cells["ClXLoc"].Value.ToString().Trim(' ');

                            if (!string.IsNullOrEmpty(dataGridView.Rows[currComputer.id].Cells["ClYLoc"].Value as string))
                                yLoc = dataGridView.Rows[currComputer.id].Cells["ClYLoc"].Value.ToString().Trim(' ');

                            if (!string.IsNullOrEmpty(dataGridView.Rows[currComputer.id].Cells["ClMoveAfter"].Value as string))
                                moveAfter = dataGridView.Rows[currComputer.id].Cells["ClMoveAfter"].Value.ToString().Trim(' ');

                            if (!string.IsNullOrEmpty(dataGridView.Rows[currComputer.id].Cells["ClDescription"].Value as string))
                                description = dataGridView.Rows[currComputer.id].Cells["ClDescription"].Value.ToString().Trim(' ');

                            //Writes the variables that are set to "" if the cells are found to be null or empty.
                            //If the cells are not found to be null or empty, we write the cells contents to the file.
                            file.WriteLine(commandLine + ',' +
                                           arguments + ',' +
                                           ip + ',' +
                                           port + ',' +
                                           width + ',' +
                                           height + ',' +
                                           xLoc + ',' +
                                           yLoc + ',' +
                                           moveAfter + ',' +
                                           isDefaultStr + ',' +
                                           description);
                        }
                        SaveRecord();
                        Log("Leaving  \"private void CSVWrite()\" foreach loop", LoggingLevel.high);
                    }
                }
            }
            catch (Exception error)
            {
                Log("Exception Caught \"private void CSVWrite()\" --> " + error.Message, LoggingLevel.mid);
                Log("Exception Caught \"private void CSVWrite()\" Destination File: --> " + DestinationFile, LoggingLevel.mid);
                String message = "Bad file path: " + DestinationFile;
                String caption = "Error detected in file path textbox.";

                MessageBox(message, caption);
            }
            Log("Leaving  \"private void CSVWrite()\"", LoggingLevel.mid);
        }


        /// <summary>
        /// Clears the dataGridView and all internal relevant lists such as _GDVI & _Threads & Default Variables
        /// </summary>
        private void ClearAll()
        {
            //Clears the contents of the main GridDataViewItems list and main _threads list
            while (_GDVI.Count > 0)
            {
                RemoveThread(_GDVI[_GDVI.Count - 1]);

                if (_GDVI[_GDVI.Count - 1].ProcessPointer != null && !_GDVI[_GDVI.Count - 1].ProcessPointer.HasExited)
                    _GDVI[_GDVI.Count - 1].ProcessPointer.CloseMainWindow();

                _GDVI.RemoveAt(_GDVI.Count - 1);
            }

            //Clear the rows of the dataGridView if there is at least one
            if (dataGridView.Rows.Count > 0)
            {
                dataGridView.Rows.Clear();
            }

            //Clears the relevant information from the Default Variables
            DefaultPort = 0; DefaultWidth = 0; DefaultHeight = 0; DefaultMoveAfter = 1; DefaultXLoc = 0; DefaultYLoc = 0;
            DefaultCommandLine = ""; DefaultArguments = ""; DefaultIp = ""; DefaultDescription = ""; DefaultRemoteFolder = "";
            DefaultSize = false;
    }


        /// <summary>
        /// Reads in a .csv file and filters the data through an .ini file if avaliable
        /// </summary>
        /// <param name="arg">Taken in from the command line arguements</param>
        private void CSVReadWithIni(String arg)
        {
            Log("Entering \"private void CSVReadWithIni(String arg)\"", LoggingLevel.mid);
            
            //Checks for command line argument
            string file;
            //The command line argument would contain the file that is to be opened.
            //If there is no file that needs to be opened, we call GetFile to get the file that needs to be opened
            if (arg == null)
            {
                file = GetFile("Open");
                if (file == null)
                    return;
            }
            //If there is an argument, set the file to the argument
            else
                file = arg;

            //Saves file path to global variable
            OpenedFile = file;
            IniFile = Path.GetFileNameWithoutExtension(OpenedFile) + ".ini";

            if (!File.Exists(file))
            {
                MessageBox("CSV file: \"" + file + "\" does not exitst", "No CSV File");                
                return;
            }

            //If there is no .ini file that matches the name of the opened file, we use the Default .ini file.
            if (!File.Exists(IniFile))
            {
                IniFile = Path.GetFileNameWithoutExtension("Default") + ".ini";
            }

            if(!AssignDefaults())
            {
                ClearAll();
                return;
            }

            try
            {
                //If there is no data in the csv file, varialble == true
                //bool NoData = true;

                //Adds the files contents to the GUI
                var lines = File.ReadAllLines(file);                               
                int ColumnIndex;
                int rowNum = 0;
                Dictionary<int, string> _headers = new Dictionary<int, string>();
                //Writes strings from the opened file to the dataGridView
                foreach (String line in lines)
                {
                    String[] currLine = line.Split(',');

                    //Creates dictonary
                    if (rowNum == 0)
                    {
                        for (int i = 0; i < currLine.Count(); i++)
                        {
                            switch (currLine[i].ToLower())
                            {
                                case "command line":
                                    _headers.Add(i, currLine[i]);
                                    break;

                                case "arguments":
                                    _headers.Add(i, currLine[i]);
                                    break;

                                case "ip address":
                                    _headers.Add(i, currLine[i]);
                                    break;

                                case "port":
                                    _headers.Add(i, currLine[i]);
                                    break;

                                case "width":
                                    _headers.Add(i, currLine[i]);
                                    break;

                                case "height":
                                    _headers.Add(i, currLine[i]);
                                    break;

                                case "x location":
                                    _headers.Add(i, currLine[i]);
                                    break;

                                case "y location":
                                    _headers.Add(i, currLine[i]);
                                    break;

                                case "move after":
                                    _headers.Add(i, currLine[i]);
                                    break;

                                case "default size":
                                    _headers.Add(i, currLine[i]);
                                    break;

                                case "description":
                                    _headers.Add(i, currLine[i]);
                                    break;

                                default:
                                    if (currLine[i].Length < 16)
                                    {
                                        MessageBox("Problem: Unknown Field: \"" + currLine[i] + "\" in file \"" + Path.GetFileName(OpenedFile) + "\"" +
                                                   "\n\nValid Fields: Command Line\n" +
                                                   "                    Arguments\n" +
                                                   "                    IP Address\n" +
                                                   "                    Port\n" +
                                                   "                    Width\n" +
                                                   "                    Height\n" +
                                                   "                    X Location\n" +
                                                   "                    Y Location\n" +
                                                   "                    Move After\n" +
                                                   "                    Default Size\n" +
                                                   "                    Description\n\n" +
                                                   "Solution: Replace unknown field: \"" + currLine[i] + "\" in file \"" + Path.GetFileName(OpenedFile) + "\" with one of the avaliable fields.\n\n\n\n", "Unknown Field Found");
                                    }
                                    else
                                    {
                                        string ReducedField = currLine[i].Substring(0, 16);

                                        MessageBox("Problem: Unknown Field: \"" + ReducedField + "\"... in file \"" + Path.GetFileName(OpenedFile) + "\"" +
                                                   "\n\nValid Fields: Command Line\n" +
                                                   "                    Arguments\n" +
                                                   "                    IP Address\n" +
                                                   "                    Port\n" +
                                                   "                    Width\n" +
                                                   "                    Height\n" +
                                                   "                    X Location\n" +
                                                   "                    Y Location\n" +
                                                   "                    Move After\n" +
                                                   "                    Default Size\n" +
                                                   "                    Description\n\n" +
                                                   "Solution: Replace unknown field: \"" + ReducedField + "\"... in file \"" + Path.GetFileName(OpenedFile) + "\" with one of the avaliable fields.\n\n\n\n", "Unknown Field Found");
                                    }

                                    Log("Arguemnt not filtered through switch--> " + currLine[i].ToLower(), LoggingLevel.high);
                                    return;
                            }
                        }
                    }
                    else
                    {
                        ColumnIndex = 0;

                        dataGridView.Rows.Add();
                        dataGridView.Rows[rowNum - 1].Cells["ClId"].Value = dataGridView.Rows.Count;
                        
                        for (int i = 0; i < currLine.Count(); i++)
                        {                        
                            switch (_headers[i].ToLower())
                            {
                                case "command line":
                                    dataGridView.Rows[rowNum - 1].Cells["ClCommandLine"].Value = currLine[ColumnIndex];
                                    ColumnIndex++;
                                    break;

                                case "arguments":
                                    dataGridView.Rows[rowNum - 1].Cells["ClArguments"].Value = currLine[ColumnIndex];
                                    ColumnIndex++;
                                    break;

                                case "ip address":
                                    dataGridView.Rows[rowNum - 1].Cells["ClIp"].Value = currLine[ColumnIndex];
                                    ColumnIndex++;
                                    break;

                                case "port":
                                    dataGridView.Rows[rowNum - 1].Cells["ClPort"].Value = currLine[ColumnIndex];
                                    ColumnIndex++;
                                    break;

                                case "width":
                                    dataGridView.Rows[rowNum - 1].Cells["ClWidth"].Value = currLine[ColumnIndex];
                                    ColumnIndex++;
                                    break;

                                case "height":
                                    dataGridView.Rows[rowNum - 1].Cells["ClHeight"].Value = currLine[ColumnIndex];
                                    ColumnIndex++;
                                    break;

                                case "x location":
                                    dataGridView.Rows[rowNum - 1].Cells["ClXLoc"].Value = currLine[ColumnIndex];
                                    ColumnIndex++;
                                    break;

                                case "y location":
                                    dataGridView.Rows[rowNum - 1].Cells["ClYLoc"].Value = currLine[ColumnIndex];
                                    ColumnIndex++;
                                    break;

                                case "move after":
                                    dataGridView.Rows[rowNum - 1].Cells["ClMoveAfter"].Value = currLine[ColumnIndex];
                                    ColumnIndex++;
                                    break;

                                case "default size":
                                    dataGridView.Rows[rowNum - 1].Cells["ClDefaultSize"].Value = false;
                                    ColumnIndex++;
                                    break;

                                case "description":
                                    dataGridView.Rows[rowNum - 1].Cells["ClDescription"].Value = currLine[ColumnIndex];
                                    ColumnIndex++;
                                    break;

                                default:
                                    if (_headers[i].Length < 16)
                                    {
                                        MessageBox("Problem: Unknown Field: \"" + _headers[i] + "\" in file \"" + Path.GetFileName(OpenedFile) + "\"" +
                                                    "\n\nValid Fields: Command Line\n" +
                                                    "                    Arguments\n" +
                                                    "                    IP Address\n" +
                                                    "                    Port\n" +
                                                    "                    Width\n" +
                                                    "                    Height\n" +
                                                    "                    X Location\n" +
                                                    "                    Y Location\n" +
                                                    "                    Move After\n" +
                                                    "                    Default Size\n" +
                                                    "                    Description\n\n" +
                                                    "Solution: Replace unknown field: \"" + _headers[i] + "\" in file \"" + Path.GetFileName(OpenedFile) + "\" with one of the avaliable fields.\n\n\n\n", "Unknown Field Found");
                                    }
                                    else
                                    {
                                        string ReducedField = _headers[i].Substring(0, 16);

                                        MessageBox("Problem: Unknown Field: \"" + ReducedField + "\"... in file \"" + Path.GetFileName(OpenedFile) + "\"" +
                                                    "\n\nValid Fields: Command Line\n" +
                                                    "                    Arguments\n" +
                                                    "                    IP Address\n" +
                                                    "                    Port\n" +
                                                    "                    Width\n" +
                                                    "                    Height\n" +
                                                    "                    X Location\n" +
                                                    "                    Y Location\n" +
                                                    "                    Move After\n" +
                                                    "                    Default Size\n" +
                                                    "                    Description\n\n" +
                                                    "Solution: Replace unknown field: \"" + ReducedField + "\"... in file \"" + Path.GetFileName(OpenedFile) + "\" with one of the avaliable fields.\n\n\n\n", "Unknown Field Found");
                                    }

                                    Log("Arguemnt not filtered through switch--> " + _headers[i].ToLower(), LoggingLevel.high);
                                    return;
                            }

                        }
                                               
                    }
                    rowNum++;
                }
                //If the file is empty, but there is a header line
                if (lines.Length == 1 || lines.Length == 0)
                {
                    MessageBox("File \"" + Path.GetFileName(OpenedFile) + "\" is empty. \n\n\n", "Empty File Found");
                    return;
                }
            }
            catch (Exception error)
            {
                ClearAll();
                Log("Exception Caught \"private void CSVReadWithIni(String arg)\" --> " + error.Message, LoggingLevel.high);
                MessageBox(error.Message, "Error On Open File");                
            }
            

            try
            {
                //Goes through the dataGridView and Assigns default values where nessessary or values from the dataGridView to the GridDataViewItem
                //which gets saved to the main GridDataViewItem List
                foreach (DataGridViewRow currRow in dataGridView.Rows)
                {
                    GridDataViewItems newItem = new GridDataViewItems
                    {
                        id = currRow.Index
                    };

                    if (String.IsNullOrEmpty(currRow.Cells["ClCommandLine"].Value as string))
                    {
                        newItem.commandLine = DefaultCommandLine;
                    }
                    else
                    {
                        newItem.commandLine = currRow.Cells["ClCommandLine"].Value.ToString();
                    }

                    if (String.IsNullOrEmpty(currRow.Cells["ClPort"].Value as string))
                    {
                        newItem.port = Convert.ToInt32(DefaultPort);
                    }
                    else
                    {
                        newItem.port = Convert.ToInt32(currRow.Cells["ClPort"].Value.ToString().Trim(' '));
                    }

                    if (String.IsNullOrEmpty(currRow.Cells["ClWidth"].Value as string))
                    {
                        newItem.width = Convert.ToInt32(DefaultWidth);
                    }
                    else
                    {
                        newItem.width = Convert.ToInt32(currRow.Cells["ClWidth"].Value.ToString().Trim(' '));
                    }

                    if (String.IsNullOrEmpty(currRow.Cells["ClHeight"].Value as string))
                    {
                        newItem.height = Convert.ToInt32(DefaultHeight);
                    }
                    else
                    {
                        newItem.height = Convert.ToInt32(currRow.Cells["ClHeight"].Value.ToString().Trim(' '));
                    }

                    if (String.IsNullOrEmpty(currRow.Cells["ClMoveAfter"].Value as string))
                    {
                        newItem.moveAfter = Convert.ToInt32(DefaultMoveAfter);
                    }
                    else
                    {
                        newItem.moveAfter = Convert.ToInt32(currRow.Cells["ClMoveAfter"].Value.ToString().Trim(' '));
                    }

                    if (String.IsNullOrEmpty(currRow.Cells["ClDefaultSize"].Value as string))
                    {
                        newItem.defaultSize = Convert.ToBoolean(DefaultSize);
                    }
                    else
                    {
                        newItem.defaultSize = Convert.ToBoolean(currRow.Cells["ClDefaultSize"].Value.ToString().Trim(' '));
                    }

                    if (String.IsNullOrEmpty(currRow.Cells["ClDescription"].Value as string))
                    {
                        newItem.description = DefaultDescription;
                    }
                    else
                    {
                        newItem.description = currRow.Cells["ClDescription"].Value.ToString().Trim(' ');
                    }

                    if (String.IsNullOrEmpty(currRow.Cells["ClXLoc"].Value as string))
                    {
                        //Do nothing
                    }
                    else
                    {
                        if (int.TryParse(currRow.Cells["ClXLoc"].Value.ToString().Trim(' '), out int xLoc))
                        {
                            newItem.xLoc = xLoc;
                        }
                    }

                    if (String.IsNullOrEmpty(currRow.Cells["ClYLoc"].Value as string))
                    {
                        //Do nothing
                    }
                    else
                    {
                        if (int.TryParse(currRow.Cells["ClYLoc"].Value.ToString().Trim(' '), out int yLoc))
                        {
                            newItem.yLoc = yLoc;
                        }
                    }


                    //Special Case for Ip. Find and replace ip to the default value ip to determine
                    //if the ip needs to be default.
                    if (String.IsNullOrEmpty(currRow.Cells["ClIp"].Value as string))
                    {
                        break;
                    }
                    else if (int.TryParse(currRow.Cells["ClIp"].Value.ToString().Trim(' '), out _))
                    {
                        if (DefaultIp != "")
                        {
                            string[] SplitIp = currRow.Cells["ClIp"].Value.ToString().Trim(' ').Split('.');
                            string StringIp = SplitIp[SplitIp.Length - 1];

                            newItem.ip = DefaultIp.Replace("###", StringIp);
                        }
                    }
                    else
                    {
                        newItem.ip = currRow.Cells["ClIp"].Value.ToString().Trim(' ');
                    }

                    //Log("22060812", LoggingLevel.mid);
                    if (String.IsNullOrEmpty(currRow.Cells["ClArguments"].Value as string))
                    {
                        if (!String.IsNullOrEmpty(newItem.ip))
                        {
                            string[] SplitIp = newItem.ip.Split('.');
                            string StringIp = SplitIp[SplitIp.Length - 1];

                            if (int.TryParse(StringIp, out int IntIp))
                            {
                                //Formats IP so it can find the correct file specific for the IP
                                if (IntIp < 1000)
                                {
                                    if (IntIp < 10)
                                    {
                                        StringIp = "00" + StringIp;
                                    }
                                    else if (IntIp < 100)
                                    {
                                        StringIp = "0" + StringIp;
                                    }

                                }
                            }

                            newItem.arguments = DefaultArguments.Replace("###", StringIp);
                            newItem.argumentsIni = true;
                        }
                    }
                    else if (currRow.Cells["ClArguments"].Value.ToString() == newItem.ip.ToString())
                    {
                        string[] splitIp = newItem.ip.Split('.');
                        newItem.arguments = DefaultArguments.Replace("###", splitIp[splitIp.Length - 1]);
                        newItem.argumentsIni = true;
                    }
                    else
                    {
                        newItem.arguments = currRow.Cells["ClArguments"].Value.ToString().Trim(' ');
                    }

                    currRow.Cells["ClAdded"].Value = true;
                    _GDVI.Add(newItem);
                }
            }
            catch (Exception error)
            {
                Log("Exception Caught \"private void CSVReadWithIni(String arg)\" --> " + error.Message, LoggingLevel.high);
                MessageBox(error.Message, "Error On Open File");
                ClearAll();
            }

            //Enables the buttons
            btnOpenCommandFolder.Enabled = true;
            buttonCreateFile.Enabled = true;
            ButtonRestore.Enabled = true;
            dataGridToolStripMenuItem.Enabled = true;

            Log("Leaving  \"private void CSVReadWithIni(String arg)\"", LoggingLevel.mid);
        }


        /// <summary>
        /// Assigns default values from the .ini file to global variables
        /// </summary>
        private bool AssignDefaults()
        {
            Log("Entering \"private void AssignDefaults()\"", LoggingLevel.mid);
            if (File.Exists(IniFile))
            {
                IniLines = File.ReadAllLines(IniFile);

                //Iterates through each line in the .ini file
                foreach (String line in IniLines)
                {
                    //Splitting each line and inserting into list or array
                    String[] currLine = line.Split('=');
                    
                    //Switch Statement that assigns the proper default variables to their value from the .ini file
                    //regardless of the order that the variables are found in the .ini file
                    switch (currLine[0].ToLower())
                    {
                        case "port":
                            if (int.TryParse(currLine[1], out int _PortNumber))
                                DefaultPort = _PortNumber;
                            else
                            {
                                MessageBox("The DefaultPort argument: " + currLine[1] + " is not a valid port number. Please ensure that the argument is a valid port number, for example the port \"5900\"\n\n\n", "Initilization File Error");
                                return false;
                            }
                            break;

                        case "width":
                            if(int.TryParse(currLine[1], out int _WidthNumber))
                                DefaultWidth = _WidthNumber;
                            else
                            {
                                MessageBox("The Width argument: " + currLine[1] + " is not a valid width value. Please ensure that the argument is a valid width value.\n\n\n", "Initilization File Error");
                                return false;
                            }
                            break;

                        case "height":
                            if(int.TryParse(currLine[1], out int _HeightNumber))
                                DefaultHeight = _HeightNumber;
                            else
                            {
                                MessageBox("The Height argument: " + currLine[1] + " is not a valid height value. Please ensure that the argument is a valid height value.\n\n\n", "Initilization File Error");
                                return false;
                            }
                            break;

                        case "move after":
                            if(int.TryParse(currLine[1], out int _MoveAfterNumber))
                                DefaultMoveAfter = _MoveAfterNumber;
                            else
                            {
                                MessageBox("The Move After argument: " + currLine[1] + " is not a valid Move After value. Please ensure that the argument is a valid time value in seconds.\n\n\n", "Initilization File Error");
                                return false;
                            }
                            break;

                        case "size":
                            if(Boolean.TryParse(currLine[1], out bool _SizeBool))
                                DefaultSize = _SizeBool;
                            else
                            {
                                MessageBox("The Size argument: " + currLine[1] + " is not a valid boolean value. Please ensure that the argument is a valid boolena value.\n\n\n", "Initilization File Error");
                                return false;
                            }
                            break;

                        case "command line":
                            DefaultCommandLine = currLine[1];
                            break;

                        case "arguments":
                            DefaultArguments = currLine[1];
                            break;

                        case "ip address":
                            DefaultIp = currLine[1];
                            break;

                        case "description":
                            DefaultDescription = currLine[1];
                            break;

                        case "remote folder":
                            DefaultRemoteFolder = currLine[1];
                            break;
                    }
                }
            }
            Log("Leaving  \"private void AssignDefaults()\"", LoggingLevel.mid);
            return true;
        }


        /// <summary>
        /// Gets and returns file to read from
        /// </summary>
        /// <returns>FilePath</returns>
        private String GetFile(string caption)
        {
            Log("Entering \"private String GetFile()\"", LoggingLevel.mid);

            //Opens file dialog to get a file path for the Save As method
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = caption
            };

            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.

            //Makes sure the result of the file dialog is not null
            if (String.IsNullOrEmpty(result.ToString()))
                return null;

            if (result == DialogResult.OK) // Test result.
            {
                Log("Leaving  \"private String GetFile()\"", LoggingLevel.mid);

                return openFileDialog1.FileName;
            }
            //Just incase the user presses cancel and the result is not null, return null
            //It is not likely that the else statement is stepped into
            else
            {
                Log("Leaving  \"private String GetFile()\"", LoggingLevel.mid);
                return null;
            }
        }


        /// <summary>
        /// Assigns a unique ID to each of the GridDataViewItems 
        /// </summary>
        private void FixIDs()
        {
            Log("Entering \"private void FixIDs()\"", LoggingLevel.mid);

            //If the dataGridView has not rows, return
            if(dataGridView.Rows.Count == 0)
            {
                Log("Leaving  \"private void FixIDs()\"", LoggingLevel.mid);
                return;
            }

            int index = 0;
            //if there are no saved GridDataViewItems
            if (_GDVI.Count() == 0)
                dataGridView.Rows[index].Cells["ClId"].Value = index + 1;

            else
            {
                Log("Entering \"private void FixIDs()\" foreach loop", LoggingLevel.high);
                //Re-assigns Current Computers id to match its index in the main GridDataViewItems list
                foreach (GridDataViewItems CurrentComputer in _GDVI)
                {
                    if (dataGridView.Rows.Count > index)
                    {
                        CurrentComputer.id = index;

                        index++;
                    }
                }
                Log("Leaving  \"private void FixIDs()\" foreach loop", LoggingLevel.high);

                Log("Entering \"private void FixIDs()\" while loop", LoggingLevel.high);

                //Updates rows of the dataGridView to match their indexes
                index = 0;
                while (index < dataGridView.Rows.Count)
                {
                    dataGridView.Rows[index].Cells["ClId"].Value = index + 1;
                    index++;
                }

                Log("Leaving  \"private void FixIDs()\" while loop", LoggingLevel.high);
            }
            Log("Leaving  \"private void FixIDs()\"", LoggingLevel.mid);
        }

        
        /// <summary>
        /// Restores windows
        /// </summary>
        private void RestoreWindows()
        {
            Log("Entering \"private void RestoreWindows()\"", LoggingLevel.mid);
            Log("Entering \"private void RestoreWindows()\" foreach loop", LoggingLevel.high);

            //For every computer in the main GridDataViewItems list
            foreach (GridDataViewItems currComputer in _GDVI)
            {
                RestoreWindow(currComputer);
            }

            Log("Leaving  \"private void RestoreWindows()\" foreach loop", LoggingLevel.high);
            Log("Leaving  \"private void RestoreWindows()\"", LoggingLevel.mid);
        }

        
        /// <summary>
        /// Restores a single computers window
        /// </summary>
        /// <param name="CurrentComputer"></param>
        private void RestoreWindow(GridDataViewItems CurrentComputer)
        {
            Log("Entering \"private void RestoreWindow(GridDataViewItems currWindow)\"", LoggingLevel.mid);
            try
            {
                //Check if the process for the current computer is null or if the window for the process is closed
                if (CurrentComputer.ProcessPointer == null || CurrentComputer.ProcessPointer.HasExited)
                {
                    CurrentComputer.ProcessPointer = StartProcess(CurrentComputer);
                    Thread.Sleep(1000);
                    RenameWindow(CurrentComputer);
                }
                //Checks that the process is not null and the process is opened before moving the window
                if (CurrentComputer.ProcessPointer != null && !CurrentComputer.ProcessPointer.HasExited)
                    MoveWindow(CurrentComputer.ProcessPointer.MainWindowHandle, CurrentComputer.xLoc, CurrentComputer.yLoc, CurrentComputer.width, CurrentComputer.height, true);
            }
            catch (Exception) { }
            Log("Leaving \"private void RestoreWindow(GridDataViewItems currWindow)\"", LoggingLevel.mid);
        }


        /// <summary>
        /// Deletes all selected rows
        /// </summary>
        private void DeleteRow()
        {
            Log("Entering \"private void DeleteRow()\"", LoggingLevel.mid);

            List<DataGridViewRow> SelectedRows = new List<DataGridViewRow>();
            
            //Adds selected dataGridViewRows to the SelectedRows SelectedRows list
            foreach (DataGridViewRow row in dataGridView.SelectedRows)
            {
                if (!SelectedRows.Contains(row))
                    SelectedRows.Add(row);
            }

            //Adds the selected dataGridViewRows that contain selected dataGridViewCells to the SelectedRows list
            foreach (DataGridViewCell Cell in dataGridView.SelectedCells)
            {
                if (!SelectedRows.Contains(Cell.OwningRow))
                    SelectedRows.Add(Cell.OwningRow);
            }

            int rowNum;
            //Iterates through the SelectedRows list to delete
            foreach(DataGridViewRow currentRow in SelectedRows)
            {
                Log("Attempting to delete: " + currentRow.Cells["ClIp"].Value as String, LoggingLevel.high);
                
                rowNum = currentRow.Index;
                
                //If the selected row or cell is contained in the only row left
                if (dataGridView.Rows.Count > 0)
                {
                    //Makes sure that the row we are attempting to delete contains an Ip Address
                    if (!String.IsNullOrEmpty(currentRow.Cells["ClIp"].Value as string))
                    {
                        if (_GDVI.Count > 0 && _GDVI[rowNum].ProcessPointer != null && !_GDVI[rowNum].ProcessPointer.HasExited)
                        {
                            Log("Deleted: " + currentRow.Cells["ClIp"].Value as String, LoggingLevel.high);

                            _GDVI[rowNum].ProcessPointer.CloseMainWindow();
                            RemoveThread(_GDVI[rowNum]);
                            _GDVI.Remove(_GDVI[rowNum]);
                        }
                    }
                }

                dataGridView.Rows.Remove(currentRow);

                //If there are no rows left in the dataGridView
                if (dataGridView.Rows.Count == 0)
                {
                    AddRow();
                    dataGridView.Rows[0].Cells["ClId"].Value = 1;
                }
            }
            OptimizeRows();
            FixIDs();

            Log("Leaving  \"private void DeleteRow()\"", LoggingLevel.mid);
        }


        /// <summary>
        /// Adds new row if there are no empty rows avaliable
        /// </summary>
        private void AddRow()
        {
            Log("Entering \"private void AddRow()\"", LoggingLevel.mid);

            //Makes sure that the number of rows is not greater than the GridDataViewItems list + 1
            if (dataGridView.Rows.Count < _GDVI.Count + 1)
            {
                dataGridView.Rows.Add();
                dataGridView.Rows[dataGridView.Rows.Count - 1].Cells["ClId"].Value = dataGridView.Rows.Count;
            }

            Log("Leaving  \"private void AddRow()\"", LoggingLevel.mid);
        }


        /// <summary>
        /// Instanciates live logging form
        /// </summary>
        private void OpenLiveLogging()
        {
            //If LiveLog is null, it is avaliable to open
            if (LiveLog == null)
            {
                LiveLog = new DialogBox();
                LiveLog.FormClosing += LiveLog_FormClosing;

                //log_lines contains all the lines that are being logged to the log file.

                //We want to add all the lines to the new instance of the live log window so we can see
                //log calls that were previously made.
                foreach (var line in log_lines)
                {
                    LiveLog.AppendText(line);
                }
            }

            //If the liveLog window is minimized in the taskbar, we open it back on.
            if (LiveLog.WindowState == FormWindowState.Minimized)
                LiveLog.WindowState = FormWindowState.Normal;
        }


        private List<string> log_lines = new List<string>();
        private void LiveLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (LiveLog != null)
                {
                    LiveLog.Closed = true;
                    log_lines = LiveLog.LogLines;
                }
            }
            catch (Exception) { }
        }


        /// <summary>
        /// Creates a command file with the contents of a selected template
        /// named the Ip of the selected row in the dataGridView
        /// </summary>
        private void CreateCommandFile()
        {
            Log("Entering \"private void CreateCommandFile()\"", LoggingLevel.mid);
            
            //Checks if the required items are selected
            #region
            //Checks if there are selected Rows or Cells and Templates
            bool NoSelectedTemplates = false, NoSelectedRowsOrCells = false, NoRemoteFolder = false;
            
            String caption = "Create File Error", Message = "";

            //Checks if there is more than one selected Template from the listBoxTemplates
            if (listBoxTemplates.SelectedItems.Count == 0)
            {
                NoSelectedTemplates = true;
                Message += "No Templates Selected\n\n";
            }
            //If there is more than one selected Row or Cell
            if (dataGridView.SelectedRows.Count == 0 && dataGridView.SelectedCells.Count == 0)
            {
                NoSelectedRowsOrCells = true;
                Message += "No Rows or Cells Selected\n\n";
            }
            //Checks if the DefaultRemoteFolder is not null or empty or if the DefaultRemoteFolder does not exist
            if (String.IsNullOrEmpty(DefaultRemoteFolder))
            {
                NoRemoteFolder = true;
                Message += "No remote folder inputed from the initialization file.\n\nEnsure there is an argument in the initiliation file for Remote Folder before executing commands.\n\n\n\n";
            }
            #endregion

            //Returns if the required items are not selected
            #region
            //Returns if there are no selected Rows or Templates
            if (NoSelectedRowsOrCells || NoSelectedTemplates || NoRemoteFolder)
            {
                MessageBox(Message, caption);
                return;
            }

            Log("private void CreateCommandFile() 22061001", LoggingLevel.mid);
            #endregion

            //Checks if the last character in the DefaultRemoteFolder string is a \
            if (!DefaultRemoteFolder.EndsWith("\\")) 
                DefaultRemoteFolder = DefaultRemoteFolder + "\\";


            //Makes directory if it does not exist
            #region
            try
            {
                //Checks if the directory exists and makes it if it does not
                if (!Directory.Exists(DefaultRemoteFolder))
                    Directory.CreateDirectory(DefaultRemoteFolder);
            }
            catch (Exception error)
            {
                Log("Exception Caught \"private void CreateCommandFile()\" --> " + error.Message, LoggingLevel.high);
                MessageBox("Bad File Path: " + DefaultRemoteFolder, "Bad File Path Error.");
                throw;
            }
            #endregion

            Log("private void CreateCommandFile() 22061002", LoggingLevel.mid);

            //Goes through all selcted dataGridView Rows and saves command files to the Command folder with unique names
            //determined by the Ip Address of the dataGridView Row's Ip cell

            List<DataGridViewRow> SelectedRows = new List<DataGridViewRow>();

            //Adds selected dataGridViewRows to the SelectedRows SelectedRows list
            foreach (DataGridViewRow row in dataGridView.SelectedRows)
            {
                if (!SelectedRows.Contains(row) && !String.IsNullOrEmpty(row.Cells["ClIp"].Value as string))
                    SelectedRows.Add(row);
            }

            //Adds the selected dataGridViewRows that contain selected dataGridViewCells to the SelectedRows list
            foreach (DataGridViewCell Cell in dataGridView.SelectedCells)
            {
                if (!SelectedRows.Contains(Cell.OwningRow) && !String.IsNullOrEmpty(Cell.OwningRow.Cells["ClIp"].Value as string))
                    SelectedRows.Add(Cell.OwningRow);
            }

            //Gets the path for the selected template
            String TemplateFilePath = @"Templates\" + listBoxTemplates.SelectedItem.ToString();

            //If the selected Template File does not exist:
            if(!File.Exists(TemplateFilePath))
            {
                MessageBox("The File \"" + listBoxTemplates.SelectedItem.ToString() + "\" does not currently exist. \n\nEnsure the File was not deleted from the Local Templates Folder.\n\n\n\n", "Template File Does Not Exist");
                return;
            }

            int NumberOfFilesCreated = 0;
            string AllFileNames = "";

            foreach (DataGridViewRow currentRow in SelectedRows)
            {
                if (currentRow.Index >= _GDVI.Count)
                    return;

                String[] IpSplit = _GDVI[currentRow.Index].ip.Split('.');
                String DestinationFile = IpSplit.LastOrDefault();

                if (int.TryParse(DestinationFile, out int DestinationFileInt))
                {
                    DestinationFileInt = Math.Abs(DestinationFileInt); 

                    if (DestinationFileInt < 10)
                        DestinationFile = "00" + DestinationFile;

                    else if (DestinationFileInt < 100)
                        DestinationFile = "0" + DestinationFile;
                }

                //Copies Template file to the Destination file, overwrites if it exists
                File.Copy(TemplateFilePath, DefaultRemoteFolder + DestinationFile + ".txt", true);

                if(File.Exists(DefaultRemoteFolder + DestinationFile + ".txt"))
                    NumberOfFilesCreated++;

                AllFileNames += "\n" + DestinationFile;
            }

            MessageBox(NumberOfFilesCreated + " files were created and saved to:\n" + DefaultRemoteFolder + "\n\nThe files were named: " + AllFileNames + "\n\n\n", "Desktop Setup Successful File Creation");

            Log("Leaving  \"private void CreateCommandFile()\"", LoggingLevel.mid);
        }


        /// <summary>
        /// Adds Command Template Files from Folder
        /// </summary>
        private void AddTemplates()
        {
            if (!Directory.Exists(@"Templates"))
            {
                Directory.CreateDirectory(@"Templates");
                return;
            }

            //Iterates through all files in the "Templates" directory to the listBoxTemplates
            foreach (FileInfo file in new DirectoryInfo(@"Templates").GetFiles())
            {
                listBoxTemplates.Items.Add(file.Name);
            }
        }
    }
}