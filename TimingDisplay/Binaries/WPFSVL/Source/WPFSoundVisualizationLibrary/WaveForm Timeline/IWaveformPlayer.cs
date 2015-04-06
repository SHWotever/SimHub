// Copyright (C) 2011 - 2012, Jacob Johnston 
//
// Permission is hereby granted, free of charge, to any person obtaining a 
// copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions: 
//
// The above copyright notice and this permission notice shall be included in 
// all copies or substantial portions of the Software. 
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE. 


using System;
namespace WPFSoundVisualizationLib
{
    /// <summary>
    /// Provides access to sound player functionality needed to
    /// generate a Waveform.
    /// </summary>
    public interface IWaveformPlayer : ISoundPlayer
    {
        /// <summary>
        /// Gets or sets the current sound streams playback position.
        /// </summary>
        double ChannelPosition { get; set; }

        /// <summary>
        /// Gets the total channel length in seconds.
        /// </summary>
        double ChannelLength { get; }

        /// <summary>
        /// Gets the raw level data for the waveform.
        /// </summary>
        /// <remarks>
        /// Level data should be structured in an array where each sucessive index
        /// alternates between left or right channel data, starting with left. Index 0
        /// should be the first left level, index 1 should be the first right level, index
        /// 2 should be the second left level, etc.
        /// </remarks>
        float[] WaveformData { get; }

        /// <summary>
        /// Gets or sets the starting time for a section of repeat/looped audio.
        /// </summary>
        TimeSpan SelectionBegin { get; set; }

        /// <summary>
        /// Gets or sets the ending time for a section of repeat/looped audio.
        /// </summary>
        TimeSpan SelectionEnd { get; set; }
    }
}
