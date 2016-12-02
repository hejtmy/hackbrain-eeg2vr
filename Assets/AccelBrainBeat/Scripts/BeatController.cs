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
using System.Collections;

namespace AccelBrain
{
    /// <summary>
    /// An "AbstractClass" of the so-called "Template Method Pattern"
    /// to be able to take responsibility for controller of binaural beat and monaural beat.
    /// </summary>
    public abstract class BeatController : MonoBehaviour
    {
        /// <summary>
        /// The left frequency.
        /// </summary>
        public double LeftFrequency = 400;
        /// <summary>
        /// The right frequency.
        /// </summary>
        public double RightFrequency = 430;
        /// <summary>
        /// The gain.
        /// </summary>
        public double Gain = 0.5;
        /// <summary>
        /// The sample rate.
        /// If this value is zero, get the mixer's current output rate.
        /// </summary>
        public double SampleRate = 44100;

        /// <summary>
        /// Gets or sets the brain beat.
        /// </summary>
        /// <value>The brain beat.</value>
        protected abstract BrainBeat _brainBeat
        {
            get;
            set;
        }

        /// <summary>
        /// Start this instance.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Play binaural beat.
        /// </summary>
        public void Play()
        {
            this._brainBeat.LeftFrequency = this.LeftFrequency;
            this._brainBeat.RightFrequency = this.RightFrequency;
            this._brainBeat.Gain = this.Gain;
            this._brainBeat.SampleRate = this.SampleRate;
            this._brainBeat.PlayBeat();
        }

        /// <summary>
        /// Stop binaural beat.
        /// </summary>
        public void Stop()
        {
            this._brainBeat.StopBeat();
            Play();
        }

        /// <summary>
        /// Raises the audio filter read event.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="channels">Channels.</param>
        void OnAudioFilterRead(float[] data, int channels)
        {
            if (this._brainBeat != null)
            {
                this._brainBeat.AudioFilterRead(data, channels);
            }
        }
    }
}
