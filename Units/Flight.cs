using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using FalconCampaign.Components;

namespace FalconCampaign.Units
{
    /// <summary>
    /// A Flight in the Campaign Engine.
    /// </summary>
    public class Flight : AirUnit
    {
        #region Properties
        /// <summary>
        /// Amount of Fuel Consumed in the Flight.
        /// </summary>
        public int FuelBurnt { get => fuel_burnt; set => fuel_burnt = value; }
        /// <summary>
        /// When the Flight was last upadated.
        /// </summary>
        public TimeSpan LastMove { get => new(0, 0, 0, 0, (int)lastMove); set => lastMove = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// When the Flight was last in Combat.
        /// </summary>
        public TimeSpan LastCombat { get => new(0, 0, 0, 0, (int)lastCombat); set => lastCombat = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Time On Target.
        /// </summary>
        public TimeSpan TimeOnTarget { get => new(0, 0, 0, 0, (int)timeOnTarget); set => timeOnTarget = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Mission Completion Time.
        /// </summary>
        public TimeSpan MissionEndTime { get => new(0, 0, 0, 0, (int)missionEndTime); set => missionEndTime = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// ID of the Mission Target.
        /// </summary>
        public short MissionTargetID { get => missionTarget; set => missionTarget = value; }
        /// <summary>
        /// When <see langword="true"/> indicates this Loadout should be used.
        /// </summary>
        public bool UseLoadout { get => useLoadout > 0; set => useLoadout = value == true ? (sbyte)1 : (sbyte)0; }
        /// <summary>
        /// Collection of Weapons available to the Flight.
        /// </summary>
        public Collection<byte> Weapons { get => weapons; set => weapons = value; }
        /// <summary>
        /// Number of Loadouts in the Flight.
        /// </summary>
        public byte Loadouts { get => (byte)loadout.Count; }
        /// <summary>
        /// Collection of Loadouts available to the Flight
        /// </summary>
        public Collection<LoadoutEntry> Loadout { get => loadout; set => loadout = value; } // CHANGED this from Loadout Entry ***
        /// <summary>
        /// Collection of Weapons available to the Flight.
        /// </summary>
        public Collection<short> Weapon { get => weapon; set => weapon = value; }
        /// <summary>
        /// Mission Identifier.
        /// </summary>
        public byte Mission { get => mission; set => mission = value; }
        /// <summary>
        /// Indicates a previous or completed Mission?
        /// </summary>
        public byte OldMission { get => oldMission; set => oldMission = value; } // TODO: Bool? Verify usage.
        /// <summary>
        /// Last Cardinal Direction the Flight was moving.
        /// </summary>
        public byte LastDirection { get => lastDirection; set => lastDirection = value; } // TODO: Enum? Could be 8 vs 4?
        /// <summary>
        /// Mission Priority.
        /// </summary>
        public byte Priority { get => priority; set => priority = value; } // TODO: Enum?
        /// <summary>
        /// Mission Identifier.
        /// </summary>
        public byte MissionID { get => missionID; set => missionID = value; }
        /// <summary>
        /// Flag values for the Flight.
        /// </summary>
        public byte EvalFlags { get => evalFlags; set => evalFlags = value; }
        /// <summary>
        /// Reason the Mission exists.
        /// </summary>
        public byte MissionContext { get => mission_context; set => mission_context = value; }
        /// <summary>
        /// Package Information ID.
        /// </summary>
        public VirtualUniqueIdentifier Package { get => package; set => package = value; }
        /// <summary>
        /// Squadron Information ID.
        /// </summary>
        public VirtualUniqueIdentifier Squadron { get => squadron; set => squadron = value; }
        /// <summary>
        /// Requester Information ID.
        /// </summary>
        public VirtualUniqueIdentifier Requester { get => requester; set => requester = value; }
        /// <summary>
        /// Collection of Aircraft IDs in the Flight.
        /// </summary>
        public Collection<byte> Slots { get => slots; set => slots = value; }
        /// <summary>
        /// Collection of Pilot IDs in the Flight.
        /// </summary>
        public Collection<byte> Pilots { get => pilots; set => pilots = value; }
        /// <summary>
        /// Collection Aircraft Statistics IDs in the Flight.
        /// </summary>
        public Collection<byte> PlaneStats { get => planeStats; set => planeStats = value; }
        /// <summary>
        /// Indicates which Slots have Players assigned.
        /// </summary>
        public Collection<byte> PlayerSlots { get => playerSlots; set => playerSlots = value; } // TODO: Bool?
        /// <summary>
        /// The last Slot in the Flight with a Player assigned. Determines if the Campaign Engine needs to fill other Slots or maintain a 3D bubble.
        /// </summary>
        public byte LastPlayerSlot { get => lastPlayerSlot; set => lastPlayerSlot = value; }
        /// <summary>
        /// Index into the list of Callsign Names.
        /// </summary>
        public byte CallsignID { get => callsignID; set => callsignID = value; }
        /// <summary>
        /// Number assigned to the Callsign.
        /// </summary>
        public byte CallsignNumber { get => callsignNumber; set => callsignNumber = value; }
        /// <summary>
        /// Amount of Fuel the Flight requires from a Tanker during the Flight.
        /// </summary>
        public uint RefuelQuantity { get => refuelQuantity; set => refuelQuantity = value; }
        #endregion Properties

        #region Fields

        private int fuel_burnt = 0;
        private uint lastMove = 0;
        private uint lastCombat = 0;
        private uint timeOnTarget = 0;
        private uint missionEndTime = 0;
        private short missionTarget = 0;
        private sbyte useLoadout = 0;
        private Collection<byte> weapons = [];
        private byte loadouts = 0;
        private Collection<LoadoutEntry> loadout = [];
        private Collection<short> weapon = [];
        private byte mission = 0;
        private byte oldMission = 0;
        private byte lastDirection = 0;
        private byte priority = 0;
        private byte missionID = 0;
        private byte dummy = 0;
        private byte evalFlags = 0;
        private byte mission_context = 0; // TODO: Enum?
        private VirtualUniqueIdentifier package = new();
        private VirtualUniqueIdentifier squadron = new();
        private VirtualUniqueIdentifier requester = new();
        private Collection<byte> slots = [];
        private Collection<byte> pilots = [];
        private Collection<byte> planeStats = [];
        private Collection<byte> playerSlots = [];
        private byte lastPlayerSlot = 0;
        private byte callsignID = 0;
        private byte callsignNumber = 0;
        private uint refuelQuantity = 0;
        private float zPos = 0;


        #endregion Fields

        #region Helper Methods
        internal new void Read(Stream stream)
        {
            using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
            // TODO: What is this? -- Might be 134, found missing for loop of 16 bytes
            reader.ReadBytes(150);

            zPos = reader.ReadSingle(); // 150
            fuel_burnt = reader.ReadInt32(); // 154                    
            lastMove = reader.ReadUInt32(); // 158
            lastCombat = reader.ReadUInt32(); // 162
            timeOnTarget = reader.ReadUInt32();  //166
            missionEndTime = reader.ReadUInt32();   // 170                 
            missionTarget = reader.ReadInt16(); // 174
                                                //TODO: Debugging-remove this.
            Trace.WriteLine(LastMove.ToString("g"));
            Trace.WriteLine(LastCombat.ToString("g"));
            Trace.WriteLine(TimeOnTarget.ToString("g"));
            Trace.WriteLine(MissionEndTime.ToString("g"));

            loadouts = reader.ReadByte(); // 176
            loadout = [];
            for (int j = 0; j < loadouts; j++)
            {
                LoadoutEntry thisLoadout = new()
                {
                    WeaponID = [],
                    WeaponCount = [],
                };
                for (int k = 0; k < 16; k++)
                    thisLoadout.WeaponID.Add(reader.ReadUInt16()); // 177
                thisLoadout.WeaponCount = [];
                for (int k = 0; k < 16; k++)
                    thisLoadout.WeaponCount.Add(reader.ReadByte()); // 209

                loadout.Add(thisLoadout);
            }
            mission = reader.ReadByte(); // 225
            oldMission = reader.ReadByte(); // 226               
            lastDirection = reader.ReadByte();  // 227
            priority = reader.ReadByte();  // 228
            missionID = reader.ReadByte();  // 229               
            evalFlags = reader.ReadByte(); // 230
            mission_context = reader.ReadByte(); // 231    
            package = new() // 232
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32()
            };
            squadron = new() // 240
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32()
            };
            requester = new() // 248
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32()
            };
            slots = [];
            for (int j = 0; j < 4; j++)
                slots.Add(reader.ReadByte()); // 256
            pilots = [];
            for (int j = 0; j < 4; j++)
                pilots.Add(reader.ReadByte()); // 260
            planeStats = [];
            for (int j = 0; j < 4; j++)
                planeStats.Add(reader.ReadByte()); // 264
            playerSlots = [];
            for (int j = 0; j < 4; j++)
                playerSlots.Add(reader.ReadByte());  // 268
            lastPlayerSlot = reader.ReadByte(); // 272
            callsignID = reader.ReadByte();
            callsignNumber = reader.ReadByte();  // 274
            refuelQuantity = reader.ReadUInt32(); // 275

            //Total 279
            // Buffer to align on int32 boundary?
            reader.ReadByte();
        }

        internal new void Write(Stream stream)
        {
            base.Write(stream);
            using var writer = new BinaryWriter(stream, Encoding.Default, leaveOpen: true);
            // Unknown
            writer.Write(new byte[150]);

            writer.Write(Z);
            writer.Write(fuel_burnt);
            writer.Write(lastMove);
            writer.Write(lastCombat);
            writer.Write(timeOnTarget);
            writer.Write(missionEndTime);
            writer.Write(missionTarget);
            writer.Write(Loadouts);
            for (int j = 0; j < Loadouts; j++)
            {
                var thisLoadout = Loadout[j];
                for (int k = 0; k < 16; k++)
                    writer.Write(thisLoadout.WeaponID[k]);
                for (int k = 0; k < 16; k++)
                    writer.Write(thisLoadout.WeaponCount[k]);
            }
            writer.Write(Mission);
            writer.Write(OldMission);
            writer.Write(lastDirection);
            writer.Write(priority);
            writer.Write(missionID);
            writer.Write(evalFlags);
            writer.Write(mission_context);
            writer.Write(package.ID);
            writer.Write(package.Creator);
            writer.Write(squadron.ID);
            writer.Write(squadron.Creator);
            writer.Write(requester.ID);
            writer.Write(requester.Creator);
            for (int j = 0; j < 4; j++)
                writer.Write(slots[j]);
            for (int j = 0; j < 4; j++)
                writer.Write(pilots[j]);
            for (int j = 0; j < 4; j++)
                writer.Write(planeStats[j]);
            for (int j = 0; j < 4; j++)
                writer.Write(playerSlots[j]);
            writer.Write(lastPlayerSlot);
            writer.Write(callsignID);
            writer.Write(callsignNumber);
            writer.Write(refuelQuantity);
            //buffer?
            writer.Write((byte)0);
        }
        #endregion Helper Methods

        #region Functional Methods

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append(base.ToString());
            sb.AppendLine("Fuel Burnt: " + FuelBurnt);
            sb.AppendLine("Last Move: " + LastMove.ToString("g"));
            sb.AppendLine("Last Combat: " + LastCombat.ToString("g"));
            sb.AppendLine("Time on Target: " + TimeOnTarget.ToString("g"));
            sb.AppendLine("Mission Over Time: " + MissionEndTime.ToString("g"));
            sb.AppendLine("Mission Target: " + MissionTargetID);
            sb.AppendLine("Use Loadout: " + UseLoadout);
            sb.AppendLine("Loadout: ");
            for (int i = 0; i < Loadout.Count; i++)
                sb.AppendLine("   Weapon ID: " + Loadout[i].WeaponID + " x " + Loadout[i].WeaponCount);
            sb.AppendLine("Weapons: ");
            for (int i = 0; i < Weapon.Count; i++)
                sb.AppendLine("   Weapon " + i + ": " + Weapon[i]);
            sb.AppendLine("Mission: " + Mission);
            sb.AppendLine("Old Mission: " + OldMission);
            sb.AppendLine("Last Direction: " + LastDirection);
            sb.AppendLine("Priority: " + Priority);
            sb.AppendLine("Mission ID: " + MissionID);
            sb.AppendLine("Eval Flags: " + EvalFlags);
            sb.AppendLine("Mission Context: " + MissionContext);
            sb.AppendLine("Package ID: " + Package.ID);
            sb.AppendLine("Squadron ID: " + Squadron.ID);
            sb.AppendLine("Requester: " + Requester.ID);
            sb.AppendLine("Slots: ");
            for (int i = 0; i < Slots.Count; i++)
                sb.AppendLine("   Slot " + i + ": " + Slots[i]);
            sb.AppendLine("Pilots: ");
            for (int i = 0; i < Pilots.Count; i++)
                sb.AppendLine("   Pilot " + i + ": " + Pilots[i]);
            sb.AppendLine("Aircraft Stats: ");
            for (int i = 0; i < PlaneStats.Count; i++)
                sb.AppendLine("   Aircraft " + i + ": " + PlaneStats[i]);
            sb.AppendLine("Aircraft Slots: ");
            for (int i = 0; i < PlayerSlots.Count; i++)
                sb.AppendLine("   Aircraft " + i + ": " + PlaneStats[i]);
            sb.AppendLine("Last Player Slot: " + LastPlayerSlot);
            sb.AppendLine("Callsign ID: " + CallsignID);
            sb.AppendLine("Callsign Number: " + CallsignNumber);
            sb.AppendLine("Refuel Quantity: " + RefuelQuantity);

            return base.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for <see cref="Flight"/>.
        /// </summary>
        protected Flight()
           : base()
        {
        }
        /// <summary>
        /// Constructor for <see cref="Flight"/> using the <see cref="Stream"/> object to read the data.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object containing the data to read.</param>
        /// <param name="version">File Version.</param>
        public Flight(Stream stream, int version)
            : base(stream, version)
        {
            Read(stream);
        }
        #endregion Constructors
    }
}
