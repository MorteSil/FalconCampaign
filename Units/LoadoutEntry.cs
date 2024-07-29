using System.Collections.ObjectModel;
using System.Text;

namespace FalconCampaign.Units
{
    /// <summary>
    /// Represetns a Single Hardpoint in the Loadout.
    /// </summary>
    public class LoadoutEntry
    {
        #region Properties
        /// <summary>
        /// Collection of Weapon ID values in the Loadout.
        /// </summary>
        public Collection<ushort> WeaponID { get => weaponID; set => weaponID = value; }
        /// <summary>
        /// Colleciton of Weapon Counts in the Loadout.
        /// </summary>
        public Collection<byte> WeaponCount { get => weaponCount; set => weaponCount = value; }
        #endregion Properties

        #region Fields
        private Collection<ushort> weaponID = [];
        private Collection<byte> weaponCount = [];
        #endregion Fields

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (WeaponID.Count == WeaponCount.Count)
            {
                for (int i = 0; i < WeaponID.Count; i++)
                {
                    sb.AppendLine("Weapon " + i + " ID: " + weaponID[i]);
                    sb.AppendLine("Weapon Count: " + weaponCount[i]);
                }
            }
            else
            {
                sb.AppendLine("Weapon IDs: ");
                foreach (ushort weaponID in WeaponID)
                    sb.AppendLine("Weapon ID: " + weaponID);

                sb.AppendLine("Weapon Cuounts: ");
                foreach (ushort weaponID in WeaponCount)
                    sb.AppendLine("Weapon Count: " + weaponID);
            }

            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        public LoadoutEntry() { }
        #endregion Constructors

    }
}
