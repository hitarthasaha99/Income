using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.SurveyLibrary
{
    /// <summary>
    /// SSS item with corresponding serial number which is the identification for like household
    /// </summary>
    public class SSSItem
    {
        /// <summary>
        /// Value for SSS
        /// </summary>
        public int SSSValue { get; set; }
        /// <summary>
        /// Serial number of the item
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// Whether selected
        /// </summary>
        public bool IsSelected { get; set; }

    }

    /// <summary>
    /// Selection item with corresponding serial number which is the identification for like household
    /// In this case it has some already selected by other schedule
    /// </summary>
    public class SItem
    {
        /// <summary>
        /// selected by other schedule, these are only considered when there is not enough household
        /// </summary>
        public bool OtherScheduleSelected { get; set; }
        /// <summary>
        /// Serial number of the item
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// Whether selected
        /// </summary>
        public bool IsSelected { get; set; }

    }
}
