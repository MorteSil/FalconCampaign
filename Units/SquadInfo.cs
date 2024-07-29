using System.Text;
using FalconCampaign.Components;
using Utilities.GeoLib;

namespace FalconCampaign.Units
{
    /// <summary>
    /// A Squadron or Organizational Unit in the Campaign.
    /// </summary>
    public class SquadInfo
    {

        #region Properties
        /// <summary>
        /// X Coordinate of the Squadron.
        /// </summary>
        public float X { get => x; set => x = value; }
        /// <summary>
        /// Y Coordinate of the Squadron
        /// </summary>
        public float Y { get => y; set => y = value; }
        /// <summary>
        /// Map Coordiante Wrapper for X and Y Coordinate Values.
        /// </summary>
        public GeoPoint Location
        {
            get => new(x, y);
            set
            {
                x = (float)value.X;
                y = (float)value.Y;
            }
        }
        /// <summary>
        /// Squadron ID.
        /// </summary>
        public VirtualUniqueIdentifier ID
        { get => iD; set => iD = value; }
        /// <summary>
        /// Squadron Descriptin Index. Used with the Name and Specialty to create the Unit Name displayed to the user.
        /// </summary>
        public short DescriptionIndex { get => descriptionIndex; set => descriptionIndex = value; }
        /// <summary>
        /// Used to build the Unit Name shown to the user.
        /// </summary>
        public short NameID { get => nameID; set => nameID = value; }
        /// <summary>
        /// Icon to use for the Airbase where this Squadron is stationed.
        /// </summary>
        public short AirbaseIcon { get => airbaseIcon; set => airbaseIcon = value; }
        /// <summary>
        /// ID of the Patch used for this Unit.
        /// </summary>
        public short SquadronPatch { get => squadronPatch; set => squadronPatch = value; }
        /// <summary>
        /// Unit Specialty Designator.
        /// </summary>
        public byte Specialty { get => specialty; set => specialty = value; } // TODO: Convert to enum?
        /// <summary>
        /// Strength rating of the Squadron.
        /// </summary>
        public byte CurrentStrength { get => currentStrength; set => currentStrength = value; }
        /// <summary>
        /// Country that owns the Squadron.
        /// </summary>
        public byte Country { get => country; set => country = value; } // TODO: Enum
        /// <summary>
        /// Airbase Name.
        /// </summary>
        public string AirbaseName { get => airbaseName; set => airbaseName = value; }
        /// <summary>
        /// Squadron Flags
        /// </summary>
        public uint Flags { get => flags; set => flags = value; }
        /// <summary>
        /// Campaign ID
        /// </summary>
        public uint CampaignID { get => campiD; set => campiD = value; }
        /// <summary>
        /// Squadron Name that can be manually set aside from the Generated Name.
        /// </summary>
        public string SquadronName { get => squadronName; set => squadronName = value; }

        #endregion Properties

        #region Fields
        private float x = 0;
        private float y = 0;
        private VirtualUniqueIdentifier iD = new();
        private short descriptionIndex = 0;
        private short nameID = 0;
        private short airbaseIcon = 0;
        private short squadronPatch = 0;
        private byte specialty = 0;
        private byte currentStrength = 0;
        private byte country = 0;
        private string airbaseName = "";
        private uint flags = 0;
        private uint campiD = new();
        private string squadronName = "";
        #endregion Fields

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ID.ToString());
            sb.AppendLine("Description Index: " + DescriptionIndex);
            sb.AppendLine("Name ID: " + NameID);
            sb.AppendLine("Airbase Icon: " + AirbaseIcon);
            sb.AppendLine("Squadron Patch: " + SquadronPatch);
            sb.AppendLine("Specialty: " + Specialty);
            sb.AppendLine("Current Strength: " + CurrentStrength);
            sb.AppendLine("Country: " + Country);
            sb.AppendLine("Airbase Name: " + AirbaseName);
            sb.AppendLine("Flags: " + flags);
            sb.AppendLine("Campaign ID: " + campiD);
            sb.AppendLine("Squadron Name: " + squadronName);

            return sb.ToString();

        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="SquadInfo"/> object.
        /// </summary>
        public SquadInfo() { }
        #endregion Constructors

    }
}
