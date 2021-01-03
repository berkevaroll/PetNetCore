using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetNetCore.Models
{
    public class BlogPostDto
    {
        public partial class List
        {
            public string BlogTitle { get; set; }
            public string BlogContent { get; set; }
            public string Photo { get; set; }
            public string Username { get; set; }

        }
    }
}
