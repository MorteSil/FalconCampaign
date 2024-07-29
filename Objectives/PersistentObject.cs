using System.Text;
using FalconCampaign.Components;

namespace FalconCampaign.Objectives
{
    public class PersistentObject
    {
        #region Properties
        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
        public PackedVirtualUniqueIdentifier UnionData { get => unionData; set => unionData = value; }
        public short VisType { get => visType; set => visType = value; }
        public short Flags { get => flags; set => flags = value; }

        #endregion Properties

        #region Fields
        private float x = 0;
        private float y = 0;
        private PackedVirtualUniqueIdentifier unionData = new();
        private short visType = 0;
        private short flags = 0;
        #endregion Fields

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("X: " + x);
            sb.AppendLine("Y: " + y);
            sb.AppendLine("Packed Data: ");
            sb.Append(unionData.ToString());
            sb.AppendLine("Vis Type: " + visType);
            sb.AppendLine("Flags: " + flags);
            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="PersistentObject"/> object.
        /// </summary>
        public PersistentObject()
        {

        }
        #endregion Constructors
    }
}
