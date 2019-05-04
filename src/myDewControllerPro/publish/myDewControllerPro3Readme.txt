myDewControllerPro3 Windows Application ChangeLog

// (c) Copyright Robert Brown 2014-2018. All Rights Reserved.
// The schematic, code and ideas for myFocuser are released into the public domain. Users are free to implement
// these but may NOT sell this project or projects based on this project for commercial gain without express 
// written permission granted from the author. Schematics, Code, Firmware, Ideas, Applications, Layout are 
// protected by Copyright Law.
// Permission is NOT granted to any person to redistribute, market, manufacture or sell for commercial gain 
// the myDewControllerPro3 products, ideas, circuits, builds, variations and units described or discussed 
// herein or on this site.
// Permission is granted for personal use only.

ONLY USE WITH VERSION 3 BOARDS
SERIAL PORT SPEED = 57600

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
// Support for PCB onboard temp/fan control if using firmware 3.18 or higher

// 3.0.4.0 29052017
// Support for Board temp sensor which regulates fan setting - Fritzing PCB Rev02 only, firmware v3.18 or higher
// Add menu setting to reset controller settings to default values

// 3.0.3.9
// Fix for possible connecting issue, command sequence changes
// Additional error messages
// Fix overlap of fanspeed and refreshrate controls on GUI
// If logging file path not set when user clicks DataLogging, then file path dialog is invoked
// When connecting, disable controls until last value is retrieved from controller
// Added logging of connection sequence time

// 3.0.3.8 24/11/2016
// Changes to variable names and error logging4
// Changes to connect code, setting ATBias
// Changes to ATBias code, do not trigger extra request when connecting and updating values
// Changes to Tracking Mode and FanSpeed, do not trigger extra request when connecting and updating values

// 3.0.3.6 10112016
// Revert to MS trackbar

// 3.0.3.5 18102016
// Remember form location

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
