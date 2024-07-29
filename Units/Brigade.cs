using System.Collections.ObjectModel;
using System.Text;
using FalconCampaign.Components;

namespace FalconCampaign.Units
{
    /// <summary>
    /// Brigade Unit in the Campaign.
    /// </summary>
    public class Brigade : GroundUnit
    {
        #region Properties
        /// <summary>
        /// Collection of Element IDs in the Brigade.
        /// </summary>
        public Collection<VirtualUniqueIdentifier> Element
        {
            get => element;
            set
            {
                while (value.Count > 16) value.RemoveAt(value.Count - 1);
                element = value;
            }
        }
        /// <summary>
        /// Number of Elements in the Brigade.
        /// </summary>
        public byte Elements { get => (byte)Element.Count; }
        #endregion Properties

        #region Fields
        private Collection<VirtualUniqueIdentifier> element = [];
        private byte elements = 0;
        #endregion Fields

        #region Helper Methods
        internal new void Read(Stream stream)
        {
            using BinaryReader reader = new(stream, Encoding.Default, leaveOpen: true);
            elements = reader.ReadByte();
            element = [];
            for (int i = 0; i < elements; i++)
                element.Add(new()
                {
                    ID = reader.ReadUInt32(),
                    Creator = reader.ReadUInt32()
                });
        }

        internal new void Write(Stream stream)
        {
            base.Write(stream);
            using BinaryWriter writer = new(stream, Encoding.Default, leaveOpen: true);
            writer.Write((byte)element.Count);
            for (int i = 0; i < element.Count; i++)
            {
                writer.Write(Element[i].ID);
                writer.Write(Element[i].Creator);
            }
        }
        #endregion Helper Methods

        #region Funcitonal Methods

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Brigade Units: ");
            for (int i = 0; i < Elements; i++)
                sb.AppendLine("   Element ID: " + element[i]);

            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="Brigade"/> object.
        /// </summary>
        public Brigade()
            : base()
        {
        }
        /// <summary>
        /// Initializes an instance of the <see cref="Brigade"/> object with the supplied data.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object with initialization data.</param>
        /// <param name="version">File Version.</param>
        public Brigade(Stream stream, int version)
            : base(stream, version)
        {
            Read(stream);

        }
        #endregion Constructors
    }
}
