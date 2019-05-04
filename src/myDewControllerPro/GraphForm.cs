using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myDewControllerPro3
{
    public partial class GraphForm : Form
    {
        public string myTime;                           // hold time at which temperature value is plotted
        public int numberofpointplots = 9;              // 9 display points on the graph
        public int hadpplotcount = 0;                  // used to start deleting first value and thus scroll chart points
        public int chpwrplotcount = 0;
        public int chtempplotcount = 0;
        public int fspcbtplotcount = 0;

        public GraphForm()
        {
            InitializeComponent();
            // Inserted code into this for handling window resizing
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            // this.MinimizeBox = false;  // do not as user cannot minimize form!!!
        }

        private void GraphForm_Load(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            this.Location = Properties.Settings.Default.GraphFormLocation;

            tabControl.SelectedIndex = 0;                   // Channel 1/2/3 temperature tab
            // tabControl.SelectedTab = tabPage1;           // alternative way of selecting tab page
            numberofpointplots = 9;

            hadpplotcount = 0;
            chtempplotcount = 0;
            chpwrplotcount = 0;

            fspcbtplotcount = 0;

            chart1.Visible = false;
            chart2.Visible = false;
            chart3.Visible = true;
            chart4.Visible = false;

            // turn off the vertical lines in the chart
            chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;    // humidity/ambient/dewpoint
            chart2.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;    // ch 1/2/3 power
            chart3.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;    // ch 1/2/3 temperatures
            chart4.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;    // bme280

            // set labels on x axis - spacing between datapoints = 1 label per datapoint
            chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            chart2.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            chart3.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            chart4.ChartAreas["ChartArea1"].AxisX.Interval = 1;

            // set label style x axis to 90 so they are vertical
            chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = 90;
            chart2.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = 90;
            chart3.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = 90;
            chart4.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = 90;
        }

        private void HideBtn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.GraphFormLocation = this.Location;
            Properties.Settings.Default.Save();
            this.Hide();
            return;
        }

        // add a temperature value to the chart
        public void AddToChart(double datavalue1, double datavalue2, double datavalue3, int pos)
        {
            String TimeNow;
            String myTime = "";

            chart1.ChartAreas[0].RecalculateAxesScale();
            chart2.ChartAreas[0].RecalculateAxesScale();
            chart3.ChartAreas[0].RecalculateAxesScale();
            chart4.ChartAreas[0].RecalculateAxesScale();

            // Calculate current time as this needs to be plotted on Graph X
            TimeNow = DateTime.Now.TimeOfDay.ToString();       // get current time
            for (int lpval = 0; lpval <= 7; lpval++)           // hh:mm:ss.nnnnn
            {
                myTime += TimeNow[lpval];
            }
            myTime += "";                                       // hh.mm.ss

            // increment the number of plotted points and if exceeds number of points on the chart 
            // then delete the first point and thus the plotted points will scroll
            switch (pos)
            {
                case 0:     // humidity, ambient and dewpoint
                    hadpplotcount++;
                    if (hadpplotcount > numberofpointplots)
                    {
                        try
                        {
                            chart1.Series["Humidity"].Points.RemoveAt(0);
                            chart1.Series["Ambient"].Points.RemoveAt(0);
                            chart1.Series["DewPoint"].Points.RemoveAt(0);

                        }
                        catch (NullReferenceException)
                        {
                            // do nothing
                            return;
                        }
                    }
                    // add the point (temperature, time) to the chart
                    // chart1.Series["Temp"].XValueType = ChartValueType.Time;
                    try
                    {
                        chart1.Series["Humidity"].Points.AddXY(myTime, datavalue1);
                        chart1.Series["Ambient"].Points.AddXY(myTime, datavalue2);
                        chart1.Series["DewPoint"].Points.AddXY(myTime, datavalue3);
                    }
                    catch (NullReferenceException)
                    {
                        // do nothing
                        return;
                    }
                    break;
                case 1:     // channel 1/2/3 temperature - chart3
                    chtempplotcount++;
                    if (chtempplotcount > numberofpointplots)
                    {
                        try
                        {
                            chart3.Series["Channel1 Temp"].Points.RemoveAt(0);
                            chart3.Series["Channel2 Temp"].Points.RemoveAt(0);
                            chart3.Series["Channel3 Temp"].Points.RemoveAt(0);
                        }
                        catch (NullReferenceException)
                        {
                            // do nothing
                            return;
                        }
                    }
                    // add the point (temperature, time) to the chart
                    // chart3.Series["Temp"].XValueType = ChartValueType.Time;
                    try
                    {
                        chart3.Series["Channel1 Temp"].Points.AddXY(myTime, datavalue1);
                        chart3.Series["Channel2 Temp"].Points.AddXY(myTime, datavalue2);
                        chart3.Series["Channel3 Temp"].Points.AddXY(myTime, datavalue3);
                    }
                    catch (NullReferenceException)
                    {
                        // do nothing
                        return;
                    }
                    break;
                case 2:     // Ch1/2/3 Power - chart2
                    chpwrplotcount++;
                    if (chpwrplotcount > numberofpointplots)
                    {
                        try
                        {
                            chart2.Series["Channel1 Pwr"].Points.RemoveAt(0);
                            chart2.Series["Channel2 Pwr"].Points.RemoveAt(0);
                            chart2.Series["Channel3 Pwr"].Points.RemoveAt(0);
                        }
                        catch (NullReferenceException)
                        {
                            // do nothing
                            return;
                        }
                    }
                    // add the point (temperature, time) to the chart
                    // chart2.Series["Temp"].XValueType = ChartValueType.Time;
                    try
                    {
                        chart2.Series["Channel1 Pwr"].Points.AddXY(myTime, datavalue1);
                        chart2.Series["Channel2 Pwr"].Points.AddXY(myTime, datavalue2);
                        chart2.Series["Channel3 Pwr"].Points.AddXY(myTime, datavalue3);
                    }
                    catch (NullReferenceException)
                    {
                        // do nothing
                        return;
                    }
                    break;
                case 3:     // pcbtemp
                    fspcbtplotcount++;
                    if (fspcbtplotcount > numberofpointplots)
                    {
                        try
                        {
                            chart4.Series["PCBTemp"].Points.RemoveAt(0);
                            chart4.Series["FanSpeed"].Points.RemoveAt(0);
                        }
                        catch (NullReferenceException)
                        {
                            // do nothing
                            return;
                        }
                    }
                    // add the point (temperature, time) to the chart
                    // chart1.Series["Temp"].XValueType = ChartValueType.Time;
                    try
                    {
                        chart4.Series["PCBTemp"].Points.AddXY(myTime, datavalue2);
                        chart4.Series["FanSpeed"].Points.AddXY(myTime, datavalue1);
                    }
                    catch (NullReferenceException)
                    {
                        // do nothing
                        return;
                    }
                    break;
            }
        }

        private void GraphForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.GraphFormLocation = this.Location;
            Properties.Settings.Default.Save();
            // do not allow form to close
            e.Cancel = true;
        }

        private void tabControl_Click(object sender, EventArgs e)
        {
            switch (tabControl.SelectedIndex)
            {
                case 0: // Ch1/2/3 Temperature
                    chart1.Visible = false;
                    chart2.Visible = false;
                    chart3.Visible = true;
                    chart4.Visible = false;
                    break;
                case 1: // Ch1/2/3 Power
                    chart1.Visible = false;
                    chart2.Visible = true;
                    chart3.Visible = false;
                    chart4.Visible = false;
                    break;
                case 2: // ambient and object temp
                    chart1.Visible = true;
                    chart2.Visible = false;
                    chart3.Visible = false;
                    chart4.Visible = false;
                    break;
                case 3: // pcbtemp/fan speed
                    chart1.Visible = false;
                    chart2.Visible = false;
                    chart3.Visible = false;
                    chart4.Visible = true;
                    break;
            }
        }

        private void ClearChTempDataBtn_Click(object sender, EventArgs e)
        {
            // ch1/2/3 temperature
            chart3.Series["Channel1 Temp"].Points.Clear();
            chart3.Series["Channel2 Temp"].Points.Clear();
            chart3.Series["Channel3 Temp"].Points.Clear();
            chtempplotcount = 0;
        }

        private void ClearCHPwrDataPointsBtn_Click(object sender, EventArgs e)
        {
            chart2.Series["Channel1 Pwr"].Points.Clear();
            chart2.Series["Channel2 Pwr"].Points.Clear();
            chart2.Series["Channel3 Pwr"].Points.Clear();
            chpwrplotcount = 0;
        }

        private void ClearAHDDataPointsBtn_Click(object sender, EventArgs e)
        {
            chart1.Series["Humidity"].Points.Clear();
            chart1.Series["Ambient"].Points.Clear();
            chart1.Series["DewPoint"].Points.Clear();
            hadpplotcount = 0;
        }

        private void PCBFanClearDataPointsBtn_Click(object sender, EventArgs e)
        {
            chart4.Series["PCBTemp"].Points.Clear();
            chart4.Series["FanSpeed"].Points.Clear();
            fspcbtplotcount = 0;
        }

        public void clearpoints()
        {
            //this.Invoke(new EventHandler(ClearChTempDataBtn_Click));
            //this.Invoke(new EventHandler(ClearCHPwrDataPointsBtn_Click));
            //this.Invoke(new EventHandler(ClearAHDDataPointsBtn_Click));
            //this.Invoke(new EventHandler(PCBFanClearDataPointsBtn_Click));
            // ch1/2/3 temperature
            chart3.Series["Channel1 Temp"].Points.Clear();
            chart3.Series["Channel2 Temp"].Points.Clear();
            chart3.Series["Channel3 Temp"].Points.Clear();
            chtempplotcount = 0;
            chart2.Series["Channel1 Pwr"].Points.Clear();
            chart2.Series["Channel2 Pwr"].Points.Clear();
            chart2.Series["Channel3 Pwr"].Points.Clear();
            chpwrplotcount = 0;
            chart1.Series["Humidity"].Points.Clear();
            chart1.Series["Ambient"].Points.Clear();
            chart1.Series["DewPoint"].Points.Clear();
            hadpplotcount = 0;
            chart4.Series["PCBTemp"].Points.Clear();
            chart4.Series["FanSpeed"].Points.Clear();
            fspcbtplotcount = 0;
        }
    }
}
