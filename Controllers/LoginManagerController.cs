using System.Threading.Tasks;
using DigitalSignage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using DigitalSignage.Services;

namespace DigitalSignage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginManagerController : ControllerBase
    {
        private IOptions<ConnectInfo> _connectInfo { get; }
        private AuthenticationToken _authenticationToken { get; }

        public LoginManagerController(
                IOptions<ConnectInfo> connectInfo,
                AuthenticationToken authenticationToken
            )
        {
            _connectInfo = connectInfo;
            _authenticationToken = authenticationToken;
        }

        private M4LoginManager.MicroareaLoginManagerSoapClient getLoginManager()
        {
            M4LoginManager.MicroareaLoginManagerSoapClient m4Login = new M4LoginManager.MicroareaLoginManagerSoapClient(M4LoginManager.MicroareaLoginManagerSoapClient.EndpointConfiguration.MicroareaLoginManagerSoap);

            m4Login.Endpoint.Address = new System.ServiceModel.EndpointAddress($"http://{_connectInfo.Value.Server}/{_connectInfo.Value.Instance}/LoginManager/LoginManager.asmx");
            
            return m4Login;
        }

        private ContentResult loginError(string message)
        {
            string context = $"Error logging in to Mago instance {_connectInfo.Value.Instance} on server {_connectInfo.Value.Server} with user {_connectInfo.Value.User} on company {_connectInfo.Value.Company}\n";
            ContentResult err = Content(context + message);
            err.StatusCode = 500;
            return err;
        }

        [HttpGet("isAlive")]
        public async Task<ActionResult<bool>> IsAlive()
        {
            try 
            {
                using (M4LoginManager.MicroareaLoginManagerSoapClient m4Login = getLoginManager())
                {
                    var data = await m4Login.IsAliveAsync();
                    return data;
                }
            }
            catch (System.Exception ex)
            {
                ContentResult err = Content(ex.Message);
                err.StatusCode = 500;
                return err;
            }
        }

        [HttpGet("enumCompanies")]
        public async Task<ActionResult<string[]>> EnumCompanies(string user)
        {
            try 
            {
                using (M4LoginManager.MicroareaLoginManagerSoapClient m4Login = getLoginManager())
                {
                    var data = await m4Login.EnumCompaniesAsync(user);
                    return data;
                }
            }
            catch (System.Exception ex)
            {
                ContentResult err = Content(ex.Message);
                err.StatusCode = 500;
                return err;
            }
        }

        [HttpGet("getConnectionInfo")]
        public ActionResult<ConnectInfo> GetConnectionInfo()
        {
            try 
            {
                ConnectInfo info = _connectInfo.Value;
                info.Password = "*******"; // clear out password before returning info
                return info;
            }
            catch (System.Exception ex)
            {
                ContentResult err = Content(ex.Message);
                err.StatusCode = 500;
                return err;
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<bool>> Login()
        {
            try 
            {
                using (M4LoginManager.MicroareaLoginManagerSoapClient m4Login = getLoginManager())
                {
                    M4LoginManager.LoginCompactResponse response = await m4Login.LoginCompactAsync(
                        new M4LoginManager.LoginCompactRequest(
                            _connectInfo.Value.User, 
                            _connectInfo.Value.Company,
                            _connectInfo.Value.Password, 
                            "DigitalSignage", 
                            true
                    ));
                    if (response.LoginCompactResult == 0)
                    {
                        _authenticationToken.Set(response.authenticationToken);
                        return true;
                    }
                    else
                    {
                        return loginError($"Login error number {response.LoginCompactResult}");
                    }
                }
            }
            catch (System.Exception ex)
            {
                return loginError(ex.Message);
            }
        }
        
        [HttpPost("logout")]
        public async Task<ActionResult<bool>> Logout()
        {
            if (!_authenticationToken.IsValid())
            {
                return loginError("Not logged in");
            }
            try 
            {
                using (M4LoginManager.MicroareaLoginManagerSoapClient m4Login = getLoginManager())
                {
                    await m4Login.LogOffAsync(_authenticationToken.Get());
                    _authenticationToken.Reset();
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                return loginError(ex.Message);
            }
        }
    }
}