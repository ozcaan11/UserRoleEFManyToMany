using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormUserRole.Helper
{
    public class Mesaj
    {
        public static DialogResult Show(string mesaj)
        {
            return MessageBox.Show(mesaj);
        }
    }
}
