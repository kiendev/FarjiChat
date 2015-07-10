using System;
using System.Net;

namespace ChatRooms.Helper
{
    public class NetworkExceptionEx : Exception
    {
        private HttpStatusCode _errorCode;
        private string _messageEx;

        public string MessageEx
        {
            get { return _messageEx; }
            set { _messageEx = value; }
        }

        public HttpStatusCode ErrorCode
        {
            get { return _errorCode; }
        }

        public NetworkExceptionEx(HttpStatusCode errocode, string message)
            : base(message)
        {
            MessageEx = message;
            _errorCode = errocode;
        }

        public NetworkExceptionEx(HttpStatusCode errocode)
        {
            //_messageEx = errocode.ToString(); 
            //AnhTT87: Hardcode
            _messageEx = "Network Error...";
            _errorCode = errocode;
        }
    }
}
