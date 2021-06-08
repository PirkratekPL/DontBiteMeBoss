using System;

namespace DontBiteMeBoss.Core
{
    public class User
    { 
        private string _username;
        private long _highscore;

        public User(string username, long highscore)
        {
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
