using System.Text;

namespace FalconCampaign.Components
{
    /// <summary>
    /// UniqueIdentifier with an Index Component for indexing into Database Tables.
    /// </summary>
    public class PackedVirtualUniqueIdentifier
    {
        #region Properties
        /// <summary>
        /// Item ID.
        /// </summary>
        public uint ID { get => num_; set => num_ = value; }
        /// <summary>
        /// Creator ID.
        /// </summary>
        public uint Creator { get => creator_; set => creator_ = value; }
        /// <summary>
        /// Item index in a given Database Table.
        /// </summary>
        public byte Index { get => index_; set => index_ = value; }
        #endregion Properties

        #region Fields
        private uint creator_;
        private uint num_;
        private byte index_;


        #endregion Fields

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ID: " + ID);
            sb.AppendLine("Creator: " + Creator);
            sb.AppendLine("Index: " + Index);
            return sb.ToString();
        }
        #endregion Funcitnoal Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="PackedVirtualUniqueIdentifier"/> object.
        /// </summary>
        public PackedVirtualUniqueIdentifier() { }
        #endregion Constructors     

    }
}
