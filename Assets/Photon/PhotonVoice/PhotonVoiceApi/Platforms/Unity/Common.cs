namespace Photon.Voice.Unity
{
    public class PhotonVoiceCreatedParams
    {
        public Voice.LocalVoice Voice { get; internal set; }
        public Voice.IAudioDesc AudioDesc { get; internal set; }
    }
}
