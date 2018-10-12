using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interop.Fm220api;

namespace Demo_CsharpBetweenSrvAndDll
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitFingerList();
        }

        private void InitFingerList()
        {
            List<FgDataList> lis_DataList = new List<FgDataList>()
            {
                new FgDataList
                {
                    fg_Name = " 1  Right Thumb",
                    fg_Value = "1"
                },
                new FgDataList
                {
                    fg_Name = " 2  Right Index",
                    fg_Value = "2"
                },
                new FgDataList
                {
                    fg_Name = " 3  Right Middle",
                    fg_Value = "3"
                },
                new FgDataList
                {
                    fg_Name = " 4  Right Ring",
                    fg_Value = "4"
                },
                new FgDataList
                {
                    fg_Name = " 5  Right Little",
                    fg_Value = "5"
                },
                new FgDataList
                {
                    fg_Name = " 6  Left Thumb",
                    fg_Value = "6"
                },
                new FgDataList
                {
                    fg_Name = " 7  Left Index",
                    fg_Value = "7"
                },
                new FgDataList
                {
                    fg_Name = " 8  Left Middle",
                    fg_Value = "8"
                },
                new FgDataList
                {
                    fg_Name = " 9  Left Ring",
                    fg_Value = "9"
                },
                new FgDataList
                {
                    fg_Name = "10  Left Little",
                    fg_Value = "10"
                }
            };

            comboBox_finger.DataSource = lis_DataList;
            comboBox_finger.DisplayMember = "fg_Name";
            comboBox_finger.ValueMember = "fg_Value";
            comboBox_finger.SelectedIndex = 0;
        }

        public class FgDataList
        {
            public string fg_Name { get; set; }
            public string fg_Value { get; set; }
        }

        private void button_enroll_Click(object sender, EventArgs e)
        {
            List<object> arguments = new List<object>();
            arguments.Add(this.richTextBox_status); //0
            arguments.Add(this.richTextBox_log);    //1
            arguments.Add(this.pictureBox1);        //2
            arguments.Add(this.richTextBox_id);     //3
            arguments.Add(this.comboBox_finger);    //4
            arguments.Add(this.checkBox_save);      //5
            arguments.Add(this.richTextBox_privilege);//6

            arguments.Add(this.checkBox_https);     //7
            arguments.Add(this.richTextBox_serverip);//8
            arguments.Add(this.richTextBox_port);   //9

            job jobs = new job();
            jobs.Job_Enroll(arguments);
        }

        private void button_identify_Click(object sender, EventArgs e)
        {
            List<object> arguments = new List<object>();
            arguments.Add(this.richTextBox_status); //0
            arguments.Add(this.richTextBox_log);    //1
            arguments.Add(this.pictureBox1);        //2
            arguments.Add(this.richTextBox_id);     //3
            arguments.Add(this.comboBox_finger);    //4
            arguments.Add(this.checkBox_save);      //5
            arguments.Add(this.richTextBox_privilege);//6

            arguments.Add(this.checkBox_https);     //7
            arguments.Add(this.richTextBox_serverip);//8
            arguments.Add(this.richTextBox_port);   //9

            job jobs = new job();
            jobs.Job_Identify(arguments);
        }

        private void button_verify_Click(object sender, EventArgs e)
        {
            List<object> arguments = new List<object>();
            arguments.Add(this.richTextBox_status); //0
            arguments.Add(this.richTextBox_log);    //1
            arguments.Add(this.pictureBox1);        //2
            arguments.Add(this.richTextBox_id);     //3
            arguments.Add(this.comboBox_finger);    //4
            arguments.Add(this.checkBox_save);      //5
            arguments.Add(this.richTextBox_privilege);//6

            arguments.Add(this.checkBox_https);     //7
            arguments.Add(this.richTextBox_serverip);//8
            arguments.Add(this.richTextBox_port);   //9

            job jobs = new job();
            jobs.Job_Verify(arguments);
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            List<object> arguments = new List<object>();
            arguments.Add(this.richTextBox_status); //0
            arguments.Add(this.richTextBox_log);    //1
            arguments.Add(this.pictureBox1);        //2
            arguments.Add(this.richTextBox_id);     //3
            arguments.Add(this.comboBox_finger);    //4
            arguments.Add(this.checkBox_save);      //5
            arguments.Add(this.richTextBox_privilege);//6

            arguments.Add(this.checkBox_https);     //7
            arguments.Add(this.richTextBox_serverip);//8
            arguments.Add(this.richTextBox_port);   //9

            job jobs = new job();
            jobs.Job_Delete(arguments);

        }
    }


}
