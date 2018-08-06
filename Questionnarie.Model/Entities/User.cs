using Tasks.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Model
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Required(ErrorMessage ="Please enter your name")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Password could't be empty")]
        public string Password { get; set; }

        [Column("RoleString")]
        public virtual string RoleString {
            get {
                StringBuilder result = new StringBuilder();
                foreach (var item in Roles)
                {
                    result.Append(item.ToString()+";");
                }
                return result.ToString();
            }
            set {
                string[] roles = value.Split(';');

                foreach (var item in roles)
                {
                    UserRoleEnum newRole;
                    if (Enum.TryParse(item, out newRole))
                    {
                        if (Roles == null)
                        {
                            Roles = new List<UserRoleEnum>();
                        }

                        Roles.Add(newRole);
                    }
                }
            }
        }
        [NotMapped]
        public virtual List<UserRoleEnum> Roles { get; set; }

        [Column("Email")] 
        public string Email { get; set; }


        [ForeignKey("ToUserId")]
        public ICollection<WorkTask> ToTasks { get; set; }
        [ForeignKey("FromUserId")]
        public ICollection<WorkTask> FromTasks { get; set; }

        
    }
}
