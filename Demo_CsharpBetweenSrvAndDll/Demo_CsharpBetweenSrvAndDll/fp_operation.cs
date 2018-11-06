using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interop.Fm220api;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

using Newtonsoft.Json.Linq;
using System.ComponentModel;

namespace Demo_CsharpBetweenSrvAndDll
{
    public class fp_operation
    {
        public fm220api api = new fm220api();
        public int m_hConnect, m_hFPCapture, m_hFPImage, m_hEnrlSet;
        public byte[] EncryptedMinutiae = new byte[512];
        public byte[] EncryptedSessionKey = new byte[256];
        public byte[] EncryptedDeleteData = new byte[256];
        int[] pEncryptedLen = new int[1];
        public byte[] piv = new byte[16];
        public byte[] minu_code1 = new byte[512];
        public byte[] minu_code2 = new byte[512];

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
        public int Get_EnrollEx_Encrypted( bool save_img, PictureBox pb, RichTextBox status, RichTextBox log)
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
        public int GetEncryptedTemplate(bool save_img, PictureBox pb, RichTextBox status, RichTextBox log)
        {

            int rtn = 0;

            Snap(save_img, pb, status, log);

            rtn = api.FP_GetEncryptedTemplate(m_hConnect, ref EncryptedMinutiae[0], 1, 0, ref piv[0], ref EncryptedSessionKey[0]);

            ui.AddLog(log, "FP_GetEncryptedTemplate = " + rtn.ToString());

            return variable.OK;
        }

        public int GetDeleteData(bool save_img, PictureBox pb, RichTextBox status, RichTextBox log, string User_Id, int fp_idx)
        {
            int rtn = 0;
            byte[] UserId = new byte[256] ;
            UserId = Encoding.ASCII.GetBytes(User_Id);
            if (UserId.Length > 256)
            {
                return -1;
            }

            rtn = api.FP_GetDeleteData(m_hConnect, ref UserId[0], fp_idx, ref EncryptedDeleteData[0], ref pEncryptedLen[0]);

            ui.AddLog(log, "FP_GetEncryptedTemplate = " + rtn.ToString());

            return variable.FAIL;
        }

        public int Enroll_to_File(bool save_img, PictureBox pb, RichTextBox status, RichTextBox log)
        {
            byte reserved = 0;
            int rtn = 0, Enrl_count;
            byte[] filename = new byte[512];

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

                //rtn = api.FP_EnrollEx_Encrypted(m_hConnect, m_hEnrlSet, ref EncryptedMinutiae[0], 1, ref piv[0], ref EncryptedSessionKey[0]); //EncryptedEnrolledMinutiae is from AES 256, EncryptedSessionKey is from RSA
                rtn = api.FP_GetTemplate(m_hConnect, ref minu_code1[0], variable.mode, variable.key);
                rtn = api.FP_EnrollEx(m_hConnect, m_hEnrlSet, ref minu_code1[0], ref minu_code2[0], variable.mode);
                if (rtn == 65)  //class A
                {
                    ui.AddLog(log, "Local enroll Success!");
                    filename = System.Text.Encoding.Default.GetBytes(variable.ist_filename);
                    rtn = api.FP_SaveISOminutia(m_hConnect, ref filename[0], ref minu_code2[0]);
                    rtn = api.FP_DestroyEnrollHandle(m_hConnect, m_hEnrlSet);

                    return variable.OK;
                }
            }

            ui.AddLog(log, "Local enroll Fail !");
            ui.ShowStatus(status, "Enroll Fail!, return = " + rtn.ToString());
            api.FP_DestroyEnrollHandle(m_hConnect, m_hEnrlSet);

            return variable.FAIL;
        }

        public int FM220_Match_to_File(bool save_img, PictureBox pb, RichTextBox status, RichTextBox log)
        {
            int rtn = 0;
            byte[] filename = new byte[512];
            int[] score = new int[1];

            try
            {
                //load template from enroll.ist or snap.ist
                filename = System.Text.Encoding.Default.GetBytes(variable.ist_filename);
                rtn = api.FP_LoadISOminutia(m_hConnect, ref filename[0], ref minu_code1[0]);
                
                if (rtn != 0)
                {
                    ui.ShowStatus(status, "No fingerprint template file exist! \nPlease enroll or snap your fingerprint first!");
                    return variable.FAIL;
                }

                Snap(save_img, pb, status, log);
                rtn = api.FP_GetTemplate(m_hConnect, ref minu_code2[0], variable.mode, variable.key);
                if (rtn != variable.OK)
                {
                    ui.ShowStatus(status, "Get Template fail");
                    return variable.FAIL; ;
                }
                rtn = api.FP_CodeMatchEx(m_hConnect, ref minu_code1[0], ref minu_code2[0], 1, ref score[0]);
                ui.ShowStatus(status, "Matching score " + Convert.ToString(score[0]));
                ui.AddLog(log, "matching score " + Convert.ToString(score[0]));

                return variable.OK;
            }
            catch (Exception ex)
            {
                return variable.FAIL;
            }
           

        }

        public int Enroll_to_MASS_File(bool save_img, PictureBox pb, RichTextBox status, RichTextBox log)
        {
            byte reserved = 0;
            int rtn = 0, Enrl_count;
            byte[] filename = new byte[512];

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



            //image_operation img_op = new image_operation();
            //Bitmap bmp = new Bitmap("rev.bmp");
            //byte[] img_raw = new byte[bmp.Width*bmp.Height];

            //img_op.get_bmp_raw(bmp, out img_raw);

            //rtn = api.FP_ConvertImageToTemplate(m_hConnect, ref img_raw[0], bmp.Width, bmp.Height, ref minu_code1[0], variable.mode, variable.key);


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

                        rtn = api.FP_GetTemplate(m_hConnect, ref minu_code1[0], variable.mode, variable.key);
                        rtn = api.FP_EnrollEx(m_hConnect, m_hEnrlSet, ref minu_code1[0], ref minu_code2[0], variable.mode);
                        if (rtn == 65)  //class A
                        {
                            ui.AddLog(log, "Local enroll Success!");
                            //filename = System.Text.Encoding.Default.GetBytes(variable.ist_filename);
                            //rtn = api.FP_SaveISOminutia(m_hConnect, ref filename[0], ref minu_code2[0]);

                            
                            dynamic lst = new JObject();
                            lst.finger = new JArray() as dynamic;

                            dynamic data = new JObject();
                            data.id = "Seagate";
                            data.fp_idx = 500.ToString();
                            data.mode = variable.mode.ToString();
                            data.minutiea = ByteArrayToString( minu_code2, minu_code2.Length);
                            lst.finger.Add(data);
                            string results = lst.ToString();
                            
                    
                    if (File.Exists(variable.ist_filename))
                    {
                        byte[] file_mem;
                        int file_len;
                        file_len = ByteArrayFromFile(variable.ist_filename, out file_mem);
                    }
                    else
                    {
                        File.Create(variable.ist_filename);
                    }
                    
                    rtn = api.FP_DestroyEnrollHandle(m_hConnect, m_hEnrlSet);

                    return variable.OK;
                }
            }

            ui.AddLog(log, "Local enroll Fail !");
            ui.ShowStatus(status, "Enroll Fail!, return = " + rtn.ToString());
            api.FP_DestroyEnrollHandle(m_hConnect, m_hEnrlSet);

            return variable.FAIL;
        }

        public int Image_Enroll_to_LST(bool save_img, PictureBox pb, RichTextBox status, RichTextBox log, Bitmap bmp)
        {
            byte reserved = 0;
            int rtn = 0;
            byte[] filename = new byte[512];

            image_operation img_op = new image_operation();
            //bmp = new Bitmap("fm220.bmp");
            byte[] raw_buf = new byte[bmp.Width * bmp.Height];
            byte[] raw_buf_to_dll = new byte[bmp.Width * bmp.Height];

            raw_buf = img_op.get_bmp_raw(bmp);
            Array.Copy(raw_buf, raw_buf_to_dll, raw_buf_to_dll.Length);

            rtn = api.FP_ConvertImageToTemplate(m_hConnect, ref raw_buf_to_dll[0], bmp.Width, bmp.Height, ref minu_code2[0], variable.mode, variable.key);


            if (rtn == variable.OK)
            {
                filename = System.Text.Encoding.Default.GetBytes(variable.ist_filename);
                rtn = api.FP_SaveISOminutia(m_hConnect, ref filename[0], ref minu_code2[0]);
                ui.AddLog(log, "Local enroll Success!");
                return variable.OK;
            }
            else
            {
                ui.AddLog(log, "Local enroll Fail !");
                ui.ShowStatus(status, "Enroll Fail!, return = " + rtn.ToString());
                return variable.FAIL;
            }
        }

        public int Image_Match_to_LST(bool save_img, PictureBox pb, RichTextBox status, RichTextBox log, Bitmap bmp)
        {
            int rtn = 0;
            byte[] filename = new byte[512];
            int[] score = new int[1];

            try
            {
                //load template from enroll.ist or snap.ist
                filename = System.Text.Encoding.Default.GetBytes(variable.ist_filename);
                rtn = api.FP_LoadISOminutia(m_hConnect, ref filename[0], ref minu_code1[0]);

                if (rtn != 0)
                {
                    ui.ShowStatus(status, "No fingerprint template file exist! \nPlease enroll or snap your fingerprint first!");
                    return variable.FAIL;
                }
                else
                {
                    byte[] raw_buf = new byte[bmp.Width * bmp.Height];
                    byte[] raw_buf_to_dll = new byte[bmp.Width * bmp.Height];

                    image_operation img_op = new image_operation(); raw_buf = img_op.get_bmp_raw(bmp);
                    Array.Copy(raw_buf, raw_buf_to_dll, raw_buf_to_dll.Length);

                    rtn = api.FP_ConvertImageToTemplate(m_hConnect, ref raw_buf_to_dll[0], bmp.Width, bmp.Height, ref minu_code2[0], variable.mode, variable.key);
                    rtn = api.FP_CodeMatchEx(m_hConnect, ref minu_code1[0], ref minu_code2[0], 1, ref score[0]);
                    ui.ShowStatus(status, "Matching score " + Convert.ToString(score[0]));
                    ui.AddLog(log, "matching score " + Convert.ToString(score[0]));
                    return variable.OK;
                }

                
            }
            catch (Exception ex)
            {
                return variable.FAIL;
            }


        }

        public bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int ByteArrayFromFile(string fileName, out byte[] byteArray)
        {
            int len = 0;

            byteArray = File.ReadAllBytes(fileName);
            len = byteArray.Length;
               
            return len;
        }

        public static string ByteArrayToString(byte[] ba, int len)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 3);

            for (int i = 0; i < len; i++)
            {
                hex.AppendFormat("{0:x2}" + " ", ba[i]);
            }

            return hex.ToString();
        }

    }
}
