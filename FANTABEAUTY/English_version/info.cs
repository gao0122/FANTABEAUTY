using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FANTABEAUTY.English_version
{
    public partial class info : Form
    {
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        const int AW_BLEND = 0x80000;
        const int AW_ACTIVATE = 0x20000;
        const int AW_HOR_POSITIVE = 0x0001;
        const int AW_HOR_NEGATIVE = 0x0002;
        const int AW_HIDE = 0x10000;

        private Timer timer = new Timer();
        private string cardVal;
        private string nameVal;
        private string phoneVal;
        private string genderVal;
        private string birthdayVal;
        private string balanceVal;

        public info()
        {
            InitializeComponent();
        }

        public info(string id, string name, string phone, string gender, string birthday, string balance)
        {
            InitializeComponent();
            cardVal = id;
            nameVal = name;
            phoneVal = phone;
            genderVal = gender;
            birthdayVal = birthday;
            balanceVal = balance;
        }

        private void info_Load(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 256, AW_BLEND | AW_ACTIVATE | AW_HOR_POSITIVE);
            Bitmap a = (Bitmap)Bitmap.FromFile("mouse.png");
            SetCursor(a, new Point(0, 0));

            lblTime.ForeColor = Color.Teal;
            lblTime.Font = new Font("微软雅黑", 11, FontStyle.Regular);
            toCn.Font = new Font("微软雅黑", 10, FontStyle.Regular);
            slash.Font = new Font("微软雅黑", 10, FontStyle.Regular);
            toEn.Font = new Font("微软雅黑", 10, FontStyle.Regular);
            toCn.Location = new Point(753, 23);
            slash.Location = new Point(785, 23);
            toEn.Location = new Point(796, 23);
            toCn.Cursor = Cursors.Hand;
            lblTime.BackColor = Color.Transparent;
            lblTime.Location = new Point(667, 489);
            lblTime.Text = DateTime.Now.ToString();

            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            this.timer.Start();

            card.Text = cardVal;
            name.Text = nameVal;
            phone.Text = phoneVal;
            gender.Text = genderVal;
            birthday.Text = birthdayVal.Split(' ')[0];
            remaining.Text = balanceVal;
        }

        public void SetCursor(Bitmap cursor, Point hotPoint)
        {
            int hotX = hotPoint.X;
            int hotY = hotPoint.Y;
            Bitmap myNewCursor = new Bitmap(cursor.Width * 2 - hotX, cursor.Height * 2 - hotY);
            Graphics g = Graphics.FromImage(myNewCursor);
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            g.DrawImage(cursor, cursor.Width - hotX, cursor.Height - hotY, cursor.Width, cursor.Height);
            this.Cursor = new Cursor(myNewCursor.GetHicon());
            g.Dispose();
            myNewCursor.Dispose();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString();
        }

        private void toCn_Click(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
            this.Hide();
            new Chinese_version.infoCn().Show();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
            this.Hide();
            new English_version.client(lblCard.Text, lblName.Text, lblPhone.Text).Show();
        }

        private void btnLogo_Click(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
            this.Hide();
            new English_version.menu().Show();
        }
    }
}
