﻿using System;
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
        private float rangeOfMotion = 0.08f;
        private float rightHandCurrentX, rightHandCurrentY, rightHandCurrentZ = 0;
        private float rightThumbCurrentX, rightThumbCurrentY, rightThumbCurrentZ = 0;
        private float leftHandCurrentX, leftHandCurrentY, leftHandCurrentZ = 0;
        private float leftThumbCurrentX, leftThumbCurrentY, leftThumbCurrentZ = 0;
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
                            Joint leftThumb = joints[JointType.ThumbLeft];

                            rightHandCurrentX = rightHand.Position.X;
                            rightHandCurrentY = rightHand.Position.Y;
                            rightHandCurrentZ = rightHand.Position.Z;

                            leftHandCurrentX = leftHand.Position.X;
                            leftHandCurrentY = leftHand.Position.Y;
                            leftHandCurrentZ = leftHand.Position.Z;

                            rightThumbCurrentX = rightThumb.Position.X;
                            rightThumbCurrentY = rightThumb.Position.Y;
                            rightThumbCurrentZ = rightThumb.Position.Z;

                            leftThumbCurrentX = leftThumb.Position.X;
                            leftThumbCurrentY = leftThumb.Position.Y;
                            leftThumbCurrentZ = leftThumb.Position.Z;

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
                                    bool isPalmUp;

                                    // TODO Change at 75% rather than 50%
                                    if (leftThumbCurrentX < leftHandCurrentX)
                                    {
                                        Console.WriteLine("Up");
                                        isPalmUp = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Down");
                                        isPalmUp = false;
                                    }
                                    //Code for snapshots
                                    //if (!isInMotion)
                                    //{
                                    //    rightHandOriginX = rightHandCurrentX; 
                                    //    rightHandOriginY = rightHandCurrentY;
                                    //    rightHandOriginZ = rightHandCurrentZ;
                                    //}

                                    if (isPalmUp)
                                    {
                                        CheckTurnOnMotion();
                                    }
                                    else
                                    {
                                        CheckTurnOffMotion();
                                    }
                                }, null, 0, cycleSpan.Milliseconds);

                                initialScan = true;
                            }

                            // Check which palm is facing up, currently not used
                            //if (rightThumbCurrentX < rightHandCurrentX)
                            //{
                            //    palmDownRadio.Checked = true;
                            //}

                            //if (rightThumbCurrentX > rightHandCurrentX)
                            //{
                            //    palmUpRadio.Checked = true;
                            //}
                        }
                    }
                }
            }
        }

        private void CheckTurnOnMotion()
        {
            if (handCoordinate != null)
            {
                if (handCoordinate.oldThumbX < rightThumbCurrentX &&
                    Math.Abs(handCoordinate.x - rightHandCurrentX) < ErrorMargin &&
                    Math.Abs(handCoordinate.y - rightHandCurrentY) < ErrorMargin &&
                    Math.Abs(handCoordinate.z - rightHandCurrentZ) < ErrorMargin)
                {
                    float xChange = Math.Abs(rightThumbCurrentX - handCoordinate.oldThumbX);
                    if (xChange >= ErrorMargin)
                    {
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

            if (changeSum >= rangeOfMotion && isInMotion) //TODO Calculate hand - thumb * 2?
            {
                Console.WriteLine("Turned on");
                sendMethod(statusLabel, "Trigger reached - Turning on the drone!");
                sendMethod(engineStateLabel, "On");
                changeSum = 0;
                isInMotion = false;
            }
            else
            {
                sendMethod(statusLabel, "Waiting for trigger");
            }
            handCoordinate = new HandCoordinate(rightHandCurrentX, rightHandCurrentY, rightHandCurrentZ,
                rightThumbCurrentX);
        }

        private void CheckTurnOffMotion()
        {
            if (handCoordinate != null)
            {
                if (handCoordinate.oldThumbX > rightThumbCurrentX &&
                    Math.Abs(handCoordinate.x - rightHandCurrentX) < ErrorMargin &&
                    Math.Abs(handCoordinate.y - rightHandCurrentY) < ErrorMargin &&
                    Math.Abs(handCoordinate.z - rightHandCurrentZ) < ErrorMargin)
                {
                    float xChange = Math.Abs(rightThumbCurrentX - handCoordinate.oldThumbX);
                    if (xChange >= ErrorMargin)
                    {
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

            if (changeSum >= rangeOfMotion && isInMotion) //TODO Calculate hand - thumb * 2?
            {
                Console.WriteLine("Turned off");
                //sendMethod("Trigger reached - Turning off the drone!");
                sendMethod(statusLabel, "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                sendMethod(engineStateLabel, "Off");
                changeSum = 0;
                isInMotion = false;
            }
            else
            {
                sendMethod(statusLabel, "Waiting for trigger");
            }
            handCoordinate = new HandCoordinate(rightHandCurrentX, rightHandCurrentY, rightHandCurrentZ,
                rightThumbCurrentX);
        }

        void sendMethod(Label label, string text)
        {
            MethodInvoker mi = delegate { label.Text = text; };
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