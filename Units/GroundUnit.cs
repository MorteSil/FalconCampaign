using System.Text;
using FalconCampaign.Components;

namespace FalconCampaign.Units
{
    /// <summary>
    /// Common properties of all Ground Units.
    /// </summary>
    public class GroundUnit : Unit
    {
        #region Properties
        /// <summary>
        /// Unit Orders.
        /// </summary>
        public byte Orders { get => orders; set => orders = value; }
        /// <summary>
        /// Parent Division of Unit.
        /// </summary>
        public short Division { get => division; set => division = value; }
        /// <summary>
        /// Attack Objective ID.
        /// </summary>
        public VirtualUniqueIdentifier AttackObjectiveID { get => aobj; set => aobj = value; }
        #endregion Proeprties

        #region Fields
        private byte orders = 0;
        private short division = 0;
        private VirtualUniqueIdentifier aobj = new();

        #endregion

        #region Helper Methods
        internal new void Read(Stream stream)
        {
            using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
            orders = reader.ReadByte();
            division = reader.ReadInt16();
            aobj = new VirtualUniqueIdentifier
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32()
            };
        }

        internal new void Write(Stream stream)
        {
            base.Write(stream);
            using var writer = new BinaryWriter(stream, Encoding.Default, leaveOpen: true);
            writer.Write(orders);
            writer.Write(division);
            writer.Write(aobj.ID);
            writer.Write(aobj.Creator);
        }
        #endregion Helper Methods

        #region Funcitonal Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.AppendLine("Orders: " + Orders);
            sb.AppendLine("Division: " + Division);
            sb.AppendLine("***** Objective ID *****");
            sb.Append(AttackObjectiveID.ID);
            return sb.ToString();
        }
        #endregion Funcitonal Methods

        #region Constructors
        protected GroundUnit()
           : base()
        {
        }
        public GroundUnit(Stream stream, int version)
            : base(stream, version)
        {
            Read(stream);

        }
        #endregion Constructors

    }
}
