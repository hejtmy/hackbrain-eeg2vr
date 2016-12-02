/*
    Copyright 2016 chimera0 (email : kimera0kimaira@gmail.com URL:http://beat.accel-brain.com/)

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License, version 2, as
    published by the Free Software Foundation.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

using UnityEngine;
using UnityEditor;
using System.Collections;
using AccelBrain;

/// <summary>
/// Brains level upper editor.
/// </summary>
public class BrainsLevelUpperEditor : EditorWindow
{
    [MenuItem("Window/Brain's Level Upper")]
    static void Open()
    {
        EditorWindow.GetWindow<BrainsLevelUpperEditor>( "Brain's Level Upper" );
    }

    /// <summary>
    /// The beat type popup.
    /// </summary>
    private int _BeatTypePopup = 0;
    /// <summary>
    /// The rhythm popup.
    /// </summary>
    private int _RhythmPopup = 0;
    /// <summary>
    /// The left frequency slider.
    /// </summary>
    private int _LeftFrequencySlider = 400;
    /// <summary>
    /// The right frequency slider.
    /// </summary>
    private int _RightFrequencySlider = 430;
    /// <summary>
    /// The sample rate.
    /// </summary>
    private int _SampleRate = 44100;
    /// <summary>
    /// The gain.
    /// </summary>
    private float _Gain = 0.5f;

    /// <summary>
    /// The minimum frequency.
    /// </summary>
    private int _MinFrequency = 0;
    /// <summary>
    /// The max frequency.
    /// </summary>
    private int _MaxFrequency = 1000;

    /// <summary>
    /// The beat object.
    /// </summary>
    GameObject BeatObject;
    /// <summary>
    /// The name of the beat object.
    /// </summary>
    private string _BeatObjectName = "AccelBrainBeatEditorObject";

    /// <summary>
    /// Raises the GU event.
    /// </summary>
    void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Choose Binaural Beats or Monaural Beats.");
        this._BeatTypePopup = EditorGUILayout.Popup(
            "Beat Type: ",
            this._BeatTypePopup,
            new string[]{"Binaural Beat", "Monaural Beat"}
        );

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tune rhythms or frequencies to choice your favorite brain waves.");
        this._RhythmPopup = EditorGUILayout.Popup(
            "Rhythms: ",
            this._RhythmPopup,
            new string[]{
                "Not selected.",
                "Gamma Rhythms(30-100 Hz): To be the peak concentration.",
                "Alpha Rhythms(8-12 Hz): To be relaxed state.",
                "Theta Rhythms(4-7 Hz): To be the meditative state and focusing the mind.",
                "Delta Rhythms(1-3 Hz): To have a good sleep."
            }
        );
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        this._LeftFrequencySlider = EditorGUILayout.IntSlider(
            "Left Frequency (Hz): ",
            this._LeftFrequencySlider,
            this._MinFrequency,
            this._MaxFrequency
        );

        if (this._RhythmPopup == 1)
        {
            this._RightFrequencySlider = this._LeftFrequencySlider + 30;
        }
        else if (this._RhythmPopup == 2)
        {
            this._RightFrequencySlider = this._LeftFrequencySlider + 8;
        }
        else if (this._RhythmPopup == 3)
        {
            this._RightFrequencySlider = this._LeftFrequencySlider + 5;
        }
        else if (this._RhythmPopup == 4)
        {
            this._RightFrequencySlider = this._LeftFrequencySlider + 3;
        }

        this._RightFrequencySlider = EditorGUILayout.IntSlider(
            "Right Frequency (Hz): ",
            this._RightFrequencySlider,
            this._MinFrequency,
            this._MaxFrequency
        );

        EditorGUILayout.IntField(
            "Brain Wave (Hz): ",
            Mathf.Abs(this._LeftFrequencySlider - this._RightFrequencySlider)
        );

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Regulate the sample Rate. This value depends on your device.");
        this._SampleRate = EditorGUILayout.IntField(
            "Sample Rate: ",
            this._SampleRate
        );

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tune volumes. But it is recommended that the value is approximately 0.5.");
        this._Gain = EditorGUILayout.Slider(
            "Volumes",
            this._Gain,
            0.1f,
            1.0f
        );

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if(GUILayout.Button("Assign the object in the Hierarchy View."))
        {
            this._DeleteBeatObject();
            BeatController beatController = null;
            AudioListener audioListener = (AudioListener) FindObjectOfType(typeof(AudioListener));
            if (audioListener == null)
            {
                if (this._BeatTypePopup == 0 || this._BeatTypePopup == 1)
                {
                    if (this._BeatTypePopup == 0)
                    {
                        beatController = this._CreateBinauralBeatObject();
                    }
                    else
                    {
                        beatController = this._CreateMonauralBeatObject();
                    }
                }
            }
            else
            {
                if (this._BeatTypePopup == 0 || this._BeatTypePopup == 1)
                {
                    if (this._BeatTypePopup == 0)
                    {
                        bool attachFlag = EditorUtility.DisplayDialog(
                            "May I attach to existing GameObject ?",
                            "May I attach the object of Binaural Beats to the GameObject named " + audioListener.name + "? In the Hierarchy View, this GameObject is attached with AudioListener. Each scene can only have one Audio Listener.",
                            "OK",
                            "cancel"
                        );
                        if (attachFlag)
                        {
                            beatController = GameObject.Find(audioListener.name).AddComponent<BinauralBeatController>();
                        }
                    }
                    else
                    {
                        bool attachFlag = EditorUtility.DisplayDialog(
                            "May I attach to existing GameObject ?",
                            "May I attach the object of Monaural Beats to the GameObject named " + audioListener.name + "? In the Hierarchy View, this GameObject is attached with AudioListener. Each scene can only have one Audio Listener.",
                            "OK",
                            "cancel"
                        );
                        if (attachFlag)
                        {
                            beatController =GameObject.Find(audioListener.name).AddComponent<MonauralBeatController>();
                        }
                    }
                }
            }

            if (beatController != null)
            {
                beatController.LeftFrequency = (float)this._LeftFrequencySlider;
                beatController.RightFrequency = (float)this._RightFrequencySlider;
                beatController.SampleRate = (float)this._SampleRate;
                beatController.Gain = this._Gain;

                EditorUtility.DisplayDialog(
                    "Info",
                    "Successful completion!!",
                    "OK"
                );
            }
        }

        if(GUILayout.Button("Delete the object from the Hierarchy View."))
        {
            bool attachFlag = EditorUtility.DisplayDialog(
                "May I delete that?",
                "Is it really okay for me to delete this?",
                "OK",
                "cancel"
            );

            if (attachFlag)
            {
                this._DeleteBeatObject();
                EditorUtility.DisplayDialog(
                    "Info",
                    "Successful completion!!",
                    "OK"
                );
            }
        }
    }

    /// <summary>
    /// Creates the binaural beat object.
    /// </summary>
    /// <returns>The binaural beat object.</returns>
    private BeatController _CreateBinauralBeatObject()
    {
        GameObject beatObject = (GameObject)AssetDatabase.LoadAssetAtPath(
            "Assets/AccelBrainBeat/BinauralBeatObject.prefab",
            typeof(GameObject)
        );
        BeatObject = (GameObject)Instantiate (beatObject);
        BeatObject.name = this._BeatObjectName;
        BeatController beatController = BeatObject.GetComponent<BinauralBeatController>();
        return beatController;
    }

    /// <summary>
    /// Creates the monaural beat object.
    /// </summary>
    /// <returns>The monaural beat object.</returns>
    private BeatController _CreateMonauralBeatObject()
    {
        GameObject beatObject = (GameObject)AssetDatabase.LoadAssetAtPath(
            "Assets/AccelBrainBeat/MonauralBeatObject.prefab",
            typeof(GameObject)
        );
        BeatObject = (GameObject)Instantiate (beatObject);
        BeatObject.name = this._BeatObjectName;
        BeatController beatController = BeatObject.GetComponent<MonauralBeatController>();
        return beatController;
    }

    /// <summary>
    /// Deletes the beat object.
    /// </summary>
    private void _DeleteBeatObject()
    {
        GameObject beatObject = GameObject.Find(this._BeatObjectName);
        if (beatObject != null)
        {
            Object.DestroyImmediate(beatObject);
            return;
        }
        BinauralBeatController binauralBeatController = (BinauralBeatController) FindObjectOfType(typeof(BinauralBeatController));
        if (binauralBeatController != null)
        {
            Object.DestroyImmediate(GameObject.Find(binauralBeatController.name).GetComponent<BinauralBeatController>());
            return;
        }
        MonauralBeatController monauralBeatController = (MonauralBeatController) FindObjectOfType(typeof(MonauralBeatController));
        if (monauralBeatController != null)
        {
            Object.DestroyImmediate(GameObject.Find(monauralBeatController.name).GetComponent<MonauralBeatController>());
            return;
        }
    }
}
