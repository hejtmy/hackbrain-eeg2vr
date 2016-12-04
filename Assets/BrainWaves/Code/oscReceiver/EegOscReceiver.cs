using System;
using SharpOSC;

namespace oscReceiver
{
    // event delegates
    public delegate void HorizontalFocusDelegate(double distance, double probabilityLeft, double probabilityRight);
    public delegate void ConcentrationLevelDeleagte(double concentrationLevel);
    public delegate void GyroscopeXYDelegate(double coordinate);

    public class EegOscReceiver
    {
        public static event HorizontalFocusDelegate HorizontalFocusEvent;
        public static event ConcentrationLevelDeleagte ConcentrationLevelEvent;
        public static event GyroscopeXYDelegate GyroscopeXEvent;
        public static event GyroscopeXYDelegate GyroscopeYEvent;
        // private vars
        private int port;
        private bool isConnected;
        private UDPListener listener;
        // constructor
        public EegOscReceiver(int port)
        {
            this.port = port;
            this.isConnected = false;
        }
        // callback for UDPListener inside StartReceiving
        private HandleOscPacket udpListenerCallback = delegate (OscPacket packet)
        {
            var msg = ((OscBundle)packet);
            for (int i = 0; i < msg.Messages.Count; i++)
            {
                string currentAddress = msg.Messages[i].Address;
                var currentArgument = msg.Messages[i].Arguments[0]; // possible error, assuming only one argument per address.
                Console.WriteLine("callback called.");
                double currentDoubleArgument; if (!Double.TryParse(currentArgument.ToString(), out currentDoubleArgument)) // failed
                    throw new ArgumentException("Expected double got: " + currentArgument.GetType().ToString());
                switch (currentAddress)
                {
                    case "/horizontalFocus":
                        double distance1, distance2, probabilityLeft, probabilityRight;

                        if (!Double.TryParse(msg.Messages[0].Arguments[0].ToString(), out distance1))
                            throw new ArgumentException("Expected double got: " + msg.Messages[0].Arguments[0].GetType().ToString());
                        if (!Double.TryParse(msg.Messages[1].Arguments[0].ToString(), out distance2))
                            throw new ArgumentException("Expected double got: " + msg.Messages[1].Arguments[0].GetType().ToString());
                        if (!Double.TryParse(msg.Messages[2].Arguments[0].ToString(), out probabilityLeft))
                            throw new ArgumentException("Expected double got: " + msg.Messages[2].Arguments[0].GetType().ToString());
                        if (!Double.TryParse(msg.Messages[3].Arguments[0].ToString(), out probabilityRight))
                            throw new ArgumentException("Expected double got: " + msg.Messages[3].Arguments[0].GetType().ToString());

                        distance1 = (distance1 + distance2) / 2;
                        if (HorizontalFocusEvent != null)
                        {
                            // Console.WriteLine("distance: " + distance1 + ", probLeft: " + probabilityLeft + ", probRight: " + probabilityRight);
                            HorizontalFocusEvent(distance1, probabilityLeft, probabilityRight);
                        }
                        break;
                    case "/GYRO-X":
                        if (GyroscopeXEvent != null)
                            GyroscopeXEvent(currentDoubleArgument);
                        break;
                    case "/GYRO-Y":
                        if (GyroscopeYEvent != null)
                            GyroscopeYEvent(currentDoubleArgument);
                        break;
                }
            }
        };

        public void StartReceiving()
        {
            listener = new UDPListener(port, udpListenerCallback);
            isConnected = true;
        }
        public void StopReceiving()
        {
            listener.Close();
            isConnected = false;
        }
        public bool IsConnected()
        {
            return isConnected;
        }
    };
}
