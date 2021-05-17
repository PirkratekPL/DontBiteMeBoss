using System;

namespace DontBiteMeBoss.Core
{
    public class User
    {
        private Guid _guid; //unique in all database, used to identify users
        private string _username;
        private long _highscore;

        public User(Guid guid, string username, long highscore)
        {
            _guid = guid;
            _username = username;
            _highscore = highscore;
        }

        ///////////////////////
        //Getters and setters//
        ///////////////////////
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public long Highscore
        {
            get { return _highscore; }
            set { _highscore = value; }
        }
    }
}
