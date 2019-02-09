using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LizstMVC.Models
{
        public class Score
        {
            public string Title { get; set; }
            public string Composer { get; set; }
            public string Genre { get; set; }
            public DateTime DateCheckedOut { get; set; }
            public DateTime DueDate { get; set; }
            public int NumberOfParts { get; set; }
            public int ID { get; set; }
        }


}
