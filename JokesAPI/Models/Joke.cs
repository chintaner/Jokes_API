using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JokesAPI.Models
{
    public class Joke
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Category { get; set; }
      
    }
}
