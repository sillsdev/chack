﻿using System;
using System.Xml.Linq;

namespace Chorus.Utilities
{
    public static class XElementExtensions
    {
        #region GetAttributeValue
        /// <summary>
        /// Gets the value of an attribute
        /// </summary>
        /// <param name="xEl">Extends this XElement Type</param>
        /// <param name="attName">An XName that contains the name of the attribute to retrieve.</param>
        /// <param name="defaultReturn">Default return if the attribute doesn't exist</param>
        /// <returns>Attribute value or default if attribute doesn't exist</returns>
        public static string GetAttributeValue(this XElement xEl, XName attName, string defaultReturn)
        {
            XAttribute att = xEl.Attribute(attName);
            if (att == null) return defaultReturn;
            return att.Value;
        }

        /// <summary>
        /// Gets the value of an attribute
        /// </summary>
        /// <param name="xEl">Extends this XElement Type</param>
        /// <param name="attName">An XName that contains the name of the attribute to retrieve.</param>
        /// <returns>Attribute value or String.Empty if element doesn't exist</returns>
        public static string GetAttributeValue(this XElement xEl, XName attName)
        {
            return xEl.GetAttributeValue(attName, String.Empty);
        }

        /// <summary>
        /// Gets the value of an attribute
        /// </summary>
        /// <param name="xEl">Extends this XElement Type</param>
        /// <param name="attName">An XName that contains the name of the attribute to retrieve.</param>
        /// <param name="defaultReturn">Default return if the attribute doesn't exist</param>
        /// <returns>Attribute value or default if attribute doesn't exist</returns>
        public static T GetAttributeValue<T>(this XElement xEl, XName attName, T defaultReturn)
        {
            string returnValue = xEl.GetAttributeValue(attName, String.Empty);
            if (returnValue == String.Empty) return defaultReturn;
            return (T)Convert.ChangeType(returnValue, typeof(T));
        }

        /// <summary>
        /// Gets the value of an attribute
        /// </summary>
        /// <param name="xEl">Extends this XElement Type</param>
        /// <param name="attName">An XName that contains the name of the attribute to retrieve.</param>
        /// <returns>Attribute value or default of T if element doesn't exist</returns>
        public static T GetAttributeValue<T>(this XElement xEl, XName attName)
        {
            return xEl.GetAttributeValue<T>(attName, default(T));
        }
        #endregion

    }

    public static class ObjectExtensions
    {
        public static string OrDefault(this object s, string defaultIfNullOrMissing)
        {
            if (s == null)
                return defaultIfNullOrMissing;
            return ((string)s) == string.Empty ? defaultIfNullOrMissing : (string)s;
        }
    }
}
