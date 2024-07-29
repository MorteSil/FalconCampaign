using System.Text;
using FalconCampaign.Components;

namespace FalconCampaign.CampaignEngine
{
    /// <summary>
    /// The Base Campaign Manager Class.
    /// </summary>
    public class CampaignManager
    {
        #region Properties
        /// <summary>
        /// Manager ID.
        /// </summary>
        public VirtualUniqueIdentifier ID { get => id; set => id = value; }
        /// <summary>
        /// Owner ID if this Manager is a child.
        /// </summary>
        public VirtualUniqueIdentifier OwnerID { get => ownerId; set => ownerId = value; }
        /// <summary>
        /// Type of Manager this object Represents.
        /// </summary>
        public ushort EntityType { get => entityType; set => entityType = value; }
        /// <summary>
        /// Flags for the Campaign Engine.
        /// </summary>
        public short ManagerFlags { get => managerFlags; set => managerFlags = value; } // TODO: Map this
        /// <summary>
        /// Owner Reference if one exists.
        /// </summary>
        public byte Owner { get => owner; set => owner = value; } // TODO: Enum
        #endregion Properties

        #region Fields
        private VirtualUniqueIdentifier id = new();
        private VirtualUniqueIdentifier ownerId = new();
        private ushort entityType = 0;
        private short managerFlags = 0;
        private byte owner = 0;
        protected readonly int version = int.MaxValue;


        #endregion

        #region Helper Methods
        internal void Read(Stream stream)
        {
            using BinaryReader reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
            id = new()
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32(),
            };
            entityType = reader.ReadUInt16();
            managerFlags = reader.ReadInt16();
            owner = reader.ReadByte();
        }

        internal void Write(Stream stream)
        {
            using var writer = new BinaryWriter(stream, Encoding.Default, leaveOpen: true);
            writer.Write(id.ID);
            writer.Write(id.Creator);
            writer.Write(entityType);
            writer.Write(managerFlags);
            writer.Write(owner);
        }
        #endregion Helper Methods

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("***** ID *****");
            sb.Append(id.ToString());
            sb.AppendLine("***** Owner ID *****");
            sb.Append(ownerId.ToString());
            sb.AppendLine("Entity Type: " + EntityType);
            sb.AppendLine("Flags: " + ManagerFlags);
            sb.AppendLine("Owner: " + Owner);

            return sb.ToString();
        }
        #endregion Funcitonal Methods

        #region Constructors
        protected CampaignManager() { }
        public CampaignManager(Stream stream, int version)
        {
            this.version = version;
            Read(stream);
        }
        #endregion Constructors

    }
}
