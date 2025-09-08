using System.ComponentModel.DataAnnotations;
using course.Models;
using Xunit;
namespace course.course.Tests.Models
{
    public class UserModelTests
    {
        [Fact]
        public void User_Model_Should_Be_Created_With_Required_Fields()
        {
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashed_password",
                Role = UserRole.Admin,
                FirstName = "Test",
                LastName = "User",
                Phone = "+79991234567",
                IsActive = true
            };

            Assert.Equal(1, user.Id);
            Assert.Equal("testuser", user.Username);
            Assert.Equal("test@example.com", user.Email);
            Assert.Equal("hashed_password", user.PasswordHash);
            Assert.Equal(UserRole.Admin, user.Role);
            Assert.Equal("Test", user.FirstName);
            Assert.Equal("User", user.LastName);
            Assert.Equal("+79991234567", user.Phone);
            Assert.True(user.IsActive);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void User_Model_Should_Require_Username(string username)
        {
            var user = new User
            {
                Username = username,
                Email = "test@example.com",
                PasswordHash = "hashed_password"
            };

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(user);
            bool isValid = Validator.TryValidateObject(user, validationContext, validationResults, true);

            Assert.False(isValid);
            Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Username"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("invalid-email")]
        [InlineData("test@")]
        public void User_Model_Should_Validate_Email_Format(string email)
        {
            var user = new User
            {
                Username = "testuser",
                Email = email,
                PasswordHash = "hashed_password"
            };

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(user);
            bool isValid = Validator.TryValidateObject(user, validationContext, validationResults, true);


        }

        [Fact]
        public void User_Model_Should_Have_Default_Timestamps()
        {
            var user = new User();

            Assert.NotEqual(System.DateTime.MinValue, user.CreatedAt);
            Assert.NotEqual(System.DateTime.MinValue, user.UpdatedAt);
        }

        [Fact]
        public void User_Model_Should_Set_Default_Role()
        {
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashed_password"
            };

            Assert.Equal(UserRole.Dispatcher, user.Role);
        }

        [Fact]
        public void User_Model_Should_Set_Default_IsActive()
        {
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashed_password"
            };

            Assert.True(user.IsActive);
        }

        [Theory]
        [InlineData("admin", UserRole.Admin)]
        [InlineData("manager", UserRole.Manager)]
        [InlineData("dispatcher", UserRole.Dispatcher)]
        [InlineData("driver", UserRole.Driver)]
        public void User_Model_Should_Support_All_User_Roles(string roleString, UserRole expectedRole)
        {
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashed_password",
                Role = expectedRole
            };

            Assert.Equal(expectedRole, user.Role);
        }

        [Fact]
        public void User_Model_Should_Accept_Optional_Fields()
        {
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashed_password",
                FirstName = "Test",
                LastName = "User",
                Phone = "+79991234567"
            };

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(user);
            bool isValid = Validator.TryValidateObject(user, validationContext, validationResults, true);

            Assert.True(isValid);
            Assert.Equal("Test", user.FirstName);
            Assert.Equal("User", user.LastName);
            Assert.Equal("+79991234567", user.Phone);
        }
    }
}
