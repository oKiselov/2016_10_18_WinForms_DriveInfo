using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kiselov_HW_DriveInfo
{
    public partial class Form1 : Form
    {
        // object for operations with txt files 
        private FileOperator _fileOperator;
        // list of DriveInfo about current state of devices 
        private List<DriveInfo> _listOfDriveInfo;

        public Form1()
        {
            InitializeComponent();

            // initialization of object 
            _fileOperator = new FileOperator();

            // checking all removable devices 
            _listOfDriveInfo = new List<DriveInfo>(DriveInfo.GetDrives())
                .Where(drive => drive.DriveType == DriveType.Removable).ToList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// button Start 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText("Start!");
        }

        /// <summary>
        ///  button Stop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// overriding of windows operation 
        /// handling messages 
        /// </summary>
        /// <param name="msg"></param>
        protected override void WndProc(ref Message msg)
        {
            List<DriveInfo> listDriveTemp = new List<DriveInfo>();
            const int WM_DEVICECHANGE = 0x0219;
            const int ADD_DEVICE = 0x8000;
            const int REMOVE_DEVICE = 0x8004;
            if (msg.Msg == WM_DEVICECHANGE)
            {
                switch ((int)msg.WParam)
                {
                    case ADD_DEVICE:
                        // check that disc is removable 
                        listDriveTemp = new List<DriveInfo>(DriveInfo.GetDrives())
                            .Where(drive =>drive.DriveType==DriveType.Removable).ToList();
                        // check that amount of removable discs has been changed 
                        if (listDriveTemp.Count > _listOfDriveInfo.Count)
                        {
                            // search what disc is new 
                            for (int i = 0; i < listDriveTemp.Count; i++)
                            {
                                if (!_listOfDriveInfo.Any(drive => drive.Name == listDriveTemp[i].Name))
                                {
                                    _listOfDriveInfo.Add(listDriveTemp[i]);
                                }
                            }

                            DriveInfo targetDrive = _listOfDriveInfo[_listOfDriveInfo.Count - 1];
                            // check that disc is ready for writing 
                            if (targetDrive.IsReady)
                            {
                                List<string> listTmp = _fileOperator.TxtBrowser(targetDrive.Name);
                                _fileOperator.ToFile(listTmp);
                                // output to the text box 
                                foreach (var VARIABLE in listTmp)
                                {
                                    richTextBox1.AppendText(VARIABLE);
                                }
                                
                            }
                        }
                        break;

                    case REMOVE_DEVICE:
                        // set new list of discs after remove 
                        _listOfDriveInfo = new List<DriveInfo>(DriveInfo.GetDrives())
                            .Where(drive =>drive.DriveType==DriveType.Removable).ToList();                     

                        break;
                    default:
                        break;
                }
            }
            // transfer to main function 
            base.WndProc(ref msg);
        }

        /// <summary>
        /// TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
