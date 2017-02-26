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
using System.Collections;
using System.Net;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;
using System.Reflection;

namespace FANTABEAUTY.English_version
{
    public partial class menu : Form
    {
        private const bool show = false;

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        const int AW_BLEND = 0x80000;
        const int AW_ACTIVATE = 0x20000;
        const int AW_HOR_POSITIVE = 0x0001;
        const int AW_HOR_NEGATIVE = 0x0002;
        const int AW_HIDE = 0x10000;

        private const string replyFileName = "reply.xml";
        private const string fileName = "state.xml";
        private string account = "";
        private Timer timer = new Timer();

        public menu()
        {
            InitializeComponent();
        }

        public menu(string account)
        {
            InitializeComponent();
            this.account = account;
        }

        private void menu_Load(object sender, EventArgs e)
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
            validation.Focus();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString();
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

        private void toCn_Click(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
            this.Hide();
            new Chinese_version.menuCn().Show();
        }

        private void logoPic_Click(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
            this.Hide();
            new English_version.login(account).Show();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            string val = validation.Text;
            if ("".Equals(val))
                return;
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

            mycom = new MySqlCommand("select * from customer_info where phone = '" + val + "'", mycon);
            myrec = mycom.ExecuteReader();

            if (myrec.Read())
            {
                AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
                this.Hide();
                new English_version.client(myrec["id"] + "", myrec["name"] + "", val).Show();
            }
            else
            {
                string code = new Random().Next(100000, 999999).ToString();
                string codeState = "";
                if (show)
                    codeState = GetHtmlFromUrl("http://utf8.sms.webchinese.cn/?Uid=yuchao&Key=7c0b143b511fe07aeaad&smsMob="
                + validation.Text + "&smsText=Welcome! Your validation code is: " + code);
                else
                    codeState = "1";
                if ("1".Equals(codeState))
                {
                    int newId = 0;
                    ArrayList ids = new ArrayList();
                    MySqlCommand getId = new MySqlCommand("select id from `customer_info`", mycon);
                    myrec.Close();
                    myrec = getId.ExecuteReader();
                    while (myrec.Read())
                        ids.Add((int)myrec["id"]);

                    ids.Sort();
                    foreach (int i in ids)
                    {
                        if (i != newId)
                            break;
                        newId++;
                    }
                    MessageBox.Show("Going to signup page!");
                    AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
                    this.Hide();
                    new English_version.signup(val, newId + "", code).Show();
                }
            }
            mycon.Close();
            myrec.Close();
        }

        private void btnLogo_Click(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
            this.Hide();
            new English_version.login().Show();
        }

        private void btnState_Click(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
            this.Hide();
            new English_version.state().Show();
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
            this.Hide();
            new English_version.data().Show();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            string val = validation.Text;
            if (val.Equals(""))
                return;

            if (File.Exists(fileName))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                string path = "//Client";
                XmlNodeList nodes = doc.SelectSingleNode(path).ChildNodes;
                // -----------------------------------------------------------------------------------------------------------------------------------
            }

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
            mycom = new MySqlCommand("select * from customer_info where phone = '" + val + "'", mycon);
            myrec = mycom.ExecuteReader();
            if (myrec.Read())
            {
                AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
                this.Hide();
                new English_version.info(myrec["id"] + "", myrec["name"] + "", val, myrec["gender"] + "",
                    myrec["birthday"] + "", myrec["balance"] + "").Show();
            }
            else
            {
                string code = new Random().Next(100000, 999999).ToString();
                string codeState = "";
                if (show)
                    codeState = GetHtmlFromUrl("http://utf8.sms.webchinese.cn/?Uid=yuchao&Key=7c0b143b511fe07aeaad&smsMob="
                + validation.Text + "&smsText=Welcome! Your validation code is: " + code);
                else
                    codeState = "1";
                if ("1".Equals(codeState))
                {
                    MessageBox.Show("Validation code has been sent! " + code);
                    int newId = 0;
                    ArrayList ids = new ArrayList();
                    MySqlCommand getId = new MySqlCommand("select * from `customer_info`", mycon);
                    myrec.Close();
                    myrec = getId.ExecuteReader();
                    while (myrec.Read())
                        ids.Add((int)myrec["id"]);

                    ids.Sort();
                    foreach (int i in ids)
                    {
                        if (i != newId)
                            break;
                        newId++;
                    }
                    AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
                    this.Hide();
                    new English_version.signup(val, newId + "", code).Show();
                }
                else
                    MessageBox.Show("Invalid phone number!");
            }
            mycon.Close();
            myrec.Close();
        }

        private void validation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8)
                e.Handled = true;
            if ((int)e.KeyChar == 13)
                btnCheck_Click(sender, e);
        }

        private string GetHtmlFromUrl(string url)
        {
            string strRet = null;
            if (url == null || url.Trim().ToString() == "")
                return strRet;

            string targeturl = url.Trim().ToString();
            try
            {
                HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
                hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                hr.Method = "GET";
                hr.Timeout = 30 * 60 * 1000;
                WebResponse hs = hr.GetResponse();
                Stream sr = hs.GetResponseStream();
                StreamReader ser = new StreamReader(sr, Encoding.Default);
                strRet = ser.ReadToEnd();
            }
            catch (Exception er)
            {
                strRet = null;
                Console.WriteLine(er.Message);
            }
            return strRet;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            //String replyMsg = "We have recieved your messsage, thanks!";
            String msg = GetHtmlFromUrl("http://sms.webchinese.cn/web_api/SMS/?Action=UP&Uid=yuchao&Key=7c0b143b511fe07aeaad&Prompt=0");
            if ("0".Equals(msg))
                MessageBox.Show("No replys now!");
            else
                try
                {
                    FileStream fs = File.Create(replyFileName);
                    byte[] data = new UTF8Encoding().GetBytes(msg);
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                    fs.Close();
                    XmlDocument replys = new XmlDocument();
                    replys.Load(replyFileName);

                    string pathOne = "//deliver[@mob = '" + validation.Text + "']";
                    XmlNodeList phoneNodes = replys.SelectNodes("//mob");
                    XmlNodeList textNodes = replys.SelectNodes("//content");
                    XmlNodeList timeNodes = replys.SelectNodes("//time");
                    MessageBox.Show(textNodes[0].InnerText);
                }
                catch (Exception er)
                {
                    Console.WriteLine(er.Message);
                }

        }

        private void validation_EditValueChanged(object sender, EventArgs e)
        {
            string val = validation.Text;
            if (val.Equals(""))
                lblCus.Text = "";

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
            mycom = new MySqlCommand("select * from customer_info where phone = '" + val + "'", mycon);
            myrec = mycom.ExecuteReader();

            if (myrec.Read())
                lblCus.Text = "Old";
            else
                lblCus.Text = "New";
            if (validation.Text.Equals(""))
                lblCus.Text = "";

            mycon.Close();
            myrec.Close();

        }

        private XmlAttribute CreateAttribute(XmlNode node, string attributeName, string value)
        {
            try
            {
                XmlDocument doc = node.OwnerDocument;
                XmlAttribute attr = doc.CreateAttribute(attributeName);
                attr.Value = value;
                node.Attributes.SetNamedItem(attr);
                return attr;
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                return null;
            }
        }
    }
}
