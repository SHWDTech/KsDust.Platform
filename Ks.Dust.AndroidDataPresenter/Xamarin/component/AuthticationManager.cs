using System;
using Android.Content;
using ApplicationConcept;
using Ks.Dust.AndroidDataPresenter.Xamarin.consts;
using Newtonsoft.Json.Linq;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.component
{
    public delegate void RefreshTokenFinished(AuthticationEventArgs args);

    public delegate void LoginFinished(AuthticationEventArgs args);

    public class AuthticationEventArgs : EventArgs
    {
        public string Message { get; set; }

        public Exception Exception { get; set; }

        public bool AuthSuccess { get; set; }

        public AuthticationResult Result { get; set; }
    }

    public class AuthticationManager
    {
        private const string SpNameRefreshToken = "refreshToken";

        private const string SpNameTokenExpireTime = "TokenExpireTime";

        private readonly ISharedPreferences _sharedPreferences;

        public event RefreshTokenFinished OnRefreshTokenFinished;

        public event LoginFinished OnLoginFinished;

        public string AccessToken { get; private set; }

        #region loginConst

        private const string ErrorUnSupportedGrantType = "unsupported_grant_type";

        private const string ErrorInvaliGgrant = "invalid_grant";

        private const string ErrorInvalidClientId = "invalid_clientId";

        #endregion

        public static AuthticationManager Instance { get; private set; }

        public static void Init(Context context)
        {
            if (Instance != null) return;
            Instance = new AuthticationManager(context);
        }

        private AuthticationManager(Context context)
        {
            _sharedPreferences = context.GetSharedPreferences(ActivityConts.KsDustAndroidSharedPreferenceName, FileCreationMode.Private);
        }

        public bool IsLoginAndTokenValid()
        {
            var token = _sharedPreferences.GetString(SpNameRefreshToken, string.Empty);
            if (string.IsNullOrWhiteSpace(token)) return false;

            DateTime.TryParse(_sharedPreferences.GetString(SpNameTokenExpireTime, string.Empty), out DateTime expireTime);
            var currentTime = DateTime.Now;
            return expireTime >= currentTime;
        }

        public void UpdateAccessTokenByRefreshToken()
        {
            var refreshToken = _sharedPreferences.GetString(SpNameRefreshToken, string.Empty);
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                RefreshTokenFinished(new AuthticationEventArgs
                {
                    AuthSuccess = false,
                    Message = "RefreshToken Is Empty",
                    Result = AuthticationResult.EmptyRefreshToken
                });
                return;
            }

            var handler = new HttpResponseHandler();
            handler.OnResponse += args =>
            {
                var jsonObj = JObject.Parse(args.Response);
                var authError = jsonObj["error"]?.ToString();
                if (authError!= null && authError == ErrorInvaliGgrant)
                {
                    RefreshTokenFinished(new AuthticationEventArgs
                    {
                        AuthSuccess = false,
                        Message = "AuthticationError",
                        Result = AuthticationResult.AuthticationError
                    });
                    return;
                }
                AccessToken = jsonObj["access_token"]?.ToString();
                if (string.IsNullOrWhiteSpace(AccessToken))
                {
                    RefreshTokenFinished(new AuthticationEventArgs
                    {
                        AuthSuccess = false,
                        Message = "Get AccessToken Failed",
                        Result = AuthticationResult.InvalidAccessToken
                    });
                    return;
                }
                refreshToken = jsonObj["refresh_token"]?.ToString();
                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    RefreshTokenFinished(new AuthticationEventArgs
                    {
                        AuthSuccess = false,
                        Message = "Get AccessToken Failed",
                        Result = AuthticationResult.InValidRefreshToken
                    });
                    return;
                }

                var editor = _sharedPreferences.Edit();
                editor.PutString(SpNameRefreshToken, refreshToken);
                var expireTime = $"{DateTime.Parse(jsonObj[".expires"]?.ToString() ?? "2000-01-01"): yyyy-MM-dd HH:mm:ss}";
                editor.PutString(SpNameTokenExpireTime, expireTime);
                editor.Apply();
                RefreshTokenFinished(new AuthticationEventArgs
                {
                    AuthSuccess = true,
                    Message = "refreshToken successed",
                    Result = AuthticationResult.RefreshTokenSuccessed
                });
            };
            handler.OnError += args =>
            {
                RefreshTokenFinished(new AuthticationEventArgs
                {
                    AuthSuccess = false,
                    Message = "refreshToken Exception",
                    Exception = args.Exception,
                    Result = AuthticationResult.RefreshTokenException
                });
            };
            ApiManager.GetTokenByRefreshToken(refreshToken, handler);
        }

        public void Login(string username, string password)
        {
            var handler = new HttpResponseHandler();
            handler.OnResponse += args =>
            {
                var jsonObj = JObject.Parse(args.Response);
                var authError = jsonObj["error"]?.ToString();
                if (authError != null && (authError == ErrorInvaliGgrant || authError == ErrorUnSupportedGrantType || authError == ErrorInvalidClientId))
                {
                    LoginFinished(new AuthticationEventArgs
                    {
                        AuthSuccess = false,
                        Message = "AuthticationError",
                        Result = AuthticationResult.AuthticationError
                    });
                    return;
                }
                AccessToken = jsonObj["access_token"]?.ToString();
                if (string.IsNullOrWhiteSpace(AccessToken))
                {
                    LoginFinished(new AuthticationEventArgs
                    {
                        AuthSuccess = false,
                        Message = "Get AccessToken Failed",
                        Result = AuthticationResult.InvalidAccessToken
                    });
                    return;
                }
                var refreshToken = jsonObj["refresh_token"]?.ToString();
                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    LoginFinished(new AuthticationEventArgs
                    {
                        AuthSuccess = false,
                        Message = "Get AccessToken Failed",
                        Result = AuthticationResult.InValidRefreshToken
                    });
                    return;
                }

                var editor = _sharedPreferences.Edit();
                editor.PutString(SpNameRefreshToken, refreshToken);
                var expireTime = $"{DateTime.Parse(jsonObj[".expires"]?.ToString() ?? "2000-01-01"): yyyy-MM-dd HH:mm:ss}";
                editor.PutString(SpNameTokenExpireTime, expireTime);
                editor.Apply();
                LoginFinished(new AuthticationEventArgs
                {
                    AuthSuccess = true,
                    Message = "login successed",
                    Result = AuthticationResult.LoginSuccessed
                });
            };
            handler.OnError += args =>
            {
                LoginFinished(new AuthticationEventArgs
                {
                    AuthSuccess = false,
                    Message = "refreshToken Exception",
                    Exception = args.Exception,
                    Result = AuthticationResult.LoginException
                });
            };
            ApiManager.GetTokenByPassword(username, password, handler);
        }

        private void RefreshTokenFinished(AuthticationEventArgs args)
        {
            OnRefreshTokenFinished?.Invoke(args);
        }

        private void LoginFinished(AuthticationEventArgs args)
        {
            OnLoginFinished?.Invoke(args);
        }

        public void Logout() => _sharedPreferences.Edit().Clear().Apply();
    }
}