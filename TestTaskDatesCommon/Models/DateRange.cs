using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestTaskDatesCommon.Models
{
    public class DateRange
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        public override string ToString()
        {
            string result = String.Format("ID: {0} Start: '{1}' End: '{2}'", ID, Start.ToShortDateString(), End.ToShortDateString());
            return result;
        }
    }
}
