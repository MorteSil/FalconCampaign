using System.Text;

namespace FalconCampaign.CampaignEngine
{
    /// <summary>
    /// Ground Component of the Campaign Manager.
    /// </summary>
    public class GroundTaskingManager : CampaignManager
    {
        #region Properties
        /// <summary>
        /// GTM Flags.
        /// </summary>
        public short Flags { get => flags; set => flags = value; } // TODO: Map this
        #endregion Properties

        #region Fields
        private short flags = 0;
        #endregion Fields

        #region Helper Methods
        internal new void Read(Stream stream)
        {
            using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
            flags = reader.ReadInt16();
        }
        internal new void Write(Stream stream)
        {
            base.Write(stream);
            using var writer = new BinaryWriter(stream, Encoding.Default, leaveOpen: true);
            writer.Write(flags);
        }

        #endregion Helper Methods

        #region Functional Methods

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("Flags: " + flags);

            return sb.ToString();
        }

        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="GroundTaskingManager"/> object.
        /// </summary>
        public GroundTaskingManager()
           : base()
        {
        }
        /// <summary>
        /// Initializes an instance of the <see cref="GroundTaskingManager"/> object with the supplied data.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object with initialization data.</param>
        /// <param name="version">File Version.</param>
        public GroundTaskingManager(Stream stream, int version)
            : base(stream, version)
        {
            Read(stream);
        }
        #endregion Cunstructors

    }
}
