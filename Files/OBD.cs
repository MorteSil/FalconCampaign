using System.Collections.ObjectModel;
using FalconCampaign.Components;
using LZSS;
using System.Text;
using Utilities.Logging;
using FalconCampaign.Objectives;

namespace FalconCampaign.Files
{
    /// <summary>
    /// Objective Delta Information. The Campaign Engine uses this to determine how to handle resupply, repairs, and other Campaign Events.
    /// </summary>
    public class OBD : AppFile, IEquatable<OBD>
    {
        #region Properties       
        /// <summary>
        /// Collection of <see cref="ObjectiveDelta"/> objects for the Campaign Engine.
        /// </summary>
        public Collection<ObjectiveDelta> Deltas { get => deltas; set => deltas = value; }
        /// <summary>
        /// File Version.
        /// </summary>
        public int Version { get => Version; set => version = value; }
        /// <summary>
        /// Return <see langword="true"/> if initialization fails and the return object has a default configuration.
        /// </summary>
        public override bool IsDefaultInitialization => false;

        #endregion Properties

        #region Fields
        private Collection<ObjectiveDelta> deltas = [];
        private int numObjectiveDeltas = 0;
        private int version = 0;

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
                    int cSize = reader.ReadInt32();
                    numObjectiveDeltas = reader.ReadInt16();
                    int uncompressedSize = reader.ReadInt32();
                    if (uncompressedSize == 0) return false;
                    var actualCompressed = reader.ReadBytes(data.Length - 10);
                    uncompressed = Lzss.Decompress(actualCompressed, uncompressedSize);
                }

                using (var stream = new MemoryStream(uncompressed))
                using (var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true))
                {
                    deltas = [];

                    for (int i = 0; i < numObjectiveDeltas; i++)
                    {
                        ObjectiveDelta thisObjectiveDelta = new();

                        VirtualUniqueIdentifier id = new()
                        {
                            ID = reader.ReadUInt32(),
                            Creator = reader.ReadUInt32()
                        };
                        thisObjectiveDelta.ID = id;

                        thisObjectiveDelta.LastRepair = new (0, 0, 0, 0, (int)reader.ReadUInt32());
                        thisObjectiveDelta.Owner = reader.ReadByte();
                        thisObjectiveDelta.Supply = reader.ReadByte();
                        thisObjectiveDelta.Fuel = reader.ReadByte();
                        thisObjectiveDelta.Losses = reader.ReadByte();
                        byte numFstatus = reader.ReadByte();
                        thisObjectiveDelta.FStatus = [];
                        for (int j = 0; j < numFstatus; j++)
                        {
                            thisObjectiveDelta.FStatus.Add(reader.ReadByte());
                        }

                        deltas.Add(thisObjectiveDelta);
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
                for (int i = 0; i < Deltas.Count; i++)
                {
                    writer.Write(deltas[i].ID.ID);
                    writer.Write(deltas[i].ID.Creator);
                    writer.Write((uint)deltas[i].LastRepair.TotalMilliseconds);
                    writer.Write(deltas[i].Owner);
                    writer.Write(deltas[i].Supply);
                    writer.Write(deltas[i].Fuel);
                    writer.Write(deltas[i].Losses);
                    writer.Write((byte)deltas[i].FStatus.Count);
                    for (int j = 0; j < deltas[i].FStatus.Count; j++)
                        writer.Write(deltas[i].FStatus[j]);
                }
                uncompressedData = stream.ToArray();
            }

            using (MemoryStream stream = new())
            using (var writer = new BinaryWriter(stream, Encoding.Default, leaveOpen: true))
            {
                writer.Write((short)deltas.Count);
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
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("************************** Objective Deltas **************************");
            sb.AppendLine("Number of Deltas: " + deltas.Count);
            for (int i = 0; i < Deltas.Count; i++)
            {
                sb.AppendLine("Delta " + i + ": ");
                sb.AppendLine("   ID: " + Deltas[i].ID.ID);
                sb.AppendLine("   Last Repair: " + Deltas[i].LastRepair.ToString("g"));
                sb.AppendLine("   Owner: " + Deltas[i].Owner);
                sb.AppendLine("   Supply: " + Deltas[i].Supply);
                sb.AppendLine("   Fuel: " + Deltas[i].Fuel);
                sb.AppendLine("   Losses: " + Deltas[i].Losses);
                sb.AppendLine("   Status: ");
                for (int j = 0; j < Deltas[i].FStatus.Count; j++)
                    sb.AppendLine("     Status " + j + ": " + Deltas[i].FStatus[j]);
            }

            return sb.ToString();
        }

        #region Equality Functions
        public bool Equals(OBD? other)
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

            if (other is not OBD comparator)
                return false;
            else
                return Equals(comparator);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 2539;
                for (int i = 0; i < Deltas.Count; i++)
                    hash ^= Deltas[i].GetHashCode();
                hash = hash * 5483 + Version;
                hash = hash * 5483 + deltas.Count;

                return hash;
            }
        }
        #endregion Equality Functions

        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Initializes a default instance of the <see cref="OBD"/> object
        /// </summary>
        public OBD()
        {
            _FileType = ApplicationFileType.CampaignOBD;
            _StreamType = FileStreamType.Binary;
            _IsCompressed = true;
        }
        /// <summary>
        /// Initializes an instance of the <see cref="OBD"/> object with the supplied data.
        /// </summary>
        /// <param name="compressedData">The raw Compressed Data read from the CAM File.</param>
        /// <param name="version">File Version.</param>
        public OBD(byte[] compressedData, int version = int.MaxValue)
            : this()
        {
            this.version = version;
            Read(compressedData);
        }

        #endregion Cunstructors

    }
}
