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

                            float ms_distance_x = rightHand.Position.X;
                            float ms_distance_y = rightHand.Position.Y;
                            float ms_distance_z = rightHand.Position.Z;

                            float lh_distance_x = leftHand.Position.X;
                            float lh_distance_y = leftHand.Position.Y;
                            float lh_distance_z = leftHand.Position.Z;

                            float rt_distance_x = rightThumb.Position.X;
                            float rt_distance_y = rightThumb.Position.Y;
                            float rt_distance_z = rightThumb.Position.Z;

                            midSpineX.Text = ms_distance_x.ToString("#.##");
                            midSpineY.Text = ms_distance_y.ToString("#.##");
                            midSpineZ.Text = ms_distance_z.ToString("#.##");

                            lhx.Text = rt_distance_x.ToString("#.##");
                            lhy.Text = rt_distance_y.ToString("#.##");
                            lhz.Text = rt_distance_z.ToString("#.##");

                            if (!initialScan)
                            {
                                gestureTimer = new Timer(x =>
                                {
                                    //Code for snapshots
                                    CheckMotionChange(rt_distance_x, rt_distance_y, rt_distance_z);

                                }, null, 0, cycleSpan.Milliseconds);

                                initialScan = true;
                            }

                            if (rt_distance_x < ms_distance_x)
                            {
                                palmDownRadio.Checked = true;
                            }

                            if (rt_distance_x > ms_distance_x)
                            {
                                palmUpRadio.Checked = true;
                            }
                        }
                    }
                }
            }
        }

        private void CheckMotionChange(float x, float y, float z)
        {
            if (handCoordinate != null)
            {
                if (handCoordinate.x < x && handCoordinate.y - y < ErrorMargin && handCoordinate.z - z < ErrorMargin)
                {
                    if (x - handCoordinate.x >= ErrorMargin)
                    {
                        isInMotion = true;
                        changeSum += x - handCoordinate.x;
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
                statusLabel.Text = "Trigger reached - Turning on the drone!";
                changeSum = 0;
                isInMotion = false;
            }
            handCoordinate = new HandCoordinate(x, y, z);
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
        public HandCoordinate(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        
    }    
}
