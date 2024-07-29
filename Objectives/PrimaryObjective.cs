using System.Collections.ObjectModel;
using System.Text;
using FalconCampaign.Components;

namespace FalconCampaign.Objectives
{
    /// <summary>
    /// Primary Objective in the Campaign.
    /// </summary>
    public class PrimaryObjective
    {
        #region Properties
        /// <summary>
        /// Objective ID.
        /// </summary>
        public VirtualUniqueIdentifier Id { get => id; set => id = value; }
        /// <summary>
        /// Team Priorities for the Objecteive.
        /// </summary>
        public Collection<short> Priority { get => priority; set => priority = value; }
        /// <summary>
        /// Objective Flags.
        /// </summary>
        public byte Flags { get => flags; set => flags = value; }
        #endregion Properties

        #region Fileds
        private VirtualUniqueIdentifier id = new();
        private Collection<short> priority = [];
        private byte flags = 0;


        #endregion Fields

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("ID: ");
            sb.Append(Id.ToString());
            for (int i = 0; i < Priority.Count; i++)
                sb.AppendLine("Priority " + i + ": " + Priority[i].ToString());
            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="PrimaryObjective"/> object.
        /// </summary>
        public PrimaryObjective() { }
        #endregion Constructors
    }
}
