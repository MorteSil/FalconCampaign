using FalconCampaign.Components;
using FalconCampaign.Enums;
using FalconCampaign.Units;
using LZSS;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using Utilities.Logging;

namespace FalconCampaign.Files
{
    /// <summary>
    /// Represents the Embedded CMP File portion of the Campaign CAM File Wrapper.
    /// </summary>
    public class CMP : AppFile, IEquatable<CMP?>
    {
        #region Properties
        /// <summary>
        ///  Current Day and Time in the Campaign.
        /// </summary>
        public TimeSpan CurrentTime { get => new(0, 0, 0, 0, (int)currentTime); set => currentTime = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Day and Time the Campaign or TE Starts.
        /// </summary>
        public TimeSpan StartTime { get => new(0, 0, 0, 0, (int)startTime); set => startTime = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Max allowable time the Campaign or TE can run.
        /// </summary>
        public TimeSpan TimeLimit { get => new(0, 0, 0, 0, (int)timeLimit); set => timeLimit = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Points required for Victory.
        /// </summary>
        public int VictoryPoints { get => victoryPoint; set => victoryPoint = value; }
        /// <summary>
        /// The Type of Engagement this Campaign File Represents.
        /// </summary>
        public CampaignType Type { get => (CampaignType)type; set => type = (int)value; }
        /// <summary>
        /// Number of Teams in the Configuration
        /// </summary>
        public int NumberOfTeams { get => numberOfTeams; set => numberOfTeams = value; }
        /// <summary>
        /// An Array of the number of Aircraft each Team has.
        /// </summary>
        public Collection<int> NumberOfAircraft 
        { 
            get => numberOfAircraft;
            set
            {
                while (value.Count > NumberOfTeams) value.RemoveAt(numberOfTeams);
                numberOfAircraft = value;
            }
        }
        /// <summary>
        /// An Array of the number of Player Aircraft each Team has.
        /// </summary>
        public Collection<int> NumberOfPlayerAircraft
        {
            get => numberOfPlayerAircraft;
            set
            {
                while (value.Count > NumberOfTeams) value.RemoveAt(NumberOfTeams);
                numberOfPlayerAircraft = value;
            }
        }
        /// <summary>
        /// Default Player Team for the Engagement.
        /// </summary>
        public int TeamID { get => teTeam; set => teTeam = value; }
        /// <summary>
        /// An Collection of the Victory Points each team has accumulated.
        /// </summary>
        public Collection<int> TeamPoints
        {
            get => teamPoints;
            set
            {
                while (value.Count > NumberOfTeams) value.RemoveAt(NumberOfTeams);
                teamPoints = value;
            }
        }
        /// <summary>
        /// Bitmasked Collection of Campaign Flags.
        /// </summary>
        public int Flags { get => flags; set => flags = value; } // TODO: Break This Out
        /// <summary>
        /// An Collection of <see cref="TeamBasicInfo"/> objects.
        /// </summary>
        public Collection<Team.TeamBasicInfo> TeamBasicInfo
        {
            get => teamBasicInfo;
            set
            {
                while (value.Count > NumberOfTeams) value.RemoveAt(NumberOfTeams);
                teamBasicInfo = value;
            } // this may not be necessary, unsure if game can handle more than 8 teams
        }
        /// <summary>
        /// Day and Time of the last Major Event.
        /// </summary>
        public TimeSpan LastMajorEvent { get => new(0, 0, 0, 0, (int)lastMajorEvent); set => lastMajorEvent = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Day and Time of the last Resupply Event.
        /// </summary>
        public TimeSpan LastResupply { get => new(0, 0, 0, 0, (int)lastResupply); set => lastResupply = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Day and Time of the last Repair Event.
        /// </summary>
        public TimeSpan LastRepair { get => new(0, 0, 0, 0, (int)lastRepair); set => lastRepair = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Day and Time of the last Reinforcement Event.
        /// </summary>
        public TimeSpan LastReinforcement { get => new(0, 0, 0, 0, (int)lastReinforcement); set => lastReinforcement = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Date and Time the File was Created.
        /// </summary>
        public DateTime TimeStamp { get => new(timeStamp); set => timeStamp = (short)value.Ticks; } // TODO: Verify if this is Created, Updated, or some other value from the game
        /// <summary>
        /// Campaign Group Identifier
        /// </summary>
        public short Group { get => group; set => group = value; } // TODO: What is this?
        /// <summary>
        /// Campaign Engine Desired Ground Forces Ratio between Teams.
        /// </summary>
        public short GroundRatio { get => groundRatio; set => groundRatio = value; } // TODO: Confirm usage of this
        /// <summary>
        /// Campaign Engine Desired Air Forces Ratio between Teams.
        /// </summary>
        public short AirRatio { get => airRatio; set => airRatio = value; } // TODO: Confirm usage of this
        /// <summary>
        /// Campaign Engine Desired Air Defense Forces Ratio between Teams.
        /// </summary>
        public short AirDefenseRatio { get => airDefenseRatio; set => airDefenseRatio = value; } // TODO: Confirm usage of this
        /// <summary>
        /// Campaign Engine Desired Naval Forces Ratio between Teams.
        /// </summary>
        public short NavalRatio { get => navalRatio; set => navalRatio = value; } // TODO: Confirm usage of this
        /// <summary>
        /// Determines which briefing to provide.
        /// </summary>
        public short Brief { get => brief; set => brief = value; }
        /// <summary>
        /// The Horizontal Length of the Theater.
        /// </summary>
        public short TheaterSizeX { get => theaterSizeX; set => theaterSizeX = value; }
        /// <summary>
        /// The Vertical Length of the Theater.
        /// </summary>
        public short TheaterSizeY { get => theaterSizeY; set => theaterSizeY = value; }
        /// <summary>
        /// The Current Day of the Campaign.
        /// </summary>
        public byte CurrentDay { get => currentDay;  set => currentDay = value; }
        /// <summary>
        /// Number of Teams Currently active in the Campaign.
        /// </summary>
        public byte ActiveTeams { get => activeTeams; set => activeTeams = value; }
        /// <summary>
        /// <para>Indicates if this is still Day 0 of the Campaign.</para>
        /// <para>This determines behavior in the Campaign Engine.</para>
        /// </summary>
        public bool DayZero { get => dayZero == 0; set => dayZero = value == true ? (byte)1 : (byte)0; }
        /// <summary>
        /// Determines the result of the Campaign Completion Event.
        /// </summary>
        public byte EndGameResult { get => endgameResult; set => endgameResult = value; } // TODO: Confirm usage of this -- Enum? Bool?
        /// <summary>
        /// Identifies the current Situation of the Campaign.
        /// </summary>
        public byte Situation { get => situation; set => situation = value; } // TODO: Enum?
        /// <summary>
        /// Determines the Expected Level of Enemy Air Power for the Briefing.
        /// </summary>
        public byte EnemyAirExpectations { get => enemyAirExp; set => enemyAirExp = value; } // TODO: Enum?
        /// <summary>
        /// Determines the Expected Level of Enemy Air Defense Power for the Briefing.
        /// </summary>
        public byte EnemyAirDefenseExpectations { get => enemyADExp; set => enemyADExp = value; } // TODO: Enum?
        /// <summary>
        /// Name associated with the Bullseye Location.
        /// </summary>
        public byte BullseyeName { get => bullseyeName; set => bullseyeName = value; } // TODO: Enum? Bool?     
        /// <summary>
        /// Campaign Bullseye Point
        /// </summary>
        public Utilities.GeoLib.GeoPoint Bullseye
        {
            get => new(bullseyeX, bullseyeY);
            set
            {
                bullseyeX = (short)value.X;
                bullseyeY = (short)value.Y;
            }
        }
        /// <summary>
        /// Name of the Theater.
        /// </summary>
        public string TheaterName { get => theaterName; set => theaterName = value; }
        /// <summary>
        /// The Scenario of the current Campaign included in the Briefing.
        /// </summary>
        public string Scenario { get => scenario; set => scenario = value; }
        /// <summary>
        /// File Name of the Campaign Save File.
        /// </summary>
        public string SaveFile { get => saveFile; set => saveFile = value; } // TODO: FileInfo? can return existing from base class?
        /// <summary>
        /// Name shown in the UI for this Campaign Save.
        /// </summary>
        public string UIName { get => uiName; set => uiName = value; }
        /// <summary>
        /// Squadron the Player is currently assigned to.
        /// </summary>
        public VirtualUniqueIdentifier PlayerSquadronID { get => playerSquadronID; set => playerSquadronID = value; }
        /// <summary>
        /// Number of entries in the Recent Events list.
        /// </summary>
        public int NumberOfRecentEventEntries => RecentEvents.Count;
        /// <summary>
        /// Collection of Recent Events.
        /// </summary>
        public Collection<EventNode> RecentEvents { get => recentEventEntries; set => recentEventEntries = value; }
        /// <summary>
        /// Number of Priority Events in the Recent Events List.
        /// </summary>
        public int NumberOfPriotityEventEntries => PriorityEvents.Count;
        /// <summary>
        /// Collection of Priority Events Entriesd.
        /// </summary>
        public Collection<EventNode> PriorityEvents { get => priorityEventEntries; set => priorityEventEntries = value; }
        /// <summary>
        /// Size in <see cref="byte"/>s of the current Campaign Map.
        /// </summary>
        public int CampaignMapSize => campMap.Length;
        /// <summary>
        /// The current Campaign Map.
        /// </summary>
        public byte[] CampaignMap { get => campMap; set => campMap = value; } // TODO: This needs to be verified for type of image
        /// <summary>
        /// Unknown
        /// </summary>
        public short LastIndexNumber
        {
            get => lastIndexNum;
            set => lastIndexNum = value;
        } // TODO: What is this?
        /// <summary>
        /// Number of Available Squadrons to select from.
        /// </summary>
        public int NumberOfAvailableSquadrons => AvailableSquadrons.Count;
        /// <summary>
        /// Collection of Squadrons Available to select from.
        /// </summary>
        public Collection<SquadInfo> AvailableSquadrons
        {
            get => squadInfo;
            set
            {
                squadInfo = value;
                numAvailableSquadrons = (short)AvailableSquadrons.Count;
            }
        }
        /// <summary>
        /// Campaign Run Speed.
        /// </summary>
        public byte Tempo { get => tempo; set => tempo = value; } // TODO: Enum?
        /// <summary>
        /// IP Address of the User who created the Campaign in Multiplayer.
        /// </summary>
        public IPAddress CreatorIP
        {
            get => new(creatorIP);
            set
            {
                byte[] bytes = value.GetAddressBytes();

                // flip big-endian(network order) to little-endian
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes);
                }

                creatorIP = BitConverter.ToInt32(bytes, 0);
            }
        } // TODO: Verify Usage
        /// <summary>
        /// Date and Time this Campaign File was Created.
        /// </summary>
        public DateTime CreationTime { get => new(creationTime); set => creationTime = (int)value.Ticks; } // TODO: Verify usage
        /// <summary>
        /// Random Seed for Campaign Initialization
        /// </summary>
        public int CreatorRand => creationRand; // TODO: Implement SetSeed()
        /// <summary>
        /// Returns <see langword="true"/> if initialization fails and the return object has a default configuration.
        /// </summary>
        public override bool IsDefaultInitialization => false;
        /// <summary>
        /// File Version.
        /// </summary>
        public int Version { get => version; set => version = value; }
        #endregion Proerties

        #region Fields

        private uint currentTime = 0;
        private uint startTime = 0;
        private uint timeLimit = 0;
        private int victoryPoint = 0;
        private int type = 0;
        private int numberOfTeams = 8;
        private Collection<int> numberOfAircraft = [];
        private Collection<int> numberOfPlayerAircraft = [];
        private int teTeam = 0;
        private Collection<int> teamPoints = [];
        private int flags = 0;
        private Collection<Team.TeamBasicInfo> teamBasicInfo = [];
        private uint lastMajorEvent = 0;
        private uint lastResupply = 0;
        private uint lastRepair = 0;
        private uint lastReinforcement = 0;
        private short timeStamp = 0;
        private short group = 0;
        private short groundRatio = 0;
        private short airRatio = 0;
        private short airDefenseRatio = 0;
        private short navalRatio = 0;
        private short brief = 0;
        private short theaterSizeX = 0;
        private short theaterSizeY = 0;
        private byte currentDay = 0;
        private byte activeTeams = 0;
        private byte dayZero = 0;
        private byte endgameResult = 0;
        private byte situation = 0;
        private byte enemyAirExp = 0;
        private byte enemyADExp = 0;
        private byte bullseyeName = 0;
        private short bullseyeX = 0;
        private short bullseyeY = 0;
        private string theaterName = "";
        private string scenario = "";
        private string saveFile = "";
        private string uiName = "";
        private VirtualUniqueIdentifier playerSquadronID = new();
        private Collection<EventNode> recentEventEntries = [];
        private Collection<EventNode> priorityEventEntries = [];
        private short campMapSize = 0;
        private byte[] campMap = [];
        private short lastIndexNum = 0;
        private short numAvailableSquadrons = 0;
        private Collection<SquadInfo> squadInfo = [];
        private byte tempo = 0;
        private int creatorIP = 0;
        private int creationTime = 0;
        private int creationRand = 0;
        private int version = int.MaxValue;

        #endregion Fields

        #region Helper Methods
        protected override bool Read(byte[] data)
        {
            try
            {
                int compressedSize = BitConverter.ToInt32(data, 0);
                int uncompressedSize = BitConverter.ToInt32(data, 4);
                if (uncompressedSize == 0) return false;

                byte[] actualCompressed = new byte[data.Length - 8];
                Array.Copy(data, 8, actualCompressed, 0, actualCompressed.Length);
                byte[] uncompressed = Lzss.Decompress(actualCompressed, uncompressedSize);


                // ***************** File Contents ***********************************//
                using MemoryStream stream = new(uncompressed);
                using BinaryReader reader = new(stream, Encoding.Default, leaveOpen: true);
                int nullLoc = 0;
                currentTime = reader.ReadUInt32();
                if (currentTime == 0) currentTime = 1;

                startTime = reader.ReadUInt32();
                timeLimit = reader.ReadUInt32();
                victoryPoint = reader.ReadInt32();
                type = reader.ReadInt32();
                numberOfTeams = reader.ReadInt32();
                for (int i = 0; i < 8; i++)
                {
                    numberOfAircraft.Add(reader.ReadInt32());
                }

                for (int i = 0; i < 8; i++)
                {
                    numberOfPlayerAircraft.Add(reader.ReadInt32());
                }

                teTeam = reader.ReadInt32();
                for (int i = 0; i < 8; i++)
                {
                    teamPoints.Add(reader.ReadInt32());
                }

                flags = reader.ReadInt32();
                nullLoc = 0;
                for (int i = 0; i < 8; i++)
                {
                    Team.TeamBasicInfo info = new()
                    {
                        TeamFlag = reader.ReadByte(),
                        TeamColor = reader.ReadByte()
                    };
                    byte[] teamNameBytes = reader.ReadBytes(20);
                    info.TeamName = Encoding.ASCII.GetString(teamNameBytes, 0, 20);
                    nullLoc = info.TeamName.IndexOf('\0');
                    if (nullLoc > -1) info.TeamName = info.TeamName[..nullLoc];
                    byte[] teamMottoBytes = reader.ReadBytes(200);
                    info.TeamMotto = Encoding.ASCII.GetString(teamMottoBytes, 0, 200);
                    nullLoc = info.TeamMotto.IndexOf('\0');
                    if (nullLoc > -1) info.TeamMotto = info.TeamMotto[..nullLoc];
                    teamBasicInfo.Add(info);
                }

                lastMajorEvent = reader.ReadUInt32();
                lastResupply = reader.ReadUInt32();
                lastRepair = reader.ReadUInt32();
                lastReinforcement = reader.ReadUInt32();
                timeStamp = reader.ReadInt16();
                group = reader.ReadInt16();
                groundRatio = reader.ReadInt16();
                airRatio = reader.ReadInt16();
                airDefenseRatio = reader.ReadInt16();
                navalRatio = reader.ReadInt16();
                brief = reader.ReadInt16();
                theaterSizeX = reader.ReadInt16();
                theaterSizeY = reader.ReadInt16();
                currentDay = reader.ReadByte();
                activeTeams = reader.ReadByte();
                dayZero = reader.ReadByte();
                endgameResult = reader.ReadByte();
                situation = reader.ReadByte();
                enemyAirExp = reader.ReadByte();
                enemyADExp = reader.ReadByte();
                bullseyeName = reader.ReadByte();
                bullseyeX = reader.ReadInt16();
                bullseyeY = reader.ReadInt16();
                byte[] theaterNameBytes = reader.ReadBytes(40);
                theaterName = Encoding.ASCII.GetString(theaterNameBytes, 0, 40);
                nullLoc = theaterName.IndexOf('\0');
                if (nullLoc > -1) theaterName = theaterName[..nullLoc];

                byte[] scenarioBytes = reader.ReadBytes(40);
                scenario = Encoding.ASCII.GetString(scenarioBytes, 0, 40);
                nullLoc = scenario.IndexOf('\0');
                if (nullLoc > -1) scenario = scenario[..nullLoc];

                byte[] saveFileBytes = reader.ReadBytes(40);
                saveFile = Encoding.ASCII.GetString(saveFileBytes, 0, 40);
                nullLoc = saveFile.IndexOf('\0');
                if (nullLoc > -1) saveFile = saveFile[..nullLoc];

                byte[] uiNameBytes = reader.ReadBytes(40);
                uiName = Encoding.ASCII.GetString(uiNameBytes, 0, 40);
                nullLoc = uiName.IndexOf('\0');
                if (nullLoc > -1) uiName = uiName[..nullLoc];

                playerSquadronID = new VirtualUniqueIdentifier
                {
                    ID = reader.ReadUInt32(),
                    Creator = reader.ReadUInt32()
                };

                int numRecentEventEntries = reader.ReadInt16();
                if (numRecentEventEntries > 0)
                {
                    recentEventEntries = [];
                    for (int i = 0; i < numRecentEventEntries; i++)
                    {
                        EventNode thisNode = new()
                        {
                            X = reader.ReadInt16(),
                            Y = reader.ReadInt16(),
                            Time = new(0, 0, 0, 0, (int)reader.ReadUInt32()),
                            Flags = reader.ReadByte(),
                            Team = reader.ReadByte()
                        };
                        reader.ReadBytes(2); //align on int32 boundary                                                 
                        reader.ReadBytes(4); //skip EventText pointer                            
                        reader.ReadBytes(4); //skip UiEventNode pointer

                        short eventTextSize = reader.ReadInt16();
                        byte[] eventTextBytes = reader.ReadBytes(eventTextSize);
                        string eventText = Encoding.ASCII.GetString(eventTextBytes, 0, eventTextSize);
                        nullLoc = eventText.IndexOf('\0');
                        if (nullLoc > -1) eventText = eventText[..nullLoc];
                        thisNode.EventText = eventText;
                        recentEventEntries.Add(thisNode);
                    }
                }

                int numPriorityEventEntries = reader.ReadInt16();
                if (numPriorityEventEntries > 0)
                {
                    priorityEventEntries = [];
                    for (int i = 0; i < numPriorityEventEntries; i++)
                    {
                        EventNode thisNode = new()
                        {
                            X = reader.ReadInt16(),
                            Y = reader.ReadInt16(),
                            Time = new(0, 0, 0, 0, (int)reader.ReadUInt32()),
                            Flags = reader.ReadByte(),
                            Team = reader.ReadByte()
                        };

                        reader.ReadBytes(2); //align on int32 boundary                                                 
                        reader.ReadBytes(4); //skip EventText pointer                            
                        reader.ReadBytes(4); //skip UiEventNode pointer

                        short eventTextSize = reader.ReadInt16();
                        byte[] eventTextBytes = reader.ReadBytes(eventTextSize);
                        string eventText = Encoding.ASCII.GetString(eventTextBytes, 0, eventTextSize);
                        nullLoc = eventText.IndexOf('\0');
                        if (nullLoc > -1) eventText = eventText[..nullLoc];
                        thisNode.EventText = eventText;
                        priorityEventEntries.Add(thisNode);
                    }
                }

                campMapSize = reader.ReadInt16();
                if (campMapSize > 0)
                {
                    campMap = reader.ReadBytes(campMapSize);
                }

                lastIndexNum = reader.ReadInt16();
                numAvailableSquadrons = reader.ReadInt16();
                if (numAvailableSquadrons > 0)
                {
                    squadInfo = [];
                    for (int i = 0; i < numAvailableSquadrons; i++)
                    {
                        SquadInfo thisSquadInfo = new()
                        {
                            X = reader.ReadSingle(),
                            Y = reader.ReadSingle()
                        };

                        VirtualUniqueIdentifier thisSquadId = new()
                        {
                            ID = reader.ReadUInt32(),
                            Creator = reader.ReadUInt32()
                        };
                        thisSquadInfo.ID = thisSquadId;

                        thisSquadInfo.DescriptionIndex = reader.ReadInt16();
                        thisSquadInfo.NameID = reader.ReadInt16();
                        thisSquadInfo.AirbaseIcon = reader.ReadInt16();
                        thisSquadInfo.SquadronPatch = reader.ReadInt16();
                        thisSquadInfo.Specialty = reader.ReadByte();
                        thisSquadInfo.CurrentStrength = reader.ReadByte();
                        thisSquadInfo.Country = reader.ReadByte();
                        thisSquadInfo.AirbaseName = Encoding.ASCII.GetString(reader.ReadBytes(80), 0, 80);
                        nullLoc = thisSquadInfo.AirbaseName.IndexOf('\0');
                        if (nullLoc > -1) thisSquadInfo.AirbaseName = thisSquadInfo.AirbaseName[..thisSquadInfo.AirbaseName.IndexOf('\0')];

                        reader.ReadByte(); //align on boundary
                        thisSquadInfo.Flags = reader.ReadUInt32();
                        thisSquadInfo.CampaignID = reader.ReadUInt32();
                        thisSquadInfo.SquadronName = Encoding.ASCII.GetString(reader.ReadBytes(80), 0, 80);
                        nullLoc = thisSquadInfo.SquadronName.IndexOf('\0');
                        if (nullLoc > -1) thisSquadInfo.SquadronName = thisSquadInfo.SquadronName[..thisSquadInfo.SquadronName.IndexOf('\0')];
                        squadInfo.Add(thisSquadInfo);
                    }
                }
                tempo = reader.ReadByte();
                creatorIP = reader.ReadInt32();
                creationTime = reader.ReadInt32();
                creationRand = reader.ReadInt32();

                if (stream.Position != stream.Length)
                    throw new InvalidDataException("The Stream Did not read to the end of the section");
            }
            catch (Exception ex)
            {                
                ErrorLog.CreateLogFile(ex, "This error occurred while attempting to load " + _FileType);
                throw;
            }
            return true;
        }
        protected override byte[] Write()
        {
            byte[] compressedData;
            int uncompressedLength;

            using (MemoryStream stream = new())
            using (BinaryWriter writer = new(stream, Encoding.Default, leaveOpen: true))
            {
                writer.Write(currentTime);
                writer.Write(startTime);
                writer.Write(timeLimit);
                writer.Write(victoryPoint);
                writer.Write(type);
                writer.Write(numberOfTeams);
                for (int i = 0; i < numberOfTeams; i++)
                {
                    writer.Write(numberOfAircraft[i]);
                }

                for (int i = 0; i < numberOfTeams; i++)
                {
                    writer.Write(numberOfPlayerAircraft[i]);
                }

                writer.Write(teTeam);
                for (int i = 0; i < numberOfTeams; i++)
                {
                    writer.Write(teamPoints[i]);
                }

                writer.Write(flags);
                for (int i = 0; i < numberOfTeams; i++)
                {
                    Team.TeamBasicInfo info = teamBasicInfo[i];
                    writer.Write(info.TeamFlag);
                    writer.Write(info.TeamColor);
                    writer.Write(Encoding.ASCII.GetBytes(info.TeamName.PadRight(20, '\0')));
                    writer.Write(Encoding.ASCII.GetBytes(info.TeamMotto.PadRight(200, '\0')));
                }

                writer.Write(lastMajorEvent);
                writer.Write(lastResupply);
                writer.Write(lastRepair);
                writer.Write(lastReinforcement);
                writer.Write(timeStamp);
                writer.Write(group);
                writer.Write(groundRatio);
                writer.Write(airRatio);
                writer.Write(airDefenseRatio);
                writer.Write(navalRatio);
                writer.Write(brief);
                writer.Write(theaterSizeX);
                writer.Write(theaterSizeY);
                writer.Write(currentDay);
                writer.Write(activeTeams);
                writer.Write(dayZero);
                writer.Write(endgameResult);
                writer.Write(situation);
                writer.Write(enemyAirExp);
                writer.Write(enemyADExp);
                writer.Write(bullseyeName);
                writer.Write(bullseyeX);
                writer.Write(bullseyeY);

                writer.Write(Encoding.ASCII.GetBytes(theaterName.PadRight(40, '\0')));
                writer.Write(Encoding.ASCII.GetBytes(scenario.PadRight(40, '\0')));
                writer.Write(Encoding.ASCII.GetBytes(saveFile.PadRight(40, '\0')));
                writer.Write(Encoding.ASCII.GetBytes(uiName.PadRight(40, '\0')));

                writer.Write(playerSquadronID.ID);
                writer.Write(playerSquadronID.Creator);

                writer.Write((short)RecentEvents.Count);
                if (RecentEvents.Count > 0)
                {
                    for (int i = 0; i < RecentEvents.Count; i++)
                    {
                        EventNode thisNode = recentEventEntries[i];
                        writer.Write(thisNode.X);
                        writer.Write(thisNode.Y);
                        writer.Write((uint)thisNode.Time.TotalMilliseconds);
                        writer.Write(thisNode.Flags);
                        writer.Write(thisNode.Team);
                        writer.Write(new byte[2]); //align on int32 boundary                                                   
                        writer.Write(new byte[4]); //skip EventText pointer                        
                        writer.Write(new byte[4]); //skip UiEventNode pointer

                        writer.Write((short)thisNode.EventText.Length + 1);
                        writer.Write(Encoding.ASCII.GetBytes(thisNode.EventText + '\0'));
                    }
                }

                writer.Write((short)priorityEventEntries.Count);
                for (int i = 0; i < priorityEventEntries.Count; i++)
                {
                    EventNode thisNode = priorityEventEntries[i];
                    writer.Write(thisNode.X);
                    writer.Write(thisNode.Y);
                    writer.Write((uint)thisNode.Time.TotalMilliseconds);
                    writer.Write(thisNode.Flags);
                    writer.Write(thisNode.Team);
                    writer.Write(new byte[2]); //align on int32 boundary                                              
                    writer.Write(new byte[4]); //skip EventText pointer                    
                    writer.Write(new byte[4]); //skip UiEventNode pointer

                    writer.Write((short)thisNode.EventText.Length + 1);
                    writer.Write(Encoding.ASCII.GetBytes(thisNode.EventText + '\0'));
                }

                writer.Write(campMapSize);
                if (campMapSize > 0)
                {
                    writer.Write(campMap);
                }

                writer.Write(lastIndexNum);
                writer.Write((short)AvailableSquadrons.Count);
                if (AvailableSquadrons.Count > 0)
                {
                    for (int i = 0; i < AvailableSquadrons.Count; i++)
                    {
                        SquadInfo thisSquadInfo = squadInfo[i];
                        writer.Write(thisSquadInfo.X);
                        writer.Write(thisSquadInfo.Y);

                        writer.Write(thisSquadInfo.ID.ID);
                        writer.Write(thisSquadInfo.ID.Creator);

                        writer.Write(thisSquadInfo.DescriptionIndex);
                        writer.Write(thisSquadInfo.NameID);
                        writer.Write(thisSquadInfo.AirbaseIcon);
                        writer.Write(thisSquadInfo.SquadronPatch);

                        writer.Write(thisSquadInfo.Specialty);
                        writer.Write(thisSquadInfo.CurrentStrength);
                        writer.Write(thisSquadInfo.Country);

                        writer.Write(Encoding.ASCII.GetBytes(thisSquadInfo.AirbaseName.PadRight(80, '\0')));
                        writer.Write((byte)0x00);//align on int32 boundary
                        writer.Write(thisSquadInfo.Flags);
                        writer.Write(thisSquadInfo.CampaignID);
                        writer.Write(Encoding.ASCII.GetBytes(thisSquadInfo.SquadronName.PadRight(80, '\0')));
                    }
                }

                writer.Write(tempo);
                writer.Write(creatorIP);
                writer.Write(creationTime);
                writer.Write(creationRand);

                writer.Flush();
                stream.Flush();
                uncompressedLength = (int)stream.Length;
                compressedData = Lzss.Compress(stream.ToArray());
            }

            byte[] output = new byte[8 + compressedData.Length];

            Buffer.BlockCopy(BitConverter.GetBytes(compressedData.Length), 0, output, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(uncompressedLength), 0, output, 4, 4);
            Buffer.BlockCopy(compressedData.ToArray(), 0, output, 8, compressedData.Length);

            return output;
        }
        #endregion Helper Methods

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("*************************** CMP File *******************************");
            sb.AppendLine("Version: " + Version);
            sb.AppendLine("Current Time: " + CurrentTime.ToString("g"));
            sb.AppendLine("Start Time: " + StartTime.ToString("g"));
            sb.AppendLine("Time Limit: " + TimeLimit.ToString("g"));
            sb.AppendLine("Victory Points: " + VictoryPoints);
            sb.AppendLine("Campaign Type: " + Type);
            sb.AppendLine("Number of Teams: " + NumberOfTeams);
            sb.AppendLine("Number of Aircraft: ");
            for (int i = 0; i < numberOfTeams; i++)
                sb.AppendLine("   Team " + i + " Aircraft: " + numberOfAircraft[i]);
            sb.AppendLine("Number of Player Aircraft: ");
            for (int i = 0; i < numberOfTeams; i++)
                sb.AppendLine("   Team " + i + " Player Aircraft: " + numberOfPlayerAircraft[i]);
            sb.AppendLine("Current Player Team: " + teTeam);
            sb.AppendLine("Team Victory Points: ");
            for (int i = 0; i < numberOfTeams; i++)
                sb.AppendLine("   Team " + i + " Points: " + teamPoints[i]);
            sb.AppendLine("Campaign Flag Mask: " + flags);
            sb.AppendLine("Team Information: ");
            for (int i = 0; i < numberOfTeams; i++)
            {
                sb.AppendLine("***** Team " + i + " *****");
                sb.Append(teamBasicInfo[i].ToString());
            }
            sb.AppendLine("Last Major Event: " + LastMajorEvent.ToString("g"));
            sb.AppendLine("Last Resupply: " + LastResupply.ToString("g"));
            sb.AppendLine("Last Repair: " + LastRepair.ToString("g"));
            sb.AppendLine("Last Reinforcement: " + LastReinforcement.ToString());
            sb.AppendLine("Timestamp: " + TimeStamp.ToString());
            sb.AppendLine("Group: " + group);
            sb.AppendLine("Ground Ratio: " + groundRatio);
            sb.AppendLine("Air Ratio: " + airRatio);
            sb.AppendLine("Air Defense Ratio: " + airDefenseRatio);
            sb.AppendLine("Naval Ratio: " + navalRatio);
            sb.AppendLine("Brief: " + brief);
            sb.AppendLine("Theater Size X: " + theaterSizeX);
            sb.AppendLine("Theater Size Y: " + theaterSizeY);
            sb.AppendLine("Current Day: " + currentDay);
            sb.AppendLine("Active Teams: " + activeTeams);
            sb.AppendLine("Day Zero: " + DayZero);
            sb.AppendLine("Endgame Result: " + endgameResult);
            sb.AppendLine("Situation: " + situation);
            sb.AppendLine("Enemy Air Expectations: " + enemyAirExp);
            sb.AppendLine("Enemy Air Defense Expectations: " + enemyADExp);
            sb.AppendLine("Bullseye Name: " + bullseyeName);
            sb.AppendLine("***** Bullseye Coordiinates *****");
            sb.Append(Bullseye.ToString());
            sb.AppendLine("Theater Name: " + theaterName);
            sb.AppendLine("Scenario: " + scenario);
            sb.AppendLine("Save File: " + saveFile);
            sb.AppendLine("UI Name: " + uiName);
            sb.AppendLine("Player Squadron ID: " + playerSquadronID.ID);
            sb.AppendLine("Number of Recent Events: " + recentEventEntries.Count);
            for (int i = 0; i < recentEventEntries.Count; i++)
            {
                sb.AppendLine("***** Event " + i + " *****");
                sb.Append(recentEventEntries[i].ToString());
            }
            sb.AppendLine("Number of Priority Events: " + priorityEventEntries.Count);
            for (int i = 0; i < priorityEventEntries.Count; i++)
            {
                sb.AppendLine("***** Priority Event " + i + " *****");
                sb.Append(priorityEventEntries[i].ToString());
            }
            sb.AppendLine("Map Size: " + campMapSize);
            sb.AppendLine("Last Index: " + lastIndexNum);
            sb.AppendLine("Number of Available Squadrons: " + numAvailableSquadrons);
            for (int i = 0; i < squadInfo.Count; i++)
            {
                sb.AppendLine("***** Squadron " + i + " *****");
                sb.Append(squadInfo[i].ToString());
            }
            sb.AppendLine("Tempo: " + tempo);
            sb.AppendLine("Creator IP: " + creatorIP);
            sb.AppendLine("Creator Random Seed: " + creationRand);
            sb.AppendLine("Creation Time: " + creationTime.ToString());

            return sb.ToString();
        }

        #region Equality Functions
        public override bool Equals(object? obj)
        {
            return Equals(obj as CMP);
        }
        public override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(currentTime);
            hash.Add(startTime);
            hash.Add(timeLimit);
            hash.Add(victoryPoint);
            hash.Add(type);
            hash.Add(numberOfTeams);
            hash.Add(numberOfAircraft);
            hash.Add(numberOfPlayerAircraft);
            hash.Add(teTeam);
            hash.Add(teamPoints);
            hash.Add(flags);
            hash.Add(teamBasicInfo);
            hash.Add(lastMajorEvent);
            hash.Add(lastResupply);
            hash.Add(lastRepair);
            hash.Add(lastReinforcement);
            hash.Add(timeStamp);
            hash.Add(group);
            hash.Add(groundRatio);
            hash.Add(airRatio);
            hash.Add(airDefenseRatio);
            hash.Add(navalRatio);
            hash.Add(brief);
            hash.Add(theaterSizeX);
            hash.Add(theaterSizeY);
            hash.Add(currentDay);
            hash.Add(activeTeams);
            hash.Add(dayZero);
            hash.Add(endgameResult);
            hash.Add(situation);
            hash.Add(enemyAirExp);
            hash.Add(enemyADExp);
            hash.Add(bullseyeName);
            hash.Add(bullseyeX);
            hash.Add(bullseyeY);
            hash.Add(theaterName);
            hash.Add(scenario);
            hash.Add(saveFile);
            hash.Add(uiName);
            hash.Add(playerSquadronID);
            hash.Add(recentEventEntries);
            hash.Add(priorityEventEntries);
            hash.Add(campMapSize);
            hash.Add(campMap);
            hash.Add(lastIndexNum);
            hash.Add(numAvailableSquadrons);
            hash.Add(squadInfo);
            hash.Add(tempo);
            hash.Add(creatorIP);
            hash.Add(creationTime);
            hash.Add(creationRand);
            hash.Add(Version);
            return hash.ToHashCode();
        }
        public bool Equals(CMP? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != GetType()) return false;

            return other.ToString() == ToString() && other.GetHashCode() == GetHashCode();
        }

        #endregion Equality Functions
        #endregion Functional Methods

        #region Constructors

        /// <summary>
        /// Initializes a default instance of the <see cref="CMP"/> object.
        /// </summary>
        internal CMP()
        {
            _FileType = ApplicationFileType.CampaignCMP;
            _StreamType = FileStreamType.Binary;
            _IsCompressed = true;
        }
        /// <summary>
        /// Initializes an instance of the <see cref="CMP"/> object with the data supplied in <paramref name="compressedData"/>
        /// </summary>
        /// <param name="compressedData">Compressed Data extracted from the CAM File this file was embedded in.</param>
        /// <param name="version">File Version number, defaults to <see cref="int.MaxValue"/> to load the most recent documented file structure.</param>
        public CMP(byte[] compressedData, int version = int.MaxValue)
            : this()
        {
            Version = version;
            Read(compressedData);
        }

        #endregion Constructors

    }

}
