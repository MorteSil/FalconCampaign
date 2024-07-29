using System.Text;

namespace FalconCampaign.Files
{
    /// <summary>
    /// Holds the File Version Information for this Campaign File.
    /// </summary>
    public class VER : AppFile, IEquatable<VER>
    {
        #region Properties
        /// <summary>
        /// Returns <see langword="true"/> if initialization fails and the return object has a default configuration.
        /// </summary>
        public override bool IsDefaultInitialization => false;
        /// <summary>
        /// File Version.
        /// </summary>
        public int Version { get => version; }
        #endregion Properties

        #region Fields
        private int version = 0;
        #endregion Fields

        #region Helper Methods       

        protected override byte[] Write()
        {
            return Encoding.ASCII.GetBytes(Version.ToString());
        }

        #endregion Helper Methods

        #region Functional Methods

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("*************************** Version **********************************");
            sb.AppendLine(Version.ToString());

            return sb.ToString();
        }

        #region Equality Functions
        public bool Equals(VER? other)
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

            if (other is not VER comparator)
                return false;
            else
                return Equals(comparator);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(version);
        }
        #endregion Equality Functions

        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Initializes a default instance of the <see cref="VER"/> object
        /// </summary>
        public VER()
        {
            _FileType = ApplicationFileType.CampaignVER;
            _StreamType = FileStreamType.Binary;
            _IsCompressed = false;
        }
        /// <summary>
        /// Initializes a <see cref="VER"/> object with the data supplied.
        /// </summary>
        /// <param name="data">Raw Uncompressed Data from the CAM File.</param>
        public VER(byte[] data)
        : this()
        {
            version = int.Parse(Encoding.ASCII.GetString(data));
        }
        #endregion Cunstructors

    }
}
