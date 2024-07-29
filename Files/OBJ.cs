using System.Collections.ObjectModel;
using System.Text;
using FalconCampaign.Objectives;
using LZSS;
using Utilities.Logging;

namespace FalconCampaign.Files
{
    /// <summary>
    /// The Objectives File embedded within a Campaign File.
    /// </summary>
    public class OBJ : AppFile, IEquatable<OBJ>
    {
        #region Properties     
        /// <summary>
        /// Collection of <see cref="Objective"/> objectives.
        /// </summary>
        public Collection<Objective> Objectives { get => objectives; set => objectives = value; }
        /// <summary>
        /// File Version
        /// </summary>
        protected int Version { get => version; }
        /// <summary>
        /// Returns <see langword="true"/> if initialization fails and the return object has a default configuration.
        /// </summary>
        public override bool IsDefaultInitialization => false;
        #endregion Properties

        #region Fields
        private Collection<Objective> objectives = [];
        private int version = 0;
        private short numObjectives = 0;

        #endregion Fields

        #region Helper Methods      

        protected override bool Read(byte[] data)
        {
            try
            {
                byte[] uncompressed;

                using (var stream = new MemoryStream(data))
                using (var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true))
                {
                    numObjectives = reader.ReadInt16();
                    int uncompressedSize = reader.ReadInt32();
                    int newSize = reader.ReadInt32();
                    if (uncompressedSize == 0) return false;

                    var actualCompressed = reader.ReadBytes(data.Length - 10);
                    uncompressed = Lzss.Decompress(actualCompressed, uncompressedSize);
                }

                Objectives = [];

                using (MemoryStream stream = new(uncompressed))
                using (BinaryReader reader = new(stream, Encoding.Default, leaveOpen: true))
                {
                    for (int i = 0; i < numObjectives; i++)
                    {
                        short thisObjectiveType = reader.ReadInt16();
                        Objectives.Add(new Objective(stream, Version) { ObjectiveType = thisObjectiveType });
                    }

                    if (stream.Position != stream.Length)
                        throw new InvalidDataException("The Stream Did not read to the end of the section");
                }

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
            byte[] uncompressedData;
            using (MemoryStream stream = new())
            using (BinaryWriter writer = new(stream, Encoding.Default, leaveOpen: true))
            {
                for (int i = 0; i < Objectives.Count; i++)
                {
                    writer.Write(Objectives[i].ObjectiveType);
                    Objectives[i].Write(stream);
                }
                writer.Flush();
                stream.Flush();
                uncompressedData = stream.ToArray();
            }

            using (MemoryStream stream = new())
            using (var writer = new BinaryWriter(stream, Encoding.Default, leaveOpen: true))
            {
                writer.Write((short)Objectives.Count);
                writer.Write(uncompressedData.Length);
                var compressedData = Lzss.Compress(uncompressedData);
                writer.Write(compressedData.Length);
                writer.Write(compressedData);
                byte[] output = stream.ToArray();
                stream.Close();
                return output;
            }
        }

        #endregion Helper Methods

        #region Functional Methods

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("************************* Objectives *******************************");
            for (int i = 0; i < Objectives.Count; i++)
            {
                sb.AppendLine("***** Objective " + i + " *****");
                Objectives[i].ToString();
            }

            return sb.ToString();
        }

        #region Equality Functions
        public bool Equals(OBJ? other)
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

            if (other is not OBJ comparator)
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
                foreach (Objective o in Objectives)
                    hash ^= o.GetHashCode();

                return hash;
            }
        }
        #endregion Equality Functions

        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Initializes a default instance of the <see cref="OBJ"/> object
        /// </summary>
        public OBJ()
        {
            _FileType = ApplicationFileType.CampaignOBJ;
            _StreamType = FileStreamType.Binary;
            _IsCompressed = true;
        }
        /// <summary>
        /// Initializes an instance of the <see cref="OBJ"/> object with the data supplied.
        /// </summary>
        /// <param name="compressedData">The raw compressed data from the CAM File.</param>
        /// <param name="version">File Version.</param>
        public OBJ(byte[] compressedData, int version = int.MaxValue)
            : this()
        {
            this.version = version;
            Read(compressedData);
        }
        #endregion Cunstructors

    }
}
