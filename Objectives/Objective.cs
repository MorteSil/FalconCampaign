using System.Collections.ObjectModel;
using System.Text;
using FalconCampaign.Components;

namespace FalconCampaign.Objectives
{
    /// <summary>
    /// An Objective within the Campaign.
    /// </summary>
    public class Objective : CampaignBase
    {
        // Is this still used or everything is in the DB Now?

        #region Properties
        /// <summary>
        /// Type of this Objective.
        /// </summary>
        public short ObjectiveType { get => objectiveType; set => objectiveType = value; } // TODO: Enum
        /// <summary>
        /// Time this Objective was last repaired.
        /// </summary>
        public TimeSpan LastRepair { get => new(0, 0, 0, 0, (int)lastRepair); set => lastRepair = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Flags for this Objective.
        /// </summary>
        public uint ObjectiveFlags { get => obj_flags; set => obj_flags = value; } // TODO: Map this
        /// <summary>
        /// Supply Value.
        /// </summary>
        public byte Supply { get => supply; set => supply = value; }
        /// <summary>
        /// Fuel Status Value.
        /// </summary>
        public byte Fuel { get => fuel; set => fuel = value; }
        /// <summary>
        /// Loss Level of Objective.
        /// </summary>
        public byte Losses { get => losses; set => losses = value; }
        /// <summary>
        /// FStatus Values.
        /// </summary>
        public Collection<byte> Fstatus { get => fstatus; set => fstatus = value; }
        /// <summary>
        /// Priority Value.
        /// </summary>
        public byte Priority { get => priority; set => priority = value; }
        /// <summary>
        /// Index into the Name Table.
        /// </summary>
        public short NameID { get => nameId; set => nameId = value; }
        /// <summary>
        /// Virtual ID of this Objective.
        /// </summary>
        public VirtualUniqueIdentifier Parent { get => parent; set => parent = value; }
        /// <summary>
        /// Initial Team that owned the Objective.
        /// </summary>
        public byte FirstOwner { get => first_owner; set => first_owner = value; }
        /// <summary>
        /// Number of Objective Links.
        /// </summary>
        public byte LinkCount { get => links; set => links = value; }
        /// <summary>
        /// Collection of Objective Travel Paths and associated Costs.
        /// </summary>
        public Collection<CampObjectiveLinkDataType> LinkData { get => link_data; set => link_data = value; }
        /// <summary>
        /// Probablility of detection for each Link.
        /// </summary>
        public Collection<float> DetectionRatio { get => detect_ratio; set => detect_ratio = value; }
        #endregion Properties

        #region  Fields
        private short objectiveType = 0;
        private uint lastRepair = 0;
        private uint obj_flags = 0;
        private byte supply = 0;
        private byte fuel = 0;
        private byte losses = 0;
        private Collection<byte> fstatus = [];
        private byte priority = 0;
        private short nameId = 0;
        private VirtualUniqueIdentifier parent = new();
        private byte first_owner = 0;
        private byte links = 0;
        private Collection<CampObjectiveLinkDataType> link_data = [];
        private Collection<float> detect_ratio = [0, 0, 0, 0, 0, 0, 0, 0];


        #endregion Fields

        #region Helper Methods
        internal new void Read(Stream stream)
        {
            using BinaryReader reader = new(stream, Encoding.Default, leaveOpen: true);
            lastRepair = reader.ReadUInt32();
            ObjectiveFlags = reader.ReadUInt32();
            Supply = reader.ReadByte();
            Fuel = reader.ReadByte();
            Losses = reader.ReadByte();
            byte numStatuses = reader.ReadByte();

            Fstatus = [];
            for (int i = 0; i < numStatuses; i++)
            {
                Fstatus.Add(reader.ReadByte());
            }
            Priority = reader.ReadByte();
            NameID = reader.ReadInt16();
            Parent = new VirtualUniqueIdentifier
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32()
            };

            FirstOwner = reader.ReadByte();
            LinkCount = reader.ReadByte();

            for (int i = 0; i < LinkCount; i++)
            {
                CampObjectiveLinkDataType thisLink = new()
                {
                    Costs = []
                };
                for (int j = 0; j < (int)FalconDatabase.Enums.MoveType.MOVEMENT_TYPES; j++)
                {
                    thisLink.Costs.Add(reader.ReadByte());
                }
                VirtualUniqueIdentifier newId = new()
                {
                    ID = reader.ReadUInt32(),
                    Creator = reader.ReadUInt32()
                };
                thisLink.Id = newId;
                LinkData.Add(thisLink);
            }

            byte hasRadarData = reader.ReadByte();
            if (hasRadarData > 0)
            {
                DetectionRatio = [];
                for (int i = 0; i < 8; i++)
                {
                    DetectionRatio.Add(reader.ReadSingle());
                }
            }
        }

        internal new void Write(Stream stream)
        {
            base.Write(stream);
            using var writer = new BinaryWriter(stream, Encoding.Default, leaveOpen: true);
            writer.Write(lastRepair);
            writer.Write(ObjectiveFlags);

            writer.Write(Supply);
            writer.Write(Fuel);
            writer.Write(Losses);
            writer.Write((byte)Fstatus.Count);
            for (int i = 0; i < Fstatus.Count; i++)
            {
                writer.Write(Fstatus[i]);
            }
            writer.Write(Priority);
            writer.Write(NameID);
            writer.Write(Parent.ID);
            writer.Write(Parent.Creator);
            writer.Write(FirstOwner);
            writer.Write((byte)LinkData.Count);

            for (int i = 0; i < LinkData.Count; i++)
            {
                var thisLink = LinkData[i];
                for (int j = 0; j < (int)FalconDatabase.Enums.MoveType.MOVEMENT_TYPES; j++)
                {
                    writer.Write(thisLink.Costs[j]);
                }
                writer.Write(thisLink.Id.ID);
                writer.Write(thisLink.Id.Creator);
            }

            byte hasRadarData = (byte)(DetectionRatio != null ? DetectionRatio.Count : 0);
            writer.Write(hasRadarData);

            if (hasRadarData > 0)
            {
                for (int i = 0; i < DetectionRatio?.Count; i++)
                {
                    writer.Write(DetectionRatio[i]);
                }
            }
        }
        #endregion Helper Methods

        #region Functional Methods

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("Objective Type: " + ObjectiveType);
            sb.AppendLine("Last Repair: " + LastRepair);
            sb.AppendLine("Flags: " + ObjectiveFlags);
            sb.AppendLine("Supply: " + Supply);
            sb.AppendLine("Fuel: " + Fuel);
            sb.AppendLine("Losses: " + Losses);
            sb.AppendLine("Status Codes: ");
            for (int i = 0; i < Fstatus.Count; i++)
                sb.AppendLine("   Status " + i + ": " + Fstatus[i]);
            sb.AppendLine("Priority: " + Priority);
            sb.AppendLine("Name ID: " + NameID);
            sb.AppendLine("Parent ID: " + Parent.ID);
            sb.AppendLine("Parent Creator: " + Parent.Creator);
            sb.AppendLine("First Owner: " + FirstOwner);
            sb.AppendLine("Number of Links: " + LinkCount);
            for (int i = 0; i < LinkData.Count; i++)
            {
                sb.AppendLine("   Link " + i + ": ");
                sb.AppendLine("     ID: " + LinkData[i].Id.ID);
                for (int j = 0; j < LinkData[i].Costs.Count; j++)
                    sb.AppendLine("     " + (FalconDatabase.Enums.MoveType)j + " Cost: " + LinkData[i].Costs[j]);
            }
            sb.AppendLine("  Detect Ratios: ");
            for (int i = 0; i < DetectionRatio.Count; i++)
                sb.AppendLine("     " + (FalconDatabase.Enums.MoveType)i + ": " + DetectionRatio[i]);

            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="Objective"/> object.
        /// </summary>
        protected Objective()
           : base()
        {
        }
        /// <summary>
        /// Initializes an <see cref="Objective"/> object with the data in <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">A <see cref="Stream"/> with initialization data for this objective.</param>
        /// <param name="version">File Version.</param>
        public Objective(Stream stream, int version)
            : base(stream, version)
        {
            Read(stream);
        }
        #endregion Constructors
    }
}
