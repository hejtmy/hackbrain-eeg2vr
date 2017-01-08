using SharpOSC;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using System;

public class bvr_SceneManager : MonoBehaviour
{
    public GameObject Player;

    public List<bvr_Object> Pyramids;

    public bvr_DirLight DirLight1;
    public bvr_DirLight DirLight2;
    public bvr_DirLight PointLight1;

    public bvr_Animator Anim1;
    bvr_Listener Listener;

    public bvr_PostProcessing PostProcessing;

    public bvr_OpenVibeSettings OpenVibeSettings;
    public bvr_ColourScheme ColourScheme;
    public bvr_SceneSettings SceneSettigns;

    private bool _colourDone = false;

    public bvr_Object Alchemy;

    private bool _thinkingDown;
    private bool _thinkingUp;

    //Raycasting helper
    Vector2 _centerPosition;

    //helping structures
    private bool _rotating = true;
    private bool _moving = false;

    private bool _previousConcentrated = false;
    private bool _concentrated = false;

    // Use this for initialization
    void Start()
    {
        Listener = bvr_Listener.Get;
        _centerPosition = new Vector2(Screen.width / 2, Screen.height / 2);
        Subscribe();
        foreach (var obj in Pyramids)
            obj.Rotate(new Vector3(0, 1, 0), 100);
        DirLight1.Rotate(new Vector3(0, 1, 0), 5);
        Anim1.PlayAnimation();
    }

    void Subscribe()
    {
        if (Listener == null) return;
        Listener.Receiver.AddAction("/alpha", AlfaChanged);
        Listener.Receiver.AddAction("/horizontalFocus", HorizontalThinking);
        Listener.Receiver.AddAction("/GyroscopeX", Concentration);
        Listener.Receiver.AddAction("/GyroscopeY", ThetaAction);
    }

    private double parseDoubleFromString(string s)
    {
        double outputDouble;

        if (!Double.TryParse(s, out outputDouble))
        {
            throw new InvalidCastException("Not able to parse double from input string.");
        }
        return outputDouble;
    }

    private void ThetaAction(OscBundle data)
    {
        double coordinate = parseDoubleFromString(data.Messages[0].Arguments[0].ToString());
        Debug.Log("Theta:" +  coordinate);
    }

    #region Subscription to Open Vibe
    private void AlfaChanged(OscBundle data)
    {
        double value = parseDoubleFromString(data.Messages[0].Arguments[0].ToString());
        if (value > 10) _rotating = true;
        else _rotating = false;
    }

    private void FocusIsUp(double value)
    {
        if ((float)value < OpenVibeSettings.focusUpThreshold) return;
        DirLight1.ChangeColor(ColourScheme.scheme1_primary, SceneSettigns.FocusChangeSpeed);
        Anim1.SpeedUp(SceneSettigns.AlchemySpeed, SceneSettigns.FocusChangeSpeed);
    }

    private void FocusIsDown(double value)
    {
        if ((float)value > OpenVibeSettings.focusDownThreshold) return;
        DirLight1.ChangeColor(ColourScheme.scheme2_primary, SceneSettigns.FocusChangeSpeed);
        Anim1.SlowDown(SceneSettigns.FocusChangeSpeed);
    }

    private void ThinkingUp(double value)
    {
        if ((float)value < OpenVibeSettings.thinkingUpThreshold) return;
        Debug.Log("ThinkginUp");
        _concentrated = true;
    }

    private void ThinkingDown(double value)
    {
        if ((float)value < OpenVibeSettings.thinkingDownThreshold) return;
        Debug.Log("ThinkginDown");
        _concentrated = false;
    }

    private void HorizontalThinking(OscBundle data)
    {      
        parseDoubleFromString(data.Messages[0].Arguments[0].ToString());

        throw new NotImplementedException("Need to fix indexes in arguments of parseDoubleFromString function according to OscBundle data.");

        double direction = parseDoubleFromString(data.Messages[0].Arguments[0].ToString());
        double probLeft = parseDoubleFromString(data.Messages[0].Arguments[0].ToString());
        double probRight = parseDoubleFromString(data.Messages[0].Arguments[0].ToString());

        if (probLeft - probRight < 0.4) return;
        if (probRight - probLeft > 0) ThinkingUp(100);
        else ThinkingDown(100);
    }

    private void Concentration(OscBundle data)
    {
        double value = parseDoubleFromString(data.Messages[0].Arguments[0].ToString());

        Debug.Log(value);
        if (value > 1) ThinkingUp(100);
        if (value < 1) ThinkingDown(100);
    }

    #endregion
    // Update is called once per frame
    void Update()
    {
        KeyInput();
        Raycasing();
        EEGActions();
    }

    private void EEGActions()
    {

        if (!Listener || !Listener.IsConnected()) return;
        //rotation of the object
        if (_previousConcentrated != _concentrated)
        {
            if (_concentrated) DoThinkingUp();
            else DothinkingDown();
        }
        _previousConcentrated = _concentrated;
    }

    private void KeyInput()
    {
        if (Input.GetKeyDown("r"))
        {
            foreach (var obj in Pyramids)
                obj.Rotate(new Vector3(0, 1, 0), 100);
        }
        if (Input.GetKeyDown("u")) DirLight1.ChangeColor(ColourScheme.scheme2_primary, 2);
        if (Input.GetKeyDown("y")) DirLight1.ChangeColor(ColourScheme.scheme1_primary, 2);
        if (Input.GetKeyDown("o")) Alchemy.Resize(2, SceneSettigns.ThinkingUpDownSpeed);
        if (Input.GetKeyDown("l")) Anim1.SlowDown(SceneSettigns.FocusChangeSpeed);
        if (Input.GetKeyDown("k")) Anim1.SpeedUp(3, SceneSettigns.FocusChangeSpeed); ;
        if (Input.GetKeyDown("p")) DirLight1.Rotate(new Vector3(0, 1, 0), 5);
        if (Input.GetKeyDown("t")) Anim1.PlayAnimation();
        if (Input.GetKeyDown("m"))
        {
            foreach (var obj in Pyramids)
                obj.MoveAway(3);
        }
        if (Input.GetKeyDown("n"))
        {
            _concentrated = true;
        }
        if (Input.GetKeyDown("b"))
        {
            _concentrated = false;
        }
        
    }

    void DoThinkingUp()
    {
        DirLight1.ChangeColor(ColourScheme.scheme1_primary, SceneSettigns.FocusChangeSpeed);
        DirLight2.ChangeColor(ColourScheme.scheme1_primary, SceneSettigns.FocusChangeSpeed);
        PostProcessing.LerpVignette(SceneSettigns.VignetteFocused, SceneSettigns.FocusChangeSpeed);
        Anim1.SpeedUp(3, SceneSettigns.FocusChangeSpeed);
        foreach (var obj in Pyramids)
            obj.MoveAway(3);
    }

    void DothinkingDown()
    {
        DirLight1.ChangeColor(ColourScheme.scheme2_primary, SceneSettigns.FocusChangeSpeed);
        DirLight2.ChangeColor(ColourScheme.scheme2_primary, SceneSettigns.FocusChangeSpeed);
        PostProcessing.LerpVignette(SceneSettigns.VignetteRelaxed, SceneSettigns.FocusChangeSpeed);
        Anim1.SlowDown(SceneSettigns.FocusChangeSpeed);
        foreach (var obj in Pyramids)
            obj.MoveBack(3);
    }

    void Raycasing()
    {
        Ray ray = Player.GetComponentInChildren<Camera>().ScreenPointToRay(_centerPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            if (hit.collider.tag == "BrainObject") { }
                //Object1 = hit.collider.gameObject.GetComponent<_brain_Object>();
    }

    #region PostProcessing changes

    #endregion
}
