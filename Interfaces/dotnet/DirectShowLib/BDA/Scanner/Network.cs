// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 10-08-2021
// ***********************************************************************
// <copyright file="Network.cs" company="VisioForge">
//     Copyright (c) 2006-2021
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    using VisioForge.DirectShowLib;
    using VisioForge.DirectShowLib.BDA;

    /// <summary>
    /// Class Network.
    /// Implements the <see cref="System.Xml.Serialization.IXmlSerializable" />.
    /// </summary>
    /// <seealso cref="System.Xml.Serialization.IXmlSerializable" />
    internal class Network : IXmlSerializable
    {
        /// <summary>
        /// The bandwidth.
        /// </summary>
        private int bandwidth;

        /// <summary>
        /// The center frequency.
        /// </summary>
        private int? centerFrequency;

        /// <summary>
        /// The frequency.
        /// </summary>
        private int frequency;

        /// <summary>
        /// The identifier.
        /// </summary>
        private Guid? id;

        /// <summary>
        /// The network name.
        /// </summary>
        private string networkName;

        /// <summary>
        /// The services.
        /// </summary>
        private Dictionary<short, Service> services;

        /// <summary>
        /// The symbolrate.
        /// </summary>
        private int symbolrate;

        /// <summary>
        /// The modulation type.
        /// </summary>
        private ModulationType modulationType;

        /// <summary>
        /// Initializes a new instance of the <see cref="Network"/> class.
        /// </summary>
        public Network()
        {
            this.bandwidth = 7;
            this.services = new Dictionary<short, Service>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Network"/> class.
        /// </summary>
        /// <param name="tuningInfo">The tuning information.</param>
        public Network(DVBTTuningInfo tuningInfo)
        {
            this.services = new Dictionary<short, Service>();
            this.frequency = tuningInfo.Frequency;
            this.bandwidth = tuningInfo.Bandwidth;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Network"/> class.
        /// </summary>
        /// <param name="tuningInfo">The tuning information.</param>
        public Network(DVBCTuningInfo tuningInfo)
        {
            this.bandwidth = 7;
            this.services = new Dictionary<short, Service>();
            this.frequency = tuningInfo.Frequency;
            this.modulationType = tuningInfo.ModulationType;
            this.symbolrate = tuningInfo.SymbolRate;
        }

        //private Network(DbDataReader reader)
        //{
        //    this.bandwidth = 7;
        //    this.services = new Dictionary<short, Service>();
        //    this.id = new Guid?(reader.GetGuid(0));
        //    this.networkName = reader.GetString(1);
        //    this.frequency = reader.GetInt32(2);
        //    this.bandwidth = reader.GetInt32(3);
        //    DB db = new DB();
        //    using (LockedDbConnection connection = db.CreateConnection())
        //    {
        //        using (DbDataReader reader2 = db.ExecuteReader(connection, "SELECT ID, Name, Type, PmtPid, PcrPid, ProgramNumber, ChannelNumber, HasSchedule, HasNowNext FROM Services WHERE NetworkID = ?", new object[] { this.id.ToString() }))
        //        {
        //            while (reader2.Read())
        //            {
        //                try
        //                {
        //                    Service service = new Service(reader2, this);
        //                    this.services.Add(service.ProgramNumber, service);
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
        /// Initializes a new instance of the <see cref="Network"/> class.
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        /// <param name="bandwidth">The bandwidth.</param>
        public Network(int frequency, int bandwidth)
        {
            this.services = new Dictionary<short, Service>();
            this.frequency = frequency;
            this.bandwidth = bandwidth;
        }

        /// <summary>
        /// Adds the nit.
        /// </summary>
        /// <param name="nit">The nit.</param>
        public void AddNIT(NITTable nit)
        {
            this.networkName = nit.NetworkName;
            nit.SelectStream(this.frequency);
            this.centerFrequency = nit.CenterFrequency;
            this.frequency = centerFrequency.HasValue ? centerFrequency.GetValueOrDefault() : this.frequency;
        }

        /// <summary>
        /// Adds the PMT.
        /// </summary>
        /// <param name="pmtPid">The PMT pid.</param>
        /// <param name="pmt">The PMT.</param>
        /// <param name="nit">The nit.</param>
        public void AddPMT(short pmtPid, PMTTable pmt, NITTable nit)
        {
            Service service = new Service(this, pmt.ProgramNumber, pmtPid, pmt);
            this.AddService(service);
            foreach (NITTableStreamDescriptor descriptor in nit.Streams)
            {
                short num;
                if (descriptor.channelNumbers.TryGetValue(service.ProgramNumber, out num))
                {
                    service.ChannelNumber = num;
                }
            }
        }

        /// <summary>
        /// Adds the SDT.
        /// </summary>
        /// <param name="sdt">The SDT.</param>
        public void AddSDT(SDTTable sdt)
        {
            foreach (short num in sdt.Services.Keys)
            {
                Service service;
                if (this.services.TryGetValue(num, out service))
                {
                    SDTTable.ServiceDescription description = sdt.Services[num];
                    service.Name = description.Name;
                    service.Type = description.Type;
                    service.HasSchedule = new bool?(description.HasSchedule);
                    service.HasNowNext = new bool?(description.HasPresentFollowing);
                }
            }
        }

        /// <summary>
        /// Adds the service.
        /// </summary>
        /// <param name="service">The service.</param>
        public void AddService(Service service)
        {
            this.services.Add(service.ProgramNumber, service);
        }

        //public void DeleteFromDB()
        //{
        //    DB db = new DB();
        //    db.ExecuteNonQuery("DELETE FROM ServiceStreams WHERE ServiceID IN (SELECT ID FROM Services WHERE NetworkID = ?)", new object[] { this.ID.ToString() });
        //    db.ExecuteNonQuery("DELETE FROM Services WHERE NetworkID = ?", new object[] { this.ID.ToString() });
        //    db.ExecuteNonQuery("DELETE FROM Networks WHERE ID = ?", new object[] { this.ID.ToString() });
        //}

        /// <summary>
        /// Gets the now next.
        /// </summary>
        /// <param name="mpegData">The MPEG data.</param>
        public void GetNowNext(IMpeg2Data mpegData)
        {
            foreach (var table in Utility.LoadTable(mpegData, 0x12, TableType.EITNowNextActual, 0x1f40))
            {
                if (table is EITTable eitTable)
                {
                    Service service;
                    if (this.services.TryGetValue(eitTable.serviceID, out service))
                    {
                        foreach (EITTable.Event event2 in eitTable.events)
                        {
                            service.AddEvent(event2);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method is reserved and should not be used. When implementing the <see langword="IXmlSerializable" /> interface, you should return <see langword="null" /> (<see langword="Nothing" /> in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute" /> to the class.
        /// </summary>
        /// <returns>An <see cref="T:System.Xml.Schema.XmlSchema" /> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" /> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)" /> method.</returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="programID">The program identifier.</param>
        /// <returns>Service.</returns>
        public Service GetService(short programID)
        {
            Service service;
            if (this.services.TryGetValue(programID, out service))
            {
                return service;
            }

            return null;
        }

        /// <summary>
        /// Loads all.
        /// </summary>
        /// <returns>List&lt;Network&gt;.</returns>
        public static List<Network> LoadAll()
        {
            //DB db = new DB();
            List<Network> list = new List<Network>();

            //using (LockedDbConnection connection = db.CreateConnection())
            //{
            //    using (DbDataReader reader = db.ExecuteReader(connection, "SELECT ID, [Name], Frequency, Bandwidth FROM Networks", new object[0]))
            //    {
            //        while (reader.Read())
            //        {
            //            try
            //            {
            //                list.Add(new Network(reader));
            //                continue;
            //            }
            //            catch
            //            {
            //                continue;
            //            }
            //        }
            //    }
            //}
            return list;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Service));
            if (reader.Read())
            {
                while (((reader.NodeType != XmlNodeType.EndElement) || (reader.Name != "Network")) && (reader.NodeType != XmlNodeType.None))
                {
                    string name = reader.Name;
                    if (name == null)
                    {
                        goto Label_0101;
                    }

                    if (name != "name")
                    {
                        if (name == "frequency")
                        {
                            goto Label_0083;
                        }

                        if (name == "bandwidth")
                        {
                            goto Label_00A2;
                        }

                        if (name == "services")
                        {
                            goto Label_00C1;
                        }

                        goto Label_0101;
                    }

                    reader.ReadStartElement();
                    this.networkName = reader.ReadString();
                    reader.ReadEndElement();
                    continue;
                Label_0083:
                    reader.ReadStartElement();
                    this.frequency = Convert.ToInt32(reader.ReadString());
                    reader.ReadEndElement();
                    continue;
                Label_00A2:
                    reader.ReadStartElement();
                    this.bandwidth = Convert.ToByte(reader.ReadString());
                    reader.ReadEndElement();
                    continue;
                Label_00C1:
                    reader.ReadStartElement();
                    while (reader.NodeType != XmlNodeType.EndElement)
                    {
                        Service service = (Service)serializer.Deserialize(reader);
                        service.Network = this;
                        this.services.Add(service.ProgramNumber, service);
                    }

                    reader.ReadEndElement();
                    continue;
                Label_0101:
                    reader.Read();
                }
            }
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            //new DB().ExecuteNonQuery("INSERT INTO Networks ([Name], Frequency, Bandwidth, ID) SELECT ?, ?, ?, ?", new object[] { this.NetworkName, this.Frequency, this.Bandwidth, this.ID.ToString() });
            foreach (Service service in this.Services)
            {
                service.Save();
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Service));
            writer.WriteElementString("name", this.networkName);
            writer.WriteElementString("frequency", this.frequency.ToString());
            writer.WriteElementString("bandwidth", this.bandwidth.ToString());
            writer.WriteStartElement("services");
            foreach (Service service in this.services.Values)
            {
                serializer.Serialize(writer, service);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Gets or sets the bandwidth.
        /// </summary>
        /// <value>The bandwidth.</value>
        public int Bandwidth
        {
            get
            {
                return this.bandwidth;
            }

            set
            {
                this.bandwidth = value;
            }
        }

        /// <summary>
        /// Gets or sets the center frequency.
        /// </summary>
        /// <value>The center frequency.</value>
        public int? CenterFrequency
        {
            get
            {
                return this.centerFrequency;
            }

            set
            {
                this.centerFrequency = value;
            }
        }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>The frequency.</value>
        public int Frequency
        {
            get
            {
                return this.frequency;
            }

            set
            {
                this.frequency = value;
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
        /// Gets or sets the name of the network.
        /// </summary>
        /// <value>The name of the network.</value>
        public string NetworkName
        {
            get
            {
                return this.networkName;
            }

            set
            {
                this.networkName = value;
            }
        }

        /// <summary>
        /// Gets or sets the services.
        /// </summary>
        /// <value>The services.</value>
        public ICollection<Service> Services
        {
            get
            {
                return this.services.Values;
            }

            set
            {
                if (this.services == null)
                {
                    this.services = new Dictionary<short, Service>();
                }
                else
                {
                    this.services.Clear();
                }

                foreach (Service service in value)
                {
                    this.services.Add(service.ProgramNumber, service);
                }
            }
        }

        /// <summary>
        /// Gets the tuning information.
        /// </summary>
        /// <value>The tuning information.</value>
        public TuningInfo TuningInfo
        {
            get
            {
                return new DVBTTuningInfo(this.frequency, this.bandwidth);
            }
        }
    }
}