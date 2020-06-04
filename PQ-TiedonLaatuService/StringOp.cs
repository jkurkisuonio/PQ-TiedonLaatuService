using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PQ_TiedonLaatuService
{
    public static class StringOp
    {


        /// <summary>
        /// Replaces the 5 predefined entities in XML with the appropriate name /// escape character.
        /// Source: https://www.wipfli.com/insights/blogs/connect-microsoft-dynamics-365-blog/c-regex-multiple-replacements
        /// </summary>
        /// <param name="source">string to search</param>
        /// <returns>source string with replaced value or original string</returns>
        public static string ReplaceXmlEntity(string source, Dictionary<string, string> replacements)
        {
            // NullGuards
            if (string.IsNullOrEmpty(source)) return source;
            if (replacements == null) return source;

            // The XML specification defines five "predefined entities" representing
            // special characters, and requires that all XML processors honor them. 
            //var xmlEntityReplacements = new Dictionary<string, string> {{ "&", "&amp;" }, { "'", "&apos;" },  { "<", "&lt;" },  { ">", "&gt;" },  { """, "&quot;" }};



            // Create an array and populate from the dictionary keys, then convert
            // the array to a pipe-delimited string to serve as the regex search
            // values and replace
            return Regex.Replace(source, string.Join("|", replacements.Keys.Select(k => k.ToString()).ToArray()), m => replacements[m.Value]);
        }
    }
}

