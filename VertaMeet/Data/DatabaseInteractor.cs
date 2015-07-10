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
        private static readonly string SqlConnectionStr = new EntityConnectionStringBuilder(ConfigurationManager.ConnectionStrings["VertaMeetDataEntities"].ConnectionString).ProviderConnectionString;

        #region Get

        /// <summary>
        /// Gets data about the event with eventId
        /// </summary>
        /// <returns>Null if no event by that id, otherwise the event with that id</returns>
        public static EventModel GetEventById(int eventId)
        {
            EventModel outputEvent = null;

            using (var conn = new SqlConnection(SqlConnectionStr))
            using (var command = new SqlCommand("GetEventById", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.Parameters.Add(new SqlParameter("@eventId", eventId));
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        outputEvent = GetEventModelFromReader(rdr);
                    }
                }
                conn.Close();
            }

            return outputEvent;
        }

        /// <summary>
        /// Gets data about the user with userId
        /// </summary>
        /// <returns>Null if no user by that id, otherwise the user with that id</returns>
        public static InterestGroupModel GetInterestGroupById(int interestGroupId)
        {
            InterestGroupModel outputInterestGroup = null;

            using (var conn = new SqlConnection(SqlConnectionStr))
            using (var command = new SqlCommand("GetInterestGroupById", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.Parameters.Add(new SqlParameter("@interestGroupId", interestGroupId));
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        outputInterestGroup = GetInterestGroupFromReader(rdr);
                    }
                }
                conn.Close();
            }

            return outputInterestGroup;
        }

        /// <summary>
        /// Gets data about the user with userId
        /// </summary>
        /// <returns>Null if no user by that id, otherwise the user with that id</returns>
        public static UserModel GetUserById(int userId)
        {
            UserModel outputUser = null;

            using (var conn = new SqlConnection(SqlConnectionStr))
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

            using (var conn = new SqlConnection(SqlConnectionStr))
            using (var command = new SqlCommand("GetAllUsers", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
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

            using (var conn = new SqlConnection(SqlConnectionStr))
            using (var command = new SqlCommand("CreateEvent", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.Parameters.Add(new SqlParameter("@eventId", eventModel.Id));
                command.Parameters.Add(new SqlParameter("@name", eventModel.Name));
                command.Parameters.Add(new SqlParameter("@description", eventModel.Description));
                command.Parameters.Add(new SqlParameter("@time", eventModel.Time));
                command.Parameters.Add(new SqlParameter("@imageUrl", eventModel.ImageUrl));
                command.Parameters.Add(new SqlParameter("@location", eventModel.Location.ToString()));
                command.Parameters.Add(new SqlParameter("@interestGroupId", eventModel.InterestGroup));
                command.ExecuteNonQuery();
                conn.Close();
            }

            return new DatabaseInteractionResponse() { Success = true };
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

            using (var conn = new SqlConnection(SqlConnectionStr))
            using (var command = new SqlCommand("CreateInterestGroupModel", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.Parameters.Add(new SqlParameter("@interestGroupId", interestGroupModel.Id));
                command.Parameters.Add(new SqlParameter("@name", interestGroupModel.Name));
                command.Parameters.Add(new SqlParameter("@description", interestGroupModel.Description));
                command.Parameters.Add(new SqlParameter("@managerId", interestGroupModel.Manager.Id));
                command.Parameters.Add(new SqlParameter("@imageUrl", interestGroupModel.ImageUrl));
                command.ExecuteNonQuery();
                conn.Close();
            }

            return new DatabaseInteractionResponse() { Success = true };
        }


        public static DatabaseInteractionResponse CreateUser(UserModel userModel)
        {
            // Check if userId is currently being used
            if (GetUserById(userModel.Id) != null)
            {
                return new DatabaseInteractionResponse() { Success = false, Message = "There already exists a user with the id: " + userModel.Id };
            }

            using (var conn = new SqlConnection(SqlConnectionStr))
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
            using (var conn = new SqlConnection(SqlConnectionStr))
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

        #endregion

        #region Helpers

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