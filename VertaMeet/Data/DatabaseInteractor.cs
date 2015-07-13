﻿using System;
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
        private static readonly string SqlConnectionStr = new EntityConnectionStringBuilder(ConfigurationManager.ConnectionStrings["VertaMeetDataEntities"].ConnectionString).ProviderConnectionString;

        #region Get

        /// <summary>
        /// Gets data about the event with eventId
        /// </summary>
        /// <returns>Null if no event by that id, otherwise the event with that id</returns>
        public static EventModel GetEventById(int eventId)
        {
            Func<SqlDataReader, EventModel> readEvent = delegate(SqlDataReader rdr)
            {
                return rdr.Read() ? GetEventModelFromReader(rdr) : null;
            };

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@eventId", eventId) };

            return (EventModel)ExecuteSqlReader("GetEventById", readEvent, parameters);
        }

        /// <summary>
        /// Gets data about the user with userId
        /// </summary>
        /// <returns>Null if no user by that id, otherwise the user with that id</returns>
        public static InterestGroupModel GetInterestGroupById(int interestGroupId)
        {
            Func<SqlDataReader, InterestGroupModel> readInterestGroup = delegate(SqlDataReader rdr)
            {
                return rdr.Read() ? GetInterestGroupFromReader(rdr) : null;
            };

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@interestGroupId", interestGroupId) };

            return (InterestGroupModel)ExecuteSqlReader("GetInterestGroupById", readInterestGroup, parameters);
        }

        /// <summary>
        /// Gets data about the user with userId
        /// </summary>
        /// <returns>Null if no user by that id, otherwise the user with that id</returns>
        public static UserModel GetUserById(int userId)
        {
            Func<SqlDataReader, UserModel> readUser = delegate(SqlDataReader rdr)
            {
                return rdr.Read() ? GetUserModelFromReader(rdr) : null;
            };

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@userId", userId) };

            return (UserModel)ExecuteSqlReader("GetUserById", readUser, parameters);
        }

        /// <summary>
        /// Returns a list of all the users in the database
        /// </summary>
        /// <returns>All the users in the database</returns>
        public static List<UserModel> GetAllUsers()
        {
            Func<SqlDataReader, List<UserModel>> readUsers = delegate(SqlDataReader rdr)
            {
                List<UserModel> users = new List<UserModel>();

                while (rdr.Read())
                {
                    UserModel user = GetUserModelFromReader(rdr);
                    users.Add(user);
                }

                return users;
            };

            return (List<UserModel>)ExecuteSqlReader("GetAllUsers", readUsers);
        }

        #endregion

        #region Modify

        public static DatabaseInteractionResponse CreateEvent(EventModel eventModel)
        {
            // Check if eventId is currently being used
            if (GetEventById(eventModel.Id) != null)
            {
                return new DatabaseInteractionResponse() { Success = false, Message = "There already exists an event with the id: " + eventModel.Id };
            }
            
            if (eventModel.Attendees.Count > 0)
            {
                throw new NotImplementedException();
            }

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@eventId", eventModel.Id),
                new SqlParameter("@name", eventModel.Name),
                new SqlParameter("@description", eventModel.Description),
                new SqlParameter("@time", eventModel.Time),
                new SqlParameter("@imageUrl", eventModel.ImageUrl),
                new SqlParameter("@location", eventModel.Location),
                new SqlParameter("@interestGroupId", eventModel.InterestGroup.Id)
            };

            return ExecuteSqlNonQuery("CreateUser", parameters);
        }

        public static DatabaseInteractionResponse CreateInterestGroup(InterestGroupModel interestGroupModel)
        {
            // Check if userId is currently being used
            if (GetInterestGroupById(interestGroupModel.Id) != null)
            {
                return new DatabaseInteractionResponse() { Success = false, Message = "There already exists an interest group with the id: " + interestGroupModel.Id };
            }

            if (interestGroupModel.Members.Count > 0)
            {
                throw new NotImplementedException();
            }

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@interestGroupId", interestGroupModel.Id), 
                new SqlParameter("@name", interestGroupModel.Name), 
                new SqlParameter("@description", interestGroupModel.Description), 
                new SqlParameter("@managerId", interestGroupModel.Manager.Id), 
                new SqlParameter("@imageUrl", interestGroupModel.ImageUrl)
            };

            return ExecuteSqlNonQuery("CreateUser", parameters);
        }


        public static DatabaseInteractionResponse CreateUser(UserModel userModel)
        {
            // Check if userId is currently being used
            if (GetUserById(userModel.Id) != null)
            {
                return new DatabaseInteractionResponse() { Success = false, Message = "There already exists a user with the id: " + userModel.Id };
            }

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@userId", userModel.Id), 
                new SqlParameter("@name", userModel.Name), 
                new SqlParameter("@email", userModel.Email), 
                new SqlParameter("@location", userModel.Location.ToString())
            };

            return ExecuteSqlNonQuery("CreateUser", parameters);
        }

        public static DatabaseInteractionResponse DeleteUser(int userId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@userId", userId)
            };

            return ExecuteSqlNonQuery("DeleteUser", parameters);
        }

        #endregion

        #region Helpers

        private static DatabaseInteractionResponse ExecuteSqlNonQuery(string commandText, List<SqlParameter> parameters = null)
        {
            try
            {
                using (var conn = new SqlConnection(SqlConnectionStr))
                using (var command = new SqlCommand(commandText, conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    conn.Open();

                    // Add parameters, if any
                    foreach (var param in parameters ?? new List<SqlParameter>())
                    {
                        command.Parameters.Add(param);
                    }
                    command.ExecuteNonQuery();
                    conn.Close();
                }

                return new DatabaseInteractionResponse() {Success = true};
            }
            catch (Exception e)
            {
                return new DatabaseInteractionResponse() { Success = false, Message = "An error occurred during sql nonquery execution. Message: " + e.Message};
            }
            
        }

        /// <summary>
        /// Executes a Sql command and executes func with the SqlDataReader
        /// </summary>
        /// <param name="commandText">Name of the stored procedure</param>
        /// <param name="func">The function to be executed with the reader</param>
        /// <param name="parameters">Optional: parameters to add to the command</param>
        /// <returns></returns>
        private static object ExecuteSqlReader(string commandText, Func<SqlDataReader, object> func, List<SqlParameter> parameters = null)
        {
            object output;

            using (var conn = new SqlConnection(SqlConnectionStr))
            using (var command = new SqlCommand(commandText, conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();

                // Add parameters, if any
                foreach (var param in parameters ?? new List<SqlParameter>())
                {
                    command.Parameters.Add(param);
                }
                // Execute func
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    output = func(rdr);
                }

                conn.Close();
            }

            return output;
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

        private static EventModel GetEventModelFromReader(SqlDataReader rdr)
        {
            //rdr["Attendees"];
            return new EventModel()
            {
                Attendees = GetEventAttendees((int)rdr["EventId"]),
                Description = rdr["Description"].ToString(),
                Id = (int)rdr["EventId"],
                ImageUrl = rdr["ImageUrl"].ToString(),
                InterestGroup = GetInterestGroupById((int)rdr["InterestGroupId"]),
                Location = rdr["Location"].ToString(),
                Name = rdr["Name"].ToString(),
                Time = (DateTime)rdr["Time"]
            };
        }

        private static List<UserModel> GetEventAttendees(int eventId)
        {
            List<UserModel> attendees = new List<UserModel>();

            using (var conn = new SqlConnection(SqlConnectionStr))
            using (var command = new SqlCommand("GetEventAttendees", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.Parameters.Add(new SqlParameter("@eventId", eventId));
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        UserModel attendee = GetUserModelFromReader(rdr);
                        attendees.Add(attendee);
                    }
                }
                conn.Close();
            }

            return attendees;
        }

        private static InterestGroupModel GetInterestGroupFromReader(SqlDataReader rdr)
        {
            //rdr["Attendees"];
            return new InterestGroupModel()
            {
                Description = rdr["Description"].ToString(),
                Id = (int)rdr["InterestGroupId"],
                ImageUrl = rdr["ImageUrl"].ToString(),
                Manager = GetUserById((int)rdr["ManagerId"]),
                Members = GetInterestGroupMembers((int)rdr["InterestGroupId"]),
                Name = rdr["Name"].ToString(),
            };
        }

        private static List<UserModel> GetInterestGroupMembers(int interestGroupId)
        {
            List<UserModel> members = new List<UserModel>();

            using (var conn = new SqlConnection(SqlConnectionStr))
            using (var command = new SqlCommand("GetInterestGroupMembers", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.Parameters.Add(new SqlParameter("@interestGroupId", interestGroupId));
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        UserModel attendee = GetUserModelFromReader(rdr);
                        members.Add(attendee);
                    }
                }
                conn.Close();
            }

            return members;
        }

        #endregion
    }
}