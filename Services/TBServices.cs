using DigitalSignage.Models;
using Microsoft.Extensions.Options;
using M4TbServices;
using System.IO;

namespace DigitalSignage.Services  
{  
    public class TbServices
    {  
        private IOptions<ConnectInfo> _connectInfo { get; } 
        private LoginManager _loginManager { get; }               

        public TbServices(
            IOptions<ConnectInfo> connectInfo,
            LoginManager loginManager
        )  
        {  
            _connectInfo = connectInfo;
            _loginManager = loginManager;
        }  

        public TbServicesSoapClient getTbServices()
        {
            TbServicesSoapClient m4Tb = new TbServicesSoapClient(TbServicesSoapClient.EndpointConfiguration.TbServicesSoap);

            m4Tb.Endpoint.Address = new System.ServiceModel.EndpointAddress($"http://{_connectInfo.Value.Server}/{_connectInfo.Value.Instance}/TbServices/TbServices.asmx");
            
            return m4Tb;
        }

        public bool downloadImage(string imgNamespace, string toPath)
        {
            using (TbServicesSoapClient m4Tb = getTbServices())
            {
                try 
                {
                    GetFileStreamResponse response = m4Tb.GetFileStreamAsync(
                        _loginManager.authenticationToken,
                        imgNamespace,
                        string.Empty,
                        _connectInfo.Value.Company
                    ).Result;
                    if (response.Body.GetFileStreamResult != null)
                    {
                        if (!Directory.Exists(toPath))
                            Directory.CreateDirectory(toPath);
                        File.WriteAllBytes(Path.Combine(toPath, imgNamespace), response.Body.GetFileStreamResult);
                        return true;
                    }
                    return false;
                }
                catch(System.Exception e)
                {
                    throw(e);
                }
            }
        } 
    }
}