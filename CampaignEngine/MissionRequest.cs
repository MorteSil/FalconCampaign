using FalconCampaign.Components;
using FalconCampaign.Enums;
using FalconDatabase.Enums;
using System.Collections.ObjectModel;
using System.Text;
using Utilities.GeoLib;

namespace FalconCampaign.CampaignEngine
{
    /// <summary>
    /// A Request Entity for the Campaign Engine to react to an event.
    /// </summary>
    public class MissionRequest
    {
        #region Properties
        /// <summary>
        /// The ID of the Entity that requested the Mission.
        /// </summary>
        public VirtualUniqueIdentifier RequesterID { get => requesterID; set => requesterID = value; }
        /// <summary>
        /// The Target of the Mission Request.
        /// </summary>
        public VirtualUniqueIdentifier TargetID { get => targetID; set => targetID = value; }
        /// <summary>
        /// Secondary Target ID for the Mission.
        /// </summary>
        public VirtualUniqueIdentifier SecondaryID { get => secondaryID; set => secondaryID = value; }
        /// <summary>
        /// PAK Reagion of the Mission Location.
        /// </summary>
        public VirtualUniqueIdentifier PakID { get => pakID; set => pakID = value; }
        /// <summary>
        /// The Entity being assigned.
        /// </summary>
        public CountryList Who { get => (CountryList)who; set => who = (byte)value; }
        /// <summary>
        /// The Opposing Entity (Target)
        /// </summary>
        public CountryList Vs { get => (CountryList)vs; set => vs = (byte)value; }
        /// <summary>
        /// Desired Time when the Units should arrive for execution.
        /// </summary>
        public TimeSpan ToT { get => new(0, 0, 0, 0, (int)tot); set => tot = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Target X Coordinate.
        /// </summary>
        public short Tx { get => tx; set => tx = value; }
        /// <summary>
        /// Target Y Coordinate.
        /// </summary>
        public short Ty { get => ty; set => ty = value; }
        /// <summary>
        /// Location of the Target.
        /// </summary>
        public GeoPoint Location
        {
            get => new(tx, ty);
            set
            {
                tx = (short)value.X;
                ty = (short)value.Y;
            }
        }
        /// <summary>
        /// Mission Request Flags
        /// </summary>
        public uint Flags { get => flags; set => flags = value; } // TODO: Map this
        /// <summary>
        /// Required Capabilities for the Mission.
        /// </summary>
        public short Caps { get => caps; set => caps = value; } // TODO: Confirm usage
        /// <summary>
        /// Number of Targets
        /// </summary>
        public short TargetCount { get => targetCount; set => targetCount = value; }
        /// <summary>
        /// Speed Value for the Request.
        /// </summary>
        public short Speed { get => speed; set => speed = value; }
        /// <summary>
        /// Opponent Strength to Match with the Request.
        /// </summary>
        public short MatchStrength { get => matchStrength; set => matchStrength = value; }
        /// <summary>
        /// Mission Priority.
        /// </summary>
        public short Priority { get => priority; set => priority = value; }
        /// <summary>
        /// Time on Target Type. Determines Station Time.
        /// </summary>
        public byte ToTType { get => totType; set => totType = value; } // TODO: This might be whether it's a TOT or Takeoff Locked package timing?
        /// <summary>
        /// Action Type of the Mission being Requested.
        /// </summary>
        public byte ActionType { get => actionType; set => actionType = value; }
        /// <summary>
        /// Mission Identifier.
        /// </summary>
        public byte Mission { get => mission; set => mission = value; }
        /// <summary>
        /// Aircraft Type Identifier.
        /// </summary>
        public byte Aircraft { get => aircraft; set => aircraft = value; }
        /// <summary>
        /// Reason the Mission was Requested.
        /// </summary>
        public MissionContext Context { get => (MissionContext)context; set => context = (byte)value; }
        /// <summary>
        /// Is the Request within RoEs.
        /// </summary>
        public byte RoECheck { get => roeCheck; set => roeCheck = value; }
        /// <summary>
        /// How many times has the Request been delayed.
        /// </summary>
        public byte Delayed { get => delayed; set => delayed = value; }
        /// <summary>
        /// Time Block ID for Takeoff.
        /// </summary>
        public byte StartBlock { get => startBlock; set => startBlock = value; }
        /// <summary>
        /// Time Block ID for Landing.
        /// </summary>
        public byte FinalBlock { get => finalBlock; set => finalBlock = value; }
        /// <summary>
        /// Minimum Time Block where assets are available.
        /// </summary>
        public sbyte MinimumTakeoff { get => minTakeoff; set => minTakeoff = value; }
        /// <summary>
        /// Maximum Time Block where assets are available.
        /// </summary>
        public sbyte MaxTakeoff { get => maxTakeoff; set => maxTakeoff = value; }
        /// <summary>
        /// Collection of Slots in the Mission Request.
        /// </summary>
        public Collection<byte> Slots { get => slots; set => slots = value; }
        #endregion Properties

        #region Fields
        private VirtualUniqueIdentifier requesterID = new();
        private VirtualUniqueIdentifier targetID = new();
        private VirtualUniqueIdentifier secondaryID = new();
        private VirtualUniqueIdentifier pakID = new();
        private byte who = 0;
        private byte vs = 0;
        private uint tot = 0;
        private short tx = 0;
        private short ty = 0;
        private uint flags = 0;
        private short caps = 0;
        private short targetCount = 0;
        private short speed = 0;
        private short matchStrength = 0;
        private short priority = 0;
        private byte totType = 0;
        private byte actionType = 0;
        private byte mission = 0;
        private byte aircraft = 0;
        private byte context = 0;
        private byte roeCheck = 0;
        private byte delayed = 0;
        private byte startBlock = 0;
        private byte finalBlock = 0;
        private Collection<byte> slots = [];
        private sbyte minTakeoff = 0;
        private sbyte maxTakeoff = 0;

        #endregion Fields

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Requestor ID: " + requesterID.ID);
            sb.AppendLine("Target ID: " + targetID.ID);
            sb.AppendLine("Secondary Target ID: " + secondaryID.ID);
            sb.AppendLine("PAK ID: " + pakID.ID);
            sb.AppendLine("Who: " + who);
            sb.AppendLine("Vs: " + vs);
            sb.AppendLine("Target Location:");
            sb.Append(Location.ToString());
            sb.AppendLine("Flags: " + flags);
            sb.AppendLine("Capabilities: " + caps);
            sb.AppendLine("Target Time: " + ToT.ToString("g"));
            sb.AppendLine("Speed: " + speed);
            sb.AppendLine("Opponent Strength: " + matchStrength);
            sb.AppendLine("Priority: " + priority);
            sb.AppendLine("ToT Type:" + totType);
            sb.AppendLine("Action Type: " + actionType);
            sb.AppendLine("Mission" + mission);
            sb.AppendLine("Aircraft: " + aircraft);
            sb.AppendLine("Context: " + context);
            sb.AppendLine("RoE Check:" + roeCheck);
            sb.AppendLine("Delayed Count: " + delayed);
            sb.AppendLine("Start Block: " + startBlock);
            sb.AppendLine("Final Block: " + finalBlock);
            sb.AppendLine("Squadrons: ");
            for (int i = 0; i < slots.Count; i++)
            {
                sb.Append("   Squadron ID: " + slots[i]);
            }
            sb.AppendLine("Minimum Takeoff: " + minTakeoff);
            sb.AppendLine("Maximum Takeoff: " + maxTakeoff);
            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="MissionRequest"/> object.
        /// </summary>
        public MissionRequest() { }

        #endregion Constructors
    }
}
