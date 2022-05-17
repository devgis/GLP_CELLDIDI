using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bddd.Common
{
    public class MessageHelper
    {
        public static void ShowInfo(string info)
        {
            MessageBox.Show(info, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void ShowError(string info)
        {
            MessageBox.Show(info, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
