using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Library.Core.Data;
using Library.Core.Models;
using Umbraco.Core.Persistence.Repositories;

namespace Library.Core.Repositories
{
    public class MemberRepository : BaseRepository, IMemberMasterRepository
    {
        public List<MemberMaster> GetAllMembers()
        {
            var members = new List<MemberMaster>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT MemberId, FullName, Mobile, Email, IsActive, CreatedDate 
                            FROM MemberMaster WHERE IsActive = 1 ORDER BY FullName";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        members.Add(new MemberMaster
                        {
                            MemberId = reader.GetInt32(reader.GetOrdinal("MemberId")),
                            FullName = reader.GetString(reader.GetOrdinal("FullName")),
                            Mobile = reader.GetString(reader.GetOrdinal("Mobile")),
                            Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                        });
                    }
                }
            }

            return members;
        }

        public MemberMaster GetMemberById(int memberId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"SELECT MemberId, FullName, Mobile, Email, IsActive, CreatedDate 
                            FROM MemberMaster WHERE MemberId = @MemberId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MemberId", memberId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new MemberMaster
                            {
                                MemberId = reader.GetInt32(reader.GetOrdinal("MemberId")),
                                FullName = reader.GetString(    reader.GetOrdinal("FullName")),
                                Mobile = reader.GetString(reader.GetOrdinal("Mobile")),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")   ),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                            };
                        }
                    }
                }
            }

            return null;
        }

        public int AddMember(MemberMaster member)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"INSERT INTO MemberMaster (FullName, Mobile, Email, IsActive, CreatedDate)
                            OUTPUT INSERTED.MemberId
                            VALUES (@FullName, @Mobile, @Email, 1, GETDATE())";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FullName", member.FullName);
                    command.Parameters.AddWithValue("@Mobile", member.Mobile);
                    command.Parameters.AddWithValue("@Email", member.Email ?? (object)DBNull.Value);

                    return (int)command.ExecuteScalar();
                }
            }
        }

        public bool UpdateMember(MemberMaster member)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = @"UPDATE MemberMaster SET 
                            FullName = @FullName, Mobile = @Mobile, Email = @Email
                            WHERE MemberId = @MemberId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MemberId", member.MemberId);
                    command.Parameters.AddWithValue("@FullName", member.FullName);
                    command.Parameters.AddWithValue("@Mobile", member.Mobile);
                    command.Parameters.AddWithValue("@Email", member.Email ?? (object)DBNull.Value);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteMember(int memberId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var query = "UPDATE MemberMaster SET IsActive = 0 WHERE MemberId = @MemberId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MemberId", memberId);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<MemberMaster> GetActiveMembers()
        {
            return GetAllMembers(); // Already filters for active members
        }
    }
}
