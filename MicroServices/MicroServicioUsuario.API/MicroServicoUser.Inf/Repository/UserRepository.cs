using System.Xml.XPath;
using MicroServicioUser.Dom.Entities;
using MicroServicioUser.Dom.Patterns;
using MicroServicioUser.Dom.Interfaces;
using MicroServicoUser.Inf.Persistence.Database;

namespace MicroServicoUser.Inf.Repository
{
    public class IdDto
    {
        public int Id { get; set; }
    }
<<<<<<< HEAD

    public class UserRepository : IRepository
    {
        private readonly MySqlConnectionManager _dbConnectionManager;

=======
    public class UserRepository : IRepository
    {
        private readonly MySqlConnectionManager _dbConnectionManager;
>>>>>>> AnalisisSonarEstablishment
        public UserRepository(MySqlConnectionManager dbConnectionManager)
        {
            _dbConnectionManager = dbConnectionManager;
        }

<<<<<<< HEAD
        public Task<Result<IEnumerable<User>>> GetAll()
=======
        public async Task<Result<IEnumerable<User>>> GetAll()
>>>>>>> AnalisisSonarEstablishment
        {
            string query = @"
                SELECT
                    id            AS Id,
                    username      AS Username,
                    password_hash AS PasswordHash,
                    first_name    AS FirstName,
<<<<<<< HEAD
                    first_last_name AS LastName,
=======
                    first_last_name     AS LastName,
>>>>>>> AnalisisSonarEstablishment
                    email         AS Email,
                    role          AS Role,
                    created_by    AS CreatedBy,
                    created_date  AS CreatedDate,
                    last_update   AS LastUpdate,
                    status        AS Status,
                    first_login   AS FirstLogin
                FROM user
                WHERE status = TRUE
                ORDER BY id DESC;";
<<<<<<< HEAD

            try
            {
                return Task.FromResult(
                    Result<IEnumerable<User>>.Success(_dbConnectionManager.ExecuteQuery<User>(query))
                );
            }
            catch (Exception ex)
            {
                return Task.FromResult(
                    Result<IEnumerable<User>>.Failure(ex.Message)
                );
            }
        }

        public Task<Result<int>> Insert(User model)
=======
            try
            {
                
                return Result<IEnumerable<User>>.Success(_dbConnectionManager.ExecuteQuery<User>(query));
            }
            catch(Exception ex)
            {
                return Result<IEnumerable<User>>.Failure(ex.Message);
            }
            
        }

        public async Task<Result<int>> Insert(User model)
>>>>>>> AnalisisSonarEstablishment
        {
            string query = @"
                INSERT INTO user
                (
                    username,
                    password_hash,
                    first_name,
                    first_last_name,
                    email,
                    role,
                    created_by,
                    created_date,
                    last_update,
                    status,
                    first_login
                )
                VALUES
                (
                    @Username,
                    @PasswordHash,
                    @FirstName,
                    @LastName,
                    @Email,
                    @Role,
                    @CreatedBy,
                    CURRENT_TIMESTAMP,
                    CURRENT_TIMESTAMP,
                    @Status,
                    @FirstLogin
                );";
<<<<<<< HEAD

            try
            {
                return Task.FromResult(
                    Result<int>.Success(_dbConnectionManager.ExecuteParameterizedNonQuery(query, model))
                );
            }
            catch (Exception e)
            {
                return Task.FromResult(
                    Result<int>.Failure(e.Message)
                );
            }
        }

        public Task<Result<int>> Update(User model)
=======
            try
            {
                return Result<int>.Success(_dbConnectionManager.ExecuteParameterizedNonQuery(query, model));
            }
            catch (Exception e)
            {
                return Result<int>.Failure(e.Message);
            }
        }
        public async Task<Result<int>> Update(User model)
>>>>>>> AnalisisSonarEstablishment
        {
            string query = @"
                UPDATE user
                SET
                    username      = @Username,
                    password_hash = @PasswordHash,
                    first_name    = @FirstName,
<<<<<<< HEAD
                    first_last_name = @LastName,
                    email         = @Email,
                    role          = @Role,
                    created_by    = @CreatedBy,
                    first_login   = @FirstLogin,
                    last_update   = CURRENT_TIMESTAMP
                WHERE id = @Id;";

            try
            {
                return Task.FromResult(
                    Result<int>.Success(_dbConnectionManager.ExecuteParameterizedNonQuery(query, model))
                );
            }
            catch (Exception e)
            {
                return Task.FromResult(
                    Result<int>.Failure(e.Message)
                );
            }
        }

        public Task<Result<IEnumerable<User>>> Search(string property)
=======
                    first_last_name     = @LastName,
                    email         = @Email,
                    role         = @Role,
                    created_by    = @CreatedBy,
                    first_login    = @FirstLogin,
                    last_update   = CURRENT_TIMESTAMP
                WHERE id = @Id;";
            try
            {
                return Result<int>.Success(_dbConnectionManager.ExecuteParameterizedNonQuery(query, model));
            }
            catch (Exception e)
            {
                return Result<int>.Failure(e.Message);
            }
            
        }

        public async Task<Result<IEnumerable<User>>> Search(string property)
>>>>>>> AnalisisSonarEstablishment
        {
            var probe = new User
            {
                Username = property,
                FirstName = property,
                SecondName = property,
                LastName = property,
                SecondLastName = property,
                Email = property,
                Role = property
            };

            const string query = @"
                SELECT
<<<<<<< HEAD
                    id,
                    username,
                    password_hash,
                    first_name,
                    first_last_name,
                    email,
                    role,
                    created_by,
                    created_date,
                    last_update,
                    status,
=======
                    id            ,
                    username      ,
                    password_hash ,
                    first_name    ,
                    first_last_name     ,
                    email         ,
                    role          ,
                    created_by    ,
                    created_date  ,
                    last_update   ,
                    status        ,
>>>>>>> AnalisisSonarEstablishment
                    first_login
                FROM user
                WHERE status = TRUE AND (
                    (@Username IS NOT NULL AND username LIKE CONCAT('%', @Username, '%')) OR
                    (@FirstName IS NOT NULL AND first_name LIKE CONCAT('%', @FirstName, '%')) OR
                    (@LastName IS NOT NULL AND first_last_name LIKE CONCAT('%', @LastName, '%')) OR
                    (@Email IS NOT NULL AND email LIKE CONCAT('%', @Email, '%')) OR
                    (@Role IS NOT NULL AND role LIKE CONCAT('%', @Role, '%'))
                )
                ORDER BY id DESC;";
<<<<<<< HEAD

            try
            {
                return Task.FromResult(
                    Result<IEnumerable<User>>.Success(_dbConnectionManager.ExecuteParameterizedQuery<User>(query, probe))
                );
            }
            catch (Exception e)
            {
                return Task.FromResult(
                    Result<IEnumerable<User>>.Failure(e.Message)
                );
            }
        }

        public Task<Result<User?>> GetById(int id)
=======
            try
            {
                return Result<IEnumerable<User>>.Success(_dbConnectionManager.ExecuteParameterizedQuery<User>(query, probe));
            }
            catch (Exception e)
            {
                return Result<IEnumerable<User>>.Failure(e.Message);
            }
        }

        public async Task<Result<User?>> GetById(int id)
>>>>>>> AnalisisSonarEstablishment
        {
            string query = @"
                SELECT
                    id            AS Id,
                    username      AS Username,
                    password_hash AS PasswordHash,
                    first_name    AS FirstName,
<<<<<<< HEAD
                    first_last_name AS LastName,
=======
                    first_last_name     AS LastName,
>>>>>>> AnalisisSonarEstablishment
                    email         AS Email,
                    role          AS Role,
                    created_by    AS CreatedBy,
                    created_date  AS CreatedDate,
                    last_update   AS LastUpdate,
                    status        AS Status,
                    first_login   AS FirstLogin
                FROM user
                WHERE id = @Id AND status = TRUE
                LIMIT 1;";

            var model = new User { Id = id };
<<<<<<< HEAD

            try
            {
                var res = Result<User?>.Success(
                    _dbConnectionManager.ExecuteParameterizedQuery<User>(query, model).FirstOrDefault()
                );

                return Task.FromResult(res);
            }
            catch (Exception e)
            {
                return Task.FromResult(
                    Result<User?>.Failure(e.Message)
                );
            }
        }

        public Task<Result<User?>> GetByUsername(string username)
=======
            try
            {
                var res = Result<User?>.Success(_dbConnectionManager.ExecuteParameterizedQuery<User>(query, model).FirstOrDefault());
                return res;
            }
            catch (Exception e)
            {
                return Result<User?>.Failure(e.Message);
            }
        }

        public async Task<Result<User?>> GetByUsername(string username)
>>>>>>> AnalisisSonarEstablishment
        {
            const string sql = @"
                SELECT
                    id            AS Id,
                    username      AS Username,
                    password_hash AS PasswordHash,
                    first_name    AS FirstName,
<<<<<<< HEAD
                    first_last_name AS LastName,
=======
                    first_last_name     AS LastName,
>>>>>>> AnalisisSonarEstablishment
                    email         AS Email,
                    role          AS Role,
                    created_by    AS CreatedBy,
                    created_date  AS CreatedDate,
                    last_update   AS LastUpdate,
                    status        AS Status,
                    first_login   AS FirstLogin
                FROM user
                WHERE username = @Username
                LIMIT 1;";
<<<<<<< HEAD

            var probe = new User { Username = username };

            try
            {
                return Task.FromResult(
                    Result<User?>.Success(
                        _dbConnectionManager.ExecuteParameterizedQuery<User>(sql, probe).FirstOrDefault()
                    )
                );
            }
            catch (Exception e)
            {
                return Task.FromResult(
                    Result<User?>.Failure(e.Message)
                );
            }
        }

        public Task<Result<int>> Delete(int id)
=======
            var probe = new User { Username = username };
            try
            {
                return Result<User?>.Success(_dbConnectionManager.ExecuteParameterizedQuery<User>(sql, probe).FirstOrDefault());

            }
            catch (Exception e)
            {
                return Result<User?>.Failure(e.Message);
            }
        }
        public async Task<Result<int>> Delete(int id)
>>>>>>> AnalisisSonarEstablishment
        {
            string query = @"
                UPDATE user
                SET last_update = CURRENT_TIMESTAMP,
                    status = FALSE
                WHERE id = @Id;";
<<<<<<< HEAD

            try
            {
                return Task.FromResult(
                    Result<int>.Success(
                        _dbConnectionManager.ExecuteParameterizedNonQuery(query, new IdDto { Id = id })
                    )
                );
            }
            catch (Exception e)
            {
                return Task.FromResult(
                    Result<int>.Failure(e.Message)
                );
            }
        }
    }
}
=======
            try
            {
                return Result<int>.Success(_dbConnectionManager.ExecuteParameterizedNonQuery(query, new IdDto() { Id = id}));
            }
            catch (Exception e)
            {
                return Result<int>.Failure(e.Message);
            }
        }
    }
}
>>>>>>> AnalisisSonarEstablishment
