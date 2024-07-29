using FalconCampaign.Components;
using System.Collections.ObjectModel;
using System.Text;
using Utilities.Logging;

namespace FalconCampaign.Files
{
    public class EVT : AppFile, IEquatable<EVT>
    {
        #region Properties
        public short NumEvents { get => (short)CampaignEvents.Count; }
        public Collection<CampaignEvent> CampaignEvents { get => campEvents; set => campEvents = value; }
        public override bool IsDefaultInitialization => false;
        #endregion Properties

        #region Fields
        private short numEvents = 0;
        private Collection<CampaignEvent> campEvents = [];
        private int version = 0;
        #endregion Fields

        #region Helper Methods
        protected override bool Read(byte[] data)
        {
            try
            {
                using var stream = new MemoryStream(data);
                using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
                numEvents = reader.ReadInt16();

                CampaignEvents = [];
                for (int i = 0; i < numEvents; i++)
                    CampaignEvents.Add(new()
                    {
                        ID = reader.ReadInt16(),
                        Flags = reader.ReadInt16(),
                    });
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
            writer.Write(NumEvents);
            for (int i = 0; i < NumEvents; i++)
            {
                writer.Write(CampaignEvents[i].ID);
                writer.Write(CampaignEvents[i].Flags);
            }
            byte[] output = stream.ToArray();
            stream.Close();
            return output;
        }
        #endregion Helper Methods

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new ();
            sb.AppendLine("*********************** Campaign Events ****************************");
            for (int i = 0; i < CampaignEvents.Count; i++)
            {
                sb.AppendLine("***** Campaign Event " + i + " *****");
                CampaignEvents[i].ToString();
            }

            return sb.ToString();
        }

        #region Equality Functions
        public bool Equals(EVT? other)
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

            if (other is not EVT comparator)
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
                foreach (CampaignEvent e in campEvents)
                    hash ^= e.GetHashCode();

                return hash;
            }
        }
        #endregion Equality Functions

        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="EVT"/> object.
        /// </summary>
        public EVT()
        {
            _FileType = ApplicationFileType.CampaignEVT;
            _StreamType = FileStreamType.Binary;
            _IsCompressed = false;
        }
        /// <summary>
        /// Initializes an instance of the <see cref="EVT"/> Embedded Campaign Object.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> to read the data from.</param>
        /// <param name="version">File Version.</param>
        public EVT(byte[] data, int version)
            :this()
        {
            this.version = version;
            Read(data);
        }
        #endregion Constructors
        
    }
}
