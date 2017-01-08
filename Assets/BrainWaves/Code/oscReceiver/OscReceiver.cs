using SharpOSC;
using System;
using System.Collections.Generic;

public class OscReceiver
{
    // private variables
    private UInt16 port;
    private bool isReceiving;
    private bool isPortSet;
    private UDPListener listener;

    // static needed to allow access from asynchronousCallback
    private static Dictionary<string, Action<OscBundle>> actionMap;

    // constructors
    public OscReceiver()
    {
        this.isPortSet = false;
        actionMap = new Dictionary<string, Action<OscBundle>>();
    }

    public OscReceiver(UInt16 port)
    {
        this.port = port;
        this.isPortSet = true;
        actionMap = new Dictionary<string, Action<OscBundle>>();
    }

    // private methods
    private HandleOscPacket asynchronousCallback = delegate (OscPacket packet)
    {
        OscBundle bundle = (OscBundle)packet;
        Action<OscBundle> action;
        if (actionMap.TryGetValue(bundle.Messages[0].Address, out action))
        {
            action(bundle);
        }
    };

    // public methods
    public bool SetPort(UInt16 port)
    {
        if (!this.isReceiving)
        {
            this.port = port;
            this.isPortSet = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public UInt16 GetPort()
    {
        return this.port;
    }

    public bool StartReceiving()
    {
        if (!this.isReceiving && this.isPortSet)
        {
            this.listener = new UDPListener(this.port, this.asynchronousCallback);
            this.isReceiving = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool StartReceiving(UInt16 port)
    {
        if (!this.isReceiving)
        {
            this.port = port;
            this.isPortSet = true;
            this.listener = new UDPListener(this.port, this.asynchronousCallback);
            this.isReceiving = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool StopReceiving()
    {
        if (this.isReceiving)
        {
            listener.Close();
            this.isReceiving = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsReceiving()
    {
        return this.isReceiving;
    }

    public bool IsPortSet()
    {
        return this.isPortSet;
    }

    public void AddAction(string address, Action<OscBundle> function)
    {
        actionMap.Add(address, function);
    }

    public void RemoveAction(string address)
    {
        actionMap.Remove(address);
    }

    public void ClearAllActions()
    {
        actionMap.Clear();
    }
};