using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace FANTABEAUTY.English_version
{
    public partial class login : Form
    {
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        const int AW_BLEND = 0x80000;
        const int AW_ACTIVATE = 0x20000;
        const int AW_HOR_POSITIVE = 0x0001;
        const int AW_HOR_NEGATIVE = 0x0002;
        const int AW_HIDE = 0x10000;

        private string ACCOUNT = "";
        private string PWD = "";
        private Timer timer = new Timer();

        public login()
        {
            InitializeComponent();
        }

        public login(string account)
        {
            InitializeComponent();
            acct.Text = account;
        }

        public login(string account, string pwd)
        {
            InitializeComponent();
            acct.Text = account;
            password.Text = pwd;
        }

        private void login_Load(object sender, EventArgs e)
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            MySqlConnection mycon = null;
            MySqlCommand mycom = null;
            MySqlDataReader myrec = null;

            mycon = new MySqlConnection("Host = qdm194418158.my3w.com; Database = qdm194418158_db; Username = qdm194418158; Password = Cydlz001");
            mycon.Open();

            if (mycon.State.ToString() != "Open")
            {
                MessageBox.Show("Fail to connect!");
                mycon.Close();
                return;
            }
            mycom = new MySqlCommand("select * from manager_info where account = '" + acct.Text + "'", mycon);
            myrec = mycom.ExecuteReader();

            while (myrec.Read())
            {
                ACCOUNT = acct.Text;
                PWD = myrec["pwd"] + "";
            }

            mycon.Close();
            myrec.Close();

            this.errLoginAcc.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            this.errLoginPwd.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;

            if (string.IsNullOrEmpty(acct.Text))
                this.errLoginAcc.SetError(this.acct, "Please enter your account name!");
            else
                this.errLoginAcc.Clear();
            
            if (string.IsNullOrEmpty(password.Text))
                this.errLoginPwd.SetError(this.password, "Please enter your password!");
            else
                this.errLoginPwd.Clear();

            if (!string.IsNullOrEmpty(password.Text) && (!string.IsNullOrEmpty(acct.Text)))
                if (acct.Text.Equals(ACCOUNT) && password.Text.Equals(PWD))
                {
                    AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
                    this.Hide();
                    new FANTABEAUTY.English_version.menu(acct.Text).Show();
                }
                else
                {
                    this.errLoginAcc.SetError(this.acct, "Account name or password is wrong!");
                    this.errLoginPwd.SetError(this.acct, "Account name or password is wrong!");
                }
        }

        private void toCn_Click(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
            this.Hide();
            new Chinese_version.loginCn(acct.Text).Show();
        }

        private void pwd_TextChanged(object sender, EventArgs e)
        {
            password.Properties.UseSystemPasswordChar = true;
        }

    }
}
