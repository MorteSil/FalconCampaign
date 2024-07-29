using System.Text;

namespace FalconCampaign.Components
{
    /// <summary>
    /// A significant event in the Campaign.
    /// </summary>
    public class CampaignEvent
    {
        #region Properties
        /// <summary>
        /// Event ID
        /// </summary>
        public short ID { get => id; set => id = value; }
        /// <summary>
        /// Event Flags
        /// </summary>
        public short Flags { get => flags; set => flags = value; }        
        #endregion Properties

        #region Fields
        private short id = 0;
        private short flags = 0;
        #endregion Fields

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new ();
            sb.AppendLine("ID: " + id);
            sb.AppendLine("Flags: " + flags);
            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        public CampaignEvent()
        {

        }
        #endregion Constructors


    }
}
