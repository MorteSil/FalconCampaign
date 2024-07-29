using FalconCampaign.Components;
using FalconCampaign.Enums;
using System.Text;
using Utilities.GeoLib;

namespace FalconCampaign.Units
{
    /// <summary>
    /// A Waypoint used by units in the Campaign.
    /// </summary>
    public class Waypoint
    {
        #region Constants
        /// <summary>
        /// BitMask for determining if the Flight needs to remain at a Waypoint for a period of time.
        /// </summary>
        public const byte WP_HAVE_DEPTIME = 0x01;
        /// <summary>
        /// BitMask for determining if the Waypoint has a Target assoviated with it.
        /// </summary>
        public const byte WP_HAVE_TARGET = 0x02;
        #endregion Constants

        #region Properties
        /// <summary>
        /// <para>Property Identifier Mask for Departure Time and Target.</para>
        /// <para>Indicators can be set with HasDepartureTime and HasTarget Properties.</para>
        /// </summary>
        public byte Haves { get => haves; }
        /// <summary>
        /// Inidicates if the Waypoint has a Departure Time indicating a Loiter or Station Time must be fulfilled
        /// </summary>
        public bool HasDepartTime
        {
            get => (WP_HAVE_DEPTIME & haves) == WP_HAVE_DEPTIME;
            set
            {
                if (value)
                    haves |= WP_HAVE_DEPTIME;
                else
                    haves &= WP_HAVE_DEPTIME;
            }
        }
        /// <summary>
        /// Indicates if the Waypoint has a Target.
        /// </summary>
        public bool HasTarget
        {
            get => (WP_HAVE_TARGET & haves) == WP_HAVE_TARGET;
            set
            {
                if (value)
                    haves |= WP_HAVE_TARGET;
                else
                    haves &= WP_HAVE_TARGET;
            }
        }
        /// <summary>
        /// Grid-Based X Coordinate.
        /// </summary>
        public short GridX { get => gridX; set => gridX = value; }
        /// <summary>
        /// Grid-Based Y Coordiante.
        /// </summary>
        public short GridY { get => gridY; set => gridY = value; }
        /// <summary>
        /// Grid-Based Z Coordinate.
        /// </summary>
        public short GridZ { get => gridZ; set => gridZ = value; }
        /// <summary>
        /// Coordinate-Form Location.
        /// </summary>
        public GeoPoint Location
        {
            get => new(GridX, GridY, GridZ);
            set
            {
                GridX = (short)value.X; GridY = (short)value.Y; GridZ = (short)value.Elevation;
            }
        }
        /// <summary>
        /// Arrival Time.
        /// </summary>
        public TimeSpan Arrive { get => new(0, 0, 0, 0, (int)arrive); set => arrive = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Action to perform at the Waypoint.
        /// </summary>
        public STPTAirAction Action { get => (STPTAirAction)action; set => action = (byte)value; }
        /// <summary>
        /// Action to perform enroute to the Waypoint.
        /// </summary>
        public STPTAirAction RouteAction { get => (STPTAirAction)routeAction; set => routeAction = (byte)value; }
        /// <summary>
        /// Formation to use at the Waypoint.
        /// </summary>
        public byte Formation { get => formation; set => formation = value; } // TODO: Enum
        /// <summary>
        /// Spacing between aircraft and elements of the Formation.
        /// </summary>
        public short FormationSpacing { get => formationSpacing; set => formationSpacing = value; }
        /// <summary>
        /// Unit Flags.
        /// </summary>
        public uint Flags { get => flags; set => flags = value; }
        /// <summary>
        /// Target ID.
        /// </summary>
        public VirtualUniqueIdentifier TargetID { get => targetID; set => targetID = value; }
        /// <summary>
        /// Target Building at the Waypoint.
        /// </summary>
        public byte TargetBuilding { get => targetBuilding; set => targetBuilding = value; }
        /// <summary>
        /// Departure Time. If this is different than the Arrival Time it indicates a Loiter Time or Station Time must be fulfilled.
        /// </summary>
        public TimeSpan Depart { get => new(0, 0, 0, 0, (int)depart); set => depart = (uint)value.TotalMilliseconds; }
        #endregion Properties

        #region Fields
        private byte haves = 0;
        private short gridX = 0;
        private short gridY = 0;
        private short gridZ = 0;
        private uint arrive = 0;
        private byte action = 0;
        private byte routeAction = 0;
        private byte formation = 0;
        private short formationSpacing = 0;
        private uint flags = 0;
        private VirtualUniqueIdentifier targetID = new();
        private byte targetBuilding = 0;
        private uint depart = 0;
        private readonly int version = int.MaxValue;


        #endregion Fields

        #region Helper Methods
        internal void Read(Stream stream)
        {
            using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
            haves = reader.ReadByte();
            gridX = reader.ReadInt16();
            gridY = reader.ReadInt16();
            gridZ = reader.ReadInt16();
            arrive = reader.ReadUInt32();
            action = reader.ReadByte();
            routeAction = reader.ReadByte();
            var tmp = reader.ReadByte();
            formation = (byte)(tmp & 0x0f);
            formationSpacing = (short)((tmp >> 4 & 0x0F) - 8);

            flags = reader.ReadUInt32();

            if ((Haves & WP_HAVE_TARGET) != 0)
            {
                targetID = new()
                {
                    ID = reader.ReadUInt32(),
                    Creator = reader.ReadUInt32()
                };
                targetBuilding = reader.ReadByte();
            }
            else
            {
                targetID = new();
                targetBuilding = 255;
            }
            if ((Haves & WP_HAVE_DEPTIME) != 0)
                depart = reader.ReadUInt32();
            else
                depart = arrive;
        }
        internal void Write(Stream stream)
        {
            using var writer = new BinaryWriter(stream, Encoding.Default, leaveOpen: true);
            writer.Write(haves);
            writer.Write(gridX);
            writer.Write(gridY);
            writer.Write(gridZ);
            writer.Write(arrive);
            writer.Write(action);
            writer.Write(routeAction);

            var tmp = (byte)(formation & 0x0F | (formationSpacing + 8 & 0x0F) << 4);
            writer.Write(tmp);

            writer.Write(flags);
            if ((Haves & WP_HAVE_TARGET) != 0)
            {
                writer.Write(targetID.ID);
                writer.Write(targetID.Creator);
                writer.Write(targetBuilding);
            }

            if ((Haves & WP_HAVE_DEPTIME) != 0)
                writer.Write(depart);
        }
        #endregion Helper MEthods

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Haves: " + Haves);
            sb.AppendLine("   Has Departure Time: " + HasDepartTime);
            sb.AppendLine("   Has Target: " + HasTarget);
            sb.AppendLine("Grid X: " + GridX);
            sb.AppendLine("Grid Y: " + GridY);
            sb.AppendLine("Grid Z: " + GridZ);
            sb.AppendLine(Location.ToString());
            sb.AppendLine("Arrive: " + Arrive.ToString("g"));
            sb.AppendLine("Action: " + Action);
            sb.AppendLine("Route Action: " + RouteAction);
            sb.AppendLine("Formation: " + Formation);
            sb.AppendLine("Formation Spacing: " + FormationSpacing);
            sb.AppendLine("Flags: " + Flags);
            sb.AppendLine("Target ID: " + TargetID.ID);
            sb.AppendLine("Target Building: " + TargetBuilding);
            sb.AppendLine("Depart: " + Depart.ToString("g"));

            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Waypoint Constructor.
        /// </summary>
        public Waypoint()
            : base()
        {
        }
        /// <summary>
        /// Initializes a Waypointwith the data supplied.
        /// </summary>
        /// <param name="stream">Data stream with values to read.</param>
        /// <param name="version">File Version.</param>
        public Waypoint(Stream stream, int version)
            : this()
        {
            this.version = version;
            Read(stream);
        }
        #endregion Constructors

    }
}
