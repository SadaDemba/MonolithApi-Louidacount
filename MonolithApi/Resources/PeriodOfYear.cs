using System.ComponentModel.DataAnnotations.Schema;

namespace MonolithApi.Resources
{
    [NotMapped]
    public class PeriodOfYear
    {
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        public PeriodOfYear() { }

        /// <summary>
        /// Check if the current date is included in this period
        /// </summary>
        /// <returns></returns>
        public bool IncludesToday()
        {
            DateTime date = DateTime.UtcNow;

            return (date.Month >= BeginDate.Month && date.Month <= EndDate.Month) &&
                (date.Day >= BeginDate.Day && date.Day <= EndDate.Day) &&
                (date.TimeOfDay >= BeginDate.TimeOfDay && date.TimeOfDay <= EndDate.TimeOfDay);
        }


    }
    

}
