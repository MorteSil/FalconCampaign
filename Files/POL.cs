using FalconCampaign.Objectives;
using System.Collections.ObjectModel;
using System.Text;
using Utilities.Logging;

namespace FalconCampaign.Files
{
    public class POL : AppFile, IEquatable<POL>
    {
        #region Properties  

        /// <summary>
        /// Returns <see langword="true"/> if initialization fails and the return object has a default configuration.
        /// </summary>
        public override bool IsDefaultInitialization => false;
        /// <summary>
        /// File Version
        /// </summary>
        public int Version { get => version; set => version = value; }
        /// <summary>
        /// Persistent Objects in the Campaign.
        /// </summary>
        public Collection<PrimaryObjective> PrimaryObjectives { get => primaryObjectives; set => primaryObjectives = value; }
        /// <summary>
        /// Mask used to for determining which team the Objective is for.
        /// </summary>
        public byte Teammask { get => teammask; set => teammask = value; }
        #endregion Properties

        #region Fields        
        private byte teammask;
        public Collection<PrimaryObjective> primaryObjectives = [];
        private int version = int.MaxValue;
        #endregion Fields

        #region Helper Methods       

        protected override bool Read(byte[] data)
        {
            try
            {
                using var stream = new MemoryStream(data);
                using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
                {
                    Teammask = reader.ReadByte();
                    int numPrimaryObjectives = reader.ReadInt16();
                    primaryObjectives = [];
                    for (int i = 0; i < numPrimaryObjectives; i++)
                    {
                        PrimaryObjective thisObjective = new()
                        {
                            Id = new()
                            {
                                ID = reader.ReadUInt32(),
                                Creator = reader.ReadUInt32()
                            },
                            Priority = []
                        };
                        thisObjective.Priority = [];
                        for (int j = 0; j < 8; j++)
                        {
                            if ((Teammask & (1 << j)) > 0)
                            {
                                thisObjective.Priority.Add(reader.ReadInt16());
                                thisObjective.Flags = reader.ReadByte();
                            }
                        }
                    }
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
            try
            {
                using MemoryStream stream = new();
                using BinaryWriter writer = new(stream, Encoding.Default, leaveOpen: true);
                {

                    writer.Write(Teammask);
                    writer.Write((short)primaryObjectives.Count);
                    for (int i = 0; i < primaryObjectives.Count; i++)
                    {                        
                        writer.Write(primaryObjectives[i].Id.ID);
                        writer.Write(primaryObjectives[i].Id.Creator);
                        for (int j = 0; j < 8; j++)
                        {
                            if ((Teammask & (1 << j)) > 0)
                            {
                                writer.Write(primaryObjectives[i].Priority[j]);
                                writer.Write(primaryObjectives[i].Flags);
                            }
                        }
                    }

                    byte[] output = stream.ToArray();
                    stream.Close();
                    return output;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CreateLogFile(ex, "This error occurred while attempting to write " + _FileType);
                throw;
            }

        }

        #endregion Helper Methods

        #region Functional Methods

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("***************************** POL File *****************************");
            sb.AppendLine("Version: " + Version);
            foreach (PrimaryObjective obj in primaryObjectives)
                sb.Append(obj.ToString());

            return sb.ToString();
        }

        #region Equality Functions
        public bool Equals(POL? other)
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

            if (other is not POL comparator)
                return false;
            else
                return Equals(comparator);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 2539;
                foreach (PrimaryObjective obj in primaryObjectives)
                    hash = hash * 5483 + obj.GetHashCode();

                return hash;
            }
        }
        #endregion Equality Functions

        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Initializes a default instance of the <see cref="POL"/> object
        /// </summary>
        public POL()
        {
            _FileType = ApplicationFileType.CampaignPOL;
            _StreamType = FileStreamType.Binary;
            _IsCompressed = false;
        }
        /// <summary>
        /// Initializes an instance of the <see cref="POL"/> object with the supplied data.
        /// </summary>
        /// <param name="data">Raw Data from the CAM File.</param>
        /// <param name="version">File Version</param>
        public POL(byte[] data, int version = int.MaxValue)
            : this()
        {
            Version = version;
            Read(data);
        }
        #endregion Cunstructors
    }
}
