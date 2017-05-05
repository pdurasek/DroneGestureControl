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
      private float rangeOfMotion = 0.08f;
      private float rightHandCurrentX, rightHandCurrentY, rightHandCurrentZ = 0;
      private float rightThumbCurrentX, rightThumbCurrentY, rightThumbCurrentZ = 0;
      private float leftHandCurrentX, leftHandCurrentY, leftHandCurrentZ = 0;
      private float leftThumbCurrentX, leftThumbCurrentY, leftThumbCurrentZ = 0;
      private float rightElbowCurrentX, rightElbowCurrentY, rightElbowCurrentZ;
      // Hand origin locations
      private float rightHandOriginX, rightHandOriginY, rightHandOriginZ = 0;
      private int sequence = 0;
      private int currentCycle = 0;

      private double distance = 0;
      private double error = 0.1;

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
                     Joint rightElbow = joints[JointType.ElbowRight];

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

                     rightElbowCurrentX = rightElbow.Position.X;
                     rightElbowCurrentY = rightElbow.Position.Y;
                     rightElbowCurrentZ = rightElbow.Position.Z;

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
                           //bool isPalmUp;

                           //        // TODO Change at 75% rather than 50%
                           //        if (leftThumbCurrentX < leftHandCurrentX)
                           //{
                           //   Console.WriteLine("Up");
                           //   isPalmUp = true;
                           //}
                           //else
                           //{
                           //   Console.WriteLine("Down");
                           //   isPalmUp = false;
                           //}
                           //Code for snapshots
                           //if (!isInMotion)
                           //{
                           //    rightHandOriginX = rightHandCurrentX; 
                           //    rightHandOriginY = rightHandCurrentY;
                           //    rightHandOriginZ = rightHandCurrentZ;
                           //}

                           //if (isPalmUp)
                           //{
                           //   CheckTurnOnMotion();
                           //}
                           //else
                           //{
                           //   CheckTurnOffMotion();
                           //}
                           //KonCha();
                           Shokugeki();
                        }, null, 0, cycleSpan.Milliseconds);

                        double distX = Math.Pow(rightHandCurrentX - rightElbowCurrentX, 2);
                        double distY = Math.Pow(rightHandCurrentY - rightElbowCurrentY, 2);
                        double distZ = Math.Pow(rightHandCurrentZ - rightElbowCurrentZ, 2);
                        distance = Math.Sqrt(distX + distY + distZ);

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

      private void Shokugeki()
      {
         if (rightThumbCurrentX > rightHandCurrentX)
         {
            Console.WriteLine("Up");
         }
         else if (rightThumbCurrentX < rightHandCurrentX)
         {
            Console.WriteLine("Down");
         }

         if (rightHandCurrentX > rightElbowCurrentX + distance - error)
         {
            Console.WriteLine("KonCha Right");
         }
         else if (rightElbowCurrentX > rightHandCurrentX + distance - error)
         {
            Console.WriteLine("KonCha Left");
         }
      }

      //private void KonCha()
      //{
      //   if (handCoordinate != null)
      //   {
      //      currentCycle++;

      //      if (currentCycle > 1)
      //      {
      //         if (rightElbowCurrentY < rightHandCurrentY && rightElbowCurrentX > rightHandCurrentX + 0.1)
      //         {
      //            if (sequence % 2 == 0)
      //            {
      //               sequence++;
      //               currentCycle = 0;
      //            }
      //            else
      //            {
      //               sequence = 0;
      //            }
      //         }
      //         else if (rightElbowCurrentY < rightHandCurrentY && rightElbowCurrentX < rightElbowCurrentX + 0.1)
      //         {
      //            if (sequence % 2 == 1)
      //            {
      //               sequence++;
      //               currentCycle = 0;
      //            }
      //            else
      //            {
      //               sequence = 0;
      //            }
      //         }
      //      }
      //   }

      //   if (sequence >= 6)
      //   {
      //      sequence = 0;
      //      Console.WriteLine("Konnichiwa");
      //   }

      //   handCoordinate = new HandCoordinate(rightHandCurrentX, rightHandCurrentY, rightHandCurrentZ,
      //       rightThumbCurrentX);
      //}

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