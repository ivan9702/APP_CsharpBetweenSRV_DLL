using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Demo_CsharpBetweenSrvAndDll
{
    public class ui
    {
        public static void ShowImage(PictureBox pb, Bitmap bmp)
        {
            if (pb.InvokeRequired)
            {
                pb.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        pb.Image = (Image)bmp;
                    }
                    catch (Exception ex)
                    {
                        string log = ex.Message;
                        MessageBox.Show(log);
                    }

                });
                return;
            }
        }

        public static void ShowStatus(RichTextBox tb, string str)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        tb.Text = str;
                    }
                    catch (Exception ex)
                    {
                        string log = ex.Message;
                        MessageBox.Show(log);
                    }

                });
                return;
            }
        }

        public static void AddLog(RichTextBox tb, string str)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        tb.AppendText("\r\n" + str);
                        tb.ScrollToCaret();
                    }
                    catch (Exception ex)
                    {
                        string log = ex.Message;
                        MessageBox.Show(log);
                    }

                });
                return;
            }
        }

        public static void AddColorText(RichTextBox tb, string str, Color color)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        Color ori_color = tb.SelectionColor;
                        tb.SelectionColor = color;

                        tb.AppendText(str);

                        tb.SelectionColor = ori_color;
                        tb.ScrollToCaret();
                    }
                    catch (Exception ex)
                    {
                        string log = ex.Message;
                        MessageBox.Show(log);
                    }

                });
                return;
            }
        }

        public static void ClearLog(RichTextBox tb)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        tb.Clear();
                    }
                    catch (Exception ex)
                    {
                        string log = ex.Message;
                        MessageBox.Show(log);
                    }

                });
                return;
            }
        }

        public static void ClearListBox(ListBox lb)
        {
            if (lb.InvokeRequired)
            {
                lb.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        lb.Items.Clear();
                    }
                    catch (Exception ex)
                    {
                        string log = ex.Message;
                        MessageBox.Show(log);
                    }

                });
                return;
            }
        }

        public static void AddListBox(ListBox lb, string str)
        {
            if (lb.InvokeRequired)
            {
                lb.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        lb.Items.Add(str);
                        lb.SelectedIndex = 0;
                    }
                    catch (Exception ex)
                    {
                        string log = ex.Message;
                        MessageBox.Show(log);
                    }

                });
                return;
            }
        }

        public static void AddLine(RichTextBox tb, string str)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        tb.AppendText(str);

                    }
                    catch (Exception ex)
                    {
                        string log = ex.Message;
                        MessageBox.Show(log);
                    }

                });
                return;
            }
            else
            {
                tb.AppendText(str);
            }
        }
        public static void NewText(Control tb, string str)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        tb.Text = str;

                    }
                    catch (Exception ex)
                    {
                        string log = ex.Message;
                        MessageBox.Show(log);
                    }
                });
                return;
            }
            else
            {
                tb.Text = str;
            }
        }

        public static string ReadText(Control tb)
        {
            string str = "";

            if (tb.InvokeRequired)
            {
                tb.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        str = tb.Text;
                    }
                    catch (Exception ex)
                    {
                        string log = ex.Message;
                        MessageBox.Show(log);
                    }

                });
                return str;
            }
            else
            {
                return tb.Text;
            }
        }

        public static void ClearText(RichTextBox tb)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        tb.Clear();
                    }
                    catch (Exception ex)
                    {
                        string log = ex.Message;
                        MessageBox.Show(log);
                    }

                });
                return;
            }
            else
            {
                tb.Clear();
            }
        }

        public static string ReadComboBox(ComboBox cb)
        {
            string str = "";

            if (cb.InvokeRequired)
            {
                cb.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        str = cb.SelectedValue.ToString();

                    }
                    catch (Exception ex)
                    {
                        string log = ex.Message;
                        MessageBox.Show(log);
                    }

                });
                return str;
            }
            else
            {
                return cb.SelectedValue.ToString();
            }
        }

        public static bool ReadCheckBox(CheckBox cb)
        {
            bool data = false;

            if (cb.InvokeRequired)
            {
                cb.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        data = cb.Checked;

                    }
                    catch (Exception ex)
                    {
                        string log = ex.Message;
                        MessageBox.Show(log);
                    }
                });
                return data;
            }
            else
            {
                return cb.Checked;
            }
        }
    }
}
