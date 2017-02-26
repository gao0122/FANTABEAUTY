using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Net;

namespace FANTABEAUTY.English_version
{
    public partial class client : Form
    {
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        const int AW_BLEND = 0x80000;
        const int AW_ACTIVATE = 0x20000;
        const int AW_HOR_POSITIVE = 0x0001;
        const int AW_HOR_NEGATIVE = 0x0002;
        const int AW_HIDE = 0x10000;

        private string fileName = "state.xml";
        private string roomName = "room.xml";
        private string serviceName = "service.xml";

        private Timer timer = new Timer();
        string idVal, nameVal, phoneVal, serviceVal;
        public client()
        {
            InitializeComponent();
        }

        public client(string id, string name, string phone)
        {
            InitializeComponent();
            this.idVal = id;
            this.nameVal = name;
            this.phoneVal = phone;
            this.serviceVal = "";
            date.Text = DateTime.Today.ToString().Split(' ')[0];
            time.Text = DateTime.Now.ToString().Split(' ')[1];
        }


        private void client_Load(object sender, EventArgs e)
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
            this.card.Text = idVal;
            this.name.Text = nameVal;
            this.price.Text = "";

            lblTime.Text = DateTime.Now.ToString();
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            this.timer.Start();

            XmlDocument doc = new XmlDocument();
            doc.Load(roomName);
            string path = "//rooms";
            XmlNodeList nodes = doc.SelectNodes(path);
            if (nodes != null)
            {
                foreach (XmlNode n in nodes)
                {
                    string i = n.Attributes[0].Value;
                    if ("1".Equals(i))
                        room1.Enabled = false;
                    if ("2".Equals(i))
                        room2.Enabled = false;
                    if ("3".Equals(i))
                        room3.Enabled = false;
                    if ("4".Equals(i))
                        room4.Enabled = false;
                }

            }
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
            new Chinese_version.clientCn().Show();
        }

        private void service_SelectedIndexChanged(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(serviceName);
            string path = "//Service";
            XmlNodeList nodes = doc.SelectSingleNode(path).ChildNodes;
            if (nodes != null)
                foreach (XmlNode n in nodes)
                    if (service.Text.Equals(n.Name))
                    {
                        price.Text = n.InnerText + "/time";
                        timelong.Text = n.Attributes[0].Value + " minutes";
                    }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (!"".Equals(service.Text))
            {
                serviceVal = service.Text;
                string roomNum = "";
                if (room1.Checked)
                    roomNum = "1";
                else if (room2.Checked)
                    roomNum = "2";
                else if (room3.Checked)
                    roomNum = "3";
                else if (room4.Checked)
                    roomNum = "4";

                if (File.Exists(fileName))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fileName);
                    XmlNode node = doc.SelectSingleNode("//Client");
                    XmlNode childNode = doc.CreateElement("Customer");
                    childNode.Attributes.Append(CreateAttribute(node, "phone", phoneVal));
                    childNode.Attributes.Append(CreateAttribute(node, "name", name.Text));
                    childNode.Attributes.Append(CreateAttribute(node, "service", service.Text));
                    childNode.Attributes.Append(CreateAttribute(node, "price", price.Text));
                    childNode.Attributes.Append(CreateAttribute(node, "room", roomNum));
                    childNode.Attributes.Append(CreateAttribute(node, "starttime", time.Text));
                    childNode.Attributes.Append(CreateAttribute(node, "howlong", timelong.Text));
                    node.AppendChild(childNode);
                    doc.Save(fileName);
                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                    doc.AppendChild(dec);

                    XmlElement root = doc.CreateElement("Client");
                    doc.AppendChild(root);

                    XmlNode childNode = doc.CreateElement("Customer");
                    childNode.Attributes.Append(CreateAttribute(root, "phone", phoneVal));
                    childNode.Attributes.Append(CreateAttribute(root, "name", name.Text));
                    childNode.Attributes.Append(CreateAttribute(root, "service", service.Text));
                    childNode.Attributes.Append(CreateAttribute(root, "price", price.Text));
                    childNode.Attributes.Append(CreateAttribute(root, "room", roomNum));
                    childNode.Attributes.Append(CreateAttribute(root, "starttime", time.Text));
                    childNode.Attributes.Append(CreateAttribute(root, "howlong", timelong.Text));
                    root.AppendChild(childNode);
                    doc.Save(fileName);
                }

                AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
                this.Hide();
                new English_version.state().Show();
            }
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

        private void btnLogo_Click(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
            this.Hide();
            new English_version.menu().Show();
        }
    }
}
