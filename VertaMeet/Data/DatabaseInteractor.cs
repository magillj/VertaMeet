using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SqlClient;
using VertaMeet.Models;

namespace VertaMeet.Data
{
    public static class DatabaseInteractor
    {
        //private static readonly string SqlConnectionStr = new EntityConnectionStringBuilder(ConfigurationManager.ConnectionStrings["VertaMeetDataEntities"].ConnectionString).ProviderConnectionString;
        private static readonly string SqlConnectionStr =
            ConfigurationManager.ConnectionStrings["VertaMeetDataEntities"].ConnectionString;

        #region Get

        /// <summary>
        /// Returns a list of all the Interest Groups in the database
        /// </summary>
        /// <returns>All the interest groups in the database</returns>
        public static List<InterestGroupModel> GetAllInterestGroups()
        {
            Func<SqlDataReader, List<InterestGroupModel>> readInterestGroups = delegate(SqlDataReader rdr)
            {
                List<InterestGroupModel> interestGroups = new List<InterestGroupModel>();

                while (rdr.Read())
                {
                    InterestGroupModel interestGroupModel = GetInterestGroupModelFromReader(rdr);
                    interestGroups.Add(interestGroupModel);
                }

                return interestGroups;
            };

            return (List<InterestGroupModel>)ExecuteSqlReader("GetAllInterestGroups", readInterestGroups);
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
        /// Gets a list of events corresponding to the interest group with interestGroupId
        /// </summary>
        public static List<EventModel> GetEventsForInterestGroup(int interestGroupId)
        {
            Func<SqlDataReader, List<EventModel>> readEvents = delegate(SqlDataReader rdr)
            {
                List<EventModel> events = new List<EventModel>();

                while (rdr.Read())
                {
                    EventModel eventModel = GetEventModelFromReader(rdr);
                    events.Add(eventModel);
                }

                return events;
            };

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@interestGroupId", interestGroupId) };

            return (List<EventModel>)ExecuteSqlReader("GetEventsForInterestGroup", readEvents, parameters);
        }

        public static int GetHighestEventId()
        {
            // Cant return int, workaround with string
            Func<SqlDataReader, string> readHighestEvent = delegate(SqlDataReader rdr)
            {
                return rdr.Read() ? rdr["EventId"] + "" : "-1";
            };

            return Int32.Parse((string)ExecuteSqlReader("GetHighestEventId", readHighestEvent));
        }

        public static int GetHighestInterestGroupId()
        { 
            // Cant return int, workaround with string
            Func<SqlDataReader, string> readHighestInterestGroup = delegate(SqlDataReader rdr)
            {
                return rdr.Read() ? rdr["InterestGroupId"] + "" : "-1";
            };

            return Int32.Parse((string)ExecuteSqlReader("GetHighestInterestGroupId", readHighestInterestGroup));
        }

        public static int GetHighestUserId()
        {
            // Cant return int, workaround with string
            Func<SqlDataReader, string> readHighestUser = delegate(SqlDataReader rdr)
            {
                return rdr.Read() ? rdr["UserId"] + "" : "-1";
            };

            return Int32.Parse((string)ExecuteSqlReader("GetHighestUserId", readHighestUser));
        }

        public static List<string> GetImagesForInterestGroup(int interestGroupId)
        {
            Func<SqlDataReader, List<string>> readInterestGroup = delegate(SqlDataReader rdr)
            {
                List<string> images = new List<string>();
                while (rdr.Read())
                {
                    images.Add((string)rdr["ImageUrl"]);
                }
                return images;
            };

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@interestGroupId", interestGroupId) };

            return (List<string>)ExecuteSqlReader("GetInterestGroupImages", readInterestGroup, parameters);
        } 

        /// <summary>
        /// Gets data about the user with userId
        /// </summary>
        /// <returns>Null if no user by that id, otherwise the user with that id</returns>
        public static InterestGroupModel GetInterestGroupById(int interestGroupId)
        {
            Func<SqlDataReader, InterestGroupModel> readInterestGroup = delegate(SqlDataReader rdr)
            {
                return rdr.Read() ? GetInterestGroupModelFromReader(rdr) : null;
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

        #endregion

        #region Modify

        public static DatabaseInteractionResponse AddImageToInterestGroup(int interestGroupId, string imageUrl)
        {
            // Check if image is already in Interest Group
            if (GetImagesForInterestGroup(interestGroupId).Contains(imageUrl))
            {
                return new DatabaseInteractionResponse()
                {
                    Success = false,
                    Message = "Image is already associated with that interest group"
                };
            }

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@interestGroupId", interestGroupId),
                new SqlParameter("@imageUrl", imageUrl)
            };

            return ExecuteSqlNonQuery("AddImageToInterestGroup", parameters);
        }

        public static DatabaseInteractionResponse AddUserToEvent(int eventId, int userId)
        {
            // Check if user is already in event
            if (GetEventAttendees(eventId).Contains(GetUserById(userId)))
            {
                return new DatabaseInteractionResponse()
                {
                    Success = false,
                    Message = "User is already a member of the event"
                };
            }

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@eventId", eventId),
                new SqlParameter("@userId", userId)
            };

            return ExecuteSqlNonQuery("AddUserToEvent", parameters);
        }

        public static DatabaseInteractionResponse AddUserToInterestGroup(int interestGroupId, int userId)
        {
            // Check if user is already in interest group
            if (GetInterestGroupMembers(interestGroupId).Contains(GetUserById(userId)))
            {
                return new DatabaseInteractionResponse()
                {
                    Success = false,
                    Message = "User is already a member of the interest group"
                };
            }

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@interestGroupId", interestGroupId),
                new SqlParameter("@userId", userId)
            };

            return ExecuteSqlNonQuery("AddUserToInterestGroup", parameters);
        }

        public static DatabaseInteractionResponse CreateEvent(EventModel eventModel)
        {
            // Check if eventId is currently being used
            if (GetEventById(eventModel.Id) != null)
            {
                return new DatabaseInteractionResponse() { Success = false, Message = "There already exists an event with the id: " + eventModel.Id };
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

            DatabaseInteractionResponse eventResponse = ExecuteSqlNonQuery("CreateEvent", parameters);

            if (eventResponse.Success)
            {
                foreach (UserModel user in eventModel.Attendees)
                {
                    DatabaseInteractionResponse userResponse = AddUserToEvent(eventModel.Id, user.Id);

                    // TODO: This is a really bad failure. Log on critical or undo everything
                    if (!userResponse.Success)
                    {
                        return userResponse;
                    }
                }

                return new DatabaseInteractionResponse() { Success = true };
            }

            return eventResponse;
        }

        public static DatabaseInteractionResponse CreateInterestGroup(InterestGroupModel interestGroupModel)
        {
            // Check if userId is currently being used
            if (GetInterestGroupById(interestGroupModel.Id) != null)
            {
                return new DatabaseInteractionResponse() { Success = false, Message = "There already exists an interest group with the id: " + interestGroupModel.Id };
            }

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@interestGroupId", interestGroupModel.Id), 
                new SqlParameter("@name", interestGroupModel.Name), 
                new SqlParameter("@description", interestGroupModel.Description), 
                new SqlParameter("@managerId", interestGroupModel.Manager.Id), 
                new SqlParameter("@imageUrl", interestGroupModel.ImageUrl)
            };

            DatabaseInteractionResponse interestGroupResponse = ExecuteSqlNonQuery("CreateInterestGroup", parameters);

            if (interestGroupResponse.Success)
            {
                foreach (UserModel user in interestGroupModel.Members)
                {
                    DatabaseInteractionResponse userResponse = AddUserToInterestGroup(interestGroupModel.Id, user.Id);

                    // TODO: This is a really bad failure. Log on critical or undo everything
                    if (!userResponse.Success)
                    {
                        return userResponse;
                    }
                }

                return new DatabaseInteractionResponse() { Success = true };
            }

            return interestGroupResponse;
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

        public static DatabaseInteractionResponse DeleteEvent(int eventId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@eventId", eventId)
            };

            return ExecuteSqlNonQuery("DeleteEvent", parameters);
        }

        public static DatabaseInteractionResponse DeleteInterestGroup(int interestGroup)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@interestGroupId", interestGroup)
            };

            return ExecuteSqlNonQuery("DeleteInterestGroup", parameters);
        }

        public static DatabaseInteractionResponse DeleteUser(int userId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@userId", userId)
            };

            //TODO: Delete all references in other tables

            return ExecuteSqlNonQuery("DeleteUser", parameters);
        }

        public static DatabaseInteractionResponse RemoveUserFromEvent(int eventId, int userId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@eventId", eventId),
                new SqlParameter("@userId", userId)
            };

            return ExecuteSqlNonQuery("RemoveUserFromEvent", parameters);
        }

        public static DatabaseInteractionResponse RemoveUserFromInterestGroup(int interestGroup, int userId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@interestGroupId", interestGroup),
                new SqlParameter("@userId", userId)
            };

            return ExecuteSqlNonQuery("RemoveUserFromEventInterestGroup", parameters);
        }

        public static DatabaseInteractionResponse UpdateEvent(EventModel eventModel)
        {
            DatabaseInteractionResponse deleteResponse = DeleteEvent(eventModel.Id);
            
            return deleteResponse.Success ? CreateEvent(eventModel) : deleteResponse;
        }

        public static DatabaseInteractionResponse UpdateInterestGroup(InterestGroupModel interestGroupModel)
        {
            DatabaseInteractionResponse deleteResponse = DeleteInterestGroup(interestGroupModel.Id);

            return deleteResponse.Success ? CreateInterestGroup(interestGroupModel) : deleteResponse;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Executes a Sql Nonquery
        /// </summary>
        /// <param name="commandText">Name of the stored procedure</param>
        /// <param name="parameters">Optional: parameters to add to the command</param>
        /// <returns>A DatabaseInteractionREsponse indicating the outcome of the procedure</returns>
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

        private static InterestGroupModel GetInterestGroupModelFromReader(SqlDataReader rdr)
        {
            return new InterestGroupModel()
            {
                Description = rdr["Description"].ToString(),
                ImageGallery = GetImagesForInterestGroup((int)rdr["InterestGroupId"]),
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