// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 10-08-2021
// ***********************************************************************
// <copyright file="Service.cs" company="VisioForge">
//     Copyright (c) VisioForge. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Xml.Serialization;

    using VisioForge.DirectShowLib;

    /// <summary>
    /// Class Service.
    /// </summary>
    internal class Service
    {
        /// <summary>
        /// The channel number.
        /// </summary>
        private short channelNumber;

        /// <summary>
        /// The event i ds.
        /// </summary>
        private readonly List<short> eventIDs;

        /// <summary>
        /// The following show.
        /// </summary>
        [NonSerialized]
        private ShowEvent followingShow;

        /// <summary>
        /// The has now next.
        /// </summary>
        private bool? hasNowNext;

        /// <summary>
        /// The has schedule.
        /// </summary>
        private bool? hasSchedule;

        /// <summary>
        /// The identifier.
        /// </summary>
        private Guid? id;

        /// <summary>
        /// The name.
        /// </summary>
        private string name;

        /// <summary>
        /// The network.
        /// </summary>
        private Network network;

        /// <summary>
        /// The PCR pid.
        /// </summary>
        private short pcrPid;

        /// <summary>
        /// The PMT pid.
        /// </summary>
        private short pmtPid;

        /// <summary>
        /// The present show.
        /// </summary>
        [NonSerialized]
        private ShowEvent presentShow;

        /// <summary>
        /// The program number.
        /// </summary>
        private short programNumber;

        /// <summary>
        /// The streams.
        /// </summary>
        private readonly List<Stream> streams;

        /// <summary>
        /// The type.
        /// </summary>
        private ServiceType type;

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Service"/> class.
        /// </summary>
        public Service()
        {
            this.streams = new List<Stream>();
            this.eventIDs = new List<short>();
        }

        //internal Service(DbDataReader reader, Network network)
        //{
        //    this.streams = new List<Stream>();
        //    this.eventIDs = new List<short>();
        //    this.network = network;
        //    this.id = new Guid?(reader.GetGuid(0));
        //    this.name = reader.GetString(1);
        //    this.type = (ServiceType)Enum.Parse(typeof(ServiceType), reader.GetString(2));
        //    this.pmtPid = reader.GetInt16(3);
        //    this.pcrPid = reader.GetInt16(4);
        //    this.programNumber = reader.GetInt16(5);
        //    this.channelNumber = reader.GetInt16(6);
        //    if (!reader.IsDBNull(7))
        //    {
        //        this.hasSchedule = new bool?(Convert.ToBoolean(Convert.ToInt32(reader.GetValue(7))));
        //    }
        //    if (!reader.IsDBNull(8))
        //    {
        //        this.hasNowNext = new bool?(Convert.ToBoolean(Convert.ToInt32(reader.GetValue(8))));
        //    }
        //    DB db = new DB();
        //    using (LockedDbConnection connection = db.CreateConnection())
        //    {
        //        using (DbDataReader reader2 = db.ExecuteReader(connection, "SELECT ID, Type, PID, ExtraData FROM ServiceStreams WHERE ServiceID = ?", new object[] { this.id.ToString() }))
        //        {
        //            while (reader2.Read())
        //            {
        //                try
        //                {
        //                    this.streams.Add(new Stream(reader2));
        //                    continue;
        //                }
        //                catch (Exception)
        //                {
        //                    continue;
        //                }
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="Service"/> class.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="programNumber">The program number.</param>
        /// <param name="pmtPid">The PMT pid.</param>
        public Service(Network network, short programNumber, short pmtPid)
        {
            this.streams = new List<Stream>();
            this.eventIDs = new List<short>();
            this.network = network;
            this.programNumber = programNumber;
            this.pmtPid = pmtPid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Service"/> class.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="programNumber">The program number.</param>
        /// <param name="pmtPid">The PMT pid.</param>
        /// <param name="pmt">The PMT.</param>
        public Service(Network network, short programNumber, short pmtPid, PMTTable pmt)
        {
            this.streams = new List<Stream>();
            this.eventIDs = new List<short>();
            this.network = network;
            this.programNumber = programNumber;
            this.pmtPid = pmtPid;
            this.AddPMT(pmt);
        }

        /// <summary>
        /// Adds the event.
        /// </summary>
        /// <param name="e">The e.</param>
        public void AddEvent(EITTable.Event e)
        {
            switch (e.runningStatus)
            {
                case RunningStatus.Undefined:
                    if ((this.followingShow != null) && (this.followingShow.id == e.eventID))
                    {
                        break;
                    }

                    this.followingShow = new ShowEvent(e);
                    this.OnPropertyChange("FollowingShow");
                    return;

                case RunningStatus.Running:
                    if ((this.presentShow == null) || (this.presentShow.id != e.eventID))
                    {
                        this.presentShow = new ShowEvent(e);
                        this.OnPropertyChange("PresentShow");
                        return;
                    }

                    break;

                default:
                    //Log.WriteLine("Unexpected running status.", new object[0]);
                    break;
            }
        }

        /// <summary>
        /// Adds the PMT.
        /// </summary>
        /// <param name="pmt">The PMT.</param>
        public void AddPMT(PMTTable pmt)
        {
            this.pcrPid = pmt.PcrPid;
            foreach (PMTTable.StreamDescription description in pmt.Streams)
            {
                this.streams.Add(new Stream(description));
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            Service service = obj as Service;
            if (service == null)
            {
                return false;
            }

            return ((this.network.Frequency == service.network.Frequency) && (this.pmtPid == service.pmtPid));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return (this.network.Frequency ^ this.pmtPid);
        }

        /// <summary>
        /// Called when [property change].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void OnPropertyChange(string propertyName)
        {
            //Log.WriteLine("Set {0} for {1}", new object[] { propertyName, this.Name });
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            //new DB().ExecuteNonQuery("INSERT INTO Services (NetworkID, Name, Type, PmtPid, PcrPid, ProgramNumber, ChannelNumber, HasSchedule, HasNowNext, ID) SELECT ?, ?, ?, ?, ?, ?, ?, ?, ?, ?", new object[] { this.Network.ID.ToString(), this.Name, this.Type.ToString(), this.PmtPid, this.PcrPid, this.ProgramNumber, this.ChannelNumber, this.HasSchedule, this.HasNowNext, this.ID.ToString() });
            foreach (Stream stream in this.Streams)
            {
                stream.Save(this);
            }
        }

        /// <summary>
        /// Scans the specified MPEG data.
        /// </summary>
        /// <param name="mpegData">The MPEG data.</param>
        public void Scan(IMpeg2Data mpegData)
        {
            foreach (var table in Utility.LoadTable(mpegData, this.pmtPid, TableType.PMT, 0x1f40))
            {
                if (table is PMTTable pmt)
                {
                    AddPMT(pmt);
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0} ({1})", this.name, this.Network.NetworkName);
        }

        /// <summary>
        /// Gets a pid.
        /// </summary>
        /// <value>a pid.</value>
        public int? APid
        {
            get
            {
                foreach (Stream stream in this.streams)
                {
                    if (stream.IsAudio && !stream.IsAC3)
                    {
                        return new int?(stream.PID);
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the channel number.
        /// </summary>
        /// <value>The channel number.</value>
        public short ChannelNumber
        {
            get
            {
                return this.channelNumber;
            }

            set
            {
                this.channelNumber = value;
            }
        }

        /// <summary>
        /// Gets the following show.
        /// </summary>
        /// <value>The following show.</value>
        public ShowEvent FollowingShow
        {
            get
            {
                return this.followingShow;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has now next.
        /// </summary>
        /// <value><c>null</c> if [has now next] contains no value, <c>true</c> if [has now next]; otherwise, <c>false</c>.</value>
        public bool? HasNowNext
        {
            get
            {
                return this.hasNowNext;
            }

            set
            {
                this.hasNowNext = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has schedule.
        /// </summary>
        /// <value><c>null</c> if [has schedule] contains no value, <c>true</c> if [has schedule]; otherwise, <c>false</c>.</value>
        public bool? HasSchedule
        {
            get
            {
                return this.hasSchedule;
            }

            set
            {
                this.hasSchedule = value;
            }
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid ID
        {
            get
            {
                if (!this.id.HasValue)
                {
                    this.id = new Guid?(Guid.NewGuid());
                }

                return this.id.Value;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the network.
        /// </summary>
        /// <value>The network.</value>
        [XmlIgnore]
        public Network Network
        {
            get
            {
                return this.network;
            }

            set
            {
                this.network = value;
            }
        }

        /// <summary>
        /// Gets or sets the PCR pid.
        /// </summary>
        /// <value>The PCR pid.</value>
        public short PcrPid
        {
            get
            {
                return this.pcrPid;
            }

            set
            {
                this.pcrPid = value;
            }
        }

        /// <summary>
        /// Gets or sets the PMT pid.
        /// </summary>
        /// <value>The PMT pid.</value>
        public short PmtPid
        {
            get
            {
                return this.pmtPid;
            }

            set
            {
                this.pmtPid = value;
            }
        }

        /// <summary>
        /// Gets the present show.
        /// </summary>
        /// <value>The present show.</value>
        public ShowEvent PresentShow
        {
            get
            {
                return this.presentShow;
            }
        }

        /// <summary>
        /// Gets or sets the program number.
        /// </summary>
        /// <value>The program number.</value>
        public short ProgramNumber
        {
            get
            {
                return this.programNumber;
            }

            set
            {
                this.programNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the raw.
        /// </summary>
        /// <value>The type of the raw.</value>
        [XmlElement("Type")]
        public int RawType
        {
            get
            {
                return (int)this.type;
            }

            set
            {
                this.type = (ServiceType)value;
            }
        }

        /// <summary>
        /// Gets the streams.
        /// </summary>
        /// <value>The streams.</value>
        public List<Stream> Streams
        {
            get
            {
                return this.streams;
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [XmlIgnore]
        public ServiceType Type
        {
            get
            {
                return this.type;
            }

            set
            {
                this.type = value;
            }
        }

        /// <summary>
        /// Gets the v pid.
        /// </summary>
        /// <value>The v pid.</value>
        public int? VPid
        {
            get
            {
                foreach (Stream stream in this.streams)
                {
                    if (stream.IsVideo)
                    {
                        return new int?(stream.PID);
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Class ShowEvent.
        /// </summary>
        internal class ShowEvent
        {
#pragma warning disable S1104 // Fields should not have public accessibility

            /// <summary>
            /// The description.
            /// </summary>
            public string Description;

            /// <summary>
            /// The duration.
            /// </summary>
            public TimeSpan Duration;

            /// <summary>
            /// The identifier.
            /// </summary>
            internal short id;

            /// <summary>
            /// The name.
            /// </summary>
            public string Name;

            /// <summary>
            /// The start time.
            /// </summary>
            public DateTime StartTime;

            /// <summary>
            /// Initializes a new instance of the <see cref="ShowEvent"/> class.
            /// </summary>
            /// <param name="e">The e.</param>
            internal ShowEvent(EITTable.Event e)
            {
                this.id = e.eventID;
                this.Name = e.name;
                this.StartTime = e.startTime;
                this.Duration = e.duration;
            }
        }

#pragma warning restore S1104 // Fields should not have public accessibility
    }
}