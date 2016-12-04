using oscReceiver;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class _brain_SceneManager : MonoBehaviour
{
    public GameObject Player;

    public List<_brain_Object> Pyramids;

    public _brain_DirLight DirLight1;
    public _brain_DirLight DirLight2;
    public _brain_DirLight PointLight1;

    public bvr_Animator Anim1;
    public bvr_Listener Listener;

    public bvr_PostProcessing PostProcessing;

    public _brain_OpenVibeSettings OpenVibeSettings;
    public _brain_ColourScheme ColourScheme;
    public bvr_SceneSettings SceneSettigns;

    private bool _colourDone = false;

    public _brain_Object Alchemy;

    private bool _thinkingDown;
    private bool _thinkingUp;

    //Raycasting helper
    Vector2 _centerPosition;

    //helping structures
    private bool _rotating = false;
    private bool _moving = false;

    // Use this for initialization
    void Start()
    {
        _centerPosition = new Vector2(Screen.width / 2, Screen.height / 2);
        Subscribe();
    }

    void Subscribe()
    {
        if (Listener == null) return;
        Listener.AlfaChanged += AlfaChanged;
        //EegOscReceiver.ActiveFocusUpEvent += FocusIsUp;
        //EegOscReceiver.ActiveFocusDownEvent += FocusIsDown;
        EegOscReceiver.HorizontalFocusEvent += HorizontalThinking;
    }

    #region Subscription to Open Vibe
    private void AlfaChanged(double value)
    {
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

        _thinkingDown = false;
        _thinkingUp = true;
    }

    private void ThinkingDown(double value)
    {
        if ((float)value < OpenVibeSettings.thinkingDownThreshold) return;
        Debug.Log("ThinkginDown");
        _thinkingDown = true;
        _thinkingUp = false;
    }

    private void HorizontalThinking(double direction, double probLeft, double probRight)
    {      
        if (probLeft - probRight < 0.4) return;
        if (probRight - probLeft > 0) ThinkingUp(100);
        else ThinkingDown(100);
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
        if (!Listener.IsConnected()) return;
        //rotation of the object
        if (_rotating)
        {
            foreach (var obj in Pyramids)
                obj.Rotate(new Vector3(0, 1, 0), 100);
        }
        else
            foreach (var obj in Pyramids)
                obj.StopRotating();
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
            DirLight1.ChangeColor(ColourScheme.scheme1_primary, SceneSettigns.FocusChangeSpeed);
            DirLight2.ChangeColor(ColourScheme.scheme1_primary, SceneSettigns.FocusChangeSpeed);
            PostProcessing.LerpVignette(SceneSettigns.VignetteFocused, SceneSettigns.FocusChangeSpeed);
            foreach (var obj in Pyramids)
                obj.Rotate(new Vector3(0, 1, 0), 100);
            foreach (var obj in Pyramids)
                obj.MoveBack(3);
        }
        if (Input.GetKeyDown("b"))
        {
            Anim1.SpeedUp(SceneSettigns.AlchemySpeed, SceneSettigns.FocusChangeSpeed);
            PostProcessing.LerpVignette(SceneSettigns.VignetteRelaxed, SceneSettigns.FocusChangeSpeed);
            foreach (var obj in Pyramids)
                obj.MoveAway(3);
            DirLight1.ChangeColor(ColourScheme.scheme2_primary, 2);
            DirLight2.ChangeColor(ColourScheme.scheme2_primary, 2);

        }

        if (_thinkingUp)
        {
            Alchemy.Resize(2, SceneSettigns.ThinkingUpDownSpeed);
            Anim1.SpeedUp(SceneSettigns.AlchemySpeed, SceneSettigns.FocusChangeSpeed);
            PostProcessing.LerpVignette(SceneSettigns.VignetteRelaxed, SceneSettigns.FocusChangeSpeed);
            foreach (var obj in Pyramids)
                obj.MoveAway(3);
            DirLight1.ChangeColor(ColourScheme.scheme2_primary, 2);
            DirLight2.ChangeColor(ColourScheme.scheme2_primary, 2);
        }

        if (_thinkingDown)
        {
            DirLight1.ChangeColor(ColourScheme.scheme1_primary, SceneSettigns.FocusChangeSpeed);
            DirLight2.ChangeColor(ColourScheme.scheme1_primary, SceneSettigns.FocusChangeSpeed);
            PostProcessing.LerpVignette(SceneSettigns.VignetteFocused, SceneSettigns.FocusChangeSpeed);
            foreach (var obj in Pyramids)
                obj.Rotate(new Vector3(0, 1, 0), 100);
            foreach (var obj in Pyramids)
                obj.MoveBack(3);
        }
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
