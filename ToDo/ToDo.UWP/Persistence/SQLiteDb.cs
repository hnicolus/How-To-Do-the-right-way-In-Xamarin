using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Persistence;

namespace ToDo.UWP.Persistence
{
    class SQLiteDb : ISQLiteDb
    {
        public SQLiteConnection GetConnection()
        {
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentPath, "Tododb.db3");

            return new SQLiteConnection(path);
        }
    }
}
