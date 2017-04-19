using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Kinect;
using Timer = System.Threading.Timer;

namespace KinectTest
{
    public partial class Form1 : Form
    {
        private KinectSensor kinectSensor = null;
        private BodyFrameReader bodyFrameReader = null;
        private Body[] bodies = null;
        private Timer gestureTimer = null;
        private bool initialScan = false;
        private TimeSpan cycleSpan = new TimeSpan(0, 0, 0, 0, 250);
        private HandCoordinate handCoordinate = null;
        private bool isInMotion = false;
        private float changeSum = 0;
        public readonly double ErrorMargin = 0.03;
        private float rightHandCurrentX, rightHandCurrentY, rightHandCurrentZ = 0;
        private float rightThumbCurrentX, rightThumbCurrentY, rightThumbCurrentZ = 0;
        // Hand origin locations
        private float rightHandOriginX, rightHandOriginY, rightHandOriginZ = 0;


        public Form1()
        {
            InitializeComponent();
            InitializeKinect();
        }

        public void InitializeKinect()
        {
            kinectSensor = KinectSensor.GetDefault();
            if (kinectSensor != null)
            {
                kinectSensor.Open();
            }

            bodyFrameReader = kinectSensor.BodyFrameSource.OpenReader();

            if (bodyFrameReader != null)
            {
                bodyFrameReader.FrameArrived += Reader_FrameArrived;
            }
        }

        private void Reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataRecieved = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (bodies == null)
                    {
                        bodies = new Body[bodyFrame.BodyCount];
                    }

                    bodyFrame.GetAndRefreshBodyData(bodies);
                    dataRecieved = true;
                }

                if (dataRecieved)
                {
                    foreach (Body body in bodies)
                    {
                        if (body.IsTracked)
                        {                            

                            IReadOnlyDictionary<JointType, Joint> joints = body.Joints;
                            Dictionary<JointType, Point> jointPoints = new Dictionary<JointType, Point>();

                            Joint rightHand = joints[JointType.HandRight];
                            Joint leftHand = joints[JointType.HandLeft];
                            Joint rightThumb = joints[JointType.ThumbRight];

                            rightHandCurrentX = rightHand.Position.X;
                            rightHandCurrentY = rightHand.Position.Y;
                            rightHandCurrentZ = rightHand.Position.Z;

                            float lh_distance_x = leftHand.Position.X;
                            float lh_distance_y = leftHand.Position.Y;
                            float lh_distance_z = leftHand.Position.Z;

                            rightThumbCurrentX = rightThumb.Position.X;
                            rightThumbCurrentY = rightThumb.Position.Y;
                            rightThumbCurrentZ = rightThumb.Position.Z;

                            midSpineX.Text = rightHandCurrentX.ToString("#.##");
                            midSpineY.Text = rightHandCurrentY.ToString("#.##");
                            midSpineZ.Text = rightHandCurrentZ.ToString("#.##");

                            lhx.Text = rightThumbCurrentX.ToString("#.##");
                            lhy.Text = rightThumbCurrentY.ToString("#.##");
                            lhz.Text = rightThumbCurrentZ.ToString("#.##");

                            if (!initialScan)
                            {
                                gestureTimer = new Timer(x =>
                                {
                                    //Code for snapshots
                                    if (!isInMotion)
                                    {
                                        Console.WriteLine("dickerino");
                                        rightHandOriginX = rightHandCurrentX; // Get x of the hand for the motion
                                        rightHandOriginY = rightHandCurrentY;
                                        rightHandOriginZ = rightHandCurrentZ;
                                    }
                                    CheckMotionChange();

                                }, null, 0, cycleSpan.Milliseconds);

                                initialScan = true;
                            }

                            if (rightThumbCurrentX < rightHandCurrentX)
                            {
                                palmDownRadio.Checked = true;
                            }

                            if (rightThumbCurrentX > rightHandCurrentX)
                            {
                                palmUpRadio.Checked = true;
                            }
                        }
                    }
                }
            }
        }

        private void CheckMotionChange()
        {
            Console.WriteLine("x input: " + rightHandCurrentX + ", y input: " + rightHandCurrentY + ", z input: " + rightHandCurrentZ);
            //Console.WriteLine("xTest: " + xtest);
            //Console.WriteLine("thumbX: " + thumbX);
            if (handCoordinate != null) 
            {
                //Console.WriteLine("a");
                //Console.WriteLine(handCoordinate.x < xtest);
                //Console.WriteLine(Math.Abs(handCoordinate.x - x));
                //Console.WriteLine(Math.Abs(handCoordinate.y - y));
                //Console.WriteLine(Math.Abs(handCoordinate.z - z));
                if (handCoordinate.oldThumbX < rightThumbCurrentX && Math.Abs(handCoordinate.x - rightHandCurrentX) < ErrorMargin &&
                    Math.Abs(handCoordinate.y - rightHandCurrentY) < ErrorMargin && Math.Abs(handCoordinate.z - rightHandCurrentZ) < ErrorMargin)
                {
                    Console.WriteLine("b");
                    float xChange = Math.Abs(rightThumbCurrentX - handCoordinate.x);
                    if (xChange >= ErrorMargin)
                    {
                        Console.WriteLine("c");
                        isInMotion = true;
                        changeSum += xChange;
                    }
                    else
                    {
                        isInMotion = false;
                        changeSum = 0;
                    }
                }
                else
                {
                    isInMotion = false;
                    changeSum = 0;
                }
                
            }

            if (changeSum >= 0.08 && isInMotion)
            {
                Console.WriteLine("SUCCESS");
                sendMethod("Trigger reached - Turning on the drone!");
                changeSum = 0;
                isInMotion = false;
            }
            else
            {
                sendMethod("Waiting for trigger");
                Console.WriteLine(changeSum);
            }
            handCoordinate = new HandCoordinate(rightHandCurrentX, rightHandCurrentY, rightHandCurrentZ, rightThumbCurrentX);
        }

        void sendMethod(string text)
        {
            MethodInvoker mi = delegate {
                statusLabel.Text = text;
            };
            if (InvokeRequired)
                this.Invoke(mi);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }

    class HandCoordinate
    {

        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public float oldThumbX { get; set; }
        public HandCoordinate(float x, float y, float z, float oldThumbX)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.oldThumbX = oldThumbX;
        }

        
    }    
}
