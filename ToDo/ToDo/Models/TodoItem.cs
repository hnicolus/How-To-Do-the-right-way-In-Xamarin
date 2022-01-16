using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Models
{
    public class TodoItem
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool Completed { get; set; }
        public DateTime Due { get; set; } = DateTime.Now.AddDays(1);
        public string Description { get; set; } = string.Empty;
            
    }
}
