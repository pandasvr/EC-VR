using System;

[Serializable]
public class User
{
    public string idUser {get;set;}
    public string userName { get; set; }
    public string cryptPassword { get; set; }
    public string userEmail { get; set; }
    public string userLevel { get; set; }
    public string labelUserLevel { get; set; }
    public string userFirstName { get; set; }
    public string userLastName { get; set; }
}
