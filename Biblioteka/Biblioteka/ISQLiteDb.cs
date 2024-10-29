using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteka
{
    public interface ISQLiteDb
    {
        SQLite.SQLiteConnection GetConnection();
    }
}
