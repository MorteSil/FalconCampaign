using System.Text;
using FalconCampaign.Units;

namespace FalconCampaign.Team
{
    /// <summary>
    /// Basic Information about a Team in the Campaign.
    /// </summary>
    public class TeamBasicInfo
    {
        #region Properties
        /// <summary>
        /// Flag associated with the Team.
        /// </summary>
        public byte TeamFlag { get => teamFlag; set => teamFlag = value; }
        /// <summary>
        /// Color associated with the Team.
        /// </summary>
        public byte TeamColor { get => teamColor; set => teamColor = value; }
        /// <summary>
        /// Team Name.
        /// </summary>
        public string TeamName { get => teamName; set => teamName = value; }
        /// <summary>
        /// Team Motto.
        /// </summary>
        public string TeamMotto { get => teamMotto; set => teamMotto = value; }
        #endregion Properties

        #region Fields
        private byte teamFlag = 0;
        private byte teamColor = 0;
        private string teamName = "";
        private string teamMotto = "";


        #endregion Fields

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Team Flag: " + TeamName);
            sb.AppendLine("Team Color: " + TeamColor);
            sb.AppendLine("Team Name: " + TeamName);
            sb.AppendLine("Team Motto: " + TeamMotto);
            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="SquadInfo"/> object.
        /// </summary>
        public TeamBasicInfo() { }
        #endregion Constructors

    }
}
