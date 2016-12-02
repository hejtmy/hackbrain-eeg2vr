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
    /// An "ConcreteClass" of the so-called "Template Method Pattern"
    /// to be able to take responsibility for only binaural beat.
    /// </summary>
    public class BinauralBeatController : BeatController
    {
        /// <summary>
        /// The object of Beat.
        /// </summary>
        /// <value>The brain beat.</value>
        protected override BrainBeat _brainBeat{
            set;
            get;
        }

        /// <summary>
        /// Start this instance.
        /// </summary>
        public override void Start ()
        {
            this._brainBeat= new BinauralBeat();
            if (this.SampleRate == 0)
            {
                this._brainBeat.SampleRate = AudioSettings.outputSampleRate;
            }
            this.Play();
        }
    }
}
