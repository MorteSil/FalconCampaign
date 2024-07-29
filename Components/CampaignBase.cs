using System.Text;
using Utilities.GeoLib;

namespace FalconCampaign.Components
{
    /// <summary>
    /// Holds basic values common to all campaign objects.
    /// </summary>
    public class CampaignBase
    {        
        #region Properties
        /// <summary>
        /// Object ID.
        /// </summary>
        public VirtualUniqueIdentifier ID { get => id; set => id = value; }
        /// <summary>
        /// Entity Type Index.
        /// </summary>
        public ushort EntityType { get => entityType; set => entityType = value; }
        /// <summary>
        /// X Coordinate Value.
        /// </summary>
        public short X { get => x; set => x = value; }
        /// <summary>
        /// Y Coordiante Value.
        /// </summary>
        public short Y { get => y; set => y = value; }
        /// <summary>
        /// Z Coordiante Value.
        /// </summary>
        public float Z { get => z; set => z = value; }
        /// <summary>
        /// 3D Poisition of the Object.
        /// </summary>
        public GeoPoint Location
        {
            get => new(new GeoPoint(x, y, z));
            set { x = (short)value.X; y = (short)value.Y; z = (short)value.Elevation; }
        }
        /// <summary>
        /// Time when the Object was last spotted.
        /// </summary>
        public TimeSpan SpotTime { get => new(0, 0, 0, 0, (int)spotTime); set => spotTime = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Indicates if the Object has been Spotted.
        /// </summary>
        public short Spotted { get => spotted; set => spotted = value; } // TODO: bool?
        /// <summary>
        /// Object Flags.
        /// </summary>
        public short BaseFlags { get => baseFlags; set => baseFlags = value; } // TODO: Map this
        /// <summary>
        /// Object Owner ID.
        /// </summary>
        public byte Owner { get => owner; set => owner = value; } // TODO: This has an enum associated with it
        /// <summary>
        /// Object ID within the Campaign Engine.
        /// </summary>
        public short CampaignID { get => campId; set => campId = value; }
        #endregion Properties

        #region Fields

        protected VirtualUniqueIdentifier id = new();
        protected ushort entityType = 0;
        protected short x = 0;
        protected short y = 0;
        protected float z = 0;
        protected uint spotTime = 0;
        protected short spotted = 0;
        protected short baseFlags = 0;
        protected byte owner = 0;
        protected short campId = 0;
        protected int version = 0;

        #endregion

        #region Helper Methods
        /// <summary>
        /// Reads the values for this campaign object.
        /// </summary>
        /// <param name="stream">Data Stream.</param>
        internal virtual void Read(Stream stream)
        {
            using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
            id = new VirtualUniqueIdentifier
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32()
            };
            entityType = reader.ReadUInt16();
            x = reader.ReadInt16();
            y = reader.ReadInt16();
            z = reader.ReadSingle();
            spotTime = reader.ReadUInt32();
            Spotted = reader.ReadInt16();
            BaseFlags = reader.ReadInt16();
            Owner = reader.ReadByte();
            CampaignID = reader.ReadInt16();
        }

        /// <summary>
        /// Writes the values for this campaign object.
        /// </summary>
        /// <param name="stream">Data Stream.</param>
        internal virtual void Write(Stream stream)
        {
            using var writer = new BinaryWriter(stream, Encoding.Default, leaveOpen: true);
            writer.Write(id.ID);
            writer.Write(id.Creator);
            writer.Write(entityType);
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
            writer.Write(spotTime);
            writer.Write(spotted);
            writer.Write(baseFlags);
            writer.Write(owner);
            writer.Write(campId);
        }
        #endregion Helper Methods

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("***** ID ***** ");
            sb.Append(id.ToString());
            sb.AppendLine("X: " + X);
            sb.AppendLine("Y: " + Y);
            sb.AppendLine("Z: " + Z);
            sb.AppendLine("Spot Time: " + SpotTime.ToString("g"));
            sb.AppendLine("Spotted: " + Spotted);
            sb.AppendLine("Base Flags: " + BaseFlags);
            sb.AppendLine("Owner: " + Owner);
            sb.AppendLine("Campaign ID: " + CampaignID);
            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for Generic <see cref="CampaignBase"/> class.
        /// </summary>
        protected CampaignBase() { }
        /// <summary>
        /// Initializes an instance of the <see cref="CampaignBase"/> object with the supplied data.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object with the data to read.</param>
        /// <param name="version">File Version.</param>
        public CampaignBase(Stream stream, int version = int.MaxValue)
            : this()
        {
            this.version = version;
            Read(stream);
        }
        #endregion Constructors
    }
}
