using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalconCampaign.Units
{
    /// <summary>
    /// Loadout Information.
    /// </summary>
    public class Loadout
    {
        #region Properties
        /// <summary>
        /// Collection of <see cref="LoadoutStruct"/> objects that make up the Loadout.
        /// </summary>
        public Collection<LoadoutEntry> Stores { get => stores; set => stores = value; }
        #endregion Properties

        #region Fields
        private Collection<LoadoutEntry> stores = [];
        #endregion Fields

        #region Functional Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Loadout:");
            foreach (LoadoutEntry entry in Stores)
            {
                sb.AppendLine("***** Hardpoint *****");
                sb.Append(entry.ToString());
            }
            return sb.ToString();
        }
        #endregion Funcitnoal Methods

        #region Constructors
        public Loadout() { }
        #endregion Constructors

    }
}
