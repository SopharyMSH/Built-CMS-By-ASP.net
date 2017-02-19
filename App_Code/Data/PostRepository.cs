using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.Data;
using System.Dynamic;
/// <summary>
/// Summary description for PostRepository
/// </summary>
public class PostRepository
{
    private static readonly string _connectionString = "DefaultConnection";
	public PostRepository()
	{
		
	}
    public static dynamic Get(int id)
    {
        using (var db = Database.Open(_connectionString))
        {
            var sql = "SELECT * FROM tbPosts WHERE postID = @0";
            return db.QuerySingle(sql, id);
        }
    }
    public static dynamic Get(string slug)
    {
        using (var db = Database.Open(_connectionString))
        {
            
            var sql = "SELECT * FROM tbPosts WHERE Slug = @0";
            return db.QuerySingle(sql, slug);
        }
    }

    public static IEnumerable<dynamic> GetAll(string orderBy = null)
    {
        var posts = new List<dynamic>();
        using (var db = Database.Open(_connectionString))
        {
             var sql = "SELECT p.*, t.ID as TagId, t.Name as TagName," +
                        "t.UrlFriendlyName as UrlFriendlyName  FROM tbPosts p" +
                        "LEFT JOIN tbPostTagMap m ON p.postID = m.PostId" +
                        "LEFT JOIN tbTags t ON t.ID = m.TagId";
            if (!string.IsNullOrEmpty(orderBy))
            {
                sql += " ORDER BY " + orderBy;
            }

            var results = db.Query(sql);

            foreach (var result in results)
            {
                dynamic post = posts.SingleOrDefault(p => p.Id = result.Id);

                if (post == null)
                {
                    post = result;
                    post.Tags = new List<dynamic>();

                    posts.Add(post);
                }

                if (result.TagId == null)
                {
                    continue;
                }

                dynamic tag = new ExpandoObject();

                tag.Id = result.TagId;
                tag.Name = result.TagName;
                tag.UrlFriendlyName = result.TagUrlFriendlyName;

                post.Tags.Add(tag);

            }
        }
        return posts.ToArray();
    }
    public static void Add(string title, string content,string slug,DateTime? datePublished, int authorID)
    {
        using (var db = Database.Open("DefaultConnection"))
        {
            var sql = "INSERT INTO tbPosts (postTitle, Content, DateCreated, DatePublished, AuthorID, Slug) VALUES(@0,@1,@2,@3,@4,@5)";

            db.Execute(sql, title, content, DateTime.Now.ToShortDateString(), datePublished, authorID, slug);

        }
    }
    public static void Edit(int id, string title, string content,string slug,DateTime? datePublished, int authorID)
    {
        using (var db = Database.Open("DefaultConnection"))
        {
            var sql = "UPDATE tbPosts SET postTitle= @0, Content = @1, DatePublished = @2, AuthorID = @3, Slug = @4 WHERE postID = @5";

            db.Execute(sql, title, content,datePublished, authorID, slug,id);

        }
    }

    internal static void Remove(string slug)
    {
        using (var db = Database.Open("DefaultConnection"))
        {
            var sql = "DELETE FROM tbPosts WHERE Slug = @0";

            db.Execute(sql,slug);

        }
    }
}