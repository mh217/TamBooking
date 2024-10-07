using Microsoft.Extensions.Configuration;
using Npgsql;
using TamBooking.Model;
using TamBooking.Repository.Common;

namespace TamBooking.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly string _connectionString;

        public NotificationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Notification>> GetNotificationAsync(Guid id)
        {
            List<Notification> notifications = new List<Notification>();
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = "SELECT note.\"Id\", note.\"From\", note.\"To\", note.\"Title\", note.\"Text\", note.\"DateUpdated\", note.\"DateCreated\", note.\"CreatedByUserId\", note.\"UpdatedByUserId\",note.\"GigId\", note.\"IsActive\", note.\"BandId\", note.\"ClientId\", note.\"RecepientTypeId\"  " +
                "FROM \"Notification\" note " +
                "LEFT JOIN \"Band\" bd ON note.\"BandId\" = bd.\"Id\" " +
                "LEFT JOIN  \"RecepientType\" rt ON note.\"RecepientTypeId\" = rt.\"Id\" " +
                "LEFT JOIN   \"Client\" cl ON note.\"ClientId\" = cl.\"Id\" " +
                "WHERE  note.\"IsActive\" = true  " +
                "AND ((rt.\"Name\" = 'client' AND note.\"ClientId\" = @userId) OR (rt.\"Name\" = 'band' AND note.\"BandId\" = @userId)) ORDER BY note.\"DateUpdated\" DESC;";


            try
            {


                var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@userId", id);
                connection.Open();
                using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        Notification notification = new Notification();
                        notification.Id = reader["Id"] != DBNull.Value ? Guid.Parse(reader["Id"].ToString()) : Guid.Empty;
                        notification.From = reader["From"].ToString();
                        notification.To = reader["To"].ToString();
                        notification.Title = reader["Title"].ToString();
                        notification.Text = reader["Text"].ToString();
                        notification.IsActive = (bool)reader["IsActive"];
                        notification.DateCreated = DateTime.Parse(reader["DateCreated"].ToString());
                        notification.DateUpdated = DateTime.Parse(reader["DateUpdated"].ToString());
                        notification.CreatedByUserId = Guid.Parse(reader["CreatedByUserId"].ToString());
                        notification.UpdatedByUserId = Guid.Parse(reader["UpdatedByUserId"].ToString());
                        notification.RecepientTypeId = Guid.Parse(reader["RecepientTypeId"].ToString());
                        notification.BandId = Guid.Parse(reader["BandId"].ToString());
                        notification.GigId = Guid.Parse(reader["GigId"].ToString());
                        notification.ClientId = Guid.Parse(reader["ClientId"].ToString());
                        notifications.Add(notification);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }


            return notifications;
        }

        public async Task<Notification?> GetNotificationByIdAsync(Guid id)
        {
            Notification notification = new Notification();
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = "SELECT * FROM \"Notification\"  WHERE \"Id\" = @id;";

            var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                await reader.ReadAsync();
                notification.Id = reader["Id"] != DBNull.Value ? Guid.Parse(reader["Id"].ToString()) : Guid.Empty;
                notification.From = reader["From"].ToString();
                notification.To = reader["To"].ToString();
                notification.Title = reader["Title"].ToString();
                notification.Text = reader["Text"].ToString();
                notification.IsActive = (bool)reader["IsActive"];
                notification.DateCreated = DateTime.Parse(reader["DateCreated"].ToString());
                notification.DateUpdated = DateTime.Parse(reader["DateUpdated"].ToString());
                notification.CreatedByUserId = Guid.Parse(reader["CreatedByUserId"].ToString());
                notification.UpdatedByUserId = Guid.Parse(reader["UpdatedByUserId"].ToString());
                notification.RecepientTypeId = Guid.Parse(reader["RecepientTypeId"].ToString());
                notification.BandId = Guid.Parse(reader["BandId"].ToString());
                notification.ClientId = Guid.Parse(reader["ClientId"].ToString());
                notification.GigId = Guid.Parse(reader["GigId"].ToString());
            }
            else
            {
                return null;
            }
            return notification;
        }

        public async Task<Guid> CreateNotificationAsync(Guid bandId, string role, Guid userId, string userEmail, string bandEmail, Guid gigId, Gig gig)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            string commandText = "INSERT INTO \"Notification\" (\"From\", \"To\", \"Title\", \"Text\", \"CreatedByUserId\", \"UpdatedByUserId\", " +
                "\"BandId\", \"ClientId\",\"RecepientTypeId\", \"GigId\", \"IsActive\") " +
                "VALUES (@from, @to, @title, @text, @createdByUserId, @updatedByUserId, @bandId, @clientId, @recepientTypeId, @gigId, @isActive) " +
                "RETURNING \"Id\";";

            using var command = new NpgsqlCommand(commandText, connection);

            if (role == "client")
            {
                command.Parameters.AddWithValue("@from", userEmail);
                command.Parameters.AddWithValue("@to", bandEmail);
                command.Parameters.AddWithValue("@title", "Upit za rezervaciju svirke");
                command.Parameters.AddWithValue("@text", $"{userEmail} želi zakazati svirku! Datum: {gig.OccasionDate}, Adresa: {gig.Address.Line + " " + gig.Address.BuildingNumber + " " + gig.Address.Suite} ");
                command.Parameters.AddWithValue("@createdByUserId", userId);
                command.Parameters.AddWithValue("@updatedByUserId", userId);
                command.Parameters.AddWithValue("@bandId", bandId);
                command.Parameters.AddWithValue("@clientId", userId);
                command.Parameters.AddWithValue("@recepientTypeId", Guid.Parse("c3d4e5f6-a7b8-9012-cd34-ef56ac78bc90")); 
                command.Parameters.AddWithValue("@gigId", gigId);
                command.Parameters.AddWithValue("@isActive", true);
            }
            

            
            var result = await command.ExecuteScalarAsync();

            if (result == null)
            {
                throw new Exception("Failed to create notification.");
            }

            return (Guid)result;
        }


        public async Task<Guid> CreateCancelNotificationAsync(Guid bandId, string role, Guid userId, string userEmail, string bandEmail, Guid gigId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = "INSERT INTO \"Notification\" (\"From\", \"To\", \"Title\", \"Text\", \"CreatedByUserId\", \"UpdatedByUserId\", " +
                "\"BandId\", \"ClientId\",\"RecepientTypeId\", \"GigId\", \"IsActive\") " +
                "VALUES (@from, @to, @title, @text, @createdByUserId, @updatedByUserId, @bandId, @clientId, " +
                "@recepientTypeId, @gigId, @isActive) RETURNING \"Id\";";

            using var command = new NpgsqlCommand(commandText, connection);

            if (role == "client")
            {
                command.Parameters.AddWithValue("@from", userEmail);
                command.Parameters.AddWithValue("@to", bandEmail);
                command.Parameters.AddWithValue("@title", "Otkazivanje svirke!");
                command.Parameters.AddWithValue("@text", $"{userEmail} otkazuje svirku!");
                command.Parameters.AddWithValue("@createdByUserId", userId);
                command.Parameters.AddWithValue("@updatedByUserId", userId);
                command.Parameters.AddWithValue("@bandId", bandId);
                command.Parameters.AddWithValue("@clientId", userId);
                command.Parameters.AddWithValue("@recepientTypeId", Guid.Parse("c3d4e5f6-a7b8-9012-cd34-ef56ac78bc90"));
                command.Parameters.AddWithValue("@gigId", gigId);
                command.Parameters.AddWithValue("@isActive", true);
            }
            else
            {
                command.Parameters.AddWithValue("@from", bandEmail);
                command.Parameters.AddWithValue("@to", userEmail);
                command.Parameters.AddWithValue("@title", "Otkazivanje svirke!");
                command.Parameters.AddWithValue("@text", $"{bandEmail} otkazuje svirku!");
                command.Parameters.AddWithValue("@createdByUserId", bandId);
                command.Parameters.AddWithValue("@updatedByUserId", bandId);
                command.Parameters.AddWithValue("@bandId", bandId);
                command.Parameters.AddWithValue("@clientId", userId);
                command.Parameters.AddWithValue("@recepientTypeId", Guid.Parse("d4e5f6a7-b8c9-0123-de45-ac67ab89cd01"));
                command.Parameters.AddWithValue("@gigId", gigId);
                command.Parameters.AddWithValue("@isActive", true);
            }

            await connection.OpenAsync();
            var notificationId = await command.ExecuteScalarAsync();

            if (notificationId == null)
            {
                throw new Exception("Failed to create notification.");
            }

            return Guid.Parse(notificationId.ToString());
        }


        public async Task<Guid> CreateConfirmNotificationAsync(Guid bandId, string role, Guid userId, string userEmail, string bandEmail, Guid gigId, Guid clientId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = "INSERT INTO \"Notification\" (\"From\", \"To\", \"Title\", \"Text\", \"CreatedByUserId\", " +
                "\"UpdatedByUserId\", \"BandId\", \"ClientId\",\"RecepientTypeId\", \"GigId\", \"IsActive\") " +
                "VALUES (@from, @to, @title, @text, @createdByUserId, @updatedByUserId, @bandId, @clientId, @recepientTypeId, @gigId, @isActive) RETURNING \"Id\";";
            using var command = new NpgsqlCommand(commandText, connection);
            const string recepientTypeId = "d4e5f6a7-b8c9-0123-de45-ac67ab89cd01";

            command.Parameters.AddWithValue("@from", bandEmail);
            command.Parameters.AddWithValue("@to", userEmail);
            command.Parameters.AddWithValue("@title", "Odgovor za rezervaciju!");
            command.Parameters.AddWithValue("@text", $"{bandEmail} potvrdjuje vasu svirku!");
            command.Parameters.AddWithValue("@createdByUserId", userId);
            command.Parameters.AddWithValue("@updatedByUserId", userId);
            command.Parameters.AddWithValue("@bandId", bandId);
            command.Parameters.AddWithValue("@clientId", clientId);
            command.Parameters.AddWithValue("@recepientTypeId", Guid.Parse(recepientTypeId));
            command.Parameters.AddWithValue("@gigId", gigId);
            command.Parameters.AddWithValue("@isActive", true);

            connection.Open();
            var result = new object();
            try
            {
               result = await command.ExecuteScalarAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
                
            

            
            if (result == null)
            {
                throw new Exception("Failed to create notification.");
            }

            return (Guid)result;
        }

        public async Task<bool> DeleteNotificationAsync(Guid id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            string commandText = "UPDATE \"Notification\" SET \"IsActive\" = false WHERE \"Id\" = @id;";
            var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            var numberOfCommits = await command.ExecuteNonQueryAsync();

            if (numberOfCommits == 0)
            {
                return false;
            }
            return true;
        }
    }
}