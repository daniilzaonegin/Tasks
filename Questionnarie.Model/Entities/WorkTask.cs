using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Model.Entities
{
    [Table("Tasks")]
    public class WorkTask
    {
        [Key]
        [Column(name:"Id")]
        public int TaskId { get; set; }
        [Display(Name ="To")]

        [ForeignKey("ToUserId")]
        //not virtual to disable lazy loading feature of EF
        public virtual User ToUser { get; set; }
        public string ToUserId { get; set; }

        [Display(Name ="From")]
        [ForeignKey("FromUserId")]
        //not virtual to disable lazy loading feature of EF
        public User FromUser { get; set; }
        public string FromUserId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime When { get; set; }

        [Required]
        public string Summary { get; set; }
        public string Subject { get; set; }


        public string CompleteMsg { get; set; }
        public bool Completed { get; set; }
    }
}
