using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using DevExpress.Charts;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Net;
using System.IO;
using System.Xml;

namespace FANTABEAUTY.English_version
{
    public partial class data : Form
    {
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        const int AW_BLEND = 0x80000;
        const int AW_ACTIVATE = 0x20000;
        const int AW_HOR_POSITIVE = 0x0001;
        const int AW_HOR_NEGATIVE = 0x0002;
        const int AW_HIDE = 0x10000;

        private Timer timer = new Timer();
        public data()
        {
            InitializeComponent();
        }

        private void toCn_Click(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
            this.Hide();
            new Chinese_version.dataCn().Show();
        }

        private void data_Load(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 256, AW_BLEND | AW_ACTIVATE | AW_HOR_POSITIVE);
            Bitmap a = (Bitmap)Bitmap.FromFile("mouse.png");
            SetCursor(a, new Point(0, 0));

            toCn.Cursor = Cursors.Hand;
            lblTime.ForeColor = Color.Teal;
            lblTime.Text = DateTime.Now.ToString();
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            this.timer.Start();

            chartControl.Series[0].Points.Clear();
            this.chartControl.Visible = false;
            this.panelControler.Visible = true;
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

        private void btnLogo_Click(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 256, AW_BLEND | AW_HIDE | AW_HOR_NEGATIVE);
            this.Hide();
            new English_version.menu().Show();
        }

        private void card_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar != 8 && ((int)e.KeyChar < 48 || (int)e.KeyChar > 57)
                    && (int)e.KeyChar != 97 && (int)e.KeyChar != 108)
                e.Handled = true;
            if ((int)e.KeyChar == 13 && (!"".Equals(card.Text)))
                btnCheck_Click(sender, e);
        }

        private void phone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar != 8 && ((int)e.KeyChar < 48 || (int)e.KeyChar > 57))
                e.Handled = true;
            if ((int)e.KeyChar == 13 && (!"".Equals(phone.Text)))
                btnCheck_Click(sender, e);
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            panelControler.Controls.Clear();
            panelControler.Visible = true;
            chartControl.Visible = false;

            Label lb1 = new Label();
            lb1.Name = "lb1";
            lb1.AutoSize = true;
            lb1.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            lb1.Text = "id";
            lb1.Location = new Point(100, 10);
            Label lb2 = new Label();
            lb2.Name = "lb2";
            lb2.AutoSize = true;
            lb2.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            lb2.Text = "name";
            lb2.Location = new Point(200, 10);
            Label lb3 = new Label();
            lb3.Name = "lb3";
            lb3.AutoSize = true;
            lb3.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            lb3.Text = "date";
            lb3.Location = new Point(300, 10);
            //this.Controls.Add(lb3);
            Label lb4 = new Label();
            lb4.Name = "lb4";
            lb4.AutoSize = true;
            lb4.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            lb4.Text = "start time";
            lb4.Location = new Point(400, 10);
            //this.Controls.Add(lb4); 
            Label lb5 = new Label();
            lb5.Name = "lb5";
            lb5.AutoSize = true;
            lb5.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            lb5.Text = "end time";
            lb5.Location = new Point(500, 10);
            //this.Controls.Add(lb5);
            Label lb6 = new Label();
            lb6.Name = "lb6";
            lb6.AutoSize = true;
            lb6.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            lb6.Text = "service";
            lb6.Location = new Point(600, 10);
            Label lb7 = new Label();
            lb7.Name = "lb7";
            lb7.AutoSize = true;
            lb7.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            lb7.Text = "cost";
            lb7.Location = new Point(700, 10);
            panelControler.Controls.Add(lb1);
            panelControler.Controls.Add(lb2);
            panelControler.Controls.Add(lb3);
            panelControler.Controls.Add(lb4);
            panelControler.Controls.Add(lb5);
            panelControler.Controls.Add(lb6);
            panelControler.Controls.Add(lb7);

            if (!"all".Equals(card.Text))
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

                if ((!"".Equals(phone.Text)) && "".Equals(card.Text))
                {
                    mycom = new MySqlCommand("select * from customer_consumption where phone = '" + phone.Text + "'", mycon);
                }
                else if ((!"".Equals(card.Text)) && "".Equals(phone.Text))
                {
                    mycom = new MySqlCommand("select * from customer_consumption where id = '" + card.Text + "'", mycon);
                }
                else if ((!"".Equals(card.Text)) && (!"".Equals(phone.Text)))
                {
                    mycom = new MySqlCommand("select * from customer_consumption where id = '" + card.Text + "'", mycon);
                }
                else
                    return;

                myrec = mycom.ExecuteReader();

                int num = 0;
                while (myrec.Read())
                {
                    card.Text = myrec["id"] + "";
                    phone.Text = myrec["phone"] + "";
                    num++;
                    Label l1 = new Label();
                    l1.Name = "l1";
                    l1.AutoSize = true;
                    l1.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    l1.Text = myrec["id"] + "";
                    l1.Location = new Point(100, num * 50);
                    Label l2 = new Label();
                    l2.Name = "l2";
                    l2.AutoSize = true;
                    l2.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    l2.Text = myrec["name"] + "";
                    l2.Location = new Point(200, num * 50);
                    Label l3 = new Label();
                    l3.Name = "l3";
                    l3.AutoSize = true;
                    l3.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    l3.Text = (myrec["date"] + "").Split(' ')[0];
                    l3.Location = new Point(300, num * 50);
                    //this.Controls.Add(l3);
                    Label l4 = new Label();
                    l4.Name = "l4";
                    l4.AutoSize = true;
                    l4.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    l4.Text = myrec["start_time"] + "";
                    l4.Location = new Point(400, num * 50);
                    //this.Controls.Add(l4); 
                    Label l5 = new Label();
                    l5.Name = "l5";
                    l5.AutoSize = true;
                    l5.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    l5.Text = myrec["end_time"] + "";
                    l5.Location = new Point(500, num * 50);
                    //this.Controls.Add(l5);
                    Label l6 = new Label();
                    l6.Name = "l6";
                    l6.AutoSize = true;
                    l6.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    l6.Text = myrec["service"] + "";
                    l6.Location = new Point(600, num * 50);
                    Label l7 = new Label();
                    l7.Name = "l7";
                    l7.AutoSize = true;
                    l7.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    l7.Text = myrec["cost"] + "";
                    l7.Location = new Point(700, num * 50);
                    panelControler.Controls.Add(l1);
                    panelControler.Controls.Add(l2);
                    panelControler.Controls.Add(l3);
                    panelControler.Controls.Add(l4);
                    panelControler.Controls.Add(l5);
                    panelControler.Controls.Add(l6);
                    panelControler.Controls.Add(l7);
                    panelControler.AutoScroll = true;
                }

                myrec.Close();
                mycon.Close();
            }
            else
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
                mycom = new MySqlCommand("select * from customer_consumption", mycon);
                myrec = mycom.ExecuteReader();

                int num = 0;
                while (myrec.Read())
                {
                    num++;
                    Label l1 = new Label();
                    l1.Name = "l1";
                    l1.AutoSize = true;
                    l1.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    l1.Text = myrec["id"] + "";
                    l1.Location = new Point(100, num * 50);
                    Label l2 = new Label();
                    l2.Name = "l2";
                    l2.AutoSize = true;
                    l2.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    l2.Text = myrec["name"] + "";
                    l2.Location = new Point(200, num * 50);
                    Label l3 = new Label();
                    l3.Name = "l3";
                    l3.AutoSize = true;
                    l3.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    l3.Text = (myrec["date"] + "").Split(' ')[0];
                    l3.Location = new Point(300, num * 50);
                    //this.Controls.Add(l3);
                    Label l4 = new Label();
                    l4.Name = "l4";
                    l4.AutoSize = true;
                    l4.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    l4.Text = myrec["start_time"] + "";
                    l4.Location = new Point(400, num * 50);
                    //this.Controls.Add(l4); 
                    Label l5 = new Label();
                    l5.Name = "l5";
                    l5.AutoSize = true;
                    l5.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    l5.Text = myrec["end_time"] + "";
                    l5.Location = new Point(500, num * 50);
                    //this.Controls.Add(l5);
                    Label l6 = new Label();
                    l6.Name = "l6";
                    l6.AutoSize = true;
                    l6.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    l6.Text = myrec["service"] + "";
                    l6.Location = new Point(600, num * 50);
                    Label l7 = new Label();
                    l7.Name = "l7";
                    l7.AutoSize = true;
                    l7.Font = new Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    l7.Text = myrec["cost"] + "";
                    l7.Location = new Point(700, num * 50);
                    panelControler.Controls.Add(l1);
                    panelControler.Controls.Add(l2);
                    panelControler.Controls.Add(l3);
                    panelControler.Controls.Add(l4);
                    panelControler.Controls.Add(l5);
                    panelControler.Controls.Add(l6);
                    panelControler.Controls.Add(l7);
                    panelControler.AutoScroll = true;
                }

                myrec.Close();
                mycon.Close();
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (chartControl.Visible == false)
            {
                chartControl.Visible = true;
                btnShow.Text = "Hide";
            }
            else if (chartControl.Visible == true)
            {
                chartControl.Visible = false;
                btnShow.Text = "Show";
            }

            int maxMonth = 0;
            int maxDay = 0;
            int minMonth = 13;
            int minDay = 32;
            int firstMon = 0;
            int firstDay = 0;
            int num = 0;

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

            if (!"all".Equals(card.Text))
            {
                if ((!"".Equals(card.Text)) && "".Equals(phone.Text))
                {
                    num = 0;
                    mycom = new MySqlCommand("select * from customer_consumption where id = '" + card.Text + "'", mycon);
                    myrec = mycom.ExecuteReader();

                    while (myrec.Read())
                    {
                        string date = myrec["date"] + "";
                        chartControl.Series[0].Points.Add(new SeriesPoint(date, new object[] { ((object)(int.Parse(myrec["cost"] + ""))) }));
                        int tmp = int.Parse(date.Split(' ')[0].Split('/')[1]);
                        int tmpDay = int.Parse(date.Split(' ')[0].Split('/')[2]);
                        if (num == 0)
                        {
                            phone.Text = myrec["phone"] + "";
                            firstDay = tmpDay;
                            firstMon = tmp;
                        }
                        if (tmp >= maxMonth)
                        {
                            if (tmp == maxMonth && tmpDay > maxDay)
                                maxDay = tmpDay;
                            else
                                maxMonth = tmp;
                        }
                        if (tmp <= minMonth)
                        {
                            if (tmp == minMonth && tmpDay < minDay)
                                minDay = tmpDay;
                            else
                                minMonth = tmp;
                        }
                        num++;
                    }

                }
                else if ((!"".Equals(phone.Text)) && "".Equals(card.Text))
                {
                    num = 0;
                    mycom = new MySqlCommand("select * from customer_consumption where phone = '" + phone.Text + "'", mycon);
                    myrec = mycom.ExecuteReader();

                    while (myrec.Read())
                    {
                        string date = myrec["date"] + "";
                        chartControl.Series[0].Points.Add(new SeriesPoint(date, new object[] { ((object)(int.Parse(myrec["cost"] + ""))) }));
                        int tmp = int.Parse(date.Split(' ')[0].Split('/')[1]);
                        int tmpDay = int.Parse(date.Split(' ')[0].Split('/')[2]);
                        if (num == 0)
                        {
                            card.Text = myrec["id"] + "";
                            firstDay = tmpDay;
                            firstMon = tmp;
                        }
                        if (tmp >= maxMonth)
                        {
                            if (tmp == maxMonth && tmpDay > maxDay)
                                maxDay = tmpDay;
                            else
                                maxMonth = tmp;
                        }
                        if (tmp <= minMonth)
                        {
                            if (tmp == minMonth && tmpDay < minDay)
                                minDay = tmpDay;
                            else
                                minMonth = tmp;
                        }
                        num++;
                    }
                }
                else if ((!"".Equals(phone.Text)) && (!"".Equals(card.Text)))
                {
                    num = 0;
                    mycom = new MySqlCommand("select * from customer_consumption where phone = '" + phone.Text + "'", mycon);
                    myrec = mycom.ExecuteReader();

                    while (myrec.Read())
                    {
                        string date = myrec["date"] + "";
                        chartControl.Series[0].Points.Add(new SeriesPoint(date, new object[] { ((object)(int.Parse(myrec["cost"] + ""))) }));
                        int tmp = int.Parse(date.Split(' ')[0].Split('/')[1]);
                        int tmpDay = int.Parse(date.Split(' ')[0].Split('/')[2]);
                        if (num == 0)
                        {
                            firstDay = tmpDay;
                            firstMon = tmp;
                        }
                        if (tmp >= maxMonth)
                        {
                            if (tmp == maxMonth && tmpDay > maxDay)
                                maxDay = tmpDay;
                            else
                                maxMonth = tmp;
                        }
                        if (tmp <= minMonth)
                        {
                            if (tmp == minMonth && tmpDay < minDay)
                                minDay = tmpDay;
                            else
                                minMonth = tmp;
                        }
                        num++;
                    }
                }
                else
                    return;
            }
            else
            {
                mycom = new MySqlCommand("select * from customer_consumption", mycon);
                myrec = mycom.ExecuteReader();
                num = 0;
                while (myrec.Read())
                {
                    string date = myrec["date"] + "";
                    chartControl.Series[0].Points.Add(new SeriesPoint(date, new object[] { ((object)(int.Parse(myrec["cost"] + ""))) }));
                    int tmp = int.Parse(date.Split(' ')[0].Split('/')[1]);
                    int tmpDay = int.Parse(date.Split(' ')[0].Split('/')[2]);
                    if (num == 0)
                    {
                        firstDay = tmpDay;
                        firstMon = tmp;
                    }
                    if (tmp >= maxMonth)
                    {
                        if (tmp == maxMonth && tmpDay > maxDay)
                            maxDay = tmpDay;
                        else
                            maxMonth = tmp;
                    }
                    if (tmp <= minMonth)
                    {
                        if (tmp == minMonth && tmpDay < minDay)
                            minDay = tmpDay;
                        else
                            minMonth = tmp;
                    }
                    num++;
                }
            }
            int max = (maxMonth - minMonth) * 31 + (maxDay - minDay);
            int min = (minMonth - maxMonth) * 31 - (maxDay - minDay);
            XYDiagram xyd = (XYDiagram)this.chartControl.Diagram;
            xyd.AxisX.Range.MaxValueInternal = max + 1.0D;
            xyd.AxisX.Range.MinValueInternal = min - 2.5D;
            xyd.EnableAxisXScrolling = true;
            xyd.EnableAxisYScrolling = true;
            myrec.Close();
            mycon.Close();
        }

        private void card_Click(object sender, EventArgs e)
        {
            if ("id".Equals(card.Text))
                card.Text = "";
        }

        private void phone_Click(object sender, EventArgs e)
        {
            if ("phone".Equals(phone.Text))
                phone.Text = "";
        }

    }
}
