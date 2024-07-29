using System.Text;

namespace FalconCampaign.Pilot
{
    public class PilotInfo
    {
        #region Properties
        public short Usage { get => usage; set => usage = value; }
        public byte VoiceID { get => voiceID; set => voiceID = value; }
        public byte PhotoID { get => photoID; set => photoID = value; }
        #endregion Properties

        #region Fields
        private short usage = 0;								// How many times this pilot is being used
        private byte voiceID = 0;							// Which voice data to use
        private byte photoID = 0;                           // Assigned through the UI
        #endregion Fields

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("Usage: " + usage);
            sb.AppendLine("Voice ID: " + voiceID);
            sb.AppendLine("Photo ID: " + photoID);
            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        public PilotInfo()
        { }
        #endregion Constructors

    };
}
