using FalconCampaign.Objectives;
using System.Collections.ObjectModel;
using System.Text;
using Utilities.Logging;

namespace FalconCampaign.Files
{
    public class PST : AppFile, IEquatable<PST>
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
        public Collection<PersistentObject> PersistentObjects { get => persistentObjects; set => persistentObjects = value; }
        #endregion Properties

        #region Fields
        private Collection<PersistentObject> persistentObjects = [];
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
                    int numPersistantObjects = 0;
                   
                    numPersistantObjects = reader.ReadInt32();
                    persistentObjects = [];
                    for (int i = 0; i < numPersistantObjects; i++)
                    {
                        PersistentObject obj = new()
                        {
                            X = reader.ReadSingle(),
                            Y = reader.ReadSingle(),
                            UnionData = new()
                            {
                                Creator = reader.ReadUInt32(),
                                ID = reader.ReadUInt32(),
                                Index = reader.ReadByte()
                            }
                        };
                        
                        reader.ReadBytes(3); //align on Int32 boundary
                        obj.VisType = reader.ReadInt16();
                        obj.Flags = reader.ReadInt16();
                        persistentObjects.Add(obj);
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
                    
                    writer.Write(persistentObjects.Count);
                    for (int i = 0; i < persistentObjects.Count; i++)
                    {                        
                        writer.Write(persistentObjects[i].X);
                        writer.Write(persistentObjects[i].Y);
                        writer.Write(persistentObjects[i].UnionData.Creator);
                        writer.Write(persistentObjects[i].UnionData.ID);
                        writer.Write(persistentObjects[i].UnionData.Index);
                        writer.Write(new byte[3]); //align on Int32 boundary
                        writer.Write(persistentObjects[i].VisType);
                        writer.Write(persistentObjects[i].Flags);
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
            StringBuilder sb = new ();
            sb.AppendLine("***************************** PST File *****************************");
            sb.AppendLine("Version: " + Version);
            foreach (PersistentObject persistentObject in persistentObjects)
                sb.Append(persistentObject.ToString());

            return sb.ToString();
        }

        #region Equality Functions
        public bool Equals(PST? other)
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

            if (other is not PST comparator)
                return false;
            else
                return Equals(comparator);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 2539;
                foreach (PersistentObject persistentObject in persistentObjects)
                    hash = hash * 5483 + persistentObject.GetHashCode();

                return hash;
            }
        }
        #endregion Equality Functions

        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Initializes a default instance of the <see cref="PST"/> object
        /// </summary>
        public PST()
        {
            _FileType = ApplicationFileType.CampaignPST;
            _StreamType = FileStreamType.Binary;
            _IsCompressed = false;
        }
        /// <summary>
        /// Initializes an instance of the <see cref="PST"/> object with the supplied data.
        /// </summary>
        /// <param name="data">Raw Data from the CAM File.</param>
        /// <param name="version">File Version</param>
        public PST(byte[] data, int version = int.MaxValue)
            : this()
        {
            Version = version;
            Read(data);
        }
        #endregion Cunstructors
    }
}
