using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Infrastructure.Extensions
{
    /// <summary>
    /// String extension class to provide string helper methods.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Formats the with.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
    }
}
