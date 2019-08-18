namespace OpenDentBusiness
{
    public class CentralConnection
    {
        public string ServerName { get; set; }

        public string DatabaseName { get; set; }
        
        public string MySqlUser { get; set; }

        public string MySqlPassword { get; set; }

        public string ServiceURI { get; set; }

        public bool IsAutomaticLogin { get; set; }
    }
}