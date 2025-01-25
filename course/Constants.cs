using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace course;

public class Constants
{
    public const string SecretKey = "{16A542C5-CB78-4057-963D-1D90DE97769A}";

    public const string Iss = "http://localhost:5214";

    public const string Aud = "http://localhost:5214";

    public static SymmetricSecurityKey SymmetricSecurityKey => new(Encoding.UTF8.GetBytes(SecretKey));
}