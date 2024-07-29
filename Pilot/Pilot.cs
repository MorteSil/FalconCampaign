using System.Text;

namespace FalconCampaign.Pilot
{
    /// <summary>
    /// A Flight within the campaign.
    /// </summary>
    public class Pilot
    {
        #region Properties
        /// <summary>
        /// Index in the Database Pilot Table.
        /// </summary>
        public short PilotID { get => pilotID; set => pilotID = value; }
        /// <summary>
        /// Pilot Skill and Rating Bit Mask.
        /// </summary>
        public byte PilotSkillAndRating { get => pilotSkillRatings; set => pilotSkillRatings = value; }
        /// <summary>
        /// Pilot Status Indicator.
        /// </summary>
        public byte PilotStatus { get => pilotStatus; set => pilotStatus = value; }
        /// <summary>
        /// Piot A-A Kills.
        /// </summary>
        public byte KillsAA { get => aaKills; set => aaKills = value; }
        /// <summary>
        /// Pilot A-G Kills.
        /// </summary>
        public byte KillsAG { get => agKills; set => agKills = value; }
        /// <summary>
        /// Pilot Static Kills.
        /// </summary>
        public byte KillsAS { get => asKills; set => asKills = value; }
        /// <summary>
        /// Pilot Naval Kills.
        /// </summary>
        public byte KillsAN { get => anKills; set => anKills = value; }
        /// <summary>
        /// Number of Mission Flown by the Pilot.
        /// </summary>
        public short MissionCount { get => missionsFlown; set => missionsFlown = value; }
        #endregion Properties

        #region Fields
        private short pilotID = 0;							// Index into the PilotInfoClass table
        private byte pilotSkillRatings = 0;				// Low Nibble: Skill, Hi Nibble: Rating
        private byte pilotStatus = 0;
        private byte aaKills = 0;
        private byte agKills = 0;
        private byte asKills = 0;
        private byte anKills = 0;
        private short missionsFlown = 0;


        #endregion Fields

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("Pilot ID: " + PilotID);
            sb.AppendLine("Pilot Skill and Rating: " + PilotSkillAndRating);
            sb.AppendLine("   Pilot Skill: " + (PilotSkillAndRating & 3));
            sb.AppendLine("   Pilot Rating: " + (PilotSkillAndRating & 12));
            sb.AppendLine("Pilot Status: " + PilotStatus);
            sb.AppendLine("A-A Kills: " + KillsAA);
            sb.AppendLine("A-G Kills: " + KillsAG);
            sb.AppendLine("A-S Kills: " + KillsAS);
            sb.AppendLine("A-N Kills: " + KillsAN);
            sb.AppendLine("Missions Flown: " + MissionCount);

            return sb.ToString();
            ;
        }
        #endregion Functional Methods

        #region Constructors
        public Pilot() { }
        #endregion Constructors

    }
}
