using System.ComponentModel;

namespace FirebaseAuthenticationDotNetCore.Website.Models;

public class UserDetails
{
    public string Name { get; set; }

    [DisplayName("Email Address")]
    public string EmailAddress { get; set; }
    
    [DisplayName("Phone Number")]
    public string PhoneNumber { get; set; }
}