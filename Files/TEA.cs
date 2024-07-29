using System.Collections.ObjectModel;
using System.Text;
using FalconCampaign.CampaignEngine;
using Utilities.Logging;

namespace FalconCampaign.Files
{
    /// <summary>
    /// Contains Team Information from the Campaign CAM File.
    /// </summary>
    public class TEA : AppFile, IEquatable<TEA>
    {
        #region Properties  
        /// <summary>
        /// Collection of Teams in the Campaign.
        /// </summary>
        public Collection<Team.Team> Teams { get => teams; set => teams = value; }
        /// <summary>
        /// Collection of each Team's Air Tasking Manager.
        /// </summary>
        public Collection<AirTaskingManager> AirTaskingManagers { get => airTaskingManagers; set => airTaskingManagers = value; }
        /// <summary>
        /// Collection of each Team's Ground Tasking Manager.
        /// </summary>
        internal Collection<GroundTaskingManager> GroundTaskingManagers { get => groundTaskingManagers; set => groundTaskingManagers = value; }
        /// <summary>
        /// Collection of each Team's Naval Tasking Manager.
        /// </summary>
        public Collection<NavalTaskingManager> NavalTaskingManagers { get => navalTaskingManagers; set => navalTaskingManagers = value; }
        /// <summary>
        /// Returns <see langword="true"/> if initialization fails and the return object has a default configuration.
        /// </summary>
        public override bool IsDefaultInitialization => false;
        /// <summary>
        /// File Version
        /// </summary>
        public int Version { get => version; set => version = value; }
        #endregion Properties

        #region Fields
        private short numTeams = 0;
        private Collection<Team.Team> teams = [];
        private Collection<AirTaskingManager> airTaskingManagers = [];
        private Collection<GroundTaskingManager> groundTaskingManagers = [];
        private Collection<NavalTaskingManager> navalTaskingManagers = [];
        private int version = int.MaxValue;
        #endregion Fields

        #region Helper Methods       

        protected override bool Read(byte[] data)
        {
            try
            {
                using MemoryStream stream = new(data);
                using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
                numTeams = reader.ReadInt16();

                if (numTeams > 8)
                    numTeams = 8;
                teams = [];
                airTaskingManagers = [];
                groundTaskingManagers = [];
                navalTaskingManagers = [];

                for (int i = 0; i < numTeams; i++)
                {
                    teams.Add(new(stream, Version));
                    airTaskingManagers.Add(new(stream, Version));
                    groundTaskingManagers.Add(new(stream, Version));
                    navalTaskingManagers.Add(new(stream, Version));
                }
            }
            catch (Exception ex)
            {                
                ErrorLog.CreateLogFile(ex, "This error occurred while attempting to load " + _FileType);                
                throw;
            }
            return true;
        }

        protected override byte[] Write()
        {
            try
            {
                using MemoryStream stream = new();
                using var writer = new BinaryWriter(stream, Encoding.Default, leaveOpen: true);
                numTeams = (short)Math.Min(8, teams.Count);
                writer.Write(numTeams);
                for (int i = 0; i < numTeams; i++)
                {
                    Teams[i].Write(stream);
                    AirTaskingManagers[i].Write(stream);
                    GroundTaskingManagers[i].Write(stream);
                    NavalTaskingManagers[i].Write(stream);
                }
                byte[] output = stream.ToArray();
                stream.Close();
                return output;

            }
            catch (Exception ex)
            {
                ErrorLog.CreateLogFile(ex, "This error occurred while attempting to write " + _FileType);
                throw;
            }

        }

        #endregion Helper Methods

        #region Functional Methods

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("***************************** TEA File *****************************");
            sb.AppendLine("Version: " + Version);
            sb.AppendLine("Number of Teams: " + teams.Count);
            for (int i = 0; i < Teams.Count; i++)
            {
                sb.AppendLine("***** Team Information *****");
                sb.Append(teams[i].ToString());
            }
            for (int i = 0; i < airTaskingManagers.Count; i++)
            {
                sb.AppendLine("***** Air Tasking Manager Information *****");
                sb.Append(airTaskingManagers[i].ToString());
            }
            for (int i = 0; i < airTaskingManagers.Count; i++)
            {
                sb.AppendLine("***** Ground Tasking Manager Information *****");
                sb.Append(groundTaskingManagers[i].ToString());
            }
            for (int i = 0; i < navalTaskingManagers.Count; i++)
            {
                sb.AppendLine("***** Naval Tasking Manager Information *****");
                sb.Append(navalTaskingManagers[i].ToString());
            }

            return sb.ToString();
        }

        #region Equality Functions
        public bool Equals(TEA? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != GetType()) return false;

            return other.ToString() == ToString() && other.GetHashCode() == GetHashCode();
        }
        public override bool Equals(object? other)
        {
            if (other == null)
                return false;

            if (other is not TEA comparator)
                return false;
            else
                return Equals(comparator);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 2539;
                hash = hash * 5483 + Version.GetHashCode();
                hash = hash * 5483 + teams.GetHashCode();
                hash = hash * 5483 + airTaskingManagers.GetHashCode();
                hash = hash * 5483 + groundTaskingManagers.GetHashCode();
                hash = hash * 5483 + navalTaskingManagers.GetHashCode();

                return hash;
            }
        }
        #endregion Equality Functions

        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Initializes a default instance of the <see cref="TEA"/> object
        /// </summary>
        public TEA()
        {
            _FileType = ApplicationFileType.CampaignTEA;
            _StreamType = FileStreamType.Binary;
            _IsCompressed = false;
        }
        /// <summary>
        /// Initializes an instance of the <see cref="TEA"/> object with the supplied data.
        /// </summary>
        /// <param name="data">Raw Data from the CAM File.</param>
        /// <param name="version">File Version</param>
        public TEA(byte[] data, int version = int.MaxValue)
            : this()
        {
            Version = version;
            Read(data);
        }
        #endregion Cunstructors

    }
}
