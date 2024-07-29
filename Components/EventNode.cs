using System.Text;

namespace FalconCampaign.Components
{
    /// <summary>
    /// An Event in the Campaign.
    /// </summary>
    public class EventNode
    {
        #region Properties
        /// <summary>
        /// X Coordinate of the Event.
        /// </summary>
        public short X { get => x; set => x = value; }
        /// <summary>
        /// Y Coordinate of the Event.
        /// </summary>
        public short Y { get => y; set => y = value; }
        /// <summary>
        /// Map Coordinate of the Event.
        /// </summary>
        public Utilities.GeoLib.GeoPoint Location
        {
            get => new(x, y);
            set
            {
                x = (short)value.X;
                y = (short)value.Y;
            }
        }
        /// <summary>
        /// Offset from Day 0, 00:00 in milliseconds when the event took place.
        /// </summary>
        public TimeSpan Time { get => new(0,0,0,0,(int)time); set => time = (uint)value.TotalMilliseconds; }
        /// <summary>
        /// Event Flags.
        /// </summary>
        public byte Flags { get => flags; set => flags = value; } // TODO: Break This Out
        /// <summary>
        /// Team that executed the event.
        /// </summary>
        public byte Team { get => team; set => team = value; }
        /// <summary>
        /// Description of the Event.
        /// </summary>
        public string EventText { get => eventText; set => eventText = value; }
        #endregion Properties

        #region Fields
        private short x = 0;
        private short y = 0;
        private uint time = 0;
        private byte flags = 0;
        private byte team = 0;
        private string eventText = "";
        #endregion Fields

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Team: " + Team);
            sb.AppendLine("Time: " + Time);
            sb.AppendLine("Flags: " + Flags);
            sb.AppendLine("Location: ");
            sb.Append(Location.ToString());
            sb.AppendLine("Description: " + EventText);

            return sb.ToString();


        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="EventNode"/> object.
        /// </summary>
        public EventNode()
        {

        }
        #endregion Constructors

    }
}
