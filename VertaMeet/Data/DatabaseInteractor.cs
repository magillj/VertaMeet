using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.Common;
using System.Data.SqlClient;
using VertaMeet.Models;

namespace VertaMeet.Data
{
    public static class DatabaseInteractor
    {
        private static string sqlConnectionStr = new EntityConnectionStringBuilder(ConfigurationManager.ConnectionStrings["VertaMeetDataEntities"].ConnectionString).ProviderConnectionString;

        #region Get

        /// <summary>
        /// Gets data about the user with userId
        /// </summary>
        /// <returns>Null if no user by that id, otherwise the user with that id</returns>
        public static UserModel GetUserById(int userId)
        {
            UserModel outputUser = null;

            using (var conn = new SqlConnection(sqlConnectionStr))
            using (var command = new SqlCommand("GetUserById", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.Parameters.Add(new SqlParameter("@userId", userId));
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        outputUser = GetUserModelFromReader(rdr);
                    }
                }
                conn.Close();
            }

            return outputUser;
        }

        /// <summary>
        /// Returns a list of all the users in the database
        /// </summary>
        /// <returns>All the users in the database</returns>
        public static List<UserModel> GetAllUsers()
        {
            List<UserModel> users = new List<UserModel>();

            using (var conn = new SqlConnection(sqlConnectionStr))
            using (var command = new SqlCommand("GetAllUsers", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.ExecuteNonQuery();

                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        UserModel user = GetUserModelFromReader(rdr);
                        users.Add(user);
                    }
                }

                conn.Close();
            }

            return users;
        }

        #endregion

        #region Modify

        public static DatabaseInteractionResponse CreateUser(UserModel userModel)
        {
            // Check if userId is currently being used
            if (GetUserById(userModel.Id) != null)
            {
                return new DatabaseInteractionResponse() { Success = false, Message = "There already exists a user with the id: " + userModel.Id };
            }

            using (var conn = new SqlConnection(sqlConnectionStr))
            using (var command = new SqlCommand("CreateUser", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.Parameters.Add(new SqlParameter("@userId", userModel.Id));
                command.Parameters.Add(new SqlParameter("@name", userModel.Name));
                command.Parameters.Add(new SqlParameter("@email", userModel.Email));
                command.Parameters.Add(new SqlParameter("@location", userModel.Location.ToString()));
                command.ExecuteNonQuery();
                conn.Close();
            }

            return new DatabaseInteractionResponse() { Success = true };
        }

        public static DatabaseInteractionResponse DeleteUser(int userId)
        {
            using (var conn = new SqlConnection(sqlConnectionStr))
            using (var command = new SqlCommand("DeleteUser", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.Parameters.Add(new SqlParameter("@userId", userId));
                command.ExecuteNonQuery();
                conn.Close();
            }

            return new DatabaseInteractionResponse() { Success = true };
        }



        private static UserModel GetUserModelFromReader(SqlDataReader rdr)
        {
            return new UserModel()
            {
                Id = (int)rdr["UserId"],
                Name = rdr["Name"].ToString(),
                Email = rdr["Email"].ToString(),
                Location = (UserModel.LOCATION)Enum.Parse(typeof(UserModel.LOCATION), rdr["Location"].ToString())
            };
        }

        #endregion
    }
}