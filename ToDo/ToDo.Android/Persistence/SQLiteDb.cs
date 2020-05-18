using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using ToDo.Droid.Persistence;
using ToDo.Persistence;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDb))]
namespace ToDo.Droid.Persistence
{
    class SQLiteDb : ISQLiteDb
    {
        public SQLiteConnection GetConnection()
        {
            var documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentPath, "Tododb.db3");

            return new SQLiteConnection(path);
        }
    }
}