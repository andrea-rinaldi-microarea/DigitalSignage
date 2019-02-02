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
        private LoginManager _loginManager { get; }

        public LoginManagerController(
                IOptions<ConnectInfo> connectInfo,
                LoginManager loginManager
            )
        {
            _connectInfo = connectInfo;
            _loginManager = loginManager;
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
                using (M4LoginManager.MicroareaLoginManagerSoapClient m4Login = _loginManager.getLoginManager())
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
                using (M4LoginManager.MicroareaLoginManagerSoapClient m4Login = _loginManager.getLoginManager())
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
                if (_loginManager.IsConnected())
                    await Logout();
                _loginManager.Reset();
                
                using (M4LoginManager.MicroareaLoginManagerSoapClient m4Login = _loginManager.getLoginManager())
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
                        _loginManager.authenticationToken = response.authenticationToken;
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
            if (!_loginManager.IsConnected())
            {
                return loginError("Not logged in");
            }
            try 
            {
                using (M4LoginManager.MicroareaLoginManagerSoapClient m4Login = _loginManager.getLoginManager())
                {
                    await m4Login.LogOffAsync(_loginManager.authenticationToken);
                    _loginManager.Reset();
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