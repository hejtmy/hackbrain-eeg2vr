using System;
using SharpOSC;

namespace oscReceiver
{
    // event delegates
    public delegate void ActiveFocusUpDelegate(double eventData);
    public delegate void ActiveFocusDownDelegate(double eventData);
    public delegate void BrainExcitementLevelDelegate(double eventData);
    public delegate void AffectionLevelDelegate(double eventData);
    public delegate void VerbalExcitementLevelDelegate(double eventData);
    public delegate void VisualExcitementLevelDelegate(double eventData);

    public class EegOscReceiver
    {
        public static event ActiveFocusUpDelegate ActiveFocusUpEvent;
        public static event ActiveFocusDownDelegate ActiveFocusDownEvent;
        public static event BrainExcitementLevelDelegate BrainExcitementLevelEvent;
        public static event AffectionLevelDelegate AffectionLevelEvent;
        public static event VerbalExcitementLevelDelegate VerbalExcitementLevelEvent;
        public static event VisualExcitementLevelDelegate VisualExcitementLevelEvent;
        // private vars
        private int port;
        private UDPListener listener;
        // constructor
        public EegOscReceiver(int port)
        {
            this.port = port;
        }
        // callback for UDPListener inside StartReceiving
        private HandleOscPacket udpListenerCallback = delegate (OscPacket packet)
        {
            var msg = ((OscBundle)packet);
            for (int i = 0; i < msg.Messages.Count; i++)
            {
                string currentAddress = msg.Messages[i].Address;
                var currentArgument = msg.Messages[i].Arguments[0]; // possible error, assuming only one argument per address.
                double currentDoubleArgument;

                if (!Double.TryParse(currentArgument.ToString(), out currentDoubleArgument))
                {
                    // failed
                    throw new ArgumentException("Expected double got: " + currentArgument.GetType().ToString());
                }

                switch (currentAddress)
                {
                    case "up":
                        if (ActiveFocusUpEvent != null)
                            ActiveFocusUpEvent(currentDoubleArgument);
                        break;
                    case "down":
                        if (ActiveFocusDownEvent != null)
                            ActiveFocusDownEvent(currentDoubleArgument);
                        break;
                    case "weightedAlphaSum": // excitement
                        if (BrainExcitementLevelEvent != null)
                            BrainExcitementLevelEvent(currentDoubleArgument);
                        break;
                    case "frontalAlfaAsymetry": // positive reaction
                        if (AffectionLevelEvent != null)
                            AffectionLevelEvent(currentDoubleArgument);
                        break;
                    case "verbalCenterExcitement":
                        if (VerbalExcitementLevelEvent != null)
                            VerbalExcitementLevelEvent(currentDoubleArgument);
                        break;
                    case "visualCenterExcitement":
                        if (VisualExcitementLevelEvent != null)
                            VisualExcitementLevelEvent(currentDoubleArgument);
                        break;
                }
            }
        };

        public void StartReceiving()
        {
            listener = new UDPListener(port, udpListenerCallback);
        }
        public void StopReceiving()
        {
            listener.Close();
        }
    };
}
