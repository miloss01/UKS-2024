import psycopg2
from passlib.context import CryptContext
from config import DATABASE_URI

pwd_context = CryptContext(schemes=["pbkdf2_sha256"], deprecated="auto")

class AccessService:
    def __init__(self):
        self.connection_string = DATABASE_URI

    def authenticate_user(self, username: str, password: str) -> str | None:
        """Returns user id if authentication is correct, otherwise returns None"""
        connection = None
        cursor = None
        try:
            # Establish the connection
            connection = psycopg2.connect(self.connection_string)
            cursor = connection.cursor()

            query = """
                SELECT Id, Username, Password
                FROM Users
                WHERE Username = %s;
            """

            # Execute the query with parameters
            cursor.execute(query, (username,))
            user = cursor.fetchone()

            # If user not found, return False
            if not user:
                return None

            # Get the stored password hash
            stored_password_hash = user[2]

            # Compare the provided password with the stored hash using passlib
            if pwd_context.verify(password, stored_password_hash):
                return user[0]
            else:
                return None

        except Exception as e:
            raise e
        finally:
            # Ensure the cursor and connection are closed properly
            if cursor:
                cursor.close()
            if connection:
                connection.close()

    def check_pull_permission(self, repository_name: str, user_id: str) -> bool:
        connection = None
        cursor = None
        try:
            # Establish the connection
            connection = psycopg2.connect(self.connection_string)
            cursor = connection.cursor()

            query = """
                SELECT 1
                FROM DockerRepositories AS r
                WHERE r.Name = %s
                  AND (r.IsPublic = true
                       OR r.UserOwnerId = %s
                       OR EXISTS (
                           SELECT 1
                           FROM OrganizationMembers
                           WHERE OrganizationId = r.Id AND MemberId = %s
                       ))
                LIMIT 1;
            """

            # Execute the query with parameters
            cursor.execute(query, (repository_name, user_id, user_id))

            # Fetch the result
            result = cursor.fetchone()

            return result is not None

        except Exception as e:
            raise e
        finally:
            if cursor:
                cursor.close()
            if connection:
                connection.close()

    def check_push_permission(self, repository_name: str, user_id: str) -> bool:
        connection = None
        cursor = None
        try:
            # Establish the connection
            connection = psycopg2.connect(self.connection_string)
            cursor = connection.cursor()

            query = """
                SELECT 1
                FROM DockerRepositories AS r
                WHERE r.Name = %s
                AND (
                    r.UserOwnerId = %s
                    OR EXISTS (
                        SELECT 1
                        FROM TeamPermissions
                        WHERE permission IN ('ReadWrite', 'Admin')
                            AND RepositoryId = r.Id
                    )
                )
                LIMIT 1;
            """

            # Execute the query with parameters
            cursor.execute(query, (repository_name, user_id, user_id))

            # Fetch the result
            result = cursor.fetchone()

            return result is not None

        except Exception as e:
            raise e
        finally:
            if cursor:
                cursor.close()
            if connection:
                connection.close()        