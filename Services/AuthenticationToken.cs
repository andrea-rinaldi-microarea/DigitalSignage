namespace DigitalSignage.Services  
{  
    public class AuthenticationToken
    {  
        private string token = "";  
        public AuthenticationToken()  
        {  
        }  
        public string Get()  
        {  
            return token;  
        } 
        public void Set(string newToken)
        {
            token = newToken;
        }
        public void Reset()
        {
            token = "";
        }
        public bool IsValid()
        {
            return token.Length > 0;
        }
    }  
}  