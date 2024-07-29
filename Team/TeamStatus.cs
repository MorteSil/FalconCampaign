using System.Text;

namespace FalconCampaign.Team
{
    /// <summary>
    /// Status Information about a Team in the Campaign.
    /// </summary>
    public class TeamStatus
    {
        #region Properties
        /// <summary>
        /// Number of Air Defense Units Available.
        /// </summary>
        public ushort AirDefenseVehicles { get => airDefenseVehs; set => airDefenseVehs = value; }
        /// <summary>
        /// Number of Aircraft Units Available.
        /// </summary>
        public ushort Aircraft { get => aircraft; set => aircraft = value; }
        /// <summary>
        /// Number of Ground Units Available.
        /// </summary>
        public ushort GroundVehicles { get => groundVehs; set => groundVehs = value; }
        /// <summary>
        /// Number of Naval Units Available.
        /// </summary>
        public ushort Ships { get => ships; set => ships = value; }
        /// <summary>
        /// Supply Level.
        /// </summary>
        public ushort Supply { get => supply; set => supply = value; }
        /// <summary>
        /// Fuel Level.
        /// </summary>
        public ushort Fuel { get => fuel; set => fuel = value; }
        /// <summary>
        /// Number of Airbases Available.
        /// </summary>
        public ushort Airbases { get => airbases; set => airbases = value; }
        /// <summary>
        /// Percentage Value of Supplies Compared to Maximum Value.
        /// </summary>
        public byte SupplyLevel { get => supplyLevel; set => supplyLevel = value; }
        /// <summary>
        /// Percentage Value of Fuel Compared to Maximum Value.
        /// </summary>
        public byte FuelLevel { get => fuelLevel; set => fuelLevel = value; }

        #endregion Properties

        #region Fields
        private ushort airDefenseVehs = 0;
        private ushort aircraft = 0;
        private ushort groundVehs = 0;
        private ushort ships = 0;
        private ushort supply = 0;
        private ushort fuel = 0;
        private ushort airbases = 0;
        private byte supplyLevel = 0;
        private byte fuelLevel = 0;
        #endregion Fields

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Air Defense Units: " + airDefenseVehs);
            sb.AppendLine("Aircraft Units: " + aircraft);
            sb.AppendLine("Ground Units: " + groundVehs);
            sb.AppendLine("Naval Units: " + ships);
            sb.AppendLine("Supply Level: " + supply + " (" + supplyLevel + "% of Maximum)");
            sb.AppendLine("Supply Level: " + Fuel + " (" + fuelLevel + "% of Maximum)");

            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Constructor for the <see cref="TeamStatus"/> object.
        /// </summary>
        public TeamStatus() { }
        #endregion Constructors


    }
}
