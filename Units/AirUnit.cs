using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalconCampaign.Units
{
    /// <summary>
    /// Common Properties for all Air Units.
    /// </summary>
    public class AirUnit : Unit
    {
        #region Functional Methods
        public new void Write(Stream stream)
        {
            base.Write(stream);
        }

        public override string ToString()
        {
            return base.ToString();
        }
        #endregion Functional MEthods

        #region Constructors
        /// <summary>
        /// Default Constructor.
        /// </summary>
        protected AirUnit()
            : base()
        {
        }
        /// <summary>
        /// Constructor using the supplied stream and version information.
        /// </summary>
        /// <param name="stream"><see cref="stream"/> object to read the data from.</param>
        /// <param name="version">File version information passed to Base Class.</param>
        public AirUnit(Stream stream, int version)
            : base(stream, version)
        {

        }
        #endregion Constructors

    }
}
