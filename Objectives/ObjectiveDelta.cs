using System.Collections.ObjectModel;
using System.Text;
using FalconCampaign.Components;

namespace FalconCampaign.Objectives
{
    /// <summary>
    /// Holds Objective Delta Data. "Delta" is used to describe a difference in expected value or a deficiency.
    /// </summary>
    public class ObjectiveDelta
    {
        #region Properties
        /// <summary>
        /// Objective ID.
        /// </summary>
        public VirtualUniqueIdentifier ID { get => id; set => id = value; }
        /// <summary>
        /// Last Time the Objective was repaired.
        /// </summary>
        public TimeSpan LastRepair { get => new(0, 0, 0, 0, (int)lastRepair); set => lastRepair = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Owner of the Objective Delta.
        /// </summary>
        public byte Owner { get => owner; set => owner = value; }
        /// <summary>
        /// Supply Delta Value.
        /// </summary>
        public byte Supply { get => supply; set => supply = value; }
        /// <summary>
        /// Fuel Delta Value.
        /// </summary>
        public byte Fuel { get => fuel; set => fuel = value; }
        /// <summary>
        /// Losses Delta Value.
        /// </summary>
        public byte Losses { get => losses; set => losses = value; }
        /// <summary>
        /// Collection of F Status Values.
        /// </summary>
        public Collection<byte> FStatus { get => fStatus; set => fStatus = value; }
        #endregion Properties

        #region Fields
        private VirtualUniqueIdentifier id = new();
        private uint lastRepair = 0;
        private byte owner = 0;
        private byte supply = 0;
        private byte fuel = 0;
        private byte losses = 0;
        private Collection<byte> fStatus = [];


        #endregion Fields

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("***** Objective ID *****");
            sb.Append(ID.ToString());
            sb.AppendLine("Last Repair: " + LastRepair.ToString("dd, HH:mm"));
            sb.AppendLine("Owner: " + owner);
            sb.AppendLine("Supply: " + supply);
            sb.AppendLine("Fuel:" + fuel);
            sb.AppendLine("Losses: " + losses);
            sb.AppendLine("F Status Values: ");
            for (int i = 0; i < FStatus.Count; i++)
                sb.AppendLine("F Status " + i + ": " + FStatus[i]);
            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="ObjectiveDelta"/> object.
        /// </summary>
        public ObjectiveDelta() { }
        #endregion Constructors

    }
}
