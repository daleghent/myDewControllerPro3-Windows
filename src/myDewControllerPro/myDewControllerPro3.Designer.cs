namespace myDewControllerPro3
{
    partial class myDewController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(myDewController));
            this.ExitBtn = new System.Windows.Forms.Button();
            this.comportConnectBtn = new System.Windows.Forms.Button();
            this.ch2tempTxtBox = new System.Windows.Forms.TextBox();
            this.dewpointTxtBox = new System.Windows.Forms.TextBox();
            this.labelnewTxtBox = new System.Windows.Forms.TextBox();
            this.labelnewBtn = new System.Windows.Forms.Button();
            this.ch1100Btn = new System.Windows.Forms.Button();
            this.ch2100Btn = new System.Windows.Forms.Button();
            this.off100Btn = new System.Windows.Forms.Button();
            this.writeEEPROMBtn = new System.Windows.Forms.Button();
            this.soundChkBox = new System.Windows.Forms.CheckBox();
            this.offsetminusBtn = new System.Windows.Forms.Button();
            this.offsetzeroBtn = new System.Windows.Forms.Button();
            this.offsetplusBtn = new System.Windows.Forms.Button();
            this.trackingmodeGroupBox = new System.Windows.Forms.GroupBox();
            this.TrackModeMidPoint = new System.Windows.Forms.RadioButton();
            this.TrackModeDewPoint = new System.Windows.Forms.RadioButton();
            this.TrackModeAmbient = new System.Windows.Forms.RadioButton();
            this.fanspeedGroupBox = new System.Windows.Forms.GroupBox();
            this.FanSpeed100 = new System.Windows.Forms.RadioButton();
            this.FanSpeed75 = new System.Windows.Forms.RadioButton();
            this.FanSpeed50 = new System.Windows.Forms.RadioButton();
            this.FanSpeedZero = new System.Windows.Forms.RadioButton();
            this.atbiasoffsetGroupBox = new System.Windows.Forms.GroupBox();
            this.ATBiasPlus3 = new System.Windows.Forms.RadioButton();
            this.ATBiasPlus2 = new System.Windows.Forms.RadioButton();
            this.ATBiasPlus1 = new System.Windows.Forms.RadioButton();
            this.ATBiasZero = new System.Windows.Forms.RadioButton();
            this.ATBiasMinus1 = new System.Windows.Forms.RadioButton();
            this.ATBiasMinus2 = new System.Windows.Forms.RadioButton();
            this.ATBiasMinus3 = new System.Windows.Forms.RadioButton();
            this.ATBiasMinus4 = new System.Windows.Forms.RadioButton();
            this.automateChkBox = new System.Windows.Forms.CheckBox();
            this.statusmsgTxtBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.refreshrateGroupBox = new System.Windows.Forms.GroupBox();
            this.RefreshRate5m = new System.Windows.Forms.RadioButton();
            this.RefreshRate1m = new System.Windows.Forms.RadioButton();
            this.RefreshRate30s = new System.Windows.Forms.RadioButton();
            this.RefreshRate10s = new System.Windows.Forms.RadioButton();
            this.IdleTimer1 = new System.Windows.Forms.Timer(this.components);
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.disconnectComPortBtn = new System.Windows.Forms.Button();
            this.RefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.RefreshComPortBtn = new System.Windows.Forms.Button();
            this.ControllerVersionTxtBox = new System.Windows.Forms.TextBox();
            this.DataLogBtn = new System.Windows.Forms.Button();
            this.LogDataChkBox = new System.Windows.Forms.CheckBox();
            this.ch1tempoffsetBtn = new System.Windows.Forms.Button();
            this.ch2tempoffsetBtn = new System.Windows.Forms.Button();
            this.getchoffsetBtn = new System.Windows.Forms.Button();
            this.cleartempoffsetBtn = new System.Windows.Forms.Button();
            this.SetDirectoryBtn = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.LogFileNameTxtBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.LCDEnableChkBox = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableErrorLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableErrorLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forceExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getControllerFirmwareVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lCDSCreenDisplayTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.secondsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.secondsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.secondsToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.secondsToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.secondsToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.secondsToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.resetErrorLogPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shadowDewChannelSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oFFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.channel1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.channel2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useTempProbe3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showCurrentShadowChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.temperatureModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.celsiusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fahrenheitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.writeEEROMOnExiitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setEEPROMDefaultSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testModeStandaloneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getfirmwareversion = new System.Windows.Forms.Label();
            this.ch3pwrSetButton = new System.Windows.Forms.Button();
            this.ch3tempTxtBox = new System.Windows.Forms.TextBox();
            this.ch3tempoffsetBtn = new System.Windows.Forms.Button();
            this.ch3PwrTrackBar = new System.Windows.Forms.TrackBar();
            this.getPCBTempBtn = new System.Windows.Forms.Button();
            this.setFanTempOnBtn = new System.Windows.Forms.Button();
            this.pcbtempTxtBox = new System.Windows.Forms.TextBox();
            this.getFanTempBtn = new System.Windows.Forms.Button();
            this.ClearStatusMsgsTimer = new System.Windows.Forms.Timer(this.components);
            this.GraphPic = new System.Windows.Forms.PictureBox();
            this.ch3tempoffTxtBox = new System.Windows.Forms.TextBox();
            this.ch2tempoffTxtBox = new System.Windows.Forms.TextBox();
            this.ch1tempoffTxtBox = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.LogDirNametxtBox = new System.Windows.Forms.TextBox();
            this.PCBFanControlsGrpBox = new System.Windows.Forms.GroupBox();
            this.getFanTempOffBtn = new System.Windows.Forms.Button();
            this.setFanTempOffBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.fantempoffTxtBox = new System.Windows.Forms.TextBox();
            this.fantemponTxtBox = new System.Windows.Forms.TextBox();
            this.trackingModeOffsetGroupBox = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ComPortSpeed = new System.Windows.Forms.ComboBox();
            this.mytabControl = new System.Windows.Forms.TabControl();
            this.tabTemperature = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.manualupdateBtn = new System.Windows.Forms.Button();
            this.TemperatureGroupBox = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ch3label = new System.Windows.Forms.Label();
            this.ch3templabel = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.dplabel = new System.Windows.Forms.Label();
            this.ch2label = new System.Windows.Forms.Label();
            this.ch1label = new System.Windows.Forms.Label();
            this.atlabel = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.ch2templabel = new System.Windows.Forms.Label();
            this.ch1tempTxtBox = new System.Windows.Forms.TextBox();
            this.ch1templabel = new System.Windows.Forms.Label();
            this.ambientTemperatureTxtBox = new System.Windows.Forms.TextBox();
            this.relativeHumidityTxtBox = new System.Windows.Forms.TextBox();
            this.label48 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.OffsetsGroupBox = new System.Windows.Forms.GroupBox();
            this.label50 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.tabPower = new System.Windows.Forms.TabPage();
            this.PowerGroupBox = new System.Windows.Forms.GroupBox();
            this.ch3pwrTxtBox = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.ch3pwrlabel = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.ch2pwrTxtBox = new System.Windows.Forms.TextBox();
            this.ch1pwrTxtBox = new System.Windows.Forms.TextBox();
            this.ch2pwrlabel = new System.Windows.Forms.Label();
            this.ch1pwrlabel = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tabTracking = new System.Windows.Forms.TabPage();
            this.tabLogging = new System.Windows.Forms.TabPage();
            this.reseterrorlogpathBtn = new System.Windows.Forms.Button();
            this.tabSerialPort = new System.Windows.Forms.TabPage();
            this.tabFanAndPCB = new System.Windows.Forms.TabPage();
            this.tabMisc = new System.Windows.Forms.TabPage();
            this.tesmodestandaloneBtn = new System.Windows.Forms.Button();
            this.resetControllertodefaultsBtn = new System.Windows.Forms.Button();
            this.writeEEPROMonExitChkBox = new System.Windows.Forms.CheckBox();
            this.trackingmodeGroupBox.SuspendLayout();
            this.fanspeedGroupBox.SuspendLayout();
            this.atbiasoffsetGroupBox.SuspendLayout();
            this.refreshrateGroupBox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ch3PwrTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GraphPic)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.PCBFanControlsGrpBox.SuspendLayout();
            this.trackingModeOffsetGroupBox.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.mytabControl.SuspendLayout();
            this.tabTemperature.SuspendLayout();
            this.TemperatureGroupBox.SuspendLayout();
            this.OffsetsGroupBox.SuspendLayout();
            this.tabPower.SuspendLayout();
            this.PowerGroupBox.SuspendLayout();
            this.tabTracking.SuspendLayout();
            this.tabLogging.SuspendLayout();
            this.tabSerialPort.SuspendLayout();
            this.tabFanAndPCB.SuspendLayout();
            this.tabMisc.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExitBtn
            // 
            this.ExitBtn.Location = new System.Drawing.Point(524, 503);
            this.ExitBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ExitBtn.Name = "ExitBtn";
            this.ExitBtn.Size = new System.Drawing.Size(109, 38);
            this.ExitBtn.TabIndex = 0;
            this.ExitBtn.Text = "Exit";
            this.ExitBtn.UseVisualStyleBackColor = true;
            this.ExitBtn.Click += new System.EventHandler(this.ExitBtn_Click);
            // 
            // comportConnectBtn
            // 
            this.comportConnectBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comportConnectBtn.Location = new System.Drawing.Point(6, 98);
            this.comportConnectBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comportConnectBtn.Name = "comportConnectBtn";
            this.comportConnectBtn.Size = new System.Drawing.Size(107, 34);
            this.comportConnectBtn.TabIndex = 6;
            this.comportConnectBtn.Text = "Connect";
            this.comportConnectBtn.UseVisualStyleBackColor = true;
            this.comportConnectBtn.Click += new System.EventHandler(this.comportConnectBtn_Click);
            // 
            // ch2tempTxtBox
            // 
            this.ch2tempTxtBox.Location = new System.Drawing.Point(177, 118);
            this.ch2tempTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ch2tempTxtBox.Name = "ch2tempTxtBox";
            this.ch2tempTxtBox.ReadOnly = true;
            this.ch2tempTxtBox.Size = new System.Drawing.Size(56, 26);
            this.ch2tempTxtBox.TabIndex = 14;
            // 
            // dewpointTxtBox
            // 
            this.dewpointTxtBox.Location = new System.Drawing.Point(177, 178);
            this.dewpointTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dewpointTxtBox.Name = "dewpointTxtBox";
            this.dewpointTxtBox.ReadOnly = true;
            this.dewpointTxtBox.Size = new System.Drawing.Size(56, 26);
            this.dewpointTxtBox.TabIndex = 16;
            // 
            // labelnewTxtBox
            // 
            this.labelnewTxtBox.Location = new System.Drawing.Point(10, 230);
            this.labelnewTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelnewTxtBox.MaxLength = 8;
            this.labelnewTxtBox.Name = "labelnewTxtBox";
            this.labelnewTxtBox.Size = new System.Drawing.Size(191, 26);
            this.labelnewTxtBox.TabIndex = 21;
            // 
            // labelnewBtn
            // 
            this.labelnewBtn.Location = new System.Drawing.Point(206, 230);
            this.labelnewBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelnewBtn.Name = "labelnewBtn";
            this.labelnewBtn.Size = new System.Drawing.Size(125, 30);
            this.labelnewBtn.TabIndex = 22;
            this.labelnewBtn.Text = "Reset Labels";
            this.labelnewBtn.UseVisualStyleBackColor = true;
            this.labelnewBtn.Click += new System.EventHandler(this.labelnewBtn_Click);
            // 
            // ch1100Btn
            // 
            this.ch1100Btn.Location = new System.Drawing.Point(255, 34);
            this.ch1100Btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ch1100Btn.Name = "ch1100Btn";
            this.ch1100Btn.Size = new System.Drawing.Size(101, 29);
            this.ch1100Btn.TabIndex = 23;
            this.ch1100Btn.Text = "Ch1 100%";
            this.ch1100Btn.UseVisualStyleBackColor = true;
            this.ch1100Btn.Click += new System.EventHandler(this.ch1100Btn_Click);
            // 
            // ch2100Btn
            // 
            this.ch2100Btn.Location = new System.Drawing.Point(255, 68);
            this.ch2100Btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ch2100Btn.Name = "ch2100Btn";
            this.ch2100Btn.Size = new System.Drawing.Size(101, 29);
            this.ch2100Btn.TabIndex = 24;
            this.ch2100Btn.Text = "Ch2 100%";
            this.ch2100Btn.UseVisualStyleBackColor = true;
            this.ch2100Btn.Click += new System.EventHandler(this.ch2100Btn_Click);
            // 
            // off100Btn
            // 
            this.off100Btn.Location = new System.Drawing.Point(366, 34);
            this.off100Btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.off100Btn.Name = "off100Btn";
            this.off100Btn.Size = new System.Drawing.Size(101, 29);
            this.off100Btn.TabIndex = 25;
            this.off100Btn.Text = "100% OFF";
            this.off100Btn.UseVisualStyleBackColor = true;
            this.off100Btn.Click += new System.EventHandler(this.off100Btn_Click);
            // 
            // writeEEPROMBtn
            // 
            this.writeEEPROMBtn.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.writeEEPROMBtn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.writeEEPROMBtn.FlatAppearance.BorderSize = 2;
            this.writeEEPROMBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.writeEEPROMBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.writeEEPROMBtn.Location = new System.Drawing.Point(10, 10);
            this.writeEEPROMBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.writeEEPROMBtn.Name = "writeEEPROMBtn";
            this.writeEEPROMBtn.Size = new System.Drawing.Size(300, 40);
            this.writeEEPROMBtn.TabIndex = 26;
            this.writeEEPROMBtn.Text = "Write Current Settings to EEPROM";
            this.writeEEPROMBtn.UseVisualStyleBackColor = false;
            this.writeEEPROMBtn.Click += new System.EventHandler(this.writeEEPROMBtn_Click);
            // 
            // soundChkBox
            // 
            this.soundChkBox.AutoSize = true;
            this.soundChkBox.Location = new System.Drawing.Point(140, 114);
            this.soundChkBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.soundChkBox.Name = "soundChkBox";
            this.soundChkBox.Size = new System.Drawing.Size(82, 24);
            this.soundChkBox.TabIndex = 27;
            this.soundChkBox.Text = "Sound";
            this.soundChkBox.UseVisualStyleBackColor = true;
            this.soundChkBox.CheckedChanged += new System.EventHandler(this.soundBtn_CheckedChanged);
            // 
            // offsetminusBtn
            // 
            this.offsetminusBtn.Location = new System.Drawing.Point(11, 31);
            this.offsetminusBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.offsetminusBtn.Name = "offsetminusBtn";
            this.offsetminusBtn.Size = new System.Drawing.Size(68, 38);
            this.offsetminusBtn.TabIndex = 28;
            this.offsetminusBtn.Text = "<";
            this.offsetminusBtn.UseVisualStyleBackColor = true;
            this.offsetminusBtn.Click += new System.EventHandler(this.offsetminusBtn_Click);
            // 
            // offsetzeroBtn
            // 
            this.offsetzeroBtn.Location = new System.Drawing.Point(101, 31);
            this.offsetzeroBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.offsetzeroBtn.Name = "offsetzeroBtn";
            this.offsetzeroBtn.Size = new System.Drawing.Size(68, 38);
            this.offsetzeroBtn.TabIndex = 29;
            this.offsetzeroBtn.Text = "0";
            this.offsetzeroBtn.UseVisualStyleBackColor = true;
            this.offsetzeroBtn.Click += new System.EventHandler(this.offsetzeroBtn_Click);
            // 
            // offsetplusBtn
            // 
            this.offsetplusBtn.Location = new System.Drawing.Point(191, 31);
            this.offsetplusBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.offsetplusBtn.Name = "offsetplusBtn";
            this.offsetplusBtn.Size = new System.Drawing.Size(68, 38);
            this.offsetplusBtn.TabIndex = 30;
            this.offsetplusBtn.Text = ">";
            this.offsetplusBtn.UseVisualStyleBackColor = true;
            this.offsetplusBtn.Click += new System.EventHandler(this.offsetplusBtn_Click);
            // 
            // trackingmodeGroupBox
            // 
            this.trackingmodeGroupBox.Controls.Add(this.TrackModeMidPoint);
            this.trackingmodeGroupBox.Controls.Add(this.TrackModeDewPoint);
            this.trackingmodeGroupBox.Controls.Add(this.TrackModeAmbient);
            this.trackingmodeGroupBox.Location = new System.Drawing.Point(5, 5);
            this.trackingmodeGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackingmodeGroupBox.Name = "trackingmodeGroupBox";
            this.trackingmodeGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackingmodeGroupBox.Size = new System.Drawing.Size(141, 131);
            this.trackingmodeGroupBox.TabIndex = 33;
            this.trackingmodeGroupBox.TabStop = false;
            this.trackingmodeGroupBox.Text = "Mode";
            // 
            // TrackModeMidPoint
            // 
            this.TrackModeMidPoint.AutoSize = true;
            this.TrackModeMidPoint.Location = new System.Drawing.Point(10, 90);
            this.TrackModeMidPoint.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TrackModeMidPoint.Name = "TrackModeMidPoint";
            this.TrackModeMidPoint.Size = new System.Drawing.Size(99, 24);
            this.TrackModeMidPoint.TabIndex = 2;
            this.TrackModeMidPoint.TabStop = true;
            this.TrackModeMidPoint.Text = "Mid Point";
            this.TrackModeMidPoint.UseVisualStyleBackColor = true;
            this.TrackModeMidPoint.CheckedChanged += new System.EventHandler(this.TrackModeMidPoint_CheckedChanged);
            // 
            // TrackModeDewPoint
            // 
            this.TrackModeDewPoint.AutoSize = true;
            this.TrackModeDewPoint.Location = new System.Drawing.Point(10, 60);
            this.TrackModeDewPoint.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TrackModeDewPoint.Name = "TrackModeDewPoint";
            this.TrackModeDewPoint.Size = new System.Drawing.Size(106, 24);
            this.TrackModeDewPoint.TabIndex = 1;
            this.TrackModeDewPoint.TabStop = true;
            this.TrackModeDewPoint.Text = "Dew Point";
            this.TrackModeDewPoint.UseVisualStyleBackColor = true;
            this.TrackModeDewPoint.CheckedChanged += new System.EventHandler(this.TrackModeDewPoint_CheckedChanged);
            // 
            // TrackModeAmbient
            // 
            this.TrackModeAmbient.AutoSize = true;
            this.TrackModeAmbient.Location = new System.Drawing.Point(10, 30);
            this.TrackModeAmbient.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TrackModeAmbient.Name = "TrackModeAmbient";
            this.TrackModeAmbient.Size = new System.Drawing.Size(93, 24);
            this.TrackModeAmbient.TabIndex = 0;
            this.TrackModeAmbient.TabStop = true;
            this.TrackModeAmbient.Text = "Ambient";
            this.TrackModeAmbient.UseVisualStyleBackColor = true;
            this.TrackModeAmbient.CheckedChanged += new System.EventHandler(this.TrackModeAmbient_CheckedChanged);
            // 
            // fanspeedGroupBox
            // 
            this.fanspeedGroupBox.Controls.Add(this.FanSpeed100);
            this.fanspeedGroupBox.Controls.Add(this.FanSpeed75);
            this.fanspeedGroupBox.Controls.Add(this.FanSpeed50);
            this.fanspeedGroupBox.Controls.Add(this.FanSpeedZero);
            this.fanspeedGroupBox.Location = new System.Drawing.Point(240, 10);
            this.fanspeedGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.fanspeedGroupBox.Name = "fanspeedGroupBox";
            this.fanspeedGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.fanspeedGroupBox.Size = new System.Drawing.Size(126, 170);
            this.fanspeedGroupBox.TabIndex = 34;
            this.fanspeedGroupBox.TabStop = false;
            this.fanspeedGroupBox.Text = "Fan Speed";
            // 
            // FanSpeed100
            // 
            this.FanSpeed100.AutoSize = true;
            this.FanSpeed100.Location = new System.Drawing.Point(10, 134);
            this.FanSpeed100.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FanSpeed100.Name = "FanSpeed100";
            this.FanSpeed100.Size = new System.Drawing.Size(75, 24);
            this.FanSpeed100.TabIndex = 3;
            this.FanSpeed100.TabStop = true;
            this.FanSpeed100.Text = "100%";
            this.FanSpeed100.UseVisualStyleBackColor = true;
            this.FanSpeed100.CheckedChanged += new System.EventHandler(this.FanSpeed100_CheckedChanged);
            // 
            // FanSpeed75
            // 
            this.FanSpeed75.AutoSize = true;
            this.FanSpeed75.Location = new System.Drawing.Point(10, 99);
            this.FanSpeed75.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FanSpeed75.Name = "FanSpeed75";
            this.FanSpeed75.Size = new System.Drawing.Size(66, 24);
            this.FanSpeed75.TabIndex = 2;
            this.FanSpeed75.TabStop = true;
            this.FanSpeed75.Text = "75%";
            this.FanSpeed75.UseVisualStyleBackColor = true;
            this.FanSpeed75.CheckedChanged += new System.EventHandler(this.FanSpeed75_CheckedChanged);
            // 
            // FanSpeed50
            // 
            this.FanSpeed50.AutoSize = true;
            this.FanSpeed50.Location = new System.Drawing.Point(10, 62);
            this.FanSpeed50.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FanSpeed50.Name = "FanSpeed50";
            this.FanSpeed50.Size = new System.Drawing.Size(66, 24);
            this.FanSpeed50.TabIndex = 1;
            this.FanSpeed50.TabStop = true;
            this.FanSpeed50.Text = "50%";
            this.FanSpeed50.UseVisualStyleBackColor = true;
            this.FanSpeed50.CheckedChanged += new System.EventHandler(this.FanSpeed50_CheckedChanged);
            // 
            // FanSpeedZero
            // 
            this.FanSpeedZero.AutoSize = true;
            this.FanSpeedZero.Location = new System.Drawing.Point(10, 28);
            this.FanSpeedZero.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FanSpeedZero.Name = "FanSpeedZero";
            this.FanSpeedZero.Size = new System.Drawing.Size(43, 24);
            this.FanSpeedZero.TabIndex = 0;
            this.FanSpeedZero.TabStop = true;
            this.FanSpeedZero.Text = "0";
            this.FanSpeedZero.UseVisualStyleBackColor = true;
            this.FanSpeedZero.CheckedChanged += new System.EventHandler(this.FanSpeedZero_CheckedChanged);
            // 
            // atbiasoffsetGroupBox
            // 
            this.atbiasoffsetGroupBox.Controls.Add(this.ATBiasPlus3);
            this.atbiasoffsetGroupBox.Controls.Add(this.ATBiasPlus2);
            this.atbiasoffsetGroupBox.Controls.Add(this.ATBiasPlus1);
            this.atbiasoffsetGroupBox.Controls.Add(this.ATBiasZero);
            this.atbiasoffsetGroupBox.Controls.Add(this.ATBiasMinus1);
            this.atbiasoffsetGroupBox.Controls.Add(this.ATBiasMinus2);
            this.atbiasoffsetGroupBox.Controls.Add(this.ATBiasMinus3);
            this.atbiasoffsetGroupBox.Controls.Add(this.ATBiasMinus4);
            this.atbiasoffsetGroupBox.Location = new System.Drawing.Point(355, 5);
            this.atbiasoffsetGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.atbiasoffsetGroupBox.Name = "atbiasoffsetGroupBox";
            this.atbiasoffsetGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.atbiasoffsetGroupBox.Size = new System.Drawing.Size(136, 250);
            this.atbiasoffsetGroupBox.TabIndex = 35;
            this.atbiasoffsetGroupBox.TabStop = false;
            this.atbiasoffsetGroupBox.Text = "AT Bias Offset";
            // 
            // ATBiasPlus3
            // 
            this.ATBiasPlus3.AutoSize = true;
            this.ATBiasPlus3.Location = new System.Drawing.Point(11, 294);
            this.ATBiasPlus3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ATBiasPlus3.Name = "ATBiasPlus3";
            this.ATBiasPlus3.Size = new System.Drawing.Size(52, 24);
            this.ATBiasPlus3.TabIndex = 7;
            this.ATBiasPlus3.TabStop = true;
            this.ATBiasPlus3.Text = "+3";
            this.ATBiasPlus3.UseVisualStyleBackColor = true;
            this.ATBiasPlus3.CheckedChanged += new System.EventHandler(this.ATBiasPlus3_CheckedChanged);
            // 
            // ATBiasPlus2
            // 
            this.ATBiasPlus2.AutoSize = true;
            this.ATBiasPlus2.Location = new System.Drawing.Point(11, 215);
            this.ATBiasPlus2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ATBiasPlus2.Name = "ATBiasPlus2";
            this.ATBiasPlus2.Size = new System.Drawing.Size(52, 24);
            this.ATBiasPlus2.TabIndex = 6;
            this.ATBiasPlus2.TabStop = true;
            this.ATBiasPlus2.Text = "+2";
            this.ATBiasPlus2.UseVisualStyleBackColor = true;
            this.ATBiasPlus2.CheckedChanged += new System.EventHandler(this.ATBiasPlus2_CheckedChanged);
            // 
            // ATBiasPlus1
            // 
            this.ATBiasPlus1.AutoSize = true;
            this.ATBiasPlus1.Location = new System.Drawing.Point(11, 184);
            this.ATBiasPlus1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ATBiasPlus1.Name = "ATBiasPlus1";
            this.ATBiasPlus1.Size = new System.Drawing.Size(52, 24);
            this.ATBiasPlus1.TabIndex = 5;
            this.ATBiasPlus1.TabStop = true;
            this.ATBiasPlus1.Text = "+1";
            this.ATBiasPlus1.UseVisualStyleBackColor = true;
            this.ATBiasPlus1.CheckedChanged += new System.EventHandler(this.ATBiasPlus1_CheckedChanged);
            // 
            // ATBiasZero
            // 
            this.ATBiasZero.AutoSize = true;
            this.ATBiasZero.Location = new System.Drawing.Point(11, 153);
            this.ATBiasZero.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ATBiasZero.Name = "ATBiasZero";
            this.ATBiasZero.Size = new System.Drawing.Size(43, 24);
            this.ATBiasZero.TabIndex = 4;
            this.ATBiasZero.TabStop = true;
            this.ATBiasZero.Text = "0";
            this.ATBiasZero.UseVisualStyleBackColor = true;
            this.ATBiasZero.CheckedChanged += new System.EventHandler(this.ATBiasZero_CheckedChanged);
            // 
            // ATBiasMinus1
            // 
            this.ATBiasMinus1.AutoSize = true;
            this.ATBiasMinus1.Location = new System.Drawing.Point(11, 122);
            this.ATBiasMinus1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ATBiasMinus1.Name = "ATBiasMinus1";
            this.ATBiasMinus1.Size = new System.Drawing.Size(48, 24);
            this.ATBiasMinus1.TabIndex = 3;
            this.ATBiasMinus1.TabStop = true;
            this.ATBiasMinus1.Text = "-1";
            this.ATBiasMinus1.UseVisualStyleBackColor = true;
            this.ATBiasMinus1.CheckedChanged += new System.EventHandler(this.ATBiasMinus1_CheckedChanged);
            // 
            // ATBiasMinus2
            // 
            this.ATBiasMinus2.AutoSize = true;
            this.ATBiasMinus2.Location = new System.Drawing.Point(11, 91);
            this.ATBiasMinus2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ATBiasMinus2.Name = "ATBiasMinus2";
            this.ATBiasMinus2.Size = new System.Drawing.Size(48, 24);
            this.ATBiasMinus2.TabIndex = 2;
            this.ATBiasMinus2.TabStop = true;
            this.ATBiasMinus2.Text = "-2";
            this.ATBiasMinus2.UseVisualStyleBackColor = true;
            this.ATBiasMinus2.CheckedChanged += new System.EventHandler(this.ATBiasMinus2_CheckedChanged);
            // 
            // ATBiasMinus3
            // 
            this.ATBiasMinus3.AutoSize = true;
            this.ATBiasMinus3.Location = new System.Drawing.Point(11, 59);
            this.ATBiasMinus3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ATBiasMinus3.Name = "ATBiasMinus3";
            this.ATBiasMinus3.Size = new System.Drawing.Size(48, 24);
            this.ATBiasMinus3.TabIndex = 1;
            this.ATBiasMinus3.TabStop = true;
            this.ATBiasMinus3.Text = "-3";
            this.ATBiasMinus3.UseVisualStyleBackColor = true;
            this.ATBiasMinus3.CheckedChanged += new System.EventHandler(this.ATBiasMinus3_CheckedChanged);
            // 
            // ATBiasMinus4
            // 
            this.ATBiasMinus4.AutoSize = true;
            this.ATBiasMinus4.Location = new System.Drawing.Point(11, 28);
            this.ATBiasMinus4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ATBiasMinus4.Name = "ATBiasMinus4";
            this.ATBiasMinus4.Size = new System.Drawing.Size(48, 24);
            this.ATBiasMinus4.TabIndex = 0;
            this.ATBiasMinus4.TabStop = true;
            this.ATBiasMinus4.Text = "-4";
            this.ATBiasMinus4.UseVisualStyleBackColor = true;
            this.ATBiasMinus4.CheckedChanged += new System.EventHandler(this.ATBiasMinus4_CheckedChanged);
            // 
            // automateChkBox
            // 
            this.automateChkBox.AutoSize = true;
            this.automateChkBox.Location = new System.Drawing.Point(11, 114);
            this.automateChkBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.automateChkBox.Name = "automateChkBox";
            this.automateChkBox.Size = new System.Drawing.Size(105, 24);
            this.automateChkBox.TabIndex = 37;
            this.automateChkBox.Text = "Automate";
            this.automateChkBox.UseVisualStyleBackColor = true;
            this.automateChkBox.CheckedChanged += new System.EventHandler(this.automateChkBox_CheckedChanged);
            // 
            // statusmsgTxtBox
            // 
            this.statusmsgTxtBox.Location = new System.Drawing.Point(4, 510);
            this.statusmsgTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.statusmsgTxtBox.Name = "statusmsgTxtBox";
            this.statusmsgTxtBox.Size = new System.Drawing.Size(510, 26);
            this.statusmsgTxtBox.TabIndex = 38;
            this.statusmsgTxtBox.TextChanged += new System.EventHandler(this.statusmsgTxtBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 482);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 20);
            this.label6.TabIndex = 39;
            this.label6.Text = "Status";
            // 
            // refreshrateGroupBox
            // 
            this.refreshrateGroupBox.Controls.Add(this.RefreshRate5m);
            this.refreshrateGroupBox.Controls.Add(this.RefreshRate1m);
            this.refreshrateGroupBox.Controls.Add(this.RefreshRate30s);
            this.refreshrateGroupBox.Controls.Add(this.RefreshRate10s);
            this.refreshrateGroupBox.Location = new System.Drawing.Point(10, 220);
            this.refreshrateGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.refreshrateGroupBox.Name = "refreshrateGroupBox";
            this.refreshrateGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.refreshrateGroupBox.Size = new System.Drawing.Size(144, 119);
            this.refreshrateGroupBox.TabIndex = 40;
            this.refreshrateGroupBox.TabStop = false;
            this.refreshrateGroupBox.Text = "Refresh Rate";
            // 
            // RefreshRate5m
            // 
            this.RefreshRate5m.AutoSize = true;
            this.RefreshRate5m.Location = new System.Drawing.Point(79, 78);
            this.RefreshRate5m.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RefreshRate5m.Name = "RefreshRate5m";
            this.RefreshRate5m.Size = new System.Drawing.Size(56, 24);
            this.RefreshRate5m.TabIndex = 3;
            this.RefreshRate5m.TabStop = true;
            this.RefreshRate5m.Text = "5m";
            this.RefreshRate5m.UseVisualStyleBackColor = true;
            this.RefreshRate5m.CheckedChanged += new System.EventHandler(this.RefreshRate5m_CheckedChanged);
            // 
            // RefreshRate1m
            // 
            this.RefreshRate1m.AutoSize = true;
            this.RefreshRate1m.Location = new System.Drawing.Point(14, 78);
            this.RefreshRate1m.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RefreshRate1m.Name = "RefreshRate1m";
            this.RefreshRate1m.Size = new System.Drawing.Size(56, 24);
            this.RefreshRate1m.TabIndex = 2;
            this.RefreshRate1m.TabStop = true;
            this.RefreshRate1m.Text = "1m";
            this.RefreshRate1m.UseVisualStyleBackColor = true;
            this.RefreshRate1m.CheckedChanged += new System.EventHandler(this.RefreshRate1m_CheckedChanged);
            // 
            // RefreshRate30s
            // 
            this.RefreshRate30s.AutoSize = true;
            this.RefreshRate30s.Location = new System.Drawing.Point(79, 38);
            this.RefreshRate30s.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RefreshRate30s.Name = "RefreshRate30s";
            this.RefreshRate30s.Size = new System.Drawing.Size(60, 24);
            this.RefreshRate30s.TabIndex = 1;
            this.RefreshRate30s.TabStop = true;
            this.RefreshRate30s.Text = "30s";
            this.RefreshRate30s.UseVisualStyleBackColor = true;
            this.RefreshRate30s.CheckedChanged += new System.EventHandler(this.RefreshRate30s_CheckedChanged);
            // 
            // RefreshRate10s
            // 
            this.RefreshRate10s.AutoSize = true;
            this.RefreshRate10s.Location = new System.Drawing.Point(14, 36);
            this.RefreshRate10s.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RefreshRate10s.Name = "RefreshRate10s";
            this.RefreshRate10s.Size = new System.Drawing.Size(60, 24);
            this.RefreshRate10s.TabIndex = 0;
            this.RefreshRate10s.TabStop = true;
            this.RefreshRate10s.Text = "10s";
            this.RefreshRate10s.UseVisualStyleBackColor = true;
            this.RefreshRate10s.CheckedChanged += new System.EventHandler(this.RefreshRate10s_CheckedChanged);
            // 
            // IdleTimer1
            // 
            this.IdleTimer1.Interval = 120000;
            this.IdleTimer1.Tick += new System.EventHandler(this.IdleTimer1_Tick);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(6, 22);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(106, 28);
            this.comboBox1.TabIndex = 41;
            // 
            // disconnectComPortBtn
            // 
            this.disconnectComPortBtn.Location = new System.Drawing.Point(6, 136);
            this.disconnectComPortBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.disconnectComPortBtn.Name = "disconnectComPortBtn";
            this.disconnectComPortBtn.Size = new System.Drawing.Size(107, 34);
            this.disconnectComPortBtn.TabIndex = 42;
            this.disconnectComPortBtn.Text = "Disconnect";
            this.disconnectComPortBtn.UseVisualStyleBackColor = true;
            this.disconnectComPortBtn.Click += new System.EventHandler(this.disconnectComPortBtn_Click);
            // 
            // RefreshTimer
            // 
            this.RefreshTimer.Interval = 10000;
            this.RefreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 57600;
            this.serialPort1.DtrEnable = true;
            this.serialPort1.PortName = "COM7";
            this.serialPort1.ReadTimeout = 5000;
            this.serialPort1.RtsEnable = true;
            this.serialPort1.WriteTimeout = 5000;
            // 
            // RefreshComPortBtn
            // 
            this.RefreshComPortBtn.Location = new System.Drawing.Point(121, 136);
            this.RefreshComPortBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RefreshComPortBtn.Name = "RefreshComPortBtn";
            this.RefreshComPortBtn.Size = new System.Drawing.Size(90, 34);
            this.RefreshComPortBtn.TabIndex = 43;
            this.RefreshComPortBtn.Text = "Refresh";
            this.RefreshComPortBtn.UseVisualStyleBackColor = true;
            this.RefreshComPortBtn.Click += new System.EventHandler(this.RefreshComPortBtn_Click);
            // 
            // ControllerVersionTxtBox
            // 
            this.ControllerVersionTxtBox.Location = new System.Drawing.Point(160, 220);
            this.ControllerVersionTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ControllerVersionTxtBox.Name = "ControllerVersionTxtBox";
            this.ControllerVersionTxtBox.Size = new System.Drawing.Size(180, 26);
            this.ControllerVersionTxtBox.TabIndex = 44;
            // 
            // DataLogBtn
            // 
            this.DataLogBtn.Location = new System.Drawing.Point(250, 108);
            this.DataLogBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DataLogBtn.Name = "DataLogBtn";
            this.DataLogBtn.Size = new System.Drawing.Size(135, 38);
            this.DataLogBtn.TabIndex = 45;
            this.DataLogBtn.Text = "Data Logging";
            this.DataLogBtn.UseVisualStyleBackColor = true;
            this.DataLogBtn.Click += new System.EventHandler(this.DataLogBtn_Click);
            // 
            // LogDataChkBox
            // 
            this.LogDataChkBox.AutoSize = true;
            this.LogDataChkBox.Location = new System.Drawing.Point(11, 148);
            this.LogDataChkBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LogDataChkBox.Name = "LogDataChkBox";
            this.LogDataChkBox.Size = new System.Drawing.Size(101, 24);
            this.LogDataChkBox.TabIndex = 46;
            this.LogDataChkBox.Text = "Log Data";
            this.LogDataChkBox.UseVisualStyleBackColor = true;
            // 
            // ch1tempoffsetBtn
            // 
            this.ch1tempoffsetBtn.Location = new System.Drawing.Point(15, 81);
            this.ch1tempoffsetBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ch1tempoffsetBtn.Name = "ch1tempoffsetBtn";
            this.ch1tempoffsetBtn.Size = new System.Drawing.Size(67, 31);
            this.ch1tempoffsetBtn.TabIndex = 50;
            this.ch1tempoffsetBtn.Text = "Set";
            this.ch1tempoffsetBtn.UseVisualStyleBackColor = true;
            this.ch1tempoffsetBtn.Click += new System.EventHandler(this.ch1tempoffsetBtn_Click);
            // 
            // ch2tempoffsetBtn
            // 
            this.ch2tempoffsetBtn.Location = new System.Drawing.Point(91, 81);
            this.ch2tempoffsetBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ch2tempoffsetBtn.Name = "ch2tempoffsetBtn";
            this.ch2tempoffsetBtn.Size = new System.Drawing.Size(67, 31);
            this.ch2tempoffsetBtn.TabIndex = 51;
            this.ch2tempoffsetBtn.Text = "Set";
            this.ch2tempoffsetBtn.UseVisualStyleBackColor = true;
            this.ch2tempoffsetBtn.Click += new System.EventHandler(this.ch2tempoffsetBtn_Click);
            // 
            // getchoffsetBtn
            // 
            this.getchoffsetBtn.Location = new System.Drawing.Point(240, 45);
            this.getchoffsetBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.getchoffsetBtn.Name = "getchoffsetBtn";
            this.getchoffsetBtn.Size = new System.Drawing.Size(67, 31);
            this.getchoffsetBtn.TabIndex = 52;
            this.getchoffsetBtn.Text = "Get";
            this.getchoffsetBtn.UseVisualStyleBackColor = true;
            this.getchoffsetBtn.Click += new System.EventHandler(this.getchoffsetBtn_Click);
            // 
            // cleartempoffsetBtn
            // 
            this.cleartempoffsetBtn.Location = new System.Drawing.Point(240, 81);
            this.cleartempoffsetBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cleartempoffsetBtn.Name = "cleartempoffsetBtn";
            this.cleartempoffsetBtn.Size = new System.Drawing.Size(67, 31);
            this.cleartempoffsetBtn.TabIndex = 55;
            this.cleartempoffsetBtn.Text = "Clear";
            this.cleartempoffsetBtn.UseVisualStyleBackColor = true;
            this.cleartempoffsetBtn.Click += new System.EventHandler(this.cleartempoffsetBtn_Click);
            // 
            // SetDirectoryBtn
            // 
            this.SetDirectoryBtn.Location = new System.Drawing.Point(336, 26);
            this.SetDirectoryBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SetDirectoryBtn.Name = "SetDirectoryBtn";
            this.SetDirectoryBtn.Size = new System.Drawing.Size(48, 31);
            this.SetDirectoryBtn.TabIndex = 56;
            this.SetDirectoryBtn.Text = "...";
            this.SetDirectoryBtn.UseVisualStyleBackColor = true;
            this.SetDirectoryBtn.Click += new System.EventHandler(this.SetDirectoryBtn_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(228, 29);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(103, 20);
            this.label9.TabIndex = 57;
            this.label9.Text = "Log Directory";
            // 
            // LogFileNameTxtBox
            // 
            this.LogFileNameTxtBox.Location = new System.Drawing.Point(11, 70);
            this.LogFileNameTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LogFileNameTxtBox.Name = "LogFileNameTxtBox";
            this.LogFileNameTxtBox.ReadOnly = true;
            this.LogFileNameTxtBox.Size = new System.Drawing.Size(300, 26);
            this.LogFileNameTxtBox.TabIndex = 58;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(316, 74);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 20);
            this.label10.TabIndex = 59;
            this.label10.Text = "Filename";
            // 
            // LCDEnableChkBox
            // 
            this.LCDEnableChkBox.AutoSize = true;
            this.LCDEnableChkBox.Checked = true;
            this.LCDEnableChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LCDEnableChkBox.Location = new System.Drawing.Point(10, 190);
            this.LCDEnableChkBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LCDEnableChkBox.Name = "LCDEnableChkBox";
            this.LCDEnableChkBox.Size = new System.Drawing.Size(121, 24);
            this.LCDEnableChkBox.TabIndex = 60;
            this.LCDEnableChkBox.Text = "LCD Enable";
            this.LCDEnableChkBox.UseVisualStyleBackColor = true;
            this.LCDEnableChkBox.CheckedChanged += new System.EventHandler(this.LCDEnableChkBox_CheckedChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(638, 33);
            this.menuStrip1.TabIndex = 61;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(51, 29);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.disableErrorLogFileToolStripMenuItem,
            this.enableErrorLogFileToolStripMenuItem,
            this.forceExitToolStripMenuItem,
            this.getControllerFirmwareVersionToolStripMenuItem,
            this.lCDSCreenDisplayTimeToolStripMenuItem,
            this.resetErrorLogPathToolStripMenuItem,
            this.shadowDewChannelSettingsToolStripMenuItem,
            this.temperatureModeToolStripMenuItem,
            this.writeEEROMOnExiitToolStripMenuItem,
            this.setEEPROMDefaultSettingsToolStripMenuItem,
            this.testModeStandaloneToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(88, 29);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // disableErrorLogFileToolStripMenuItem
            // 
            this.disableErrorLogFileToolStripMenuItem.Name = "disableErrorLogFileToolStripMenuItem";
            this.disableErrorLogFileToolStripMenuItem.Size = new System.Drawing.Size(346, 30);
            this.disableErrorLogFileToolStripMenuItem.Text = "Disable Error Log File";
            this.disableErrorLogFileToolStripMenuItem.Click += new System.EventHandler(this.disableErrorLogFileToolStripMenuItem_Click);
            // 
            // enableErrorLogFileToolStripMenuItem
            // 
            this.enableErrorLogFileToolStripMenuItem.Name = "enableErrorLogFileToolStripMenuItem";
            this.enableErrorLogFileToolStripMenuItem.Size = new System.Drawing.Size(346, 30);
            this.enableErrorLogFileToolStripMenuItem.Text = "Enable Error Log File";
            this.enableErrorLogFileToolStripMenuItem.Click += new System.EventHandler(this.enableErrorLogFileToolStripMenuItem_Click);
            // 
            // forceExitToolStripMenuItem
            // 
            this.forceExitToolStripMenuItem.Name = "forceExitToolStripMenuItem";
            this.forceExitToolStripMenuItem.Size = new System.Drawing.Size(346, 30);
            this.forceExitToolStripMenuItem.Text = "Force Exit";
            this.forceExitToolStripMenuItem.Click += new System.EventHandler(this.forceExitToolStripMenuItem_Click);
            // 
            // getControllerFirmwareVersionToolStripMenuItem
            // 
            this.getControllerFirmwareVersionToolStripMenuItem.Name = "getControllerFirmwareVersionToolStripMenuItem";
            this.getControllerFirmwareVersionToolStripMenuItem.Size = new System.Drawing.Size(346, 30);
            this.getControllerFirmwareVersionToolStripMenuItem.Text = "Get Controller Firmware Version";
            this.getControllerFirmwareVersionToolStripMenuItem.Click += new System.EventHandler(this.getControllerFirmwareVersionToolStripMenuItem_Click);
            // 
            // lCDSCreenDisplayTimeToolStripMenuItem
            // 
            this.lCDSCreenDisplayTimeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.secondsToolStripMenuItem,
            this.secondsToolStripMenuItem1,
            this.secondsToolStripMenuItem2,
            this.secondsToolStripMenuItem3,
            this.secondsToolStripMenuItem4,
            this.secondsToolStripMenuItem5});
            this.lCDSCreenDisplayTimeToolStripMenuItem.Name = "lCDSCreenDisplayTimeToolStripMenuItem";
            this.lCDSCreenDisplayTimeToolStripMenuItem.Size = new System.Drawing.Size(346, 30);
            this.lCDSCreenDisplayTimeToolStripMenuItem.Text = "LCD Screen Display Time";
            // 
            // secondsToolStripMenuItem
            // 
            this.secondsToolStripMenuItem.Name = "secondsToolStripMenuItem";
            this.secondsToolStripMenuItem.Size = new System.Drawing.Size(192, 30);
            this.secondsToolStripMenuItem.Text = "2.5 seconds";
            this.secondsToolStripMenuItem.Click += new System.EventHandler(this.secondsToolStripMenuItem_Click);
            // 
            // secondsToolStripMenuItem1
            // 
            this.secondsToolStripMenuItem1.Name = "secondsToolStripMenuItem1";
            this.secondsToolStripMenuItem1.Size = new System.Drawing.Size(192, 30);
            this.secondsToolStripMenuItem1.Text = "3.0 Seconds";
            this.secondsToolStripMenuItem1.Click += new System.EventHandler(this.secondsToolStripMenuItem1_Click);
            // 
            // secondsToolStripMenuItem2
            // 
            this.secondsToolStripMenuItem2.Name = "secondsToolStripMenuItem2";
            this.secondsToolStripMenuItem2.Size = new System.Drawing.Size(192, 30);
            this.secondsToolStripMenuItem2.Text = "3.5 seconds";
            this.secondsToolStripMenuItem2.Click += new System.EventHandler(this.secondsToolStripMenuItem2_Click);
            // 
            // secondsToolStripMenuItem3
            // 
            this.secondsToolStripMenuItem3.Name = "secondsToolStripMenuItem3";
            this.secondsToolStripMenuItem3.Size = new System.Drawing.Size(192, 30);
            this.secondsToolStripMenuItem3.Text = "4.0 seconds";
            this.secondsToolStripMenuItem3.Click += new System.EventHandler(this.secondsToolStripMenuItem3_Click);
            // 
            // secondsToolStripMenuItem4
            // 
            this.secondsToolStripMenuItem4.Name = "secondsToolStripMenuItem4";
            this.secondsToolStripMenuItem4.Size = new System.Drawing.Size(192, 30);
            this.secondsToolStripMenuItem4.Text = "4.5 seconds";
            this.secondsToolStripMenuItem4.Click += new System.EventHandler(this.secondsToolStripMenuItem4_Click);
            // 
            // secondsToolStripMenuItem5
            // 
            this.secondsToolStripMenuItem5.Name = "secondsToolStripMenuItem5";
            this.secondsToolStripMenuItem5.Size = new System.Drawing.Size(192, 30);
            this.secondsToolStripMenuItem5.Text = "5.0 seconds";
            this.secondsToolStripMenuItem5.Click += new System.EventHandler(this.secondsToolStripMenuItem5_Click);
            // 
            // resetErrorLogPathToolStripMenuItem
            // 
            this.resetErrorLogPathToolStripMenuItem.Name = "resetErrorLogPathToolStripMenuItem";
            this.resetErrorLogPathToolStripMenuItem.Size = new System.Drawing.Size(346, 30);
            this.resetErrorLogPathToolStripMenuItem.Text = "Reset Error Log Path";
            this.resetErrorLogPathToolStripMenuItem.Click += new System.EventHandler(this.resetErrorLogPathToolStripMenuItem_Click);
            // 
            // shadowDewChannelSettingsToolStripMenuItem
            // 
            this.shadowDewChannelSettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oFFToolStripMenuItem,
            this.channel1ToolStripMenuItem,
            this.channel2ToolStripMenuItem,
            this.manualSettingToolStripMenuItem,
            this.useTempProbe3ToolStripMenuItem,
            this.showCurrentShadowChannelToolStripMenuItem});
            this.shadowDewChannelSettingsToolStripMenuItem.Name = "shadowDewChannelSettingsToolStripMenuItem";
            this.shadowDewChannelSettingsToolStripMenuItem.Size = new System.Drawing.Size(346, 30);
            this.shadowDewChannelSettingsToolStripMenuItem.Text = "Shadow Dew Channel Settings";
            // 
            // oFFToolStripMenuItem
            // 
            this.oFFToolStripMenuItem.Name = "oFFToolStripMenuItem";
            this.oFFToolStripMenuItem.Size = new System.Drawing.Size(332, 30);
            this.oFFToolStripMenuItem.Text = "OFF";
            this.oFFToolStripMenuItem.Click += new System.EventHandler(this.oFFToolStripMenuItem_Click);
            // 
            // channel1ToolStripMenuItem
            // 
            this.channel1ToolStripMenuItem.Name = "channel1ToolStripMenuItem";
            this.channel1ToolStripMenuItem.Size = new System.Drawing.Size(332, 30);
            this.channel1ToolStripMenuItem.Text = "Channel 1";
            this.channel1ToolStripMenuItem.Click += new System.EventHandler(this.channel1ToolStripMenuItem_Click);
            // 
            // channel2ToolStripMenuItem
            // 
            this.channel2ToolStripMenuItem.Name = "channel2ToolStripMenuItem";
            this.channel2ToolStripMenuItem.Size = new System.Drawing.Size(332, 30);
            this.channel2ToolStripMenuItem.Text = "Channel 2";
            this.channel2ToolStripMenuItem.Click += new System.EventHandler(this.channel2ToolStripMenuItem_Click);
            // 
            // manualSettingToolStripMenuItem
            // 
            this.manualSettingToolStripMenuItem.Name = "manualSettingToolStripMenuItem";
            this.manualSettingToolStripMenuItem.Size = new System.Drawing.Size(332, 30);
            this.manualSettingToolStripMenuItem.Text = "Manual Setting";
            this.manualSettingToolStripMenuItem.Click += new System.EventHandler(this.manualSettingToolStripMenuItem_Click);
            // 
            // useTempProbe3ToolStripMenuItem
            // 
            this.useTempProbe3ToolStripMenuItem.Name = "useTempProbe3ToolStripMenuItem";
            this.useTempProbe3ToolStripMenuItem.Size = new System.Drawing.Size(332, 30);
            this.useTempProbe3ToolStripMenuItem.Text = "Use Temp Probe 3";
            this.useTempProbe3ToolStripMenuItem.Click += new System.EventHandler(this.useTempProbe3ToolStripMenuItem_Click);
            // 
            // showCurrentShadowChannelToolStripMenuItem
            // 
            this.showCurrentShadowChannelToolStripMenuItem.Name = "showCurrentShadowChannelToolStripMenuItem";
            this.showCurrentShadowChannelToolStripMenuItem.Size = new System.Drawing.Size(332, 30);
            this.showCurrentShadowChannelToolStripMenuItem.Text = "Show current shadow channel";
            this.showCurrentShadowChannelToolStripMenuItem.Click += new System.EventHandler(this.showCurrentShadowChannelToolStripMenuItem_Click);
            // 
            // temperatureModeToolStripMenuItem
            // 
            this.temperatureModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.celsiusToolStripMenuItem,
            this.fahrenheitToolStripMenuItem});
            this.temperatureModeToolStripMenuItem.Name = "temperatureModeToolStripMenuItem";
            this.temperatureModeToolStripMenuItem.Size = new System.Drawing.Size(346, 30);
            this.temperatureModeToolStripMenuItem.Text = "Temperature Mode";
            // 
            // celsiusToolStripMenuItem
            // 
            this.celsiusToolStripMenuItem.Name = "celsiusToolStripMenuItem";
            this.celsiusToolStripMenuItem.Size = new System.Drawing.Size(177, 30);
            this.celsiusToolStripMenuItem.Text = "Celsius";
            this.celsiusToolStripMenuItem.Click += new System.EventHandler(this.celsiusToolStripMenuItem_Click);
            // 
            // fahrenheitToolStripMenuItem
            // 
            this.fahrenheitToolStripMenuItem.Name = "fahrenheitToolStripMenuItem";
            this.fahrenheitToolStripMenuItem.Size = new System.Drawing.Size(177, 30);
            this.fahrenheitToolStripMenuItem.Text = "Fahrenheit";
            this.fahrenheitToolStripMenuItem.Click += new System.EventHandler(this.fahrenheitToolStripMenuItem_Click);
            // 
            // writeEEROMOnExiitToolStripMenuItem
            // 
            this.writeEEROMOnExiitToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noToolStripMenuItem,
            this.yesToolStripMenuItem});
            this.writeEEROMOnExiitToolStripMenuItem.Name = "writeEEROMOnExiitToolStripMenuItem";
            this.writeEEROMOnExiitToolStripMenuItem.Size = new System.Drawing.Size(346, 30);
            this.writeEEROMOnExiitToolStripMenuItem.Text = "Write EEPROM on Exiit";
            // 
            // noToolStripMenuItem
            // 
            this.noToolStripMenuItem.Name = "noToolStripMenuItem";
            this.noToolStripMenuItem.Size = new System.Drawing.Size(121, 30);
            this.noToolStripMenuItem.Text = "No";
            this.noToolStripMenuItem.Click += new System.EventHandler(this.noToolStripMenuItem_Click);
            // 
            // yesToolStripMenuItem
            // 
            this.yesToolStripMenuItem.Name = "yesToolStripMenuItem";
            this.yesToolStripMenuItem.Size = new System.Drawing.Size(121, 30);
            this.yesToolStripMenuItem.Text = "Yes";
            this.yesToolStripMenuItem.Click += new System.EventHandler(this.yesToolStripMenuItem_Click);
            // 
            // setEEPROMDefaultSettingsToolStripMenuItem
            // 
            this.setEEPROMDefaultSettingsToolStripMenuItem.Name = "setEEPROMDefaultSettingsToolStripMenuItem";
            this.setEEPROMDefaultSettingsToolStripMenuItem.Size = new System.Drawing.Size(346, 30);
            this.setEEPROMDefaultSettingsToolStripMenuItem.Text = "Set to EEPROM default settings";
            this.setEEPROMDefaultSettingsToolStripMenuItem.Click += new System.EventHandler(this.setEEPROMDefaultSettingsToolStripMenuItem_Click);
            // 
            // testModeStandaloneToolStripMenuItem
            // 
            this.testModeStandaloneToolStripMenuItem.Name = "testModeStandaloneToolStripMenuItem";
            this.testModeStandaloneToolStripMenuItem.Size = new System.Drawing.Size(346, 30);
            this.testModeStandaloneToolStripMenuItem.Text = "Test Mode Standalone";
            this.testModeStandaloneToolStripMenuItem.Click += new System.EventHandler(this.testModeStandaloneToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(74, 29);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // getfirmwareversion
            // 
            this.getfirmwareversion.AutoSize = true;
            this.getfirmwareversion.Location = new System.Drawing.Point(6, 220);
            this.getfirmwareversion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.getfirmwareversion.Name = "getfirmwareversion";
            this.getfirmwareversion.Size = new System.Drawing.Size(132, 20);
            this.getfirmwareversion.TabIndex = 70;
            this.getfirmwareversion.Text = "Firmware Version";
            this.getfirmwareversion.Click += new System.EventHandler(this.getfirmwareversion_Click);
            // 
            // ch3pwrSetButton
            // 
            this.ch3pwrSetButton.Location = new System.Drawing.Point(399, 167);
            this.ch3pwrSetButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ch3pwrSetButton.Name = "ch3pwrSetButton";
            this.ch3pwrSetButton.Size = new System.Drawing.Size(68, 31);
            this.ch3pwrSetButton.TabIndex = 75;
            this.ch3pwrSetButton.Text = "Set";
            this.ch3pwrSetButton.UseVisualStyleBackColor = true;
            this.ch3pwrSetButton.Click += new System.EventHandler(this.ch3pwrSetButton_Click);
            // 
            // ch3tempTxtBox
            // 
            this.ch3tempTxtBox.Location = new System.Drawing.Point(177, 148);
            this.ch3tempTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ch3tempTxtBox.Name = "ch3tempTxtBox";
            this.ch3tempTxtBox.ReadOnly = true;
            this.ch3tempTxtBox.Size = new System.Drawing.Size(56, 26);
            this.ch3tempTxtBox.TabIndex = 77;
            // 
            // ch3tempoffsetBtn
            // 
            this.ch3tempoffsetBtn.Location = new System.Drawing.Point(165, 81);
            this.ch3tempoffsetBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ch3tempoffsetBtn.Name = "ch3tempoffsetBtn";
            this.ch3tempoffsetBtn.Size = new System.Drawing.Size(67, 31);
            this.ch3tempoffsetBtn.TabIndex = 80;
            this.ch3tempoffsetBtn.Text = "Set";
            this.ch3tempoffsetBtn.UseVisualStyleBackColor = true;
            this.ch3tempoffsetBtn.Click += new System.EventHandler(this.ch3tempoffsetBtn_Click);
            // 
            // ch3PwrTrackBar
            // 
            this.ch3PwrTrackBar.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ch3PwrTrackBar.Location = new System.Drawing.Point(14, 167);
            this.ch3PwrTrackBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ch3PwrTrackBar.Maximum = 100;
            this.ch3PwrTrackBar.Name = "ch3PwrTrackBar";
            this.ch3PwrTrackBar.Size = new System.Drawing.Size(368, 69);
            this.ch3PwrTrackBar.SmallChange = 5;
            this.ch3PwrTrackBar.TabIndex = 83;
            this.ch3PwrTrackBar.TickFrequency = 10;
            this.ch3PwrTrackBar.Scroll += new System.EventHandler(this.ch3PwrTrackBar_Scroll);
            this.ch3PwrTrackBar.ValueChanged += new System.EventHandler(this.ch3PwrTrackBar_ValueChanged);
            // 
            // getPCBTempBtn
            // 
            this.getPCBTempBtn.Location = new System.Drawing.Point(76, 25);
            this.getPCBTempBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.getPCBTempBtn.Name = "getPCBTempBtn";
            this.getPCBTempBtn.Size = new System.Drawing.Size(122, 31);
            this.getPCBTempBtn.TabIndex = 85;
            this.getPCBTempBtn.Text = "Get";
            this.getPCBTempBtn.UseVisualStyleBackColor = true;
            this.getPCBTempBtn.Click += new System.EventHandler(this.getPCBTempBtn_Click);
            // 
            // setFanTempOnBtn
            // 
            this.setFanTempOnBtn.Location = new System.Drawing.Point(76, 73);
            this.setFanTempOnBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.setFanTempOnBtn.Name = "setFanTempOnBtn";
            this.setFanTempOnBtn.Size = new System.Drawing.Size(56, 31);
            this.setFanTempOnBtn.TabIndex = 88;
            this.setFanTempOnBtn.Text = "Set T";
            this.setFanTempOnBtn.UseVisualStyleBackColor = true;
            this.setFanTempOnBtn.Click += new System.EventHandler(this.setFanTempOnBtn_Click);
            // 
            // pcbtempTxtBox
            // 
            this.pcbtempTxtBox.Location = new System.Drawing.Point(10, 25);
            this.pcbtempTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pcbtempTxtBox.Name = "pcbtempTxtBox";
            this.pcbtempTxtBox.ReadOnly = true;
            this.pcbtempTxtBox.Size = new System.Drawing.Size(56, 26);
            this.pcbtempTxtBox.TabIndex = 84;
            this.pcbtempTxtBox.Text = "0";
            // 
            // getFanTempBtn
            // 
            this.getFanTempBtn.Location = new System.Drawing.Point(142, 73);
            this.getFanTempBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.getFanTempBtn.Name = "getFanTempBtn";
            this.getFanTempBtn.Size = new System.Drawing.Size(56, 31);
            this.getFanTempBtn.TabIndex = 89;
            this.getFanTempBtn.Text = "Get T";
            this.getFanTempBtn.UseVisualStyleBackColor = true;
            this.getFanTempBtn.Click += new System.EventHandler(this.getFanTempBtn_Click);
            // 
            // ClearStatusMsgsTimer
            // 
            this.ClearStatusMsgsTimer.Interval = 3000;
            this.ClearStatusMsgsTimer.Tick += new System.EventHandler(this.ClearStatusMsgsTimer_Tick);
            // 
            // GraphPic
            // 
            this.GraphPic.Image = ((System.Drawing.Image)(resources.GetObject("GraphPic.Image")));
            this.GraphPic.InitialImage = ((System.Drawing.Image)(resources.GetObject("GraphPic.InitialImage")));
            this.GraphPic.Location = new System.Drawing.Point(355, 262);
            this.GraphPic.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GraphPic.Name = "GraphPic";
            this.GraphPic.Size = new System.Drawing.Size(79, 92);
            this.GraphPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.GraphPic.TabIndex = 90;
            this.GraphPic.TabStop = false;
            this.GraphPic.Click += new System.EventHandler(this.GraphPic_Click);
            // 
            // ch3tempoffTxtBox
            // 
            this.ch3tempoffTxtBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::myDewControllerPro3.Properties.Settings.Default, "ch3tempoffset", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ch3tempoffTxtBox.Location = new System.Drawing.Point(165, 45);
            this.ch3tempoffTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ch3tempoffTxtBox.Name = "ch3tempoffTxtBox";
            this.ch3tempoffTxtBox.Size = new System.Drawing.Size(67, 26);
            this.ch3tempoffTxtBox.TabIndex = 79;
            this.ch3tempoffTxtBox.Text = global::myDewControllerPro3.Properties.Settings.Default.ch3tempoffset;
            this.ch3tempoffTxtBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ch3tempoffTxtBox_KeyPress);
            // 
            // ch2tempoffTxtBox
            // 
            this.ch2tempoffTxtBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::myDewControllerPro3.Properties.Settings.Default, "ch2tempoffset", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ch2tempoffTxtBox.Location = new System.Drawing.Point(91, 45);
            this.ch2tempoffTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ch2tempoffTxtBox.Name = "ch2tempoffTxtBox";
            this.ch2tempoffTxtBox.Size = new System.Drawing.Size(67, 26);
            this.ch2tempoffTxtBox.TabIndex = 49;
            this.ch2tempoffTxtBox.Text = global::myDewControllerPro3.Properties.Settings.Default.ch2tempoffset;
            this.ch2tempoffTxtBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ch2tempoffTxtBox_KeyPress);
            // 
            // ch1tempoffTxtBox
            // 
            this.ch1tempoffTxtBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::myDewControllerPro3.Properties.Settings.Default, "ch1tempoffset", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ch1tempoffTxtBox.Location = new System.Drawing.Point(14, 45);
            this.ch1tempoffTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ch1tempoffTxtBox.Name = "ch1tempoffTxtBox";
            this.ch1tempoffTxtBox.Size = new System.Drawing.Size(67, 26);
            this.ch1tempoffTxtBox.TabIndex = 48;
            this.ch1tempoffTxtBox.Text = global::myDewControllerPro3.Properties.Settings.Default.ch1tempoffset;
            this.ch1tempoffTxtBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ch1tempoffTxtBox_KeyPress);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.LogFileNameTxtBox);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.SetDirectoryBtn);
            this.groupBox3.Controls.Add(this.LogDirNametxtBox);
            this.groupBox3.Controls.Add(this.automateChkBox);
            this.groupBox3.Controls.Add(this.LogDataChkBox);
            this.groupBox3.Controls.Add(this.DataLogBtn);
            this.groupBox3.Controls.Add(this.soundChkBox);
            this.groupBox3.Location = new System.Drawing.Point(10, 10);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Size = new System.Drawing.Size(395, 198);
            this.groupBox3.TabIndex = 93;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Logging";
            // 
            // LogDirNametxtBox
            // 
            this.LogDirNametxtBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::myDewControllerPro3.Properties.Settings.Default, "errorlogdir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.LogDirNametxtBox.Location = new System.Drawing.Point(11, 28);
            this.LogDirNametxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LogDirNametxtBox.Name = "LogDirNametxtBox";
            this.LogDirNametxtBox.ReadOnly = true;
            this.LogDirNametxtBox.Size = new System.Drawing.Size(208, 26);
            this.LogDirNametxtBox.TabIndex = 47;
            this.LogDirNametxtBox.Text = global::myDewControllerPro3.Properties.Settings.Default.errorlogdir;
            // 
            // PCBFanControlsGrpBox
            // 
            this.PCBFanControlsGrpBox.Controls.Add(this.getFanTempOffBtn);
            this.PCBFanControlsGrpBox.Controls.Add(this.setFanTempOffBtn);
            this.PCBFanControlsGrpBox.Controls.Add(this.label5);
            this.PCBFanControlsGrpBox.Controls.Add(this.label4);
            this.PCBFanControlsGrpBox.Controls.Add(this.fantempoffTxtBox);
            this.PCBFanControlsGrpBox.Controls.Add(this.getFanTempBtn);
            this.PCBFanControlsGrpBox.Controls.Add(this.setFanTempOnBtn);
            this.PCBFanControlsGrpBox.Controls.Add(this.fantemponTxtBox);
            this.PCBFanControlsGrpBox.Controls.Add(this.getPCBTempBtn);
            this.PCBFanControlsGrpBox.Controls.Add(this.pcbtempTxtBox);
            this.PCBFanControlsGrpBox.Location = new System.Drawing.Point(4, 10);
            this.PCBFanControlsGrpBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PCBFanControlsGrpBox.Name = "PCBFanControlsGrpBox";
            this.PCBFanControlsGrpBox.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PCBFanControlsGrpBox.Size = new System.Drawing.Size(216, 170);
            this.PCBFanControlsGrpBox.TabIndex = 94;
            this.PCBFanControlsGrpBox.TabStop = false;
            this.PCBFanControlsGrpBox.Text = "PCB/Fan";
            // 
            // getFanTempOffBtn
            // 
            this.getFanTempOffBtn.Location = new System.Drawing.Point(142, 126);
            this.getFanTempOffBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.getFanTempOffBtn.Name = "getFanTempOffBtn";
            this.getFanTempOffBtn.Size = new System.Drawing.Size(56, 31);
            this.getFanTempOffBtn.TabIndex = 94;
            this.getFanTempOffBtn.Text = "Get T";
            this.getFanTempOffBtn.UseVisualStyleBackColor = true;
            this.getFanTempOffBtn.Click += new System.EventHandler(this.getFanTempOffBtn_Click);
            // 
            // setFanTempOffBtn
            // 
            this.setFanTempOffBtn.Location = new System.Drawing.Point(76, 126);
            this.setFanTempOffBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.setFanTempOffBtn.Name = "setFanTempOffBtn";
            this.setFanTempOffBtn.Size = new System.Drawing.Size(56, 31);
            this.setFanTempOffBtn.TabIndex = 93;
            this.setFanTempOffBtn.Text = "Set T";
            this.setFanTempOffBtn.UseVisualStyleBackColor = true;
            this.setFanTempOffBtn.Click += new System.EventHandler(this.setFanTempOffBtn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 20);
            this.label5.TabIndex = 92;
            this.label5.Text = "Off Temp";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 20);
            this.label4.TabIndex = 91;
            this.label4.Text = "On Temp";
            // 
            // fantempoffTxtBox
            // 
            this.fantempoffTxtBox.Location = new System.Drawing.Point(10, 126);
            this.fantempoffTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.fantempoffTxtBox.Name = "fantempoffTxtBox";
            this.fantempoffTxtBox.Size = new System.Drawing.Size(56, 26);
            this.fantempoffTxtBox.TabIndex = 90;
            this.fantempoffTxtBox.Text = "0";
            // 
            // fantemponTxtBox
            // 
            this.fantemponTxtBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::myDewControllerPro3.Properties.Settings.Default, "fantempsetting", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.fantemponTxtBox.Location = new System.Drawing.Point(10, 73);
            this.fantemponTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.fantemponTxtBox.Name = "fantemponTxtBox";
            this.fantemponTxtBox.Size = new System.Drawing.Size(56, 26);
            this.fantemponTxtBox.TabIndex = 87;
            this.fantemponTxtBox.Text = global::myDewControllerPro3.Properties.Settings.Default.fantempsetting;
            this.fantemponTxtBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fantemponTxtBox_KeyPress);
            // 
            // trackingModeOffsetGroupBox
            // 
            this.trackingModeOffsetGroupBox.Controls.Add(this.offsetplusBtn);
            this.trackingModeOffsetGroupBox.Controls.Add(this.offsetzeroBtn);
            this.trackingModeOffsetGroupBox.Controls.Add(this.offsetminusBtn);
            this.trackingModeOffsetGroupBox.Location = new System.Drawing.Point(153, 5);
            this.trackingModeOffsetGroupBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.trackingModeOffsetGroupBox.Name = "trackingModeOffsetGroupBox";
            this.trackingModeOffsetGroupBox.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.trackingModeOffsetGroupBox.Size = new System.Drawing.Size(270, 84);
            this.trackingModeOffsetGroupBox.TabIndex = 95;
            this.trackingModeOffsetGroupBox.TabStop = false;
            this.trackingModeOffsetGroupBox.Text = "Tracking Mode Offset";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.ComPortSpeed);
            this.groupBox6.Controls.Add(this.RefreshComPortBtn);
            this.groupBox6.Controls.Add(this.disconnectComPortBtn);
            this.groupBox6.Controls.Add(this.comboBox1);
            this.groupBox6.Controls.Add(this.comportConnectBtn);
            this.groupBox6.Location = new System.Drawing.Point(10, 10);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox6.Size = new System.Drawing.Size(216, 180);
            this.groupBox6.TabIndex = 96;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Com Port";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(115, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 20);
            this.label2.TabIndex = 46;
            this.label2.Text = "Speed";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(115, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 20);
            this.label1.TabIndex = 45;
            this.label1.Text = "Port";
            // 
            // ComPortSpeed
            // 
            this.ComPortSpeed.FormattingEnabled = true;
            this.ComPortSpeed.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.ComPortSpeed.Location = new System.Drawing.Point(6, 61);
            this.ComPortSpeed.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComPortSpeed.Name = "ComPortSpeed";
            this.ComPortSpeed.Size = new System.Drawing.Size(106, 28);
            this.ComPortSpeed.TabIndex = 44;
            this.ComPortSpeed.SelectedIndexChanged += new System.EventHandler(this.ComPortSpeed_SelectedIndexChanged);
            // 
            // mytabControl
            // 
            this.mytabControl.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.mytabControl.Controls.Add(this.tabTemperature);
            this.mytabControl.Controls.Add(this.tabPower);
            this.mytabControl.Controls.Add(this.tabTracking);
            this.mytabControl.Controls.Add(this.tabLogging);
            this.mytabControl.Controls.Add(this.tabFanAndPCB);
            this.mytabControl.Controls.Add(this.tabMisc);
            this.mytabControl.Controls.Add(this.tabSerialPort);
            this.mytabControl.Location = new System.Drawing.Point(4, 36);
            this.mytabControl.Name = "mytabControl";
            this.mytabControl.SelectedIndex = 0;
            this.mytabControl.Size = new System.Drawing.Size(622, 440);
            this.mytabControl.TabIndex = 97;
            // 
            // tabTemperature
            // 
            this.tabTemperature.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tabTemperature.Controls.Add(this.label7);
            this.tabTemperature.Controls.Add(this.manualupdateBtn);
            this.tabTemperature.Controls.Add(this.TemperatureGroupBox);
            this.tabTemperature.Controls.Add(this.OffsetsGroupBox);
            this.tabTemperature.Controls.Add(this.GraphPic);
            this.tabTemperature.Controls.Add(this.atbiasoffsetGroupBox);
            this.tabTemperature.Location = new System.Drawing.Point(4, 32);
            this.tabTemperature.Name = "tabTemperature";
            this.tabTemperature.Padding = new System.Windows.Forms.Padding(3);
            this.tabTemperature.Size = new System.Drawing.Size(614, 404);
            this.tabTemperature.TabIndex = 0;
            this.tabTemperature.Text = "Temperature";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(440, 301);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(103, 20);
            this.label7.TabIndex = 95;
            this.label7.Text = "Graph Data";
            // 
            // manualupdateBtn
            // 
            this.manualupdateBtn.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.manualupdateBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.manualupdateBtn.Location = new System.Drawing.Point(355, 360);
            this.manualupdateBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.manualupdateBtn.Name = "manualupdateBtn";
            this.manualupdateBtn.Size = new System.Drawing.Size(160, 38);
            this.manualupdateBtn.TabIndex = 111;
            this.manualupdateBtn.Text = "Manual Update";
            this.manualupdateBtn.UseVisualStyleBackColor = false;
            // 
            // TemperatureGroupBox
            // 
            this.TemperatureGroupBox.Controls.Add(this.label3);
            this.TemperatureGroupBox.Controls.Add(this.ch3tempTxtBox);
            this.TemperatureGroupBox.Controls.Add(this.ch3label);
            this.TemperatureGroupBox.Controls.Add(this.ch3templabel);
            this.TemperatureGroupBox.Controls.Add(this.label38);
            this.TemperatureGroupBox.Controls.Add(this.dplabel);
            this.TemperatureGroupBox.Controls.Add(this.ch2label);
            this.TemperatureGroupBox.Controls.Add(this.ch1label);
            this.TemperatureGroupBox.Controls.Add(this.atlabel);
            this.TemperatureGroupBox.Controls.Add(this.label45);
            this.TemperatureGroupBox.Controls.Add(this.dewpointTxtBox);
            this.TemperatureGroupBox.Controls.Add(this.ch2templabel);
            this.TemperatureGroupBox.Controls.Add(this.ch1tempTxtBox);
            this.TemperatureGroupBox.Controls.Add(this.labelnewBtn);
            this.TemperatureGroupBox.Controls.Add(this.ch2tempTxtBox);
            this.TemperatureGroupBox.Controls.Add(this.labelnewTxtBox);
            this.TemperatureGroupBox.Controls.Add(this.ch1templabel);
            this.TemperatureGroupBox.Controls.Add(this.ambientTemperatureTxtBox);
            this.TemperatureGroupBox.Controls.Add(this.relativeHumidityTxtBox);
            this.TemperatureGroupBox.Controls.Add(this.label48);
            this.TemperatureGroupBox.Controls.Add(this.label49);
            this.TemperatureGroupBox.Location = new System.Drawing.Point(5, 5);
            this.TemperatureGroupBox.Name = "TemperatureGroupBox";
            this.TemperatureGroupBox.Size = new System.Drawing.Size(340, 270);
            this.TemperatureGroupBox.TabIndex = 110;
            this.TemperatureGroupBox.TabStop = false;
            this.TemperatureGroupBox.Text = "Temperatures";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 210);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 20);
            this.label3.TabIndex = 109;
            this.label3.Text = "New Label Value";
            // 
            // ch3label
            // 
            this.ch3label.AutoSize = true;
            this.ch3label.Location = new System.Drawing.Point(241, 155);
            this.ch3label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ch3label.Name = "ch3label";
            this.ch3label.Size = new System.Drawing.Size(20, 20);
            this.ch3label.TabIndex = 105;
            this.ch3label.Text = "C";
            // 
            // ch3templabel
            // 
            this.ch3templabel.AutoSize = true;
            this.ch3templabel.Location = new System.Drawing.Point(10, 150);
            this.ch3templabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ch3templabel.Name = "ch3templabel";
            this.ch3templabel.Size = new System.Drawing.Size(133, 20);
            this.ch3templabel.TabIndex = 103;
            this.ch3templabel.Text = "Ch3 Temperature";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(241, 32);
            this.label38.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(23, 20);
            this.label38.TabIndex = 97;
            this.label38.Text = "%";
            // 
            // dplabel
            // 
            this.dplabel.AutoSize = true;
            this.dplabel.Location = new System.Drawing.Point(241, 185);
            this.dplabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dplabel.Name = "dplabel";
            this.dplabel.Size = new System.Drawing.Size(20, 20);
            this.dplabel.TabIndex = 96;
            this.dplabel.Text = "C";
            // 
            // ch2label
            // 
            this.ch2label.AutoSize = true;
            this.ch2label.Location = new System.Drawing.Point(241, 125);
            this.ch2label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ch2label.Name = "ch2label";
            this.ch2label.Size = new System.Drawing.Size(20, 20);
            this.ch2label.TabIndex = 95;
            this.ch2label.Text = "C";
            // 
            // ch1label
            // 
            this.ch1label.AutoSize = true;
            this.ch1label.Location = new System.Drawing.Point(241, 95);
            this.ch1label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ch1label.Name = "ch1label";
            this.ch1label.Size = new System.Drawing.Size(20, 20);
            this.ch1label.TabIndex = 94;
            this.ch1label.Text = "C";
            // 
            // atlabel
            // 
            this.atlabel.AutoSize = true;
            this.atlabel.Location = new System.Drawing.Point(241, 65);
            this.atlabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.atlabel.Name = "atlabel";
            this.atlabel.Size = new System.Drawing.Size(20, 20);
            this.atlabel.TabIndex = 93;
            this.atlabel.Text = "C";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(10, 180);
            this.label45.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(81, 20);
            this.label45.TabIndex = 87;
            this.label45.Text = "Dew Point";
            // 
            // ch2templabel
            // 
            this.ch2templabel.AutoSize = true;
            this.ch2templabel.Location = new System.Drawing.Point(10, 120);
            this.ch2templabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ch2templabel.Name = "ch2templabel";
            this.ch2templabel.Size = new System.Drawing.Size(133, 20);
            this.ch2templabel.TabIndex = 85;
            this.ch2templabel.Text = "Ch2 Temperature";
            // 
            // ch1tempTxtBox
            // 
            this.ch1tempTxtBox.Location = new System.Drawing.Point(178, 88);
            this.ch1tempTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ch1tempTxtBox.Name = "ch1tempTxtBox";
            this.ch1tempTxtBox.ReadOnly = true;
            this.ch1tempTxtBox.Size = new System.Drawing.Size(56, 26);
            this.ch1tempTxtBox.TabIndex = 84;
            // 
            // ch1templabel
            // 
            this.ch1templabel.AutoSize = true;
            this.ch1templabel.Location = new System.Drawing.Point(10, 90);
            this.ch1templabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ch1templabel.Name = "ch1templabel";
            this.ch1templabel.Size = new System.Drawing.Size(133, 20);
            this.ch1templabel.TabIndex = 83;
            this.ch1templabel.Text = "Ch1 Temperature";
            // 
            // ambientTemperatureTxtBox
            // 
            this.ambientTemperatureTxtBox.Location = new System.Drawing.Point(178, 58);
            this.ambientTemperatureTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ambientTemperatureTxtBox.Name = "ambientTemperatureTxtBox";
            this.ambientTemperatureTxtBox.ReadOnly = true;
            this.ambientTemperatureTxtBox.Size = new System.Drawing.Size(56, 26);
            this.ambientTemperatureTxtBox.TabIndex = 82;
            // 
            // relativeHumidityTxtBox
            // 
            this.relativeHumidityTxtBox.Location = new System.Drawing.Point(178, 28);
            this.relativeHumidityTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.relativeHumidityTxtBox.Name = "relativeHumidityTxtBox";
            this.relativeHumidityTxtBox.ReadOnly = true;
            this.relativeHumidityTxtBox.Size = new System.Drawing.Size(56, 26);
            this.relativeHumidityTxtBox.TabIndex = 81;
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(10, 60);
            this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(163, 20);
            this.label48.TabIndex = 80;
            this.label48.Text = "Ambient Temperature";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(10, 30);
            this.label49.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(131, 20);
            this.label49.TabIndex = 79;
            this.label49.Text = "Relative Humidity";
            // 
            // OffsetsGroupBox
            // 
            this.OffsetsGroupBox.Controls.Add(this.label50);
            this.OffsetsGroupBox.Controls.Add(this.cleartempoffsetBtn);
            this.OffsetsGroupBox.Controls.Add(this.ch3tempoffsetBtn);
            this.OffsetsGroupBox.Controls.Add(this.ch3tempoffTxtBox);
            this.OffsetsGroupBox.Controls.Add(this.label51);
            this.OffsetsGroupBox.Controls.Add(this.getchoffsetBtn);
            this.OffsetsGroupBox.Controls.Add(this.label52);
            this.OffsetsGroupBox.Controls.Add(this.ch2tempoffsetBtn);
            this.OffsetsGroupBox.Controls.Add(this.ch1tempoffTxtBox);
            this.OffsetsGroupBox.Controls.Add(this.ch1tempoffsetBtn);
            this.OffsetsGroupBox.Controls.Add(this.ch2tempoffTxtBox);
            this.OffsetsGroupBox.Location = new System.Drawing.Point(5, 280);
            this.OffsetsGroupBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.OffsetsGroupBox.Name = "OffsetsGroupBox";
            this.OffsetsGroupBox.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.OffsetsGroupBox.Size = new System.Drawing.Size(320, 120);
            this.OffsetsGroupBox.TabIndex = 108;
            this.OffsetsGroupBox.TabStop = false;
            this.OffsetsGroupBox.Text = "Channel Offsets";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(161, 24);
            this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(38, 20);
            this.label50.TabIndex = 81;
            this.label50.Text = "Ch3";
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(86, 25);
            this.label51.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(38, 20);
            this.label51.TabIndex = 54;
            this.label51.Text = "Ch2";
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(11, 24);
            this.label52.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(38, 20);
            this.label52.TabIndex = 53;
            this.label52.Text = "Ch1";
            // 
            // tabPower
            // 
            this.tabPower.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tabPower.Controls.Add(this.PowerGroupBox);
            this.tabPower.Location = new System.Drawing.Point(4, 32);
            this.tabPower.Name = "tabPower";
            this.tabPower.Padding = new System.Windows.Forms.Padding(3);
            this.tabPower.Size = new System.Drawing.Size(614, 404);
            this.tabPower.TabIndex = 1;
            this.tabPower.Text = "Power";
            // 
            // PowerGroupBox
            // 
            this.PowerGroupBox.Controls.Add(this.ch3pwrTxtBox);
            this.PowerGroupBox.Controls.Add(this.label34);
            this.PowerGroupBox.Controls.Add(this.ch3pwrlabel);
            this.PowerGroupBox.Controls.Add(this.label36);
            this.PowerGroupBox.Controls.Add(this.label37);
            this.PowerGroupBox.Controls.Add(this.ch2pwrTxtBox);
            this.PowerGroupBox.Controls.Add(this.ch1pwrTxtBox);
            this.PowerGroupBox.Controls.Add(this.ch2pwrlabel);
            this.PowerGroupBox.Controls.Add(this.ch1pwrlabel);
            this.PowerGroupBox.Controls.Add(this.ch3pwrSetButton);
            this.PowerGroupBox.Controls.Add(this.ch3PwrTrackBar);
            this.PowerGroupBox.Controls.Add(this.label13);
            this.PowerGroupBox.Controls.Add(this.off100Btn);
            this.PowerGroupBox.Controls.Add(this.ch1100Btn);
            this.PowerGroupBox.Controls.Add(this.ch2100Btn);
            this.PowerGroupBox.Location = new System.Drawing.Point(5, 5);
            this.PowerGroupBox.Name = "PowerGroupBox";
            this.PowerGroupBox.Size = new System.Drawing.Size(474, 252);
            this.PowerGroupBox.TabIndex = 112;
            this.PowerGroupBox.TabStop = false;
            this.PowerGroupBox.Text = "Power";
            // 
            // ch3pwrTxtBox
            // 
            this.ch3pwrTxtBox.Location = new System.Drawing.Point(104, 100);
            this.ch3pwrTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ch3pwrTxtBox.Name = "ch3pwrTxtBox";
            this.ch3pwrTxtBox.ReadOnly = true;
            this.ch3pwrTxtBox.Size = new System.Drawing.Size(56, 26);
            this.ch3pwrTxtBox.TabIndex = 105;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(168, 105);
            this.label34.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(23, 20);
            this.label34.TabIndex = 111;
            this.label34.Text = "%";
            // 
            // ch3pwrlabel
            // 
            this.ch3pwrlabel.AutoSize = true;
            this.ch3pwrlabel.Location = new System.Drawing.Point(10, 104);
            this.ch3pwrlabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ch3pwrlabel.Name = "ch3pwrlabel";
            this.ch3pwrlabel.Size = new System.Drawing.Size(86, 20);
            this.ch3pwrlabel.TabIndex = 110;
            this.ch3pwrlabel.Text = "Ch3 Power";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(168, 69);
            this.label36.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(23, 20);
            this.label36.TabIndex = 109;
            this.label36.Text = "%";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(168, 35);
            this.label37.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(23, 20);
            this.label37.TabIndex = 108;
            this.label37.Text = "%";
            // 
            // ch2pwrTxtBox
            // 
            this.ch2pwrTxtBox.Location = new System.Drawing.Point(104, 68);
            this.ch2pwrTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ch2pwrTxtBox.Name = "ch2pwrTxtBox";
            this.ch2pwrTxtBox.ReadOnly = true;
            this.ch2pwrTxtBox.Size = new System.Drawing.Size(56, 26);
            this.ch2pwrTxtBox.TabIndex = 104;
            // 
            // ch1pwrTxtBox
            // 
            this.ch1pwrTxtBox.Location = new System.Drawing.Point(104, 34);
            this.ch1pwrTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ch1pwrTxtBox.Name = "ch1pwrTxtBox";
            this.ch1pwrTxtBox.ReadOnly = true;
            this.ch1pwrTxtBox.Size = new System.Drawing.Size(56, 26);
            this.ch1pwrTxtBox.TabIndex = 103;
            // 
            // ch2pwrlabel
            // 
            this.ch2pwrlabel.AutoSize = true;
            this.ch2pwrlabel.Location = new System.Drawing.Point(10, 71);
            this.ch2pwrlabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ch2pwrlabel.Name = "ch2pwrlabel";
            this.ch2pwrlabel.Size = new System.Drawing.Size(86, 20);
            this.ch2pwrlabel.TabIndex = 107;
            this.ch2pwrlabel.Text = "Ch2 Power";
            // 
            // ch1pwrlabel
            // 
            this.ch1pwrlabel.AutoSize = true;
            this.ch1pwrlabel.Location = new System.Drawing.Point(10, 37);
            this.ch1pwrlabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ch1pwrlabel.Name = "ch1pwrlabel";
            this.ch1pwrlabel.Size = new System.Drawing.Size(86, 20);
            this.ch1pwrlabel.TabIndex = 106;
            this.ch1pwrlabel.Text = "Ch1 Power";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(10, 143);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(64, 20);
            this.label13.TabIndex = 1;
            this.label13.Text = "Ch3 set";
            // 
            // tabTracking
            // 
            this.tabTracking.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tabTracking.Controls.Add(this.trackingModeOffsetGroupBox);
            this.tabTracking.Controls.Add(this.trackingmodeGroupBox);
            this.tabTracking.Location = new System.Drawing.Point(4, 32);
            this.tabTracking.Name = "tabTracking";
            this.tabTracking.Size = new System.Drawing.Size(614, 404);
            this.tabTracking.TabIndex = 2;
            this.tabTracking.Text = "Tracking";
            // 
            // tabLogging
            // 
            this.tabLogging.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tabLogging.Controls.Add(this.reseterrorlogpathBtn);
            this.tabLogging.Controls.Add(this.groupBox3);
            this.tabLogging.Controls.Add(this.refreshrateGroupBox);
            this.tabLogging.Location = new System.Drawing.Point(4, 32);
            this.tabLogging.Name = "tabLogging";
            this.tabLogging.Size = new System.Drawing.Size(614, 404);
            this.tabLogging.TabIndex = 3;
            this.tabLogging.Text = "Logging";
            // 
            // reseterrorlogpathBtn
            // 
            this.reseterrorlogpathBtn.Location = new System.Drawing.Point(10, 357);
            this.reseterrorlogpathBtn.Name = "reseterrorlogpathBtn";
            this.reseterrorlogpathBtn.Size = new System.Drawing.Size(180, 38);
            this.reseterrorlogpathBtn.TabIndex = 94;
            this.reseterrorlogpathBtn.Text = "Reset Error Log Path";
            this.reseterrorlogpathBtn.UseVisualStyleBackColor = true;
            this.reseterrorlogpathBtn.Click += new System.EventHandler(this.reseterrorlogpathBtn_Click);
            // 
            // tabSerialPort
            // 
            this.tabSerialPort.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tabSerialPort.Controls.Add(this.groupBox6);
            this.tabSerialPort.Location = new System.Drawing.Point(4, 32);
            this.tabSerialPort.Name = "tabSerialPort";
            this.tabSerialPort.Size = new System.Drawing.Size(614, 404);
            this.tabSerialPort.TabIndex = 4;
            this.tabSerialPort.Text = "Connection";
            // 
            // tabFanAndPCB
            // 
            this.tabFanAndPCB.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tabFanAndPCB.Controls.Add(this.PCBFanControlsGrpBox);
            this.tabFanAndPCB.Controls.Add(this.fanspeedGroupBox);
            this.tabFanAndPCB.Location = new System.Drawing.Point(4, 32);
            this.tabFanAndPCB.Name = "tabFanAndPCB";
            this.tabFanAndPCB.Size = new System.Drawing.Size(614, 404);
            this.tabFanAndPCB.TabIndex = 5;
            this.tabFanAndPCB.Text = "Fan/PCB";
            // 
            // tabMisc
            // 
            this.tabMisc.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tabMisc.Controls.Add(this.tesmodestandaloneBtn);
            this.tabMisc.Controls.Add(this.resetControllertodefaultsBtn);
            this.tabMisc.Controls.Add(this.writeEEPROMonExitChkBox);
            this.tabMisc.Controls.Add(this.getfirmwareversion);
            this.tabMisc.Controls.Add(this.ControllerVersionTxtBox);
            this.tabMisc.Controls.Add(this.writeEEPROMBtn);
            this.tabMisc.Controls.Add(this.LCDEnableChkBox);
            this.tabMisc.Location = new System.Drawing.Point(4, 32);
            this.tabMisc.Name = "tabMisc";
            this.tabMisc.Size = new System.Drawing.Size(614, 404);
            this.tabMisc.TabIndex = 6;
            this.tabMisc.Text = "Misc";
            // 
            // tesmodestandaloneBtn
            // 
            this.tesmodestandaloneBtn.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.tesmodestandaloneBtn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.tesmodestandaloneBtn.FlatAppearance.BorderSize = 2;
            this.tesmodestandaloneBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.tesmodestandaloneBtn.Location = new System.Drawing.Point(10, 110);
            this.tesmodestandaloneBtn.Name = "tesmodestandaloneBtn";
            this.tesmodestandaloneBtn.Size = new System.Drawing.Size(300, 40);
            this.tesmodestandaloneBtn.TabIndex = 73;
            this.tesmodestandaloneBtn.Text = "Test Mode - Standalone";
            this.tesmodestandaloneBtn.UseVisualStyleBackColor = false;
            this.tesmodestandaloneBtn.Click += new System.EventHandler(this.tesmodestandaloneBtn_Click);
            // 
            // resetControllertodefaultsBtn
            // 
            this.resetControllertodefaultsBtn.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.resetControllertodefaultsBtn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.resetControllertodefaultsBtn.FlatAppearance.BorderSize = 2;
            this.resetControllertodefaultsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.resetControllertodefaultsBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetControllertodefaultsBtn.Location = new System.Drawing.Point(10, 60);
            this.resetControllertodefaultsBtn.Name = "resetControllertodefaultsBtn";
            this.resetControllertodefaultsBtn.Size = new System.Drawing.Size(300, 40);
            this.resetControllertodefaultsBtn.TabIndex = 72;
            this.resetControllertodefaultsBtn.Text = "Reset Controller to Defaults";
            this.resetControllertodefaultsBtn.UseVisualStyleBackColor = false;
            this.resetControllertodefaultsBtn.Click += new System.EventHandler(this.resetControllertodefaultsBtn_Click);
            // 
            // writeEEPROMonExitChkBox
            // 
            this.writeEEPROMonExitChkBox.AutoSize = true;
            this.writeEEPROMonExitChkBox.Checked = global::myDewControllerPro3.Properties.Settings.Default.WriteEEPROMonExit;
            this.writeEEPROMonExitChkBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::myDewControllerPro3.Properties.Settings.Default, "WriteEEPROMonExit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.writeEEPROMonExitChkBox.Location = new System.Drawing.Point(10, 160);
            this.writeEEPROMonExitChkBox.Name = "writeEEPROMonExitChkBox";
            this.writeEEPROMonExitChkBox.Size = new System.Drawing.Size(213, 24);
            this.writeEEPROMonExitChkBox.TabIndex = 71;
            this.writeEEPROMonExitChkBox.Text = "Write to EEPROM on exit";
            this.writeEEPROMonExitChkBox.UseVisualStyleBackColor = true;
            // 
            // myDewController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(638, 554);
            this.Controls.Add(this.mytabControl);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.statusmsgTxtBox);
            this.Controls.Add(this.ExitBtn);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "myDewController";
            this.Text = "myDewControllerPro3";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.myDewController_FormClosing);
            this.Load += new System.EventHandler(this.myDewController_Load);
            this.trackingmodeGroupBox.ResumeLayout(false);
            this.trackingmodeGroupBox.PerformLayout();
            this.fanspeedGroupBox.ResumeLayout(false);
            this.fanspeedGroupBox.PerformLayout();
            this.atbiasoffsetGroupBox.ResumeLayout(false);
            this.atbiasoffsetGroupBox.PerformLayout();
            this.refreshrateGroupBox.ResumeLayout(false);
            this.refreshrateGroupBox.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ch3PwrTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GraphPic)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.PCBFanControlsGrpBox.ResumeLayout(false);
            this.PCBFanControlsGrpBox.PerformLayout();
            this.trackingModeOffsetGroupBox.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.mytabControl.ResumeLayout(false);
            this.tabTemperature.ResumeLayout(false);
            this.tabTemperature.PerformLayout();
            this.TemperatureGroupBox.ResumeLayout(false);
            this.TemperatureGroupBox.PerformLayout();
            this.OffsetsGroupBox.ResumeLayout(false);
            this.OffsetsGroupBox.PerformLayout();
            this.tabPower.ResumeLayout(false);
            this.PowerGroupBox.ResumeLayout(false);
            this.PowerGroupBox.PerformLayout();
            this.tabTracking.ResumeLayout(false);
            this.tabLogging.ResumeLayout(false);
            this.tabSerialPort.ResumeLayout(false);
            this.tabFanAndPCB.ResumeLayout(false);
            this.tabMisc.ResumeLayout(false);
            this.tabMisc.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ExitBtn;
        private System.Windows.Forms.Button comportConnectBtn;
        private System.Windows.Forms.TextBox ch2tempTxtBox;
        private System.Windows.Forms.TextBox dewpointTxtBox;
        private System.Windows.Forms.TextBox labelnewTxtBox;
        private System.Windows.Forms.Button labelnewBtn;
        private System.Windows.Forms.Button ch1100Btn;
        private System.Windows.Forms.Button ch2100Btn;
        private System.Windows.Forms.Button off100Btn;
        private System.Windows.Forms.Button writeEEPROMBtn;
        private System.Windows.Forms.CheckBox soundChkBox;
        private System.Windows.Forms.Button offsetminusBtn;
        private System.Windows.Forms.Button offsetzeroBtn;
        private System.Windows.Forms.Button offsetplusBtn;
        private System.Windows.Forms.GroupBox trackingmodeGroupBox;
        private System.Windows.Forms.RadioButton TrackModeDewPoint;
        private System.Windows.Forms.RadioButton TrackModeAmbient;
        private System.Windows.Forms.RadioButton TrackModeMidPoint;
        private System.Windows.Forms.GroupBox fanspeedGroupBox;
        private System.Windows.Forms.RadioButton FanSpeed100;
        private System.Windows.Forms.RadioButton FanSpeed75;
        private System.Windows.Forms.RadioButton FanSpeed50;
        private System.Windows.Forms.RadioButton FanSpeedZero;
        private System.Windows.Forms.GroupBox atbiasoffsetGroupBox;
        private System.Windows.Forms.RadioButton ATBiasPlus3;
        private System.Windows.Forms.RadioButton ATBiasPlus2;
        private System.Windows.Forms.RadioButton ATBiasPlus1;
        private System.Windows.Forms.RadioButton ATBiasZero;
        private System.Windows.Forms.RadioButton ATBiasMinus1;
        private System.Windows.Forms.RadioButton ATBiasMinus2;
        private System.Windows.Forms.RadioButton ATBiasMinus3;
        private System.Windows.Forms.RadioButton ATBiasMinus4;
        private System.Windows.Forms.CheckBox automateChkBox;
        private System.Windows.Forms.TextBox statusmsgTxtBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox refreshrateGroupBox;
        private System.Windows.Forms.RadioButton RefreshRate5m;
        private System.Windows.Forms.RadioButton RefreshRate1m;
        private System.Windows.Forms.RadioButton RefreshRate30s;
        private System.Windows.Forms.RadioButton RefreshRate10s;
        private System.Windows.Forms.Timer IdleTimer1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button disconnectComPortBtn;
        private System.Windows.Forms.Timer RefreshTimer;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button RefreshComPortBtn;
        private System.Windows.Forms.TextBox ControllerVersionTxtBox;
        private System.Windows.Forms.Button DataLogBtn;
        private System.Windows.Forms.CheckBox LogDataChkBox;
        private System.Windows.Forms.TextBox LogDirNametxtBox;
        private System.Windows.Forms.TextBox ch1tempoffTxtBox;
        private System.Windows.Forms.TextBox ch2tempoffTxtBox;
        private System.Windows.Forms.Button ch1tempoffsetBtn;
        private System.Windows.Forms.Button ch2tempoffsetBtn;
        private System.Windows.Forms.Button getchoffsetBtn;
        private System.Windows.Forms.Button cleartempoffsetBtn;
        private System.Windows.Forms.Button SetDirectoryBtn;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox LogFileNameTxtBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox LCDEnableChkBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableErrorLogFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableErrorLogFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetErrorLogPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forceExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getControllerFirmwareVersionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem writeEEROMOnExiitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem temperatureModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem celsiusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fahrenheitToolStripMenuItem;
        private System.Windows.Forms.Label getfirmwareversion;
        private System.Windows.Forms.ToolStripMenuItem shadowDewChannelSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oFFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem channel1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem channel2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showCurrentShadowChannelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualSettingToolStripMenuItem;
        private System.Windows.Forms.Button ch3pwrSetButton;
        private System.Windows.Forms.TextBox ch3tempTxtBox;
        private System.Windows.Forms.Button ch3tempoffsetBtn;
        private System.Windows.Forms.TextBox ch3tempoffTxtBox;
        private System.Windows.Forms.ToolStripMenuItem useTempProbe3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lCDSCreenDisplayTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem secondsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem secondsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem secondsToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem secondsToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem secondsToolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem secondsToolStripMenuItem5;
        private System.Windows.Forms.TrackBar ch3PwrTrackBar;
        private System.Windows.Forms.TextBox pcbtempTxtBox;
        private System.Windows.Forms.Button getPCBTempBtn;
        private System.Windows.Forms.TextBox fantemponTxtBox;
        private System.Windows.Forms.Button setFanTempOnBtn;
        private System.Windows.Forms.Button getFanTempBtn;
        private System.Windows.Forms.ToolStripMenuItem setEEPROMDefaultSettingsToolStripMenuItem;
        private System.Windows.Forms.Timer ClearStatusMsgsTimer;
        private System.Windows.Forms.PictureBox GraphPic;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox PCBFanControlsGrpBox;
        private System.Windows.Forms.GroupBox trackingModeOffsetGroupBox;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ToolStripMenuItem testModeStandaloneToolStripMenuItem;
        private System.Windows.Forms.ComboBox ComPortSpeed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button getFanTempOffBtn;
        private System.Windows.Forms.Button setFanTempOffBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox fantempoffTxtBox;
        private System.Windows.Forms.TabControl mytabControl;
        private System.Windows.Forms.TabPage tabTemperature;
        private System.Windows.Forms.TabPage tabPower;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TabPage tabTracking;
        private System.Windows.Forms.TabPage tabLogging;
        private System.Windows.Forms.TabPage tabSerialPort;
        private System.Windows.Forms.TabPage tabFanAndPCB;
        private System.Windows.Forms.TabPage tabMisc;
        private System.Windows.Forms.Label ch3label;
        private System.Windows.Forms.Label ch3templabel;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label dplabel;
        private System.Windows.Forms.Label ch2label;
        private System.Windows.Forms.Label ch1label;
        private System.Windows.Forms.Label atlabel;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label ch2templabel;
        private System.Windows.Forms.TextBox ch1tempTxtBox;
        private System.Windows.Forms.Label ch1templabel;
        private System.Windows.Forms.TextBox ambientTemperatureTxtBox;
        private System.Windows.Forms.TextBox relativeHumidityTxtBox;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.GroupBox OffsetsGroupBox;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.TextBox ch3pwrTxtBox;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label ch3pwrlabel;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox ch2pwrTxtBox;
        private System.Windows.Forms.TextBox ch1pwrTxtBox;
        private System.Windows.Forms.Label ch2pwrlabel;
        private System.Windows.Forms.Label ch1pwrlabel;
        private System.Windows.Forms.GroupBox TemperatureGroupBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox PowerGroupBox;
        private System.Windows.Forms.Button reseterrorlogpathBtn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox writeEEPROMonExitChkBox;
        private System.Windows.Forms.Button resetControllertodefaultsBtn;
        private System.Windows.Forms.Button tesmodestandaloneBtn;
        private System.Windows.Forms.Button manualupdateBtn;
    }
}

