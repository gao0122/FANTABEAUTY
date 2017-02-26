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
using System.Xml;
using System.IO;

namespace FANTABEAUTY.English_version
{
    public partial class state : Form
    {
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        const int AW_BLEND = 0x80000;
        const int AW_ACTIVATE = 0x20000;
        const int AW_HOR_POSITIVE = 0x0001;
        const int AW_HOR_NEGATIVE = 0x0002;
        const int AW_HIDE = 0x10000;

        private const string fileName = "state.xml";
        private Timer timer = new Timer();

        public state()
        {
            InitializeComponent();
        }

        private void state_Load(object sender, EventArgs e)
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

            btnEnd1.Visible = false;
            btnEnd2.Visible = false;
            btnEnd3.Visible = false;
            btnEnd4.Visible = false;

            if (File.Exists(fileName))
            {
                XmlDocument docState = new XmlDocument();
                docState.Load(fileName);
                string pathState = "//Client";
                /*   0       childNode.Attributes.Append(CreateAttribute(node, "phone", phone));
                     1       childNode.Attributes.Append(CreateAttribute(node, "name", name));
                     2       childNode.Attributes.Append(CreateAttribute(node, "service", serviceType));
                     3       childNode.Attributes.Append(CreateAttribute(node, "price", lblPrice.Text));
                     4       childNode.Attributes.Append(CreateAttribute(node, "room", roomNum));
                     5      childNode.Attributes.Append(CreateAttribute(node, "starttime", lblTimes.Text));
                     6      childNode.Attributes.Append(CreateAttribute(node, "howlong", lblTimeLong.Text)); */
                XmlNodeList nodesState = docState.SelectSingleNode(pathState).ChildNodes;
                if (nodesState != null)
                    foreach (XmlNode n in nodesState)
                    {
                        string i = n.Attributes[4].Value;
                        if ("1".Equals(i))
                        {
                            mycom = new MySqlCommand("select * from customer_info where phone = '" + n.Attributes[0].Value + "'", mycon);
                            myrec = mycom.ExecuteReader();
                            if (myrec.Read())
                                lblCard1.Text = myrec["id"] + "";
                            btnEnd1.Visible = true;
                            lblService1.Text = "Inservice";
                            lblName1.Text = n.Attributes[1].Value;
                            lblServiceName1.Text = n.Attributes[2].Value;
                            lblStart1.Text = n.Attributes[5].Value;
                            myrec.Close();
                        }
                        if ("2".Equals(i))
                        {
                            mycom = new MySqlCommand("select * from customer_info where phone = '" + n.Attributes[0].Value + "'", mycon);
                            myrec = mycom.ExecuteReader();
                            if (myrec.Read())
                                lblCard2.Text = myrec["id"] + "";
                            btnEnd2.Visible = true;
                            lblService2.Text = "Inservice";
                            lblName2.Text = n.Attributes[1].Value;
                            lblServiceName2.Text = n.Attributes[2].Value;
                            lblStart2.Text = n.Attributes[5].Value;
                            myrec.Close();

                        }
                        if ("3".Equals(i))
                        {
                            mycom = new MySqlCommand("select * from customer_info where phone = '" + n.Attributes[0].Value + "'", mycon);
                            myrec = mycom.ExecuteReader();
                            if (myrec.Read())
                                lblCard3.Text = myrec["id"] + "";
                            btnEnd3.Visible = true;
                            lblService3.Text = "Inservice";
                            lblName3.Text = n.Attributes[1].Value;
                            lblServiceName3.Text = n.Attributes[2].Value;
                            lblStart3.Text = n.Attributes[5].Value;
                            myrec.Close();
                        }
                        if ("4".Equals(i))
                        {
                            mycom = new MySqlCommand("select * from customer_info where phone = '" + n.Attributes[0].Value + "'", mycon);
                            myrec = mycom.ExecuteReader();
                            if (myrec.Read())
                                lblCard4.Text = myrec["id"] + "";
                            btnEnd1.Visible = true;
                            lblService4.Text = "Inservice";
                            lblName4.Text = n.Attributes[1].Value;
                            lblServiceName4.Text = n.Attributes[2].Value;
                            lblStart4.Text = n.Attributes[5].Value;
                            myrec.Close();
                        }
                    }
            }
            mycon.Close();
            myrec.Close();
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
            new Chinese_version.stateCn().Show();
        }

        private void btnLogo_Click(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
            this.Hide();
            new English_version.menu().Show();
        }

        private void btnEnd1_Click(object sender, EventArgs e)
        {
            string cost = "";
            XmlDocument docState = new XmlDocument();
            docState.Load(fileName);
            string pathState = "//Client";
            XmlNodeList nodesState = docState.SelectSingleNode(pathState).ChildNodes;
            if (nodesState != null)
                foreach (XmlNode n in nodesState)
                    if (n.Attributes[4].Value.Equals("1"))
                    {
                        cost = n.Attributes[3].Value.Split('/')[0];
                        savaToDatabase(1, cost);
                        docState.SelectSingleNode(pathState).RemoveChild(n);
                        docState.Save(fileName);
                    }
            lblCard1.Text = "";
            btnEnd1.Visible = false;
            lblService1.Text = "Free";
            lblName1.Text = "";
            lblServiceName1.Text = "Receivable: " + cost + " Yuan";
            lblStart1.Text = "";
        }

        private void btnEnd2_Click(object sender, EventArgs e)
        {
            string cost = "";
            XmlDocument docState = new XmlDocument();
            docState.Load(fileName);
            string pathState = "//Client";
            XmlNodeList nodesState = docState.SelectSingleNode(pathState).ChildNodes;
            if (nodesState != null)
                foreach (XmlNode n in nodesState)
                    if (n.Attributes[4].Value.Equals("2"))
                    {
                        cost = n.Attributes[3].Value.Split('/')[0];
                        savaToDatabase(2, cost);
                        docState.SelectSingleNode(pathState).RemoveChild(n);
                        docState.Save(fileName);
                    }
            lblCard2.Text = "";
            btnEnd2.Visible = false;
            lblService2.Text = "Free";
            lblName2.Text = "";
            lblServiceName2.Text = "Receivable: " + cost + " Yuan";
            lblStart2.Text = "";
        }

        private void btnEnd3_Click(object sender, EventArgs e)
        {
            string cost = "";
            XmlDocument docState = new XmlDocument();
            docState.Load(fileName);
            string pathState = "//Client";
            XmlNodeList nodesState = docState.SelectSingleNode(pathState).ChildNodes;
            if (nodesState != null)
                foreach (XmlNode n in nodesState)
                    if (n.Attributes[4].Value.Equals("3"))
                    {
                        cost = n.Attributes[3].Value.Split('/')[0];
                        savaToDatabase(3, cost);
                        docState.SelectSingleNode(pathState).RemoveChild(n);
                        docState.Save(fileName);
                    }
            lblCard3.Text = "";
            btnEnd3.Visible = false;
            lblService3.Text = "Free";
            lblName3.Text = "";
            lblServiceName3.Text = "Receivable: " + cost + " Yuan";
            lblStart3.Text = "";
        }

        private void btnEnd4_Click(object sender, EventArgs e)
        {
            string cost = "";
            XmlDocument docState = new XmlDocument();
            docState.Load(fileName);
            string pathState = "//Client";
            XmlNodeList nodesState = docState.SelectSingleNode(pathState).ChildNodes;
            if (nodesState != null)
                foreach (XmlNode n in nodesState)
                    if (n.Attributes[4].Value.Equals("4"))
                    {
                        cost = n.Attributes[3].Value.Split('/')[0];
                        savaToDatabase(4, cost);
                        docState.SelectSingleNode(pathState).RemoveChild(n);
                        docState.Save(fileName);
                    }
            lblCard4.Text = "";
            btnEnd4.Visible = false;
            lblService4.Text = "Free";
            lblName4.Text = "";
            lblServiceName4.Text = "Receivable: " + cost + " Yuan";
            lblStart4.Text = "";
        }

        private void savaToDatabase(int v, string price)
        {
            string name = "";
            string card = "";
            string service = "";
            string start = "";
            string balance = "";
            MySqlConnection mycon = null;
            MySqlCommand mycom = null;
            MySqlCommand mycom2 = null;
            MySqlCommand mycom3 = null;
            MySqlDataReader myrec = null;

            mycon = new MySqlConnection("Host = qdm194418158.my3w.com; Database = qdm194418158_db; Username = qdm194418158; Password = Cydlz001");
            mycon.Open();

            if (mycon.State.ToString() != "Open")
            {
                MessageBox.Show("Fail to connect!");
                mycon.Close();
                return;
            }

            if (v == 1)
            {
                mycom = new MySqlCommand("select * from customer_info where id = '" + lblCard1.Text + "'", mycon);
                name = lblName1.Text;
                card = lblCard1.Text;
                service = lblServiceName1.Text;
                start = lblStart1.Text;
            }
            else if (v == 2)
            {
                mycom = new MySqlCommand("select * from customer_info where id = '" + lblCard2.Text + "'", mycon);
                name = lblName2.Text;
                card = lblCard2.Text;
                service = lblServiceName2.Text;
                start = lblStart2.Text;
            }
            else if (v == 3)
            {
                mycom = new MySqlCommand("select * from customer_info where id = '" + lblCard3.Text + "'", mycon);
                name = lblName3.Text;
                card = lblCard3.Text;
                service = lblServiceName3.Text;
                start = lblStart3.Text;
            }
            else if (v == 4)
            {
                mycom = new MySqlCommand("select * from customer_info where id = '" + lblCard4.Text + "'", mycon);
                name = lblName4.Text;
                card = lblCard4.Text;
                service = lblServiceName4.Text;
                start = lblStart4.Text;
            }
            myrec = mycom.ExecuteReader();
            if (myrec.Read())
            {
                balance = myrec["balance"] + "";
                string gender = myrec["gender"] + "";
                string phone = myrec["phone"] + "";
                myrec.Close();
                string enddate = lblTime.Text.Split(' ')[0];
                string endtime = lblTime.Text.Split(' ')[1];
                string endhour = endtime.Split(':')[0];
                string starthour = start.Split(':')[0];
                int remain = int.Parse(balance) - int.Parse(price);
                mycom2 = new MySqlCommand("INSERT INTO customer_consumption(`id`, `name`, `gender`, `phone`, `date`, `start_time`, `end_time`, `service`, `cost`, `room`, `remain_money`)"
                    + "values('" + card + "', '" + name + "', '" + gender + "', '" + phone + "', '" + enddate + "', '"
                    + start + "', '" + endtime + "', '" + service + "', '" + price + "', '" + v + "', '" + remain + "')", mycon);
                myrec = mycom2.ExecuteReader();
                myrec.Close();
                mycom3 = new MySqlCommand("UPDATE `customer_info` SET `balance` = '" + remain + "' WHERE id = '" + card + "'", mycon);
                myrec = mycom3.ExecuteReader();
            }
            mycon.Close();
            myrec.Close();
        }
    }
}
