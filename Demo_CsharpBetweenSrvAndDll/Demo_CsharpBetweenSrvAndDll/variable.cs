using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_CsharpBetweenSrvAndDll
{
    public class variable
    {
        public static string model = "FM220";
        public static int img_w = 264;
        public static int img_h = 324;

        //fm220 sdk Constant define
        public static short OK = 0;
        public static short FAIL = -1;
        public static short LARGE = 10;
        public static short SMALL = 11;

        public static short RAW = 12;
        public static short BMP = 13;
        public static byte GRAY_IMAGE = 8;

        public static string[] running_symble = { " .", " ..", " ...", " ...." };

        public static int mode = 1, key = 0;  //mode : 1 ISO ;2 :M1-378
        public static String ist_filename = "enroll.ist";

        public static int superc_width = 256;
        public static int superc_height = 360;

        public static int fm220_width = 264;
        public static int fm220_height = 324;
    }
}
