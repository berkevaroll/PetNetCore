using System;
using System.Collections.Generic;

namespace PetNetCore.Entity
{
    public partial class BlogPost
    {
        public int Id { get; set; }
        public string BlogTitle { get; set; }
        public string BlogContent { get; set; }
        public string Photo { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
