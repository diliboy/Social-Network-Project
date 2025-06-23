using Microsoft.Data.SqlClient;
using System.Data;

namespace Social_Network_Project_BE.Models
{
    public class Dal
    {
        public Response Registartion(Registration registration, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("INSERT INTO Registration(Name,Email,Password,PhoneNo,IsActive,IsApproved)" +
                "VALUES('" + registration.Name + "','" + registration.Email + "','" + registration.Password + "','" + registration.PhoneNo + "',1,0)", connection);

            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();

            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Registration Successful";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Registration Failed";
            }

            return response;
        }

        public Response Login(Registration registration, SqlConnection connection)
        {
            Response response = new Response();

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Registration WHERE Email = '" + registration.Email + "' AND Password ='" + registration.Password + "' ", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);



            if (dt.Rows.Count > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Login Successful";

                Registration reg = new Registration();
                reg.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                reg.Name = Convert.ToString(dt.Rows[0]["Name"]);
                reg.Email = Convert.ToString(dt.Rows[0]["Email"]);

                response.Registration = reg;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Login Failed";

                response.Registration = null;
            }
            return response;
        }

        public Response UserApproval(Registration registration, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("UPDATE Registration SET IsApproved = 1 WHERE Id = '" + registration.Id + "' AND IsActive =1", connection);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "User Approved Successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "User Approval Failed";
            }
            return response;
        }

        public Response AddNews(News news, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("INSERT INTO News(Title,Content,Email,IsActive,CreatedOn)" +
                "VALUES('" + news.Title + "','" + news.Content + "','" + news.Email + "',1, GETDATE())", connection);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "News Created Successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "News Creation Failed";
            }
            return response;
        }

        public Response NewsList(SqlConnection connection)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM News WHERE IsActive = 1", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                response.listNews = new List<News>();
                foreach (DataRow row in dt.Rows)
                {
                    News news = new News
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Title = Convert.ToString(row["Title"]),
                        Content = Convert.ToString(row["Content"]),
                        Email = Convert.ToString(row["Email"]),
                        IsActive = Convert.ToInt32(row["IsActive"]),
                        CreatedOn = Convert.ToString(row["CreatedOn"])
                    };
                    response.listNews.Add(news);
                }
                response.StatusCode = 200;
                response.StatusMessage = "News Retrieved Successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No News Found";
                response.listNews = null;
            }
            return response;
        }

        public Response AddArticle(Article article, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("INSERT INTO Articel(Title,Content,Email,Image,IsActive,IsApproved)" +
                "VALUES('" + article.Title + "','" + article.Content + "','" + article.Email + "','" + article.Image + "',1, 0)", connection);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Article Created Successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Article Creation Failed";
            }
            return response;
        }

        public Response ArtcleList(Article article,SqlConnection connection)
        {
            Response response = new Response();
            SqlDataAdapter da = null;
            if (article.Type == "User")
            {
                new SqlDataAdapter("SELECT * FROM Article WHERE Email = '"+article.Email+"' AND IsActive = 1", connection);

            }
            if (article.Type == "Page")
            {
                new SqlDataAdapter("SELECT * FROM Article WHERE IsActive = 1", connection);
            }
            
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                response.listArticle = new List<Article>();
                foreach (DataRow row in dt.Rows)
                {
                    Article art = new Article
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Title = Convert.ToString(row["Title"]),
                        Content = Convert.ToString(row["Content"]),
                        Email = Convert.ToString(row["Email"]),
                        IsActive = Convert.ToInt32(row["IsActive"]),
                        Image = Convert.ToString(row["Image"])
                    };
                    response.listArticle.Add(art);
                }
                response.StatusCode = 200;
                response.StatusMessage = "Articles Retrieved Successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Articles Found";
                response.listNews = null;
            }
            return response;
        }
    }
}
