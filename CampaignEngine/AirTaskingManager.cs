using FalconCampaign.Enums;
using FalconDatabase.Enums;
using System.Collections.ObjectModel;
using System.Text;

namespace FalconCampaign.CampaignEngine
{
    /// <summary>
    /// Air Component of the Campaign Manager.
    /// </summary>
    public class AirTaskingManager : CampaignManager
    {
        #region Properties
        /// <summary>
        /// Air Tasking Manager Flags.
        /// </summary>
        public short Flags { get => flags; set => flags = value; } // TODO: Map this
        /// <summary>
        /// Average CAS Strength to maintain for the ATM to maintain.
        /// </summary>
        public short AverageCAStrength { get => averageCAStrength; set => averageCAStrength = value; }
        /// <summary>
        /// Average Counter Air Mission Strength for the ATM to maintain.
        /// </summary>
        public short AverageCounterAirMissions { get => averageCAMissions; set => averageCAMissions = value; }
        /// <summary>
        /// How often to evaluate the status of the Engine.
        /// </summary>
        public byte SampleCycles { get => sampleCycles; set => sampleCycles = value; }
        /// <summary>
        /// Number of Airbases being managed.
        /// </summary>
        public byte NumAirbases { get => (byte)Airbases.Count; }
        /// <summary>
        /// Collection of Airbases being managed by the ATM.
        /// </summary>
        public Collection<ATMAirbase> Airbases { get => airbases; set => airbases = value; }
        /// <summary>
        /// Number of Cycles that have passed since the Campaign Engine Status was checked.
        /// </summary>
        public byte Cycle { get => cycle; set => cycle = value; }
        /// <summary>
        /// Number of Mission Requests being processed by the ATM.
        /// </summary>
        public short NumMissionRequests { get => (short)MissionRequests.Count; }
        /// <summary>
        /// Mission Requests currently being processed by the ATM.
        /// </summary>
        public Collection<MissionRequest> MissionRequests { get => missionRequests; set => missionRequests = value; }
        #endregion Properties

        #region Fields
        private short flags = 0;
        private short averageCAStrength = 0;
        private short averageCAMissions = 0;
        private byte sampleCycles = 0;
        private byte numAirbases = 0;
        Collection<ATMAirbase> airbases = [];
        private byte cycle = 0;
        private short numMissionRequests = 0;
        private Collection<MissionRequest> missionRequests = [];


        #endregion

        #region Helper Methods
        internal new void Read(Stream stream)
        {

            using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
            flags = reader.ReadInt16();
            averageCAStrength = reader.ReadInt16();
            averageCAMissions = reader.ReadInt16();
            sampleCycles = reader.ReadByte();
            numAirbases = reader.ReadByte();
            Airbases = [];
            for (int j = 0; j < numAirbases; j++)
                Airbases.Add(new ATMAirbase(stream));

            cycle = reader.ReadByte();
            numMissionRequests = reader.ReadInt16();
            missionRequests = [];
            for (int j = 0; j < numMissionRequests; j++)
            {
                var mis_request = new MissionRequest
                {
                    RequesterID = new()
                    {
                        ID = reader.ReadUInt32(),
                        Creator = reader.ReadUInt32(),
                    },

                    TargetID = new()
                    {
                        ID = reader.ReadUInt32(),
                        Creator = reader.ReadUInt32()
                    },

                    SecondaryID = new()
                    {
                        ID = reader.ReadUInt32(),
                        Creator = reader.ReadUInt32()
                    },

                    PakID = new()
                    {
                        ID = reader.ReadUInt32(),
                        Creator = reader.ReadUInt32()
                    },

                    Who = (CountryList)reader.ReadByte(),
                    Vs = (CountryList)reader.ReadByte()
                };
                reader.ReadBytes(2);//align on int32 boundary
                mis_request.ToT = new(0, 0, 0, 0, (int)reader.ReadUInt32());
                mis_request.Tx = reader.ReadInt16();
                mis_request.Ty = reader.ReadInt16();
                mis_request.Flags = reader.ReadUInt32();
                mis_request.Caps = reader.ReadInt16();
                mis_request.TargetCount = reader.ReadInt16();
                mis_request.Speed = reader.ReadInt16();
                mis_request.MatchStrength = reader.ReadInt16();
                mis_request.Priority = reader.ReadInt16();
                mis_request.ToTType = reader.ReadByte();
                mis_request.ActionType = reader.ReadByte();
                mis_request.Mission = reader.ReadByte();
                mis_request.Aircraft = reader.ReadByte();
                mis_request.Context = (MissionContext)reader.ReadByte();
                mis_request.RoECheck = reader.ReadByte();
                mis_request.Delayed = reader.ReadByte();
                mis_request.StartBlock = reader.ReadByte();
                mis_request.FinalBlock = reader.ReadByte();

                mis_request.Slots = [];
                for (int k = 0; k < 4; k++)
                    mis_request.Slots.Add(reader.ReadByte());

                mis_request.MinimumTakeoff = reader.ReadSByte();
                mis_request.MaxTakeoff = reader.ReadSByte();
                reader.ReadBytes(3);// align on int32 boundary
                MissionRequests.Add(mis_request);

            }
        }

        internal new void Write(Stream stream)
        {
            base.Write(stream);
            using var writer = new BinaryWriter(stream, Encoding.Default, leaveOpen: true);
            writer.Write(flags);
            writer.Write(averageCAStrength);
            writer.Write(averageCAMissions);
            writer.Write(sampleCycles);
            writer.Write(NumAirbases);
            for (int j = 0; j < NumAirbases; j++)
                Airbases[j].Write(stream);

            writer.Write(cycle);
            writer.Write(NumMissionRequests);
            for (int j = 0; j < NumMissionRequests; j++)
            {
                MissionRequest mis_request = MissionRequests[j];

                writer.Write(mis_request.RequesterID.ID);
                writer.Write(mis_request.RequesterID.Creator);

                writer.Write(mis_request.TargetID.ID);
                writer.Write(mis_request.TargetID.Creator);

                writer.Write(mis_request.SecondaryID.ID);
                writer.Write(mis_request.SecondaryID.Creator);

                writer.Write(mis_request.PakID.ID);
                writer.Write(mis_request.PakID.Creator);

                writer.Write((byte)mis_request.Who);
                writer.Write((byte)mis_request.Vs);
                writer.Write(new byte[2]);//align on int32 boundary

                writer.Write((uint)mis_request.ToT.TotalMilliseconds);
                writer.Write(mis_request.Tx);
                writer.Write(mis_request.Ty);
                writer.Write(mis_request.Flags);
                writer.Write(mis_request.Caps);
                writer.Write(mis_request.TargetCount);
                writer.Write(mis_request.Speed);
                writer.Write(mis_request.MatchStrength);
                writer.Write(mis_request.Priority);
                writer.Write(mis_request.ToTType);
                writer.Write(mis_request.ActionType);
                writer.Write(mis_request.Mission);
                writer.Write(mis_request.Aircraft);
                writer.Write((byte)mis_request.Context);
                writer.Write(mis_request.RoECheck);
                writer.Write(mis_request.Delayed);
                writer.Write(mis_request.StartBlock);
                writer.Write(mis_request.FinalBlock);
                for (int k = 0; k < 4; k++)
                    writer.Write(mis_request.Slots[k]);

                writer.Write(mis_request.MinimumTakeoff);
                writer.Write(mis_request.MaxTakeoff);
                writer.Write(new byte[3]);// align on int32 boundary
            }
        }
        #endregion Helper Methods

        #region Functional Methods

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Flags: " + flags);
            sb.AppendLine("Average CAS Strength: " + averageCAStrength);
            sb.AppendLine("Average Counter Air Missions: " + averageCAMissions);
            sb.AppendLine("Sample Cycles: " + sampleCycles);
            sb.AppendLine("Airbase Count: " + NumAirbases);
            for (int i = 0; i < Airbases.Count; i++)
            {
                sb.AppendLine("***** Airbase Information *****");
                sb.Append(airbases[i].ToString());
            }
            sb.AppendLine("Cycle: " + cycle);
            sb.AppendLine("***** Mission Requests *****" + NumMissionRequests);
            for (int i = 0; i < NumMissionRequests; i++)
                MissionRequests[i].ToString();


            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="AirTaskingManager"/> object.
        /// </summary>
        protected AirTaskingManager() { }
        /// <summary>
        /// Initializes an instance of the <see cref="AirTaskingManager"/> object with the data supplied.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> with initialization data.</param>
        /// <param name="version">File Version.</param>
        public AirTaskingManager(Stream stream, int version)
            : base(stream, version)
        {
            Read(stream);
        }
        #endregion Constructors


    }
}
