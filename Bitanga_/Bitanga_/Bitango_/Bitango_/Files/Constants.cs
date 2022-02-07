using System;
namespace Bitango_.Files
{
    public class Constants
    {
		public static string AppName = "Bitanga";

		//-----------------------------------Google-----------------------------------
		public static string iOSClientId = "28153154759-fi597di3ok4fqsudvmbscuk5a9dfe3cn.apps.googleusercontent.com";
		public static string AndroidClientId = "28153154759-uhprie5ft9h9ofj94bk5k2fcn92gv4ov.apps.googleusercontent.com";

		public static string Scope = "https://www.googleapis.com/auth/userinfo.email";
		public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
		public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
		public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

		public static string iOSRedirectUrl = "com.googleusercontent.apps.28153154759-fi597di3ok4fqsudvmbscuk5a9dfe3cn:/oauth2redirect";
		public static string AndroidRedirectUrl = "com.googleusercontent.apps.28153154759-uhprie5ft9h9ofj94bk5k2fcn92gv4ov:/oauth2redirect";

		//-----------------------------------Facebook-----------------------------------
		public static string FacebookiOSClientId = "1195234190841618";
		public static string FacebookAndroidClientId = "1195234190841618";

		public static string FacebookScope = "email";
		public static string FacebookAuthorizeUrl = "https://www.facebook.com/dialog/oauth/";
		public static string FacebookAccessTokenUrl = "https://www.facebook.com/connect/login_success.html";
		public static string FacebookUserInfoUrl = "https://graph.facebook.com/me?fields=email&access_token={accessToken}";

		// Set these to reversed iOS/Android client ids, with :/oauth2redirect appended
		public static string FacebookiOSRedirectUrl = "fb1195234190841618:/oauth2redirect";
		public static string FacebookAndroidRedirectUrl = "fb1195234190841618:/oauth2redirect";
	}
}
