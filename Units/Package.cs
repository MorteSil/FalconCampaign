using FalconCampaign.CampaignEngine;
using FalconCampaign.Components;
using FalconCampaign.Enums;
using FalconDatabase.Enums;
using System.Collections.ObjectModel;
using System.Text;
using Utilities.GeoLib;

namespace FalconCampaign.Units
{
    /// <summary>
    /// Package in the Campaign.
    /// </summary>
    public class Package : AirUnit
    {
        #region Properties
        /// <summary>
        /// Number of Package Elements, limited to 16.
        /// </summary>
        public byte ElementCount { get => elements; set => elements = value; }
        /// <summary>
        /// Collection of Package Elements.
        /// </summary>
        public Collection<VirtualUniqueIdentifier> Element { get => element; set => element = value; }
        /// <summary>
        /// Package ID for Interceptor Support.
        /// </summary>
        public VirtualUniqueIdentifier Interceptor { get => interceptor; set => interceptor = value; }
        /// <summary>
        /// Package ID for AWACS Support.
        /// </summary>
        public VirtualUniqueIdentifier AWACS { get => awacs; set => awacs = value; }
        /// <summary>
        /// Package ID for JSTARS Support.
        /// </summary>
        public VirtualUniqueIdentifier JSTARS { get => jstar; set => jstar = value; }
        /// <summary>
        /// Package ID for ECM Support.
        /// </summary>
        public VirtualUniqueIdentifier ECM { get => ecm; set => ecm = value; }
        /// <summary>
        /// Package ID for Tanker Support.
        /// </summary>
        public VirtualUniqueIdentifier Tanker { get => tanker; set => tanker = value; }
        /// <summary>
        /// Package Delays.
        /// </summary>
        public byte WaitCycles { get => waitCycles; set => waitCycles = value; }
        /// <summary>
        /// Number of Flights in the Package, limited to 16.
        /// </summary>
        public byte FlightCount { get => flights; set => flights = value; }
        /// <summary>
        /// Time to Delay.
        /// </summary>
        public short WaitLength { get => waitLength; set => waitLength = value; } // TODO: Confirm Usage.
        /// <summary>
        /// Ingress Point to build Flight Waypoints from.
        /// </summary>
        public GeoPoint IngressPoint
        {
            get => new(iax, iay);
            set
            {
                iax = (short)value.X;
                iay = (short)value.Y;
            }
        }
        /// <summary>
        /// Egress Point to build Flight Waypoints from.
        /// </summary>
        public GeoPoint EgressPoint
        {
            get => new(eax, eay);
            set
            {
                eax = (short)value.X;
                eay = (short)value.Y;
            }
        }
        /// <summary>
        /// Base Point to build Flight Waypoints from.
        /// </summary>
        public GeoPoint BasePoint
        {
            get => new(bpx, bpy);
            set
            {
                bpx = (short)value.X;
                bpy = (short)value.Y;
            }
        }
        /// <summary>
        /// Target Point to build Flight Waypoints from.
        /// </summary>
        public GeoPoint TargetPoint
        {
            get => new(tpx, tpy);
            set
            {
                tpx = (short)value.X;
                tpy = (short)value.Y;
            }
        }
        /// <summary>
        /// Takeoff Time.
        /// </summary>
        public TimeSpan Takeoff
        {
            get => new(0, 0, 0, 0, (int)takeoff);
            set => takeoff = (uint)value.TotalMilliseconds;
        }
        /// <summary>
        /// Target Time.
        /// </summary>
        public TimeSpan TargetTime
        {
            get => new TimeSpan(0, 0, 0, 0, (int)tp_time);
            set => tp_time = (uint)value.TotalMilliseconds;
        }
        /// <summary>
        /// Package Flags.
        /// </summary>
        public uint PackageFlags { get => packageFlags; set => packageFlags = value; }
        /// <summary>
        /// Capabilities Index.
        /// </summary>
        public short Caps { get => caps; set => caps = value; } // TODO: Confirm usage.
        /// <summary>
        /// Mission Requests.
        /// </summary>
        public short Requests { get => requests; set => requests = value; } // TODO: Confirm usage.
        /// <summary>
        /// Package Threats.
        /// </summary>
        public short ThreatStats { get => threatStats; set => threatStats = value; }
        /// <summary>
        /// Responses to Requests.
        /// </summary>
        public short Responses { get => responses; set => responses = value; }
        /// <summary>
        /// Number of Ingress Points to the Target.
        /// </summary>
        public byte IngressWaypointCount { get => (byte)ingress_waypoints.Count; }
        /// <summary>
        /// Collection of Waypoints to build the Ingress Path.
        /// </summary>
        public Collection<Waypoint> IngressWaypoints
        {
            get => ingress_waypoints;
            set
            {
                while (value.Count > 16) value.RemoveAt(value.Count - 1);
                ingress_waypoints = value;
            }
        }
        /// <summary>
        /// Number of Egress Points from the Target.
        /// </summary>
        public byte EgressWaypointCount { get => (byte)EgressWaypoints.Count; }
        /// <summary>
        /// Collection of Egress Points.
        /// </summary>
        public Collection<Waypoint> EgressWaypoints
        {
            get => egressWaypoints;
            set
            {
                while (value.Count > 16) value.RemoveAt(value.Count - 1);
                egressWaypoints = value;
            }
        }
        /// <summary>
        /// Mission Request Data.
        /// </summary>
        public MissionRequest MissionRequest { get => missionRequest; set => missionRequest = value; }
        #endregion Properties

        #region Fields
        private byte elements = 0;
        private Collection<VirtualUniqueIdentifier> element = [];
        private VirtualUniqueIdentifier interceptor = new();
        private VirtualUniqueIdentifier awacs = new();
        private VirtualUniqueIdentifier jstar = new();
        private VirtualUniqueIdentifier ecm = new();
        private VirtualUniqueIdentifier tanker = new();
        private byte waitCycles = 0;
        private byte flights = 0;
        private short waitLength = 0;
        private short iax = 0; // Ingress
        public short iay = 0;
        public short eax = 0; // Egress
        public short eay = 0;
        public short bpx = 0; // ?
        public short bpy = 0;
        public short tpx = 0; // Target
        public short tpy = 0;
        public uint takeoff = 0;
        public uint tp_time = 0;
        private uint packageFlags = 0;
        private short caps = 0;
        private short requests = 0;
        private short threatStats = 0;
        private short responses = 0;
        private byte numIngressPoints = 0;
        private Collection<Waypoint> ingress_waypoints = [];
        private byte numEgressWaypoints = 0;
        private Collection<Waypoint> egressWaypoints = [];
        private MissionRequest missionRequest = new();

        #endregion

        #region Helper Methods        
        internal new void Read(Stream stream)
        {
            using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
            elements = reader.ReadByte();
            element = [];
            for (int i = 0; i < elements; i++)
            {
                VirtualUniqueIdentifier thisElement = new()
                {
                    ID = reader.ReadUInt32(),
                    Creator = reader.ReadUInt32()
                };
                element.Add(thisElement);
            }
            Interceptor = new()
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32()
            };
            AWACS = new()
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32()
            };
            JSTARS = new()
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32()
            };
            ECM = new()
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32()
            };
            Tanker = new()
            {
                ID = reader.ReadUInt32(),
                Creator = reader.ReadUInt32()
            };
            waitCycles = reader.ReadByte();
            missionRequest = new MissionRequest();
            if (Final && WaitCycles == 0)
            {
                requests = reader.ReadInt16();
                responses = reader.ReadInt16();
                missionRequest.Mission = (byte)reader.ReadInt16();
                missionRequest.Context = (MissionContext)reader.ReadInt16();
                missionRequest.RequesterID = new()
                {
                    ID = reader.ReadUInt32(),
                    Creator = reader.ReadUInt32()
                };
                missionRequest.TargetID = new()
                {
                    ID = reader.ReadUInt32(),
                    Creator = reader.ReadUInt32()
                };
                missionRequest.ToT = new(0, 0, 0, 0, (int)reader.ReadUInt32());
                missionRequest.ActionType = reader.ReadByte();
                missionRequest.Priority = reader.ReadInt16();
                packageFlags = 0;
            }
            else
            {
                flights = reader.ReadByte();
                waitLength = reader.ReadInt16();
                iax = reader.ReadInt16();
                iay = reader.ReadInt16();
                eax = reader.ReadInt16();
                eay = reader.ReadInt16();
                bpx = reader.ReadInt16();
                bpy = reader.ReadInt16();
                tpx = reader.ReadInt16();
                tpy = reader.ReadInt16();
                takeoff = reader.ReadUInt32();
                tp_time = reader.ReadUInt32();
                packageFlags = reader.ReadUInt32();
                caps = reader.ReadInt16();
                requests = reader.ReadInt16();
                Responses = reader.ReadInt16();
                numIngressPoints = reader.ReadByte();
                ingress_waypoints = [];
                for (int j = 0; j < numIngressPoints; j++)
                    ingress_waypoints.Add(new Waypoint(stream, version));
                numEgressWaypoints = reader.ReadByte();
                egressWaypoints = [];
                for (int j = 0; j < numEgressWaypoints; j++)
                    egressWaypoints.Add(new Waypoint(stream, version));
                missionRequest.RequesterID = new()
                {
                    ID = reader.ReadUInt32(),
                    Creator = reader.ReadUInt32()
                };
                missionRequest.TargetID = new()
                {
                    ID = reader.ReadUInt32(),
                    Creator = reader.ReadUInt32()
                };
                missionRequest.SecondaryID = new()
                {
                    ID = reader.ReadUInt32(),
                    Creator = reader.ReadUInt32()
                };
                missionRequest.PakID = new()
                {
                    ID = reader.ReadUInt32(),
                    Creator = reader.ReadUInt32()
                };
                missionRequest.Who = (CountryList)reader.ReadByte();
                missionRequest.Vs = (CountryList)reader.ReadByte();
                reader.ReadBytes(2); //align on int32 boundary
                missionRequest.ToT = new(0, 0, 0, 0, (int)reader.ReadUInt32());
                missionRequest.Tx = reader.ReadInt16();
                missionRequest.Ty = reader.ReadInt16();
                missionRequest.Flags = reader.ReadUInt32();
                missionRequest.Caps = reader.ReadInt16();
                missionRequest.TargetCount = reader.ReadInt16();
                missionRequest.Speed = reader.ReadInt16();
                missionRequest.MatchStrength = reader.ReadInt16();
                missionRequest.Priority = reader.ReadInt16();
                missionRequest.ToTType = reader.ReadByte();
                missionRequest.ActionType = reader.ReadByte();
                missionRequest.Mission = reader.ReadByte();
                missionRequest.Aircraft = reader.ReadByte();
                missionRequest.Context = (MissionContext)reader.ReadByte();
                missionRequest.RoECheck = reader.ReadByte();
                missionRequest.Delayed = reader.ReadByte();
                missionRequest.StartBlock = reader.ReadByte();
                missionRequest.FinalBlock = reader.ReadByte();
                missionRequest.Slots = [];
                for (int k = 0; k < 4; k++)
                    missionRequest.Slots.Add(reader.ReadByte());
                missionRequest.MinimumTakeoff = reader.ReadSByte();
                missionRequest.MaxTakeoff = reader.ReadSByte();
                reader.ReadBytes(3);// align on int32 boundary
            }
        }

        internal new void Write(Stream stream)
        {
            using var writer = new BinaryWriter(stream, Encoding.Default, leaveOpen: true);
            while (element.Count > 16) element.RemoveAt(element.Count - 1);
            writer.Write((byte)element.Count);
            for (int i = 0; i < element.Count; i++)
            {
                var thisElement = element[i];
                writer.Write(thisElement.ID);
                writer.Write(thisElement.Creator);
            }
            writer.Write(Interceptor.ID);
            writer.Write(Interceptor.Creator);
            writer.Write(AWACS.ID);
            writer.Write(AWACS.Creator);
            writer.Write(JSTARS.ID);
            writer.Write(JSTARS.Creator);
            writer.Write(ECM.ID);
            writer.Write(ECM.Creator);
            writer.Write(Tanker.ID);
            writer.Write(Tanker.Creator);
            writer.Write(WaitCycles);
            if (Final && WaitCycles == 0)
            {
                writer.Write(Requests);
                writer.Write(Responses);
                writer.Write((short)MissionRequest.Mission);
                writer.Write((short)MissionRequest.Context);
                writer.Write(MissionRequest.RequesterID.ID);
                writer.Write(MissionRequest.RequesterID.Creator);
                writer.Write(MissionRequest.TargetID.ID);
                writer.Write(MissionRequest.TargetID.Creator);
                writer.Write((uint)MissionRequest.ToT.TotalMilliseconds);
                writer.Write(MissionRequest.ActionType);
                writer.Write(MissionRequest.Priority);
            }
            else
            {
                writer.Write(flights);
                writer.Write(waitLength);
                writer.Write(iax);
                writer.Write(iay);
                writer.Write(eax);
                writer.Write(eay);
                writer.Write(bpx);
                writer.Write(bpy);
                writer.Write(tpx);
                writer.Write(tpy);
                writer.Write(takeoff);
                writer.Write(tp_time);
                writer.Write(packageFlags);
                writer.Write(caps);
                writer.Write(requests);
                writer.Write(responses);
                writer.Write((byte)ingress_waypoints.Count);
                for (int j = 0; j < Math.Min(16, ingress_waypoints.Count); j++)
                    ingress_waypoints[j].Write(stream);
                writer.Write((byte)egressWaypoints.Count);
                for (int j = 0; j < Math.Min(16, egressWaypoints.Count); j++)
                    ingress_waypoints[j].Write(stream);
                writer.Write(missionRequest.RequesterID.ID);
                writer.Write(missionRequest.RequesterID.Creator);
                writer.Write(missionRequest.TargetID.ID);
                writer.Write(missionRequest.TargetID.Creator);
                writer.Write(missionRequest.SecondaryID.ID);
                writer.Write(missionRequest.SecondaryID.Creator);
                writer.Write(missionRequest.PakID.ID);
                writer.Write(missionRequest.PakID.Creator);
                writer.Write((byte)missionRequest.Who);
                writer.Write((byte)missionRequest.Vs);
                writer.Write(new byte[2]); //align on int32 boundary
                writer.Write((uint)missionRequest.ToT.TotalMilliseconds);
                writer.Write(missionRequest.Tx);
                writer.Write(missionRequest.Ty);
                writer.Write(missionRequest.Flags);
                writer.Write(missionRequest.Caps);
                writer.Write(missionRequest.TargetCount);
                writer.Write(missionRequest.Speed);
                writer.Write(missionRequest.MatchStrength);
                writer.Write(missionRequest.Priority);
                writer.Write(missionRequest.ToTType);
                writer.Write(missionRequest.ActionType);
                writer.Write(missionRequest.Mission);
                writer.Write(missionRequest.Aircraft);
                writer.Write((byte)missionRequest.Context);
                writer.Write(missionRequest.RoECheck);
                writer.Write(missionRequest.Delayed);
                writer.Write(missionRequest.StartBlock);
                writer.Write(missionRequest.FinalBlock);
                for (int k = 0; k < 4; k++)
                    writer.Write(missionRequest.Slots[k]);
                writer.Write(missionRequest.MinimumTakeoff);
                writer.Write(missionRequest.MaxTakeoff);
                writer.Write(new byte[3]);// align on int32 boundary
            }
        }
        #endregion Helper Methods

        #region Functional Methods

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Element Count: " + ElementCount);
            for (int i = 0; i < Element.Count; i++)
                sb.AppendLine("   Element " + i + ": " + Element[i]);
            sb.AppendLine("Interceptor ID: " + Interceptor.ID);
            sb.AppendLine("AWACS ID: " + AWACS.ID);
            sb.AppendLine("JSTAR ID: " + JSTARS.ID);
            sb.AppendLine("ECM ID: " + ECM.ID);
            sb.AppendLine("Tanker ID: " + Tanker.ID);
            sb.AppendLine("Wait Cycles: " + WaitCycles);
            sb.AppendLine("Flights: " + FlightCount);
            sb.AppendLine("Wait For: " + WaitLength);
            sb.AppendLine("Ingress Point: ");
            sb.Append(IngressPoint.ToString());
            sb.AppendLine("Egress Point: ");
            sb.Append(EgressPoint.ToString());
            sb.AppendLine("Base Point: ");
            sb.Append(BasePoint.ToString());
            sb.AppendLine("Target Point: ");
            sb.Append(TargetPoint.ToString());
            sb.AppendLine("Takeoff: " + Takeoff.ToString("g"));
            sb.AppendLine("Target Time: " + TargetTime.ToString("g"));
            sb.AppendLine("Flags: " + packageFlags);
            sb.AppendLine("Capabilities: " + caps);
            sb.AppendLine("Requests: " + requests);
            sb.AppendLine("Threat Stats: " + threatStats);
            sb.AppendLine("Responses: " + responses);
            sb.AppendLine("Number of Ingress Waypoints: " + IngressWaypointCount);
            for (int i = 0; i < IngressWaypoints.Count; i++)
            {
                sb.AppendLine("***** Ingress Point : " + i + " *****");
                sb.Append(IngressWaypoints[i].ToString());
            }
            sb.AppendLine("Number of Egress Waypoints: " + EgressWaypointCount);
            for (int i = 0; i < EgressWaypoints.Count; i++)
            {
                sb.AppendLine("***** Ingress Point : " + i + " *****");
                sb.Append(EgressWaypoints[i].ToString());
            }
            sb.AppendLine("***** Mission Request *****");
            sb.Append(missionRequest.ToString());


            return sb.ToString();
        }
        #endregion Functional Methods

        #region Constructors
        /// <summary>
        /// Default Package Constructor.
        /// </summary>
        protected Package()
            : base()
        {
        }
        /// <summary>
        /// Initializes a Package with the values supplied.
        /// </summary>
        /// <param name="stream">Stream with initialization data.</param>
        /// <param name="version">File Version.</param>
        public Package(Stream stream, int version)
            : base(stream, version)
        {
            Read(stream);
        }
        #endregion Constructors
    }
}
