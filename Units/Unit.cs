using System.Collections.ObjectModel;
using System.Text;
using FalconCampaign.Components;
using Utilities.GeoLib;

namespace FalconCampaign.Units
{
    /// <summary>
    /// Basic Unit Data.
    /// </summary>
    public class Unit : CampaignBase
    {
        #region Constants
        /// <summary>
        /// BitMask for final Unit Identifier in a group.
        /// </summary>
        public const int U_FINAL = 0x100000;
        #endregion

        #region Properties
        /// <summary>
        /// If this is the last Unit in an element.
        /// </summary>
        protected bool Final
        {
            get
            {
                return (UnitFlags & U_FINAL) > 0;
            }
        }
        /// <summary>
        /// Unit Type from the Database.
        /// </summary>
        public short UnitType { get => unitType; set => unitType = value; } // TODO: Enum
        /// <summary>
        /// Last time this unit was checked by the Campaign Engine.
        /// </summary>
        public TimeSpan LastCheck { get => new(0, 0, 0, 0, (int)lastCheck); set => lastCheck = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Unit Roster.
        /// </summary>
        public int Roster { get => roster; set => roster = value; }
        /// <summary>
        /// Unit Flags.
        /// </summary>
        public int UnitFlags { get => unitFlags; set => unitFlags = value; }
        /// <summary>
        /// Destination X Coordinate.
        /// </summary>
        public short DestinationX { get => destinationX; set => destinationX = value; }
        /// <summary>
        /// Destination Y Coordinate.
        /// </summary>
        public short DestinationY { get => destinationY; set => destinationY = value; }
        /// <summary>
        /// Map Coordinate of Destination.
        /// </summary>
        public GeoPoint Destination
        {
            get => new(destinationX, destinationY);
            set
            {
                destinationX = (short)value.X;
                destinationY = (short)value.Y;
            }
        }
        /// <summary>
        /// Target ID.
        /// </summary>
        public VirtualUniqueIdentifier TargetID { get => targetID; set => targetID = value; }
        /// <summary>
        /// Cargo ID.
        /// </summary>
        public VirtualUniqueIdentifier CargoID { get => cargoID; set => cargoID = value; }
        /// <summary>
        /// Unit Movement indicator.
        /// </summary>
        public byte Moved { get => moved; set => moved = value; }
        /// <summary>
        /// Unit Losses.
        /// </summary>
        public byte Losses { get => losses; set => losses = value; }
        /// <summary>
        /// Tactics being used by the Unit.
        /// </summary>
        public byte Tactic { get => tactic; set => tactic = value; } // TODO: Enum.
        /// <summary>
        /// Current Waypoint Index.
        /// </summary>
        public ushort CurrentWaypoint { get => currentWP; set => currentWP = value; }
        /// <summary>
        /// Unit Name ID.
        /// </summary>
        public short NameID { get => nameID; set => nameID = value; }
        /// <summary>
        /// Reinforcements Required.
        /// </summary>
        public short Reinforcement { get => reinforcement; set => reinforcement = value; } // TODO: Confirm usage.
        /// <summary>
        /// Number of Waypoints the Unit has.
        /// </summary>
        public ushort WaypointCount { get => (ushort)waypoints.Count; }
        /// <summary>
        /// Collection of Unit Waypoints.
        /// </summary>
        public Collection<Waypoint> Waypoints { get => waypoints; set => waypoints = value; }
        #endregion Properties

        #region Public Fields
        private short unitType = 0;
        private uint lastCheck = 0;
        private int roster = 0;
        private int unitFlags = 0;
        private short destinationX = 0;
        private short destinationY = 0;
        private VirtualUniqueIdentifier targetID = new();
        private VirtualUniqueIdentifier cargoID = new();
        private byte moved = 0;
        private byte losses = 0;
        private byte tactic = 0;
        private ushort currentWP = 0;
        private short nameID = 0;
        private short reinforcement = 0;
        private ushort numWaypoints = 0;
        private Collection<Waypoint> waypoints = [];
        #endregion

        #region Helper Methods
        internal new void Read(Stream stream)
        {
            using BinaryReader reader = new(stream, Encoding.Default, leaveOpen: true);
            lastCheck = reader.ReadUInt32();
            roster = reader.ReadInt32();
            unitFlags = reader.ReadInt32();
            destinationX = reader.ReadInt16();
            destinationY = reader.ReadInt16();
            targetID = new()
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32()
            };
            cargoID = new()
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32()
            };
            moved = reader.ReadByte();
            losses = reader.ReadByte();
            tactic = reader.ReadByte();
            currentWP = reader.ReadUInt16();
            nameID = reader.ReadInt16();
            reinforcement = reader.ReadInt16();
            numWaypoints = reader.ReadUInt16();
            if (numWaypoints > 500) return;
            waypoints = [];
            for (int i = 0; i < numWaypoints; i++)
                waypoints.Add(new Waypoint(stream, version));

        }
        internal new void Write(Stream stream)
        {
            base.Write(stream);
            using var writer = new BinaryWriter(stream, Encoding.Default, leaveOpen: true);
            writer.Write(lastCheck);
            writer.Write(roster);
            writer.Write(unitFlags);
            writer.Write(destinationX);
            writer.Write(destinationY);
            writer.Write(targetID.ID);
            writer.Write(targetID.Creator);
            writer.Write(cargoID.ID);
            writer.Write(cargoID.Creator);
            writer.Write(moved);
            writer.Write(losses);
            writer.Write(tactic);
            writer.Write(currentWP);
            writer.Write(nameID);
            writer.Write(reinforcement);
            writer.Write(waypoints.Count);
            for (int i = 0; i < waypoints.Count; i++)
                waypoints[i].Write(stream);
        }
        #endregion Helper Methods

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.AppendLine("Unit Type: " + UnitType);
            sb.AppendLine("Last Check: " + LastCheck);
            sb.AppendLine("Roster: " + Roster);
            sb.AppendLine("Unit Flags: " + UnitFlags);
            sb.AppendLine("Destination: " + Destination.ToString());
            sb.AppendLine("***** Target ID *****");
            sb.Append(TargetID.ToString());
            sb.AppendLine("***** Cargo ID *****");
            sb.Append(cargoID.ToString());
            sb.AppendLine("Moved: " + Moved);
            sb.AppendLine("Losses: " + Losses);
            sb.AppendLine("Tactic: " + Tactic);
            sb.AppendLine("Current Waypoint: " + CurrentWaypoint);
            sb.AppendLine("Name ID: " + NameID);
            sb.AppendLine("Reinforcements: " + Reinforcement);
            sb.AppendLine("Waypoints: ");
            for (int i = 0; i < Waypoints.Count; i++)
            {
                sb.AppendLine("***** Waypoint " + i + " *****");
                sb.Append(waypoints[i].ToString());
            }


            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Unit Constructor
        /// </summary>
        public Unit()
            : base()
        {
        }
        /// <summary>
        /// Intitializes a <see cref="Unit"/> object with the supplied data.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object with the initializationd data to read.</param>
        /// <param name="version">File Version.</param>
        public Unit(Stream stream, int version)
            : base(stream, version)
        {
            Read(stream);
        }
        #endregion Constructors
    }
}
