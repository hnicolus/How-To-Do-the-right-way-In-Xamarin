using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Persistence
{
    public interface ISQLiteDb
    {
        SQLiteConnection GetConnection();

    }
}
