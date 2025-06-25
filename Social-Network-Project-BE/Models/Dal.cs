using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Social_Network_Project_BE.Models
{
    public class Dal
    {
        public Response Registartion(Registration registration, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("INSERT INTO Registration(Name,Email,Password,PhoneNo,IsActive,IsApproved,UserType)" +
                "VALUES('" + registration.Name + "','" + registration.Email + "','" + registration.Password + "','" + registration.PhoneNo + "',1,0,'USER')", connection);

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

        //public Response Login(Registration registration, SqlConnection connection)
        //{
        //    Response response = new Response();

        //    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Registration WHERE Email = '" + registration.Email + "' AND Password ='" + registration.Password + "' AND IsApproved = 1", connection);
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);



        //    if (dt.Rows.Count > 0)
        //    {
        //        response.StatusCode = 200;
        //        response.StatusMessage = "Login Successful";

        //        Registration reg = new Registration();
        //        reg.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
        //        reg.Name = Convert.ToString(dt.Rows[0]["Name"]);
        //        reg.Email = Convert.ToString(dt.Rows[0]["Email"]);

        //        response.Registration = reg;
        //    }
        //    else
        //    {
        //        response.StatusCode = 100;
        //        response.StatusMessage = "Login Failed";

        //        response.Registration = null;
        //    }
        //    return response;
        //}

        public Response Login(Registration registration, SqlConnection connection)
        {
            Response response = new Response();

            // Step 1: Check if user exists with the given email and password
            string query = "SELECT * FROM Registration WHERE Email = @Email AND Password = @Password";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Email", registration.Email);
            cmd.Parameters.AddWithValue("@Password", registration.Password);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                // No user found
                response.StatusCode = 100;
                response.StatusMessage = "User not found or invalid credentials.";
                response.Registration = null;
            }
            else
            {
                DataRow row = dt.Rows[0];
                int isApproved = Convert.ToInt32(row["IsApproved"]);

                if (isApproved == 0)
                {
                    // User exists but not approved
                    response.StatusCode = 101;
                    response.StatusMessage = "Account is not approved yet. Please wait for admin approval.";
                    response.Registration = null;
                }
                else
                {
                    // Success
                    response.StatusCode = 200;
                    response.StatusMessage = "Login Successful";

                    Registration reg = new Registration
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Name = Convert.ToString(row["Name"]),
                        Email = Convert.ToString(row["Email"])
                    };

                    response.Registration = reg;
                }
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
            SqlCommand cmd = new SqlCommand("INSERT INTO Article(Title,Content,Email,Image,IsActive,IsApproved)" +
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
                da = new SqlDataAdapter("SELECT * FROM Article WHERE Email = '"+article.Email+"' AND IsActive = 1", connection);

            }
            if (article.Type == "Page")
            {
                da = new SqlDataAdapter("SELECT * FROM Article WHERE IsActive = 1", connection);
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
                        Image = Convert.ToString(row["Image"]),
                        IsApproved = Convert.ToInt32(row["IsApproved"]),
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
        public Response ArticleApproval(Article article, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("UPDATE Article SET IsApproved = 1 WHERE Id = '" + article.Id + "' AND IsActive =1", connection);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Article Approved Successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Article Approval Failed";
            }
            return response;
        }
        public Response StaffRegistartion(Registration staff, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("INSERT INTO Registration(Name,Email,Password,PhoneNo,IsActive,IsApproved,UserType)" +
                "VALUES('" + staff.Name + "','" + staff.Email + "','" + staff.Password + "','-',1,1,'"+staff.UserType+ "')", connection);

            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();

            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Staff Registration Successful";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Staff Registration Failed";
            }

            return response;
        }
        public Response DeleteStaff(Staff staff, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("DELETE FROM Staff WHERE Id = '" + staff.Id + "' AND IsActive = 1", connection);

            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();

            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Staff Deletion Successful";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Staff Deletion Failed";
            }

            return response;
        }
        public Response AddEvent(Events events, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("INSERT INTO Events(Title,Content,Email,IsActive,CreatedOn)" +
                "VALUES('" + events.Title + "','" + events.Content + "','" + events.Email + "',1, GETDATE())", connection);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Event Created Successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Event Creation Failed";
            }
            return response;
        }

        public Response EventList(SqlConnection connection)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Events WHERE IsActive = 1", connection); ;
 
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                response.listEvents = new List<Events>();
                foreach (DataRow row in dt.Rows)
                {
                    Events events = new Events
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Title = Convert.ToString(row["Title"]),
                        Content = Convert.ToString(row["Content"]),
                        Email = Convert.ToString(row["Email"]),
                        IsActive = Convert.ToInt32(row["IsActive"]),
                        CreatedOn = Convert.ToString(row["CreatedOn"])
                    };
                    response.listEvents.Add(events);
                }
                response.StatusCode = 200;
                response.StatusMessage = "Events Retrieved Successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Events Found";
                response.listNews = null;
            }
            return response;
        }

        public Response RegistrationList(Registration registration, SqlConnection connection)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Registration WHERE IsActive = 1 AND UserType = '"+registration.UserType+"'", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                response.listRegistration = new List<Registration>();
                foreach (DataRow row in dt.Rows)
                {
                    Registration reg = new Registration
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Name = Convert.ToString(row["Name"]),
                        Password = Convert.ToString(row["Password"]),
                        Email = Convert.ToString(row["Email"]),
                        PhoneNo = Convert.ToString(row["PhoneNo"]),
                        IsActive = Convert.ToInt32(row["IsActive"]),
                        IsApproved = Convert.ToInt32(row["IsApproved"]),
                        UserType = Convert.ToString(row["UserType"])
                    };
                    response.listRegistration.Add(reg);
                }
                response.StatusCode = 200;
                response.StatusMessage = "Registration List Retrieved Successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Registration List Found";
                response.listRegistration = null;
            }
            return response;
        }

    }
}

   
