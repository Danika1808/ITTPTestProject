using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? RevokedOn { get; set; }
        public string RevokedBy { get; set; }

    }
}
