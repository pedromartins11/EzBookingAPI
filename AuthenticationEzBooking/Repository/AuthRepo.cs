using AuthenticationEzBooking.Dto;
using AuthenticationEzBooking.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AuthenticationEzBooking.Repository
{
    public class AuthRepo
    {
        private readonly IConfiguration _configuration;

        public AuthRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void UpdateUserToken(int userId, string newToken)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();

                string query = "UPDATE Users SET token = @newToken WHERE id_user = @userId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@newToken", newToken);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public AuthDto GetUserByEmail(string email)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();

                string query = "SELECT * from Users WHERE email like @email";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);

                    // Use um leitor para obter os resultados da consulta.
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Preencha um objeto User (ou o tipo que você estiver usando) com os dados do leitor.
                            AuthDto auth = new AuthDto
                            {
                                email = reader["email"].ToString(),
                                password = reader["password"].ToString()
                            };

                            return auth;
                        }
                        else
                        {
                            // Se não houver resultado, retorne null ou lance uma exceção, dependendo do que faz sentido em seu contexto.
                            return null;
                        }
                    }
                }
            }
        }

        public string GetTokenById(int userId)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();

                string query = "SELECT token FROM Users WHERE id_user = @userId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    // ExecuteScalar é usado para obter um único valor do banco de dados.
                    object result = cmd.ExecuteScalar();

                    string token = (result != null) ? result.ToString() : string.Empty;

                    return token;
                }
            }
        }


        public int GetUserIdbyEmail(string email)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();

                string query = "SELECT id_user FROM Users WHERE email = @email";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);

                    // ExecuteScalar é usado para obter um único valor do banco de dados.
                    object result = cmd.ExecuteScalar();

                    // Converte o resultado para int ou trata o caso em que é nulo.
                    int userId = (result != null) ? Convert.ToInt32(result) : 0;

                    return userId;
                }
            }
        }

        private string GetUserEmailById(int userId)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();

                string query = "SELECT Email FROM Users WHERE id_user = @userId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    // ExecuteScalar é usado para obter um único valor do banco de dados.
                    object result = cmd.ExecuteScalar();

                    // Converte o resultado para string (e-mail) ou trata o caso em que é nulo.
                    string userEmail = (result != null) ? result.ToString() : string.Empty;

                    return userEmail;
                }
            }
        }

        public int GetUserTypeById(int userId)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();

                string query = "SELECT userTypeid_userType FROM Users WHERE id_user = @userId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    // ExecuteScalar é usado para obter um único valor do banco de dados.
                    object result = cmd.ExecuteScalar();

                    // Converte o resultado para string (e-mail) ou trata o caso em que é nulo.
                    int userType = (result != null) ? Convert.ToInt32(result) : 0;

                    return userType;
                }
            }
        }

        public string CreateToken(int userId, int userType)
        {
            string userEmail = GetUserEmailById(userId);
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userEmail),
                new Claim(ClaimTypes.UserData, userType.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString(), ClaimValueTypes.Integer32),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private RefreshToken GetRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.Now,
                Expires = DateTime.Now.AddDays(7)
            };
            return refreshToken;
        }

        public bool TokenIsRevoked(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
            
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.token, cookieOptions);

        }

        private void UpdateUserRefreshToken(int userId, string refreshToken)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();

                string query = "SELECT COUNT(*) FROM RevokedTokens WHERE Token = @token";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@token", token);

                    int count = (int)cmd.ExecuteScalar();

                    return count > 0;
                }
            }
        }

        public void RevokeToken(string token)
        {
            
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();

                string query = "INSERT INTO RevokedTokens (Token, RevocationDate) VALUES (@token, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@token", token);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool IsTokenValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                string query = "UPDATE Users SET tokenExpires = @tokenExpires WHERE id_user = @userId";

                if (jsonToken?.ValidTo != null && jsonToken.ValidTo < DateTime.UtcNow)
                {
                    return false;
                }


                return true;
            }
            catch (Exception)
            {
                return false; // Token inválido
            }
        }






        //public RefreshToken GenerateRefreshToken()
        //{
        //    var refreshToken = new RefreshToken
        //    {
        //        token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
        //        Created = DateTime.Now,
        //        Expires = DateTime.Now.AddDays(7)
        //    };
        //    return refreshToken;
        //}

        //public void SetRefreshToken(RefreshToken newRefreshToken, HttpResponse Response)
        //{
        //    var cookieOptions = new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Expires = newRefreshToken.Expires,

        //    };
        //    Response.Cookies.Append("refreshToken", newRefreshToken.token, cookieOptions);

        //}

        //public void UpdateUserRefreshToken(int userId, string refreshToken)
        //{
        //    using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //    {
        //        conn.Open();

        //        string query = "UPDATE Users SET refreshToken = @newRefreshToken WHERE id_user = @userId";

        //        using (SqlCommand cmd = new SqlCommand(query, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@newRefreshToken", refreshToken);
        //            cmd.Parameters.AddWithValue("@userId", userId);

        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}

        //public void UpdateUserTokenCreated(int userId, DateTime dateCreated)
        //{
        //    using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //    {
        //        conn.Open();

        //        string query = "UPDATE Users SET dateCreated = @dateCreated WHERE id_user = @userId";

        //        using (SqlCommand cmd = new SqlCommand(query, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@dateCreated", dateCreated);
        //            cmd.Parameters.AddWithValue("@userId", userId);

        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}

        //public void UpdateUserTokenExpires(int userId, DateTime tokenExpires)
        //{
        //    using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //    {
        //        conn.Open();

        //        string query = "UPDATE Users SET tokenExpires = @tokenExpires WHERE id_user = @userId";

        //        using (SqlCommand cmd = new SqlCommand(query, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@tokenExpires", tokenExpires);
        //            cmd.Parameters.AddWithValue("@userId", userId);

        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}

        //public string GetRefreshTokenById(int userId)
        //{
        //    using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //    {
        //        conn.Open();

        //        string query = "SELECT refreshToken FROM Users WHERE id_user = @userId";

        //        using (SqlCommand cmd = new SqlCommand(query, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@userId", userId);

        //            // ExecuteScalar é usado para obter um único valor do banco de dados.
        //            object result = cmd.ExecuteScalar();

        //            // Converte o resultado para string (e-mail) ou trata o caso em que é nulo.
        //            string refreshToken = (result != null) ? result.ToString() : string.Empty;

        //            return refreshToken;
        //        }
        //    }
        //}

        //public DateTime GetTokenExpiresById(int userId)
        //{
        //    using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //    {
        //        conn.Open();

        //        string query = "SELECT tokenExpires FROM Users WHERE id_user = @userId";

        //        using (SqlCommand cmd = new SqlCommand(query, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@userId", userId);

        //            // ExecuteScalar é usado para obter um único valor do banco de dados.
        //            object result = cmd.ExecuteScalar();

        //            // Converte o resultado para string (e-mail) ou trata o caso em que é nulo.
        //            DateTime tokenExpires = (result != null) ? Convert.ToDateTime(result) : DateTime.MinValue;

        //            return tokenExpires;
        //        }
        //    }
        //}

    }
}