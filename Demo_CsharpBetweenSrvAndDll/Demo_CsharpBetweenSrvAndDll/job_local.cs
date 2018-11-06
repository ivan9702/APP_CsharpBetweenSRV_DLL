using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interop.Fm220api;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace Demo_CsharpBetweenSrvAndDll
{
    class job_local
    {
        public BackgroundWorker BGW_File_Enroll = new BackgroundWorker();
        public void Job_File_Enroll(List<object> arguments)
        {
            BGW_File_Enroll.DoWork += new DoWorkEventHandler(BGW_File_Enroll_DoWork);
            BGW_File_Enroll.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_File_Enroll_Completed);
            BGW_File_Enroll.ProgressChanged += new ProgressChangedEventHandler(BGW_File_Enroll_ProgressChanged);
            BGW_File_Enroll.WorkerReportsProgress = true;
            BGW_File_Enroll.WorkerSupportsCancellation = true;
            BGW_File_Enroll.RunWorkerAsync(arguments);
        }

        private static void BGW_File_Enroll_DoWork(object sender, DoWorkEventArgs e)
        {
            // get arguments from list
            List<object> genericlist = e.Argument as List<object>;
            RichTextBox rtb_status = (RichTextBox)genericlist[0];
            RichTextBox rtb_log = (RichTextBox)genericlist[1];
            PictureBox pb = (PictureBox)genericlist[2];
            RichTextBox rtb_id = (RichTextBox)genericlist[3];
            ComboBox cb = (ComboBox)genericlist[4];
            CheckBox chk = (CheckBox)genericlist[5];
            RichTextBox rtb_privilege = (RichTextBox)genericlist[6];
            bool save_img = ui.ReadCheckBox(chk);

            CheckBox chk_https = (CheckBox)genericlist[7];
            RichTextBox rtb_ip = (RichTextBox)genericlist[8];
            RichTextBox rtb_port = (RichTextBox)genericlist[9];

            string filename = (string)genericlist[10];
            ui.ClearLog(rtb_log);

            ui.AddLog(rtb_log, "select file path=" + filename);
            Bitmap img = new Bitmap(filename);

            ui.ShowImage(pb, img);

            if (save_img == true)
            {
                Bitmap clone = img.Clone(new Rectangle(0, 0, img.Width, img.Height), img.PixelFormat);
                clone.Save("temp.bmp");
                clone.Dispose();
            }

            fp_operation fp = new fp_operation();
            int connect = fp.DeviceConnect(rtb_log);
            if (connect != 0)
            {
                fp.Image_Enroll_to_LST(save_img, pb, rtb_status, rtb_log, img);
                fp.DeviceDisconnect(rtb_log);
                ui.ShowStatus(rtb_status, "Enroll finished");
            }

        }

        private static void BGW_File_Enroll_Completed(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private static void BGW_File_Enroll_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        public BackgroundWorker BGW_File_Match = new BackgroundWorker();
        public void Job_File_Match(List<object> arguments)
        {
            BGW_File_Match.DoWork += new DoWorkEventHandler(BGW_File_Match_DoWork);
            BGW_File_Match.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_File_Match_Completed);
            BGW_File_Match.ProgressChanged += new ProgressChangedEventHandler(BGW_File_Match_ProgressChanged);
            BGW_File_Match.WorkerReportsProgress = true;
            BGW_File_Match.WorkerSupportsCancellation = true;
            BGW_File_Match.RunWorkerAsync(arguments);
        }

        private static void BGW_File_Match_DoWork(object sender, DoWorkEventArgs e)
        {
            // get arguments from list
            List<object> genericlist = e.Argument as List<object>;
            RichTextBox rtb_status = (RichTextBox)genericlist[0];
            RichTextBox rtb_log = (RichTextBox)genericlist[1];
            PictureBox pb = (PictureBox)genericlist[2];
            RichTextBox rtb_id = (RichTextBox)genericlist[3];
            ComboBox cb = (ComboBox)genericlist[4];
            CheckBox chk = (CheckBox)genericlist[5];
            RichTextBox rtb_privilege = (RichTextBox)genericlist[6];
            bool save_img = ui.ReadCheckBox(chk);

            CheckBox chk_https = (CheckBox)genericlist[7];
            RichTextBox rtb_ip = (RichTextBox)genericlist[8];
            RichTextBox rtb_port = (RichTextBox)genericlist[9];

            string filename = (string)genericlist[10];

            ui.ClearLog(rtb_log);

            ui.AddLog(rtb_log, "select file path=" + filename);
            Bitmap img = new Bitmap(filename);


            ui.ShowImage(pb, img);

            if (save_img == true)
            {
                Bitmap clone = img.Clone(new Rectangle(0, 0, img.Width, img.Height), img.PixelFormat);
                clone.Save("temp.bmp");
                clone.Dispose();
            }


            fp_operation fp = new fp_operation();
            int connect = fp.DeviceConnect(rtb_log);
            if (connect != 0)
            {
                fp.Image_Match_to_LST(save_img, pb, rtb_status, rtb_log, img);
                fp.DeviceDisconnect(rtb_log);
            }

            //ui.ShowStatus(rtb_status, "Match finished");
        }

        private static void BGW_File_Match_Completed(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private static void BGW_File_Match_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        /*
        public BackgroundWorker BGW_SuperC_Enroll = new BackgroundWorker();
        public void Job_SuperC_Enroll(List<object> arguments)
        {
            BGW_SuperC_Enroll.DoWork += new DoWorkEventHandler(BGW_SuperC_Enroll_DoWork);
            BGW_SuperC_Enroll.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_SuperC_Enroll_Completed);
            BGW_SuperC_Enroll.ProgressChanged += new ProgressChangedEventHandler(BGW_SuperC_Enroll_ProgressChanged);
            BGW_SuperC_Enroll.WorkerReportsProgress = true;
            BGW_SuperC_Enroll.WorkerSupportsCancellation = true;
            BGW_SuperC_Enroll.RunWorkerAsync(arguments);
        }

        private static void BGW_SuperC_Enroll_DoWork(object sender, DoWorkEventArgs e)
        {
            // get arguments from list
            List<object> genericlist = e.Argument as List<object>;
            RichTextBox rtb_status = (RichTextBox)genericlist[0];
            RichTextBox rtb_log = (RichTextBox)genericlist[1];
            PictureBox pb = (PictureBox)genericlist[2];
            RichTextBox rtb_id = (RichTextBox)genericlist[3];
            ComboBox cb = (ComboBox)genericlist[4];
            CheckBox chk = (CheckBox)genericlist[5];
            RichTextBox rtb_privilege = (RichTextBox)genericlist[6];
            bool save_img = ui.ReadCheckBox(chk);

            CheckBox chk_https = (CheckBox)genericlist[7];
            RichTextBox rtb_ip = (RichTextBox)genericlist[8];
            RichTextBox rtb_port = (RichTextBox)genericlist[9];
            superc SuperC = (superc)genericlist[10];

            ui.ClearLog(rtb_log);

            Bitmap img = SuperC.SuperC_v2_Get_Image();

            ui.ShowImage(pb, img);

            if (save_img == true)
            {
                Bitmap clone = img.Clone(new Rectangle(0, 0, img.Width, img.Height), img.PixelFormat);
                clone.Save("temp.bmp");
                clone.Dispose();
            }

            fp_operation fp = new fp_operation();
            int connect = fp.DeviceConnect(rtb_log);
            if (connect != 0)
            {
                fp.Image_Enroll_to_LST(save_img, pb, rtb_status, rtb_log, img);
                fp.DeviceDisconnect(rtb_log);
                ui.ShowStatus(rtb_status, "Enroll finished");
            }

        }

        private static void BGW_SuperC_Enroll_Completed(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private static void BGW_SuperC_Enroll_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        public BackgroundWorker BGW_SuperC_Match = new BackgroundWorker();
        public void Job_SuperC_Match(List<object> arguments)
        {
            BGW_SuperC_Match.DoWork += new DoWorkEventHandler(BGW_SuperC_Match_DoWork);
            BGW_SuperC_Match.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_SuperC_Match_Completed);
            BGW_SuperC_Match.ProgressChanged += new ProgressChangedEventHandler(BGW_SuperC_Match_ProgressChanged);
            BGW_SuperC_Match.WorkerReportsProgress = true;
            BGW_SuperC_Match.WorkerSupportsCancellation = true;
            BGW_SuperC_Match.RunWorkerAsync(arguments);
        }

        private static void BGW_SuperC_Match_DoWork(object sender, DoWorkEventArgs e)
        {
            // get arguments from list
            List<object> genericlist = e.Argument as List<object>;
            RichTextBox rtb_status = (RichTextBox)genericlist[0];
            RichTextBox rtb_log = (RichTextBox)genericlist[1];
            PictureBox pb = (PictureBox)genericlist[2];
            RichTextBox rtb_id = (RichTextBox)genericlist[3];
            ComboBox cb = (ComboBox)genericlist[4];
            CheckBox chk = (CheckBox)genericlist[5];
            RichTextBox rtb_privilege = (RichTextBox)genericlist[6];
            bool save_img = ui.ReadCheckBox(chk);

            CheckBox chk_https = (CheckBox)genericlist[7];
            RichTextBox rtb_ip = (RichTextBox)genericlist[8];
            RichTextBox rtb_port = (RichTextBox)genericlist[9];

            superc SuperC = (superc)genericlist[10];

            ui.ClearLog(rtb_log);

            Bitmap img = SuperC.SuperC_v2_Get_Image();
            //Bitmap img = new Bitmap("fm220.bmp");

            ui.ShowImage(pb, img);

            if (save_img == true)
            {
                Bitmap clone = img.Clone(new Rectangle(0, 0, img.Width, img.Height), img.PixelFormat);
                clone.Save("temp.bmp");
                clone.Dispose();
            }


            fp_operation fp = new fp_operation();
            int connect = fp.DeviceConnect(rtb_log);
            if(connect != 0)
            {
                fp.Image_Match_to_LST(save_img, pb, rtb_status, rtb_log, img);
                fp.DeviceDisconnect(rtb_log);
            }


            //ui.ShowStatus(rtb_status, "Match finished");
        }

        private static void BGW_SuperC_Match_Completed(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private static void BGW_SuperC_Match_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        */
    }
}
