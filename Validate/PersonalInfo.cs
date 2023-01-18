using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoAn_QLKS_dotnet.InformationProcessing;

namespace DoAn_QLKS_dotnet.Validate
{
    public class PersonalInfo
    {
        XULYDULIEU xldl = new XULYDULIEU();
        
        public static bool IsNumber(string pValue)
        {
            foreach (Char c in pValue)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }
        public static bool CheckSDT(string sdt)
        {
            int i = 0;
            string Frist = sdt.Substring(0, 1);
            foreach (char chr in sdt)
            {
                i++;
            }

            if (i == 10 && Frist == "0" && IsNumber(sdt)) return true;
            else return false;
        }
        public static bool CheckCMND(string cccd)
        {
            int i = 0;
            foreach (char chr in cccd)
            {
                i++;
            }
            if (i == 12 && IsNumber(cccd)) return true;
            else return false;
        }
        public static bool CheckUserName(string ussername)
        {
            foreach (char i in ussername.Trim())
            {
                if ((i > 47 && i < 58 || i > 96 && i < 123 || i > 64 && i < 91 || i < 32) == false)
                {
                    return false;
                }
            }
            return true;
        }
        

    }
}
