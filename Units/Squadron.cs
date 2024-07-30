using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using FalconCampaign.Components;

namespace FalconCampaign.Units
{
    /// <summary>
    /// A Squadron in the Campaign.
    /// </summary>
    public class Squadron : AirUnit
    {
        #region Properties
        /// <summary>
        /// Squadron Fuel Level.
        /// </summary>
        public int Fuel { get => fuel; set => fuel = value; }
        /// <summary>
        /// Squadron Specialty.
        /// </summary>
        public byte Specialty { get => specialty; set => specialty = value; } // TODO: This is an enum
        /// <summary>
        /// Collection of Squadron Stores values, limited to 1000.
        /// </summary>
        public Collection<byte> Stores { get => stores; set => stores = value; }
        /// <summary>
        /// Pilots assigned to the Squadron, limited to 48.
        /// </summary>
        public Collection<Pilot.Pilot> Pilots { get => pilots; set => pilots = value; }
        /// <summary>
        /// Flight IDs on the Squadron Schedule, limited to 16.
        /// </summary>
        public Collection<int> Schedule { get => schedule; set => schedule = value; }
        /// <summary>
        /// Index into the Airbase Table the Squadron is assigned to.
        /// </summary>
        public VirtualUniqueIdentifier AirbaseID { get => airbaseID; set => airbaseID = value; }
        /// <summary>
        /// Specific area of the Theater the Squadron focuses on.
        /// </summary>
        public VirtualUniqueIdentifier HotSpot { get => hotSpot; set => hotSpot = value; } // TODO: Is this a PAKID?
        /// <summary>
        /// Placeholder used while parsing the underlying database.
        /// </summary>
        public VirtualUniqueIdentifier Junk { get => junk; set => junk = value; }
        /// <summary>
        /// Squadron ratings against certain types of targets, limited to 16.
        /// </summary>
        public Collection<byte> Rating { get => rating; set => rating = value; }
        /// <summary>
        /// Number of A-A Kills.
        /// </summary>
        public short KillsAA { get => aaKills; set => aaKills = value; }
        /// <summary>
        /// Number of A-G Kills.
        /// </summary>
        public short KillsAG { get => agKills; set => agKills = value; }
        /// <summary>
        /// Number of Static Target Kills.
        /// </summary>
        public short KillsAS { get => asKills; set => asKills = value; }
        /// <summary>
        /// Number of Naval Kills.
        /// </summary>
        public short KillsAN { get => anKills; set => anKills = value; }
        /// <summary>
        /// Number of Missions Flown by this Squadron.
        /// </summary>
        public short MissionCount { get => missionsFlown; set => missionsFlown = value; }
        /// <summary>
        /// Accumulated Mission Scores for the Squadron.
        /// </summary>
        public short MissionScore { get => missionScore; set => missionScore = value; }
        /// <summary>
        /// Losses sustained by the Squadron.
        /// </summary>
        public byte TotalLosses { get => totalLosses; set => totalLosses = value; }
        /// <summary>
        /// Pilot Losses sustained by the Squadron.
        /// </summary>
        public byte PilotLosses { get => pilotLosses; set => pilotLosses = value; }
        /// <summary>
        /// Squadron Patch ID.
        /// </summary>
        public ushort SquadronPatchID { get => squadronPatch; set => squadronPatch = value; }
        /// <summary>
        /// Collection of Campaign Specific Ratings for the Squadron.
        /// </summary>
        public Collection<byte> CampaignRatings { get => campaignRatings; set => campaignRatings = value; }
        /// <summary>
        /// Time when the Unit will be retasked.
        /// </summary>
        public TimeSpan RetaskTime { get => new(0,0,0,0,(int)retaskTime); set => retaskTime = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// The Default Skin to use.
        /// </summary>
        public uint DefaultSkin { get => defaultSkin; set => defaultSkin = value; }
        #endregion Properties

        #region Fields
        private int fuel = 0;
        private byte specialty = 0;
        private Collection<byte> campaignRatings = [];
        private Collection<byte> stores = [];
        private Collection<Pilot.Pilot> pilots = [];
        private Collection<int> schedule = [];
        private VirtualUniqueIdentifier airbaseID = new();
        private VirtualUniqueIdentifier hotSpot = new();
        private VirtualUniqueIdentifier junk = new();
        private Collection<byte> rating = [];
        private short aaKills = 0;
        private short agKills = 0;
        private short asKills = 0;
        private short anKills = 0;
        private short missionsFlown = 0;
        private short missionScore = 0;
        private byte totalLosses = 0;
        private byte pilotLosses = 0;
        private ushort squadronPatch = 0;
        private uint retaskTime = 0;
        private uint defaultSkin = 0;


        #endregion Fields

        #region Helper Methods
        internal new void Read(Stream stream)
        {
            using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
            fuel = reader.ReadInt32();
            specialty = reader.ReadByte();
            for (int j = 0; j < 16; j++)
                campaignRatings.Add(reader.ReadByte());

            Stores = [];
            for (int i = 0; i < 1000; i++)
                Stores.Add(reader.ReadByte());

            Pilots = [];
            for (int j = 0; j < 48; j++)
            {
                Pilot.Pilot thisPilot = new()
                {
                    PilotID = reader.ReadInt16(),
                    PilotSkillAndRating = reader.ReadByte(),
                    PilotStatus = reader.ReadByte(),
                    KillsAA = reader.ReadByte(),
                    KillsAG = reader.ReadByte(),
                    KillsAS = reader.ReadByte(),
                    KillsAN = reader.ReadByte(),
                    MissionCount = reader.ReadInt16()
                };
                pilots.Add(thisPilot);
            }

            schedule = [];
            for (int j = 0; j < 16; j++)
                Schedule.Add(reader.ReadInt32());

            airbaseID = new()
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32()
            };
            hotSpot = new()
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32()
            };

            rating = [];
            for (int j = 0; j < 16; j++)
                rating.Add(reader.ReadByte());

            aaKills = reader.ReadInt16();
            agKills = reader.ReadInt16();
            asKills = reader.ReadInt16();
            anKills = reader.ReadInt16();
            missionsFlown = reader.ReadInt16();
            missionScore = reader.ReadInt16();
            totalLosses = reader.ReadByte();
            pilotLosses = reader.ReadByte();
            squadronPatch = reader.ReadUInt16();   // This changed from byte to ushort          
            retaskTime = reader.ReadUInt32();
            Trace.WriteLine(reader.ReadByte()); // Value always seems to be 0--Alignment byte?
            defaultSkin = reader.ReadUInt32();  
            
            
        }

        internal new void Write(Stream stream)
        {
            base.Write(stream);
            using var writer = new BinaryWriter(stream, Encoding.Default, leaveOpen: true);
            writer.Write(fuel);
            writer.Write(specialty);
            while (stores.Count < 1000) stores.Add(0);
            for (int i = 0; i < 1000; i++)
                writer.Write(stores[i]);
            while (pilots.Count < 48) pilots.Add(new Pilot.Pilot());
            for (int j = 0; j < 48; j++)
            {
                var thisPilot = pilots[j];
                writer.Write(thisPilot.PilotID);
                writer.Write(thisPilot.PilotSkillAndRating);
                writer.Write(thisPilot.PilotStatus);
                writer.Write(thisPilot.KillsAA);
                writer.Write(thisPilot.KillsAG);
                writer.Write(thisPilot.KillsAS);
                writer.Write(thisPilot.KillsAN);
                writer.Write(thisPilot.MissionCount);
            }

            while (schedule.Count < 16) schedule.Add(0);
            for (int j = 0; j < 16; j++)
                writer.Write(Schedule[j]);

            writer.Write(airbaseID.ID);
            writer.Write(airbaseID.Creator);
            writer.Write(hotSpot.ID);
            writer.Write(hotSpot.Creator);
            while (rating.Count < 16) rating.Add(0);
            for (int j = 0; j < 16; j++)
                writer.Write(Rating[j]);

            writer.Write(aaKills);
            writer.Write(agKills);
            writer.Write(asKills);
            writer.Write(anKills);
            writer.Write(missionsFlown);
            writer.Write(missionScore);
            writer.Write(totalLosses);
            writer.Write(pilotLosses);
            writer.Write(squadronPatch);
            writer.Write(retaskTime);
            writer.Write((byte)0); // Alignment?
            writer.Write(defaultSkin);
        }
        #endregion Helper Methods

        #region Functional Methods

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append(base.ToString());
            sb.AppendLine("Fuel: " + Fuel);
            sb.AppendLine("Specialty: " + Specialty);
            sb.AppendLine("Stores: ");
            for (int i = 0; i < Stores.Count; i++)
            {
                sb.AppendLine("   " + Stores[i].ToString());
            }
            sb.AppendLine("Pilots: ");
            for (int i = 0; i < Pilots.Count; i++)
            {
                sb.AppendLine("***** Pilot " + i + " *****");
                sb.Append(Pilots[i].ToString());
            }
            sb.AppendLine("Schedule: ");
            for (int i = 0; i < Schedule.Count; i++)
                sb.AppendLine("   Activity: " + i + ": " + Schedule[i].ToString());
            sb.AppendLine("Airbase ID: " + AirbaseID.ID);
            sb.AppendLine("Hot Spot: " + HotSpot.ID);
            sb.AppendLine("Junk: " + Junk.ID);
            sb.AppendLine("Ratings: " + Rating);
            for (int i = 0; i < Rating.Count; i++)
                sb.AppendLine("   Rating " + i + ": " + Rating[i].ToString());
            sb.AppendLine("A-A Kills: " + KillsAA);
            sb.AppendLine("A-G Kills: " + KillsAG);
            sb.AppendLine("A-S Kills: " + KillsAS);
            sb.AppendLine("A-N Kills: " + KillsAN);
            sb.AppendLine("Missions Flown: " + MissionCount);
            sb.AppendLine("Mission Score: " + MissionScore);
            sb.AppendLine("Total Losses: " + TotalLosses);
            sb.AppendLine("Patch ID: " + SquadronPatchID);

            return sb.ToString();

        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Squadron Constructor.
        /// </summary>
        protected Squadron()
           : base()
        {
        }
        /// <summary>
        /// Initializes a Squadron with the data supplied.
        /// </summary>
        /// <param name="stream">Data stream with initialization data.</param>
        /// <param name="version">File Version.</param>
        public Squadron(Stream stream, int version)
            : base(stream, version)
        {
            Read(stream);
        }
        #endregion Constructors
    }
}
