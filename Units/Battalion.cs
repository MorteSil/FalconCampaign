using System.Text;
using FalconCampaign.Components;
using Utilities.GeoLib;

namespace FalconCampaign.Units
{
    /// <summary>
    /// Battalion Unit.
    /// </summary>
    public class Battalion : GroundUnit
    {
        #region Properties
        /// <summary>
        /// Last time the Unit Moved.
        /// </summary>
        public TimeSpan LastMovement { get => new(0, 0, 0, 0, (int)lastMove); set => lastMove = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Last time the unit was in Combat.
        /// </summary>
        public TimeSpan LastCombat { get => new(0, 0, 0, 0, (int)lastCombat); set => lastCombat = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Unit ID of the Parent Brigade.
        /// </summary>
        public VirtualUniqueIdentifier ParentID { get => parentID; set => parentID = value; }
        /// <summary>
        /// Objective ID of the Target Objective.
        /// </summary>
        public VirtualUniqueIdentifier LastObjectiveID { get => lastObjective; set => lastObjective = value; }
        /// <summary>
        /// Left Flank X Coordiante.
        /// </summary>
        public short Lfx { get => lfx; set => lfx = value; }
        /// <summary>
        /// Left Flank Y Coordinate.
        /// </summary>
        public short Lfy { get => lfy; set => lfy = value; }
        /// <summary>
        /// Left Flank Map Coordinate.
        /// </summary>
        public GeoPoint LeftFlank
        {
            get => new(lfx, lfy);
            set
            {
                lfx = (short)value.X;
                lfy = (short)value.Y;
            }
        }
        /// <summary>
        /// Right Flank X Coordiante.
        /// </summary>
        public short Rfx { get => rfx; set => rfx = value; }
        /// <summary>
        /// Right Flank Y Coordinate.
        /// </summary>
        public short Rfy { get => rfy; set => rfy = value; }
        /// <summary>
        /// Right Flank Map Coordinate.
        /// </summary>
        public GeoPoint RightFlank
        {
            get => new(rfx, rfy);
            set
            {
                rfx = (short)value.X;
                rfy = (short)value.Y;
            }
        }
        /// <summary>
        /// Supply Level.
        /// </summary>
        public byte Supply { get => supply; set => supply = value; }
        /// <summary>
        /// Fatigue Level.
        /// </summary>
        public byte Fatigue { get => fatigue; set => fatigue = value; }
        /// <summary>
        /// Morale Level.
        /// </summary>
        public byte Morale { get => morale; set => morale = value; }
        /// <summary>
        /// Current Heading.
        /// </summary>
        public byte Heading { get => heading; set => heading = value; }
        /// <summary>
        /// Target Final Heading.
        /// </summary>
        public byte FinalHeading { get => finalHeading; set => finalHeading = value; }
        /// <summary>
        /// Position Indicator.
        /// </summary>
        public byte Position { get => position; set => position = value; }


        #endregion Properties

        #region Fields
        private uint lastMove = 0;
        private uint lastCombat = 0;
        private VirtualUniqueIdentifier parentID = new();
        private VirtualUniqueIdentifier lastObjective = new();
        private short lfx = 0;
        private short lfy = 0;
        private short rfx = 0;
        private short rfy = 0;
        private byte supply = 0;
        private byte fatigue = 0;
        private byte morale = 0;
        private byte heading = 0;
        private byte finalHeading = 0;
        private byte position = 0;


        #endregion Fields

        #region Helper Methods
        protected new void Read(Stream stream)
        {
            using BinaryReader reader = new(stream, Encoding.Default, leaveOpen: true);
            lastMove = reader.ReadUInt32();
            lastCombat = reader.ReadUInt32();

            parentID = new()
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32()
            };

            lastObjective = new()
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32()
            };

            supply = reader.ReadByte();
            fatigue = reader.ReadByte();
            morale = reader.ReadByte();
            heading = reader.ReadByte();
            finalHeading = reader.ReadByte();
            position = reader.ReadByte();
        }

        public new void Write(Stream stream)
        {
            base.Write(stream);
            using BinaryWriter writer = new(stream, Encoding.Default, leaveOpen: true);
            writer.Write(lastMove);
            writer.Write(lastCombat);

            writer.Write(parentID.ID);
            writer.Write(parentID.Creator);

            writer.Write(lastObjective.ID);
            writer.Write(lastObjective.Creator);

            writer.Write(supply);
            writer.Write(fatigue);
            writer.Write(morale);
            writer.Write(heading);
            writer.Write(finalHeading);
            writer.Write(position);
        }
        #endregion Helper Methods

        #region Functional Methods

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Last Movement: " + LastMovement.ToString("g"));
            sb.AppendLine("Last Combat: " + LastCombat.ToString("g"));
            sb.AppendLine("Parent ID: " + parentID.ID);
            sb.AppendLine("Objective ID: " + lastObjective.ID);
            sb.AppendLine("Left Flank: " + LeftFlank.ToString());
            sb.AppendLine("Right Flank: " + RightFlank.ToString());
            sb.AppendLine("Supply Level: " + supply);
            sb.AppendLine("Fatigue Level: " + fatigue);
            sb.AppendLine("Morale Level: " + morale);
            sb.AppendLine("Heading: " + heading);
            sb.AppendLine("Final Target Heading: " + finalHeading);
            sb.AppendLine("Position: " + position);
            return base.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="Battalion"/> object.
        /// </summary>
        protected Battalion()
            : base()
        {
        }
        /// <summary>
        /// Initializes a <see cref="Battalion"/> object with the supplied data.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object with the intiialization data.</param>
        /// <param name="version">File Version.</param>
        public Battalion(Stream stream, int version)
            : base(stream, version)
        {
            Read(stream);
        }
        #endregion Constructors
    }
}
