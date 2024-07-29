using System.Collections.ObjectModel;
using System.Text;
using FalconCampaign.Components;

namespace FalconCampaign.Objectives
{
    /// <summary>
    /// Holds values determining the cost for a unit to travel to a location.
    /// </summary>
    public class CampObjectiveLinkDataType
    {
        #region Properties
        /// <summary>
        /// ID of this Link Data Entity
        /// </summary>
        public VirtualUniqueIdentifier Id { get => id; set => id = value; }
        /// <summary>
        /// Collection of costs to tracel based on distance and movement type.
        /// </summary>
        public Collection<byte> Costs { get => costs; set => costs = value; }

        #endregion Properties

        #region Fields
        private Collection<byte> costs = [];
        private VirtualUniqueIdentifier id = new();
        #endregion Fields

        #region Funcitonal Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("***** ID *****");
            sb.Append(id.ToString());
            for (int i = 0; i < Costs.Count; i++)
                sb.AppendLine("Cost " + i + ": " + costs[i]);
            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="CampObjectiveLinkDataType"/> object.
        /// </summary>
        public CampObjectiveLinkDataType() { }
        #endregion Construtors

    }
}
