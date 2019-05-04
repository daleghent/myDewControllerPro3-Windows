using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Media;             // required to play sounds
using System.Globalization;
using WMPLib;

// 3.0.5.8
// small fixes

// 3.0.5.7
// Redesign GUI using tab controls to minimize screen real-estate
// Reset signed certificate date to 5/03/2024 4:09:37 PM

// 3.0.5.6
// Fix error in application settings

// 3.0.5.5
// retrieve pcb temp off value when connecting

// 3.0.5.4
// Add PCB Temp OFF setting

// 3.0.5.3
// fix logging of error messages upon exception [sometimes ignored]

// 3.0.5.2 29032018
// Add serial port speed, 57600 for USB, 9600 for BT

// 3.0.5.1 17032018
// Display temperature and dewpoint values to 2 decimal places

// 3.0.5.0 10022018
// Fix error in data log file not being able to be viewed by logviewerpro

// 3.0.4.9 04022108
// Fix errors in handling controller revision number
// Add debug code for standalone testing

// 3.0.4.8 23012018
// Change GUI layout and resize form to fit smaller screens
// Fix minor errors in Celsius/Fahrenheit, Updating, Delays in serial routines etc

// 3.0.4.6 17012018
// Graph data now shown on separate form

// 3.0.4.5 02122017
// Change app locale to eng-us

// 3.0.4.4 29112017
// Changes to data logging to handle windows locale settings
// Add boardtemp to datalog

// 3.0.4.2 10072017
// Fix for ch1/ch2/ch3 temp offset value settings re Windows Location; eg; Europe
// Check and validate offsets on application start

// 3.0.4.0 29052017
// Support for Board temp sensor which regulates fan setting - Fritzing PCB Rev02 only, firmware v3.18 or higher
// Add menu setting to reset controller settings to default values

// 3.0.3.4 14092016
// Changes to error handling

// 3.0.3.3 29082016
// Fix for app staying resident in memory after closing (rare occurrence)

// 3.0.3.2 27062016
// Increased temperature offset for channels 1-3 to -3.5 to +3.5 and changed type to Double from float

// 3.0.3.1 20042016
// Error in humidity fixed

// 3.0.3.0 11042016
// Minor changes

// v3.0.2.9
// Graph plotcount was not being reset when connect-disconnect-connect

// v3.0.2.8
// Changes to return strings for protocol v303, must use v309 firmware or higher
// cannot use with firmware v308 or lower
// Remove of menu item - test if controller connected

// v3.0.2.7
// Rewritten to use new protocol v302

// v3.0.2.6
// Fixed bug in Ch3 temp decimal places

// v3.0.2.5
// Added macTrackBar control to replace MS trackbar
// Fixed some spelling mistakes, cleaned up error logging messages, added tooltips for ch3

// v3.0.2.3
// Changes for Win10 compatibility and size of form to fit lower resolution screens

// v3.0.2.1
// First general public release

// v3.0.1.9
// Fixed error is setting one of the settings menu options for ch3

// v3.0.1.7 
// Removed support for older display screens, '.' and '-', was running short on controller program memory space
// Now only the new style is supported, menu option is removed

// v3.0.1.6
// Error in E command handling ch3pwr on Connect()

// v3.0.1.5
// Make data log file compatible with myLogViewerPro

// v3.0.1.3
// minor fixes to manual updating, some routines were being called twice
// fixed bug in ch3 pwr handling
// fixed bug in ch3 menu settings 

// v3.0.1.2
// altered some of the delays in Connect
// small fixes in value states and disabled ch3pwr set button before connect and on disconnect
// set for ch1/2/3 offsets was not bound checking the values before sending to controller
// changed E# command for ch3 to return mode, temp and pwr values for ch3
// added extra page to LCD1602 displays so ch3 data is displayed
// retrieve lcddisplaytime on connect() and set the settings menu 
// disabled some menu items when not connected...., they get enabled when a connection is established to a valid controller

// v3.0.1.1
// using GUID cdb7addd-fcbf-487f-8774- allows both v2 and v3 to co-exist 
// changed names of logfile and errorlog so no conflict with v2
// changed assembly to myDewControllerPro3 so not to conflict with v2 myDewControllerPro
// Added ch3 temp and pwr values to chart, fixed updating via protocol (301) command changes
// Added extra values to be exported by the datalogging function
// Added alternative display style for LCD2004 display, under "Settings" "LCD2004 Display Styles
// Added more logging messages
// Changed serial read timeout value to allow for changes in Arduino Firmware v3x to respond on Connect calls
// Fixed bug in not retrieving channel 3 temp if enabled
// Added ch3 and probe3 data to datalog file, fixed bugs in ch3
// added ch3 offsets and temp probe3
// on E# the controller returns ch3 power setting so update the value shown in text box 
// Added third dew channel, extra commands, only works with v300 or greater hardware

namespace myDewControllerPro3
{
    public partial class myDewController : Form
    {
        // public System.IO.Ports.SerialPort myserialPort; 
        static Mutex sermutex = new Mutex(true);
        public string myVersion;
        private double ch1temp = 0;         // channel 1 temperature reading
        private double ch2temp = 0;         // channel 2 temperature reading
        private double ch3temp = 0;         // channel 3 temperature reading
        private double ambient = 15.2;      // ambient temperature reading
        private double humidity = 44;       // relative humidity reading
        private int ch1pwr = 0;             // channel 1 power setting
        private int ch2pwr = 0;             // channel 2 power setting
        private int ch3pwr = 0;             // channel 3 power setting
        private double dewpoint = 0;        // calculated dew point
        private bool soundenabled;          // enable sound for indicating update completed
        private bool Overridech1;	        // power override enable for channel 1
        private bool Overridech2;           // power override enable for channel 2
        private int OffsetVal = 0;          // this is tracking offset value for temperature band range shift
        private int ATBiasVal = 0;          // this is ambient temperature offset value
        private int trackingmode = 1;       // 1 = ambient, 2 = dew point, 3 = mid-point tracking mode
        private int fanspeed = 0;           // fan speed setting, off
        private int boardtemp = 0;          // temp setting for board at which fan turns ON
        private int fantempsetting = 0;
        public const char cmdterminator = ((char)35);    // # the character that means end of command value
        public const char strterminator = ((char)36);    // $ end of recd command string
        private bool comconnected = false;  // indicates if comport is connected
        private bool updatingvals;          // indicates that a command string has been received from controller
        // and we are in process of updating values so please ignore request
        // to change values;
        public string ComPortName;          // hold comport name of last used comport
        public string ComPortBaudRate;      // holds com port baud rate
        public float ch1tempoffsetval;      // used within ch1tempoffTxtBox_KeyPress
        public float ch2tempoffsetval;      // used within ch2tempoffTxtBox_KeyPress
        public float ch3tempoffsetval;      // used within ch3tempoffTxtBox_KeyPress

        public Boolean controllerresponse;  // used to query if connected to controller
        public String controllerresponsestring1;    // should hold DCOK if successful
        public String controllerresponsestring2;    // should hold DCOK if successful

        public static OpenFileDialog ofd = new OpenFileDialog();  // for data logging
        public string LogDirPath;           // directory path of where datalog file is 
        public string logtimestamp;         // timestamp of data sample
        public string logfilename;          // name of logfile for data
        public string proglogfilename;      // name of logfile for program errors/flow
        public bool logerrorfile;           // log to error file, yes | no
        public static string cpystrbrown = "myDewControllerPro3 ©RB Brown 2014-2019. ";
        public string CopyrightStr = "\n© " + cpystrbrown + "\n All Rights Reserved.\n\n" +
"The schematic, code and ideas are released into the public domain. Users are free to implement these but " +
"may NOT sell projects based on this project for commercial gain without express written permission " +
"granted from the author.\n\nSchematics, Code, Firmware, Ideas, Software Applications, Layout are protected by " +
"Copyright Law. Permission is NOT granted to any person to redistribute, market, manufacture or " +
"sell for commercial gain the myDewController2 or myDewController3 products, ideas, circuits, builds, variations and units as described, discussed and shown. " +
"\n\nPermission is granted for personal and Academic/Educational use only.\n\n";

        public ErrorLogPathName elogpathfrm;
        public GraphForm graphfrm;

        public const int Celsius = 0;
        public const int Fahrenheit = 1;
        public int DisplayMode;             // celsius or fahrenheit
        public int numberoftempprobes;      // number of temperature probes
        public Boolean commandinprogress = false;
        public string ArduinoFirmwareRev;
        public Double ControllerRev;
        public char MajorRevNumber;             // updated whenever the firmware version is retrieved from controller, also on connect()
        public int ch3mode = 0;                 // used to write to datalogfile
        public Boolean Done = false;            // set true after connected and all values retrieved from controller
        // prevents trigger event for control happening when updating values when connecting to controller

        public CultureInfo thisCulture;
        public CultureInfo thisICCulture;
        public CultureInfo newCulture;
        public CultureInfo newICCulture;

        public Boolean testmode;

        public myDewController()
        {
            InitializeComponent();
            // Inserted code into this for handling window resizing
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            // this.MinimizeBox = false;  // do not as user cannot minimize form!!!
        }

        public void PauseForTime(int myseconds, int mymseconds)
        {
            LogMessageToFile("PauseForTime " + myseconds.ToString() + "s " + mymseconds.ToString() + "ms");
            // this is a generic wait that works with all versions of .NET
            System.DateTime ThisMoment = System.DateTime.Now;
            System.TimeSpan duration = new System.TimeSpan(0, 0, 0, myseconds, mymseconds);
            // System.TimeSpan( days, hrs, mins, secs, millisecs);
            System.DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = System.DateTime.Now;
            }
        }

        public void LogMessageToFile(string msg)
        {
            sermutex.WaitOne();
            if (logerrorfile == true)
            {
                System.IO.StreamWriter sw = System.IO.File.AppendText(
                Properties.Settings.Default.errorlogpath + "\\" +
                Properties.Settings.Default.ErrorLogName);

                try
                {
                    string logLine = System.String.Format("{0:G}: {1}", System.DateTime.Now, msg);
                    sw.WriteLine(logLine);
                }
                finally
                {
                    sw.Close();
                }
            }
            sermutex.ReleaseMutex();
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            LogMessageToFile("ExitBtn: Start");
            if (serialPort1.IsOpen)    // com port is open, so tidy up
            {
                LogMessageToFile("ExitBtn: error - Serial Port is open");
                MessageBox.Show("Disconnect the Serial Port before exiting", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Information);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else                    // Port is closed, save settings and exit
            {
                LogMessageToFile("ExitBtn: Exiting");
                LogMessageToFile("ExitBtn: Saving application settings");
                Properties.Settings.Default.Save();
                LogMessageToFile("myDewControllerPro3 Version: " + myVersion + "\n");
                LogMessageToFile("ExitBtn: Closed");
                // all tidy up is done in form_closing
                this.Close();
            }
        }

        public double GetRandomDoubleNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        public int GetRandomIntNumber(int minimum, int maximum)
        {
            Random random = new Random();
            return random.Next(minimum, maximum);
        }

        public void setvalues()
        {
            // test values here   
            humidity = 75 + GetRandomDoubleNumber(0.25, 2.5);
            humidity = Math.Round(humidity, 2);
            ambient = 23.2 + GetRandomDoubleNumber(-0.25, 1.5);
            ambient = Math.Round(ambient, 2);
            dewpoint = 19.01 + GetRandomDoubleNumber(-0.25, 0.75);
            dewpoint = Math.Round(dewpoint, 2);
            ch1pwr = 20 + GetRandomIntNumber(-10, 10);
            ch2pwr = 50 + GetRandomIntNumber(-10, 10);
            ch3pwr = 75 + GetRandomIntNumber(-10, 10);
            ch1temp = 21.1 + GetRandomDoubleNumber(-0.25, 1.5);
            ch1temp = Math.Round(ch1temp, 2);
            ch2temp = 24.2 + GetRandomDoubleNumber(-0.25, 1.5);
            ch2temp = Math.Round(ch2temp, 2);
            ch3temp = 26.3 + GetRandomDoubleNumber(-0.25, 1.5);
            ch3temp = Math.Round(ch3temp, 2);
            boardtemp = 34 + GetRandomIntNumber(-2, 2);
            fanspeed = 75 + GetRandomIntNumber(-25, 25);
            relativeHumidityTxtBox.Text = humidity.ToString();
            relativeHumidityTxtBox.Update();
            ambientTemperatureTxtBox.Text = ambient.ToString();
            ambientTemperatureTxtBox.Update();
            dewpointTxtBox.Text = dewpoint.ToString();
            dewpointTxtBox.Update();
            ch1tempTxtBox.Text = ch1temp.ToString();
            ch1tempTxtBox.Update();
            ch2tempTxtBox.Text = ch2temp.ToString();
            ch2tempTxtBox.Update();
            ch3tempTxtBox.Text = ch3temp.ToString();
            ch3tempTxtBox.Update();
            pcbtempTxtBox.Text = boardtemp.ToString();
            pcbtempTxtBox.Update();
            if (DisplayMode == Celsius)
                this.Invoke(new EventHandler(celsiusToolStripMenuItem_Click));
            else if (DisplayMode == Fahrenheit)
                this.Invoke(new EventHandler(fahrenheitToolStripMenuItem_Click));

            ch1pwrTxtBox.Text = ch1pwr.ToString();
            ch1pwrTxtBox.Update();
            ch2pwrTxtBox.Text = ch2pwr.ToString();
            ch2pwrTxtBox.Update();
            ch3PwrTrackBar.Value = ch3pwr;
            ch3PwrTrackBar.Update();
            ControllerVersionTxtBox.Text = "3.20DHT";
            ControllerVersionTxtBox.Update();
            LogDirNametxtBox.Text = "D:\\";
            LogDirNametxtBox.Update();
            comboBox1.Items.Add("COM1");
            comboBox1.Update();
            comboBox1.SelectedIndex = 0;
            TrackModeAmbient.Checked = true;
            TrackModeDewPoint.Checked = false;
            TrackModeMidPoint.Checked = false;
            FanSpeed75.Checked = true;
            FanSpeed50.Checked = false;
            FanSpeedZero.Checked = false;
            FanSpeed100.Checked = false;
            ATBiasMinus1.Checked = false;
            ATBiasMinus2.Checked = false;
            ATBiasMinus3.Checked = false;
            ATBiasMinus4.Checked = false;
            ATBiasPlus1.Checked = true;
            ATBiasPlus2.Checked = false;
            ATBiasPlus3.Checked = false;
            ATBiasZero.Checked = false;
            fantemponTxtBox.Text = "20";
            fantemponTxtBox.Update();
            myAddToChart();
        }

        public void myAddToChart()
        {
            // insert dummy values for testing
            // setvalues();

            // graph ambient
            if (graphfrm != null)
            {
                // graphfrm.Show();
                try
                {
                    graphfrm.AddToChart(humidity, ambient, dewpoint, 0);
                    LogMessageToFile("Add humidity/ambient/dewpoint value to chart: " + humidity.ToString() + ", " + ambient.ToString() + ", " + dewpoint.ToString());
                }
                catch (ObjectDisposedException)
                {
                    // object was disposed because window form was closed with X
                    // recreate object
                    graphfrm = new GraphForm();
                    graphfrm.Show();
                    graphfrm.AddToChart(humidity, ambient, dewpoint, 0);
                }
            }
            // graph ch1/2/3
            if (graphfrm != null)
            {
                // graphfrm.Show();
                try
                {
                    graphfrm.AddToChart(ch1temp, ch2temp, ch3temp, 1);
                    LogMessageToFile("Add ch1/2/3 temp value to chart: " + ch1temp.ToString() + ", " + ch2temp.ToString() + ", " + ch3temp.ToString());
                }
                catch (ObjectDisposedException)
                {
                    // object was disposed because window form was closed with X
                    // recreate object
                    graphfrm = new GraphForm();
                    graphfrm.Show();
                    graphfrm.AddToChart(ch1temp, ch2temp, ch3temp, 1);
                }
            }
            // graph ch1/2/3 pwr
            if (graphfrm != null)
            {
                // graphfrm.Show();
                try
                {
                    graphfrm.AddToChart(ch1pwr, ch2pwr, ch3pwr, 2);
                    LogMessageToFile("Add ch1/2/3 power value to chart: " + ch1pwr.ToString() + ", " + ch2pwr.ToString() + ", " + ch3pwr.ToString());
                }
                catch (ObjectDisposedException)
                {
                    // object was disposed because window form was closed with X
                    // recreate object
                    graphfrm = new GraphForm();
                    graphfrm.Show();
                    graphfrm.AddToChart(ch1pwr, ch2pwr, ch3pwr, 2);
                }
            }
            // graph fanspeed/pcbtemp
            if (graphfrm != null)
            {
                // graphfrm.Show();
                try
                {
                    graphfrm.AddToChart(fanspeed, (double)boardtemp, 0, 3);
                    LogMessageToFile("Add fanspeed/boardtemp value to chart: " + fanspeed.ToString() + ", " + boardtemp.ToString());
                }
                catch (ObjectDisposedException)
                {
                    // object was disposed because window form was closed with X
                    // recreate object
                    graphfrm = new GraphForm();
                    graphfrm.Show();
                    graphfrm.AddToChart(fanspeed, (double)boardtemp, 0, 3);
                }
            }
        }

        // use this to send a command to the controller when a response is required
        // send the command string to the controller and wait for a response
        private string CommandString(string Command)
        {
            string cmd = Command;		// save the command into cmd
            string recbuf = "";			// clear the receive buffer
            string tempstr = "";
            int mypos = 0;
            string statusmsg = "";

            LogMessageToFile("CommandString Send: " + Command);

            // check to see if the serial port is open, if so then send command
            if (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.DiscardInBuffer();     // clear buffer
                    serialPort1.Write(Command);        // send the command

                    // check what to do for each command
                    // not all commands have return strings

                    // get ambient
                    // A#	Avalue$		Returns Ambient temperature in C
                    if (cmd == "A#")
                    {
                        LogMessageToFile("CommandString: cmd = " + cmd);
                        LogMessageToFile("CommandString: get ambient");
                        tempstr = "";
                        // try to read response from serial port
                        try
                        {
                            recbuf = serialPort1.ReadTo("$");
                            // add back the end of command terminator
                            recbuf = recbuf + "$";
                        }
                        // handle exception
                        catch (TimeoutException)
                        {
                            LogMessageToFile("CommandString: A# Timeout exception raised");
                            statusmsgTxtBox.Text = "Timeout exception raised A#";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }
                        LogMessageToFile("CommandString response recd: A#: " + recbuf);
                        // no need to strip off terminator
                        // update the position
                        // this is ambient #value$
                        if (recbuf == "")
                        {
                            statusmsgTxtBox.Text = "Null response A#";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }
                        else
                        {
                            // recbuff is Aambient$, extract just the version
                            tempstr = "";
                            for (mypos = 1; Convert.ToChar(recbuf[mypos]) != '$'; mypos++)
                            {
                                tempstr = tempstr + recbuf[mypos];
                            }
                            // this is ambient temp
                            try
                            {
                                LogMessageToFile("CommandString: try to convert response for ambient=" + tempstr);
                                ambient = Double.Parse(tempstr, newCulture);
                                LogMessageToFile("CommandString: convert OK ambient = " + Convert.ToString(ambient, newCulture));
                                // display in textbox
                                // controller always returns centigrade values
                                LogMessageToFile("CommandString: ambient=" + Convert.ToString(ambient, newCulture));
                                // first adjust ambient using offset calibration value
                                ambient = ambient + ATBiasVal;
                                ambient = Math.Round(ambient, 2);

                                if (celsiusToolStripMenuItem.Checked)
                                {
                                    ambientTemperatureTxtBox.Text = Convert.ToString(ambient, newCulture);
                                    ambientTemperatureTxtBox.Update();
                                    statusmsgTxtBox.Text = "AT=" + Convert.ToString(ambient, newCulture);
                                    statusmsgTxtBox.Update();
                                    atlabel.Text = "C";
                                    atlabel.Update();
                                }
                                else
                                {
                                    double tempC, tempF;
                                    try
                                    {
                                        tempC = ambient;
                                        tempF = (tempC * 1.8) + 32;
                                        tempF = Math.Round(tempF, 2);
                                        ambientTemperatureTxtBox.Text = Convert.ToString(tempF, newCulture);
                                        ambientTemperatureTxtBox.Update();
                                        atlabel.Text = "F";
                                        atlabel.Update();
                                    }
                                    catch (FormatException)
                                    {
                                        MessageBox.Show("Format Exception error converting value", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        LogMessageToFile("CommandString: FormatException error");
                                        if (soundenabled)               // beep to indicate response
                                            SystemSounds.Exclamation.Play();
                                        statusmsgTxtBox.Text = "Format exception raised";
                                        statusmsgTxtBox.Update();
                                        commandinprogress = false;
                                        return recbuf;
                                    }
                                    catch (Exception)
                                    {
                                        MessageBox.Show("Exception error converting value", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        LogMessageToFile("CommandString: Exception error");
                                        if (soundenabled)               // beep to indicate response
                                            SystemSounds.Exclamation.Play();
                                        statusmsgTxtBox.Text = "Exception raised";
                                        statusmsgTxtBox.Update();
                                        commandinprogress = false;
                                        return recbuf;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                LogMessageToFile("CommandString: conversion for response ambient error");
                                statusmsgTxtBox.Text = "Error converting ambient temperature = " + tempstr;
                                statusmsgTxtBox.Update();
                                commandinprogress = false;
                                return recbuf;
                            }
                        }
                    }

                    // get humidity
                    // R#	Rvalue$		Returns Relative Humidity
                    else if (cmd == "R#")
                    {
                        LogMessageToFile("CommandString: cmd = " + cmd);
                        LogMessageToFile("CommandString: get humidity");
                        tempstr = "";
                        // try to read response from serial port
                        try
                        {
                            recbuf = serialPort1.ReadTo("$");
                            // add back the end of command terminator
                            recbuf = recbuf + "$";
                        }
                        // handle exception
                        catch (TimeoutException)
                        {
                            LogMessageToFile("CommandString: E# Timeout exception raised");
                            statusmsgTxtBox.Text = "Timeout exception raised R#";
                            statusmsgTxtBox.Update();
                            return recbuf;
                        }
                        LogMessageToFile("CommandString response recd: R: " + recbuf);
                        tempstr = "";
                        for (mypos = 1; Convert.ToChar(recbuf[mypos]) != '$'; mypos++)
                        {
                            tempstr = tempstr + recbuf[mypos];
                        }
                        // this is relative humidity
                        try
                        {
                            LogMessageToFile("CommandString: try to convert response for humdity=" + tempstr);
                            humidity = double.Parse(tempstr, newCulture);
                            humidity = Math.Round(humidity, 2);
                            // display in textbox
                            // controller always returns centigrade values
                            LogMessageToFile("CommandString: convert OK humidity = " + Convert.ToString(humidity, newCulture));
                            relativeHumidityTxtBox.Text = Convert.ToString(humidity, newCulture);
                            statusmsgTxtBox.Text = "RH=" + Convert.ToString(humidity, newCulture);
                            statusmsgTxtBox.Update();
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("CommandString: conversion for response humidity error");
                            statusmsgTxtBox.Text = "Error converting humidity temperature=" + tempstr;
                            statusmsgTxtBox.Update();
                            statusmsgTxtBox.Text = "Exception error - humidity";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }
                    }

                    // get dewpoint
                    // D#	Dvalue$		Returns Dew Point in C
                    else if (cmd == "D#")
                    {
                        LogMessageToFile("CommandString: cmd = " + cmd);
                        LogMessageToFile("CommandString: get dew point");
                        tempstr = "";
                        // try to read response from serial port
                        try
                        {
                            recbuf = serialPort1.ReadTo("$");
                            // add back the end of command terminator
                            recbuf = recbuf + "$";
                        }
                        // handle exception
                        catch (TimeoutException)
                        {
                            LogMessageToFile("CommandString: D# Timeout exception raised");
                            statusmsgTxtBox.Text = "Timeout exception raised D#";
                            statusmsgTxtBox.Update();
                            return recbuf;
                        }
                        LogMessageToFile("CommandString response recd: D: " + recbuf);
                        tempstr = "";
                        for (mypos = 1; Convert.ToChar(recbuf[mypos]) != '$'; mypos++)
                        {
                            tempstr = tempstr + recbuf[mypos];
                        }
                        // this is dewpoint temp
                        try
                        {
                            LogMessageToFile("CommandString: try to convert response for dewpoint=" + tempstr);
                            dewpoint = Double.Parse(tempstr, newCulture);
                            dewpoint = Math.Round(dewpoint, 2);
                            LogMessageToFile("CommandString: convert OK dewpoint = " + Convert.ToString(dewpoint, newCulture));
                            // display in textbox
                            // controller always returns centigrade values
                            LogMessageToFile("CommandString: dewpoint=" + Convert.ToString(dewpoint, newCulture));
                            if (celsiusToolStripMenuItem.Checked)
                            {
                                dewpointTxtBox.Text = Convert.ToString(dewpoint, newCulture);
                                dewpointTxtBox.Update();
                                statusmsgTxtBox.Text = "DP=" + Convert.ToString(dewpoint, newCulture);
                                statusmsgTxtBox.Update();
                                ch1label.Text = "C";
                                ch1label.Update();
                            }
                            else
                            {
                                double tempC, tempF;
                                try
                                {
                                    tempC = dewpoint;
                                    tempF = (tempC * 1.8) + 32;
                                    tempF = Math.Round(tempF, 2);
                                    dewpointTxtBox.Text = Convert.ToString(tempF, newCulture);
                                    dewpointTxtBox.Update();
                                    ch1label.Text = "F";
                                    ch1label.Update();
                                }
                                catch (FormatException)
                                {
                                    MessageBox.Show("Format Exception error converting value", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    LogMessageToFile("CommandString: Format Exception error");
                                    if (soundenabled)               // beep to indicate response
                                        SystemSounds.Exclamation.Play();
                                    statusmsgTxtBox.Text = "Format Exception error - DewPoint";
                                    statusmsgTxtBox.Update();
                                    commandinprogress = false;
                                    return recbuf;
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("Exception error converting value", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    LogMessageToFile("CommandString: Exception error");
                                    if (soundenabled)               // beep to indicate response
                                        SystemSounds.Exclamation.Play();
                                    statusmsgTxtBox.Text = "Exception error - DewPoint";
                                    statusmsgTxtBox.Update();
                                    commandinprogress = false;
                                    return recbuf;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("CommandString: conversion for response dewpoint error");
                            statusmsgTxtBox.Text = "Error converting dewpoint temperature = " + tempstr;
                            statusmsgTxtBox.Update();
                            statusmsgTxtBox.Text = "Exception error - DewPoint";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }
                    }

                    // get ch1/ch2/ch3 temps
                    // C#	Cch1temp#ch2temp#ch3temp$	Returns the temperature in Celsius for Channel1 and Channel2 and Channel3
                    else if (cmd == "C#")
                    {
                        LogMessageToFile("CommandString: cmd = " + cmd);
                        LogMessageToFile("CommandString: get ch1/2/3 temp");
                        tempstr = "";
                        // try to read response from serial port
                        try
                        {
                            recbuf = serialPort1.ReadTo("$");
                            // add back the end of command terminator
                            recbuf = recbuf + "$";
                        }
                        // handle exception
                        catch (TimeoutException)
                        {
                            LogMessageToFile("CommandString: C# Timeout exception raised");
                            statusmsgTxtBox.Text = "Timeout exception raised C#";
                            statusmsgTxtBox.Update();
                            return recbuf;
                        }
                        LogMessageToFile("CommandString response recd: C#: " + recbuf);
                        tempstr = "";
                        for (mypos = 1; Convert.ToChar(recbuf[mypos]) != '#'; mypos++)
                        {
                            tempstr = tempstr + recbuf[mypos];
                        }

                        // this is ch1 temp
                        try
                        {
                            LogMessageToFile("CommandString: try to convert response for ch1 temp=" + tempstr);
                            ch1temp = Double.Parse(tempstr, newCulture);
                            ch1temp = Math.Round(ch1temp, 2);
                            LogMessageToFile("CommandString: convert OK ch1 temp= " + Convert.ToString(ch1temp, newCulture));
                            // display in textbox
                            // controller always returns centigrade values
                            LogMessageToFile("CommandString: ch1 temp=" + Convert.ToString(ch1temp, newCulture));
                            if (celsiusToolStripMenuItem.Checked)
                            {
                                ch1tempTxtBox.Text = Convert.ToString(ch1temp, newCulture);
                                ch1tempTxtBox.Update();
                                statusmsgTxtBox.Text = "Ch1=" + Convert.ToString(ch1temp, newCulture);
                                statusmsgTxtBox.Update();
                                ch1label.Text = "C";
                                ch1label.Update();
                            }
                            else
                            {
                                double tempC, tempF;
                                try
                                {
                                    tempC = ch1temp;
                                    tempF = (tempC * 1.8) + 32;
                                    tempF = Math.Round(tempF, 2);
                                    ch1tempTxtBox.Text = Convert.ToString(tempF, newCulture);
                                    ch1tempTxtBox.Update();
                                    ch1label.Text = "F";
                                    ch1label.Update();
                                }
                                catch (FormatException)
                                {
                                    MessageBox.Show("Format Exception error converting value", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    LogMessageToFile("CommandString: Format Exception error");
                                    if (soundenabled)               // beep to indicate response
                                        SystemSounds.Exclamation.Play();
                                    statusmsgTxtBox.Text = "Format Exception error - ch1 temp";
                                    statusmsgTxtBox.Update();
                                    commandinprogress = false;
                                    return recbuf;
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("Exception error converting value", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    LogMessageToFile("CommandString: Exception error");
                                    if (soundenabled)               // beep to indicate response
                                        SystemSounds.Exclamation.Play();
                                    statusmsgTxtBox.Text = "Exception error - ch1 temp";
                                    statusmsgTxtBox.Update();
                                    commandinprogress = false;
                                    return recbuf;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("CommandString: conversion for response ch1 temp error");
                            statusmsgTxtBox.Text = "Error converting ch1 temperature = " + tempstr;
                            statusmsgTxtBox.Update();
                            statusmsgTxtBox.Text = "Format Exception error - ch1temp";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }

                        // next up is #ch2temp#ch3temp$
                        tempstr = "";
                        mypos++;        // get past #
                        for (; Convert.ToChar(recbuf[mypos]) != '#'; mypos++)
                        {
                            tempstr = tempstr + recbuf[mypos];
                        }
                        // this is ch2 temp
                        try
                        {
                            LogMessageToFile("CommandString: try to convert response for ch2 temp=" + tempstr);
                            ch2temp = Double.Parse(tempstr, newCulture);
                            ch2temp = Math.Round(ch2temp, 2);
                            LogMessageToFile("CommandString: convert OK ch2 temp= " + Convert.ToString(ch2temp, newCulture));
                            // display in textbox
                            // controller always returns centigrade values
                            LogMessageToFile("CommandString: ch2 temp=" + Convert.ToString(ch2temp, newCulture));
                            if (celsiusToolStripMenuItem.Checked)
                            {
                                ch2tempTxtBox.Text = Convert.ToString(ch2temp, newCulture);
                                ch2tempTxtBox.Update();
                                statusmsgTxtBox.Text = "Ch2=" + Convert.ToString(ch2temp, newCulture);
                                statusmsgTxtBox.Update();
                                ch2label.Text = "C";
                                ch2label.Update();
                            }
                            else
                            {
                                double tempC, tempF;
                                try
                                {
                                    tempC = ch2temp;
                                    tempF = (tempC * 1.8) + 32;
                                    tempF = Math.Round(tempF, 2);
                                    ch2tempTxtBox.Text = Convert.ToString(tempF, newCulture);
                                    ch2tempTxtBox.Update();
                                    ch2label.Text = "F";
                                    ch2label.Update();
                                }
                                catch (FormatException)
                                {
                                    MessageBox.Show("Format Exception error converting value", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    LogMessageToFile("CommandString: Format Exception error");
                                    if (soundenabled)               // beep to indicate response
                                        SystemSounds.Exclamation.Play();
                                    statusmsgTxtBox.Update();
                                    statusmsgTxtBox.Text = "Format Exception error - ch2 temp";
                                    statusmsgTxtBox.Update();
                                    commandinprogress = false;
                                    return recbuf;
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("Exception error converting value", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    LogMessageToFile("CommandString: Exception error");
                                    if (soundenabled)               // beep to indicate response
                                        SystemSounds.Exclamation.Play();
                                    statusmsgTxtBox.Update();
                                    statusmsgTxtBox.Text = "Exception error - ch2 temp";
                                    statusmsgTxtBox.Update();
                                    commandinprogress = false;
                                    return recbuf;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("CommandString: conversion for response ch2 temp error");
                            statusmsgTxtBox.Text = "Error converting ch2 temperature = " + tempstr;
                            statusmsgTxtBox.Update();
                            statusmsgTxtBox.Text = "Exception error - ch2 temp";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }

                        // next up is #ch3temp$
                        tempstr = "";
                        mypos++;        // get past #
                        for (; Convert.ToChar(recbuf[mypos]) != '$'; mypos++)
                        {
                            tempstr = tempstr + recbuf[mypos];
                        }
                        // this is ch3 temp
                        try
                        {
                            LogMessageToFile("CommandString: try to convert response for ch3 temp=" + tempstr);
                            ch3temp = Double.Parse(tempstr, newCulture);
                            ch3temp = Math.Round(ch3temp, 2);
                            LogMessageToFile("CommandString: convert OK ch3 temp= " + Convert.ToString(ch3temp, newCulture));
                            // display in textbox
                            // controller always returns centigrade values
                            LogMessageToFile("CommandString: ch3 temp=" + Convert.ToString(ch3temp, newCulture));
                            if (celsiusToolStripMenuItem.Checked)
                            {
                                ch3tempTxtBox.Text = Convert.ToString(ch3temp, newCulture);
                                ch3tempTxtBox.Update();
                                statusmsgTxtBox.Text = "Ch3=" + Convert.ToString(ch3temp, newCulture);
                                statusmsgTxtBox.Update();
                                ch3label.Text = "C";
                                ch3label.Update();
                            }
                            else
                            {
                                double tempC, tempF;
                                try
                                {
                                    tempC = ch3temp;
                                    tempF = (tempC * 1.8) + 32;
                                    tempF = Math.Round(tempF, 2);
                                    ch3tempTxtBox.Text = Convert.ToString(tempF, newCulture);
                                    ch3tempTxtBox.Update();
                                    ch3label.Text = "F";
                                    ch3label.Update();
                                }
                                catch (FormatException)
                                {
                                    MessageBox.Show("Format Exception error converting value", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    LogMessageToFile("CommandString: Format Exception error");
                                    if (soundenabled)               // beep to indicate response
                                        SystemSounds.Exclamation.Play();
                                    statusmsgTxtBox.Update();
                                    statusmsgTxtBox.Text = "Format Exception error - ch3 temp";
                                    statusmsgTxtBox.Update();
                                    commandinprogress = false;
                                    return recbuf;
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("Exception error converting value", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    LogMessageToFile("CommandString: Exception error");
                                    if (soundenabled)               // beep to indicate response
                                        SystemSounds.Exclamation.Play();
                                    statusmsgTxtBox.Update();
                                    statusmsgTxtBox.Text = "Exception error - ch3 temp";
                                    statusmsgTxtBox.Update();
                                    commandinprogress = false;
                                    return recbuf;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("CommandString: conversion for response ch3 temp error");
                            statusmsgTxtBox.Text = "Error converting ch3 temperature = " + tempstr;
                            statusmsgTxtBox.Update();
                            statusmsgTxtBox.Text = "Exception error - ch3 temp";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }
                    }

                    else if (cmd == "W#")
                    {
                        // get ch1/ch2/ch3 pwr
                        // W#	Wch1pwr#ch2pwr#ch3pwr$	Returns the power settings for ch1/ch2/ch3
                        LogMessageToFile("CommandString: cmd = " + cmd);
                        LogMessageToFile("CommandString: get ch1/2/3 power");
                        tempstr = "";
                        // try to read response from serial port
                        try
                        {
                            recbuf = serialPort1.ReadTo("$");
                            // add back the end of command terminator
                            recbuf = recbuf + "$";
                        }
                        // handle exception
                        catch (TimeoutException)
                        {
                            LogMessageToFile("CommandString: W# Timeout exception raised");
                            statusmsgTxtBox.Text = "Timeout exception raised W#";
                            statusmsgTxtBox.Update();
                            return recbuf;
                        }
                        LogMessageToFile("CommandString response recd: W#: " + recbuf);
                        tempstr = "";
                        for (mypos = 1; Convert.ToChar(recbuf[mypos]) != '#'; mypos++)
                        {
                            tempstr = tempstr + recbuf[mypos];
                        }
                        // this is ch1 power
                        try
                        {
                            LogMessageToFile("CommandString: try to convert response for ch1pwr=" + tempstr);
                            ch1pwr = Convert.ToInt32(tempstr);
                            LogMessageToFile("CommandString: convert OK ch1pwr " + ch1pwr.ToString());
                            // display in textbox
                            ch1pwrTxtBox.Text = Convert.ToString(ch1pwr);
                            statusmsgTxtBox.Text = "CH1 Pwr=" + Convert.ToString(ch1pwr);
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("CommandString: conversion for response ch1pwr error");
                            statusmsgTxtBox.Text = "Error converting ch1pwr =" + tempstr;
                            statusmsgTxtBox.Update();
                            statusmsgTxtBox.Text = "Exception error ch1 power";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }

                        // next up is #ch2pwr#ch3pwr$
                        tempstr = "";
                        mypos++;        // get past #
                        for (; Convert.ToChar(recbuf[mypos]) != '#'; mypos++)
                        {
                            tempstr = tempstr + recbuf[mypos];
                        }

                        // this is ch2pwr
                        try
                        {
                            LogMessageToFile("CommandString: try to convert response for ch2pwr=" + tempstr);
                            ch2pwr = Convert.ToInt32(tempstr);
                            LogMessageToFile("CommandString: convert OK ch2pwr " + ch2pwr.ToString());
                            // display in textbox
                            ch2pwrTxtBox.Text = Convert.ToString(ch2pwr);
                            statusmsgTxtBox.Text = "CH2 Pwr=" + Convert.ToString(ch2pwr);
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("CommandString: conversion for response ch2pwr error");
                            statusmsgTxtBox.Text = "Error converting ch2pwr =" + tempstr;
                            statusmsgTxtBox.Update();
                            statusmsgTxtBox.Text = "Exception error ch2 power";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }

                        // next up is #ch3pwr$
                        tempstr = "";
                        mypos++;        // get past #
                        for (; Convert.ToChar(recbuf[mypos]) != '$'; mypos++)
                        {
                            tempstr = tempstr + recbuf[mypos];
                        }

                        // this is ch3pwr
                        try
                        {
                            LogMessageToFile("CommandString: try to convert response for ch3pwr=" + tempstr);
                            ch3pwr = Convert.ToInt32(tempstr);
                            LogMessageToFile("CommandString: convert OK ch3pwr " + ch3pwr.ToString());
                            // display in textbox
                            ch3pwrTxtBox.Text = Convert.ToString(ch3pwr);
                            statusmsgTxtBox.Text = "CH3 Pwr=" + Convert.ToString(ch3pwr);
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("CommandString: conversion for response ch3pwr error");
                            statusmsgTxtBox.Text = "Error converting ch3pwr temperature=" + tempstr;
                            statusmsgTxtBox.Update();
                            statusmsgTxtBox.Text = "Exception error ch3 power";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }
                    }

                    // get atbias
                    // B#	Bvalue$		Returns the ATBias value
                    else if (cmd == "B#")
                    {
                        LogMessageToFile("CommandString: cmd = " + cmd);
                        LogMessageToFile("CommandString: get ATBias");
                        tempstr = "";
                        // try to read response from serial port
                        try
                        {
                            recbuf = serialPort1.ReadTo("$");
                            // add back the end of command terminator
                            recbuf = recbuf + "$";
                        }
                        // handle exception
                        catch (TimeoutException)
                        {
                            LogMessageToFile("CommandString: B# Timeout exception raised");
                            statusmsgTxtBox.Text = "Timeout exception raised B#";
                            statusmsgTxtBox.Update();
                            return recbuf;
                        }
                        LogMessageToFile("CommandString response recd: B#: " + recbuf);

                        // now get atbias
                        tempstr = "";
                        for (mypos = 1; Convert.ToChar(recbuf[mypos]) != '$'; mypos++)
                        {
                            tempstr = tempstr + recbuf[mypos];
                        }
                        // this is the ATBIAS
                        try
                        {
                            LogMessageToFile("CommandString: try to convert response for ATBias=" + tempstr);
                            ATBiasVal = Convert.ToInt32(tempstr);
                            LogMessageToFile("CommandString: ATBias=" + ATBiasVal.ToString());
                            statusmsgTxtBox.Text = "ATBias=" + Convert.ToString(ATBiasVal);
                            statusmsgTxtBox.Update();
                            atbiasoffsetGroupBox.Enabled = false;
                            ATBiasMinus4.Enabled = false;
                            ATBiasMinus3.Enabled = false;
                            ATBiasMinus2.Enabled = false;
                            ATBiasMinus1.Enabled = false;
                            ATBiasZero.Enabled = false;
                            ATBiasPlus1.Enabled = false;
                            ATBiasPlus2.Enabled = false;
                            ATBiasPlus3.Enabled = false;
                            switch (ATBiasVal)
                            {
                                case -4:
                                    ATBiasMinus4.Checked = true;
                                    ATBiasMinus3.Checked = false;
                                    ATBiasMinus2.Checked = false;
                                    ATBiasMinus1.Checked = false;
                                    ATBiasZero.Checked = false;
                                    ATBiasPlus1.Checked = false;
                                    ATBiasPlus2.Checked = false;
                                    ATBiasPlus3.Checked = false;
                                    break;
                                case -3:
                                    ATBiasMinus4.Checked = false;
                                    ATBiasMinus3.Checked = true;
                                    ATBiasMinus2.Checked = false;
                                    ATBiasMinus1.Checked = false;
                                    ATBiasZero.Checked = false;
                                    ATBiasPlus1.Checked = false;
                                    ATBiasPlus2.Checked = false;
                                    ATBiasPlus3.Checked = false;
                                    break;
                                case -2:
                                    ATBiasMinus4.Checked = false;
                                    ATBiasMinus3.Checked = false;
                                    ATBiasMinus2.Checked = true;
                                    ATBiasMinus1.Checked = false;
                                    ATBiasZero.Checked = false;
                                    ATBiasPlus1.Checked = false;
                                    ATBiasPlus2.Checked = false;
                                    ATBiasPlus3.Checked = false;
                                    break;
                                case -1:
                                    ATBiasMinus4.Checked = false;
                                    ATBiasMinus3.Checked = false;
                                    ATBiasMinus2.Checked = false;
                                    ATBiasMinus1.Checked = true;
                                    ATBiasZero.Checked = false;
                                    ATBiasPlus1.Checked = false;
                                    ATBiasPlus2.Checked = false;
                                    ATBiasPlus3.Checked = false;
                                    break;
                                case 0:
                                    ATBiasMinus4.Checked = false;
                                    ATBiasMinus3.Checked = false;
                                    ATBiasMinus2.Checked = false;
                                    ATBiasMinus1.Checked = false;
                                    ATBiasZero.Checked = true;
                                    ATBiasPlus1.Checked = false;
                                    ATBiasPlus2.Checked = false;
                                    ATBiasPlus3.Checked = false;
                                    break;
                                case 1:
                                    ATBiasMinus4.Checked = false;
                                    ATBiasMinus3.Checked = false;
                                    ATBiasMinus2.Checked = false;
                                    ATBiasMinus1.Checked = false;
                                    ATBiasZero.Checked = false;
                                    ATBiasPlus1.Checked = true;
                                    ATBiasPlus2.Checked = false;
                                    ATBiasPlus3.Checked = false;
                                    break;
                                case 2:
                                    ATBiasMinus4.Checked = false;
                                    ATBiasMinus3.Checked = false;
                                    ATBiasMinus2.Checked = false;
                                    ATBiasMinus1.Checked = false;
                                    ATBiasZero.Checked = false;
                                    ATBiasPlus1.Checked = false;
                                    ATBiasPlus2.Checked = true;
                                    ATBiasPlus3.Checked = false;
                                    break;
                                case 3:
                                    ATBiasMinus4.Checked = false;
                                    ATBiasMinus3.Checked = false;
                                    ATBiasMinus2.Checked = false;
                                    ATBiasMinus1.Checked = false;
                                    ATBiasZero.Checked = false;
                                    ATBiasPlus1.Checked = false;
                                    ATBiasPlus2.Checked = false;
                                    ATBiasPlus3.Checked = true;
                                    break;
                            }
                            atbiasoffsetGroupBox.Enabled = true;
                            ATBiasMinus4.Enabled = true;
                            ATBiasMinus3.Enabled = true;
                            ATBiasMinus2.Enabled = true;
                            ATBiasMinus1.Enabled = true;
                            ATBiasZero.Enabled = true;
                            ATBiasPlus1.Enabled = true;
                            ATBiasPlus2.Enabled = true;
                            ATBiasPlus3.Enabled = true;
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("CommandString: conversion for response ATBias error");
                            statusmsgTxtBox.Text = "Error converting ATBias temperature=" + tempstr;
                            statusmsgTxtBox.Update();
                            statusmsgTxtBox.Text = "Exception error - atbias";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }
                    }

                    // get tracking mode
                    // T#	Ttrackingmode$		Return tracking mode, 1=Ambient, 2=Dewpoint, 3=Midpoint)
                    else if (cmd == "T#")
                    {
                        LogMessageToFile("CommandString: cmd = " + cmd);
                        LogMessageToFile("CommandString: get tracking mode");
                        tempstr = "";
                        // try to read response from serial port
                        try
                        {
                            recbuf = serialPort1.ReadTo("$");
                            // add back the end of command terminator
                            recbuf = recbuf + "$";
                        }
                        // handle exception
                        catch (TimeoutException)
                        {
                            LogMessageToFile("CommandString: T# Timeout exception raised");
                            statusmsgTxtBox.Text = "Timeout exception raised T#";
                            statusmsgTxtBox.Update();
                            return recbuf;
                        }
                        LogMessageToFile("CommandString response recd: T#: " + recbuf);

                        tempstr = "";
                        for (mypos = 1; Convert.ToChar(recbuf[mypos]) != '$'; mypos++)
                        {
                            tempstr = tempstr + recbuf[mypos];
                        }
                        // this is the tracking mode
                        try
                        {
                            LogMessageToFile("CommandString: try to convert response for tracking mode =" + tempstr);
                            trackingmode = Convert.ToInt32(tempstr);
                            // display in textbox
                            trackingmodeGroupBox.Enabled = false;
                            TrackModeAmbient.Enabled = false;
                            TrackModeDewPoint.Enabled = false;
                            TrackModeMidPoint.Enabled = false;
                            switch (trackingmode)
                            {
                                case 1:
                                    {
                                        TrackModeAmbient.Checked = true;
                                        TrackModeDewPoint.Checked = false;
                                        TrackModeMidPoint.Checked = false;
                                        LogMessageToFile("CommandString: tracking Mode=Ambient");
                                    }
                                    break;
                                case 2:
                                    {
                                        TrackModeAmbient.Checked = false;
                                        TrackModeDewPoint.Checked = true;
                                        TrackModeMidPoint.Checked = false;
                                        LogMessageToFile("CommandString: tracking Mode=DewPoint");
                                    }
                                    break;
                                case 3:
                                    {
                                        TrackModeAmbient.Checked = false;
                                        TrackModeDewPoint.Checked = false;
                                        TrackModeMidPoint.Checked = true;
                                        LogMessageToFile("CommandString: tracking Mode=MidPoint");
                                    }
                                    break;
                            }
                            TrackModeAmbient.Enabled = true;
                            TrackModeDewPoint.Enabled = true;
                            TrackModeMidPoint.Enabled = true;
                            trackingmodeGroupBox.Enabled = true;
                            statusmsgTxtBox.Update();
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("CommandString: conversion for response Tracking Mode");
                            statusmsgTxtBox.Text = "Error converting Tracking Mode=" + tempstr;
                            statusmsgTxtBox.Update();
                            statusmsgTxtBox.Text = "Exception error - tracking mode";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }
                    }

                    // get fanspeed
                    // F#	Ffanspeed$		Returns fan speed (0, 50, 75 and 100)
                    else if (cmd == "F#")
                    {
                        LogMessageToFile("CommandString: cmd = " + cmd);
                        LogMessageToFile("CommandString: get fan speed");
                        tempstr = "";
                        // try to read response from serial port
                        try
                        {
                            recbuf = serialPort1.ReadTo("$");
                            // add back the end of command terminator
                            recbuf = recbuf + "$";
                        }
                        // handle exception
                        catch (TimeoutException)
                        {
                            LogMessageToFile("CommandString: F# Timeout exception raised");
                            statusmsgTxtBox.Text = "Timeout exception raised F#";
                            statusmsgTxtBox.Update();
                            return recbuf;
                        }
                        LogMessageToFile("CommandString response recd: F#: " + recbuf);

                        tempstr = "";
                        // now get fanspeed
                        for (mypos = 1; Convert.ToChar(recbuf[mypos]) != '$'; mypos++)
                        {
                            tempstr = tempstr + recbuf[mypos];
                        }
                        // this is the fan speed
                        try
                        {
                            LogMessageToFile("CommandString: try to convert response for Fan Speed=" + tempstr);
                            fanspeed = Convert.ToInt32(tempstr);
                            LogMessageToFile("CommandString: convert OK fanspeed = " + fanspeed.ToString());
                            statusmsgTxtBox.Text = "Fanspeed=" + Convert.ToString(fanspeed);
                            statusmsgTxtBox.Update();
                            fanspeedGroupBox.Enabled = false;
                            FanSpeedZero.Enabled = false;
                            FanSpeed50.Enabled = false;
                            FanSpeed75.Enabled = false;
                            FanSpeed100.Enabled = false;
                            switch (fanspeed)
                            {
                                case 0:
                                    {
                                        FanSpeedZero.Checked = true;
                                        FanSpeed50.Checked = false;
                                        FanSpeed75.Checked = false;
                                        FanSpeed100.Checked = false;
                                        LogMessageToFile("CommandString: Fan Speed=0");
                                    }
                                    break;
                                case 50:
                                    {
                                        FanSpeedZero.Checked = false;
                                        FanSpeed50.Checked = true;
                                        FanSpeed75.Checked = false;
                                        FanSpeed100.Checked = false;
                                        LogMessageToFile("CommandString: Fan Speed=50%");
                                    }
                                    break;
                                case 75:
                                    {
                                        FanSpeedZero.Checked = false;
                                        FanSpeed50.Checked = false;
                                        FanSpeed75.Checked = true;
                                        FanSpeed100.Checked = false;
                                        LogMessageToFile("CommandString: Fan Speed=75%");
                                    }
                                    break;
                                case 100:
                                    {
                                        FanSpeedZero.Checked = false;
                                        FanSpeed50.Checked = false;
                                        FanSpeed75.Checked = false;
                                        FanSpeed100.Checked = true;
                                        LogMessageToFile("CommandString: Fan Speed=100%");
                                    }
                                    break;
                            }
                            FanSpeedZero.Enabled = true;
                            FanSpeed50.Enabled = true;
                            FanSpeed75.Enabled = true;
                            FanSpeed100.Enabled = true;
                            fanspeedGroupBox.Enabled = true;
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("CommandString: try to convert response for Fan Speed error");
                            statusmsgTxtBox.Text = "Error converting fan speed=" + tempstr;
                            statusmsgTxtBox.Update();
                            statusmsgTxtBox.Text = "Exception error - fan speed";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }
                    }

                    // get ch1/ch2/ch3 offsets
                    // ?# 	?ch1offset#ch2offset#ch3offset$	Get the ch1offset and ch2offset and ch3offset values (returns float values as strings)
                    else if (cmd == "?#")
                    {
                        LogMessageToFile("CommandString: cmd = " + cmd);
                        LogMessageToFile("CommandString: get ch1/2/3 temp offset");
                        tempstr = "";
                        // try to read response from serial port
                        try
                        {
                            recbuf = serialPort1.ReadTo("$");
                            // add back the end of command terminator
                            recbuf = recbuf + "$";
                        }
                        // handle exception
                        catch (TimeoutException)
                        {
                            LogMessageToFile("CommandString: ?# Timeout exception raised");
                            statusmsgTxtBox.Text = "Timeout exception raised E#";
                            statusmsgTxtBox.Update();
                            return recbuf;
                        }
                        LogMessageToFile("CommandString response recd: ?#: " + recbuf);

                        //code change so all the offsets are printed in one line in the stats=usmsgtextboc
                        statusmsg = "";

                        // no need to strip off terminator
                        // this is ch1/ch2/ch3 temp offset values OV#ch1offset#ch2offset#ch3offset$
                        if (recbuf == "")
                        {
                            statusmsgTxtBox.Text = "Null response -  ch1/2/3 offsets";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }
                        else
                        {
                            // recbuff is ?#ch1offset#ch2offset#ch3offset$, extract just the value
                            tempstr = "";
                            for (mypos = 1; Convert.ToChar(recbuf[mypos]) != '#'; mypos++)
                            {
                                tempstr = tempstr + recbuf[mypos];
                            }
                            // this is ch1 temp offset val
                            try
                            {
                                LogMessageToFile("CommandString: try to convert response for ch1 offset=" + tempstr);
                                ch1tempoffsetval = (float)Double.Parse(tempstr, newCulture);
                                LogMessageToFile("CommandString: convert OK ch1 offset= " + Convert.ToString(ch1tempoffsetval, newCulture));
                                // display in textbox
                                // controller always returns centigrade values
                                LogMessageToFile("CommandString: ch1 offset=" + Convert.ToString(ch1tempoffsetval, newCulture));
                                ch1tempoffTxtBox.Text = Convert.ToString(ch1tempoffsetval, newCulture);
                                ch1tempoffTxtBox.Update();

                                statusmsg = "Ch1 Offset=" + Convert.ToString(ch1tempoffsetval, newCulture) + ", ";
                            }
                            catch (Exception)
                            {
                                LogMessageToFile("CommandString: conversion for response ch1 offset error");
                                statusmsgTxtBox.Text = "Exception ch1 offset";
                                statusmsgTxtBox.Update();
                                commandinprogress = false;
                                return recbuf;
                            }
                            // recbuff is now #ch2tempoffset#ch3tempoffset$, extract just the value
                            tempstr = "";
                            mypos++;        // get past #
                            for (; Convert.ToChar(recbuf[mypos]) != '#'; mypos++)
                            {
                                tempstr = tempstr + recbuf[mypos];
                            }
                            // this is ch2 temp offset
                            try
                            {
                                LogMessageToFile("CommandString: try to convert response for ch2 offset=" + tempstr);
                                ch2tempoffsetval = (float)Double.Parse(tempstr, newCulture);
                                LogMessageToFile("CommandString: convert OK ch2 offset= " + Convert.ToString(ch2tempoffsetval, newCulture));
                                // display in textbox
                                // controller always returns centigrade values
                                LogMessageToFile("CommandString: ch2 offset=" + Convert.ToString(ch2tempoffsetval, newCulture));
                                ch2tempoffTxtBox.Text = Convert.ToString(ch2tempoffsetval, newCulture);
                                ch2tempoffTxtBox.Update();
                                statusmsg = statusmsg + "Ch2 Offset=" + Convert.ToString(ch2tempoffsetval, newCulture) + ", ";
                            }
                            catch (Exception)
                            {
                                LogMessageToFile("CommandString: conversion for response ch2 offset error");
                                statusmsgTxtBox.Text = "Exception ch2 offset";
                                statusmsgTxtBox.Update();
                                commandinprogress = false;
                                return recbuf;
                            }
                            // recbuff is now #ch3tempoffset$, extract just the value
                            tempstr = "";
                            mypos++;        // get past #
                            for (; Convert.ToChar(recbuf[mypos]) != '$'; mypos++)
                            {
                                tempstr = tempstr + recbuf[mypos];
                            }
                            // this is ch3 temp offset
                            try
                            {
                                LogMessageToFile("CommandString: try to convert response for ch3 offset=" + tempstr);
                                ch3tempoffsetval = (float)Double.Parse(tempstr, newCulture);
                                LogMessageToFile("CommandString: convert OK ch3 offset= " + Convert.ToString(ch3tempoffsetval, newCulture));
                                // display in textbox
                                // controller always returns centigrade values
                                LogMessageToFile("CommandString: ch3 offset=" + Convert.ToString(ch3tempoffsetval, newCulture));
                                ch3tempoffTxtBox.Text = Convert.ToString(ch3tempoffsetval, newCulture);
                                ch3tempoffTxtBox.Update();
                                statusmsg = statusmsg + "Ch3 Offset=" + Convert.ToString(ch3tempoffsetval, newCulture);
                                statusmsgTxtBox.Text = statusmsg;
                                statusmsgTxtBox.Update();
                            }
                            catch (Exception)
                            {
                                LogMessageToFile("CommandString: conversion for response ch3 offset error");
                                statusmsgTxtBox.Text = "Exception ch3 offset";
                                statusmsgTxtBox.Update();
                                commandinprogress = false;
                                return recbuf;
                            }
                        }
                    }

                    // get ch3mode
                    // E#	Emode $		Returns which dewstrap channel the 3rd Dewstrap is shadowing (0-none, 1=channel1, 2=channel2, 3=manual, 4=temp probe 3)
                    else if (cmd == "E#")
                    {
                        LogMessageToFile("CommandString: cmd = " + cmd);
                        LogMessageToFile("CommandString: get ch3 mode");
                        tempstr = "";
                        // try to read response from serial port
                        try
                        {
                            recbuf = serialPort1.ReadTo("$");
                            // add back the end of command terminator
                            recbuf = recbuf + "$";
                        }
                        // handle exception
                        catch (TimeoutException)
                        {
                            LogMessageToFile("CommandString: E# Timeout exception raised");
                            statusmsgTxtBox.Text = "Timeout exception raised E#";
                            statusmsgTxtBox.Update();
                            return recbuf;
                        }
                        LogMessageToFile("CommandString response recd: E#: " + recbuf);

                        // no need to strip off terminator
                        // this is the dew channel being shadowed
                        if (recbuf == "")
                        {
                            statusmsgTxtBox.Text = "Null response: :E# GetShadowDewChannel";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }
                        else
                        {
                            // recbuff is E#mode$, extract just the value
                            tempstr = Convert.ToChar(recbuf[1]) + "";
                            statusmsg = "";
                            try
                            {
                                LogMessageToFile("CommandString: try to convert response for shadowchannel3=" + recbuf[2]);
                                int shadowchannel = Convert.ToInt32(tempstr);
                                // display in textbox
                                LogMessageToFile("CommandString: convert OK ShadowChannel = " + shadowchannel.ToString());

                                // update settings menu bar for shadow dew channel3
                                switch (shadowchannel)
                                {
                                    case 0: oFFToolStripMenuItem.Checked = true;
                                        channel1ToolStripMenuItem.Checked = false;
                                        channel2ToolStripMenuItem.Checked = false;
                                        manualSettingToolStripMenuItem.Checked = false;
                                        useTempProbe3ToolStripMenuItem.Checked = false;
                                        statusmsgTxtBox.Text = "Ch3 Mode = OFF";
                                        statusmsgTxtBox.Update();
                                        break;
                                    case 1: oFFToolStripMenuItem.Checked = false;
                                        channel1ToolStripMenuItem.Checked = true;
                                        channel2ToolStripMenuItem.Checked = false;
                                        manualSettingToolStripMenuItem.Checked = false;
                                        useTempProbe3ToolStripMenuItem.Checked = false;
                                        statusmsgTxtBox.Text = "Ch3 Mode = Ch1";
                                        statusmsgTxtBox.Update();
                                        break;
                                    case 2:
                                        oFFToolStripMenuItem.Checked = false;
                                        channel1ToolStripMenuItem.Checked = false;
                                        channel2ToolStripMenuItem.Checked = true;
                                        manualSettingToolStripMenuItem.Checked = false;
                                        useTempProbe3ToolStripMenuItem.Checked = false;
                                        statusmsgTxtBox.Text = "Ch3 Mode = Ch1";
                                        statusmsgTxtBox.Update();
                                        break;
                                    case 3:  // manual setting
                                        oFFToolStripMenuItem.Checked = false;
                                        channel1ToolStripMenuItem.Checked = false;
                                        channel2ToolStripMenuItem.Checked = false;
                                        manualSettingToolStripMenuItem.Checked = true;
                                        useTempProbe3ToolStripMenuItem.Checked = false;
                                        statusmsgTxtBox.Text = "Ch3 Mode = Manual";
                                        statusmsgTxtBox.Update();
                                        break;
                                    case 4:  // use temp probe3
                                        oFFToolStripMenuItem.Checked = false;
                                        channel1ToolStripMenuItem.Checked = false;
                                        channel2ToolStripMenuItem.Checked = false;
                                        manualSettingToolStripMenuItem.Checked = false;
                                        useTempProbe3ToolStripMenuItem.Checked = true;
                                        statusmsgTxtBox.Text = "Ch3 Mode = Use ch3 Temp Probe";
                                        statusmsgTxtBox.Update();
                                        break;
                                }  // end of switch
                                ch3mode = shadowchannel;
                            } // end of try
                            catch (Exception)
                            {
                                LogMessageToFile("CommandString: conversion for response ShadowChannel3");
                                statusmsgTxtBox.Text = "Exception error - ShadowChannel3";
                                statusmsgTxtBox.Update();
                                commandinprogress = false;
                                return recbuf;
                            }
                        } // end of else if recbuf == ""
                    }

                    // get firmware version
                    else if (cmd == "v#")
                    {
                        LogMessageToFile("CommandString: cmd = " + cmd);
                        LogMessageToFile("CommandString: get firmware version");
                        // try to read response from serial port
                        try
                        {
                            recbuf = serialPort1.ReadTo("$");
                            // add back the end of command terminator
                            recbuf = recbuf + "$";
                        }
                        // handle exception
                        catch (TimeoutException)
                        {
                            LogMessageToFile("CommandString: v# Timeout exception raised");
                            statusmsgTxtBox.Text = "Timeout Exception error - Version";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }
                        LogMessageToFile("CommandString response recd: v#: " + recbuf);
                        // no need to strip off terminator
                        // this is version
                        if (recbuf == "")
                        {
                            LogMessageToFile("CommandString response: recbuf is null");
                            statusmsgTxtBox.Text = "Null response - Get Version";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }
                        else
                        {
                            // recbuff is vversion$, extract just the version
                            tempstr = "";
                            for (mypos = 1; Convert.ToChar(recbuf[mypos]) != '$'; mypos++)
                            {
                                tempstr = tempstr + recbuf[mypos];
                            }
                            ControllerVersionTxtBox.Text = "myDewControllerPro3 v" + tempstr;
                            ArduinoFirmwareRev = tempstr;
                            MajorRevNumber = ArduinoFirmwareRev[0];
                            String Rev = ArduinoFirmwareRev.Substring(0, ArduinoFirmwareRev.Length);
                            // should be xxx
                            try
                            {
                                ControllerRev = Double.Parse(Rev, newCulture);
                            }
                            catch (FormatException)
                            {
                                ControllerRev = 0;
                                LogMessageToFile("CommandString response: Error parsing ControllerRev");
                                statusmsgTxtBox.Text = "Error parsing ControllerRev";
                                statusmsgTxtBox.Update();
                            }
                        }
                    }

                    else if (cmd == "g#") // get number of temperature probes
                    {
                        LogMessageToFile("CommandString: cmd = " + cmd);
                        LogMessageToFile("CommandString: get number of temp probes");
                        // try to read response from serial port
                        try
                        {
                            recbuf = serialPort1.ReadTo("$");
                            // add back the end of command terminator
                            recbuf = recbuf + "$";
                        }
                        // handle exception
                        catch (TimeoutException)
                        {
                            commandinprogress = false;
                            LogMessageToFile("CommandString: P# Timeout exception raised");
                            statusmsgTxtBox.Text = "Timeout Exception error - Probes";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }
                        LogMessageToFile("CommandString response recd: g#: " + recbuf);
                        // no need to strip off terminator
                        // this is numer of temperature probes P#value$
                        if (recbuf == "")
                        {
                            statusmsgTxtBox.Text = "Null response: Probes";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }
                        else
                        {
                            // recbuff is Pvalue$, extract just the value
                            tempstr = "";
                            for (mypos = 1; Convert.ToChar(recbuf[mypos]) != '$'; mypos++)
                            {
                                tempstr = tempstr + recbuf[mypos];
                            }
                            // this is the number of probes
                            try
                            {
                                LogMessageToFile("CommandString: try to convert response for probes=" + tempstr);
                                numberoftempprobes = Convert.ToInt32(tempstr);
                                // display in textbox
                                // controller always returns centigrade values
                                LogMessageToFile("CommandString: convert OK Probes = " + numberoftempprobes.ToString());
                                statusmsgTxtBox.Text = "Probes=" + numberoftempprobes.ToString();
                                statusmsgTxtBox.Update();
                            }
                            catch (Exception)
                            {
                                LogMessageToFile("CommandString: conversion for response Probes error");
                                statusmsgTxtBox.Text = "Exception error - Probes";
                                statusmsgTxtBox.Update();
                                commandinprogress = false;
                                return recbuf;
                            }
                        } // end of else
                        commandinprogress = false;
                    }

                    else if (cmd == "H#") // get lcddisplaytime
                    {
                        int tempnum = 0;

                        LogMessageToFile("CommandString: cmd = " + cmd);
                        LogMessageToFile("CommandString: get lcd display time per page");
                        // try to read response from serial port
                        try
                        {
                            recbuf = serialPort1.ReadTo("$");
                            // add back the end of command terminator
                            recbuf = recbuf + "$";

                            LogMessageToFile("CommandString response recd: H#: " + recbuf);
                            // no need to strip off terminator
                            // this is ambient M#value$
                            if (recbuf == "")
                            {
                                LogMessageToFile("CommandString response: recbuf is null");
                                statusmsgTxtBox.Text = "Null response - lcddisplaytime";
                                statusmsgTxtBox.Update();
                                commandinprogress = false;
                                return recbuf;
                            }
                            else
                            {
                                // recbuff is Hfloat_num$, extract just the float_num
                                tempstr = "";
                                for (mypos = 1; Convert.ToChar(recbuf[mypos]) != '$'; mypos++)
                                {
                                    tempstr = tempstr + recbuf[mypos];
                                }
                                // this is lcddisplaytime
                                try
                                {
                                    LogMessageToFile("CommandString: try to convert response for lcddisplaytime=" + tempstr);
                                    tempnum = Convert.ToInt32(tempstr);
                                    LogMessageToFile("CommandString: convert OK lcddisplaytime = " + tempnum.ToString());

                                    // now set the menu according to lcddisplaytime
                                    switch (tempnum)
                                    {
                                        case 2500:
                                            secondsToolStripMenuItem.Checked = true;
                                            secondsToolStripMenuItem1.Checked = false;
                                            secondsToolStripMenuItem2.Checked = false;
                                            secondsToolStripMenuItem3.Checked = false;
                                            secondsToolStripMenuItem4.Checked = false;
                                            secondsToolStripMenuItem5.Checked = false;
                                            break;
                                        case 3000:
                                            secondsToolStripMenuItem.Checked = false;
                                            secondsToolStripMenuItem1.Checked = true;
                                            secondsToolStripMenuItem2.Checked = false;
                                            secondsToolStripMenuItem3.Checked = false;
                                            secondsToolStripMenuItem4.Checked = false;
                                            secondsToolStripMenuItem5.Checked = false;
                                            break;
                                        case 3500:
                                            secondsToolStripMenuItem.Checked = false;
                                            secondsToolStripMenuItem1.Checked = false;
                                            secondsToolStripMenuItem2.Checked = true;
                                            secondsToolStripMenuItem3.Checked = false;
                                            secondsToolStripMenuItem4.Checked = false;
                                            secondsToolStripMenuItem5.Checked = false;
                                            break;
                                        case 4000:
                                            secondsToolStripMenuItem.Checked = false;
                                            secondsToolStripMenuItem1.Checked = false;
                                            secondsToolStripMenuItem2.Checked = false;
                                            secondsToolStripMenuItem3.Checked = true;
                                            secondsToolStripMenuItem4.Checked = false;
                                            secondsToolStripMenuItem5.Checked = false;
                                            break;
                                        case 4500:
                                            secondsToolStripMenuItem.Checked = false;
                                            secondsToolStripMenuItem1.Checked = false;
                                            secondsToolStripMenuItem2.Checked = false;
                                            secondsToolStripMenuItem3.Checked = false;
                                            secondsToolStripMenuItem4.Checked = true;
                                            secondsToolStripMenuItem5.Checked = false;
                                            break;
                                        case 5000:
                                            secondsToolStripMenuItem.Checked = false;
                                            secondsToolStripMenuItem1.Checked = false;
                                            secondsToolStripMenuItem2.Checked = false;
                                            secondsToolStripMenuItem3.Checked = false;
                                            secondsToolStripMenuItem4.Checked = false;
                                            secondsToolStripMenuItem5.Checked = true;
                                            break;
                                    }
                                }
                                catch (Exception)
                                {
                                    LogMessageToFile("CommandString: conversion for response lcddisplaytime error");
                                    statusmsgTxtBox.Text = "Error converting kcddisplaytime = " + tempstr;
                                    statusmsgTxtBox.Update();
                                    statusmsgTxtBox.Text = "Exception error - lcddisplaytime";
                                    statusmsgTxtBox.Update();
                                    commandinprogress = false;
                                    return recbuf;
                                }
                            }
                            commandinprogress = false;
                        }
                        // handle exception
                        catch (TimeoutException)
                        {
                            LogMessageToFile("CommandString: H# Timeout exception raised");
                            statusmsgTxtBox.Text = "Timeout Exception error - lcddisplaytime";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }
                    }

                    else if (cmd == "K#")           // get PCB board temperature
                    {
                        LogMessageToFile("CommandString: cmd = " + cmd);
                        LogMessageToFile("CommandString: get board temp fan setting");
                        tempstr = "";
                        // try to read response from serial port
                        try
                        {
                            recbuf = serialPort1.ReadTo("$");
                            // add back the end of command terminator
                            recbuf = recbuf + "$";
                        }
                        // handle exception
                        catch (TimeoutException)
                        {
                            LogMessageToFile("CommandString: K# Timeout exception raised");
                            statusmsgTxtBox.Text = "Timeout exception raised K#";
                            statusmsgTxtBox.Update();
                            return recbuf;
                        }
                        LogMessageToFile("CommandString response recd: K#: " + recbuf);

                        tempstr = "";
                        // now get temperature of board
                        for (mypos = 1; Convert.ToChar(recbuf[mypos]) != '$'; mypos++)
                        {
                            tempstr = tempstr + recbuf[mypos];
                        }
                        // this is the current board temp
                        try
                        {
                            LogMessageToFile("CommandString: try to convert response for Board Temp = " + tempstr);
                            boardtemp = Convert.ToInt32(tempstr);
                            LogMessageToFile("CommandString: convert OK boardtemp = " + boardtemp.ToString());
                            statusmsgTxtBox.Text = "BoardTemp = " + Convert.ToString(boardtemp);
                            statusmsgTxtBox.Update();
                            pcbtempTxtBox.Text = boardtemp.ToString();
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("CommandString: try to convert response for Board Temp error");
                            statusmsgTxtBox.Text = "Error converting Board Temp = " + tempstr;
                            statusmsgTxtBox.Update();
                            statusmsgTxtBox.Text = "Exception error - Board Temperature";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }
                    }

                    else if (cmd == "J#")           // get fanmotor temperature ON setting
                    {
                        LogMessageToFile("CommandString: cmd = " + cmd);
                        LogMessageToFile("CommandString: get temp fan setting");
                        tempstr = "";
                        // try to read response from serial port
                        try
                        {
                            recbuf = serialPort1.ReadTo("$");
                            // add back the end of command terminator
                            recbuf = recbuf + "$";
                        }
                        // handle exception
                        catch (TimeoutException)
                        {
                            LogMessageToFile("CommandString: J# Timeout exception raised");
                            statusmsgTxtBox.Text = "Timeout exception raised J#";
                            statusmsgTxtBox.Update();
                            return recbuf;
                        }
                        LogMessageToFile("CommandString response recd: J#: " + recbuf);

                        tempstr = "";
                        // now get temperature of board
                        for (mypos = 1; Convert.ToChar(recbuf[mypos]) != '$'; mypos++)
                        {
                            tempstr = tempstr + recbuf[mypos];
                        }
                        // this is the current motor fan temp setting
                        try
                        {
                            LogMessageToFile("CommandString: try to convert response for Fan Motor Temp Setting =" + tempstr);
                            int fanmotortempsetting = Convert.ToInt32(tempstr);
                            LogMessageToFile("CommandString: convert OK fantempsetting = " + fanmotortempsetting.ToString());
                            statusmsgTxtBox.Text = "FanTempSetting = " + Convert.ToString(fanmotortempsetting);
                            statusmsgTxtBox.Update();
                            fantemponTxtBox.Text = fanmotortempsetting.ToString();
                            fantemponTxtBox.Update();
                            fantempsetting = fanmotortempsetting;
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("CommandString: try to convert response for Board Temp error");
                            statusmsgTxtBox.Text = "Error converting Board Temp = " + tempstr;
                            statusmsgTxtBox.Update();
                            statusmsgTxtBox.Text = "Exception error - Board Temperature";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }
                    }

                    else if (cmd == "L#")           // get fanmotor temperature OFF setting
                    {
                        LogMessageToFile("CommandString: cmd = " + cmd);
                        LogMessageToFile("CommandString: get temp fan setting");
                        tempstr = "";
                        // try to read response from serial port
                        try
                        {
                            recbuf = serialPort1.ReadTo("$");
                            // add back the end of command terminator
                            recbuf = recbuf + "$";
                        }
                        // handle exception
                        catch (TimeoutException)
                        {
                            LogMessageToFile("CommandString: L# Timeout exception raised");
                            statusmsgTxtBox.Text = "Timeout exception raised L#";
                            statusmsgTxtBox.Update();
                            return recbuf;
                        }
                        LogMessageToFile("CommandString response recd: L#: " + recbuf);

                        tempstr = "";
                        // now get temperature of board
                        for (mypos = 1; Convert.ToChar(recbuf[mypos]) != '$'; mypos++)
                        {
                            tempstr = tempstr + recbuf[mypos];
                        }
                        // this is the current motor fan temp off setting
                        try
                        {
                            LogMessageToFile("CommandString: try to convert response for Fan Motor Temp Setting =" + tempstr);
                            int fanmotortempsetting = Convert.ToInt32(tempstr);
                            LogMessageToFile("CommandString: convert OK fantempsetting = " + fanmotortempsetting.ToString());
                            statusmsgTxtBox.Text = "FanTempSetting OFF = " + Convert.ToString(fanmotortempsetting);
                            statusmsgTxtBox.Update();
                            fantempoffTxtBox.Text = fanmotortempsetting.ToString();
                            fantempoffTxtBox.Update();
                            fantempsetting = fanmotortempsetting;
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("CommandString: try to convert response for PCB Board Temp OFF error");
                            statusmsgTxtBox.Text = "Error converting Board Temp OFF = " + tempstr;
                            statusmsgTxtBox.Update();
                            statusmsgTxtBox.Text = "Exception error - Board Temperature OFF";
                            statusmsgTxtBox.Update();
                            commandinprogress = false;
                            return recbuf;
                        }
                    }

                    // other commands here

                }
                catch (Exception)
                {
                    LogMessageToFile("CommandString: Serial error");
                    LogMessageToFile("CommandString: Possible disconnect/close after command issued");
                    statusmsgTxtBox.Text = "Exception error sending " + cmd + " to serial port";
                    statusmsgTxtBox.Update();
                    return recbuf;
                }
            }
            else
            {
                LogMessageToFile("CommandString: Error: ComPort Closed");
                statusmsgTxtBox.Text = "Error: Serial Port is closed";
                statusmsgTxtBox.Update();
                return recbuf;
            }
            // return empty string or controller response 
            return recbuf;
        }

        // use this to send a command to the controller when NO response is required
        // alternative is to write direct to serial port but then must encase statement in try{} catch{}
        private void sendcmd(string sendcmd)
        {
            LogMessageToFile("sendcmd: Start");
            if (serialPort1.IsOpen)
            {
                LogMessageToFile("sendcmd: serialport is open");
                try
                {
                    LogMessageToFile("sendcmd: sending=" + sendcmd);
                    serialPort1.Write(sendcmd);
                    LogMessageToFile("sendcmd: sending=" + sendcmd + " was successful");
                }
                catch (TimeoutException)
                {
                    MessageBox.Show("Timeout Exception: " + sendcmd + " No response from controller\nWrong COM Port?", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMessageToFile("sendcmd: Timeout Exception sending command: " + sendcmd + "");
                    LogMessageToFile("sendcmd: Closing and disconnecting ComPort:" + ComPortName);
                    shutdownport("Timeout exception.");
                }
                catch (Exception)
                {
                    MessageBox.Show("Exception occurred sending " + sendcmd + " to controller. Possible wrong port or controller OFF or disconnected. \n\nPlease Disconnect.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMessageToFile("sendcmd: Exception sending command: " + sendcmd + "");
                    shutdownport("Error occurred sending " + sendcmd);
                }
            }
            else
            {
                LogMessageToFile("sendcmd: serial port is not open");
                statusmsgTxtBox.Text = "Error: COM Port not set";
                statusmsgTxtBox.Update();
            }
            LogMessageToFile("sendcmd: Finish");
        }

        private void shutdownport(String str)
        {
            LogMessageToFile("Serial Port has been disconnected.");
            comconnected = false;
            serialPort1.Close();
            LogMessageToFile("Disposing COMPort");
            serialPort1.Dispose();
            LogMessageToFile("Controls off");
            controlsoff();
            comconnected = false;
            Properties.Settings.Default.Save();
            RefreshTimer.Enabled = false;
            automateChkBox.Checked = false;
            automateChkBox.Enabled = false;
            LogDataChkBox.Checked = false;
            statusmsgTxtBox.Text = "Com Port closed";
            statusmsgTxtBox.Update();
            MessageBox.Show(str + "\nAn exception has occurred and the serial port has been closed.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void manualupdateBtn_Click(object sender, EventArgs e)
        {
            string ret = "";

            LogMessageToFile("ManualUpdate: Start");

            if (testmode)
            {
                LogMessageToFile("ManualUpdate: testmode = true");
                setvalues();
                return;
            }
            if (comconnected)
            {
                // this is a v300 controller
                // shadow channel 3, and power setting
                updatingvals = true;
                // query the controller and get all the values
                LogMessageToFile("ManualUpdate: Connected");
                statusmsgTxtBox.Text = "Requesting values from myDewControllerPro3";
                statusmsgTxtBox.Update();

                // in case there is a disconnection
                try
                {
                    serialPort1.DiscardInBuffer();
                }
                catch (InvalidOperationException)
                {
                    shutdownport("Error clearing serial port buffers");
                }

                // get controller values
                // ambient, humidity, dewpoint, ch1/2/3 temp, ch1/2/3 power, atbias, tracking mode, fanspeed
                LogMessageToFile("ManualUpdate - get ambient: send A#");
                statusmsgTxtBox.Text = "Getting ambient - please wait.";
                statusmsgTxtBox.Update();
                ret = CommandString("A#");

                LogMessageToFile("ManualUpdate - get humidity: send R#");
                statusmsgTxtBox.Text = "Getting humidity - please wait.";
                statusmsgTxtBox.Update();
                ret = CommandString("R#");

                LogMessageToFile("ManualUpdate - get dewpoint: send D#");
                statusmsgTxtBox.Text = "Getting dewpoint - please wait.";
                statusmsgTxtBox.Update();
                ret = CommandString("D#");

                LogMessageToFile("ManualUpdate - get ch1/2/3 temps: send C#");
                statusmsgTxtBox.Text = "Getting ch1/2/3 temps - please wait.";
                statusmsgTxtBox.Update();
                ret = CommandString("C#");

                LogMessageToFile("ManualUpdate - get ch1/2/3 power: send W#");
                statusmsgTxtBox.Text = "Getting ch1/2/3 power - please wait.";
                statusmsgTxtBox.Update();
                ret = CommandString("W#");

                LogMessageToFile("ManualUpdate - get atbias: send B#");
                statusmsgTxtBox.Text = "Getting atbias - please wait.";
                statusmsgTxtBox.Update();
                ret = CommandString("B#");

                LogMessageToFile("ManualUpdate - get tracking mode: send T#");
                statusmsgTxtBox.Text = "Getting tracking mode - please wait.";
                statusmsgTxtBox.Update();
                ret = CommandString("T#");

                LogMessageToFile("ManualUpdate - get fanspeed: send F#");
                statusmsgTxtBox.Text = "Getting fanspeed - please wait.";
                statusmsgTxtBox.Update();
                ret = CommandString("F#");

                LogMessageToFile("ManualUpdate - get pcbtemp: send K#");
                statusmsgTxtBox.Text = "Getting pcbtemp - please wait.";
                statusmsgTxtBox.Update();
                ret = CommandString("K#");

                // ch1/2/3 offset
                LogMessageToFile("ManualUpdate - get channel1/2 offsets: send ?#");
                statusmsgTxtBox.Text = "Getting ch1/2/3 offsets - please wait.";
                statusmsgTxtBox.Update();
                ret = CommandString("?#");

                // gets ch3 mode
                LogMessageToFile("ManualUpdate - get shadow channel: send E#");
                statusmsgTxtBox.Text = "Getting ch3 mode - please wait.";
                statusmsgTxtBox.Update();
                ret = CommandString("E#");
                statusmsgTxtBox.Text = "ch3 mode updated.";
                statusmsgTxtBox.Update();

                // other get calls would go here

                LogMessageToFile("ManualUpdate: all commands sent");
                updatingvals = false;

                statusmsgTxtBox.Text = "Adding to chart";
                LogMessageToFile("ManualUpdate: Adding To Chart: Start");

                myAddToChart();                   // add the new values to the chart

                statusmsgTxtBox.Text = "Manual Update Done";
                statusmsgTxtBox.Update();
                LogMessageToFile("ManualUpdate: Adding To Chart: Finish");

                // beep to indicate the update is complete
                if (soundenabled)
                {
                    SystemSounds.Beep.Play();
                    LogMessageToFile("ManualUpdate: Sound enabled");
                }
                else
                    LogMessageToFile("ManualUpdate: Sound disabled");

                // now handle datalogging
                LogMessageToFile("ManualUpdate: datalogging check");
                if (logfilename != "")          // a file has been chosen
                {
                    if (LogDataChkBox.Checked == true) // datalogging enabled
                    {
                        LogMessageToFile("ManualUpdateBtn: log data to datafile");
                        logdatatofile();                // append the data to the logfile
                    }
                }
            }
            else
            {
                LogMessageToFile("ManualUpdate: Com Port not set");
                statusmsgTxtBox.Text = "COM Port not set";
                statusmsgTxtBox.Update();
            }
            LogMessageToFile("ManualUpdate: Finish");
        }

        private void soundBtn_CheckedChanged(object sender, EventArgs e)
        {
            LogMessageToFile("soundBtn: Start");
            if (soundChkBox.Checked)
            {
                LogMessageToFile("soundBtn: ON");
                soundenabled = true;
            }
            else
            {
                LogMessageToFile("soundBtn: OFF");
                soundenabled = false;
            }
            LogMessageToFile("soundBtn: Finish");
        }

        private void ch1100Btn_Click(object sender, EventArgs e)
        {
            LogMessageToFile("ch1100Btn: Start");
            LogMessageToFile("ch1100Btn: Check if connected");
            if (comconnected)
            {
                LogMessageToFile("ch1100Btn: Check if connected is true");
                LogMessageToFile("ch1100Btn: Check if overrride");
                if (!Overridech1)
                {
                    LogMessageToFile("ch1100Btn: Check if overrride is false");
                    Overridech1 = true;
                    // start the timer and sent the command  "1#"
                    LogMessageToFile("ch1100Btn: Enable timer");
                    IdleTimer1.Enabled = true;      // will run for 2 minutes
                    LogMessageToFile("ch1100Btn: Send override to controller 1#");
                    try
                    {
                        serialPort1.Write("1#");
                        statusmsgTxtBox.Text = "Override ON + Timer1 started";
                        statusmsgTxtBox.Update();
                    }
                    catch (Exception)
                    {
                        LogMessageToFile("ch1100Btn: serial port exception");
                        Overridech1 = false;
                        IdleTimer1.Enabled = false;
                        shutdownport("Error sending command to serial port");
                    }
                }
            }
            else
            {
                LogMessageToFile("ch1100Btn: serial port not connected");
                statusmsgTxtBox.Text = "COM Port not set";
                statusmsgTxtBox.Update();
            }
            LogMessageToFile("ch1100Btn: Finish");
        }

        private void comportConnectBtn_Click(object sender, EventArgs e)
        {
            string tcmd = "";
            string ret = "";

            LogMessageToFile("Connect: Started");
            LogMessageToFile("myDewControllerPro3 Version: " + myVersion + "\n");

            if (testmode)
            {
                LogMessageToFile("Connect: testmode = true - pretending to connect");
                comconnected = true;
                setvalues();
                controlson();
                return;
            }

            if (this.comboBox1.SelectedItem != null)
            {
                LogMessageToFile("Connect: Comport selected item != NULL");
                if (!comconnected)
                {
                    // valid comport
                    // update comport name with selected item and save it
                    LogMessageToFile("Connect: save comport name");
                    ComPortName = comboBox1.GetItemText(comboBox1.SelectedItem);
                    Properties.Settings.Default.COMPORT = ComPortName;
                    Properties.Settings.Default.Save();

                    LogMessageToFile("Connect: try to connect to comport");
                    statusmsgTxtBox.Text = "Attempting connection using " + ComPortName;
                    statusmsgTxtBox.Update();
                    serialPort1.PortName = Convert.ToString(comboBox1.SelectedItem);
                    LogMessageToFile("Connect: Comport=" + serialPort1.PortName);
                    serialPort1.BaudRate = Convert.ToInt32(ComPortSpeed.GetItemText(ComPortSpeed.SelectedItem));
                    serialPort1.ReceivedBytesThreshold = 1;
                    serialPort1.ReadTimeout = 10000;     // timeout for read, 10 seconds, as startup code in firmware is pretty intensive
                    serialPort1.WriteTimeout = 5000;    // timeout for write
                    LogMessageToFile("Connect: Serial Port ReadTimeout = " + serialPort1.ReadTimeout.ToString());
                    serialPort1.DtrEnable = true;
                    serialPort1.RtsEnable = true;
                    Done = false;                   // we are not connected and have no controller parameters

                    String TimeNow = DateTime.Now.TimeOfDay.ToString(); // get current time
                    LogMessageToFile("Connect: Connection Sequence Timer Start (hh:mm:ss.nnnn) = " + TimeNow);

                    try
                    {
                        LogMessageToFile("Connect: Attempt to open serial port now");
                        serialPort1.Open();
                        LogMessageToFile("Connect: Check if serial port opened");
                        if (serialPort1.IsOpen)
                        {
                            comconnected = true;
                            LogMessageToFile("Connect: Comport=isOpen");
                            statusmsgTxtBox.Text = "Connecting.... Please wait....";
                            statusmsgTxtBox.Update();
                        }
                        else
                        {
                            LogMessageToFile("Connect: Comport=NOT connected");
                            shutdownport("Serial Port is not connected");
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        LogMessageToFile("Connect: Access to the serial port denied:");
                        LogMessageToFile("Connect: Comport=NOT connected");
                        shutdownport("Access to serial port denied");
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        LogMessageToFile("Connect: One or more properties of serial port is invalid:");
                        LogMessageToFile("Connect: Comport=NOT connected");
                        shutdownport("Error: One or more properties of serial port is invalid");
                    }
                    catch (ArgumentException)
                    {
                        LogMessageToFile("Connect: Port name does not begin with COM:");
                        LogMessageToFile("Connect: Comport=NOT connected");
                        shutdownport("Error: Serial port name does not begin with COM");
                    }
                    catch (IOException)
                    {
                        LogMessageToFile("Connect: IOexception Error:");
                        LogMessageToFile("Connect: Comport=NOT connected");
                        shutdownport("Error: Serial port IO error");
                    }
                    catch (InvalidOperationException)
                    {
                        LogMessageToFile("Connect: invalid operation exception:");
                        LogMessageToFile("Connect: Comport already open! - Closing");
                        shutdownport("Error: Serial port already open");
                    }
                    catch (Exception)
                    {
                        LogMessageToFile("Connect: Comport exception:");
                        shutdownport("Error: Could not open serial port");
                    }
                    if (serialPort1.IsOpen)
                    {
                        LogMessageToFile("Connect: Comport Opened, wait 3s");
                        statusmsgTxtBox.Text = "Connecting.... Please wait....";
                        statusmsgTxtBox.Update();

                        // reset graph points
                        if (graphfrm != null)
                            graphfrm.clearpoints();

                        // wait forcontroller to start up
                        PauseForTime(3, 0);
                        // get version number of controller
                        try
                        {
                            LogMessageToFile("Connect: sendcmd v# to get version");
                            statusmsgTxtBox.Text = "Getting firmware version...";
                            statusmsgTxtBox.Update();
                            tcmd = "v#";
                            commandinprogress = true;
                            ret = CommandString(tcmd);
                            if (ret == "")
                            {
                                comconnected = false;
                                LogMessageToFile("Connect: Could not connect, null response to v# command");
                                controlsoff();
                                // beep to indicate connection made
                                SystemSounds.Exclamation.Play();
                                shutdownport("myDewControllerPro3 did not respond to request for firmware version.");
                                return;
                            }
                            else
                            {
                                // beep to indicate connection made
                                SystemSounds.Beep.Play();
                                statusmsgTxtBox.Text = "Connected.... Please wait....";
                                statusmsgTxtBox.Update();
                                LogMessageToFile("Connect: Comport=connected");
                                LogMessageToFile("Connect: Getting controller parameters now");
                                comconnected = true;
                            }
                        }
                        catch (TimeoutException)
                        {
                            LogMessageToFile("Connect: Timeout Exception sending command: v#");
                            LogMessageToFile("Connect: Closing and disconnecting ComPort:" + ComPortName);
                            shutdownport("Error: Serial port timeout exception");
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("Connect: Exception sending command #v");
                            LogMessageToFile("Connect: Comport=disconnected");
                            shutdownport("Error: Exception when sending command to serial port");
                        }

                        statusmsgTxtBox.Text = "Checking for Version 3 myDewControllerPro3.... Please wait....";
                        statusmsgTxtBox.Update();
                        // check firmware version, must be 3 or greater
                        if (MajorRevNumber >= '3')
                        {
                            statusmsgTxtBox.Text = "Firmware is v3xx, getting settings from myDewControllerPro3, Please wait....";
                            //PauseForTime(0, 500);     // delay inserted here to give controller some time as it is still setting up
                            statusmsgTxtBox.Update();
                        }
                        else
                        {
                            LogMessageToFile("Error: Version 2 myDewControllerPro2 is attached");
                            LogMessageToFile("Connect: Version2 myDewControllerPro2 detected");
                            comconnected = false;
                            LogMessageToFile("Connect: Comport=disconnected");
                            shutdownport("Error version2 controller detected");
                        }
                        statusmsgTxtBox.Text = "Connected.... Please wait....";
                        statusmsgTxtBox.Update();
                        // PauseForTime(0, 250);             // controller is still processing startup commands
                        // set controller to celsius
                        try
                        {
                            LogMessageToFile("Connect: Set myDewControllerPro3 to celsius: c#");
                            statusmsgTxtBox.Text = "Setting myDewControllerPro3 to Celsius";
                            statusmsgTxtBox.Update();
                            serialPort1.Write("c#");
                            PauseForTime(0, 100);   // wait a 100millisec delay
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("Connect: Error writing c# to myDewControllerPro3");
                            shutdownport("Error sending command to serial port");
                        }

                        // get controller values
                        // ambient, humidity, dewpoint, ch1/2/3 temp, ch1/2/3 power, atbias, tracking mode, fanspeed
                        LogMessageToFile("Connect - get ambient: send A#");
                        statusmsgTxtBox.Text = "Getting ambient - please wait.";
                        statusmsgTxtBox.Update();
                        ret = CommandString("A#");
                        // PauseForTime(0, 100);

                        LogMessageToFile("Connect - get humidity: send R#");
                        statusmsgTxtBox.Text = "Getting humidity - please wait.";
                        statusmsgTxtBox.Update();
                        ret = CommandString("R#");
                        // PauseForTime(0, 100);

                        LogMessageToFile("Connect - get dewpoint: send D#");
                        statusmsgTxtBox.Text = "Getting dewpoint - please wait.";
                        statusmsgTxtBox.Update();
                        ret = CommandString("D#");
                        // PauseForTime(0, 100);

                        LogMessageToFile("Connect - get atbias: send B#");
                        statusmsgTxtBox.Text = "Getting atbias - please wait.";
                        statusmsgTxtBox.Update();
                        ret = CommandString("B#");
                        // PauseForTime(0, 100);

                        LogMessageToFile("Connect - get tracking mode: send T#");
                        statusmsgTxtBox.Text = "Getting tracking mode - please wait.";
                        statusmsgTxtBox.Update();
                        ret = CommandString("T#");
                        // PauseForTime(0, 100);

                        LogMessageToFile("Connect - get fanspeed: send F#");
                        statusmsgTxtBox.Text = "Getting fanspeed - please wait.";
                        statusmsgTxtBox.Update();
                        ret = CommandString("F#");
                        // PauseForTime(0, 100);

                        LogMessageToFile("Connect - get ch1/2/3 temps: send C#");
                        statusmsgTxtBox.Text = "Getting ch1/2/3 temps - please wait.";
                        statusmsgTxtBox.Update();
                        ret = CommandString("C#");
                        // PauseForTime(0, 100);

                        LogMessageToFile("Connect - get ch1/2/3 power: send W#");
                        statusmsgTxtBox.Text = "Getting ch1/2/3 power - please wait.";
                        statusmsgTxtBox.Update();
                        ret = CommandString("W#");
                        // PauseForTime(0, 100);

                        // gets ch3 mode
                        LogMessageToFile("Connect - get shadow channel: send E#");
                        statusmsgTxtBox.Text = "Getting ch3 mode - please wait.";
                        statusmsgTxtBox.Update();
                        ret = CommandString("E#");
                        // PauseForTime(0, 100);

                        // ch1/2/3 offset
                        LogMessageToFile("Connect - get channel1/2 offsets: send ?#");
                        statusmsgTxtBox.Text = "Getting ch1/2/3 offsets - please wait.";
                        statusmsgTxtBox.Update();
                        ret = CommandString("?#");
                        // PauseForTime(0, 100);

                        // number of probes
                        LogMessageToFile("Connect - get number of temp probes: send g#");
                        statusmsgTxtBox.Text = "Getting number of temp probes - please wait.";
                        statusmsgTxtBox.Update();
                        ret = CommandString("g#");
                        // PauseForTime(0, 100);

                        // lcddisplaytime - get setting
                        LogMessageToFile("Connect - get lcddisplaytime");
                        statusmsgTxtBox.Text = "Getting lcddisplaytime from myDewControllerPro3";
                        statusmsgTxtBox.Update();
                        tcmd = "H#";            // gets lcddisplaytime
                        ret = CommandString(tcmd);
                        // PauseForTime(0, 100);
                        // commandstring updates the settings menu bar

                        // other connection set/get calls would go here

                        TimeNow = DateTime.Now.TimeOfDay.ToString(); // get current time
                        LogMessageToFile("Connect: Connection Sequence Timer Stop (Time hh:mm:ss.nnn)= " + TimeNow);

                        // get pcbtemp
                        this.Invoke(new EventHandler(getPCBTempBtn_Click));

                        // get pcbtempoff
                        this.Invoke(new EventHandler(getFanTempOffBtn_Click));

                        statusmsgTxtBox.Text = "READY....";
                        statusmsgTxtBox.Update();
                        LogMessageToFile("Connect: All values retrieved");
                        comconnected = true;
                        Done = true;                    // we are connected and have controller parameters
                        //PauseForTime(0, 500);           // small pause before controls are connected                  
                        controlson();
                        // beep to indicate connection made
                        SystemSounds.Beep.Play();
                    }
                    else
                    {
                        LogMessageToFile("Connect: Com Port NOT connected - open failed");
                        statusmsgTxtBox.Text = "Com Port NOT connected - open failed";
                        statusmsgTxtBox.Update();
                        controlsoff();
                    }
                }
                else
                {
                    LogMessageToFile("Connect: Eroor: Com Port already connected");
                    statusmsgTxtBox.Text = "ERR: Com Port already connected";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("Connect: Error: Com Port not specified or null");
                statusmsgTxtBox.Text = "ERR: Com Port not specified or null";
                statusmsgTxtBox.Update();
            }
        }

        private void IdleTimer1_Tick(object sender, EventArgs e)
        {
            IdleTimer1.Enabled = false;
            Overridech1 = false;
            Overridech2 = false;
            statusmsgTxtBox.Text = "Timer stop: Override OFF";
            if (comconnected)
            {
                serialPort1.Write("n#");   // turn off override
            }
            else
            {
                statusmsgTxtBox.Text = "COM Port not set";
                statusmsgTxtBox.Update();
            }
        }

        private void ch2100Btn_Click(object sender, EventArgs e)
        {
            LogMessageToFile("ch2100Btn: Start");
            LogMessageToFile("ch2100Btn: com port connected check");
            if (comconnected)
            {
                LogMessageToFile("ch2100Btn: com port connected check is true");
                LogMessageToFile("ch2100Btn: override ch2 check");
                if (!Overridech2)
                {
                    LogMessageToFile("ch2100Btn: com port connected check is false");
                    Overridech2 = true;
                    // start the timer and sent the command  "1#"
                    IdleTimer1.Enabled = true;      // will run for 2 minutes
                    try
                    {
                        LogMessageToFile("ch2100Btn: send 2#");
                        serialPort1.Write("2#");
                        statusmsgTxtBox.Text = "Override ON + Timer2 started";
                        statusmsgTxtBox.Update();
                    }
                    catch (Exception)
                    {
                        LogMessageToFile("ch2100Btn: send 2# failed");
                        Overridech2 = false;
                        IdleTimer1.Enabled = false;      // disable
                        shutdownport("Sending command to serial port failed");
                    }
                }
            }
            else
            {
                LogMessageToFile("ch2100Btn: COM Port not connected");
                statusmsgTxtBox.Text = "COM Port not connected";
                statusmsgTxtBox.Update();
            }
            LogMessageToFile("ch2100Btn: Finish");
        }

        private void off100Btn_Click(object sender, EventArgs e)
        {
            LogMessageToFile("off100Btn: Start");
            LogMessageToFile("off100Btn: check com connected");
            if (comconnected)
            {
                LogMessageToFile("off100Btn: check com connected is true");
                Overridech1 = false;
                Overridech2 = false;
                IdleTimer1.Enabled = false;
                statusmsgTxtBox.Text = "Setting Ch1/Ch2 to normal";
                statusmsgTxtBox.Update();
                try
                {
                    LogMessageToFile("off100Btn: send n#");
                    serialPort1.Write("n#");
                }
                catch (Exception)
                {
                    LogMessageToFile("off100Btn: send n# failed, serial port error");
                    shutdownport("Sending command to serial port failed");
                }
            }
            else
            {
                LogMessageToFile("off100Btn: check com connected is false");
                Overridech1 = false;
                Overridech2 = false;
                IdleTimer1.Enabled = false;
                statusmsgTxtBox.Text = "COM Port not set";
                statusmsgTxtBox.Update();
            }
            LogMessageToFile("off100Btn: Finish");
        }

        private void offsetminusBtn_Click(object sender, EventArgs e)
        {
            LogMessageToFile("offsetminusBtn: Start");
            LogMessageToFile("offsetminusBtn: Check if com port connected");
            if (comconnected)
            {
                LogMessageToFile("offsetminusBtn: Check if com port connected is true");
                OffsetVal = OffsetVal - 1;
                if (OffsetVal < -4) OffsetVal = -4;
                statusmsgTxtBox.Text = "Offset = " + OffsetVal.ToString();
                statusmsgTxtBox.Update();
                try
                {
                    LogMessageToFile("offsetminusBtn: send <#");
                    serialPort1.Write("<#");
                }
                catch (Exception)
                {
                    LogMessageToFile("offsetminusBtn: send <# failed");
                    shutdownport("Sending command to serial port failed");
                }
            }
            else
            {
                LogMessageToFile("offsetminusBtn: Check if com port connected is false");
                statusmsgTxtBox.Text = "COM Port not set";
                statusmsgTxtBox.Update();
            }
            LogMessageToFile("offsetminusBtn: Finish");
        }

        private void offsetplusBtn_Click(object sender, EventArgs e)
        {
            LogMessageToFile("offsetplusBtn: Start");
            LogMessageToFile("offsetplusBtn: Check if connected");
            if (comconnected)
            {
                LogMessageToFile("offsetplusBtn: Check if connected is true");
                OffsetVal = OffsetVal + 1;
                if (OffsetVal > 4) OffsetVal = 4;
                statusmsgTxtBox.Text = "Offset = " + OffsetVal.ToString();
                statusmsgTxtBox.Update();
                try
                {
                    LogMessageToFile("offsetplusBtn: send ># to myDewControllerPro3");
                    serialPort1.Write(">#");
                }
                catch (Exception)
                {
                    LogMessageToFile("offsetplusBtn: send ># to myDewControllerPro3 failed");
                    shutdownport("Sending command to serial port failed");
                }
            }
            else
            {
                LogMessageToFile("offsetplusBtn: Check if connected is false");
                statusmsgTxtBox.Text = "COM Port not set";
                statusmsgTxtBox.Update();
            }
            LogMessageToFile("offsetplusBtn: Finish");
        }

        private void offsetzeroBtn_Click(object sender, EventArgs e)
        {
            LogMessageToFile("offsetzeroBtn: Start");
            LogMessageToFile("offsetzeroBtn: Check if connected");
            if (comconnected)
            {
                LogMessageToFile("offsetzeroBtn: Check if connected is true");
                OffsetVal = 0;
                statusmsgTxtBox.Text = "Offset = " + OffsetVal.ToString();
                statusmsgTxtBox.Update();
                try
                {
                    LogMessageToFile("offsetzeroBtn: send z# to myDewControllerPro");
                    serialPort1.Write("z#");
                }
                catch (Exception)
                {
                    LogMessageToFile("offsetzeroBtn: send z# to myDewControllerPro3 failed");
                    shutdownport("Sending command to serial port failed");
                }
            }
            else
            {
                LogMessageToFile("offsetzeroBtn: Check if connected is false");
                statusmsgTxtBox.Text = "COM Port not set";
                statusmsgTxtBox.Update();
            }
            LogMessageToFile("offsetzeroBtn: Finish");
        }

        private void ch1tempLabel_Click(object sender, EventArgs e)
        {
            LogMessageToFile("ch1tempLabel: Start");
            if (labelnewTxtBox.Text != "")
            {
                Properties.Settings.Default.ch1labeltext = labelnewTxtBox.Text;
                Properties.Settings.Default.Save();
                ch1templabel.Text = Properties.Settings.Default.ch1labeltext + " Temperature";
                ch1pwrlabel.Text = Properties.Settings.Default.ch1labeltext + " Power";
            }
            LogMessageToFile("ch1tempLabel: Finish");
        }

        private void ch2tempLabel_Click(object sender, EventArgs e)
        {
            LogMessageToFile("ch2tempLabel: Start");
            if (labelnewTxtBox.Text != "")
            {
                Properties.Settings.Default.ch2labeltext = labelnewTxtBox.Text;
                Properties.Settings.Default.Save();
                ch2templabel.Text = Properties.Settings.Default.ch2labeltext + " Temperature";
                ch2pwrlabel.Text = Properties.Settings.Default.ch2labeltext + " Power";
            }
            LogMessageToFile("ch2tempLabel: Finish");
        }

        private void ch3templabel_Click(object sender, EventArgs e)
        {
            LogMessageToFile("ch3tempLabel: Start");
            if (labelnewTxtBox.Text != "")
            {
                Properties.Settings.Default.ch3labeltext = labelnewTxtBox.Text;
                Properties.Settings.Default.Save();
                ch3templabel.Text = Properties.Settings.Default.ch3labeltext + " Temperature";
                ch3pwrlabel.Text = Properties.Settings.Default.ch3labeltext + " Power";
            }
            LogMessageToFile("ch3tempLabel: Finish");
        }

        private void labelnewBtn_Click(object sender, EventArgs e)
        {
            ch1templabel.Text = "Ch1 Temperature";
            ch1pwrlabel.Text = "Ch1 Power";
            ch2templabel.Text = "Ch2 Temperature";
            ch2pwrlabel.Text = "Ch2 Power";
            ch3templabel.Text = "Ch3 Temperature";
            ch3pwrlabel.Text = "Ch3 Power";
            ch1templabel.Update();
            ch2templabel.Update();
            ch3templabel.Update();
            ch1pwrlabel.Update();
            ch2pwrlabel.Update();
            ch3pwrlabel.Update();
            Properties.Settings.Default.ch1labeltext = "Ch1 ";
            Properties.Settings.Default.ch2labeltext = "Ch2 ";
            Properties.Settings.Default.ch3labeltext = "Ch3 ";
            Properties.Settings.Default.Save();
        }

        private void writeEEPROMBtn_Click(object sender, EventArgs e)
        {
            LogMessageToFile("writeEEPROMBtn: Start");
            LogMessageToFile("writeEEPROMBtn: Check if connected");
            if (comconnected)
            {
                LogMessageToFile("writeEEPROMBtn: Check if connected is true");
                statusmsgTxtBox.Text = "Writing variables to EEPROM";
                statusmsgTxtBox.Update();
                try
                {
                    LogMessageToFile("writeEEPROMBtn: Send w# to myDewControllerPro3");
                    serialPort1.Write("w#");
                }
                catch (Exception)
                {
                    LogMessageToFile("writeEEPROMBtn: Send w# to myDewControllerPro3 failed");
                    shutdownport("Sending command to serial port failed");
                }
            }
            else
            {
                LogMessageToFile("writeEEPROMBtn: Check if connected is false");
                statusmsgTxtBox.Text = "COM Port not set";
                statusmsgTxtBox.Update();
            }
            LogMessageToFile("writeEEPROMBtn: Finish");
        }

        private void TrackModeAmbient_CheckedChanged(object sender, EventArgs e)
        {
            // Ambient
            LogMessageToFile("TrackModeAmbient: Start");
            LogMessageToFile("TrackModeAmbient: Check if connected");
            if (Done)
            {
                if (comconnected)
                {
                    LogMessageToFile("TrackModeAmbient: Check if connected is true");
                    LogMessageToFile("TrackModeAmbient: Check if updating vals");
                    if (!updatingvals)
                    {
                        LogMessageToFile("TrackModeAmbient: Check if updating vals true");
                        LogMessageToFile("TrackModeAmbient: Check if button is checked");
                        if (TrackModeAmbient.Checked)
                        {
                            LogMessageToFile("TrackModeAmbient: Check if button is checked is true");
                            // tracking stete is ambient
                            statusmsgTxtBox.Text = "Setting Ambient tracking mode";
                            statusmsgTxtBox.Update();
                            try
                            {
                                LogMessageToFile("TrackModeAmbient: send a# command to myDewControllerPro3");
                                serialPort1.Write("a1#");
                            }
                            catch (Exception)
                            {
                                LogMessageToFile("TrackModeAmbient: send a# command to myDewControllerPro3 failed");
                                shutdownport("Sending command to serial port failed");
                            }
                        }
                    }
                }
                else
                {
                    LogMessageToFile("TrackModeAmbient: COM Port not set");
                    statusmsgTxtBox.Text = "COM Port not set";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("TrackModeAmbient: Done is false");
            }
            LogMessageToFile("TrackModeAmbient: Finish");
        }

        private void TrackModeDewPoint_CheckedChanged(object sender, EventArgs e)
        {
            // DewPoint
            LogMessageToFile("TrackModeDewPoint: Start");
            LogMessageToFile("TrackModeDewPoint: Check if connected");
            if (Done)
            {
                if (comconnected)
                {
                    LogMessageToFile("TrackModeDewPoint: Check if connected is true");
                    LogMessageToFile("TrackModeDewPoint: Check if updating vals");
                    if (!updatingvals)
                    {
                        LogMessageToFile("TrackModeDewPoint: Check if updating vals true");
                        LogMessageToFile("TrackModeDewPoint: Check if button is checked");
                        if (TrackModeDewPoint.Checked)
                        {
                            LogMessageToFile("TrackModeDewPoint: Check if button is checked is true");
                            // tracking stete is Dew Point
                            statusmsgTxtBox.Text = "Setting Dew Point tracking mode";
                            statusmsgTxtBox.Update();
                            try
                            {
                                LogMessageToFile("TrackModeDewPoint: send d# command to myDewControllerPro3");
                                serialPort1.Write("a2#");
                            }
                            catch (Exception)
                            {
                                LogMessageToFile("TrackModeDewPoint: send d# command to myDewControllerPro3 failed");
                                shutdownport("Sending command to serial port failed");
                            }
                        }
                    }
                }
                else
                {
                    LogMessageToFile("TrackModeDewPoint: COM Port not set");
                    statusmsgTxtBox.Text = "COM Port not set";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("TrackModeDewPoint: Done is false");
            }
            LogMessageToFile("TrackModeDewPoint: Finish");
        }

        private void TrackModeMidPoint_CheckedChanged(object sender, EventArgs e)
        {
            // Midpoint
            LogMessageToFile("TrackModeMidPoint: Start");
            LogMessageToFile("TrackModeMidPoint: Check if connected");
            if (Done)
            {
                if (comconnected)
                {
                    LogMessageToFile("TrackModeMidPoint: Check if connected is true");
                    LogMessageToFile("TrackModeMidPoint: Check if updating vals");
                    if (!updatingvals)
                    {
                        LogMessageToFile("TrackModeMidPoint: Check if updating vals true");
                        LogMessageToFile("TrackModeMidPoint: Check if button is checked");
                        if (TrackModeMidPoint.Checked)
                        {
                            LogMessageToFile("TrackModeMidPoint: Check if button is checked is true");
                            // tracking stete is Mid Point
                            statusmsgTxtBox.Text = "Setting Mid-Point tracking mode";
                            statusmsgTxtBox.Update();
                            try
                            {
                                LogMessageToFile("TrackModeMidPoint: send m# command to myDewControllerPro3");
                                serialPort1.Write("a3#");
                            }
                            catch (Exception)
                            {
                                LogMessageToFile("TrackModeMidPoint: send m# command to myDewControllerPro3 failed");
                                shutdownport("Sending command to serial port failed");
                            }
                        }
                    }
                }
                else
                {
                    LogMessageToFile("TrackModeMidPoint: COM Port not set");
                    statusmsgTxtBox.Text = "COM Port not set";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("TrackModeMidPoint: Done is false");
            }
            LogMessageToFile("TrackModeMidPoint: Finish");
        }

        private void FanSpeedZero_CheckedChanged(object sender, EventArgs e)
        {
            // Fan Speed 0
            LogMessageToFile("FanSpeedZero: Start");
            LogMessageToFile("FanSpeedZero: Check if connected");
            if (Done)
            {
                if (comconnected)
                {
                    LogMessageToFile("FanSpeedZero: Check if connected is true");
                    LogMessageToFile("FanSpeedZero: Check if updating vals");
                    if (!updatingvals)
                    {
                        LogMessageToFile("FanSpeedZero: Check if updating vals true");
                        LogMessageToFile("FanSpeedZero: Check if button is checked");
                        if (FanSpeedZero.Checked)
                        {
                            LogMessageToFile("FanSpeedZero: Check if button is checked is true");
                            // fan speed = 0
                            statusmsgTxtBox.Text = "Setting fan speed to 0";
                            statusmsgTxtBox.Update();
                            try
                            {
                                LogMessageToFile("FanSpeedZero: send s0# command to myDewControllerPro3");
                                serialPort1.Write("s0#");
                            }
                            catch (Exception)
                            {
                                LogMessageToFile("FanSpeedZero to serial port failed");
                            }
                        }
                    }
                }
                else
                {
                    LogMessageToFile("FanSpeedZero: COM Port not set");
                    statusmsgTxtBox.Text = "COM Port not set";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("FanSpeedZero: Done is false");
            }
            LogMessageToFile("FanSpeedZero: Finish");
        }

        private void FanSpeed50_CheckedChanged(object sender, EventArgs e)
        {
            // Fan Speed 50
            LogMessageToFile("FanSpeed50: Start");
            LogMessageToFile("FanSpeed50: Check if connected");
            if (Done)
            {
                if (comconnected)
                {
                    LogMessageToFile("FanSpeed50: Check if connected is true");
                    LogMessageToFile("FanSpeed50: Check if updating vals");
                    if (!updatingvals)
                    {
                        LogMessageToFile("FanSpeed50: Check if updating vals true");
                        LogMessageToFile("FanSpeed50: Check if button is checked");
                        if (FanSpeed50.Checked)
                        {
                            LogMessageToFile("FanSpeed50: Check if button is checked is true");
                            // fan speed = 50
                            statusmsgTxtBox.Text = "Setting fan speed to 50%";
                            statusmsgTxtBox.Update();
                            try
                            {
                                LogMessageToFile("FanSpeed50: send s50# command to myDewControllerPro3");
                                serialPort1.Write("s50#");
                            }
                            catch (Exception)
                            {
                                LogMessageToFile("FanSpeed50: send s50# command to myDewControllerPro3 failed");
                                shutdownport("Sending command to serial port failed");
                            }
                        }
                    }
                }
                else
                {
                    LogMessageToFile("FanSpeed50: COM Port not set");
                    statusmsgTxtBox.Text = "COM Port not set";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("FanSpeed50: Done is false");
            }
            LogMessageToFile("FanSpeed50: Finish");
        }

        private void FanSpeed75_CheckedChanged(object sender, EventArgs e)
        {
            // Fan Speed 75
            LogMessageToFile("FanSpeed75: Start");
            LogMessageToFile("FanSpeed75: Check if connected");
            if (Done)
            {
                if (comconnected)
                {
                    LogMessageToFile("FanSpeed75: Check if connected is true");
                    LogMessageToFile("FanSpeed75: Check if updating vals");
                    if (!updatingvals)
                    {
                        LogMessageToFile("FanSpeed75: Check if updating vals true");
                        LogMessageToFile("FanSpeed75: Check if button is checked");
                        if (FanSpeed75.Checked)
                        {
                            LogMessageToFile("FanSpeed75: Check if button is checked is true");
                            // fan speed = 75
                            statusmsgTxtBox.Text = "Setting fan speed to 75%";
                            statusmsgTxtBox.Update();
                            try
                            {
                                LogMessageToFile("FanSpeed75: send s75# command to myDewControllerPro3");
                                serialPort1.Write("s75#");
                            }
                            catch (Exception)
                            {
                                LogMessageToFile("FanSpeed75: send s75# command to myDewControllerPro3 failed");
                                shutdownport("Sending command to serial port failed");
                            }
                        }
                    }
                }
                else
                {
                    LogMessageToFile("FanSpeed75: COM Port not set");
                    statusmsgTxtBox.Text = "COM Port not set";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("FanSpeed75: Done is false");
            }
            LogMessageToFile("FanSpeed75: Finish");
        }

        private void FanSpeed100_CheckedChanged(object sender, EventArgs e)
        {
            // Fan Speed = 100
            LogMessageToFile("FanSpeed100: Start");
            LogMessageToFile("FanSpeed100: Check if connected");
            if (Done)
            {
                if (comconnected)
                {
                    LogMessageToFile("FanSpeed100: Check if connected is true");
                    LogMessageToFile("FanSpeed100: Check if updating vals");
                    if (!updatingvals)
                    {
                        LogMessageToFile("FanSpeed100: Check if updating vals true");
                        LogMessageToFile("FanSpeed100: Check if button is checked");
                        if (FanSpeed100.Checked)
                        {
                            LogMessageToFile("FanSpeed100: Check if button is checked is true");
                            // fan speed = 100
                            statusmsgTxtBox.Text = "Setting fan speed to 100%";
                            statusmsgTxtBox.Update();
                            try
                            {
                                LogMessageToFile("FanSpeed100: send s100# command to myDewControllerPro3");
                                serialPort1.Write("s100#");
                            }
                            catch (Exception)
                            {
                                LogMessageToFile("FanSpeed100: send s100# command to myDewControllerPro3 failed");
                                shutdownport("Sending command to serial port failed");
                            }
                        }
                    }
                }
                else
                {
                    LogMessageToFile("FanSpeed100: COM Port not set");
                    statusmsgTxtBox.Text = "COM Port not set";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("FanSpeed100: Done is false");
            }
            LogMessageToFile("FanSpeed100: Finish");
        }

        private void controlson()
        {
            // this is a dconnected state, on meaning connected to controller, so need to enable these controls
            LogMessageToFile("controlson: Start");

            // enable the lcddisplaytime menu
            lCDSCreenDisplayTimeToolStripMenuItem.Enabled = true;
            // enable the lcdstyle menu
            // enable the get controller firmware menu
            getControllerFirmwareVersionToolStripMenuItem.Enabled = true;
            // enable shadow dew channel menu
            shadowDewChannelSettingsToolStripMenuItem.Enabled = true;
            // enable temperature mode menu
            temperatureModeToolStripMenuItem.Enabled = true;
            
            refreshrateGroupBox.Enabled = true;

            trackingmodeGroupBox.Enabled = true;

            trackingModeOffsetGroupBox.Enabled = true;

            fanspeedGroupBox.Enabled = true;

            atbiasoffsetGroupBox.Enabled = true;

            PowerGroupBox.Enabled = true;

            TemperatureGroupBox.Enabled = false;

            OffsetsGroupBox.Enabled = true;

            PCBFanControlsGrpBox.Enabled = true;
            fanspeedGroupBox.Enabled = true; 
            
            automateChkBox.Enabled = true;

            disconnectComPortBtn.Enabled = true;
            writeEEPROMBtn.Enabled = true;

            manualupdateBtn.Enabled = true;
            LCDEnableChkBox.Enabled = true;
            comportConnectBtn.Enabled = false;
            RefreshComPortBtn.Enabled = false;

            resetControllertodefaultsBtn.Enabled = true;
            getfirmwareversion.Enabled = true;
            
            LogMessageToFile("controlson: Finish");
        }

        private void controlsoff()
        {
            // this is a disconnected state, off meaning not connected to controller
            LogMessageToFile("controlsoff: Start");
            IdleTimer1.Enabled = false;
            IdleTimer1.Interval = 120000;       // 2m, used for 100% override
            RefreshTimer.Enabled = false;
            RefreshTimer.Interval = 10000;      // 10s, used for automate refresh update

            // set temp and pwr boxes to 0
            // cleared before connect - in case an idiot clicks of celsius checkbox
            ambientTemperatureTxtBox.Text = "0";
            ambientTemperatureTxtBox.Update();
            relativeHumidityTxtBox.Text = "0";
            relativeHumidityTxtBox.Update();
            ch1tempTxtBox.Text = "0";
            ch1tempTxtBox.Update();
            ch2tempTxtBox.Text = "0";
            ch2tempTxtBox.Update();
            ch3tempTxtBox.Text = "0";
            ch3tempTxtBox.Update();
            dewpointTxtBox.Text = "0";
            dewpointTxtBox.Update();
            ch1pwrTxtBox.Text = "0";
            ch2pwrTxtBox.Text = "0";
            ch3pwrTxtBox.Text = "0";
            ch1pwrTxtBox.Update();
            ch2pwrTxtBox.Update();
            ch3pwrTxtBox.Update();
            // do NOT clear the ch1/2/3/tempoffset values!!!!!!!!!, they need to persist

            // set up default values - moved here from Load() in V312
            ArduinoFirmwareRev = "Not connected.";
            LCDEnableChkBox.Checked = true;
            comconnected = false;
            soundenabled = false;
            Overridech1 = false;
            Overridech2 = false;
            OffsetVal = 0;
            ControllerVersionTxtBox.Clear();
            controllerresponse = false;     // true if controller sends back DC#DCOK$
            controllerresponsestring1 = "";  // set to null for now
            controllerresponsestring2 = "";  // set to null for now

            // update settings menu bar for shadow dew channel3
            oFFToolStripMenuItem.Checked = false;
            channel1ToolStripMenuItem.Checked = false;
            channel2ToolStripMenuItem.Checked = false;
            manualSettingToolStripMenuItem.Checked = false;
            useTempProbe3ToolStripMenuItem.Checked = false;

            // disable the lcddisplaytime menu
            lCDSCreenDisplayTimeToolStripMenuItem.Enabled = false;
            // disable the lcdstyle menu

            // disable the get controller firmware menu
            getControllerFirmwareVersionToolStripMenuItem.Enabled = false;

            // disable shadow dew channel menu
            shadowDewChannelSettingsToolStripMenuItem.Enabled = false;

            // disable temperature mode menu
            temperatureModeToolStripMenuItem.Enabled = false;

            RefreshComPortBtn.Enabled = true;
            comportConnectBtn.Enabled = true;
          
            refreshrateGroupBox.Enabled = true;

            trackingmodeGroupBox.Enabled = false;

            trackingModeOffsetGroupBox.Enabled = false;

            atbiasoffsetGroupBox.Enabled = false;

            PowerGroupBox.Enabled = false;

            TemperatureGroupBox.Enabled = false;

            OffsetsGroupBox.Enabled = false;

            boardtemp = 0;
            pcbtempTxtBox.Text = boardtemp.ToString();
            pcbtempTxtBox.Update(); 
            PCBFanControlsGrpBox.Enabled = false;
            fanspeedGroupBox.Enabled = false;

            RefreshTimer.Enabled = false;
            automateChkBox.Checked = false;
            automateChkBox.Enabled = false;
            LogDataChkBox.Checked = false;
                        
            automateChkBox.Enabled = false;
            disconnectComPortBtn.Enabled = false;
            writeEEPROMBtn.Enabled = false;

            manualupdateBtn.Enabled = false;
            LCDEnableChkBox.Enabled = false;
            comportConnectBtn.Enabled = true; 
            RefreshComPortBtn.Enabled = true;

            resetControllertodefaultsBtn.Enabled = false;
            getfirmwareversion.Enabled = false;
            ControllerVersionTxtBox.Enabled = false;

            comconnected = false;
            Done = false;
            LogMessageToFile("controlsoff: Finish");
        }

        private void mysettooltips()
        {
            System.Windows.Forms.ToolTip cbTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip dbTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip rbTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip cplTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip rhTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip atTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip c1tTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip c2tTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip c3tTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip dpTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip c1pTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip c2pTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip c3pTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip nlTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip rlTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip lcdTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip tmbTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip fsbTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip atboTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip rrbTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip ch1100Tip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip ch2100Tip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip offTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip ch1toffTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip ch2toffTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip ch3toffTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip ch1soffTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip ch2soffTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip ch3soffTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip chgoffTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip ctoffTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip dlbTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip mubTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip exbTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip weebTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip soundbTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip autobTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip ldbTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip statusTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip sdbTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip ombTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip opbTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip ozbTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip ch3manpwrTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip atbiasTip = new System.Windows.Forms.ToolTip();

            cbTip.SetToolTip(this.comportConnectBtn, "Click to connect to the controller after selecting a comport");
            dbTip.SetToolTip(this.disconnectComPortBtn, "Click to disconnect the myDewControllerPro3");
            rbTip.SetToolTip(this.RefreshComPortBtn, "Click to refresh the list of available comports");
            cplTip.SetToolTip(this.comboBox1, "Click to refresh the list of available comports");
            rhTip.SetToolTip(this.relativeHumidityTxtBox, "Shows relative humdity reading from myDewControllerPro3");
            atTip.SetToolTip(this.ambientTemperatureTxtBox, "Shows ambient temperature reading from myDewControllerPro3");
            c1tTip.SetToolTip(this.ch1tempTxtBox, "Shows channel 1 temperature reading from myDewControllerPro3");
            c2tTip.SetToolTip(this.ch2tempTxtBox, "Shows channel 2 temperature reading from myDewControllerPro3");
            c3tTip.SetToolTip(this.ch3tempTxtBox, "Shows channel 3 temperature reading from myDewControllerPro3");
            dpTip.SetToolTip(this.dewpointTxtBox, "Shows calculated dewpoint temperature from myDewControllerPro3");
            c1pTip.SetToolTip(this.ch1pwrTxtBox, "Shows channel 1 power setting on myDewControllerPro3");
            c2pTip.SetToolTip(this.ch2pwrTxtBox, "Shows channel 2 power setting on myDewControllerPro3");
            c3pTip.SetToolTip(this.ch3pwrTxtBox, "Shows channel 3 power setting on myDewControllerPro3");
            nlTip.SetToolTip(this.labelnewTxtBox, "Enter a new text label here");
            rlTip.SetToolTip(this.labelnewBtn, "Click to reset text labels");
            lcdTip.SetToolTip(this.LCDEnableChkBox, "Check to enable myDewControllerPro3 LCD, uncheck to turn off myDewControllerPro3 LCD");
            tmbTip.SetToolTip(this.trackingmodeGroupBox, "Select the desired temperature tracking mode");
            fsbTip.SetToolTip(this.fanspeedGroupBox, "Select the desired fan speed");
            atboTip.SetToolTip(this.trackingmodeGroupBox, "Select the desired ambient temperature offset");
            rrbTip.SetToolTip(this.refreshrateGroupBox, "Select the desired refresh rate for graph and values");
            ch1100Tip.SetToolTip(this.ch1100Btn, "Click to set power level on channel 1 to 100%");
            ch2100Tip.SetToolTip(this.ch2100Btn, "Click to set power level on channel 2 to 100%");
            offTip.SetToolTip(this.off100Btn, "Click to set power levels on channel 1 and 2 to 0%");
            ch1toffTip.SetToolTip(this.ch1tempoffTxtBox, "Enter the desired offset value to adjust the temperature of probe1");
            ch2toffTip.SetToolTip(this.ch2tempoffTxtBox, "Enter the desired offset value to adjust the temperature of probe2");
            ch3toffTip.SetToolTip(this.ch3tempoffTxtBox, "Enter the desired offset value to adjust the temperature of probe3");
            ch1soffTip.SetToolTip(this.ch1tempoffsetBtn, "Sets the desired offset value to adjust the temperature of probe1");
            ch2soffTip.SetToolTip(this.ch2tempoffsetBtn, "Sets the desired offset value to adjust the temperature of probe2");
            ch3soffTip.SetToolTip(this.ch3tempoffsetBtn, "Sets the desired offset value to adjust the temperature of probe3");
            chgoffTip.SetToolTip(this.getchoffsetBtn, "Gets the temperature offset values for probe1/2 from the controller");
            ctoffTip.SetToolTip(this.cleartempoffsetBtn, "Clears the temperature offset values for probe1/2/3 - sets to 0");
            dlbTip.SetToolTip(this.DataLogBtn, "Click to setup datalogging of values to a file");
            mubTip.SetToolTip(this.manualupdateBtn, "Click to get values from the myDewControllerPro3");
            exbTip.SetToolTip(this.ExitBtn, "Click to exit this application");
            weebTip.SetToolTip(this.writeEEPROMBtn, "Click to save the current values/settings to the myDewControllerPro3");
            soundbTip.SetToolTip(this.soundChkBox, "Check to enable beep when application receives update from the myDewControllerPro3");
            autobTip.SetToolTip(this.automateChkBox, "Check to enable automatic updates from the myDewControllerPro3");
            ldbTip.SetToolTip(this.LogDataChkBox, "Check to enable logging of data to a datafile");
            statusTip.SetToolTip(this.statusmsgTxtBox, "Displays application and myDewControllerPro3 status messages");
            sdbTip.SetToolTip(this.SetDirectoryBtn, "Click to set the directory for the datafile logging");
            ombTip.SetToolTip(this.offsetminusBtn, "Click to subtract 1 from the offset bias used in tracking mode");
            opbTip.SetToolTip(this.offsetplusBtn, "Click to add 1 to the offset bias used in tracking mode");
            ozbTip.SetToolTip(this.offsetzeroBtn, "Click to set the offset bias used in tracking mode to 0");
            ch3manpwrTip.SetToolTip(this.ch3pwrSetButton, "Drag the slider to the desired power level then click Set to set CH3 to manual power");
            atbiasTip.SetToolTip(this.atbiasoffsetGroupBox, "Set the desired Ambient Temperature offset value to calibrate the Ambient Temperature from DHTxx sensor");
        }

        private void myDewController_Load(object sender, EventArgs e)
        {
            String tmpStr;

            testmode = false;

            // load application settings
            Properties.Settings.Default.Reload();
            this.Location = Properties.Settings.Default.FormLocation;

            // save threads culture
            newCulture = thisCulture = CultureInfo.CurrentCulture;
            newICCulture = thisICCulture = CultureInfo.CurrentUICulture;
            newCulture = CultureInfo.CreateSpecificCulture("en-US");
            newICCulture = CultureInfo.CreateSpecificCulture("en-US");

            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                myVersion = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                this.Text = cpystrbrown + myVersion;
            }
            else
            {
                myVersion = cpystrbrown;
                this.Text = myVersion;
            }

            // set tool tips
            mysettooltips();

            // set up errorlog path form
            elogpathfrm = new ErrorLogPathName();
            graphfrm = new GraphForm();

            // make up error logfilename
            int myYear = DateTime.Now.Date.Year;
            int myMonth = DateTime.Now.Date.Month;
            int myDay = DateTime.Now.Date.Day;

            // logfilename for program logging and erros = "myDewControllerPro3-" + myYearmyMonthmyDay + ".txt";
            // used by logerrorfile();
            proglogfilename = "myDewControllerPro3-" + myYear.ToString() + myMonth.ToString() + myDay.ToString() + ".txt";
            Properties.Settings.Default.ErrorLogName = proglogfilename;
            Properties.Settings.Default.Save();

            // check to see if app setting for "errorlogpath"
            if (Properties.Settings.Default.errorlogpath == "")
            {
                // its not set so activate form "ErrorLogPathName.cs" and get foldername
                // and save back into application setting
                Properties.Settings.Default.SubFormRunning = true;
                elogpathfrm.Show();
                while (Properties.Settings.Default.SubFormRunning == true)
                    PauseForTime(2, 0);
            }

            // check to see error logging state and set settings correctly
            if (Properties.Settings.Default.loggingerrors == false)
            {
                logerrorfile = false;
                disableErrorLogFileToolStripMenuItem.Checked = true;
                enableErrorLogFileToolStripMenuItem.Checked = false;
            }
            else
            {
                logerrorfile = true;
                disableErrorLogFileToolStripMenuItem.Checked = false;
                enableErrorLogFileToolStripMenuItem.Checked = true;
            }
            LogMessageToFile("Load: Started");
            LogMessageToFile("myDewControllerPro3 Version: " + myVersion + "\n");
            LogMessageToFile("Load: Current Application settings: Start");
            LogMessageToFile("ch1tempoffset =" + Properties.Settings.Default.ch1tempoffset);
            LogMessageToFile("ch2tempoffset =" + Properties.Settings.Default.ch2tempoffset);
            LogMessageToFile("ch3tempoffset =" + Properties.Settings.Default.ch3tempoffset);
            LogMessageToFile("ComPort =" + Properties.Settings.Default.COMPORT);
            LogMessageToFile("errorlogpath =" + Properties.Settings.Default.errorlogpath);
            LogMessageToFile("ErrorLogName =" + Properties.Settings.Default.ErrorLogName);
            if (Properties.Settings.Default.loggingerrors == true)
                LogMessageToFile("loggingerrors = true");
            else
                LogMessageToFile("loggingerrors = false");
            LogMessageToFile("Load: Current Application settings: Finished");

            // Get a list of serial port names.
            LogMessageToFile("Load: Adding comports to list");
            this.comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());

            ComPortName = Properties.Settings.Default.COMPORT;
            int index = this.comboBox1.FindString(ComPortName);
            if (index < 0)	// < 0 or -1 if not found
            {
                LogMessageToFile("Load: No Comports found");
                // do nothing
            }
            else
            {
                this.comboBox1.SelectedIndex = index; // set the list to the found comport
                LogMessageToFile("Load: Comport found = " + this.comboBox1.SelectedIndex.ToString());
            }

            ComPortBaudRate = Properties.Settings.Default.ComPortBaudRate;
            index = this.ComPortSpeed.FindString(ComPortBaudRate);
            if (index < 0)	// < 0 or -1 if not found
            {
                LogMessageToFile("Load: No ComPortSPeed found");
                // do nothing
            }
            else
            {
                this.ComPortSpeed.SelectedIndex = index; // set the list to the found comport
            }

            controlsoff();

            // start with data logging disabled
            LogMessageToFile("Load: Disable data logging");
            LogDataChkBox.Enabled = false;
            LogDataChkBox.Checked = false;

            // create the logfilename
            LogMessageToFile("Load: Creating data log file: Start");

            // logfilename = logfilename + DateNow + "-";
            logfilename = "myDewControllerPro3-" + myYear.ToString() + myMonth.ToString() + myDay.ToString() + "-";

            String TimeNow = DateTime.Now.TimeOfDay.ToString(); // get current time
            String myTime = "";
            for (int lpval = 0; lpval <= 4; lpval++)           // hh:mm:ss.nnnnn
            {
                if ((byte)TimeNow[lpval] != ':')
                    myTime += TimeNow[lpval];
            }
            myTime += "";                                       // hh.mm.ss
            logfilename = logfilename + myTime + ".csv";
            LogFileNameTxtBox.Text = logfilename;
            LogFileNameTxtBox.Update();

            if (Properties.Settings.Default.WriteEEPROMonExit)
            {
                yesToolStripMenuItem.Checked = true;
                noToolStripMenuItem.Checked = false;
            }
            else
            {
                yesToolStripMenuItem.Checked = false;
                noToolStripMenuItem.Checked = true;
            }

            // always start in Celsius - do not move to controlsoff as the user might change this after connect
            DisplayMode = Celsius;
            celsiusToolStripMenuItem.Checked = true;
            fahrenheitToolStripMenuItem.Checked = false;
            atlabel.Text = "C";
            ch1label.Text = "C";
            ch1label.Text = "C";
            ch2label.Text = "C";
            ch3label.Text = "C";
            atlabel.Update();
            ch1label.Update();
            ch1label.Update();
            ch2label.Update();
            ch3label.Update();

            // restore the labels for ch1/ch2/ch3 text
            ch1templabel.Text = Properties.Settings.Default.ch1labeltext + "Temperature";
            ch2templabel.Text = Properties.Settings.Default.ch2labeltext + "Temperature";
            ch3templabel.Text = Properties.Settings.Default.ch3labeltext + "Temperature";
            ch1pwrlabel.Text = Properties.Settings.Default.ch1labeltext + "Power";
            ch2pwrlabel.Text = Properties.Settings.Default.ch2labeltext + "Power";
            ch3pwrlabel.Text = Properties.Settings.Default.ch3labeltext + "Power";
            ch1templabel.Update();
            ch2templabel.Update();
            ch3templabel.Update();
            ch1pwrlabel.Update();
            ch2pwrlabel.Update();
            ch3pwrlabel.Update();

            commandinprogress = false;
            Done = false;                   // we are not connected and have no controller parameters

            // check ch1offsettemp
            tmpStr = ch1tempoffTxtBox.Text;
            if (tmpStr.Contains(","))
            {
                // replace any , with a .
                tmpStr = tmpStr.Replace(',', '.');
                ch1tempoffTxtBox.Text = tmpStr;
                ch1tempoffTxtBox.Update();
            }

            LogMessageToFile("Ch1TempOffsetTxtBox: = " + ch1tempoffTxtBox.Text);
            try
            {
                LogMessageToFile("ch1tempoffTxtBox_KeyPress Tempoffset value Before Parse = " + tmpStr);
                double temp1 = Double.Parse(tmpStr, newCulture);
                LogMessageToFile("ch1tempoffTxtBox_KeyPress Tempoffset value After Parse = " + Convert.ToString(temp1, newCulture));
                ch1tempoffsetval = (float)temp1;
                LogMessageToFile("ch1tempoffTxtBox_KeyPress Tempoffset value After Round = " + Convert.ToString(ch1tempoffsetval, newCulture));

                // rangecheck to +3 to -3
                if (ch1tempoffsetval < -3.0f)
                    ch1tempoffsetval = -3.0f;
                if (ch1tempoffsetval > 3.0f)
                    ch1tempoffsetval = 3.0f;
                ch1tempoffsetval = (float)Math.Round((Double)ch1tempoffsetval, 2);
                tmpStr = Convert.ToString(ch1tempoffsetval, newCulture);
                ch1tempoffTxtBox.Text = tmpStr;
                ch1tempoffTxtBox.Update();
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }
            catch (Exception)
            {
                LogMessageToFile("ch1tempoffTxtBox_KeyPress TempOffset - Invalid format for Temp Offset. Reset to 0");
                ch1tempoffsetval = 0.0f;
                tmpStr = Convert.ToString(ch1tempoffsetval, newCulture);
                ch1tempoffTxtBox.Text = tmpStr;
                ch1tempoffTxtBox.Update();
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }

            // check ch2offsettemp
            tmpStr = ch2tempoffTxtBox.Text;
            if (tmpStr.Contains(","))
            {
                // replace any , with a .
                tmpStr = tmpStr.Replace(',', '.');
                ch2tempoffTxtBox.Text = tmpStr;
                ch2tempoffTxtBox.Update();
            }
            LogMessageToFile("Ch2TempOffsetTxtBox: = " + ch2tempoffTxtBox.Text);
            try
            {
                LogMessageToFile("ch2tempoffTxtBox_KeyPress Tempoffset value Before Parse = " + tmpStr);
                double temp1 = Double.Parse(tmpStr, newCulture);
                LogMessageToFile("ch2tempoffTxtBox_KeyPress Tempoffset value After Parse = " + Convert.ToString(temp1, newCulture));
                ch2tempoffsetval = (float)temp1;
                LogMessageToFile("ch2tempoffTxtBox_KeyPress Tempoffset value After Round = " + Convert.ToString(ch2tempoffsetval, newCulture));

                // rangecheck to +3 to -3
                if (ch2tempoffsetval < -3.0f)
                    ch2tempoffsetval = -3.0f;
                if (ch2tempoffsetval > 3.0f)
                    ch2tempoffsetval = 3.0f;
                ch2tempoffsetval = (float)Math.Round((Double)ch2tempoffsetval, 2);
                tmpStr = Convert.ToString(ch2tempoffsetval, newCulture);
                ch2tempoffTxtBox.Text = tmpStr;
                ch2tempoffTxtBox.Update();
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }
            catch (Exception)
            {
                LogMessageToFile("ch2tempoffTxtBox_KeyPress TempOffset - Invalid format for Temp Offset. Reset to 0");
                ch2tempoffsetval = 0.0f;
                tmpStr = Convert.ToString(ch2tempoffsetval, newCulture);
                ch2tempoffTxtBox.Text = tmpStr;
                ch2tempoffTxtBox.Update();
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }

            // check ch3offsettemp
            tmpStr = ch3tempoffTxtBox.Text;
            if (tmpStr.Contains(","))
            {
                // replace any , with a .
                tmpStr = tmpStr.Replace(',', '.');
                ch3tempoffTxtBox.Text = tmpStr;
            }
            LogMessageToFile("Ch3TempOffsetTxtBox: = " + ch3tempoffTxtBox.Text);
            try
            {
                LogMessageToFile("ch3tempoffTxtBox_KeyPress Tempoffset value Before Parse = " + tmpStr);
                double temp1 = Double.Parse(tmpStr, newCulture);
                LogMessageToFile("ch3tempoffTxtBox_KeyPress Tempoffset value After Parse = " + Convert.ToString(temp1, newCulture));
                ch3tempoffsetval = (float)temp1;
                LogMessageToFile("ch3tempoffTxtBox_KeyPress Tempoffset value After Round = " + Convert.ToString(ch3tempoffsetval, newCulture));
                // rangecheck to +3 to -3
                if (ch3tempoffsetval < -3.0f)
                    ch3tempoffsetval = -3.0f;
                if (ch3tempoffsetval > 3.0f)
                    ch3tempoffsetval = 3.0f;
                ch3tempoffsetval = (float)Math.Round((Double)ch3tempoffsetval, 2);
                tmpStr = Convert.ToString(ch3tempoffsetval, newCulture);
                ch3tempoffTxtBox.Text = tmpStr;
                ch3tempoffTxtBox.Update();
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }
            catch (Exception)
            {
                LogMessageToFile("ch3tempoffTxtBox_KeyPress TempOffset - Invalid format for Temp Offset. Reset to 0");
                ch3tempoffsetval = 0.0f;
                tmpStr = Convert.ToString(ch3tempoffsetval, newCulture);
                ch3tempoffTxtBox.Text = tmpStr;
                ch3tempoffTxtBox.Update();
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }

            boardtemp = 0;

            LogMessageToFile("Load: data log filename = " + logfilename);
            LogMessageToFile("Load: Creating data log file: Finished");
            LogMessageToFile("Load: Finished");

            ClearStatusMsgsTimer.Interval = 3000;           // 3s
            ClearStatusMsgsTimer.Stop();
            ClearStatusMsgsTimer.Enabled = false;

            // start on a specific tab
            mytabControl.SelectTab(tabSerialPort);
        }

        private void disconnectComPortBtn_Click(object sender, EventArgs e)
        {
            LogMessageToFile("DisconnectComPortBtn: Start");
            LogMessageToFile("DisconnectComPortBtn: Check if connected");
            if (comconnected)
            {
                LogMessageToFile("DisconnectComPortBtn: Check if connected is true");
                // beep to indicate response required
                SystemSounds.Beep.Play();
                if (Properties.Settings.Default.WriteEEPROMonExit)
                {
                    try
                    {
                        LogMessageToFile("DisconnectComPortBtn: sendcmd w#");
                        sendcmd("w#");
                    }
                    catch (TimeoutException)
                    {
                        LogMessageToFile("DisconnectComPortBtn: Timeout Error occured sending w#");
                        shutdownport("Timeout error sending command to controller");
                    }
                    catch (Exception)
                    {
                        LogMessageToFile("DisconnectComPortBtn: Unspecified error occured sending w#");
                        shutdownport("Unspecified error sending command to controller");
                    }
                }
                LogMessageToFile("DisconnectComPortBtn: Closing COMPort");
                serialPort1.Close();
                LogMessageToFile("DisconnectComPortBtn: Disposing COMPort");
                serialPort1.Dispose();
                LogMessageToFile("DisconnectComPortBtn: Controls off");
                controlsoff();
                comconnected = false;
                Done = false;                   // we are not connected and have no controller parameters
                statusmsgTxtBox.Text = "Com Port closed";
                statusmsgTxtBox.Update();
                Properties.Settings.Default.Save();

                commandinprogress = false;

                // clear the datapoints in the graph
                LogMessageToFile("DisconnectComPortBtn: Clearing datapoints in chart");
                // clear all datapoints in chart
                if (graphfrm != null)
                    graphfrm.clearpoints();

                // v312, enable connect button
                comportConnectBtn.Enabled = true;
            }
            else
            {
                LogMessageToFile("DisconnectComPortBtn: Check if connected is false");
                statusmsgTxtBox.Text = "Com Port is not open";
                statusmsgTxtBox.Update();
            }
            LogMessageToFile("DisconnectComPortBtn: Finish");
        }

        private void automateChkBox_CheckedChanged(object sender, EventArgs e)
        {
            LogMessageToFile("automateChkBox: Start");
            LogMessageToFile("automateChkBox: Check if ticked");
            if (automateChkBox.Checked)
            {
                LogMessageToFile("automateChkBox: Check if ticked - is true");
                // start the refresh timer based on value of refresh update interval
                if (RefreshRate10s.Checked) RefreshTimer.Interval = 10000;
                else if (RefreshRate30s.Checked) RefreshTimer.Interval = 30000;
                else if (RefreshRate1m.Checked) RefreshTimer.Interval = 60000;
                else if (RefreshRate5m.Checked) RefreshTimer.Interval = 300000;
                else RefreshTimer.Interval = 10000;
                RefreshTimer.Enabled = true;
            }
            else
            {
                LogMessageToFile("automateChkBox: Check if ticked - is false");
                RefreshTimer.Enabled = false;
            }
            LogMessageToFile("automateChkBox: Finish");
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            statusmsgTxtBox.Text = "Requesting myDewControllerPro3 Values";
            statusmsgTxtBox.Update();
            this.Invoke(new EventHandler(manualupdateBtn_Click));
        }

        private void RefreshRate10s_CheckedChanged(object sender, EventArgs e)
        {
            LogMessageToFile("RefreshRate10s: Start");
            LogMessageToFile("RefreshRate10s: Check if connected");
            if (Done)
            {
                if (comconnected)
                {
                    LogMessageToFile("RefreshRate10s: Check if connected is true");
                    if (RefreshRate10s.Checked) RefreshTimer.Interval = 10000;
                }
                else
                {
                    LogMessageToFile("RefreshRate10s: Check if connected is false");
                    statusmsgTxtBox.Text = "COM Port not set";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("RefreshRate10s: Done is false");
            }
            LogMessageToFile("RefreshRate10s: Finish");
        }

        private void RefreshRate30s_CheckedChanged(object sender, EventArgs e)
        {
            LogMessageToFile("RefreshRate30s: Start");
            LogMessageToFile("RefreshRate30s: Check if connected");
            if (Done)
            {
                if (comconnected)
                {
                    LogMessageToFile("RefreshRate30s: Check if connected - is true");
                    if (RefreshRate30s.Checked) RefreshTimer.Interval = 30000;
                }
                else
                {
                    LogMessageToFile("RefreshRate30s: Check if connected - is false");
                    statusmsgTxtBox.Text = "COM Port not set";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("RefreshRate10s: Done is false");
            }
            LogMessageToFile("RefreshRate30s: Finish");
        }

        private void RefreshRate1m_CheckedChanged(object sender, EventArgs e)
        {
            LogMessageToFile("RefreshRate1m: Start");
            LogMessageToFile("RefreshRate1m: Check if connected");
            if (Done)
            {
                if (comconnected)
                {
                    LogMessageToFile("RefreshRate1m: Check if connected - is true");
                    if (RefreshRate1m.Checked) RefreshTimer.Interval = 60000;
                }
                else
                {
                    LogMessageToFile("RefreshRate1m: Check if connected - is false");
                    statusmsgTxtBox.Text = "COM Port not set";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("RefreshRate10s: Done is false");
            }
            LogMessageToFile("RefreshRate1m: Finish");
        }

        private void RefreshRate5m_CheckedChanged(object sender, EventArgs e)
        {
            LogMessageToFile("RefreshRate5m: Start");
            LogMessageToFile("RefreshRate5m: Check if connected");
            if (Done)
            {
                if (comconnected)
                {
                    LogMessageToFile("RefreshRate5m: Check if connected - is true");
                    if (RefreshRate5m.Checked) RefreshTimer.Interval = 300000;
                }
                else
                {
                    LogMessageToFile("RefreshRate5m: Check if connected - is false");
                    statusmsgTxtBox.Text = "COM Port not set";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("RefreshRate10s: Done is false");
            }
            LogMessageToFile("RefreshRate5m: Finish");
        }

        // ATBias buttons
        private void ATBiasMinus4_CheckedChanged(object sender, EventArgs e)
        {
            LogMessageToFile("ATBiasMinus4: Start");
            LogMessageToFile("ATBiasMinus4: Check if connected");
            if (Done)
            {
                if (comconnected)
                {
                    LogMessageToFile("ATBiasMinus4: Check if connected - is true");
                    LogMessageToFile("ATBiasMinus4: Check if checked");
                    if (ATBiasMinus4.Checked)
                    {
                        LogMessageToFile("ATBiasMinus4: Check if checked - is true");
                        // ATBias Offset -4
                        statusmsgTxtBox.Text = "Setting ATemp Bias Offset to -4";
                        statusmsgTxtBox.Update();
                        try
                        {
                            LogMessageToFile("ATBiasMinus4: send e-4# to myDewControllerPro3");
                            serialPort1.Write("e-4#");
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("ATBiasMinus4: send e-4# to myDewControllerPro3 failed");
                            shutdownport("Sending command to serial port failed");
                        }
                    }
                }
                else
                {
                    LogMessageToFile("ATBiasMinus4: Check if connected - is false");
                    statusmsgTxtBox.Text = "COM Port not set";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("ATBiasMinus4: Done is false");
            }
            LogMessageToFile("ATBiasMinus4: Finish");
        }

        private void ATBiasMinus3_CheckedChanged(object sender, EventArgs e)
        {
            LogMessageToFile("ATBiasMinus3: Start");
            LogMessageToFile("ATBiasMinus3: Check if connected");
            if (Done)
            {
                if (comconnected)
                {
                    LogMessageToFile("ATBiasMinus3: Check if connected - is true");
                    LogMessageToFile("ATBiasMinus3: Check if checked");
                    if (ATBiasMinus3.Checked)
                    {
                        LogMessageToFile("ATBiasMinus3: Check if checked - is true");
                        // ATBias Offset -3
                        statusmsgTxtBox.Text = "Setting ATemp Bias Offset to -3";
                        statusmsgTxtBox.Update();
                        try
                        {
                            LogMessageToFile("ATBiasMinus3: send e-3# to myDewControllerPro3");
                            serialPort1.Write("e-3#");
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("ATBiasMinus3: send e-3# to myDewControllerPro3 failed");
                            shutdownport("Sending command to serial port failed");
                        }
                    }
                }
                else
                {
                    LogMessageToFile("ATBiasMinus3: Check if connected - is false");
                    statusmsgTxtBox.Text = "COM Port not set";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("ATBiasMinus3: Done is false");
            }
            LogMessageToFile("ATBiasMinus3: Finish");
        }

        private void ATBiasMinus2_CheckedChanged(object sender, EventArgs e)
        {
            LogMessageToFile("ATBiasMinus2: Start");
            LogMessageToFile("ATBiasMinus2: Check if connected");
            if (Done)
            {
                if (comconnected)
                {
                    LogMessageToFile("ATBiasMinus2: Check if connected - is true");
                    LogMessageToFile("ATBiasMinus2: Check if checked");
                    if (ATBiasMinus2.Checked)
                    {
                        LogMessageToFile("ATBiasMinus2: Check if checked - is true");
                        // ATBias Offset -2
                        statusmsgTxtBox.Text = "Setting ATemp Bias Offset to -2";
                        statusmsgTxtBox.Update();
                        try
                        {
                            LogMessageToFile("ATBiasMinus2: send e-2# to myDewControllerPro3");
                            serialPort1.Write("e-2#");
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("ATBiasMinus2: send e-2# to myDewControllerPro3 failed");
                            shutdownport("Sending command to serial port failed");
                        }
                    }
                }
                else
                {
                    LogMessageToFile("ATBiasMinus2: Check if connected - is false");
                    statusmsgTxtBox.Text = "COM Port not set";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("ATBiasMinus2: Done is false");
            }
            LogMessageToFile("ATBiasMinus2: Finish");
        }

        private void ATBiasMinus1_CheckedChanged(object sender, EventArgs e)
        {
            LogMessageToFile("ATBiasMinus1: Start");
            LogMessageToFile("ATBiasMinus1: Check if connected");
            if (Done)
            {
                if (comconnected)
                {
                    LogMessageToFile("ATBiasMinus1: Check if connected - is true");
                    LogMessageToFile("ATBiasMinus1: Check if checked");
                    if (ATBiasMinus1.Checked)
                    {
                        LogMessageToFile("ATBiasMinus1: Check if checked - is true");
                        // ATBias Offset -1
                        statusmsgTxtBox.Text = "Setting ATemp Bias Offset to -1";
                        statusmsgTxtBox.Update();
                        try
                        {
                            LogMessageToFile("ATBiasMinus1: send e-1# to myDewControllerPro3");
                            serialPort1.Write("e-1#");
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("ATBiasMinus1: send e-1# to myDewControllerPro3 failed");
                            shutdownport("Sending command to serial port failed");
                        }
                    }
                }
                else
                {
                    LogMessageToFile("ATBiasMinus1: Check if connected - is false");
                    statusmsgTxtBox.Text = "COM Port not set";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("ATBiasMinus1: Done is false");
            }
            LogMessageToFile("ATBiasMinus1: Finish");
        }

        private void ATBiasZero_CheckedChanged(object sender, EventArgs e)
        {
            LogMessageToFile("ATBiasZero: Start");
            LogMessageToFile("ATBiasZero: Check if connected");
            if (Done)
            {
                if (comconnected)
                {
                    LogMessageToFile("ATBiasZero: Check if connected - is true");
                    LogMessageToFile("ATBiasZero: Check if checked");
                    if (ATBiasZero.Checked)
                    {
                        LogMessageToFile("ATBiasZero: Check if checked - is true");
                        // ATBias Offset 0
                        statusmsgTxtBox.Text = "Setting ATemp Bias Offset to 0";
                        statusmsgTxtBox.Update();
                        try
                        {
                            LogMessageToFile("ATBiasZero: send e0# to myDewControllerPro3");
                            serialPort1.Write("e0#");
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("ATBiasZero: send e0# to myDewControllerPro3 failed");
                            shutdownport("Sending command to serial port failed");
                        }
                    }
                }
                else
                {
                    LogMessageToFile("ATBiasZero: Check if connected - is false");
                    statusmsgTxtBox.Text = "COM Port not set";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("ATBiasZero: Done is false");
            }

            LogMessageToFile("ATBiasZero: Finish");
        }

        private void ATBiasPlus1_CheckedChanged(object sender, EventArgs e)
        {
            LogMessageToFile("ATBiasPlus1: Start");
            LogMessageToFile("ATBiasPlus1: Check if connected");
            if (Done)
            {
                if (comconnected)
                {
                    LogMessageToFile("ATBiasPlus1: Check if connected - is true");
                    LogMessageToFile("ATBiasPlus1: Check if checked");
                    if (ATBiasPlus1.Checked)
                    {
                        LogMessageToFile("ATBiasPlus1: Check if checked - is true");
                        // ATBias Offset +1
                        statusmsgTxtBox.Text = "Setting ATemp Bias Offset to +1";
                        statusmsgTxtBox.Update();
                        try
                        {
                            LogMessageToFile("ATBiasPlus1: send e1# to myDewControllerPro3");
                            serialPort1.Write("e1#");
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("ATBiasPlus1: send e1# to myDewControllerPro3 failed");
                            shutdownport("Sending command to serial port failed");
                        }
                    }
                }
                else
                {
                    LogMessageToFile("ATBiasPlus1: Check if connected - is false");
                    statusmsgTxtBox.Text = "COM Port not set";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("ATBiasPlus1: Done is false");
            }

            LogMessageToFile("ATBiasPlus1: Finish");
        }

        private void ATBiasPlus2_CheckedChanged(object sender, EventArgs e)
        {
            LogMessageToFile("ATBiasPlus2: Start");
            LogMessageToFile("ATBiasPlus2: Check if connected");
            if (Done)
            {
                if (comconnected)
                {
                    LogMessageToFile("ATBiasPlus2: Check if connected - is true");
                    LogMessageToFile("ATBiasPlus2: Check if checked");
                    if (ATBiasPlus2.Checked)
                    {
                        LogMessageToFile("ATBiasPlus2: Check if checked - is true");
                        // ATBias Offset +2
                        statusmsgTxtBox.Text = "Setting ATemp Bias Offset to +2";
                        statusmsgTxtBox.Update();
                        try
                        {
                            LogMessageToFile("ATBiasPlus2: send e2# to myDewControllerPro3");
                            serialPort1.Write("e2#");
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("ATBiasPlus2: send e2# to myDewControllerPro3 failed");
                            shutdownport("Sending command to serial port failed");
                        }
                    }
                }
                else
                {
                    LogMessageToFile("ATBiasPlus2: Check if connected - is false");
                    statusmsgTxtBox.Text = "COM Port not set";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("ATBiasPlus2: Done is false");
            }
            LogMessageToFile("ATBiasPlus2: Finish");
        }

        private void ATBiasPlus3_CheckedChanged(object sender, EventArgs e)
        {
            LogMessageToFile("ATBiasPlus3: Start");
            LogMessageToFile("ATBiasPlus3: Check if connected");
            if (Done)
            {
                if (comconnected)
                {
                    LogMessageToFile("ATBiasPlus3: Check if connected - is true");
                    LogMessageToFile("ATBiasPlus3: Check if checked");
                    if (ATBiasPlus3.Checked)
                    {
                        LogMessageToFile("ATBiasPlus3: Check if checked - is true");
                        // ATBias Offset +3
                        statusmsgTxtBox.Text = "Setting ATemp Bias Offset to +3";
                        statusmsgTxtBox.Update();
                        try
                        {
                            LogMessageToFile("ATBiasPlus3: send e3# to myDewControllerPro3");
                            serialPort1.Write("e3#");
                        }
                        catch (Exception)
                        {
                            LogMessageToFile("ATBiasPlus3: send e3# to myDewControllerPro3 failed");
                            shutdownport("Sending command to serial port failed");
                        }
                    }
                }
                else
                {
                    LogMessageToFile("ATBiasPlus3: Check if connected - is false");
                    statusmsgTxtBox.Text = "COM Port not set";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                LogMessageToFile("ATBiasPlus3: Done is false");
            }
            LogMessageToFile("ATBiasPlus3: Finish");
        }

        // refresh the list of available com ports
        private void RefreshComPortBtn_Click(object sender, EventArgs e)
        {
            LogMessageToFile("RefreshComPortBtn: Start");
            LogMessageToFile("RefreshComPortBtn: Check if connected");
            if (!comconnected)
            {
                LogMessageToFile("RefreshComPortBtn: Check if connected - is true");
                // myserialPort = new SerialPort();        // create instance of serial port
                comconnected = false;
                soundenabled = false;
                Overridech1 = false;
                Overridech2 = false;
                OffsetVal = 0;
                ControllerVersionTxtBox.Clear();
                // Get a list of serial port names.
                this.comboBox1.Items.Clear();		// erase previous list
                this.comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                if (graphfrm != null)
                    graphfrm.clearpoints();
                controlsoff();
            }
            else
            {
                LogMessageToFile("RefreshComPortBtn: Check if connected - is false");
                MessageBox.Show("Comport is already connected. Please click Disconnect first, then click Refresh", "myDewControllerPro3", MessageBoxButtons.OK);
                statusmsgTxtBox.Text = "ComPort is connected - Disconnect and try again";
                statusmsgTxtBox.Update();
            }
            LogMessageToFile("RefreshComPortBtn: Finish");
        }

        // specify and select the logfile
        private void DataLogBtn_Click(object sender, EventArgs e)
        {
            LogMessageToFile("DataLogBtn_Click: Start");
            if (RefreshTimer.Enabled == true)
            {
                LogMessageToFile("DataLogBtn_Click: Refresh timer is enabled - returning");
                MessageBox.Show("Cannot change logfile when in automated mode.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "Cannot change filename - please stop automated mode first";
                statusmsgTxtBox.Update();
                return;
            }

            // this needs to create the file based on the pathname
            // first check that the directory has been set
            if (LogDirNametxtBox.Text == "")
            {
                MessageBox.Show("Please specify a directory.", "myDewControllerPro3", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                statusmsgTxtBox.Text = "A directory needs to be specified";
                statusmsgTxtBox.Update();
                LogMessageToFile("DataLogBtn_Click: Invoke SetDirectory");
                this.Invoke(new EventHandler(SetDirectoryBtn_Click));
            }

            // create the file based on the directory and the logfilename
            // filename is stored in 

            LogMessageToFile("DataLogBtn_Click: Create file");

            ofd.Filter = "CSV|*.csv";

            ofd.InitialDirectory = LogDirNametxtBox.Text;

            // create the file
            try
            {
                if (!File.Exists(LogDirNametxtBox.Text + "\\" + LogFileNameTxtBox.Text))
                {
                    FileStream fs = File.Create(LogDirNametxtBox.Text + "\\" + LogFileNameTxtBox.Text);
                    // MessageBox.Show("File created");
                    fs.Close();
                }
                else
                {
                    // MessageBox.Show("File Exists Already", "Error creating file");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error creating log file. Cannot Access Location: ", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "Exception Error creating log file";
                statusmsgTxtBox.Update();
            }

            LogMessageToFile("DataLogBtn_Click: Select File");
            ofd.Title = "Select File";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                LogDataChkBox.Enabled = true;
                LogDataChkBox.Checked = true;
                logfilename = ofd.FileName;
                LogFileNameTxtBox.Text = ofd.SafeFileName;
                // write header line to file
                // time, ambient_temp, rel_humidity, dewpoint, ch1temp, ch2temp, ch1pwr, ch2pwr, trackmode, 
                try
                {
                    File.AppendAllText(logfilename, "// myDewControllerPro3 DataLogfile" + Environment.NewLine);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error writing data to file: ", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    statusmsgTxtBox.Text = "Exception Error - could not write to file";
                    statusmsgTxtBox.Update();
                }
            }
            else
            {
                // do not log data if no file has been setup
                LogDataChkBox.Enabled = false;
                LogDataChkBox.Checked = false;
                LogDirNametxtBox.Clear();
                LogDirNametxtBox.Clear();
                logfilename = "";       // set filename to Null
            }
            LogMessageToFile("DataLogBtn_Click: Start");
        }

        // log the controllers data to the logfile
        private void logdatatofile()
        {
            // this appends the data from the controller to a selected csv file
            // LogDataChkBox must be true and enabled
            // LogFileName must be non Null
            // log format is CSV text file
            // time, ambient_temp, rel_humidity, dewpoint, ch1temp, ch2temp, ch3temp, ch1pwr, ch2pwr, ch3pwr, 
            // ch1offset, ch2offset, ch3offset, atbiasoffset, offsetval, ch3mode, trackmode

            string bufferdata;
            string tmpString;
            String TimeNow = DateTime.Now.TimeOfDay.ToString(); // get current time
            String myTime = "";
            for (int lpval = 0; lpval <= 7; lpval++)           // hh:mm:ss.nnnnn
            {
                myTime += TimeNow[lpval];
            }
            myTime += "";                                       // hh.mm.ss
            logtimestamp = myTime;
            bufferdata = logtimestamp + ",";
            tmpString = Convert.ToString(ambient, newCulture);
            if (tmpString == "")
                tmpString = "0.0";
            tmpString.Replace(',', '.');
            bufferdata = bufferdata + tmpString + ",";
            tmpString = Convert.ToString(humidity, newCulture);
            if (tmpString == "")
                tmpString = "0.0";
            tmpString.Replace(',', '.');
            bufferdata = bufferdata + tmpString + ",";
            tmpString = Convert.ToString(dewpoint, newCulture);
            if (tmpString == "")
                tmpString = "0.0";
            tmpString.Replace(',', '.');
            bufferdata = bufferdata + tmpString + ",";
            tmpString = Convert.ToString(ch1temp, newCulture);
            if (tmpString == "")
                tmpString = "0.0";
            tmpString.Replace(',', '.');
            bufferdata = bufferdata + tmpString + ",";
            tmpString = Convert.ToString(ch2temp, newCulture);
            if (tmpString == "")
                tmpString = "0.0";
            tmpString.Replace(',', '.');
            bufferdata = bufferdata + tmpString + ",";
            tmpString = Convert.ToString(ch3temp, newCulture);
            if (tmpString == "")
                tmpString = "0.0";
            tmpString.Replace(',', '.');
            bufferdata = bufferdata + tmpString + ",";
            tmpString = Convert.ToString(ch1pwr, newCulture);
            if (tmpString == "")
                tmpString = "0.0";
            tmpString.Replace(',', '.');
            bufferdata = bufferdata + tmpString + ",";
            tmpString = Convert.ToString(ch2pwr, newCulture);
            if (tmpString == "")
                tmpString = "0.0";
            tmpString.Replace(',', '.');
            bufferdata = bufferdata + tmpString + ",";
            tmpString = Convert.ToString(ch3pwr, newCulture);
            if (tmpString == "")
                tmpString = "0.0";
            tmpString.Replace(',', '.');
            bufferdata = bufferdata + tmpString + ",";
            tmpString = Convert.ToString(ch1tempoffsetval, newCulture);
            if (tmpString == "")
                tmpString = "0.0";
            tmpString.Replace(',', '.');
            bufferdata = bufferdata + tmpString + ",";
            tmpString = Convert.ToString(ch2tempoffsetval, newCulture);
            if (tmpString == "")
                tmpString = "0.0";
            tmpString.Replace(',', '.');
            bufferdata = bufferdata + tmpString + ",";
            tmpString = Convert.ToString(ch3tempoffsetval, newCulture);
            if (tmpString == "")
                tmpString = "0.0";
            tmpString.Replace(',', '.');
            bufferdata = bufferdata + tmpString + ",";
            tmpString = Convert.ToString(ATBiasVal, newCulture);
            if (tmpString == "")
                tmpString = "0";
            bufferdata = bufferdata + tmpString + ",";
            tmpString = Convert.ToString(OffsetVal, newCulture);
            if (tmpString == "")
                tmpString = "0";
            bufferdata = bufferdata + tmpString + ",";
            tmpString = Convert.ToString(ch3mode, newCulture);
            if (tmpString == "")
                tmpString = "0";
            bufferdata = bufferdata + tmpString + ",";
            // trackingmode = 1 = ambient, 2 = dew point, 3 = mid-point tracking mode
            if (trackingmode == 1)
                bufferdata = bufferdata + "A";
            else if (trackingmode == 2)
                bufferdata = bufferdata + "D";
            else if (trackingmode == 3)
                bufferdata = bufferdata + "M";
            bufferdata = bufferdata + ",";
            // add pcbtemp
            tmpString = Convert.ToString(boardtemp, newCulture);
            if (tmpString == "")
                tmpString = "0";
            bufferdata = bufferdata + tmpString + ",";
            bufferdata = bufferdata + Environment.NewLine;
            try
            {
                File.AppendAllText(logfilename, bufferdata);
            }
            catch (Exception)
            {
                MessageBox.Show("Error writing data to file: ", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // send the channel 1 temperature offset value to the controller
        private void ch1tempoffsetBtn_Click(object sender, EventArgs e)
        {
            LogMessageToFile("ch1tempoffsetBtn: Start");
            LogMessageToFile("ch1tempoffsetBtn: Check if connected");
            if (comconnected)
            {
                LogMessageToFile("ch1tempoffsetBtn: Check if connected - is true");
                if (ch1tempoffTxtBox.Text != "")
                {
                    // convert to float and send ch1tempoffset
                    string tcmd;
                    double tempval;

                    LogMessageToFile("ch1tempoffsetBtn: Attempt to convert = " + ch1tempoffTxtBox.Text);
                    try
                    {
                        LogMessageToFile("ch1tempoffsetBtn: Attempt to convert = " + ch1tempoffTxtBox.Text + " is ok");
                        tempval = Double.Parse(ch1tempoffTxtBox.Text, newCulture);
                        // bounds check the value for the offset
                        // rangecheck to +3.5 to -3.5
                        if (tempval < -3.5)
                            tempval = -3.5;
                        if (tempval > 3.5)
                            tempval = 3.5;
                        // write back bounds check value to offset text box
                        ch1tempoffTxtBox.Text = Convert.ToString(tempval, newCulture);
                        ch1tempoffTxtBox.Update();
                        tcmd = "[" + Convert.ToString(tempval, newCulture) + "#";
                        statusmsgTxtBox.Text = "Setting ch1temp Offset to " + Convert.ToString(tempval, newCulture);
                        statusmsgTxtBox.Update();
                        serialPort1.Write(tcmd);
                    }
                    catch (Exception)
                    {
                        LogMessageToFile("ch1tempoffsetBtn: Attempt to convert = " + ch1tempoffTxtBox.Text + " is Error");
                        statusmsgTxtBox.Text = "Conversion error for ch1temp offset value";
                        statusmsgTxtBox.Update();
                    }
                }
            }
            else
            {
                LogMessageToFile("ch1tempoffsetBtn: Check if connected - is false");
                statusmsgTxtBox.Text = "COM Port not set";
                statusmsgTxtBox.Update();
            }
            LogMessageToFile("ch1tempoffsetBtn: Finish");
        }

        // send the channel 2 temperature offset value to the controller
        private void ch2tempoffsetBtn_Click(object sender, EventArgs e)
        {
            LogMessageToFile("ch2tempoffsetBtn: Start");
            LogMessageToFile("ch2tempoffsetBtn: Check if connected");
            if (comconnected)
            {
                LogMessageToFile("ch2tempoffsetBtn: Check if connected - is true");
                if (ch2tempoffTxtBox.Text != "")
                {
                    // convert to float and send ch2tempoffset
                    string tcmd;
                    double tempval;
                    LogMessageToFile("ch2tempoffsetBtn: Attempt to convert = " + ch2tempoffTxtBox.Text);
                    try
                    {
                        LogMessageToFile("ch2tempoffsetBtn: Attempt to convert = " + ch2tempoffTxtBox.Text + " is ok");
                        tempval = Double.Parse(ch2tempoffTxtBox.Text, newCulture);
                        // bounds check the value for the offset
                        // rangecheck to +3.5 to -3.5
                        if (tempval < -3.5)
                            tempval = -3.5;
                        if (tempval > 3.5)
                            tempval = 3.5;
                        // write back bounds check value to offset text box
                        ch2tempoffTxtBox.Text = Convert.ToString(tempval, newCulture);
                        ch2tempoffTxtBox.Update();
                        tcmd = "]" + Convert.ToString(tempval, newCulture) + "#";
                        statusmsgTxtBox.Text = "Setting ch2temp Offset to " + Convert.ToString(tempval, newCulture);
                        statusmsgTxtBox.Update();
                        serialPort1.Write(tcmd);
                    }
                    catch (Exception)
                    {
                        LogMessageToFile("ch2tempoffsetBtn: Attempt to convert = " + ch2tempoffTxtBox.Text + " is Error");
                        statusmsgTxtBox.Text = "Conversion error for ch2temp offset value";
                        statusmsgTxtBox.Update();
                    }
                }
            }
            else
            {
                LogMessageToFile("ch2tempoffsetBtn: Check if connected - is false");
                statusmsgTxtBox.Text = "COM Port not set";
                statusmsgTxtBox.Update();
            }
            LogMessageToFile("ch2tempoffsetBtn: Finish");
        }

        private void ch3tempoffsetBtn_Click(object sender, EventArgs e)
        {
            LogMessageToFile("ch3tempoffsetBtn: Start");
            LogMessageToFile("ch3tempoffsetBtn: Check if connected");
            if (comconnected)
            {
                LogMessageToFile("ch3tempoffsetBtn: Check if connected - is true");
                if (ch3tempoffTxtBox.Text != "")
                {
                    // convert to float and send ch1tempoffset
                    string tcmd;
                    double tempval;

                    LogMessageToFile("ch3tempoffsetBtn: send S0#");

                    LogMessageToFile("ch3tempoffsetBtn: Attempt to convert = " + ch3tempoffTxtBox.Text);
                    try
                    {
                        LogMessageToFile("ch3tempoffsetBtn: Attempt to convert = " + ch3tempoffTxtBox.Text + " is ok");
                        tempval = Double.Parse(ch3tempoffTxtBox.Text, newCulture);
                        // bounds check the value for the offset
                        // rangecheck to +3.5 to -3.5
                        if (tempval < -3.5)
                            tempval = -3.5;
                        if (tempval > 3.5)
                            tempval = 3.5;
                        // write back bounds check value to offset text box
                        ch3tempoffTxtBox.Text = Convert.ToString(tempval, newCulture);
                        ch3tempoffTxtBox.Update();
                        tcmd = "%" + Convert.ToString(tempval, newCulture) + "#";
                        statusmsgTxtBox.Text = "Setting ch3temp Offset to " + Convert.ToString(tempval, newCulture);
                        statusmsgTxtBox.Update();
                        serialPort1.Write(tcmd);
                    }
                    catch (Exception)
                    {
                        LogMessageToFile("ch3tempoffsetBtn: Attempt to convert = " + ch3tempoffTxtBox.Text + " is Error");
                        statusmsgTxtBox.Text = "Conversion error for ch1temp offset value";
                        statusmsgTxtBox.Update();
                    }
                }
            }
            else
            {
                LogMessageToFile("ch3tempoffsetBtn: Check if connected - is false");
                statusmsgTxtBox.Text = "COM Port not set";
                statusmsgTxtBox.Update();
            }
            LogMessageToFile("ch3tempoffsetBtn: Finish");
        }

        // get the temperature offset values from the controller for channel 1 and channel 2 and channel 3 temperature probes
        private void getchoffsetBtn_Click(object sender, EventArgs e)
        {
            string tcmd;
            string ret;

            LogMessageToFile("getchoffsetBtn: Start");
            LogMessageToFile("getchoffsetBtn: Check if connected");
            if (comconnected)
            {
                LogMessageToFile("getchoffsetBtn: Check if connected - is true");
                if (commandinprogress == false)
                {
                    LogMessageToFile("getchoffsetBtn testConnection: send ?#");
                    // then test connection
                    tcmd = "?#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    // commandstring updates the value
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
            }
            LogMessageToFile("getchoffsetBtn: Finish");
        }

        // clear the temperature offset values for calibrating channel 1 and channel 2 temperature probes
        private void cleartempoffsetBtn_Click(object sender, EventArgs e)
        {
            LogMessageToFile("cleartempoffset: Start");
            LogMessageToFile("cleartempoffset: check connected");
            if (comconnected)
            {
                LogMessageToFile("cleartempoffset: connected");
                try
                {
                    LogMessageToFile("cleartempoffset: send &#");
                    serialPort1.Write("&#");
                    ch1tempoffTxtBox.Text = "0";
                    ch1tempoffsetval = 0;
                    ch2tempoffTxtBox.Text = "0";
                    ch2tempoffsetval = 0;
                    ch3tempoffTxtBox.Text = "0";
                    ch3tempoffsetval = 0;
                    Properties.Settings.Default.Save();
                    statusmsgTxtBox.Text = "ch1/ch2/ch3 temperature offset values set to 0 but not saved.";
                    statusmsgTxtBox.Update();
                }
                catch (Exception)
                {
                    LogMessageToFile("cleartempoffset: Command not sent. Serial port exception occurred.");
                    shutdownport("Sending command to serial port failed");
                }
            }
            else
            {
                LogMessageToFile("cleartempoffset: COM Port not set");
                statusmsgTxtBox.Text = "COM Port not set";
                statusmsgTxtBox.Update();
            }
            LogMessageToFile("cleartempoffset: Finish");
        }

        // set the directory for the logfile
        private void SetDirectoryBtn_Click(object sender, EventArgs e)
        {
            LogMessageToFile("SetDirectoryBtn: Start");
            LogMessageToFile("SetDirectoryBtn: Check if refresh timer is enabled");
            if (RefreshTimer.Enabled == true)  // do not change the directory if we are already datalogging
            {
                LogMessageToFile("SetDirectoryBtn: Refresh timer enabled so return");
                MessageBox.Show("Cannot change logfile when in automated mode.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                LogMessageToFile("SetDirectoryBtn: refresh timer not enabled");
                LogMessageToFile("SetDirectoryBtn: Display file browser dialog");
                FolderBrowserDialog fd2 = new FolderBrowserDialog();
                if (fd2.ShowDialog() == DialogResult.OK)
                {
                    LogMessageToFile("SetDirectoryBtn: file browser dialog ok");
                    LogDirNametxtBox.Text = fd2.SelectedPath;
                }
                else
                {
                    LogMessageToFile("SetDirectoryBtn: file browser dialog != ok");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error setting directory: ", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "Exception Error";
                statusmsgTxtBox.Update();
            }
            LogMessageToFile("SetDirectoryBtn: Finish");
        }

        private void LCDEnableChkBox_CheckedChanged(object sender, EventArgs e)
        {
            LogMessageToFile("LCDEnableChkBox: Start");
            if (comconnected)
            {
                LogMessageToFile("LCDEnableChkBox: com port connected");
                if (LCDEnableChkBox.Checked == true)
                {
                    LogMessageToFile("LCDEnableChkBox: send enable command }# ");
                    // then turn on LCD display
                    try
                    {
                        serialPort1.Write("}#");
                    }
                    catch (Exception)
                    {
                        LogMessageToFile("LCDEnableChkBox: Error writing to serial port }#");
                        shutdownport("Sending command to serial port failed");
                    }
                }
                else
                {
                    LogMessageToFile("LCDEnableChkBox: send disable command {# ");
                    // checkbox state is false
                    // then turn off the LCD display
                    try
                    {
                        serialPort1.Write("{#");
                    }
                    catch (Exception)
                    {
                        LogMessageToFile("LCDEnableChkBox: Error writing to serial port {#");
                        shutdownport("Sending command to serial port failed");
                    }
                }
            }
            else
            {
                statusmsgTxtBox.Text = "COM Port not set or not connected";
                statusmsgTxtBox.Update();
            }
            LogMessageToFile("LCDEnableChkBox: Finish");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogMessageToFile("ExitToolStripMenu: Start");
            if (serialPort1.IsOpen)    // com port is open, so tidy up
            {
                LogMessageToFile("ExitToolStripMenu: error - Serial Port is open");
                MessageBox.Show("Disconnect the Serial Port before exiting", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else                    // Port is closed, save settings and exit
            {
                LogMessageToFile("ExitToolStripMenu: exiting");
                Properties.Settings.Default.Save();
                LogMessageToFile("ExitToolStripMenu: Com port closed: exiting");
                LogMessageToFile("ExitToolStripMenu: Saving application settings");
                Properties.Settings.Default.Save();
                LogMessageToFile(cpystrbrown + myVersion);
                // all tidy up is done in form_closing
                this.Close();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogMessageToFile("aboutToolStripMenu: Start");
            MessageBox.Show("myDewControllerPro3\nVersion=" + myVersion + CopyrightStr + "\nFirmware Version: " + ArduinoFirmwareRev, "myDewControllerPro3", MessageBoxButtons.OK);
            LogMessageToFile("aboutToolStripMenu: Finish");
        }

        private void enableErrorLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logerrorfile = true;
            enableErrorLogFileToolStripMenuItem.Checked = true;
            disableErrorLogFileToolStripMenuItem.Checked = false;
            Properties.Settings.Default.loggingerrors = true;
            Properties.Settings.Default.Save();
        }

        private void disableErrorLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logerrorfile = false;
            enableErrorLogFileToolStripMenuItem.Checked = false;
            disableErrorLogFileToolStripMenuItem.Checked = true;
            Properties.Settings.Default.loggingerrors = false;
            Properties.Settings.Default.Save();
        }

        private void ch1tempoffTxtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                // enter key has been pressed, so validate entry
                String tmpStr;
                tmpStr = ch1tempoffTxtBox.Text;
                if (tmpStr.Contains(","))
                {
                    // replace any , with a .
                    tmpStr = tmpStr.Replace(',', '.');
                    ch1tempoffTxtBox.Text = tmpStr;
                    ch1tempoffTxtBox.Update();
                }
                LogMessageToFile("Ch1TempOffsetTxtBox: = " + ch1tempoffTxtBox.Text);
                try
                {
                    LogMessageToFile("ch1tempoffTxtBox_KeyPress Tempoffset value Before Parse = " + tmpStr);
                    double temp1 = Double.Parse(tmpStr, newCulture);
                    LogMessageToFile("ch1tempoffTxtBox_KeyPress Tempoffset value After Parse = " + Convert.ToString(temp1, newCulture));
                    ch1tempoffsetval = (float)temp1;
                    LogMessageToFile("ch1tempoffTxtBox_KeyPress Tempoffset value After Round = " + Convert.ToString(ch1tempoffsetval, newCulture));

                    // rangecheck to +3 to -3
                    if (ch1tempoffsetval < -3.0f)
                        ch1tempoffsetval = -3.0f;
                    if (ch1tempoffsetval > 3.0f)
                        ch1tempoffsetval = 3.0f;
                    ch1tempoffsetval = (float)Math.Round((Double)ch1tempoffsetval, 2);
                    tmpStr = Convert.ToString(ch1tempoffsetval, newCulture);
                    ch1tempoffTxtBox.Text = tmpStr;
                    ch1tempoffTxtBox.Update();
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();
                }
                catch (Exception)
                {
                    LogMessageToFile("ch1tempoffTxtBox_KeyPress TempOffset - Invalid format for Temp Offset. Reset to 0");
                    ch1tempoffsetval = 0.0f;
                    tmpStr = Convert.ToString(ch1tempoffsetval, newCulture);
                    ch1tempoffTxtBox.Text = tmpStr;
                    ch1tempoffTxtBox.Update();
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();
                    e.Handled = true;
                    return;
                }
                e.Handled = true;
                return;
            }
            // allow only numeric inout and + - and backspace (8)
            char ch = e.KeyChar;

            // only allow one .
            if (ch == 46 && ch1tempoffTxtBox.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            // only allow one ,
            if (ch == 44 && ch1tempoffTxtBox.Text.IndexOf(',') != -1)
            {
                e.Handled = true;
                return;
            }

            // only allow one '-'
            if (ch == 45 && ch1tempoffTxtBox.Text.IndexOf('-') != -1)
            {
                e.Handled = true;
                return;
            }

            // only allow one '+'
            if (ch == 43 && ch1tempoffTxtBox.Text.IndexOf('+') != -1)
            {
                e.Handled = true;
                return;
            }

            // handle digits, backspace, dot separator, + and - and command
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46 && ch != 43 && ch != 45 && ch != 44)
            {
                e.Handled = true;
            }
        }

        private void ch2tempoffTxtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                // enter key has been pressed, so validate entry
                String tmpStr;
                tmpStr = ch2tempoffTxtBox.Text;
                if (tmpStr.Contains(","))
                {
                    // replace any , with a .
                    tmpStr = tmpStr.Replace(',', '.');
                    ch2tempoffTxtBox.Text = tmpStr;
                    ch2tempoffTxtBox.Update();
                }
                LogMessageToFile("Ch2TempOffsetTxtBox: = " + ch2tempoffTxtBox.Text);
                try
                {
                    LogMessageToFile("ch2tempoffTxtBox_KeyPress Tempoffset value Before Parse = " + tmpStr);
                    double temp1 = Double.Parse(tmpStr, newCulture);
                    LogMessageToFile("ch2tempoffTxtBox_KeyPress Tempoffset value After Parse = " + Convert.ToString(temp1, newCulture));
                    ch2tempoffsetval = (float)temp1;
                    LogMessageToFile("ch2tempoffTxtBox_KeyPress Tempoffset value After Round = " + Convert.ToString(ch2tempoffsetval, newCulture));
                    // rangecheck to +3 to -3
                    if (ch2tempoffsetval < -3.0f)
                        ch2tempoffsetval = -3.0f;
                    if (ch2tempoffsetval > 3.0f)
                        ch2tempoffsetval = 3.0f;
                    ch2tempoffsetval = (float)Math.Round((Double)ch2tempoffsetval, 2);
                    tmpStr = Convert.ToString(ch2tempoffsetval, newCulture);
                    ch2tempoffTxtBox.Text = tmpStr;
                    ch2tempoffTxtBox.Update();
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();
                }
                catch (Exception)
                {
                    LogMessageToFile("ch2tempoffTxtBox_KeyPress TempOffset - Invalid format for Temp Offset. Reset to 0");
                    ch2tempoffsetval = 0.0f;
                    tmpStr = Convert.ToString(ch2tempoffsetval, newCulture);
                    ch2tempoffTxtBox.Text = tmpStr;
                    ch2tempoffTxtBox.Update();
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();
                    e.Handled = true;
                    return;
                }
                e.Handled = true;
                return;
            }

            // allow only numeric inout and + - and backspace (8)
            char ch = e.KeyChar;

            // only allow one .
            if (ch == 46 && ch2tempoffTxtBox.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            // only allow one ,
            if (ch == 44 && ch2tempoffTxtBox.Text.IndexOf(',') != -1)
            {
                e.Handled = true;
                return;
            }

            // only allow one '-'
            if (ch == 45 && ch2tempoffTxtBox.Text.IndexOf('-') != -1)
            {
                e.Handled = true;
                return;
            }

            // only allow one '+'
            if (ch == 43 && ch2tempoffTxtBox.Text.IndexOf('+') != -1)
            {
                e.Handled = true;
                return;
            }

            // handle digits, backspace, dot separator, + and - and command
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46 && ch != 43 && ch != 45 && ch != 44)
            {
                e.Handled = true;
            }
        }

        private void ch3tempoffTxtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                // enter key has been pressed, so validate entry
                String tmpStr;
                tmpStr = ch3tempoffTxtBox.Text;
                if (tmpStr.Contains(","))
                {
                    // replace any , with a .
                    tmpStr = tmpStr.Replace(',', '.');
                    ch3tempoffTxtBox.Text = tmpStr;
                    ch3tempoffTxtBox.Update();
                }
                LogMessageToFile("Ch3TempOffsetTxtBox: = " + ch3tempoffTxtBox.Text);
                try
                {
                    LogMessageToFile("ch3tempoffTxtBox_KeyPress Tempoffset value Before Parse = " + tmpStr);
                    double temp1 = Double.Parse(tmpStr, newCulture);
                    LogMessageToFile("ch3tempoffTxtBox_KeyPress Tempoffset value After Parse = " + Convert.ToString(temp1, newCulture));
                    ch3tempoffsetval = (float)temp1;
                    LogMessageToFile("ch3tempoffTxtBox_KeyPress Tempoffset value After Round = " + Convert.ToString(ch3tempoffsetval, newCulture));
                    // rangecheck to +3 to -3
                    if (ch3tempoffsetval < -3.0f)
                        ch3tempoffsetval = -3.0f;
                    if (ch3tempoffsetval > 3.0f)
                        ch3tempoffsetval = 3.0f;
                    ch3tempoffsetval = (float)Math.Round((Double)ch3tempoffsetval, 2);
                    tmpStr = Convert.ToString(ch3tempoffsetval, newCulture);
                    ch3tempoffTxtBox.Text = tmpStr;
                    ch3tempoffTxtBox.Update();
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();
                }
                catch (Exception)
                {
                    LogMessageToFile("ch3tempoffTxtBox_KeyPress TempOffset - Invalid format for Temp Offset. Reset to 0");
                    ch3tempoffsetval = 0.0f;
                    tmpStr = Convert.ToString(ch3tempoffsetval, newCulture);
                    ch3tempoffTxtBox.Text = tmpStr;
                    ch3tempoffTxtBox.Update();
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();
                    e.Handled = true;
                    return;
                }
                e.Handled = true;
                return;
            }

            // allow only numeric inout and + - and backspace (8)
            char ch = e.KeyChar;

            // only allow one .
            if (ch == 46 && ch3tempoffTxtBox.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            // only allow one ,
            if (ch == 44 && ch3tempoffTxtBox.Text.IndexOf(',') != -1)
            {
                e.Handled = true;
                return;
            }

            // only allow one '-'
            if (ch == 45 && ch3tempoffTxtBox.Text.IndexOf('-') != -1)
            {
                e.Handled = true;
                return;
            }

            // only allow one '+'
            if (ch == 43 && ch3tempoffTxtBox.Text.IndexOf('+') != -1)
            {
                e.Handled = true;
                return;
            }

            // handle digits, backspace, dot separator, + and - and command
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46 && ch != 43 && ch != 45 && ch != 44)
            {
                e.Handled = true;
            }
        }

        private void resetErrorLogPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // form has not been created, or was disposed when closed at startup
            // set up errorlog path form
            elogpathfrm = new ErrorLogPathName();

            Properties.Settings.Default.SubFormRunning = true;
            elogpathfrm.Show();
            while (Properties.Settings.Default.SubFormRunning == true)
                PauseForTime(2, 0);
        }

        private void myDewController_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.FormLocation = this.Location;
            Properties.Settings.Default.Save();

            LogMessageToFile("FormClosing: Start");

            // if the comport is opened
            if (serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response required
                    SystemSounds.Beep.Play();
                LogMessageToFile("FormClosing: myDewControllerPro3: error - Serial Port is open");
                MessageBox.Show("Disconnect the Serial Port before exiting", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Information);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
                e.Cancel = true;
            }
            else   // Serial port is disconnected
            {
                serialPort1.Dispose();
                RefreshTimer.Stop();
                RefreshTimer.Enabled = false;
                LogMessageToFile("FormClosing: Serial port not connected");
                LogMessageToFile("FormClosing: Save application settings");
                Properties.Settings.Default.Save();  // save the application settings
                LogMessageToFile("FormClosing: Stop timers");
                IdleTimer1.Stop();
                IdleTimer1.Enabled = false;          // stop the timers
                RefreshTimer.Stop();
                RefreshTimer.Enabled = false;
                LogMessageToFile("FormClosing: Finish");
                Application.Exit();
            }
        }

        private void forceExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result;

            LogMessageToFile("ForceExit: Start");

            if (soundenabled)               // beep to indicate response required
                SystemSounds.Beep.Play();
            LogMessageToFile("ForceExit: Display MessageBox");
            result = MessageBox.Show("Are you sure you want to force exit?", "myDewControllerPro3", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

            LogMessageToFile("ForceExit: Check MessageBox return value");
            if (result == DialogResult.Yes)
            {
                // force exit
                // flush serial buffers
                // close serial port etc
                // print message
                LogMessageToFile("ForceExit: MessageBox = exit");
                LogMessageToFile("ForceExit: Discard Serial Port Buffer");
                if (serialPort1.IsOpen)
                    serialPort1.DiscardInBuffer();     // clear buffer

                LogMessageToFile("ForceExit: Close and Dispose Serial Port");
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                    serialPort1.Dispose();
                }
                LogMessageToFile("ForceExit: Saving application settings");
                Properties.Settings.Default.Save();
                LogMessageToFile(cpystrbrown + myVersion);
                Application.Exit();
            }
            else if (result == DialogResult.No)
            {
                // do nothing
                // print message port not disconnected
                LogMessageToFile("ForceExit: MessageBox = Do not exit");
            }
            else if (result == DialogResult.Cancel)
            {
                // do nothing
                LogMessageToFile("ForceExit: MessageBox = Cancel");
            }
        }

        private void getControllerFirmwareVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tcmd;
            string ret;

            LogMessageToFile("getControllerFirmware: Start");

            // if the comport is NOT opened
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("Getcontrollerfirmware version - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port before testing", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "Controller is not connected.";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("Getcontrollerfirmware: send v#");
                    // then test connection
                    // then send version request
                    tcmd = "v#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    statusmsgTxtBox.Text = ret.Substring(1, ret.Length - 2);
                    statusmsgTxtBox.Update();
                    commandinprogress = false;
                    // ignore return value
                    // commandstring updates the value
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
            }
            LogMessageToFile("getControllerFiremware: Finish");
        }

        private void noToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noToolStripMenuItem.Checked = true;
            yesToolStripMenuItem.Checked = false;
            Properties.Settings.Default.WriteEEPROMonExit = false;
            Properties.Settings.Default.Save();
        }

        private void yesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            yesToolStripMenuItem.Checked = true;
            noToolStripMenuItem.Checked = false;
            Properties.Settings.Default.WriteEEPROMonExit = true;
            Properties.Settings.Default.Save();
        }

        private void celsiusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // send "c#" to the controller to use celsius
            // this only changes the display LCD
            // the values sent from the controller are always in C

            LogMessageToFile("celsiusToolStripMenuItem: Start");

            //Display mode is currently Fahrenheit, so set things to Celsius
            if (testmode)
            {
                DisplayMode = Celsius;
                celsiusToolStripMenuItem.Checked = true;
                fahrenheitToolStripMenuItem.Checked = false;
                // Ensure labels are set to C
                atlabel.Text = "C";
                ch1label.Text = "C";
                ch1label.Text = "C";
                ch2label.Text = "C";
                ch3label.Text = "C";
                atlabel.Update();
                ch1label.Update();
                ch1label.Update();
                ch2label.Update();
                ch3label.Update();
                // convert existing values in text boxes to C as they are in F
                try
                {
                    ambientTemperatureTxtBox.Text = ambient.ToString();
                    ambientTemperatureTxtBox.Update();
                    dewpointTxtBox.Text = dewpoint.ToString();
                    dewpointTxtBox.Update();
                    ch1tempTxtBox.Text = ch1temp.ToString();
                    ch1tempTxtBox.Update();
                    ch2tempTxtBox.Text = ch2temp.ToString();
                    ch2tempTxtBox.Update();
                    ch3tempTxtBox.Text = ch3temp.ToString();
                    ch3tempTxtBox.Update();
                }
                catch (FormatException)
                {
                    MessageBox.Show("Format Exception error converting value", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMessageToFile("celsiusToolStripMenuItem: FormatException error");
                    statusmsgTxtBox.Text = "Format Exception Error";
                    statusmsgTxtBox.Update();
                    if (soundenabled)               // beep to indicate response
                        SystemSounds.Exclamation.Play();
                    return;
                }
                catch (Exception)
                {
                    MessageBox.Show("Exception error converting value", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMessageToFile("celsiusToolStripMenuItem: Exception error");
                    statusmsgTxtBox.Text = "Exception Error";
                    statusmsgTxtBox.Update();
                    if (soundenabled)               // beep to indicate response
                        SystemSounds.Exclamation.Play();
                    return;
                }
                return;
            }

            // if the comport is NOT opened
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("celsiusToolStripMenuItem - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
                return;
            }
            else   // Serial port is connected
            {
                LogMessageToFile("celsiusToolStripMenuItem: send c#");
                // then test connection

                try
                {
                    sendcmd("c#");
                    PauseForTime(0, 100);   // wait 100ms
                }
                catch (Exception)
                {
                    if (soundenabled)               // beep to indicate response
                        SystemSounds.Beep.Play();
                    LogMessageToFile("celsiusToolStripMenuItem: Error writing to serial port v#");
                    shutdownport("Sending command to serial port failed");
                }
            }

            DisplayMode = Celsius;
            celsiusToolStripMenuItem.Checked = true;
            fahrenheitToolStripMenuItem.Checked = false;
            // Ensure labels are set to C
            atlabel.Text = "C";
            ch1label.Text = "C";
            ch1label.Text = "C";
            ch2label.Text = "C";
            ch3label.Text = "C";
            atlabel.Update();
            ch1label.Update();
            ch1label.Update();
            ch2label.Update();
            ch3label.Update();
            // convert existing values in text boxes to C as they are in F
            try
            {
                ambientTemperatureTxtBox.Text = ambient.ToString();
                ambientTemperatureTxtBox.Update();
                dewpointTxtBox.Text = dewpoint.ToString();
                dewpointTxtBox.Update();
                ch1tempTxtBox.Text = ch1temp.ToString();
                ch1tempTxtBox.Update();
                ch2tempTxtBox.Text = ch2temp.ToString();
                ch2tempTxtBox.Update();
                ch3tempTxtBox.Text = ch3temp.ToString();
                ch3tempTxtBox.Update();
            }
            catch (FormatException)
            {
                MessageBox.Show("Format Exception error converting value", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessageToFile("celsiusToolStripMenuItem: FormatException error");
                statusmsgTxtBox.Text = "Format Exception Error";
                statusmsgTxtBox.Update();
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Exclamation.Play();
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Exception error converting value", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessageToFile("celsiusToolStripMenuItem: Exception error");
                statusmsgTxtBox.Text = "Exception Error";
                statusmsgTxtBox.Update();
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Exclamation.Play();
                return;
            }
            LogMessageToFile("celsiusToolStripMenuItem: Finish");
        }

        private void fahrenheitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // send "f#" to the controller to use fahrenheit
            // this only changes the display LCD
            // the values sent from the controller are always in C

            double tempC, tempF;
            LogMessageToFile("fahrenheitToolStripMenuItem: Start");

            //Display mode is currently Celsius, so set things to Fahrenheit
            if (testmode)
            {
                DisplayMode = Fahrenheit;
                celsiusToolStripMenuItem.Checked = false;
                fahrenheitToolStripMenuItem.Checked = true;
                // Ensure labels are set to F
                atlabel.Text = "F";
                ch1label.Text = "F";
                ch1label.Text = "F";
                ch2label.Text = "F";
                ch3label.Text = "F";
                atlabel.Update();
                ch1label.Update();
                ch1label.Update();
                ch2label.Update();
                ch3label.Update();
                // convert existing values in text boxes to F as they are in C
                try
                {
                    tempC = ambient;
                    tempF = (tempC * 1.8) + 32;
                    tempF = Math.Round(tempF, 2);
                    ambientTemperatureTxtBox.Text = tempF.ToString();
                    ambientTemperatureTxtBox.Update();
                    tempC = dewpoint;
                    tempF = (tempC * 1.8) + 32;
                    tempF = Math.Round(tempF, 2);
                    dewpointTxtBox.Text = tempF.ToString();
                    dewpointTxtBox.Update();
                    tempC = ch1temp;
                    tempF = (tempC * 1.8) + 32;
                    tempF = Math.Round(tempF, 2);
                    ch1tempTxtBox.Text = tempF.ToString();
                    ch1tempTxtBox.Update();
                    tempC = ch2temp;
                    tempF = (tempC * 1.8) + 32;
                    tempF = Math.Round(tempF, 2);
                    ch2tempTxtBox.Text = tempF.ToString();
                    ch2tempTxtBox.Update();
                    tempC = ch3temp;
                    tempF = (tempC * 1.8) + 32;
                    tempF = Math.Round(tempF, 2);
                    ch3tempTxtBox.Text = tempF.ToString();
                    ch3tempTxtBox.Update();
                }
                catch (FormatException)
                {
                    MessageBox.Show("Format Exception error converting value", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMessageToFile("fahrenheitToolStripMenuItem: FormatException error");
                    statusmsgTxtBox.Text = "Format Exception Error";
                    statusmsgTxtBox.Update();
                    if (soundenabled)               // beep to indicate response
                        SystemSounds.Exclamation.Play();
                    return;
                }
                catch (Exception)
                {
                    MessageBox.Show("Exception error converting value", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMessageToFile("fahrenheitToolStripMenuItem: Exception error");
                    statusmsgTxtBox.Text = "Exception Error";
                    statusmsgTxtBox.Update();
                    if (soundenabled)               // beep to indicate response
                        SystemSounds.Exclamation.Play();
                    return;
                }
                return;
            }
            // if the comport is NOT opened
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("fahrenheitToolStripMenuItem - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
                return;
            }
            else   // Serial port is connected
            {
                LogMessageToFile("fahrenheitToolStripMenuItem: send f#");
                // then test connection

                try
                {
                    sendcmd("f#");
                    PauseForTime(0, 100);   // wait 100ms
                }
                catch (Exception)
                {
                    if (soundenabled)               // beep to indicate response
                        SystemSounds.Beep.Play();
                    LogMessageToFile("fahrenheitToolStripMenuItem: Error writing to serial port v#" + "");
                    shutdownport("Sending command to serial port failed");
                }
            }

            DisplayMode = Fahrenheit;
            celsiusToolStripMenuItem.Checked = false;
            fahrenheitToolStripMenuItem.Checked = true;
            // Ensure labels are set to F
            atlabel.Text = "F";
            ch1label.Text = "F";
            ch1label.Text = "F";
            ch2label.Text = "F";
            ch3label.Text = "F";
            atlabel.Update();
            ch1label.Update();
            ch1label.Update();
            ch2label.Update();
            ch3label.Update();
            // convert existing values in text boxes to F as they are in C
            try
            {
                tempC = ambient;
                tempF = (tempC * 1.8) + 32;
                tempF = Math.Round(tempF, 2);
                ambientTemperatureTxtBox.Text = tempF.ToString();
                ambientTemperatureTxtBox.Update();
                tempC = dewpoint;
                tempF = (tempC * 1.8) + 32;
                tempF = Math.Round(tempF, 2);
                dewpointTxtBox.Text = tempF.ToString();
                dewpointTxtBox.Update();
                tempC = ch1temp;
                tempF = (tempC * 1.8) + 32;
                tempF = Math.Round(tempF, 2);
                ch1tempTxtBox.Text = tempF.ToString();
                ch1tempTxtBox.Update();
                tempC = ch2temp;
                tempF = (tempC * 1.8) + 32;
                tempF = Math.Round(tempF, 2);
                ch2tempTxtBox.Text = tempF.ToString();
                ch2tempTxtBox.Update();
                tempC = ch3temp;
                tempF = (tempC * 1.8) + 32;
                tempF = Math.Round(tempF, 2);
                ch3tempTxtBox.Text = tempF.ToString();
                ch3tempTxtBox.Update();
            }
            catch (FormatException)
            {
                MessageBox.Show("Format Exception error converting value", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessageToFile("fahrenheitToolStripMenuItem: FormatException error");
                statusmsgTxtBox.Text = "Format Exception Error";
                statusmsgTxtBox.Update();
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Exclamation.Play();
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Exception error converting value", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessageToFile("fahrenheitToolStripMenuItem: Exception error");
                statusmsgTxtBox.Text = "Exception Error";
                statusmsgTxtBox.Update();
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Exclamation.Play();
                return;
            }
            LogMessageToFile("fahrenheitToolStripMenuItem: Finish");
        }

        private void getfirmwareversion_Click(object sender, EventArgs e)
        {
            string tcmd;
            string ret;

            // Get firmware version
            LogMessageToFile("getfirmwareversion: Start");

            // send =# to controller 
            // if the comport is NOT opened
            LogMessageToFile("getfirmwareversion_Click - check if serial port is open");
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("getfirmwareversion - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("getfirmwareversion: send v#");
                    statusmsgTxtBox.Text = "Sending v# to get firmware version";
                    statusmsgTxtBox.Update();
                    // then send version request
                    tcmd = "v#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    statusmsgTxtBox.Text = "Done: Version =" + ret;
                    statusmsgTxtBox.Update();
                    // ignore return value
                    // commandstring updates the value
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                LogMessageToFile("getfirmwareversion: Finish");
            }
        }

        private void relhumiditylabel_Click(object sender, EventArgs e)
        {
            string tcmd;
            string ret;

            // get the relative humidity
            LogMessageToFile("relhumiditylabel: Start");

            // send =# to myDewControllerPro 
            // if the comport is NOT opened
            LogMessageToFile("relhumiditylabel_Click - check if serial port is open");
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("relhumiditylabel - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("relhumiditylabel: send R#");
                    // then send request
                    tcmd = "R#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    // commandstring updates the value
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                LogMessageToFile("relhumiditylabel: Finish");
            }
        }

        private void dewpointlabel_Click(object sender, EventArgs e)
        {
            string tcmd;
            string ret;

            // get dewpoint value
            LogMessageToFile("dewpointlabel: Start");

            // send =# to myDewControllerPro 
            // if the comport is NOT opened
            LogMessageToFile("dewpointlabel - check if serial port is open");
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("dewpointlabel - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("dewpointlabel: send D#");
                    // then send request
                    tcmd = "D#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    // commandstring updates the value
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                LogMessageToFile("dewpointlabel: Finish");
            }
        }

        private void ambienttemplabel_Click(object sender, EventArgs e)
        {
            string tcmd;
            string ret;

            // get ambient temperature
            LogMessageToFile("ambienttemplabel: Start");

            // send A# to myDewControllerPro 
            // if the comport is NOT opened
            LogMessageToFile("ambienttemplabel - check if serial port is open");
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("ambienttemplabel - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("ambienttemplabel: send A#");
                    // then send request
                    tcmd = "A#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    // commandstring updates the value
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                LogMessageToFile("ambienttemplabel: Finish");
            }
        }

        private void ch1pwrLabel_Click(object sender, EventArgs e)
        {
            string tcmd;
            string ret;

            // get power levels for ch1/ch2/ch3 W#
            LogMessageToFile("ch1pwrLabel: Start");

            // send W# to myDewControllerPro 
            // if the comport is NOT opened
            LogMessageToFile("ch1pwrLabel - check if serial port is open");
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("ch1pwrLabel - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("ch1pwrLabel: send W#");
                    // then send request
                    tcmd = "W#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    // commandstring updates the value
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                LogMessageToFile("ch1pwrLabel: Finish");
            }
        }

        private void ch2pwrLabel_Click(object sender, EventArgs e)
        {
            string tcmd;
            string ret;

            // get power levels for ch1/ch2/ch3 W#
            LogMessageToFile("ch2pwrLabel: Start");

            // send W# to myDewControllerPro 
            // if the comport is NOT opened
            LogMessageToFile("ch2pwrLabel - check if serial port is open");
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("ch2pwrLabel - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("ch2pwrLabel: send W#");
                    // then send request
                    tcmd = "W#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    // commandstring updates the value
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                LogMessageToFile("ch2pwrLabel: Finish");
            }
        }

        private void ch3pwrlabel_Click(object sender, EventArgs e)
        {
            string tcmd;
            string ret;

            // get power levels for ch1/ch2/ch3 W#
            LogMessageToFile("ch3pwrLabel: Start");

            // send W# to myDewControllerPro 
            // if the comport is NOT opened
            LogMessageToFile("ch3pwrLabel - check if serial port is open");
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("ch3pwrLabel - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("ch3pwrLabel: send W#");
                    // then send request
                    tcmd = "W#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    // commandstring updates the value
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                LogMessageToFile("ch3pwrLabel: Finish");
            }
        }

        private void ch1offsetlabel_Click(object sender, EventArgs e)
        {
            string tcmd;
            string ret;

            // get ch1/ch2/ch3 offset values ?#
            LogMessageToFile("ch1offsetlabel: Start");

            // send ?# to myDewControllerPro 
            // if the comport is NOT opened
            LogMessageToFile("ch1offsetlabel - check if serial port is open");
            statusmsgTxtBox.Text = "Requesting ch1/ch2/ch3 offset values from myDewControllerPro3";
            statusmsgTxtBox.Update();
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("ch1offsetlabel - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("ch1offsetlabel: send ?#");
                    // then send request
                    tcmd = "?#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    // commandstring updates the value
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                LogMessageToFile("ch1offsetlabel: Finish");
            }
        }

        private void ch2offsetlabel_Click(object sender, EventArgs e)
        {
            string tcmd;
            string ret;

            // get ch1/ch2/ch3 offset values ?#
            LogMessageToFile("ch2offsetlabel: Start");

            // send ?# to myDewControllerPro 
            // if the comport is NOT opened

            LogMessageToFile("ch2offsetlabel - check if serial port is open");
            statusmsgTxtBox.Text = "Requesting ch1/ch2/ch3 offset values from myDewControllerPro3";
            statusmsgTxtBox.Update();
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("ch2offsetlabel - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("ch2offsetlabel: send ?#");
                    // then send request
                    tcmd = "?#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    // commandstring updates the value
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                LogMessageToFile("ch2offsetlabel: Finish");
            }
        }

        private void ch3offsetlabel_Click(object sender, EventArgs e)
        {
            string tcmd;
            string ret;

            // get ch1/ch2/ch3 offset values ?#
            LogMessageToFile("ch3offsetlabel: Start");

            // send ?# to myDewControllerPro 
            // if the comport is NOT opened

            LogMessageToFile("ch3offsetlabel - check if serial port is open");
            statusmsgTxtBox.Text = "Requesting ch1/ch2/ch3 offset values from myDewControllerPro3";
            statusmsgTxtBox.Update();
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("ch3offsetlabel - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("ch3offsetlabel: send ?#");
                    // then send request
                    tcmd = "?#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    // commandstring updates the value
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                LogMessageToFile("ch3offsetlabel: Finish");
            }
        }

        private void oFFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // send S0# - set shadow dew channel3 OFF
            string tcmd;
            string ret;

            LogMessageToFile("oFFToolStripMenuItem: Start");

            // send S0# to myDewControllerPro 
            // if the comport is NOT opened
            LogMessageToFile("oFFToolStripMenuItem - check if serial port is open");
            statusmsgTxtBox.Text = "Preparing to send S0$ to the myDewControllerPro3";
            statusmsgTxtBox.Update();
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("oFFToolStripMenuItem - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("oFFToolStripMenuItem: send S0#");
                    // then send request
                    tcmd = "S0#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    ch3mode = 0;
                    oFFToolStripMenuItem.Checked = true;
                    channel1ToolStripMenuItem.Checked = false;
                    channel2ToolStripMenuItem.Checked = false;
                    manualSettingToolStripMenuItem.Checked = false;
                    useTempProbe3ToolStripMenuItem.Checked = false;
                    ch3pwrTxtBox.Text = "0";
                    ch3pwrTxtBox.Update();
                    ch3tempTxtBox.Text = "0.0";
                    ch3tempTxtBox.Update();
                    ch3PwrTrackBar.Value = 0;
                    ch3PwrTrackBar.Update();
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
                LogMessageToFile("oFFToolStripMenuItem: Finish");
            }
        }

        private void channel1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // send S1# - set shadow dew channel3 to mirror dewchannel 1
            string tcmd;
            string ret;

            LogMessageToFile("channel1ToolStripMenuItem: Start");

            // send S1# to myDewControllerPro 
            // if the comport is NOT opened
            LogMessageToFile("channel1ToolStripMenuItem - check if serial port is open");
            statusmsgTxtBox.Text = "Preparing to send S1# to the myDewControllerPro3";
            statusmsgTxtBox.Update();
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("channel1ToolStripMenuItem - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("channel1ToolStripMenuItem: send S1#");
                    // then send request
                    tcmd = "S1#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    ch3mode = 1;
                    oFFToolStripMenuItem.Checked = false;
                    channel1ToolStripMenuItem.Checked = true;
                    channel2ToolStripMenuItem.Checked = false;
                    manualSettingToolStripMenuItem.Checked = false;
                    useTempProbe3ToolStripMenuItem.Checked = false;
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
                LogMessageToFile("channel1ToolStripMenuItem: Finish");
            }
        }

        private void channel2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // send S2# - set shadow dew channel3 to mirror dewchannel 2
            string tcmd;
            string ret;

            LogMessageToFile("channel2ToolStripMenuItem: Start");

            // send S2# to myDewControllerPro 
            // if the comport is NOT opened
            LogMessageToFile("channel2ToolStripMenuItem - check if serial port is open");
            statusmsgTxtBox.Text = "Preparing to send S2# to the myDewControllerPro3";
            statusmsgTxtBox.Update();
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("channel2ToolStripMenuItem - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("channel2ToolStripMenuItem: send S2#");
                    tcmd = "S2#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    ch3mode = 2;
                    oFFToolStripMenuItem.Checked = false;
                    channel1ToolStripMenuItem.Checked = false;
                    channel2ToolStripMenuItem.Checked = true;
                    manualSettingToolStripMenuItem.Checked = false;
                    useTempProbe3ToolStripMenuItem.Checked = false;
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
                LogMessageToFile("channel2ToolStripMenuItem: Finish");
            }
        }

        private void showCurrentShadowChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // updates the checks against the shadowchannel in the settings menu
            string tcmd;
            string ret;

            LogMessageToFile("showCurrentShadowChannelToolStripMenuItem: Start");

            // send E# to myDewControllerPro 
            // if the comport is NOT opened
            LogMessageToFile("showCurrentShadowChannelToolStripMenuItem - check if serial port is open");
            statusmsgTxtBox.Text = "Preparing to send E to the Controller";
            statusmsgTxtBox.Update();
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("showCurrentShadowChannelToolStripMenuItem - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("showCurrentShadowChannelToolStripMenuItem: send E#");
                    tcmd = "E#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    // commandstring updates the settings menu bar
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                LogMessageToFile("showCurrentShadowChannelToolStripMenuItem: Finish");
            }
        }

        private void manualSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // updates the checks against the shadowchannel in the settings menu
            string tcmd;
            string ret;

            LogMessageToFile("manualSettingToolStripMenuItem: Start");

            // send Gnumb# to myDewControllerPro 
            // if the comport is NOT opened
            LogMessageToFile("manualSettingToolStripMenuItem - check if serial port is open");
            statusmsgTxtBox.Text = "Preparing to send G to the Controller";
            statusmsgTxtBox.Update();
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("manualSettingToolStripMenuItem - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("manualSettingToolStripMenuItem: send E#");
                    // then send request
                    // get the new ch3pwrval setting from the rotary dial 
                    tcmd = "G" + ch3PwrTrackBar.Value.ToString() + "#";
                    ch3pwrTxtBox.Text = ch3PwrTrackBar.Value.ToString();
                    ch3pwrTxtBox.Update();

                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    oFFToolStripMenuItem.Checked = false;
                    channel1ToolStripMenuItem.Checked = false;
                    channel2ToolStripMenuItem.Checked = false;
                    manualSettingToolStripMenuItem.Checked = true;
                    useTempProbe3ToolStripMenuItem.Checked = false;
                    statusmsgTxtBox.Text = ret;
                    statusmsgTxtBox.Update();
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
                LogMessageToFile("manualSettingToolStripMenuItem: Finish");
            }
        }

        private void ch3pwrSetButton_Click(object sender, EventArgs e)
        {
            // send the value if connected, will set ch3 to manual mode
            // updates the checks against the shadowchannel in the settings menu
            string tcmd;
            string ret;

            LogMessageToFile("ch3pwrSetButton: Start");

            // send Gnumb# to myDewControllerPro 
            // if the comport is NOT opened
            LogMessageToFile("ch3pwrSetButton - check if serial port is open");
            statusmsgTxtBox.Text = "Preparing to send G to the Controller";
            statusmsgTxtBox.Update();
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("ch3pwrSetButton - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("ch3pwrSetButton: send G#");
                    // then send request

                    // replace with new mactrackbar
                    tcmd = "G" + ch3PwrTrackBar.Value.ToString() + "#";
                    ch3pwrTxtBox.Text = ch3PwrTrackBar.Value.ToString();
                    ch3pwrTxtBox.Update();
                    statusmsgTxtBox.Text = "Sending " + tcmd;
                    statusmsgTxtBox.Update();

                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    oFFToolStripMenuItem.Checked = false;
                    channel1ToolStripMenuItem.Checked = false;
                    channel2ToolStripMenuItem.Checked = false;
                    manualSettingToolStripMenuItem.Checked = true;
                    useTempProbe3ToolStripMenuItem.Checked = false;
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
                LogMessageToFile("ch3pwrSetButton: Finish");
            }
        }

        private void useTempProbe3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // inform controller to use temp probe 3 for dew channel 3
            // if there is no temp probe3, then it will default to OFF
            // send the value if connected, will set ch3 to manual mode
            // updates the checks against the shadowchannel in the settings menu
            // send S4# - set shadow dew channel3 to temp probe 3
            string tcmd;
            string ret;

            LogMessageToFile("useTempProbe3ToolStripMenuItem: Start");

            // send S4# to myDewControllerPro 
            // if the comport is NOT opened
            LogMessageToFile("useTempProbe3ToolStripMenuItem - check if serial port is open");
            statusmsgTxtBox.Text = "Preparing to send S2# to the Controller";
            statusmsgTxtBox.Update();
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("useTempProbe3ToolStripMenuItem - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("useTempProbe3ToolStripMenuItem: send S2#");
                    // then send request
                    tcmd = "S4#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    oFFToolStripMenuItem.Checked = false;
                    channel1ToolStripMenuItem.Checked = false;
                    channel2ToolStripMenuItem.Checked = false;
                    manualSettingToolStripMenuItem.Checked = false;
                    useTempProbe3ToolStripMenuItem.Checked = true;
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
                LogMessageToFile("useTempProbe3ToolStripMenuItem: Finish");
            }
        }

        private void secondsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // send the lcd display screen time to the controller
            // this is 2.5s, so send b2500#
            string tcmd;
            string ret;

            LogMessageToFile("secondsToolStripMenuItem: Start");

            // if the comport is NOT opened
            LogMessageToFile("secondsToolStripMenuItem - check if serial port is open");
            statusmsgTxtBox.Text = "Preparing to send b2500# to the Controller";
            statusmsgTxtBox.Update();
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("secondsToolStripMenuItem - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("secondsToolStripMenuItem: send b2500#");
                    // then send request
                    tcmd = "b2500#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    secondsToolStripMenuItem.Checked = true;
                    secondsToolStripMenuItem1.Checked = false;
                    secondsToolStripMenuItem2.Checked = false;
                    secondsToolStripMenuItem3.Checked = false;
                    secondsToolStripMenuItem4.Checked = false;
                    secondsToolStripMenuItem5.Checked = false;
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
                LogMessageToFile("secondsToolStripMenuItem: Finish");
            }
        }

        private void secondsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // send the lcd display screen time to the controller
            // this is 3.0s, so send b3000#
            string tcmd;
            string ret;

            LogMessageToFile("secondsToolStripMenuItem1: Start");

            // if the comport is NOT opened
            LogMessageToFile("secondsToolStripMenuItem1 - check if serial port is open");
            statusmsgTxtBox.Text = "Preparing to send b3.0# to the Controller";
            statusmsgTxtBox.Update();
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("secondsToolStripMenuItem1 - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("secondsToolStripMenuItem1: send b3000#");
                    // then send request
                    tcmd = "b3000#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    secondsToolStripMenuItem.Checked = false;
                    secondsToolStripMenuItem1.Checked = true;
                    secondsToolStripMenuItem2.Checked = false;
                    secondsToolStripMenuItem3.Checked = false;
                    secondsToolStripMenuItem4.Checked = false;
                    secondsToolStripMenuItem5.Checked = false;
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
                LogMessageToFile("secondsToolStripMenuItem1: Finish");
            }
        }

        private void secondsToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            // send the lcd display screen time to the controller
            // this is 3.5s, so send b3500#
            string tcmd;
            string ret;

            LogMessageToFile("secondsToolStripMenuItem2: Start");

            // if the comport is NOT opened
            LogMessageToFile("secondsToolStripMenuItem2 - check if serial port is open");
            statusmsgTxtBox.Text = "Preparing to send b3500# to the Controller";
            statusmsgTxtBox.Update();
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("secondsToolStripMenuItem2 - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("secondsToolStripMenuItem2: send b3500#");
                    // then send request
                    tcmd = "b3500#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    secondsToolStripMenuItem.Checked = false;
                    secondsToolStripMenuItem1.Checked = false;
                    secondsToolStripMenuItem2.Checked = true;
                    secondsToolStripMenuItem3.Checked = false;
                    secondsToolStripMenuItem4.Checked = false;
                    secondsToolStripMenuItem5.Checked = false;
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
                LogMessageToFile("secondsToolStripMenuItem2: Finish");
            }
        }

        private void secondsToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            // send the lcd display screen time to the controller
            // this is 4.0s, so send b4000#
            string tcmd;
            string ret;

            LogMessageToFile("secondsToolStripMenuItem3: Start");

            // if the comport is NOT opened
            LogMessageToFile("secondsToolStripMenuItem3 - check if serial port is open");
            statusmsgTxtBox.Text = "Preparing to send b4.0# to the Controller";
            statusmsgTxtBox.Update();
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("secondsToolStripMenuItem3 - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("secondsToolStripMenuItem3: send b4000#");
                    // then send request
                    tcmd = "b4000#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    secondsToolStripMenuItem.Checked = false;
                    secondsToolStripMenuItem1.Checked = false;
                    secondsToolStripMenuItem2.Checked = false;
                    secondsToolStripMenuItem3.Checked = true;
                    secondsToolStripMenuItem4.Checked = false;
                    secondsToolStripMenuItem5.Checked = false;
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
                LogMessageToFile("secondsToolStripMenuItem3: Finish");
            }
        }

        private void secondsToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            // send the lcd display screen time to the controller
            // this is 4.5s, so send b4500#
            string tcmd;
            string ret;

            LogMessageToFile("secondsToolStripMenuItem4: Start");

            // if the comport is NOT opened
            LogMessageToFile("secondsToolStripMenuItem4 - check if serial port is open");
            statusmsgTxtBox.Text = "Preparing to send b4500# to the Controller";
            statusmsgTxtBox.Update();
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("secondsToolStripMenuItem4 - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("secondsToolStripMenuItem4: send b4500#");
                    // then send request
                    tcmd = "b4500#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    secondsToolStripMenuItem.Checked = false;
                    secondsToolStripMenuItem1.Checked = false;
                    secondsToolStripMenuItem2.Checked = false;
                    secondsToolStripMenuItem3.Checked = false;
                    secondsToolStripMenuItem4.Checked = true;
                    secondsToolStripMenuItem5.Checked = false;
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
                LogMessageToFile("secondsToolStripMenuItem4: Finish");
            }
        }

        private void secondsToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            // send the lcd display screen time to the controller
            // this is 5.0s, so send b5000#
            string tcmd;
            string ret;

            LogMessageToFile("secondsToolStripMenuItem5: Start");

            // if the comport is NOT opened
            LogMessageToFile("secondsToolStripMenuItem5 - check if serial port is open");
            statusmsgTxtBox.Text = "Preparing to send b5.0# to the Controller";
            statusmsgTxtBox.Update();
            if (!serialPort1.IsOpen)
            {
                if (soundenabled)               // beep to indicate response
                    SystemSounds.Beep.Play();
                LogMessageToFile("secondsToolStripMenuItem5 - Serial Port is not open");
                MessageBox.Show("Connect the Serial Port first.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
            }
            else   // Serial port is connected
            {
                if (commandinprogress == false)
                {
                    LogMessageToFile("secondsToolStripMenuItem5: send b5000#");
                    // then send request
                    tcmd = "b5000#";
                    commandinprogress = true;
                    ret = CommandString(tcmd);
                    commandinprogress = false;
                    // ignore return value
                    secondsToolStripMenuItem.Checked = false;
                    secondsToolStripMenuItem1.Checked = false;
                    secondsToolStripMenuItem2.Checked = false;
                    secondsToolStripMenuItem3.Checked = false;
                    secondsToolStripMenuItem4.Checked = false;
                    secondsToolStripMenuItem5.Checked = true;
                }
                else
                {
                    statusmsgTxtBox.Text = "Command is already in progress - please wait";
                    statusmsgTxtBox.Update();
                }
                statusmsgTxtBox.Text = "";
                statusmsgTxtBox.Update();
                LogMessageToFile("secondsToolStripMenuItem5: Finish");
            }
        }

        private void ch3PwrTrackBar_Scroll(object sender, EventArgs e)
        {
            statusmsgTxtBox.Text = ch3PwrTrackBar.Value.ToString();
            statusmsgTxtBox.Update();
        }

        private void ch3PwrTrackBar_ValueChanged(object sender, EventArgs e)
        {
            int tbval;
            tbval = ch3PwrTrackBar.Value;

            ch3pwrTxtBox.Text = ch3PwrTrackBar.Value.ToString();
            ch3pwrTxtBox.Update();
            LogMessageToFile("ch3PwrTrackBar_ValueChanged Pwr set to " + tbval.ToString());
        }

        private void fantemponTxtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                // enter key has been pressed, so validate entry
                try
                {
                    LogMessageToFile("fantemponTxtBox value Before Parse = " + fantemponTxtBox.Text.ToString());
                    fantempsetting = Convert.ToInt32(fantemponTxtBox.Text);
                    LogMessageToFile("fantemponTxtBox value After Parse = " + fantempsetting.ToString());
                }
                catch (Exception)
                {
                    fantempsetting = 0;
                    LogMessageToFile("fantemponTxtBox_KeyPress Tvalue not correct syntax: Setting value to 0.0");
                    statusmsgTxtBox.Text = "Exception Error";
                    statusmsgTxtBox.Update();
                }
                // rangecheck to +3 to -3
                if (fantempsetting < 0)
                    fantempsetting = 0;
                if (fantempsetting > 70)
                    fantempsetting = 70;
                fantemponTxtBox.Text = fantempsetting.ToString();
                Properties.Settings.Default.Save();
                e.Handled = true;
                return;
            }

            // allow only numeric inout and backspace (8)
            char ch = e.KeyChar;

            // handle digits, backspace
            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        // get main PCB temperature, firmware v018 and above
        private void getPCBTempBtn_Click(object sender, EventArgs e)
        {
            string tcmd;
            string ret;

            LogMessageToFile("getPCBTempBtn: Start");
            LogMessageToFile("getPCBTempBtn: Check if connected");
            if (comconnected)
            {
                if (ControllerRev > 318)
                {
                    LogMessageToFile("getPCBTempBtn: Check if connected - is true");
                    if (commandinprogress == false)
                    {
                        LogMessageToFile("getPCBTempBtn: send K#");
                        // then test connection
                        tcmd = "K#";
                        commandinprogress = true;
                        ret = CommandString(tcmd);
                        commandinprogress = false;
                        // ignore return value
                        // commandstring updates the value
                    }
                    else
                    {
                        statusmsgTxtBox.Text = "Command is already in progress - please wait";
                        statusmsgTxtBox.Update();
                    }
                }
                else
                {
                    MessageBox.Show("The controller firmware does not support this function. Please upgrade to firmware v3.18 or greater", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LogMessageToFile("getPCBTempBtn: Finish");
        }

        private void setFanTempOnBtn_Click(object sender, EventArgs e)
        {
            string tcmd;
            string ret;

            LogMessageToFile("setFanTempOnBtn: Start");
            LogMessageToFile("setFanTempOnBtn: Check if connected");
            if (comconnected)
            {
                if (ControllerRev > 318)
                {
                    LogMessageToFile("setFanTempOnBtn: Check if connected - is true");
                    if (commandinprogress == false)
                    {
                        LogMessageToFile("setFanTempOnBtn: send J#");
                        // then test connection
                        tcmd = "I" + fantemponTxtBox.Text + "#";
                        commandinprogress = true;
                        ret = CommandString(tcmd);
                        commandinprogress = false;
                        // ignore return value
                        // commandstring updates the value
                    }
                    else
                    {
                        statusmsgTxtBox.Text = "Command is already in progress - please wait";
                        statusmsgTxtBox.Update();
                    }
                }
                else
                {
                    MessageBox.Show("The controller firmware does not support this function. Please upgrade to firmware v3.18 or greater", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LogMessageToFile("setFanTempOnBtn: Finish");
        }

        private void getFanTempBtn_Click(object sender, EventArgs e)
        {
            // J
            string tcmd;
            string ret;

            LogMessageToFile("getFanTempBtn: Start");
            LogMessageToFile("getFanTempBtn: Check if connected");
            if (comconnected)
            {
                if (ControllerRev > 318)
                {
                    LogMessageToFile("getFanTempBtn: Check if connected - is true");
                    if (commandinprogress == false)
                    {
                        LogMessageToFile("setFanTempOnBtn: send J#");
                        // then test connection
                        tcmd = "J#";
                        commandinprogress = true;
                        ret = CommandString(tcmd);
                        commandinprogress = false;
                        // ignore return value
                        // commandstring updates the value
                    }
                    else
                    {
                        statusmsgTxtBox.Text = "Command is already in progress - please wait";
                        statusmsgTxtBox.Update();
                    }
                }
                else
                {
                    MessageBox.Show("The controller firmware does not support this function. Please upgrade to firmware v3.18 or greater", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LogMessageToFile("getFanTempBtn: Finish");
        }

        private void setEEPROMDefaultSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tcmd;
            string ret;

            LogMessageToFile("setEEPROMDefaultSettings: Start");
            LogMessageToFile("setEEPROMDefaultSettings: Check if connected");
            if (comconnected)
            {
                if (ControllerRev > 318)
                {
                    LogMessageToFile("setEEPROMDefaultSettings: Check if connected - is true");
                    if (commandinprogress == false)
                    {
                        LogMessageToFile("setEEPROMDefaultSettings: send r#");
                        // then test connection
                        tcmd = "r#";
                        commandinprogress = true;
                        ret = CommandString(tcmd);
                        commandinprogress = false;
                        // ignore return value
                        // commandstring updates the value
                    }
                    else
                    {
                        statusmsgTxtBox.Text = "Command is already in progress - please wait";
                        statusmsgTxtBox.Update();
                    }
                }
                else
                {
                    MessageBox.Show("The controller firmware does not support this function. Please upgrade to firmware v3.18 or greater", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LogMessageToFile("setEEPROMDefaultSettings: Finish");
        }

        private void statusmsgTxtBox_TextChanged(object sender, EventArgs e)
        {
            // when text is written, then timer starts
            ClearStatusMsgsTimer.Enabled = true;
            ClearStatusMsgsTimer.Interval = 3000;           // 3s
            ClearStatusMsgsTimer.Start();
        }

        private void ClearStatusMsgsTimer_Tick(object sender, EventArgs e)
        {
            // after 3s delay this is triggered
            statusmsgTxtBox.Enabled = false;
            statusmsgTxtBox.Clear();
            statusmsgTxtBox.Update();
            statusmsgTxtBox.Enabled = true;
            ClearStatusMsgsTimer.Interval = 3000;           // 3s
            ClearStatusMsgsTimer.Stop();
            ClearStatusMsgsTimer.Enabled = false;
        }

        private void GraphPic_Click(object sender, EventArgs e)
        {
            LogMessageToFile("GraphLogging: START ================================");
            if (graphfrm == null)
            {
                graphfrm = new GraphForm();
                graphfrm.Show();
            }
            else
            {
                try
                {
                    graphfrm.Show();
                }
                catch (ObjectDisposedException)
                {
                    // object was disposed because window form was closed with X
                    // recreate object
                    graphfrm = new GraphForm();
                    graphfrm.Show();
                }
            }
            LogMessageToFile("GraphLogging: START ================================");
        }

        private void testModeStandaloneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result;

            result = MessageBox.Show("Are you sure you want to do this?", "myDewControllerPro3", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

            LogMessageToFile("TestModeStandalone: Check MessageBox return value");
            if (result == DialogResult.Yes)
            {
                LogMessageToFile("TestModeStandalone: MessageBox = Yes");
                testmode = true;
            }
            else if (result == DialogResult.No)
            {
                // do nothing
                // print message port not disconnected
                LogMessageToFile("TestModeStandalone: MessageBox = No");
                testmode = false;
                controlsoff();
                comconnected = false;
            }
            else if (result == DialogResult.Cancel)
            {
                // do nothing
                LogMessageToFile("TestModeStandalone: MessageBox = Cancel");
            }
        }

        private void ComPortSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ComPortBaudRate = ComPortSpeed.GetItemText(ComPortSpeed.SelectedItem);
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        private void setFanTempOffBtn_Click(object sender, EventArgs e)
        {
            string tcmd;
            string ret;

            LogMessageToFile("setFanTempOffBtn: Start");
            LogMessageToFile("setFanTempOffBtn: Check if connected");
            if (comconnected)
            {
                if (ControllerRev > 330)
                {
                    LogMessageToFile("setFanTempOffBtn: Check if connected - is true");
                    if (commandinprogress == false)
                    {
                        LogMessageToFile("setFanTempOffBtn: send J#");
                        // then test connection
                        tcmd = "M" + fantempoffTxtBox.Text + "#";
                        commandinprogress = true;
                        ret = CommandString(tcmd);
                        commandinprogress = false;
                        // ignore return value
                        // commandstring updates the value
                    }
                    else
                    {
                        statusmsgTxtBox.Text = "Command is already in progress - please wait";
                        statusmsgTxtBox.Update();
                    }
                }
                else
                {
                    MessageBox.Show("The controller firmware does not support this function. Please upgrade to firmware v3.18 or greater", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LogMessageToFile("setFanTempOffBtn: Finish");
        }

        private void getFanTempOffBtn_Click(object sender, EventArgs e)
        {
            // L
            string tcmd;
            string ret;

            LogMessageToFile("getFanTempOffBtn: Start");
            LogMessageToFile("getFanTempOffBtn: Check if connected");
            if (comconnected)
            {
                if (ControllerRev > 330)
                {
                    LogMessageToFile("getFanTempOffBtn: Check if connected - is true");
                    if (commandinprogress == false)
                    {
                        LogMessageToFile("getFanTempOffBtn: send L#");
                        // then test connection
                        tcmd = "L#";
                        commandinprogress = true;
                        ret = CommandString(tcmd);
                        commandinprogress = false;
                        // ignore return value
                        // commandstring updates the value
                    }
                    else
                    {
                        statusmsgTxtBox.Text = "Command is already in progress - please wait";
                        statusmsgTxtBox.Update();
                    }
                }
                else
                {
                    MessageBox.Show("The controller firmware does not support this function. Please upgrade to firmware v3.18 or greater", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LogMessageToFile("getFanTempOffBtn: Finish");
        }

        private void reseterrorlogpathBtn_Click(object sender, EventArgs e)
        {
            // form has not been created, or was disposed when closed at startup
            // set up errorlog path form
            elogpathfrm = new ErrorLogPathName();

            Properties.Settings.Default.SubFormRunning = true;
            elogpathfrm.Show();
            while (Properties.Settings.Default.SubFormRunning == true)
                PauseForTime(2, 0);
        }

        private void resetControllertodefaultsBtn_Click(object sender, EventArgs e)
        {
            string tcmd;
            string ret;

            LogMessageToFile("resetControllertodefaults: Start");
            LogMessageToFile("resetControllertodefaults: Check if connected");
            if (comconnected)
            {
                if (ControllerRev > 318)
                {
                    LogMessageToFile("resetControllertodefaults: Check if connected - is true");
                    if (commandinprogress == false)
                    {
                        LogMessageToFile("resetControllertodefaults: send r#");
                        // then test connection
                        tcmd = "r#";
                        commandinprogress = true;
                        ret = CommandString(tcmd);
                        commandinprogress = false;
                        // ignore return value
                        // commandstring updates the value
                    }
                    else
                    {
                        statusmsgTxtBox.Text = "Command is already in progress - please wait";
                        statusmsgTxtBox.Update();
                    }
                }
                else
                {
                    MessageBox.Show("The controller firmware does not support this function. Please upgrade to firmware v3.18 or greater", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LogMessageToFile("resetControllertodefaults: Finish");
        }

        private void tesmodestandaloneBtn_Click(object sender, EventArgs e)
        {
            DialogResult result;

            result = MessageBox.Show("Are you sure you want to do this?", "myDewControllerPro3", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

            LogMessageToFile("tesmodestandalone: Check MessageBox return value");
            if (result == DialogResult.Yes)
            {
                LogMessageToFile("tesmodestandalone: MessageBox = Yes");
                testmode = true;
            }
            else if (result == DialogResult.No)
            {
                // do nothing
                // print message port not disconnected
                LogMessageToFile("tesmodestandalone: MessageBox = No");
                testmode = false;
                controlsoff();
                comconnected = false;
            }
            else if (result == DialogResult.Cancel)
            {
                // do nothing
                LogMessageToFile("tesmodestandalone: MessageBox = Cancel");
            }
        }

        private void VideosComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void playBtn_Click(object sender, EventArgs e)
        {
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
        }
    }
}
