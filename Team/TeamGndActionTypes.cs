using FalconCampaign.Components;
using FalconCampaign.Enums;
using System.Text;

namespace FalconCampaign.Team
{
    /// <summary>
    /// Ground Action Type used by the Campaign Engine.
    /// </summary>
    public class TeamGndActionType
    {
        #region Properties
        /// <summary>
        /// Time the Action Starts.
        /// </summary>
        public TimeSpan ActionTime { get => new(0, 0, 0, 0, (int)actionTime); set => actionTime = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Required completion time. If the Action is not completed by this time it is considered a failure.
        /// </summary>
        public TimeSpan ActionTimeout { get => new(0, 0, 0, 0, (int)actionTimeout); set => actionTimeout = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Objective ID.
        /// </summary>
        public VirtualUniqueIdentifier ActionObjective { get => actionObjective; set => actionObjective = value; }
        /// <summary>
        /// Type of ACtion being executed.
        /// </summary>
        public GroundActionType ActionType { get => (GroundActionType)actionType; set => actionType = (byte)value; }
        /// <summary>
        /// Determines Priority. Higher Tempo assigns more resources and makes this action more important to the Campaign Engine.
        /// </summary>
        public byte ActionTempo { get => actionTempo; set => actionTempo = value; } // TODO: Enum??
        /// <summary>
        /// Remaining points required to complete the action.
        /// </summary>
        public byte ActionPoints { get => actionPoints; set => actionPoints = value; }
        #endregion Properties

        #region Fields
        private uint actionTime = 0;
        private uint actionTimeout = 0;
        private VirtualUniqueIdentifier actionObjective = new();
        private byte actionType = 0;
        private byte actionTempo = 0;
        private byte actionPoints = 0;
        #endregion Fields

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Action Time: " + ActionTime.ToString("g"));
            sb.AppendLine("Required Completion Time: " + ActionTimeout.ToString("g"));
            sb.AppendLine("***** Objective ID *****");
            sb.Append(ActionObjective.ToString());
            sb.AppendLine("Action Type: " + ActionType);
            sb.AppendLine("Action Tempo: " + ActionTempo);
            sb.AppendLine("Remaining Action Points: " + actionPoints);

            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="TeamGndActionType"/> object.
        /// </summary>
        public TeamGndActionType() { }
        #endregion Constructors


    }
}
