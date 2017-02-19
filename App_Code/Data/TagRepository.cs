using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.Data;

public class TagRepository
{
    private static readonly string _connectionString = "DefaultConnection";
	public TagRepository()
	{
		
	}
    public static dynamic Get(int id)
    {
        using (var db = Database.Open(_connectionString))
        {
            var sql = "SELECT * FROM tbTags WHERE ID = @0";
            return db.QuerySingle(sql, id);
        }
    }
    public static dynamic Get(string friendlyname)
    {
        using (var db = Database.Open(_connectionString))
        {

            var sql = "SELECT * FROM tbTags WHERE UrlFriendlyName = @0";
            return db.QuerySingle(sql, friendlyname);
        }
    }

    public static IEnumerable<dynamic> GetAll(string orderBy = null,  string where = null)
    {
        using (var db = Database.Open(_connectionString))
        {
             var sql = "SELECT * FROM tbTags";

             if (!string.IsNullOrEmpty(where))
             {
                 sql += "WHERE" + where;
             }
            if (!string.IsNullOrEmpty(orderBy))
            {
                sql += " ORDER BY " + orderBy;
            }
            return db.Query(sql);
        }
    }
    public static void Add(string name, string friendlyName)
    {
        using (var db = Database.Open("DefaultConnection"))
        {
            var sql = "INSERT INTO tbTags (Name, UrlFriendlyName) VALUES(@0,@1)";

            db.Execute(sql, name, friendlyName);

        }
    }
    public static void Edit(int id, string name, string friendlyName)
    {
        using (var db = Database.Open("DefaultConnection"))
        {
            var sql = "UPDATE tbTags SET Name= @0, UrlFriendlyName = @1 WHERE ID = @2";

            db.Execute(sql, name, friendlyName, id);

        }
    }

    internal static void Remove(string friendlyName)
    {
        using (var db = Database.Open("DefaultConnection"))
        {
            var sql = "DELETE FROM tbTags WHERE UrlFriendlyName = @0";

            db.Execute(sql, friendlyName);

        }
    }

}