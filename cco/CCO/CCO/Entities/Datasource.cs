﻿namespace CCO.Entities
{
    public class Datasource
    {
        //public string Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConnectionString { get; set; }

        public Datasource(string type, string url, string username, string password, string connectionString)
        {
            Type = type;
            Url = url;
            Username = username;
            Password = password;
            ConnectionString = connectionString;
        }
    }
}
