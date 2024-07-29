using System.Text;

namespace FalconCampaign.Components
{
    /// <summary>
    /// Used as a Virtual ID and index into Tables and arrays for various Campaign Items.
    /// </summary>
    public class VirtualUniqueIdentifier
    {
        
        #region Properties
        /// <summary>
        /// ID / Index Value of Item
        /// </summary>
        public uint ID
        { get => iD; set => iD = value; }         
        /// <summary> 
        /// CreatorID of an object (Not Used)
        /// </summary>
        public uint Creator
        { get => creator; set => creator = value; }
        #endregion Properties

        #region Fields
        private uint iD = 0;
        private uint creator = 0;
        #endregion Fields

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ID: " + ID);
            sb.AppendLine("Creator: " + Creator);
            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="VirtualUniqueIdentifier"/> object.
        /// </summary>
        public VirtualUniqueIdentifier() { }
        #endregion Constructors

    }
}
