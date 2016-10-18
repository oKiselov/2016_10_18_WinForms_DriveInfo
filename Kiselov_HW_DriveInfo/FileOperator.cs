using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Kiselov_HW_DriveInfo
{
    class FileOperator
    {
        // available drive-devices counter 
        private int _intDriveCounter;

        public FileOperator()
        {
            _intDriveCounter = 0;
        }

        /// <summary>
        /// Method which search your directory for txt files 
        /// Returns collection of pathes to such files 
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public List<string> TxtBrowser(string strPath)
        {
            List<string> listTxt =
                new List<string>(Directory.EnumerateFiles(strPath, "*.txt", SearchOption.TopDirectoryOnly));
            listTxt.Insert(0, DateTime.Now.ToString());
            _intDriveCounter++;
            return listTxt;
        }

        /// <summary>
        /// Method writes all pathes of txt files to file TextFilesLog.txt
        /// </summary>
        /// <param name="listTxt"></param>
        public void ToFile(List<string> listTxt)
        {
            using (FileStream fs = new FileStream("TextFilesLog.txt", FileMode.Append, FileAccess.Write))
            {
                for (int i = 0; i < listTxt.Count; i++)
                {
                    byte[] arrBytes = Encoding.Unicode.GetBytes(listTxt[i] += "\r\n");
                    fs.Write(arrBytes, 0, arrBytes.Length);
                }
            }
        }

        /// <summary>
        /// Method reads all pathes of txt files from the file TextFilesLog.txt
        /// </summary>
        /// <param name="strPath"></param>
        public void FromFile(string strPath = "TextFilesLog.txt")
        {
            byte[] arrBytesTemp = new byte[4096];
            using (FileStream fs = new FileStream(strPath, FileMode.Open, FileAccess.Read))
            {
                int iCount = 0;
                while (iCount < fs.Length)
                {
                    int iRead = fs.Read(arrBytesTemp, 0, arrBytesTemp.Length);
                    if (iRead == 0)
                    {
                        break;
                    }
                    if (iRead != 4096)
                    {
                        Array.Resize(ref arrBytesTemp, iRead);
                    }
                    string strTemp = Encoding.Unicode.GetString(arrBytesTemp);
                    Console.WriteLine(strTemp);
                    iCount += (int)iRead;
                }
            }
        }

        public int DriveCounter
        {
            get { return _intDriveCounter; }
        }
    }
}