// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 02-29-2020
// ***********************************************************************
// <copyright file="ScanningManager.cs" company="VisioForge">
//     Copyright (c) 2006-2021
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Common;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    using VisioForge.DirectShowLib;
    using VisioForge.DirectShowLib.BDA;

    /// <summary>
    /// Class ScanningManager.
    /// </summary>
    internal static class ScanningManager
    {
        /// <summary>
        /// The networks.
        /// </summary>
        private static List<Network> networks;

        /// <summary>
        /// Finds the network.
        /// </summary>
        /// <param name="tuningInfo">The tuning information.</param>
        /// <returns>Network.</returns>
        public static Network FindNetwork(TuningInfo tuningInfo)
        {
            if (networks == null)
            {
                networks = Network.LoadAll();
            }

            foreach (Network network in networks)
            {
                if (network.TuningInfo.Equals(tuningInfo))
                {
                    return network;
                }
            }

            return null;
        }

        /// <summary>
        /// Initialises this instance.
        /// </summary>
        public static void Initialise()
        {
        }

        /// <summary>
        /// Scans the PMT.
        /// </summary>
        /// <param name="mpegData">The MPEG data.</param>
        /// <param name="tuningInfo">The tuning information.</param>
        /// <param name="pmtPid">The PMT pid.</param>
        /// <returns>Service.</returns>
        public static Service ScanPmt(IMpeg2Data mpegData, TuningInfo tuningInfo, short pmtPid)
        {
            //Network network = FindNetwork(tuningInfo);

            //if (network != null)
            //{
            //    foreach (Service service in network.Services)
            //    {
            //        if (service.PmtPid == pmtPid)
            //        {
            //            //Log.WriteLine("Using cached tuning data", new object[0]);
            //            return service;
            //        }
            //    }
            //}

            //Log.WriteLine("Unable to find channel details.", new object[0]);
            if (mpegData != null)
            {
                //Log.WriteLine("Scanning PMT on {0}: {1}", new object[] { tuningInfo, pmtPid });
                pmtPid = 0; //-V3061
                List<MPEGTable> list = Utility.LoadTable(mpegData, pmtPid, TableType.PAT, 0xfa0);

                if (list.Count > 0)
                {
                    //DVBTTuningInfo info = tuningInfo as DVBTTuningInfo;
                    DVBCTuningInfo info = tuningInfo as DVBCTuningInfo;
                    if (info != null)
                    {
                        Service service2 = new Service(new Network(info), 0, pmtPid);

                        PMTTable table = list[0] as PMTTable;
                        if (table != null)
                        {
                            foreach (PMTTable.StreamDescription description in table.Streams)
                            {
                                service2.Streams.Add(new Stream(description));
                            }
                        }

                        return service2;
                    }
                }
            }

            return null;
        }
    }
}