// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DSFilterInitInfo.cs" company="VisioForge">
//   VisioForge (c) 2006 - 2021
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VisioForge.DirectShowAPI
{
    using System;

    /// <summary>
    /// Filter initialization information. 
    /// Contains CLSID, name and file name (optional).
    /// </summary>
    public class DSFilterInitInfo
    {
        /// <summary>
        /// Gets or sets the clsid.
        /// </summary>
        public Guid CLSID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the filename (x86).
        /// </summary>
        public string FilenameX86 { get; set; }

        /// <summary>
        /// Gets or sets the filename (x64).
        /// </summary>
        public string FilenameX64 { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DSFilterInitInfo"/> class. 
        /// </summary>
        /// <param name="clsid">
        /// CLSID.
        /// </param>
        /// <param name="name">
        /// Filter name.
        /// </param>
        /// <param name="filenameX86">
        /// File name (x86).
        /// </param>
        /// <param name="filenameX64">
        /// File name (x64).
        /// </param>
        public DSFilterInitInfo(string clsid, string name, string filenameX86, string filenameX64)
        {
            CLSID = new Guid(clsid);
            Name = name;
            FilenameX86 = filenameX86;
            FilenameX64 = filenameX64;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DSFilterInitInfo"/> class. 
        /// </summary>
        /// <param name="clsid">
        /// CLSID.
        /// </param>
        /// <param name="name">
        /// Filter name.
        /// </param>
        public DSFilterInitInfo(Guid clsid, string name)
        {
            CLSID = clsid;
            Name = name;
        }
    }
}
