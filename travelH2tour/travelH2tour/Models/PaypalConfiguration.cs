using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using PayPal.Api;

namespace travelH2tour.Models
{
	public class PaypalConfiguration
	{
		public readonly static string clientId;
		public readonly static string clientSecret;

		static PaypalConfiguration()
		{
			var config = getConfig();
			clientId = "AU19n2S0XklvY7yavq1_U_nIU8rvlpf2Ci_Bg_h5Cx5bKiME0aa_xO1iGlTcwgjaVNAuyrl7CBLKVomH";
			clientSecret = "EFRdBm2xxPwcUb2YOZjPw7JmIiJ3UcwXI9Zs_40t_QDEnKFcSny0tUMRmSaVkDjM2FGqqLFL3euIMQ6j";
		}

		private static Dictionary<string, string> getConfig()
		{
			return PayPal.Api.ConfigManager.Instance.GetProperties();
		}

		private static string getAccessToken()
		{
			string accessToken = new OAuthTokenCredential(clientId, clientSecret, getConfig()).GetAccessToken();
			return accessToken;
		}

		public static APIContext getAPIContext()
		{
			APIContext apiContext = new APIContext(getAccessToken());
			apiContext.Config = getConfig();
			return apiContext;
		}
	}
}