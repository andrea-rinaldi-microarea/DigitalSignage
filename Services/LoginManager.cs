using DigitalSignage.Models;
using Microsoft.Extensions.Options;

namespace DigitalSignage.Services  
{  
    public class LoginManager
    {  
        public string authenticationToken { get; set; } = "";  
        public string connectionString { get; set; } = "";  

        private IOptions<ConnectInfo> _connectInfo { get; } 
               
        public LoginManager(
            IOptions<ConnectInfo> connectInfo
        )  
        {  
            _connectInfo = connectInfo;
        }  

        public void Reset()
        {
            authenticationToken = "";
            connectionString = "";
        }

        public M4LoginManager.MicroareaLoginManagerSoapClient getLoginManager()
        {
            M4LoginManager.MicroareaLoginManagerSoapClient m4Login = new M4LoginManager.MicroareaLoginManagerSoapClient(M4LoginManager.MicroareaLoginManagerSoapClient.EndpointConfiguration.MicroareaLoginManagerSoap);

            m4Login.Endpoint.Address = new System.ServiceModel.EndpointAddress($"http://{_connectInfo.Value.Server}/{_connectInfo.Value.Instance}/LoginManager/LoginManager.asmx");
            
            return m4Login;
        }

        public bool IsConnected()
        {
            return authenticationToken.Length > 0;
        }

        public string GetConnectionString()  
        {  
            if (!IsConnected())
                return ""; // todo exception
                
            if (connectionString == string.Empty)
            {
                try 
                {
                    using (M4LoginManager.MicroareaLoginManagerSoapClient m4Login = getLoginManager())
                    {
                        M4LoginManager.GetLoginInformationResponse info = m4Login.GetLoginInformationAsync(new M4LoginManager.GetLoginInformationRequest(authenticationToken)).Result;
                        connectionString = info.nonProviderCompanyConnectionString;
                    }
                }
                catch (System.Exception)
                {
                    //@@TODO errori
                }
            }

            return connectionString;
        } 

    }
}