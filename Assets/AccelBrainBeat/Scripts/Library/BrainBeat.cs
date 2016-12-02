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

using System;
using UnityEngine;

namespace AccelBrain
{
    /// <summary>
    /// An "AbstractClass" of the so-called "Template Method Pattern"
    /// to be able to take responsibility for intersection of binaural beat and monaural beat.
    /// </summary>
    abstract public class BrainBeat
    {
        /// <summary>
        /// Gets or sets the left frequency (Hz).
        /// </summary>
        /// <value>The left frequency.</value>
        public double LeftFrequency
        {
            set;
            get;
        }

        /// <summary>
        /// Gets or sets the right frequency (Hz).
        /// </summary>
        /// <value>The right frequency.</value>
        public double RightFrequency
        {
            set;
            get;
        }

        /// <summary>
        /// Gets or sets the gain.
        /// </summary>
        /// <value>The gain.</value>
        public double Gain
        {
            set;
            get;
        }

        /// <summary>
        /// The sample rate.
        /// </summary>
        private double _SampleRate = 44100;

        /// <summary>
        /// Gets or sets the sample rate.
        /// This is the mixer's current output rate.
        /// </summary>
        /// <value>The sample rate.</value>
        public double SampleRate
        {
            set
            {
                this._SampleRate = value;
            }
            get
            {
                return this._SampleRate;
            }
        }

        /// <summary>
        /// The left increment.
        /// </summary>
        protected double LeftIncrement;

        /// <summary>
        /// The right increment.
        /// </summary>
        protected double RightIncrement;

        /// <summary>
        /// The left phase.
        /// </summary>
        protected double LeftPhase;

        /// <summary>
        /// The right phase.
        /// </summary>
        protected double RightPhase;

        /// <summary>
        /// The play flag.
        /// true: playing;
        /// false: not playing;
        /// </summary>
        private bool _PlayFlag = false;

        /// <summary>
        /// Updates an array of floats comprising the audio data.
        /// There is difference between the binaural beat and monaural beat.
        /// The concrete method in "ConcreteClass" of so-called "Template Method Pattern"
        /// must take responsibility for it.
        /// </summary>
        /// <returns>The array of floats comprising the audio data.</returns>
        /// <param name="data">The array of floats comprising the audio data.</param>
        /// <param name="channels">An int that stores the number of channels of audio data passed to this delegate.</param>
        abstract public float[] UpdatePhase (float[] data, int channels);

        /// <summary>
        /// Audios the filter read.
        /// </summary>
        /// <returns>The array of floats comprising the audio data.</returns>
        /// <param name="data">The array of floats comprising the audio data.</param>
        /// <param name="channels">An int that stores the number of channels of audio data passed to this delegate.</param>
        public float[] AudioFilterRead(float[] data, int channels)
        {
            if (!this._PlayFlag)
            {
                return data;
            }
            else
            {
                return this.UpdatePhase(data, channels);
            }
        }

        /// <summary>
        /// Plaies the beat.
        /// </summary>
        public void PlayBeat()
        {
            this._PlayFlag = true;
        }

        /// <summary>
        /// Stops the beat.
        /// </summary>
        public void StopBeat()
        {
            this._PlayFlag = false;
        }
    }
}
