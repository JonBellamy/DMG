﻿// Audio code shamelessly stolen (with sincere thanks and recognition) from https://github.com/Washi1337/Emux

namespace Emux.GameBoy.Audio
{
    public interface IAudioChannelOutput
    {
        int SampleRate
        {
            get;
        }

        void BufferSoundSamples(float[] sampleData, int offset, int length);
    }
}
