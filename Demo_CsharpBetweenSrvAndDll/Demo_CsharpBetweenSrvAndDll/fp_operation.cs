using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interop.Fm220api;
using System.Windows.Forms;
using System.Drawing;

namespace Demo_CsharpBetweenSrvAndDll
{
    class fp_operation
    {
        public fm220api api = new fm220api();
        public int m_hConnect, m_hFPCapture, m_hFPImage, m_hEnrlSet;
        public byte[] EncryptedMinutiae = new byte[512];
        public byte[] EncryptedSessionKey = new byte[256];
        public byte[] EncryptedDeleteData = new byte[256];
        int[] pEncryptedLen = new int[1];
        public byte[] piv = new byte[16];

        public int DeviceConnect(RichTextBox log)
        {
            m_hConnect = api.FP_ConnectCaptureDriver(0);
            if (m_hConnect != 0)
                ui.AddLog(log, "Connect FM220 succeess.");
            else
                ui.AddLog(log, "Connect FM220 fail!");

            return m_hConnect;
        }

        public void DeviceDisconnect(RichTextBox log)
        {
            api.FP_DisconnectCaptureDriver(m_hConnect);
            ui.AddLog(log, "Disonnect FM220. ");

        }
        public int Enroll( bool save_img, PictureBox pb, RichTextBox status, RichTextBox log)
        {
            byte reserved = 0;
            int rtn = 0, Enrl_count;

            m_hEnrlSet = api.FP_CreateEnrollHandle(m_hConnect, reserved);
            if (m_hEnrlSet == 0)
            {
                ui.AddLog(log, "Create Enroll fail !");
                return variable.FAIL;
            }
            else
            {
                ui.AddLog(log, "CreateEnroll Success. ");
            }


            for (Enrl_count = 0; Enrl_count < 3; Enrl_count++)
            {
                int i = 0;
                while ((rtn = api.FP_CheckBlank(m_hConnect)) != variable.OK)
                {
                    if (i >= variable.running_symble.Length)
                    {
                        i = 0;
                    }

                    ui.ShowStatus(status, "Put finger OFF reader " + variable.running_symble[(i % variable.running_symble.Length)] + " ↑");
                    i++;
                }

                Snap(save_img, pb, status, log);

                rtn = api.FP_EnrollEx_Encrypted(m_hConnect, m_hEnrlSet, ref EncryptedMinutiae[0], 1, ref piv[0], ref EncryptedSessionKey[0]); //EncryptedEnrolledMinutiae is from AES 256, EncryptedSessionKey is from RSA
                if (rtn == 65)  //class A
                {
                    ui.AddLog(log, "Dll get EncryptedMinutiae Success!");
                    api.FP_DestroyEnrollHandle(m_hConnect, m_hEnrlSet);

                    return variable.OK;
                }
            }

            ui.AddLog(log, "Dll get EncryptedMinutiae Fail !");
            ui.ShowStatus(status, "Enroll Fail!, return = " + rtn.ToString());

            api.FP_DestroyEnrollHandle(m_hConnect, m_hEnrlSet);

            return variable.FAIL;
        }

        public int Snap(bool save_img, PictureBox pb, RichTextBox status, RichTextBox log)
        {
            Graphics g = pb.CreateGraphics();
            IntPtr hDC = g.GetHdc();
            int rtn;
#if FM300
            int fpImgQ;
#endif
            ui.AddLog(log, "Snap start.");
            m_hFPCapture = api.FP_CreateCaptureHandle(m_hConnect);
            m_hFPImage = api.FP_CreateImageHandle(m_hConnect, variable.GRAY_IMAGE, variable.LARGE);

            int i = 0;
            while (api.FP_Capture(m_hConnect, m_hFPCapture) != variable.OK)
            {
#if FM300
                fpImgQ = FM220api.FP_GetImageQuality(m_hConnect);
#endif
                rtn = api.FP_GetImage(m_hConnect, m_hFPImage);
                rtn = api.FP_DisplayImage(m_hConnect, (int)hDC, m_hFPImage, 0, 0, variable.img_w, variable.img_h);
#if FM300
                if (fpImgQ <= 2)
                    break;
#endif

                if (i >= variable.running_symble.Length)
                {
                    i = 0;
                }

                ui.ShowStatus(status, "Put finger ON reader" + variable.running_symble[(i % variable.running_symble.Length)] + " ↓");
                i++;
            }
            rtn = api.FP_GetImage(m_hConnect, m_hFPImage);
            rtn = api.FP_DisplayImage(m_hConnect, (int)hDC, m_hFPImage, 0, 0, variable.img_w, variable.img_h);
            ui.AddLog(log, "Fingerprint Capture Success !");

            if (save_img == true)
            {
                rtn = api.FP_SaveImage(m_hConnect, m_hFPImage, variable.BMP, "temp.bmp");
                ui.AddLog(log, "Fingerprint save as temp.bmp OK !");
            }

            rtn = api.FP_DestroyCaptureHandle(m_hConnect, m_hFPCapture);
            rtn = api.FP_DestroyImageHandle(m_hConnect, m_hFPImage);

            return variable.OK;
        }
        public int Verify(bool save_img, PictureBox pb, RichTextBox status, RichTextBox log)
        {

            int rtn = 0;

            Snap(save_img, pb, status, log);

            rtn = api.FP_GetEncryptedTemplate(m_hConnect, ref EncryptedMinutiae[0], 1, 0, ref piv[0], ref EncryptedSessionKey[0]);

            ui.AddLog(log, "FP_GetEncryptedTemplate = " + rtn.ToString());

            return variable.OK;
        }

        public int Identify(bool save_img, PictureBox pb, RichTextBox status, RichTextBox log)
        {
            int rtn = 0;

            Snap(save_img, pb, status, log);

            rtn = api.FP_GetEncryptedTemplate(m_hConnect, ref EncryptedMinutiae[0], 1, 0, ref piv[0], ref EncryptedSessionKey[0]);

            ui.AddLog(log, "FP_GetEncryptedTemplate = " + rtn.ToString());

            return variable.OK;
        }

        public int Delete(bool save_img, PictureBox pb, RichTextBox status, RichTextBox log, string User_Id, int fp_idx)
        {

            int rtn = 0;
            byte[] UserId = new byte[256] ;
            UserId = Encoding.ASCII.GetBytes(User_Id);
            if (UserId.Length > 256)
            {
                return -1;
            }

            rtn = api.FP_GetDeleteData(m_hConnect, ref UserId[0], fp_idx, ref EncryptedDeleteData[0], ref pEncryptedLen[0]);

            ui.AddLog(log, "FP_GetDeleteData = " + rtn.ToString());

            return variable.OK;
        }

    }
}
