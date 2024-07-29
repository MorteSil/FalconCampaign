using System.Text;

namespace FalconCampaign.Units
{
    /// <summary>
    /// Naval Task Force Unit.
    /// </summary>
    public class TaskForce : Unit
    {
        #region Properties
        /// <summary>
        /// Task Force Unit Orders
        /// </summary>
        public byte Orders { get => orders; set => orders = value; }
        /// <summary>
        /// Task Force Unit Supply.
        /// </summary>
        public byte Supply { get => supply; set => supply = value; }
        #endregion Properties

        #region  Fields
        private byte orders = 0;
        private byte supply = 0;
        #endregion Fields

        #region Helper Methods
        internal new void Read(Stream stream)
        {
            using BinaryReader reader = new(stream, Encoding.Default, leaveOpen: true);
            Orders = reader.ReadByte();
            Supply = reader.ReadByte();
        }

        internal new void Write(Stream stream)
        {
            base.Write(stream);
            using BinaryWriter writer = new(stream, Encoding.Default, leaveOpen: true);
            writer.Write(Orders);
            writer.Write(Supply);

        }
        #endregion Helper Methods

        #region Functional Methods

        public override string ToString()
        {
            return base.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="TaskForce"/> object.
        /// </summary>
        protected TaskForce()
           : base()
        {
        }
        /// <summary>
        /// Initializes a <see cref="TaskForce"/> object with the supplied data.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object with the initializaiton data.</param>
        /// <param name="version">File Version.</param>
        public TaskForce(Stream stream, int version)
            : base(stream, version)
        {
            Read(stream);
        }
        #endregion Constructors
    }
}
