namespace GymProject.Domain
{
    public class User
    {
        public string Username { get; set; }
        private string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }

    }
}
