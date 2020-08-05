using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using OpenCvSharp.Extensions;
using Flir.Atlas.Image;
using Flir.Atlas.Live.Device;
using Flir.Atlas.Live.Discovery;
using System.Configuration;
using Microsoft.Win32;
using System.Media;
using System.Net;

namespace ThermalScanningKiosk
{
    public partial class Form1 : Form
    {
        //Internal Declerations
        private Net ocvNet;
        private OpenCvSharp.VideoCapture ocvVideoStream;
        private Mat ocvFrame = new Mat();
        private Mat ocvBlob = new Mat();
        private System.Windows.Forms.Timer timerRefreshUi = new System.Windows.Forms.Timer();
        private Discovery atlasDiscovery;
        private int cameraCount = 0;
        private Camera atlasIRCamera;
        private Graphics irGraphic;
        private Flir.Atlas.Image.ImageBase irImageBase;
        private Bitmap irBitmap;
        private int ocvWebCameraIndex;
        private CameraDeviceInfo atlasIRCameraDeviceInfo;
        private bool debugMode = false;
        private bool scanResultsDisplayed = false;
        private int uiState = 0;
        private Rect ocvCompensation;
        private double tempScanResult, currentTempAvg;
        private bool scanComplete = false, thermalCalibrationComplete = false;
        private int scanStatus = 0; //0 = Not Started, 1 = Passed, -1 = Failed Once, -2 = Failed Twice
        private int faceDetections;
        private Stack<double> userPassThermalResults = new Stack<double>();
        private Queue<double> thermalResultHistory = new Queue<double>();
        private SoundPlayer spPass, spFail;
        private Scalar rectLineColor;
        private float detectionW1;
        private float detectionH1;
        private float detectionW2;
        private float detectionH2;
        private readonly OpenCvSharp.Size ocvSize = new OpenCvSharp.Size(300, 300);
        private readonly OpenCvSharp.Scalar ocvScalar = new Scalar(104.0, 177.0, 123.0);
        private Rectangle irAoI;
        private bool irImageChanged = false;
        //Startup Notice
        //This Startup Notice is required for compliance with the licensing of this application
        //This Startup Notice may only be modified in a manner consistent with the Terms of Use and License Agreement
        private int noticeTimer = 15;
        static readonly string noticeCameraTxt = "This kiosk will only work with a FLIR E8 or E8-XT Thermal Camera.";
        static readonly string noticeTxt =
            "The use of this kiosk with an approved FLIR device is intended for use \r\n" +
            "only as a skin temperature measuring tool and is not a medically-approved \r\n" +
            "body temperature measurement device or a medical diagnostic tool.";
        static readonly string noticeTxt2 =
            "The use of this kiosk is not a method for the determination or diagnosis \r\n" +
            "of a medical condition such as having COVID-19 or any other disease.  \r\n" +
            "It does not determine if the user has a fever.  This kiosk works by \r\n" +
            "comparing thermal readings from a current user with a running average \r\n" +
            "of readings from prior users.  If the current user’s readings are a \r\n" +
            "certain amount away from the average of prior users, the current user is \r\n" +
            "presented with a display indicating that further evaluation is required.  \r\n" +
            "Provisions should be made for measuring such a user’s elevated body \r\n" +
            "temperature with a device such as a non-contact infrared thermometer \r\n" +
            "or a clinical grade contact thermometer.";

        //Configuration Variables
        //Global
        static readonly string appTitle = ConfigurationManager.AppSettings.Get("appTitle");
        static readonly string orgName = ConfigurationManager.AppSettings.Get("orgName");
        static readonly string regKeyPath = "Software\\" + orgName + "\\ThermalScanningKiosk";
        static readonly bool logging = bool.Parse(ConfigurationManager.AppSettings.Get("logging"));
        static readonly string logPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + ConfigurationManager.AppSettings.Get("logPath");
        static readonly string logFilePrefix = ConfigurationManager.AppSettings.Get("logFilePrefix");
        
        //Thermal Scan
        static readonly double irHeightAoI = double.Parse(ConfigurationManager.AppSettings.Get("irHeightAoI"));
        static readonly double irWidthAoI = double.Parse(ConfigurationManager.AppSettings.Get("irWidthAoI"));
        private double preCalibrationAvg = double.Parse(ConfigurationManager.AppSettings.Get("preCalibrationAvg"));
        static readonly double highTempThreshold = double.Parse(ConfigurationManager.AppSettings.Get("highTempThreshold"));
        static readonly double lowTempThreshold = double.Parse(ConfigurationManager.AppSettings.Get("lowTempThreshold"));
        static readonly int requiredScanPasses = int.Parse(ConfigurationManager.AppSettings.Get("requiredScanPasses"));
        static readonly int runningAvgUserCount = int.Parse(ConfigurationManager.AppSettings.Get("runningAvgUserCount"));

        //Open CV
        static readonly double ocvFPS = double.Parse(ConfigurationManager.AppSettings.Get("ocvFPS"));
        static readonly int refreshInterval = (int)(1000 / ocvFPS);
        static readonly int ocvFrameWidth = int.Parse(ConfigurationManager.AppSettings.Get("ocvFrameWidth"));
        static readonly int ocvFrameHeight = int.Parse(ConfigurationManager.AppSettings.Get("ocvFrameHeight"));
        static readonly string ocvPrototxtPath = ConfigurationManager.AppSettings.Get("ocvPrototxtPath");
        static readonly string ocvCaffeModelPath = ConfigurationManager.AppSettings.Get("ocvCaffeModelPath");
        static readonly int ocvFacialDetectMinW = int.Parse(ConfigurationManager.AppSettings.Get("ocvFacialDetectMinW"));
        static readonly int ocvFacialDetectMinH = int.Parse(ConfigurationManager.AppSettings.Get("ocvFacialDetectMinH"));
        private double ocvFacialDetectMinCertainty = double.Parse(ConfigurationManager.AppSettings.Get("ocvFacialDetectMinCertainty"));

        //Image alignment compensation
        private int ocvCompX = int.Parse(ConfigurationManager.AppSettings.Get("ocvCompX"));
        private int ocvCompY = int.Parse(ConfigurationManager.AppSettings.Get("ocvCompY"));
        private int ocvCompW = int.Parse(ConfigurationManager.AppSettings.Get("ocvCompW"));
        private int ocvCompH = int.Parse(ConfigurationManager.AppSettings.Get("ocvCompH"));         
        private double ocvCompAD = double.Parse(ConfigurationManager.AppSettings.Get("ocvCompAD"));

        //User Interface
        private Color bannerColor = Color.FromArgb(0, 45, 157);
        static readonly string footerText = ConfigurationManager.AppSettings.Get("footerText");
        static readonly string greetingText = ConfigurationManager.AppSettings.Get("greetingText");
        static readonly string moveCloser1Text = ConfigurationManager.AppSettings.Get("moveCloser1Text");
        static readonly string moveCloser2Text = ConfigurationManager.AppSettings.Get("moveCloser2Text");
        static readonly string scanningText = ConfigurationManager.AppSettings.Get("scanningText");
        static readonly string onePersonText = ConfigurationManager.AppSettings.Get("onePersonText");
        static readonly string failScanText = ConfigurationManager.AppSettings.Get("failScanText");
        static readonly string passScanText = ConfigurationManager.AppSettings.Get("passScanText");
        static readonly string thermalCamNotReadyText = ConfigurationManager.AppSettings.Get("thermalCamNotReadyText");
        static readonly string searchingCamerasText = "Searching for FLIR E8 or E8-XT Camera";
        static readonly string connectingToWebCamText = ConfigurationManager.AppSettings.Get("connectingToWebCamText");
        static readonly string greetingImagePath = ConfigurationManager.AppSettings.Get("greetingImagePath");
        static readonly string passImagePath = ConfigurationManager.AppSettings.Get("passImagePath");
        static readonly string failImagePath = ConfigurationManager.AppSettings.Get("failImagePath");
        static readonly string logo1ImagePath = ConfigurationManager.AppSettings.Get("logo1ImagePath");
        static readonly string logo2ImagePath = ConfigurationManager.AppSettings.Get("logo2ImagePath");
        static readonly string logo3ImagePath = ConfigurationManager.AppSettings.Get("logo3ImagePath");
        static readonly string logo4ImagePath = ConfigurationManager.AppSettings.Get("logo4ImagePath");
        static readonly string passSoundPath = ConfigurationManager.AppSettings.Get("passSoundPath");
        static readonly string failSoundPath = ConfigurationManager.AppSettings.Get("failSoundPath");
        private Bitmap greetingImage;
        private Bitmap passImage;
        private Bitmap failImage;
        private Bitmap logo1Image;
        private Bitmap logo2Image;
        private Bitmap logo3Image;
        private Bitmap logo4Image;

        //Load the form
        public Form1()
        {
            InitializeComponent();
            noticeTimer = (int)(noticeTimer * ocvFPS);
            pbInfoGraphics.Visible = false;
            pbWebCam.Visible = false;
            lblCameraNotice.Text = noticeCameraTxt;
            lblNoticeTxt.Text = noticeTxt;
            lblNoticeTxt2.Text = noticeTxt2;
            pnlNotification.Visible = true;
            try {greetingImage = new Bitmap(greetingImagePath);}    catch { }
            try {passImage = new Bitmap(passImagePath);}            catch { }
            try {failImage = new Bitmap(failImagePath);}            catch { }
            try {logo1Image = new Bitmap(logo1ImagePath);}          catch { }
            try {logo2Image = new Bitmap(logo2ImagePath);}          catch { }
            try {logo3Image = new Bitmap(logo3ImagePath);}          catch { }
            try {logo4Image = new Bitmap(logo4ImagePath);}          catch { }
            readRegistryValues();
            initForm();
            //Load the Caffe Model for facial detection
            ocvNet = CvDnn.ReadNetFromCaffe(ocvPrototxtPath, ocvCaffeModelPath);
            ocvCompensation = new Rect(ocvCompX, ocvCompY, ocvCompW, ocvCompH);
            spPass = new SoundPlayer(passSoundPath);
            spFail = new SoundPlayer(failSoundPath);
            //Create the logging directory
            if (logging)
            {
                try
                {
                    System.IO.Directory.CreateDirectory(logPath);
                }
                catch
                {
                    //Failed to create the logging directory
                }
            }
            //initiate timer ticks
            timerRefreshUi.Interval = refreshInterval;
            timerRefreshUi.Tick += timerRefreshUi_Tick;
            timerRefreshUi.Start();
        }

        private void readRegistryValues()
        {
            //Values in registry will supersede app.config values
            try
            {
                RegistryKey _key = Registry.CurrentUser.OpenSubKey(@regKeyPath);
                if (_key != null)
                {
                    string[] _values = _key.GetValueNames();
                    foreach (string _value in _values)
                    {
                        switch (_value)
                        {
                            case "preCalibrationAvg":
                                preCalibrationAvg = double.Parse(_key.GetValue("preCalibrationAvg").ToString());
                                break;
                            case "ocvCompX":
                                ocvCompX = int.Parse(_key.GetValue("ocvCompX").ToString());
                                break;
                            case "ocvCompY":
                                ocvCompY = int.Parse(_key.GetValue("ocvCompY").ToString());
                                break;
                            case "ocvCompW":
                                ocvCompW = int.Parse(_key.GetValue("ocvCompW").ToString());
                                break;
                            case "ocvCompH":
                                ocvCompH = int.Parse(_key.GetValue("ocvCompH").ToString());
                                break;
                            case "ocvCompAD":
                                ocvCompAD = double.Parse(_key.GetValue("ocvCompAD").ToString());
                                break;
                            case "ocvFacialDetectMinCertainty":
                                ocvFacialDetectMinCertainty = double.Parse(_key.GetValue("ocvFacialDetectMinCertainty").ToString());
                                break;
                            default:
                                break;
                        }
                    }
                    _key.Close();
                    _key.Dispose();
                }
            }
            catch
            {
                //Failed to read the registry
            }
        }

        //Setup the applications initial User Interface
        private void initForm()
        {
            //Configure the initial form UI
            this.Text = appTitle;
            lblFooter.Text = footerText;
            pbLogo1.Image = logo1Image;
            pbLogo2.Image = logo2Image;
            pbLogo3.Image = logo3Image;
            pbLogo4.Image = logo4Image;
            pnlDebug.Visible = false;
            uiState = 0;
            setUIState();
            txtOCVCertainty.Text = ocvFacialDetectMinCertainty.ToString();
            txtX.Text = ocvCompX.ToString();
            txtY.Text = ocvCompY.ToString();
            txtW.Text = ocvCompW.ToString();
            txtH.Text = ocvCompH.ToString();
            txtAD.Text = ocvCompAD.ToString();
            lblVer.Text = "Version: " + System.Windows.Forms.Application.ProductVersion.ToString();
            lblHostName.Text = "Host: ";
            try{lblHostName.Text += System.Net.Dns.GetHostName();}
            catch { }
            lblIP.Text = "IP: " + GetLocalIPAddress();
        }

        //Get the local IP address of the PC
        public static string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && (!ip.ToString().StartsWith("169.")))
                    {
                        return ip.ToString();
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        //Exit the application
        //Disconnect and dispose cameras
        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            //If discovery is running
            if(atlasDiscovery != null)
            {
                try
                {
                    //Stop discovery and dispose it in a background thread
                    atlasDiscovery.Stop();
                    ThreadPool.QueueUserWorkItem(atlasDisposeDiscovery, atlasDiscovery);
                }
                catch
                {
                    //Failed to stop or dispose discovery
                }
            }

            //Disconnect and dispose cameras on form closing
            timerRefreshUi.Stop();
            if (atlasIRCamera != null)
            {
                try
                {
                    //Disconnect and dispose Thermal Camera
                    atlasDisconnectCamera();
                    atlasDisposeCamera();
                }
                catch
                {
                    //Failed to disconnect or dispose camera
                }
            }
            if (ocvVideoStream != null)
            {
                try
                {
                    //Disconnect and dispose WebCam
                    ocvVideoStream.Release();
                    ocvVideoStream.Dispose();
                    ocvVideoStream = null;
                }
                catch
                {
                    //Failed to disconnect or dispose camera
                }
            }
            
        }

        //Process a timer Tick
        //Frequency devined by ocvFPS (Open CV Frames Per Second) in app.config
        //Do not increase FPS above 30 as this will greatly impact performance
        void timerRefreshUi_Tick(object sender, EventArgs e)
        {
            //No faces have been detected for the current frame tick because the image has not yet been processed
            faceDetections = 0;

            //Display the Startup Notification at startup for the defined period of time
            if(noticeTimer > 0)
            {
                lblNoticeTimer.Text = ((int)(noticeTimer / ocvFPS)+1).ToString();
                //Notification Timer Countdown
                noticeTimer--;
                if (!pnlNotification.Visible)
                    pnlNotification.Visible = true;
                if (pbInfoGraphics.Visible)
                    pbInfoGraphics.Visible = false;
                if(pbWebCam.Visible)
                    pbWebCam.Visible = false;
            }
            else
            {
                if(pnlNotification.Visible)
                    pnlNotification.Visible = false;
                if (!pbInfoGraphics.Visible)
                    pbInfoGraphics.Visible = true;
                if (!pbWebCam.Visible)
                    pbWebCam.Visible = true;
            }

            //If the Web Cam stream is present
            if (ocvVideoStream != null)
            {
                //If the startup notice timer has expired
                if (noticeTimer <= 0)
                {
                    //Get a frame from the web camera
                    ocvVideoStream.Read(ocvFrame);
                    try
                    {
                        //Process the frame for facial detection
                        findFaces();
                        //get the bitmap from thr frame
                        Bitmap _image = ocvFrame.ToBitmap();
                        //Invert the frame for the user because it's a mirror image 
                        //And leaving it as a mirror makes user interaction difficult
                        _image.RotateFlip(RotateFlipType.Rotate180FlipY);
                        //display the frame in the web cam picture box
                        pbWebCam.Image = _image;
                    }
                    catch
                    { }
                }
                //If the atlas camera is present
                if (atlasIRCamera != null)
                {
                    //If in debug mode
                    if (debugMode && irBitmap != null)
                    {
                        //Display the thermal image
                        pbInfoGraphics.Image = irBitmap;
                        //Update the parameters when they are modified in debug mode
                        //Realtime updates will be displayed to the admin but need to be saved 
                        //Using the save button to stay persistant
                        try
                        {
                            ocvFacialDetectMinCertainty = double.Parse(txtOCVCertainty.Text);
                            ocvCompensation.X = int.Parse(txtX.Text);
                            ocvCompensation.Y = int.Parse(txtY.Text);
                            ocvCompensation.Width = int.Parse(txtW.Text);
                            ocvCompensation.Height = int.Parse(txtH.Text);
                            ocvCompensation.Height = int.Parse(txtH.Text);
                            ocvCompAD = double.Parse(txtAD.Text);
                        }
                        catch
                        { }
                    }
                }
                //Missing Thermal Camera
                //Serach for Cameras
                else
                {
                    uiState = 7;
                    setUIState();
                    //Dont start Discovery if it is already running
                    if (atlasDiscovery == null)
                    {
                        atlasStartDiscovery();
                    }
                }
            }
            //Missing Web Camera
            //Search for Cameras
            else
            {
                uiState = 10;
                setUIState();
                //Dont start Discovery if it is already running
                if (atlasDiscovery == null)
                {
                    atlasStartDiscovery();
                }
            }
        }

        //Togle Debug Mode
        private void pbLogo2_Click(object sender, EventArgs e)
        {
            //Toggle Debug Mode
            debugMode = !debugMode;
            pnlDebug.Visible = debugMode;
            setUIState();
        }

        //Save Settings from Debug Mode to Registry
        //These are HKEY Current User settings so they will not persist if different users login to the PC
        //These settings will override app.config file settings
        private void btnSave_Click(object sender, EventArgs e)
        {
            RegistryKey _key = Registry.CurrentUser.CreateSubKey(@regKeyPath);
            try
            {    
                ocvCompX = int.Parse(txtX.Text);
                ocvCompY = int.Parse(txtY.Text);
                ocvCompW = int.Parse(txtW.Text);
                ocvCompH = int.Parse(txtH.Text);
                ocvCompAD = double.Parse(txtAD.Text);
                ocvFacialDetectMinCertainty = double.Parse(txtOCVCertainty.Text);

                _key.SetValue("ocvFacialDetectMinCertainty", txtOCVCertainty.Text);
                _key.SetValue("ocvCompX", txtX.Text);
                _key.SetValue("ocvCompY", txtY.Text);
                _key.SetValue("ocvCompW", txtW.Text);
                _key.SetValue("ocvCompH", txtH.Text);
                _key.SetValue("ocvCompAD", txtAD.Text);
                ocvCompensation = new Rect(ocvCompX, ocvCompY, ocvCompW, ocvCompH);
            }
            catch
            { }
            _key.Close();
            _key.Dispose();
        }

        //Write events to log file if logging is enabled (enabled by default)
        //Base path is %ProgramData%
        //Subfolders and Logging Toggle can be set in app.config
        private void writeLogMsg(string _msg)
        {
            try
            {
                using (System.IO.StreamWriter _file =
                new System.IO.StreamWriter(logPath + "" + logFilePrefix + DateTime.Now.ToString("yyyy-MM-dd") + ".csv", true))
                {
                    _file.WriteLine(_msg);
                }
            }
            catch { }
        }

        #region Facial Detection
        //Perform Facial Detection on the current WebCam frame
        private void findFaces()
        {
            try
            {
                //Apply Image Compensation 
                ocvFrame = ocvFrame.Clone(ocvCompensation);
            }
            catch
            {
                //ocvCompensation is out of range or ocvFrame is null
                return;
            }
            ocvBlob = CvDnn.BlobFromImage(ocvFrame.Resize(ocvSize), 1, ocvSize, ocvScalar, true, false);
            ocvNet.SetInput(ocvBlob);
            Mat _detection = ocvNet.Forward();
            for (int i = 0; i < _detection.Size(3); i++)
            {
                //Find Open CV Confidence for Facial Detection against Caffe Model
                var confidence = _detection.At<float>(0, 0, i, 2);
                //If the Confidence is above the ocvFacialDetectMinCertainty then a face is detected
                //Lower the ocvFacialDetectMinCertainty to increase sensativity
                //Increase ocvFacialDetectMinCertainty to reduce sensativity
                //ocvFacialDetectMinCertainty below 0.20 is not recomended and will likely result in false detections
                if (confidence > ocvFacialDetectMinCertainty)
                {
                    faceDetections++;

                    detectionW1 = _detection.At<float>(0, 0, i, 3);
                    detectionH1 = _detection.At<float>(0, 0, i, 4);
                    detectionW2 = _detection.At<float>(0, 0, i, 5);
                    detectionH2 = _detection.At<float>(0, 0, i, 6);

                    int _cvWidth = ocvFrame.Width;
                    int _cvHeight = ocvFrame.Height;

                    int _ocvW1 = (int)(detectionW1 * _cvWidth);
                    int _ocvH1 = (int)(detectionH1 * _cvHeight);
                    int _ocvW2 = (int)(detectionW2 * _cvWidth);
                    int _ocvH2 = (int)(detectionH2 * _cvHeight);
                    int _width = (_ocvW2 - _ocvW1);
                    int _height = (_ocvH2 - _ocvH1);
                    
                    //Scanning has been completed - Only update video images but don't initiate additional scanning
                    if(scanComplete)
                    {
                        Rect _ocvRect = new Rect(_ocvW1, _ocvH1, _width, _height);
                        Cv2.Rectangle(ocvFrame, _ocvRect, rectLineColor, 8, LineTypes.Link8, 0);
                        getThermalImage(_height);
                        break;
                    }                   
                    //Qualified Face Detection - Initiate Scanning
                    if (faceDetections == 1 && _width >= ocvFacialDetectMinW && _height >= ocvFacialDetectMinH)
                    {
                        uiState = 4;
                        setUIState();
                        rectLineColor = Scalar.Blue;
                        Rect _ocvRect = new Rect(_ocvW1, _ocvH1, _width, _height);
                        Cv2.Rectangle(ocvFrame, _ocvRect, rectLineColor, 8, LineTypes.Link8, 0);
                        if (atlasIRCamera != null)
                        {
                            getThermalImage(_height);
                            if(irImageBase != null) thermalScanPass(irImageBase as ThermalImage);
                        }
                        else
                        {
                            uiState = 8;
                            setUIState();
                        }
                    }
                    //Face Detected but subject is too far away
                    if (faceDetections == 1 && (_width < ocvFacialDetectMinW || _height < ocvFacialDetectMinH))
                    {
                        if (_width >= ocvFacialDetectMinW * 0.90 && _height >= ocvFacialDetectMinH *0.90)
                            uiState = 3;
                        else
                            uiState = 2;
                        if (debugMode) getThermalImage();
                        setUIState();
                    }
                    //More than one face detected - Only one person at a time is allowed
                    if (faceDetections > 1)
                    {
                        if (debugMode) getThermalImage();
                        resetScan();
                        uiState = 9;
                        setUIState();
                        break;
                    }
                }
                //No faces detected
                if (faceDetections <= 0)
                {
                    if (debugMode) getThermalImage();
                    resetScan();
                    uiState = 0;
                    setUIState();
                }
            }
        }
#endregion

        #region Thermal Scan Functions

        //Get a Thermal Image from the Thermal Camera
        public void getThermalImage()
        {
            irImageBase = null;
            irBitmap = null;
            //If the thermal camera is present and connected
            //Then get a Thermal Image
            if (atlasIRCamera != null && atlasIRCamera.IsConnected)
            {
                atlasIRCamera.GetImage().EnterLock();
                irImageBase = atlasIRCamera.GetImage();
                atlasIRCamera.GetImage().ExitLock();
                if(irImageBase == null)
                {
                    resetScan();
                    if(atlasDiscovery == null)
                        atlasReConnectCamera();
                }
                else
                {
                    irBitmap = irImageBase.Image;
                }
            }
            //Thermal Camera is not connected
            //Then Reset the scan Reconnect the camera
            else if (atlasDiscovery == null)
            {
                resetScan();
                if (atlasDiscovery == null)
                    atlasReConnectCamera();
            }
        }

        //Apply Image Compensation and Area of Interest to the Thermal Image
        public void getThermalImage(int _height)
        {
            getThermalImage();
            //Check that the image is present
            if (irBitmap != null)
            {
                //Apply Image Compensation
                int _irWidth = irBitmap.Width;
                int _irHeight = irBitmap.Height;

                int _irW1 = (int)(detectionW1 * _irWidth);
                int _irH1 = (int)(detectionH1 * _irHeight);
                int _irW2 = (int)(detectionW2 * _irWidth);
                int _irH2 = (int)(detectionH2 * _irHeight);

                double _heightComp = (double)(((double)(_height - ocvFacialDetectMinH) / _height) * ocvCompAD);

                //Apply the Area of Interest
                irAoI = new Rectangle(_irW1, (int)(_irH1 - _heightComp), (int)((_irW2 - _irW1) * irWidthAoI), (int)((_irH2 - _irH1) * irHeightAoI));
                //If in Debug Mode
                //Then display the thermal image with Area of Interest
                if (debugMode)
                {
                    irGraphic = Graphics.FromImage(irBitmap);
                    irGraphic.DrawRectangle(new Pen(Color.White, 3), irAoI);
                }
            }
            //If the Thermal Camera is not Mounted
            //Then Reset the scan and reconnect the camera
            else if (atlasDiscovery == null)
            {
                resetScan();
                if (atlasDiscovery == null)
                    atlasReConnectCamera();
            }
        }

        //Process a scanning pass on the Thermal Image to find the highest temperature within the Area of Interest
        private void thermalScanPass(ThermalImage _irThermal)
        {
            //If the scan process is complete then don't process the thermal image
            if (scanComplete) return;
            //If the userPassThermalResults stack is empty then this is the first pass
            if (userPassThermalResults.Count == 0)
            {
                //If logging is enabled then log a start scan
                if (logging)
                {
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        writeLogMsg(DateTime.Now.ToString() + "," + scanStatus.ToString() + ", Starting Scan");
                    }).Start();
                }
            }
            //If the user stack does not have the required number of user results indicated in requiredScanPasses then process the image
            //Thermal results are pushed to the userPassThermalResults stack 
            if (userPassThermalResults.Count < requiredScanPasses)
            {
                _irThermal.TemperatureUnit = TemperatureUnit.Fahrenheit;
                _irThermal.DistanceUnit = DistanceUnit.Feet;
                userPassThermalResults.Push((double)_irThermal.GetValues(irAoI).Max());
            }
            //If the stack has the required number of results then process the stack and complete the user scanning session
            if (userPassThermalResults.Count >= requiredScanPasses)
                processThermalScan();
        }

        //User has completed the required number of thermal samples
        //Process the user results from the user stack 
        private void processThermalScan()
        {
            //If the scanning session is already complete then don't process the session again
            if (scanComplete) return;
            else
            {
                #region Calculate Baseline average
                //If the running average queue has the required number of previous session results
                //Then calculate the runnig baseline average
                if (thermalResultHistory.Count >= runningAvgUserCount)
                { 
                    //Take an average of the result queue
                    currentTempAvg = thermalResultHistory.Average();
                    //If this is the first time that the required number of user session results have been fulfiled 
                    //Then record the running average result to the registry for use when the kiosk is rebooted
                    if(!thermalCalibrationComplete)
                    {
                        RegistryKey _key = Registry.CurrentUser.CreateSubKey(@regKeyPath);
                        _key.SetValue("preCalibrationAvg", currentTempAvg.ToString("0.##"));
                        _key.Close();
                        _key.Dispose();
                    }
                    //Mark calibration complete so that the running average is not continually written to the registry
                    //Only the first calibration for the day is desired in the registry for use the next day
                    thermalCalibrationComplete = true;
                }
                //If this is the first scanning session of the day
                //Then load the default / precalibration value 
                //This will be the initial value configured in app.config 
                //Or the previous day's initial running average if it is available in the registry
                else if (thermalResultHistory.Count == 0)
                    currentTempAvg = preCalibrationAvg;
                //If some previous session results are availabe in the queue 
                //But not enough to meet the required number defined by runningAvgUserCount
                //Then use the default value and averegae it into the values available in the queue
                else if (thermalResultHistory.Count < runningAvgUserCount && thermalResultHistory.Count > 0)
                    currentTempAvg = ((preCalibrationAvg + thermalResultHistory.Average()) / 2);
                #endregion

                #region Calculate User Session
                if (userPassThermalResults.Count >= requiredScanPasses)
                {
                    //If the thermal image has not changed at all over all of the user samples
                    //Then the thermal camera image is likely frozen
                    //Reset the scanning session and disconnect / re-connect the Thermal Camera
                    if(!irImageChanged)
                    {
                        resetScan();
                        if (atlasDiscovery == null)
                            atlasReConnectCamera();
                    }

                    //Average the thermal samples in the userPassThermalResults stack
                    tempScanResult = userPassThermalResults.Average();

                    //User thermal results are within the acceptable deviation from the running baseline average
                    //Scanning session passed
                    if (tempScanResult <= currentTempAvg + highTempThreshold && tempScanResult >= currentTempAvg - lowTempThreshold)
                    {
                        //Add the results to the rolling baseline average queue
                        thermalResultHistory.Enqueue(tempScanResult);
                        //If the queue exceeds the required number of results for the rolling average
                        //Then remove the oldest value from the queue
                        if (thermalResultHistory.Count > runningAvgUserCount) thermalResultHistory.Dequeue();
                        //Passing Scan
                        scanStatus = 1;
                        //Scan Session Complete
                        scanComplete = true;
                        //Update the UI
                        uiState = 5;
                        setUIState();
                    }
                    //If the user is above the threshhold
                    if (tempScanResult > currentTempAvg + highTempThreshold || tempScanResult < currentTempAvg - lowTempThreshold)
                    {
                        scanStatus--;
                        //This is the users first failed screnning session
                        //Take a second set of samples to validate
                        if (scanStatus == -1)
                        {
                            scanComplete = false;
                            userPassThermalResults.Clear();
                            if (debugMode) lblInstructions.Text = "Starting Second Scan";
                        }
                        //User has failed two complete scanning session display failed status and
                        //Require further evaluation
                        else if (scanStatus == -2)
                        {
                            scanComplete = true;
                            uiState = 6;
                            setUIState();
                        }
                        //Something unexpected happened, Reset the entire scanning process and start again
                        else
                        {
                            resetScan();
                            uiState = 0;
                            setUIState();
                        }
                    }
                    //If logging is enabled, log the results
                    //User identifiable information is NOT recorded
                    if (logging)
                    {
                        new Thread(() =>
                        {
                            Thread.CurrentThread.IsBackground = true;
                            writeLogMsg(DateTime.Now.ToString() + "," + scanStatus.ToString() + "," + tempScanResult.ToString("0.##") + "," + currentTempAvg.ToString("0.##") + "," + highTempThreshold.ToString() + "," + lowTempThreshold.ToString());
                        }).Start();
                    }
                }
                #endregion
            }
        }

        //Reset the scanning session and clear user thermal results from memmory
        private void resetScan()
        {
            userPassThermalResults.Clear();
            scanStatus = 0;
            scanComplete = false;
            irImageBase = null;
            irBitmap = null;
            irImageChanged = false;
        }
        #endregion

        #region User Interface
        private void setUIState()
        {
            setUIState(uiState);
        }
        //Update the User Interface State
        private void setUIState(int _uiState)
        {
            switch (_uiState)
            {
                case 0:
                    //Greeting - Initial State
                    lblInstructions.ForeColor = Color.White;
                    lblInstructions.BackColor = bannerColor;
                    pnlBanner.BackColor = bannerColor;
                    lblInstructions.Text = greetingText;
                    if (!debugMode) pbInfoGraphics.Image = greetingImage;
                    rectLineColor = Scalar.Blue;
                    scanResultsDisplayed = false;
                    break;
                case 2:
                    //Move closer 1
                    lblInstructions.ForeColor = Color.White;
                    lblInstructions.BackColor = bannerColor;
                    pnlBanner.BackColor = bannerColor;
                    lblInstructions.Text = moveCloser1Text;
                    if (!debugMode) pbInfoGraphics.Image = greetingImage;
                    scanResultsDisplayed = false;
                    break;
                case 3:
                    //Move closer 2
                    lblInstructions.ForeColor = Color.White;
                    lblInstructions.BackColor = bannerColor;
                    pnlBanner.BackColor = bannerColor;
                    lblInstructions.Text = moveCloser2Text;
                    if (!debugMode) pbInfoGraphics.Image = greetingImage;
                    scanResultsDisplayed = false;
                    break;
                case 4:
                    //scanning
                    lblInstructions.ForeColor = Color.White;
                    lblInstructions.BackColor = bannerColor;
                    pnlBanner.BackColor = bannerColor;
                    lblInstructions.Text = scanningText;
                    rectLineColor = Scalar.Blue;
                    if (!debugMode) pbInfoGraphics.Image = greetingImage;
                    scanResultsDisplayed = false;
                    break;
                case 5:
                    //Passed scanning
                    if (!scanResultsDisplayed)
                    {
                        spPass.Play();
                        lblInstructions.ForeColor = Color.White;
                        lblInstructions.BackColor = Color.DarkGreen;
                        pnlBanner.BackColor = Color.DarkGreen;
                        rectLineColor = Scalar.DarkGreen;
                        lblInstructions.Text = lblInstructions.Text = debugMode ? "Passed " + tempScanResult.ToString("0.##") + "/" + currentTempAvg.ToString("0.##") + " (+" + highTempThreshold + ")" : passScanText;
                        scanResultsDisplayed = true;
                    }
                    if (!debugMode) pbInfoGraphics.Image = passImage;
                    break;
                case 6:
                    //Failed scanning
                    if (!scanResultsDisplayed)
                    {
                        spFail.Play();
                        lblInstructions.ForeColor = Color.White;
                        lblInstructions.BackColor = Color.IndianRed;
                        pnlBanner.BackColor = Color.IndianRed;
                        rectLineColor = Scalar.IndianRed;
                        lblInstructions.Text = lblInstructions.Text = debugMode ? "Failed " + tempScanResult.ToString("0.##") + "/" + currentTempAvg.ToString("0.##") + " (+" + highTempThreshold + ")" : failScanText;
                        scanResultsDisplayed = true;
                    }
                    if (!debugMode) pbInfoGraphics.Image = failImage;
                    break;
                case 7:
                    //Camera discovery
                    lblInstructions.ForeColor = Color.White;
                    lblInstructions.BackColor = bannerColor;
                    pnlBanner.BackColor = bannerColor;
                    lblInstructions.Text = searchingCamerasText;
                    pbInfoGraphics.Image = greetingImage;
                    scanResultsDisplayed = false;
                    break;
                case 8:
                    //Thermal camera not ready
                    lblInstructions.ForeColor = Color.White;
                    lblInstructions.BackColor = bannerColor;
                    pnlBanner.BackColor = bannerColor;
                    lblInstructions.Text = thermalCamNotReadyText;
                    pbInfoGraphics.Image = greetingImage;
                    scanResultsDisplayed = false;
                    break;
                case 9:
                    //One person at a time
                    lblInstructions.ForeColor = Color.White;
                    lblInstructions.BackColor = bannerColor;
                    pnlBanner.BackColor = bannerColor;
                    lblInstructions.Text = onePersonText;
                    if (!debugMode) pbInfoGraphics.Image = greetingImage;
                    scanResultsDisplayed = false;
                    break;
                case 10:
                    //Connecting to WebCam
                    lblInstructions.ForeColor = Color.White;
                    lblInstructions.BackColor = bannerColor;
                    pnlBanner.BackColor = bannerColor;
                    lblInstructions.Text = connectingToWebCamText;
                    pbInfoGraphics.Image = greetingImage;
                    scanResultsDisplayed = false;
                    break;
                default:
                    //Unknown State
                    lblInstructions.ForeColor = Color.White;
                    lblInstructions.BackColor = bannerColor;
                    pnlBanner.BackColor = bannerColor;
                    lblInstructions.Text = greetingText;
                    pbInfoGraphics.Image = greetingImage;
                    rectLineColor = Scalar.Blue;
                    scanResultsDisplayed = false;
                    break;
            }
        }
        #endregion

        #region Discover Cameras

        //Search for USB Cameras
        private void atlasStartDiscovery()
        {
            atlasDiscovery = new Discovery();
            atlasDiscovery.DeviceFound += atlasDiscovery_DeviceFound;
            atlasDiscovery.DeviceError += atlasDiscovery_DeviceError;
            atlasDiscovery.DeviceLost += atlasDiscovery_DeviceLost;
            //Discover USB Cameras
            atlasDiscovery.Start(Interface.Usb);
        }

        //Stop searching for cameras
        private void atlasStopDiscovery()
        {
            if (atlasDiscovery == null) return;
            atlasDiscovery.Stop();
            ThreadPool.QueueUserWorkItem(atlasDisposeDiscovery, atlasDiscovery);
        }

        //Dispose the Discovery Object
        private void atlasDisposeDiscovery(Object context)
        {
            atlasDiscovery.Dispose();
            atlasDiscovery = null;
        }

        //Discovery raised an Error
        void atlasDiscovery_DeviceError(object sender, DeviceErrorEventArgs e)
        {
            BeginInvoke((Action)(() => ShowError(e.ErrorMessage)));
        }

        //Display the Error
        private static void ShowError(string message)
        {
            MessageBox.Show(message);
        }

        //Discovery raised device lost event
        //USB camera was disconnected during discovery
        //Don't do anything, just keep searching for USB cameras
        void atlasDiscovery_DeviceLost(object sender, CameraDeviceInfoEventArgs e)
        {
            //Nothing to do
        }

        //A USB Camera was found process it through atlasAddDevice
        void atlasDiscovery_DeviceFound(object sender, CameraDeviceInfoEventArgs e)
        {
            BeginInvoke((Action)(() => atlasAddDevice(e.CameraDevice)));
        }
        
        //Process a camera to see if it should be mounted for use
        private void atlasAddDevice(CameraDeviceInfo _cameraDeviceInfo)
        {
            //Keep track of the number of cameras detected
            cameraCount++;
            //Cameras can have multiple streaming formats on a single USB Interface
            //Itterate through the available formats for the camera device
            foreach (var streamingFormat in _cameraDeviceInfo.StreamingFormats)
            {
                //If the camera is not a FLIR Camera and it supports ARGB
                //Then mount this as the Web Camera
                if ((!_cameraDeviceInfo.IsFlirCamera) && streamingFormat == Flir.Atlas.Live.Discovery.ImageFormat.Argb)
                {
                    //Check that a web camera has not already been mounted
                    if (ocvVideoStream == null)
                    {
                        //Connect the web camera for the live color images
                        ocvWebCameraIndex = cameraCount - 1;
                        ocvConnectCamera(ocvWebCameraIndex);
                    }
                }
                //If the Camera is a FLIR camera and supports the FLIR Thermal Image Format
                //Then Mount the Camera Device as the Thermal Camera
                if ((_cameraDeviceInfo.IsFlirCamera) && streamingFormat == Flir.Atlas.Live.Discovery.ImageFormat.FlirFileFormat)
                {
                    //Check that a thermal camera has not already been mounted
                    if (atlasIRCamera == null)
                    {
                        //Set the selected format to the FLIR Thermal Image format
                        _cameraDeviceInfo.SelectedStreamingFormat = Flir.Atlas.Live.Discovery.ImageFormat.FlirFileFormat;
                        //Connect the Thermal Camera
                        atlasIRCameraDeviceInfo = _cameraDeviceInfo;
                        atlasConnectCamera(atlasIRCameraDeviceInfo);
                    }
               }
            }
        }
        #endregion

        #region Connect and Disconnect Cameras

        //Connect the Web Camera Device
        private void ocvConnectCamera(int _cameraIndex)
        {
            ocvVideoStream = new VideoCapture(_cameraIndex);
            ocvVideoStream.Fps = ocvFPS;
            ocvVideoStream.Set(CaptureProperty.FrameWidth, ocvFrameWidth);
            ocvVideoStream.Set(CaptureProperty.FrameHeight, ocvFrameHeight);
            //If both the Web Camera and Thermal Camera have been mounted then stop Discovery
            if (atlasIRCamera != null && ocvVideoStream != null)
                atlasStopDiscovery();
        }

        //Connect the Thermal Camera
        private void atlasConnectCamera(CameraDeviceInfo _cameraDeviceInfo)
        {
            if (_cameraDeviceInfo.SelectedStreamingFormat == Flir.Atlas.Live.Discovery.ImageFormat.FlirFileFormat)
            {
                atlasIRCamera = new ThermalCameraDevice.ThermalCamera().Connect(_cameraDeviceInfo);
                if (atlasIRCamera == null)
                {
                    return;
                }
                else
                    atlasIRCamera.GetImage().Changed += irImage_Changed;
            }
            //If both the Web Camera and Thermal Camera have been mounted then stop Discovery
            if (atlasIRCamera != null && ocvVideoStream != null)
                atlasStopDiscovery();
        }

        //Disconnect and Reconnect the Thermal Camera
        private void atlasReConnectCamera()
        {
            timerRefreshUi.Stop();
            if (atlasIRCamera != null)
            {
                atlasIRCamera.Disconnect();
                atlasIRCamera.Dispose();
                atlasIRCamera = null;
            }
            atlasStartDiscovery();
            timerRefreshUi.Start();
        }

        //If the Thermal Image Changed Event has been raised then set irImageChanged
        //This tracks to ensure that the thermal image is updating and not frozen between user sessions
        void irImage_Changed(object sender, Flir.Atlas.Image.ImageChangedEventArgs e)
        {
            irImageChanged = true;
        }

        //Disconnect the Thermal Camera
        private void atlasDisconnectCamera()
        {
            if (atlasIRCamera == null) return;
            atlasIRCamera.Disconnect();
        }

        //Dispose the Thermal Camera Object
        private void atlasDisposeCamera()
        {
            if (atlasIRCamera == null) return;
            atlasIRCamera.Dispose();
            atlasIRCamera = null;
        }
        #endregion
    }
}
