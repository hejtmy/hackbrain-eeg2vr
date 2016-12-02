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

namespace AccelBrain
{
    /// <summary>
    /// An "ConcreteClass" of the so-called "Template Method Pattern"
    /// to be able to take responsibility for only binaural beat.
    /// </summary>
    public class BinauralBeat : BrainBeat
    {
        /// <summary>
        /// Updates an array of floats comprising the audio data.
        /// There is difference between the binaural beat and monaural beat.
        /// This concrete method must take responsibility for only binaural beat.
        /// </summary>
        /// <returns>The array of floats comprising the audio data.</returns>
        /// <param name="data">The array of floats comprising the audio data.</param>
        /// <param name="channels">An int that stores the number of channels of audio data passed to this delegate.</param>
        public override float[] UpdatePhase (float[] data, int channels)
        {
            if (channels != 2)
            {
                throw new ArgumentException("The number of channels of audio data must be 2 to play binaural beat.");
            }

            this.LeftIncrement = LeftFrequency * 2 * Math.PI / this.SampleRate;
            this.RightIncrement = RightFrequency * 2 * Math.PI / this.SampleRate;
            for (var i = 0; i < data.Length; i = i + channels)
            {
                this.LeftPhase += this.LeftIncrement;
                this.RightPhase += this.RightIncrement;
                if (this.LeftPhase > 2 * Math.PI)
                {
                    this.LeftPhase -= Math.PI * 2;
                }
                if (this.RightPhase > 2 * Math.PI)
                {
                    this.RightPhase -= Math.PI * 2;
                }
                data[i] = (float)(this.Gain * Math.Sin(this.LeftPhase));
                data[i + 1] = (float)(this.Gain * Math.Sin(this.RightPhase));
            }
            return data;
        }
    }
}
