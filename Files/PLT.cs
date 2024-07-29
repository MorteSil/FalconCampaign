using FalconCampaign.Pilot;
using System.Collections.ObjectModel;
using System.Text;
using Utilities.Logging;

namespace FalconCampaign.Files
{
    public class PLT : AppFile, IEquatable<PLT>
    {
        #region Properties
        /// <summary>
        /// Number of Pilots in the File.
        /// </summary>
        public short NumPilots { get => (short)PilotInfo.Count; }
        /// <summary>
        /// Collection of the Pilot Data in the File.
        /// </summary>
        public Collection<PilotInfo> PilotInfo { get => pilotInfo; set => pilotInfo = value; }
        /// <summary>
        /// Collection of Callsigns for the Pilots.
        /// </summary>
        public short NumCallsigns { get => (short)CallsignData.Count; }
        /// <summary>
        /// Callsign Data.
        /// </summary>
        public Collection<byte> CallsignData { get => callsignData; set => callsignData = value; }
        public override bool IsDefaultInitialization => false;
        #endregion Properties

        #region Fields
        private Collection<PilotInfo> pilotInfo = [];
        private Collection<byte> callsignData = [];
        private int version = 0;
        #endregion Fields

        #region Helper Methods
        protected override bool Read(byte[] data)
        {
            try
            {
                using var stream = new MemoryStream(data);
                using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);               
                int numPilots = reader.ReadInt16();
                PilotInfo = [];
                for (var j = 0; j < numPilots; j++)
                    this.pilotInfo.Add(new()
                    {
                        Usage = reader.ReadInt16(),
                        VoiceID = reader.ReadByte(),
                        PhotoID = reader.ReadByte()
                    });

                int numCallsigns = reader.ReadInt16();
                CallsignData = [];
                for (var j = 0; j < numCallsigns; j++)
                    CallsignData.Add(reader.ReadByte());
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
            using MemoryStream stream = new();
            using BinaryWriter writer = new(stream, Encoding.Default, leaveOpen: true);           
            writer.Write(NumPilots);
            for (var j = 0; j < this.PilotInfo.Count; j++)
            {

                writer.Write(this.PilotInfo[j].Usage);
                writer.Write(this.PilotInfo[j].VoiceID);
                writer.Write(this.PilotInfo[j].PhotoID);
            }

            writer.Write(NumCallsigns);
            for (var j = 0; j < NumCallsigns; j++)
                writer.Write(CallsignData[j]);

            byte[] output = stream.ToArray();
            stream.Close();
            return output;
        }
        #endregion Helper Methods

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("*********************** Pilot Info ****************************");
            for (int i = 0; i < pilotInfo.Count; i++)
            {
                sb.AppendLine("Pilot " + i + ": ");
                sb.AppendLine("  Usage: " + pilotInfo[i].Usage);
                sb.AppendLine("  Photo ID: " + pilotInfo[i].PhotoID);
                sb.AppendLine("  Voice ID: " + pilotInfo[i].VoiceID);
            }

            for (int i=0;i<CallsignData.Count;i++)
                sb.AppendLine("Callsign " + i + ": " + CallsignData[i]);

            return sb.ToString();
        }

        #region Equality Functions
        public bool Equals(PLT? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != GetType()) return false;

            return other.ToString() == ToString() && other.GetHashCode() == GetHashCode();
        }
        public override bool Equals(object? other)
        {
            if (other == null)
                return false;

            if (other is not PLT comparator)
                return false;
            else
                return Equals(comparator);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 2539;
                hash = hash * 5483 + version;
                foreach (PilotInfo p in pilotInfo)
                    hash ^= p.GetHashCode();
                hash = hash * 5483 + CallsignData.GetHashCode();

                return hash;
            }
        }
        #endregion Equality Functions

        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="PLT"/> object.
        /// </summary>
        public PLT()
        {
            _FileType = ApplicationFileType.CampaignPLT;
            _StreamType = FileStreamType.Binary;
            _IsCompressed = false;
        }
        /// <summary>
        /// Initializes an instance of the <see cref="PLT"/> Embedded Campaign Object.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> to read the data from.</param>
        /// <param name="version">File Version.</param>
        public PLT(byte[] data, int version)
            : this()
        {
            this.version = version;
            Read(data);
        }
        #endregion Constructors

    }
}
