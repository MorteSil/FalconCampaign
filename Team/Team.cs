using FalconCampaign.Components;
using FalconCampaign.Enums;
using FalconDatabase.Enums;
using System.Collections.ObjectModel;
using System.Text;

namespace FalconCampaign.Team
{
    /// <summary>
    /// A Team object in the Campaign Engine.
    /// </summary>
    public class Team
    {
        #region Properties
        /// <summary>
        /// Team Identifier.
        /// </summary>
        public VirtualUniqueIdentifier ID { get => id; set => id = value; }
        /// <summary>
        /// Entity Type Identifier.
        /// </summary>
        public ushort EntityType { get => entityType; set => entityType = value; } // TODO: Enum or CT Index?
        /// <summary>
        /// Country Identifier.
        /// </summary>
        public CountryList Who { get => (CountryList)who; set => who = (byte)value; }
        /// <summary>
        /// Which Major Coalition Team this Team belongs to.
        /// </summary>
        public CountryList CoalitionTeam { get => (CountryList)cteam; set => cteam = (byte)value; }
        /// <summary>
        /// Team Flags.
        /// </summary>
        public short Flags { get => flags; set => flags = value; } // TODO: Map this
        /// <summary>
        /// Countries that are part of this Team, limited to 8.
        /// </summary>
        public Collection<byte> Member
        {
            get => member;
            set
            {
                while (value.Count > 16) value.RemoveAt(16);
                member = value;
            }
        }
        /// <summary>
        /// Attitude toward other Countries and Teams, limited to 8.
        /// </summary>
        public Collection<short> Stance
        {
            get => stance;
            set
            {
                while (value.Count > 16) value.RemoveAt(16);
                stance = value;
            }
        }
        /// <summary>
        /// Index into the Pilot List of the First Colonel.
        /// </summary>
        public short FirstColonel { get => firstColonel; set => firstColonel = value; }
        /// <summary>
        /// Index into the Pilot List of the First Commander.
        /// </summary>
        public short FirstCommander { get => firstCommander; set => firstCommander = value; }
        /// <summary>
        /// Index into the Pilot List of the First Wingman.
        /// </summary>
        public short FirstWingman { get => firstWingman; set => firstWingman = value; }
        /// <summary>
        /// Index into the Pilot List of the Last Wingman.
        /// </summary>
        public short LastWingman { get => lastWingman; set => lastWingman = value; }
        /// <summary>
        /// Air Unit Experience Level.
        /// </summary>
        public byte AirExperience { get => airExperience; set => airExperience = value; }
        /// <summary>
        /// Air Defense Unit Experience Level.
        /// </summary>
        public byte AirDefenseExperience { get => airDefenseExperience; set => airDefenseExperience = value; }
        /// <summary>
        /// Ground Unit Experience Level/
        /// </summary>
        public byte GroundExperience { get => groundExperience; set => groundExperience = value; }
        /// <summary>
        /// Naval Unit Experience Level/
        /// </summary>
        public byte NavalExperience { get => navalExperience; set => navalExperience = value; }
        /// <summary>
        /// Major Initiative of the Team.
        /// </summary>
        public short Initiative { get => initiative; set => initiative = value; }
        /// <summary>
        /// Team Supplies Available.
        /// </summary>
        public ushort SupplyAvailability { get => supplyAvail; set => supplyAvail = value; }
        /// <summary>
        /// Team Fuel Available.
        /// </summary>
        public ushort FuelAvailability { get => fuelAvail; set => fuelAvail = value; }
        /// <summary>
        /// Replacements Available.
        /// </summary>
        public ushort ReplacementsAvailability { get => replacementsAvail; set => replacementsAvail = value; }
        /// <summary>
        /// Player Rating.
        /// </summary>
        public float PlayerRating { get => playerRating; set => playerRating = value; }
        /// <summary>
        /// Last time a Player Mission took place.
        /// </summary>
        public TimeSpan LastPlayerMission { get => new(0, 0, 0, 0, (int)lastPlayerMission); set => lastPlayerMission = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Current Team Statistics.
        /// </summary>
        public TeamStatus CurrentStats { get => currentStats; set => currentStats = value; }
        /// <summary>
        /// Team Statistics at the Start of the Campaign.
        /// </summary>
        public TeamStatus StartStats { get => startStats; set => startStats = value; }
        /// <summary>
        /// Reinforcement Levels.
        /// </summary>
        public short Reinforcement { get => reinforcement; set => reinforcement = value; } // TODO: Confirm this.
        /// <summary>
        /// Collection of Bonus Objectives for the Team.
        /// </summary>
        public Collection<VirtualUniqueIdentifier> BonusObjs { get => bonusObjs; set => bonusObjs = value; }
        /// <summary>
        /// Times when Bonus Objectives are Active..
        /// </summary>
        public Collection<TimeSpan> BonusTime
        {
            get
            {
                Collection<TimeSpan> timeSpans = [];
                foreach (uint val in bonusTime)
                    timeSpans.Add(new(0, 0, 0, 0, (int)val));
                return timeSpans;
            }
            set
            {
                bonusTime.Clear();
                foreach (TimeSpan t in value)
                    bonusTime.Add((uint)t.TotalMilliseconds);
            }
        }
        /// <summary>
        /// Collection of Priorities for Types of Objectives.
        /// </summary>
        public Collection<byte> ObjectiveTypePriorities { get => objtype_priority; set => objtype_priority = value; }
        /// <summary>
        /// Collection of Priorities for Types of Units.
        /// </summary>
        public Collection<byte> UnitTypePriorities { get => unittype_priority; set => unittype_priority = value; }
        /// <summary>
        /// Collection of Priorities for Types of Missions.
        /// </summary>
        public Collection<byte> MissionTypePriorities { get => mission_priority; set => mission_priority = value; }
        /// <summary>
        /// Major Offensive Attack Time.
        /// </summary>
        public TimeSpan AttackTime { get => new(0, 0, 0, 0, (int)attackTime); set => attackTime = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Indicates an Offensive Loss.
        /// </summary>
        public byte OffensiveLoss { get => offensiveLoss; set => offensiveLoss = value; } // TODO: Bool?
        /// <summary>
        /// Collection of Maximum Numbers for Vehicle Types.
        /// </summary>
        public Collection<byte> MaxVehicleCounts { get => max_vehicle; set => max_vehicle = value; } // TODO: Confirm usage
        /// <summary>
        /// Index into the Flag List for this Team Flag.
        /// </summary>
        public byte TeamFlag { get => teamFlag; set => teamFlag = value; }
        /// <summary>
        /// Color Index for this Team Color.
        /// </summary>
        public byte TeamColor { get => teamColor; set => teamColor = value; }
        /// <summary>
        /// Identifies the Type of Equipment used by this Team.
        /// </summary>
        public byte Equipment { get => equipment; set => equipment = value; }
        /// <summary>
        /// Team Name.
        /// </summary>
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// Team Motto.
        /// </summary>
        public string TeamMotto { get => teamMotto; set => teamMotto = value; }
        /// <summary>
        /// Ground Action Focus for the Team used by the Campaign Engine.
        /// </summary>
        public TeamGndActionType GroundAction { get => groundAction; set => groundAction = value; }
        /// <summary>
        /// Defensive Air Action Focus for the Team used by the Campaign Engine.
        /// </summary>
        public TeamAirActionType DefensiveAirAction { get => defensiveAirAction; set => defensiveAirAction = value; }
        /// <summary>
        /// Offensive Air Action Focus for the Team used by the Campaign Engine.
        /// </summary>
        public TeamAirActionType OffensiveAirAction { get => offensiveAirAction; set => offensiveAirAction = value; }

        #endregion Properties

        #region Fields
        private VirtualUniqueIdentifier id = new();
        private ushort entityType = 0;
        private byte who = 0;
        private byte cteam = 0;
        private short flags = 0;
        private Collection<byte> member = [];
        private Collection<short> stance = [];
        private short firstColonel = 0;
        private short firstCommander = 0;
        private short firstWingman = 0;
        private short lastWingman = 0;
        private byte airExperience = 0;
        private byte airDefenseExperience = 0;
        private byte groundExperience = 0;
        private byte navalExperience = 0;
        private short initiative = 0;
        private ushort supplyAvail = 0;
        private ushort fuelAvail = 0;
        private ushort replacementsAvail = 0;
        private float playerRating = 0;
        private uint lastPlayerMission = 0;
        private TeamStatus currentStats = new();
        private TeamStatus startStats = new();
        private short reinforcement = 0;
        private Collection<VirtualUniqueIdentifier> bonusObjs = [];
        private Collection<uint> bonusTime = [];
        private Collection<byte> objtype_priority = [];
        private Collection<byte> unittype_priority = [];
        private Collection<byte> mission_priority = [];
        private uint attackTime = 0;
        private byte offensiveLoss = 0;
        private Collection<byte> max_vehicle = [];
        private byte teamFlag = 0;
        private byte teamColor = 0;
        private byte equipment = 0;
        private string name = "";
        private string teamMotto = "";


        private TeamGndActionType groundAction = new();
        private TeamAirActionType defensiveAirAction = new();
        private TeamAirActionType offensiveAirAction = new();

        private readonly int version = int.MaxValue;
        #endregion Fields

        #region Helper Methods
        internal void Read(Stream stream)
        {
            using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
            id = new()
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32()
            };

            entityType = reader.ReadUInt16();
            who = reader.ReadByte();
            cteam = reader.ReadByte();
            flags = reader.ReadInt16();
            Member = [];
            for (int j = 0; j < 8; j++)
            {
                Member.Add(reader.ReadByte());
            }

            Stance = [];
            for (int j = 0; j < 8; j++)
            {
                Stance.Add(reader.ReadInt16());
            }

            firstColonel = reader.ReadInt16();
            firstCommander = reader.ReadInt16();
            firstWingman = reader.ReadInt16();
            lastWingman = reader.ReadInt16();
            playerRating = 0.0F;
            lastPlayerMission = 0;
            airExperience = reader.ReadByte();
            airDefenseExperience = reader.ReadByte();
            groundExperience = reader.ReadByte();
            navalExperience = reader.ReadByte();
            initiative = reader.ReadInt16();
            supplyAvail = reader.ReadUInt16();
            fuelAvail = reader.ReadUInt16();
            replacementsAvail = reader.ReadUInt16();
            playerRating = reader.ReadSingle();
            lastPlayerMission = reader.ReadUInt32();
            currentStats = new()
            {
                AirDefenseVehicles = reader.ReadUInt16(),
                Aircraft = reader.ReadUInt16(),
                GroundVehicles = reader.ReadUInt16(),
                Ships = reader.ReadUInt16(),
                Supply = reader.ReadUInt16(),
                Fuel = reader.ReadUInt16(),
                Airbases = reader.ReadUInt16(),
                SupplyLevel = reader.ReadByte(),
                FuelLevel = reader.ReadByte()
            };

            startStats = new()
            {
                AirDefenseVehicles = reader.ReadUInt16(),
                Aircraft = reader.ReadUInt16(),
                GroundVehicles = reader.ReadUInt16(),
                Ships = reader.ReadUInt16(),
                Supply = reader.ReadUInt16(),
                Fuel = reader.ReadUInt16(),
                Airbases = reader.ReadUInt16(),
                SupplyLevel = reader.ReadByte(),
                FuelLevel = reader.ReadByte()
            };

            reinforcement = reader.ReadInt16();

            bonusObjs = [];
            for (int j = 0; j < 20; j++)
            {
                VirtualUniqueIdentifier thisId = new()
                {
                    ID = reader.ReadUInt32(),
                    Creator = reader.ReadUInt32()
                };
                bonusObjs.Add(thisId);
            }

            bonusTime = [];
            for (int j = 0; j < 20; j++)
                bonusTime.Add(reader.ReadUInt32());

            objtype_priority = [];
            for (int j = 0; j < 36; j++)
                objtype_priority.Add(reader.ReadByte());

            unittype_priority = [];
            for (int j = 0; j < 20; j++)
                unittype_priority.Add(reader.ReadByte());

            MissionTypePriorities = [];
            for (int j = 0; j < 50; j++)
                MissionTypePriorities.Add(reader.ReadByte());

            max_vehicle = [];
            for (int j = 0; j < 4; j++)
                max_vehicle.Add(reader.ReadByte());

            int nullLoc;
            teamFlag = reader.ReadByte();
            teamColor = reader.ReadByte();
            equipment = reader.ReadByte();

            name = Encoding.ASCII.GetString(reader.ReadBytes(20), 0, 20);
            nullLoc = name.IndexOf('\0');
            if (nullLoc > 0)
                name = name.Substring(0, nullLoc);
            else
                Name = string.Empty;

            teamMotto = Encoding.ASCII.GetString(reader.ReadBytes(200), 0, 200);
            nullLoc = teamMotto.IndexOf('\0');
            if (nullLoc > 0)
                teamMotto = teamMotto.Substring(0, nullLoc);
            else
                teamMotto = string.Empty;

            groundAction = new()
            {
                ActionTime = new(0, 0, 0, 0, (int)reader.ReadUInt32()),
                ActionTimeout = new(0, 0, 0, 0, (int)reader.ReadUInt32()),
                ActionObjective = new()
                {
                    ID = reader.ReadUInt32(),
                    Creator = reader.ReadUInt32()
                },
                ActionType = (GroundActionType)reader.ReadByte(),
                ActionTempo = reader.ReadByte(),
                ActionPoints = reader.ReadByte()
            };

            defensiveAirAction = new()
            {
                ActionStartTime = new(0, 0, 0, 0, (int)reader.ReadUInt32()),
                ActionStopTime = new(0, 0, 0, 0, (int)reader.ReadUInt32()),
                ActionObjective = new()
                {
                    ID = reader.ReadUInt32(),
                    Creator = reader.ReadUInt32()
                },
                LastActionObjective = new()
                {
                    ID = reader.ReadUInt32(),
                    Creator = reader.ReadUInt32()
                },
                ActionType = (AirActionType)reader.ReadByte()
            };
            reader.ReadBytes(3); //align on int32 boundary

            offensiveAirAction = new()
            {
                ActionStartTime = new(0, 0, 0, 0, (int)reader.ReadUInt32()),
                ActionStopTime = new(0, 0, 0, 0, (int)reader.ReadUInt32()),
                ActionObjective = new()
                {
                    ID = reader.ReadUInt32(),
                    Creator = reader.ReadUInt32()
                },
                LastActionObjective = new()
                {
                    ID = reader.ReadUInt32(),
                    Creator = reader.ReadUInt32()
                },
                ActionType = (AirActionType)reader.ReadByte()
            };
            reader.ReadBytes(3); //align on int32 boundary
        }

        internal void Write(Stream stream)
        {
            using var writer = new BinaryWriter(stream, Encoding.Default, leaveOpen: true);
            writer.Write(id.ID);
            writer.Write(id.Creator);

            writer.Write(entityType);
            writer.Write(who);
            writer.Write(cteam);
            writer.Write(flags);
            for (int j = 0; j < member.Count; j++)
                writer.Write(member[j]);

            for (int j = 0; j < stance.Count; j++)
                writer.Write(stance[j]);

            writer.Write(firstColonel);
            writer.Write(firstCommander);
            writer.Write(firstWingman);
            writer.Write(lastWingman);
            writer.Write(airExperience);
            writer.Write(airDefenseExperience);
            writer.Write(groundExperience);
            writer.Write(navalExperience);
            writer.Write(initiative);
            writer.Write(supplyAvail);
            writer.Write(fuelAvail);
            writer.Write(replacementsAvail);
            writer.Write(playerRating);
            writer.Write(lastPlayerMission);
            writer.Write(currentStats.AirDefenseVehicles);
            writer.Write(currentStats.Aircraft);
            writer.Write(currentStats.GroundVehicles);
            writer.Write(currentStats.Ships);
            writer.Write(currentStats.Supply);
            writer.Write(currentStats.Fuel);
            writer.Write(currentStats.Airbases);
            writer.Write(currentStats.SupplyLevel);
            writer.Write(currentStats.FuelLevel);
            writer.Write(startStats.AirDefenseVehicles);
            writer.Write(startStats.Aircraft);
            writer.Write(startStats.GroundVehicles);
            writer.Write(startStats.Ships);
            writer.Write(startStats.Supply);
            writer.Write(startStats.Fuel);
            writer.Write(startStats.Airbases);
            writer.Write(startStats.SupplyLevel);
            writer.Write(startStats.FuelLevel);
            writer.Write(reinforcement);
            for (int j = 0; j < bonusObjs.Count; j++)
            {
                VirtualUniqueIdentifier thisId = BonusObjs[j];
                writer.Write(thisId.ID);
                writer.Write(thisId.Creator);
            }

            for (int j = 0; j < bonusTime.Count; j++)
                writer.Write(bonusTime[j]);

            for (int j = 0; j < objtype_priority.Count; j++)
                writer.Write(objtype_priority[j]);

            for (int j = 0; j < unittype_priority.Count; j++)
                writer.Write(unittype_priority[j]);

            for (int j = 0; j < mission_priority.Count; j++)
                writer.Write(mission_priority[j]);

            for (int j = 0; j < max_vehicle.Count; j++)
                writer.Write(max_vehicle[j]);

            writer.Write(teamFlag);
            writer.Write(teamColor);
            writer.Write(equipment);
            writer.Write(Encoding.ASCII.GetBytes(name.PadRight(20, '\0')));
            writer.Write(Encoding.ASCII.GetBytes(teamMotto.PadRight(200, '\0')));
            writer.Write((uint)groundAction.ActionTime.TotalMilliseconds);
            writer.Write((uint)groundAction.ActionTimeout.TotalMilliseconds);
            writer.Write(groundAction.ActionObjective.ID);
            writer.Write(groundAction.ActionObjective.Creator);
            writer.Write((byte)groundAction.ActionType);
            writer.Write(groundAction.ActionTempo);
            writer.Write(groundAction.ActionPoints);
            writer.Write((uint)defensiveAirAction.ActionStartTime.TotalMilliseconds);
            writer.Write((uint)defensiveAirAction.ActionStopTime.TotalMilliseconds);
            writer.Write(defensiveAirAction.ActionObjective.ID);
            writer.Write(defensiveAirAction.ActionObjective.Creator);
            writer.Write(defensiveAirAction.LastActionObjective.ID);
            writer.Write(defensiveAirAction.LastActionObjective.Creator);
            writer.Write((byte)defensiveAirAction.ActionType);
            writer.Write(new byte[3]);//align on int32 boundary

            writer.Write((uint)offensiveAirAction.ActionStartTime.TotalMilliseconds);
            writer.Write((uint)offensiveAirAction.ActionStopTime.TotalMilliseconds);
            writer.Write(offensiveAirAction.ActionObjective.ID);
            writer.Write(offensiveAirAction.ActionObjective.Creator);
            writer.Write(offensiveAirAction.LastActionObjective.ID);
            writer.Write(offensiveAirAction.LastActionObjective.Creator);
            writer.Write((byte)offensiveAirAction.ActionType);
            writer.Write(new byte[3]);//align on int32 boundary
        }
        #endregion Helper Methods

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new();

            sb.AppendLine("***** Team ID *****");
            sb.Append(id.ToString());
            sb.AppendLine("Entity Type: " + entityType);
            sb.AppendLine("Country: " + Who);
            sb.AppendLine("External Team ID: " + cteam);
            sb.AppendLine("Flags: " + flags);
            sb.AppendLine("Members:");
            for (int i = 0; i < member.Count; i++)
                sb.AppendLine("   Member ID: " + member[i]);
            sb.AppendLine("Stances:");
            for (int i = 0; i < stance.Count; i++)
                sb.AppendLine("   Member ID: " + stance[i]);
            sb.AppendLine("Points required for First Colonel: " + firstColonel);
            sb.AppendLine("Points required for First Commander: " + firstCommander);
            sb.AppendLine("Points required for First Wingman: " + firstWingman);
            sb.AppendLine("Points required for Last Wingman: " + lastWingman);
            sb.AppendLine("Air Experience: " + airExperience);
            sb.AppendLine("Air Defense Experience: " + airDefenseExperience);
            sb.AppendLine("Ground Experience: " + groundExperience);
            sb.AppendLine("Naval Experience: " + navalExperience);
            sb.AppendLine("Initiative: " + initiative);
            sb.AppendLine("Supply Availability: " + supplyAvail);
            sb.AppendLine("Fuel Availability: " + fuelAvail);
            sb.AppendLine("Replacements Availability: " + replacementsAvail);
            sb.AppendLine("Player Rating: " + playerRating);
            sb.AppendLine("Last Player Mission: " + LastPlayerMission.ToString("g"));
            sb.AppendLine("***** Current Team Statistics *****");
            sb.Append(currentStats.ToString());
            sb.AppendLine("***** Starting Team Statistics *****");
            sb.Append(startStats.ToString());
            sb.AppendLine("Reinforcements: " + reinforcement);
            sb.AppendLine("Bonus Objectives:");
            for (int i = 0; i < bonusObjs.Count; i++)
                sb.AppendLine("   Bonus Objective " + i + ": " + bonusObjs[i]);
            sb.AppendLine("Bonus Objectives Time Slots:");
            for (int i = 0; i < bonusTime.Count; i++)
                sb.AppendLine("   Bonus Objective Time Slot " + i + ": " + bonusTime[i]);
            sb.AppendLine("Objective Type Priorities: ");
            for (int i = 0; i < objtype_priority.Count; i++)
                sb.AppendLine("   Objective Type " + i + " Priority: " + objtype_priority[i]);
            sb.AppendLine("Unit Type Priorities: ");
            for (int i = 0; i < unittype_priority.Count; i++)
                sb.AppendLine("   Unit Type " + i + " Priority: " + unittype_priority[i]);
            sb.AppendLine("Mission Type Priorities: ");
            for (int i = 0; i < mission_priority.Count; i++)
                sb.AppendLine("   Mission Type " + i + " Priority: " + mission_priority[i]);
            sb.AppendLine("Attack Time: " + AttackTime.ToString("g"));
            sb.AppendLine("Offensive Loss Indicator: " + offensiveLoss);
            sb.AppendLine("Maximum Vehicles Counts: ");
            for (int i = 0; i < max_vehicle.Count; i++)
                sb.AppendLine("   Vehicle Type " + i + ": " + max_vehicle[i]);
            sb.AppendLine("Team Flag: " + TeamFlag);
            sb.AppendLine("Team Color: " + teamColor);
            sb.AppendLine("Team Name: " + name);
            sb.AppendLine("Team Motto: " + teamMotto);
            sb.AppendLine("***** Ground Action Type *****");
            sb.Append(groundAction.ToString());
            sb.AppendLine("***** Offensive Air Action Type *****");
            sb.Append(offensiveAirAction.ToString());
            sb.AppendLine("***** Defensive Air Action Type *****");
            sb.Append(offensiveAirAction.ToString());


            return sb.ToString();
        }

        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="Team"/> object.
        /// </summary>
        public Team()
        {
        }
        /// <summary>
        /// Initializes an instance of the <see cref="Team"/> object with the supplied data.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object with the initialization data.</param>
        /// <param name="version">File Version.</param>
        public Team(Stream stream, int version)
            : this()
        {
            this.version = version;
            Read(stream);
        }

        #endregion Constructors


    }
}
