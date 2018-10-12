using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interop.Fm220api;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;

namespace Demo_CsharpBetweenSrvAndDll
{
    class job
    {
        public BackgroundWorker BGW_Enroll = new BackgroundWorker();
        public void Job_Enroll(List<object> arguments)
        {
            BGW_Enroll = new BackgroundWorker();
            BGW_Enroll.DoWork += new DoWorkEventHandler(BGW_Enroll_DoWork);
            BGW_Enroll.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_Enroll_Completed);
            BGW_Enroll.ProgressChanged += new ProgressChangedEventHandler(BGW_Enroll_ProgressChanged);
            BGW_Enroll.WorkerReportsProgress = true;
            BGW_Enroll.WorkerSupportsCancellation = true;
            BGW_Enroll.RunWorkerAsync(arguments);
        }

        private static void BGW_Enroll_DoWork(object sender, DoWorkEventArgs e)
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

            ui.ClearLog(rtb_log);

            //ask API.dll to provide finger data
            fp_operation fp = new fp_operation();
            fp.DeviceConnect(rtb_log);
            fp.Enroll(save_img, pb, rtb_status, rtb_log);
            fp.DeviceDisconnect(rtb_log);

            string id = ui.ReadText(rtb_id);
            int fp_idx;
            int.TryParse(ui.ReadComboBox(cb), out fp_idx);

            int privilege;
            int.TryParse(ui.ReadText(rtb_privilege), out privilege);

            //feed finger data to to remote "redirect server"
            srv_operation srv = new srv_operation();
            string encMinutiae = BitConverter.ToString(fp.EncryptedMinutiae).Replace("-", "");
            string eSkey = BitConverter.ToString(fp.EncryptedSessionKey).Replace("-", "");
            string iv = BitConverter.ToString(fp.piv).Replace("-", "");

            string json_str = srv.BuildJson_Enroll(encMinutiae, eSkey, iv, id, fp_idx, privilege);

            bool https_en = ui.ReadCheckBox(chk_https);
            string ip = ui.ReadText(rtb_ip);
            string port = ui.ReadText(rtb_port);
            string srv_rtn = srv.Srv_Enroll(json_str, https_en, ip, port);

            ui.AddLog(rtb_log, "Send server = " + json_str);
            ui.AddLog(rtb_log, "server return = " + srv_rtn);
            ui.ShowStatus(rtb_status, "Enroll finished");

        }

        private static void BGW_Enroll_Completed(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private static void BGW_Enroll_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        public BackgroundWorker BGW_Verify = new BackgroundWorker();
        public void Job_Verify(List<object> arguments)
        {
            BGW_Verify = new BackgroundWorker();
            BGW_Verify.DoWork += new DoWorkEventHandler(BGW_Verify_DoWork);
            BGW_Verify.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_Verify_Completed);
            BGW_Verify.ProgressChanged += new ProgressChangedEventHandler(BGW_Verify_ProgressChanged);
            BGW_Verify.WorkerReportsProgress = true;
            BGW_Verify.WorkerSupportsCancellation = true;
            BGW_Verify.RunWorkerAsync(arguments);
        }

        private static void BGW_Verify_DoWork(object sender, DoWorkEventArgs e)
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

            ui.ClearLog(rtb_log);

            //ask API.dll to provide finger data
            fp_operation fp = new fp_operation();
            fp.DeviceConnect(rtb_log);
            fp.Verify(save_img, pb, rtb_status, rtb_log);
            fp.DeviceDisconnect(rtb_log);

            string id = ui.ReadText(rtb_id);
            int fp_idx;
            int.TryParse(ui.ReadComboBox(cb), out fp_idx);

            int privilege;
            int.TryParse(ui.ReadText(rtb_privilege), out privilege);

            //feed finger data to webapi(to remote redirect server)
            srv_operation srv = new srv_operation();
            string encMinutiae = BitConverter.ToString(fp.EncryptedMinutiae).Replace("-", "");
            string eSkey = BitConverter.ToString(fp.EncryptedSessionKey).Replace("-", "");
            string iv = BitConverter.ToString(fp.piv).Replace("-", "");

            string json_str = srv.BuildJson_Verify(encMinutiae, eSkey, iv, id, fp_idx, privilege);

            bool https_en = ui.ReadCheckBox(chk_https);
            string ip = ui.ReadText(rtb_ip);
            string port = ui.ReadText(rtb_port);
            string srv_rtn = srv.Srv_Verify(json_str, https_en, ip, port);

            ui.AddLog(rtb_log, "Send server = " + json_str);
            ui.AddLog(rtb_log, "server return = " + srv_rtn);
            ui.ShowStatus(rtb_status, "Verify finished");
        }

        private static void BGW_Verify_Completed(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private static void BGW_Verify_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        public BackgroundWorker BGW_Identify = new BackgroundWorker();
        public void Job_Identify(List<object> arguments)
        {
            BGW_Identify = new BackgroundWorker();
            BGW_Identify.DoWork += new DoWorkEventHandler(BGW_Identify_DoWork);
            BGW_Identify.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_Identify_Completed);
            BGW_Identify.ProgressChanged += new ProgressChangedEventHandler(BGW_Identify_ProgressChanged);
            BGW_Identify.WorkerReportsProgress = true;
            BGW_Identify.WorkerSupportsCancellation = true;
            BGW_Identify.RunWorkerAsync(arguments);
        }

        private static void BGW_Identify_DoWork(object sender, DoWorkEventArgs e)
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

            ui.ClearLog(rtb_log);

            //ask API.dll to provide finger data
            fp_operation fp = new fp_operation();
            fp.DeviceConnect(rtb_log);
            fp.Identify(save_img, pb, rtb_status, rtb_log);
            fp.DeviceDisconnect(rtb_log);

            string id = ui.ReadText(rtb_id);
            int fp_idx;
            int.TryParse(ui.ReadComboBox(cb), out fp_idx);

            int privilege;
            int.TryParse(ui.ReadText(rtb_privilege), out privilege);

            //feed finger data to webapi(to remote redirect server)
            srv_operation srv = new srv_operation();
            string encMinutiae = BitConverter.ToString(fp.EncryptedMinutiae).Replace("-", "");
            string eSkey = BitConverter.ToString(fp.EncryptedSessionKey).Replace("-", "");
            string iv = BitConverter.ToString(fp.piv).Replace("-", "");

            string json_str = srv.BuildJson_Identify(encMinutiae, eSkey, iv);

            bool https_en = ui.ReadCheckBox(chk_https);
            string ip = ui.ReadText(rtb_ip);
            string port = ui.ReadText(rtb_port);
            string srv_rtn = srv.Srv_Identify(json_str, https_en, ip, port);

            ui.AddLog(rtb_log, "Send server = " + json_str);
            ui.AddLog(rtb_log, "Server return = " + srv_rtn);
            ui.ShowStatus(rtb_status, "Identify finished");
        }

        private static void BGW_Identify_Completed(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private static void BGW_Identify_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        public BackgroundWorker BGW_Delete = new BackgroundWorker();
        public void Job_Delete(List<object> arguments)
        {
            BGW_Delete = new BackgroundWorker();
            BGW_Delete.DoWork += new DoWorkEventHandler(BGW_Delete_DoWork);
            BGW_Delete.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_Delete_Completed);
            BGW_Delete.ProgressChanged += new ProgressChangedEventHandler(BGW_Delete_ProgressChanged);
            BGW_Delete.WorkerReportsProgress = true;
            BGW_Delete.WorkerSupportsCancellation = true;
            BGW_Delete.RunWorkerAsync(arguments);
        }

        private static void BGW_Delete_DoWork(object sender, DoWorkEventArgs e)
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

            ui.ClearLog(rtb_log);

            string id = ui.ReadText(rtb_id);
            int fp_idx;
            int.TryParse(ui.ReadComboBox(cb), out fp_idx);

            //ask API.dll to provide finger data
            fp_operation fp = new fp_operation();
            fp.DeviceConnect(rtb_log);
            fp.Delete(save_img, pb, rtb_status, rtb_log, id, fp_idx);
            fp.DeviceDisconnect(rtb_log);

            int privilege;
            int.TryParse(ui.ReadText(rtb_privilege), out privilege);

            //feed finger data to webapi(to remote redirect server)
            srv_operation srv = new srv_operation();
            string encDeleteData = BitConverter.ToString(fp.EncryptedDeleteData).Replace("-", "");

            string json_str = srv.BuildJson_Delete(id, encDeleteData);

            bool https_en = ui.ReadCheckBox(chk_https);
            string ip = ui.ReadText(rtb_ip);
            string port = ui.ReadText(rtb_port);
            string srv_rtn = srv.Srv_Delete(json_str, https_en, ip, port);

            ui.AddLog(rtb_log, "Send server = " + json_str);
            ui.AddLog(rtb_log, "server return = " + srv_rtn);
            ui.ShowStatus(rtb_status, "Delete finished");
        }

        private static void BGW_Delete_Completed(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private static void BGW_Delete_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }
    }
}
