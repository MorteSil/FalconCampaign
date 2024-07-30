using System.Collections.ObjectModel;
using System.Text;
using Utilities.Logging;

namespace FalconCampaign.Files
{
    /// <summary>
    /// <para>Represents the Campaign CAM File Type</para>
    /// <para>CAM Files are Container Files with multiple other files embedded within.</para>
    /// <para>Some Embedded Files may be compressed with the LZSS Compression scheme and will need to be decompressed prior to use.</para>
    /// </summary>
    public class CampaignFile : AppFile
    {
        #region Properties
        /// <summary>
        /// Information about the Embedded Files in the main Campaign CAM File.
        /// </summary>
        public Collection<EmbeddedFileInfo> EmbeddedFiles
        {
            get => new(embeddedFiles);
            set
            {
                embeddedFiles.Clear();
                foreach (EmbeddedFileInfo embeddedFileInfo in value)
                { embeddedFiles.Add(embeddedFileInfo); }
            }
        }
        /// <summary>
        /// Main Campaign Data File.
        /// </summary>
        public CMP CMPFile { get => cmpFile; set => cmpFile = value; }
        /// <summary>
        /// Campaign Objective File
        /// </summary>
        public OBJ OBJFile { get => objFile; set => objFile = value; }
        /// <summary>
        /// Campaign Objective Delta File.
        /// </summary>
        public OBD OBDFile { get => obdFile; set => obdFile = value; }
        /// <summary>
        /// Campaign Unit File.
        /// </summary>
        public UNI UNIFile { get => uniFile; set => uniFile = value; }
        /// <summary>
        /// Contains the Team Information for the Campaign.
        /// </summary>
        public TEA TeaFile { get => teaFile; set => teaFile = value; }
        /// <summary>
        /// Campaign File Version Info.
        /// </summary>
        public VER VERFile { get => verFile; set => verFile = value; }        
        /// <summary>
        /// Campaign Event File.
        /// </summary>
        public EVT EVTFile { get => evtFile; set => evtFile = value; }
        /// <summary>
        /// Persistent Objects in the Campaign.
        /// </summary>
        public PST PSTFile { get => pstFile; set => pstFile = value; }
        /// <summary>
        /// Pilot Information in the Campaign File.
        /// </summary>
        public PLT PLTFile { get => pltFile; set => pltFile = value; }
        /// <summary>
        /// Campaign Object Database.
        /// </summary>
        public FalconDatabase.Database Database { get => database; set => database = value; }
        /// <summary>
        /// Return <see langword="true"/> if initialization fails and the return object has a default configuration.
        /// </summary>
        public override bool IsDefaultInitialization { get { return false; } }
       
        #endregion Properties

        #region Fields

        private Collection<EmbeddedFileInfo> embeddedFiles = [];
        protected byte[] rawBytes = [];

        // Embedded Files
        private CMP cmpFile = new();
        private OBJ objFile = new();
        private VER verFile = new();
        private OBD obdFile = new();
        private UNI uniFile = new();
        private TEA teaFile = new();
        private EVT evtFile = new();
        private PLT pltFile = new();
        private PST pstFile = new();
        private POL polFile = new();

        // DB 
        private FalconDatabase.Database database = new();
        #endregion Fields

        #region Helper Methods
        protected override bool Read(byte[] data)
        {

            try
            {
                uint directoryStartOffset = BitConverter.ToUInt32(data, 0);
                uint numEmbeddedFiles = BitConverter.ToUInt32(data, (int)directoryStartOffset);
                int curLoc = (int)directoryStartOffset + 4;
                for (int i = 0; i < numEmbeddedFiles; i++)
                {
                    EmbeddedFileInfo thisFileResourceInfo = new ();
                    byte thisFileNameLength = (byte)(data[curLoc] & 0xFF);
                    curLoc++;
                    string thisFileName = Encoding.ASCII.GetString(data, curLoc, thisFileNameLength);
                    thisFileResourceInfo.FileName = thisFileName;
                    curLoc += thisFileNameLength;
                    thisFileResourceInfo.FileOffset = BitConverter.ToUInt32(data, curLoc);
                    curLoc += 4;
                    thisFileResourceInfo.FileSizeBytes = BitConverter.ToUInt32(data, curLoc);
                    curLoc += 4;
                    embeddedFiles.Add(thisFileResourceInfo);
                }

                int index = 0;
                // Get Version Info First
                for (index = 0; index < embeddedFiles.Count; index++)
                    if (embeddedFiles[index].FileName.Contains("VER", StringComparison.CurrentCultureIgnoreCase))
                        VERFile = new VER(GetEmbeddedFileContents(embeddedFiles[index].FileName, data));

                int? ver = VERFile.Version;

                for (index = 0; index < embeddedFiles.Count; index++)
                    if (embeddedFiles[index].FileName.Contains("CMP", StringComparison.CurrentCultureIgnoreCase))
                        cmpFile = new CMP(GetEmbeddedFileContents(embeddedFiles[index].FileName, data), ver is null ? int.MaxValue : (int)ver);
                    else if (embeddedFiles[index].FileName.Contains("OBJ", StringComparison.CurrentCultureIgnoreCase))
                        objFile = new OBJ(GetEmbeddedFileContents(embeddedFiles[index].FileName, data), ver is null ? int.MaxValue : (int)ver);
                    else if (embeddedFiles[index].FileName.Contains("OBD", StringComparison.CurrentCultureIgnoreCase))
                        obdFile = new OBD(GetEmbeddedFileContents(embeddedFiles[index].FileName, data), ver is null ? int.MaxValue : (int)ver);
                    else if (embeddedFiles[index].FileName.Contains("TEA", StringComparison.CurrentCultureIgnoreCase))
                        teaFile = new TEA(GetEmbeddedFileContents(embeddedFiles[index].FileName, data), ver is null ? int.MaxValue : (int)ver);
                    else if (embeddedFiles[index].FileName.Contains("UNI", StringComparison.CurrentCultureIgnoreCase))
                        uniFile = new UNI(GetEmbeddedFileContents(embeddedFiles[index].FileName, data), ver is null ? int.MaxValue : (int)ver, database.Classes);
                    else if (embeddedFiles[index].FileName.Contains("EVT", StringComparison.CurrentCultureIgnoreCase))
                        evtFile = new EVT(GetEmbeddedFileContents(embeddedFiles[index].FileName, data), ver is null ? int.MaxValue : (int)ver);
                    else if (embeddedFiles[index].FileName.Contains("PLT", StringComparison.CurrentCultureIgnoreCase))
                        pltFile = new PLT(GetEmbeddedFileContents(embeddedFiles[index].FileName, data), ver is null ? int.MaxValue : (int)ver);
                    else if (embeddedFiles[index].FileName.Contains("PST", StringComparison.CurrentCultureIgnoreCase))
                        pstFile = new PST(GetEmbeddedFileContents(embeddedFiles[index].FileName, data), ver is null ? int.MaxValue : (int)ver);
                    else if (embeddedFiles[index].FileName.Contains("POL", StringComparison.CurrentCultureIgnoreCase))
                        polFile = new POL(GetEmbeddedFileContents(embeddedFiles[index].FileName, data), ver is null ? int.MaxValue : (int)ver);



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
            // Implement after files are added
            return [0];
        }

        #endregion Helper Methods

        #region Functional Methods

        private byte[] GetEmbeddedFileContents(string embeddedFileName, byte[] data)
        {
            for (int i = 0; i < embeddedFiles.Count; i++)
            {
                EmbeddedFileInfo thisFile = embeddedFiles[i];
                if (thisFile.FileName.Equals(embeddedFileName, StringComparison.InvariantCultureIgnoreCase))
                {
                    byte[] toReturn = new byte[thisFile.FileSizeBytes];
                    Array.Copy(data, thisFile.FileOffset, toReturn, 0, thisFile.FileSizeBytes);
                    return toReturn;
                }
            }
            throw new FileNotFoundException(embeddedFileName);
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            for (int i = 0; i < embeddedFiles.Count; i++)
            {
                sb.AppendLine("********************* Embedded File " + i + ": *********************");
                sb.AppendLine("     File Name: " + embeddedFiles[i].FileName);
                sb.AppendLine("     File Offset: " + embeddedFiles[i].FileOffset);
                sb.AppendLine("     File Size: " + embeddedFiles[i].FileSizeBytes);
            }
            return sb.ToString();
        }

        #region Equality Functions
        public bool Equals(CampaignFile? other)
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

            if (other is not CampaignFile comparator)
                return false;
            else
                return Equals(comparator);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 2539;
                foreach (EmbeddedFileInfo fileInfo in embeddedFiles)
                    hash ^= fileInfo.GetHashCode();

                return hash;
            }
        }
        #endregion Equality Functions

        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Initializes a default instance of the <see cref="CampaignFile"/> object.
        /// </summary>
        public CampaignFile()
        {
            _FileType = ApplicationFileType.CampaignCAM;
            _StreamType = FileStreamType.Binary;
            _IsCompressed = false;
        }
        /// <summary>
        /// <para>Initializes an instance of the <see cref="CampaignFile"/> object.</para>
        /// <para>Accepts a File Path as a valid input and checks if the file exists and has valid <see cref="CampaignFile"/> initialization data.</para> 
        /// </summary>
        /// <param name="path">Path to a file with the Initialization Data Required for this <see cref="AppFile"/> type.</param>
        /// <param name="database"><see cref="FalconDatabase.Database.Database"/> object with values loaded from the Falcon4 Database Files in the [InstallFolder]/Data[/Campaign]/TerrData/Objects/ Folder</param>
        public CampaignFile(string path, FalconDatabase.Database database)
            : this()
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(path);
            this.database = database;
            if (File.Exists(path))
                Load(path);
        }
        /// <summary>
        /// <para>Initializes an instance of the <see cref="CampaignFile"/> object.</para>
        /// <para>Accepts a File Path as a valid input and checks if the file exists and has valid <see cref="CampaignFile"/> initialization data.</para> 
        /// </summary>
        /// <param name="campaignPath">Path to a valid Falcon 4 Campaign Folder.</param>
        /// <param name="databasePath">Path to a valid Falcon 4 Database Folder--Must include a subfolder with ObjectiveData</param>
        /// <exception cref="ArgumentException"></exception>
        public CampaignFile(string campaignPath, string databasePath)
            : this()
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(campaignPath);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(databasePath);
            this.database = new(databasePath);
            if (database is null)
                throw new ArgumentException(databasePath + " does not contain a valid path to a Falcon 4 Database File.");
        }

        #endregion Constructors

        #region Subclasses
        /// <summary>
        /// Dictionary details for an Embedded File within the Content Area of this Container File.
        /// </summary>
        public class EmbeddedFileInfo
        {
            #region Properties
            /// <summary>
            /// File Name of the Embedded File.
            /// </summary>
            public string FileName
            { get; set; } = "";
            /// <summary>
            /// <para>Offset within the Raw Data where the Embedded File data starts.</para>
            /// <para>The data may need to be decompressed using the LZSS Decompression Method.</para>
            /// </summary>
            public uint FileOffset
            { get; set; } = 0;
            /// <summary>
            /// Size of the embedded data in <see cref="byte"/>s.
            /// </summary>
            public uint FileSizeBytes
            { get; set; } = 0;
            #endregion Properties

            #region Constructors
            /// <summary>
            /// Default Constructor for the <see cref="EmbeddedFileInfo"/> object.
            /// </summary>
            public EmbeddedFileInfo()
            {

            }
            /// <summary>
            /// Initializes an instance of the <see cref="EmbeddedFileInfo"/> object with data supplied.
            /// </summary>
            /// <param name="fileName">The Filename of the embedded file in the CAM File.</param>
            /// <param name="fileOffset">The Offset where the embedded file begins.</param>
            /// <param name="fileSize">Length in bytes of the embedded file.</param>
            public EmbeddedFileInfo(string fileName, uint fileOffset, uint fileSize)
            {
                FileName = fileName;
                FileOffset = fileOffset;
                FileSizeBytes = fileSize;
            }

            #endregion Constructors
        }
        #endregion Subclasses
    }
}

