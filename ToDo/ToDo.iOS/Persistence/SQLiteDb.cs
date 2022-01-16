using SQLite;
using System;
using System.IO;
using ToDo.iOS.Persistence;
using ToDo.Persistence;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDb))]
namespace ToDo.iOS.Persistence
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentPath, "Tododb.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}
