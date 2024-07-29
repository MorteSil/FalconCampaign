using LZSS;
using System.Collections.ObjectModel;
using System.Text;
using Utilities.Logging;
using FalconCampaign.Units;

namespace FalconCampaign.Files
{
    /// <summary>
    /// Campaign UNI File embedded in the Campaign CAM File.
    /// </summary>
    public class UNI : AppFile, IEquatable<UNI>
    {
        #region Properties
        /// <summary>
        /// Collection of <see cref="Unit"/> objects in the Campaign.
        /// </summary>
        public Collection<Unit> Units { get => units; set => units = value; }
        /// <summary>
        /// Number of <see cref="Unit"/> objects in the Campaign.
        /// </summary>
        public int NumUnits { get => Units.Count; }
        /// <summary>
        /// Return <see langword="true"/> if initialization fails and the return object has a default configuration.
        /// </summary>
        public override bool IsDefaultInitialization => false;
        #endregion Properties

        #region Fields
        private int version = 0;
        private Collection<Unit> units = [];
        private int numUnits = 0;
        private FalconDatabase.Tables.ClassTable classTable = new();

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
                    var cSize = reader.ReadInt32();
                    numUnits = reader.ReadInt16();
                    var uncompressedSize = reader.ReadInt32();
                    if (uncompressedSize == 0) return false;
                    var actualCompressed = reader.ReadBytes(data.Length - 10);
                    uncompressed = Lzss.Decompress(actualCompressed, uncompressedSize);
                }

                using (var stream = new MemoryStream(uncompressed))
                using (var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true))
                {
                    Units = [];
                    int i = 0;
                    while (i < numUnits)
                    {
                        Unit? thisUnit = null;
                        short thisUnitType = reader.ReadInt16();
                        // TODO: Debugging--Remove This.
                        if (thisUnitType > numUnits)
                            ;
                        if (thisUnitType > 0)
                        {
                            FalconDatabase.Components.ClassEntityDefinition classTableEntry = classTable.Classes[Math.Max(0, thisUnitType - 100)];
                            if (classTableEntry.ClassData.Domain == FalconDatabase.Enums.ClasstableDomain.Air)
                            {
                                if (classTableEntry.ClassData.Type == (int)FalconDatabase.Enums.AirUnit.FLIGHT)
                                {
                                    thisUnit = new Flight(stream, version) { UnitType = thisUnitType };
                                }
                                else if (classTableEntry.ClassData.Type == (int)FalconDatabase.Enums.AirUnit.SQUADRON)
                                {
                                    thisUnit = new Squadron(stream, version) { UnitType = thisUnitType };
                                }
                                else if (classTableEntry.ClassData.Type == (int)FalconDatabase.Enums.AirUnit.PACKAGE)
                                {
                                    thisUnit = new Package(stream, version) { UnitType = thisUnitType };
                                }
                                else
                                {
                                    thisUnit = null;
                                }
                            }
                            else if (classTableEntry.ClassData.Domain == FalconDatabase.Enums.ClasstableDomain.Land)
                            {
                                if (classTableEntry.ClassData.Type == (int)FalconDatabase.Enums.GroundUnit.BRIGADE)
                                {
                                    thisUnit = new Brigade(stream, version) { UnitType = thisUnitType };
                                }
                                else if (classTableEntry.ClassData.Type == (int)FalconDatabase.Enums.GroundUnit.BATTALION)
                                {
                                    thisUnit = new Battalion(stream, version) { UnitType = thisUnitType };
                                }
                                else
                                {
                                    thisUnit = null;
                                }

                            }
                            else if (classTableEntry.ClassData.Domain == FalconDatabase.Enums.ClasstableDomain.Sea)
                            {
                                if (classTableEntry.ClassData.Type == (int)FalconDatabase.Enums.SeaUnit.TASKFORCE)
                                {
                                    thisUnit = new TaskForce(stream, version) { UnitType = thisUnitType };
                                }
                                else
                                {
                                    thisUnit = null;
                                }
                            }
                            else if (classTableEntry.ClassData.Domain == FalconDatabase.Enums.ClasstableDomain.Undersea)
                            {
                                if (classTableEntry.ClassData.Type == (int)FalconDatabase.Enums.UnderseaUnit.WOLFPACK)
                                {
                                    thisUnit = new TaskForce(stream, version) { UnitType = thisUnitType };
                                }
                                else
                                {
                                    thisUnit = null;
                                }
                            }                            
                            else
                            {
                                thisUnit = null;
                            }
                        }
                        if (thisUnit != null)
                        {
                            Units.Add(thisUnit);
                            i++;
                        }
                        else
                        {

                        }
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

                for (var i = 0; i < Units.Count; i++)
                {
                    var thisUnit = Units[i];
                    writer.Write(thisUnit.UnitType);
                    if (thisUnit.UnitType > 0)
                    {
                        if (thisUnit is Flight)
                            (thisUnit as Flight)?.Write(stream);
                        else if (thisUnit is Squadron)
                            (thisUnit as Squadron)?.Write(stream);
                        else if (thisUnit is Package)
                            (thisUnit as Package)?.Write(stream);
                        else if (thisUnit is Brigade)
                            (thisUnit as Brigade)?.Write(stream);
                        else if (thisUnit is Battalion)
                            (thisUnit as Battalion)?.Write(stream);
                        else if (thisUnit is TaskForce)
                            (thisUnit as TaskForce)?.Write(stream);
                    }
                }
                writer.Flush();
                stream.Flush();
                uncompressedData = stream.ToArray();

            }

            using (MemoryStream stream = new())
            using (BinaryWriter writer = new(stream, Encoding.Default, leaveOpen: true))
            {
                var compressedData = Lzss.Compress(uncompressedData);
                writer.Write(compressedData.Length);
                writer.Write((short)Units.Count);
                writer.Write(uncompressedData.Length);
                writer.Write(compressedData);

                return compressedData;
            }

        }

        #endregion Helper Methods

        #region Functional Methods

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("******************************** Units *******************************");
            for (int i = 0; i < units.Count; i++)
            {
                sb.AppendLine(" ***** Unit " + i + " *****");
                sb.Append(units[i].ToString());
            }
            return sb.ToString();
        }

        #region Equality Functions
        public bool Equals(UNI? other)
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

            if (other is not UNI comparator)
                return false;
            else
                return Equals(comparator);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 2539;
                for (int i = 0; i < units.Count; i++)
                    hash ^= units[i].GetHashCode();
                hash = hash * 5483 + version;
                hash = hash * 5483 + units.Count;

                return hash;
            }
        }
        #endregion Equality Functions

        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Initializes a default instance of the <see cref="UNI"/> object
        /// </summary>
        public UNI()
        {
            _FileType = ApplicationFileType.CampaignUNI;
            _StreamType = FileStreamType.Binary;
            _IsCompressed = true;
        }
        /// <summary>
        /// Initializes an instance of the <see cref="UNI"/> object with the supplied data.
        /// </summary>
        /// <param name="compressedData">Raw Compressed Data from the CAM File.</param>
        /// <param name="version">File Version</param>
        /// <param name="table"><see cref="Database.ClassTable"/> object representing the Class Table from the Database.</param>
        public UNI(byte[] compressedData, int version, FalconDatabase.Tables.ClassTable table)
            : this()
        {
            ArgumentNullException.ThrowIfNull(compressedData);
            classTable = table;
            this.version = version;
            Read(compressedData);
        }
        #endregion Cunstructors

    }
}
