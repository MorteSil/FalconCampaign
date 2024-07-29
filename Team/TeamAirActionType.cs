using FalconCampaign.Components;
using FalconCampaign.Enums;
using System.Text;

namespace FalconCampaign.Team
{
    /// <summary>
    /// Defines an Air Action used by the Campaign Engine.
    /// </summary>
    public class TeamAirActionType
    {
        #region Properties
        /// <summary>
        /// Start Time of the Action.
        /// </summary>
        public TimeSpan ActionStartTime { get => new(0, 0, 0, 0, (int)actionStartTime); set => actionStartTime = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Stop Time of the Action.
        /// </summary>
        public TimeSpan ActionStopTime { get => new(0, 0, 0, 0, (int)actionStopTime); set => actionStopTime = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Objective ID.
        /// </summary>
        public VirtualUniqueIdentifier ActionObjective { get => actionObjective; set => actionObjective = value; }
        /// <summary>
        /// Final Objective ID.
        /// </summary>
        public VirtualUniqueIdentifier LastActionObjective { get => lastActionObjective; set => lastActionObjective = value; }
        /// <summary>
        /// Type of Action.
        /// </summary>
        public AirActionType ActionType { get => (AirActionType)actionType; set => actionType = (byte)value; }
        #endregion Properties

        #region Fields
        private uint actionStartTime = 0;
        private uint actionStopTime = 0;
        private VirtualUniqueIdentifier actionObjective = new();
        private VirtualUniqueIdentifier lastActionObjective = new();
        private byte actionType = 0;
        #endregion Fields

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Start Time: " + ActionStartTime.ToString("g"));
            sb.AppendLine("End Time: " + ActionStopTime.ToString("g"));
            sb.AppendLine("***** Objective ID *****");
            sb.Append(ActionObjective.ToString());
            sb.AppendLine("***** Last Objective ID *****)");
            sb.Append(LastActionObjective.ToString());
            sb.AppendLine("Action Type: " + ActionType);

            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        public TeamAirActionType()
        {

        }
        #endregion Constructors        
    }
}
