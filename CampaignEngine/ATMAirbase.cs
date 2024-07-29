using System.Collections.ObjectModel;
using System.Text;
using FalconCampaign.Components;

namespace FalconCampaign.CampaignEngine
{
    /// <summary>
    /// Airbase Component for the Air Tasking Manager.
    /// </summary>
    public class ATMAirbase
    {
        #region Properties
        /// <summary>
        /// Airbase ID.
        /// </summary>
        public VirtualUniqueIdentifier ID { get => id; set => id = value; }
        /// <summary>
        /// Current Airbase Schedule.
        /// </summary>
        public Collection<byte> Schedule { get => schedule; set => schedule = value; }
        #endregion Properties

        #region Fields
        private VirtualUniqueIdentifier id = new();
        Collection<byte> schedule = [];
        #endregion Fields

        #region Helper Methods
        internal void Read(Stream stream)
        {
            using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
            id = new()
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32()
            };

            Schedule = [];
            for (int j = 0; j < 32; j++)
                Schedule.Add(reader.ReadByte());
        }

        internal void Write(Stream stream)
        {
            using var writer = new BinaryWriter(stream, Encoding.Default, leaveOpen: true);
            writer.Write(id.ID);
            writer.Write(id.Creator);

            for (int j = 0; j < (schedule.Count < 33 ? schedule.Count : 32); j++)
                writer.Write(Schedule[j]);
        }
        #endregion Helper Methods

        #region Functional Methods

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("***** ID *****");
            sb.Append(id.ToString());
            sb.AppendLine("Schedule: ");
            for (int i = 0; i < schedule.Count; i++)
                sb.AppendLine("   Event " + i + ": " + schedule[i]);
            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="ATMAirbase"/> object.
        /// </summary>
        protected ATMAirbase() { }
        /// <summary>
        /// Initializes an instance of the <see cref="ATMAirbase"/> object with the data supplied.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> with initialization data.</param>
        public ATMAirbase(Stream stream)
            : this()
        {
            Read(stream);
        }
        #endregion Constructors

    }
}
