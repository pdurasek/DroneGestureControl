﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
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
        private TimeSpan cycleSpan = new TimeSpan(0, 0, 0, 0, 200);
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

        SerialPort port = new SerialPort("COM4", 9600);

        public Form1()
        {
            InitializeComponent();
            InitializeKinect();
            port.Open();
            //port.DataReceived += (usb, a) =>
            //{
            //    byte[] buffer = new byte[port.BytesToRead];
            //    port.Read(buffer, 0, port.BytesToRead);
            //    Console.WriteLine(Encoding.UTF8.GetString(buffer));
            //};            
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
                                    //port.Write("[" + rightHandCurrentX + "," + rightHandCurrentY + "," + rightHandCurrentZ + "]");
                                    calculado();
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

        private void calculado()
        {
            double currentX = rightHandCurrentX * 10;
            double currentY = rightHandCurrentY * 10;
            double currentZ = rightHandCurrentZ * 10;
            double r = Math.Sqrt(Math.Pow(rightHandCurrentX, 2) + Math.Pow(rightHandCurrentY, 2) + Math.Pow(rightHandCurrentZ, 2)) * 10;
            double bazniKut = Math.Acos(currentZ/r) *100;
            double lakat = Math.Acos(currentY/r);
            double rotacija = Math.Atan(currentX/r) * 100;
            //Console.WriteLine("R: " + r);
            //Console.WriteLine("Curent X: " + currentX + ", Current Y:" + currentY + ", Current Z: " + currentZ);
            //Console.WriteLine("bazni: " + bazniKut);
            //Console.WriteLine("lakat: " + lakat);
            //Console.WriteLine("rotacija: " + rotacija);
            //port.Write("[" + (int)rotacija + "," + (int)bazniKut + "," + (int)lakat + "]");
           
            //Console.WriteLine(MapValue(-40, 40, 15, 155, currentY));
            lakat = MapValue(-3.5, 9, 80, 170, currentY, 80, 170);
            
            bazniKut = MapValue(10, 19, 150, 0, currentZ, 0, 150);
            Console.WriteLine("bazni kut: " + bazniKut);
            rotacija = MapValue(-4.5, 4.5, 155, 15, currentX, 15, 155);
            Console.WriteLine("[" + (int)rotacija + "," + (int)bazniKut + "," + (int)lakat + "]");
            port.Write("[" + (int)rotacija + "," + 90 + "," + 180 + "]");
        }

        public double MapValue(double a0, double a1, double b0, double b1, double a, int motorMin, int motorMax)
        {
            double rotus = b0 + (b1 - b0) * ((a - a0) / (a1 - a0));

            if (rotus < motorMin)
            {
                rotus = motorMin;
            }
            if (rotus > motorMax)
            {
                rotus = motorMax;
            }

            return rotus;
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