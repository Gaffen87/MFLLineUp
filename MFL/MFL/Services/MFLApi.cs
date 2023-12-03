using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MFL.Services;

public class MFLApi
{
	HttpClient _httpClient;
	HttpClientHandler _handler;
	CookieContainer _cookieContainer;
	public MFLApi()
	{
		_cookieContainer = new CookieContainer();
		_handler = new HttpClientHandler();
		_handler.CookieContainer = _cookieContainer;
		_httpClient = new(_handler);
	}

	public async Task<bool> ValidateUser(string email, string password)
	{
		bool result = false;

		StringContent content = new("");
		Uri uri = new($"https://api.myfantasyleague.com/2023/login?USERNAME={email}&PASSWORD={password}&XML=1");
		var response = await _httpClient.PostAsync(uri, content);

		var cookies = _cookieContainer.GetCookies(uri);

		if (cookies.Count > 0) 
		{
			Uri domain = await GetDomain();
			result = true;
			foreach ( Cookie cookie in cookies )
			{
				 _cookieContainer.Add(domain, new Cookie(cookie.Name, cookie.Value));
			}
		}

		return result;
	}

	private async Task<Uri> GetDomain()
	{
		JObject myLeagues = JObject.Parse(await _httpClient.GetStringAsync("https://api.myfantasyleague.com/2023/export?TYPE=myleagues&YEAR=&FRANCHISE_NAMES=&JSON=1"));
		var result = myLeagues["leagues"]["league"]["url"].ToString();

		return new Uri(result);
	}

	public async Task<string> GetFranchiseID()
	{
		JObject myLeagues = JObject.Parse(await _httpClient.GetStringAsync("https://api.myfantasyleague.com/2023/export?TYPE=myleagues&YEAR=&FRANCHISE_NAMES=&JSON=1"));
		var result = myLeagues["leagues"]["league"].ToList();
		try
		{
			var franchiseId = result.FirstOrDefault(x => x["name"].ToString() == "Ikast Dynasty League");
			return franchiseId["franchise_id"].ToString();
		}
		catch (Exception)
		{
			return myLeagues["leagues"]["league"]["franchise_id"].ToString();
		}
		
	}

}
