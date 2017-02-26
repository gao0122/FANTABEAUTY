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
    public partial class signup : Form
    {
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        const int AW_BLEND = 0x80000;
        const int AW_ACTIVATE = 0x20000;
        const int AW_HOR_POSITIVE = 0x0001;
        const int AW_HOR_NEGATIVE = 0x0002;
        const int AW_HIDE = 0x10000;

        private Timer timer = new Timer();
        private string phoneVal;
        private string idVal;
        private string codeVal;

        public signup()
        {
            InitializeComponent();
        }

        public signup(string phone, string id, string code)
        {
            InitializeComponent();
            this.phoneVal = phone;
            this.idVal = id;
            this.codeVal = code;
        }

        public signup(string phone, string id, string code, string birthday, string money, string gender, string comments)
        {
            InitializeComponent();
            this.phoneVal = phone;
            this.idVal = id;
            this.codeVal = code;

            this.phone.Text = phone;
            this.card.Text = id;
            this.birthday.Text = birthday;
            this.code.Text = code;
            this.money.Text = money;
            this.gender.Text = gender;
            this.comments.Text = comments;
        }

        private void signup_Load(object sender, EventArgs e)
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
            lblTime.Location = new Point(667, 489);
            toCn.Cursor = Cursors.Hand;
            lblTime.BackColor = Color.Transparent;

            lblTime.Text = DateTime.Now.ToString();
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            this.timer.Start();
            phone.Text = phoneVal;
            card.Text = idVal;
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
            new Chinese_version.signupCn(phone.Text, card.Text, code.Text, birthday.Text, money.Text, gender.Text, comments.Text).Show();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (codeVal.Equals(code.Text))
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

                mycom = new MySqlCommand("INSERT INTO `customer_info`(`id`, `name`, `gender`, `phone`, `birthday`, `balance`, `comment`) VALUES ('" + card.Text + "', '"
                    + name.Text + "', '" + gender.Text + "', '" + phone.Text + "', '" + birthday.Text + "', '" + money.Text + "', '" + comments.Text + "')", mycon);

                myrec = mycom.ExecuteReader();
                AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
                this.Hide();
                new English_version.client(card.Text, name.Text, phone.Text).Show();
                myrec.Close();
                mycon.Close();
            }
            else
                MessageBox.Show("incorrected code!");
        }

        private void code_Click(object sender, EventArgs e)
        {
            if (code.Text.Trim().Split(' ').Length == 2)
                code.Text = "";
        }

        private void btnLogo_Click(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
            this.Hide();
            new English_version.menu().Show();
        }

        private void birthday_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!Char.IsDigit(e.KeyChar)) && e.KeyChar != 8)
                e.Handled = true;
            if ((!isNull()) && (int)e.KeyChar == 13)
                btnConfirm_Click(sender, e);
        }

        private void name_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 32)
                e.Handled = true;
            if ((!isNull()) && (int)e.KeyChar == 13)
                btnConfirm_Click(sender, e);
        }

        private void money_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!Char.IsDigit(e.KeyChar)) && e.KeyChar != 8)
                e.Handled = true;
            if ((!isNull()) && (int)e.KeyChar == 13)
                btnConfirm_Click(sender, e);
        }

        private void gender_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!isNull()) && (int)e.KeyChar == 13)
                btnConfirm_Click(sender, e);
        }

        private void code_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!isNull()) && (int)e.KeyChar == 13)
                btnConfirm_Click(sender, e);
        }

        private void comments_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!isNull()) && (int)e.KeyChar == 13)
                btnConfirm_Click(sender, e);
        }

        private bool isNull()
        {
            return "".Equals(card.Text) || "".Equals(phone.Text) || "".Equals(name.Text)
                || "".Equals(gender.Text) || "".Equals(birthday.Text) || "".Equals(code.Text) || "".Equals(money.Text);
        }
    }
}
