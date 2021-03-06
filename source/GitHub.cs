﻿using System;
using System.Net.Http;
using System.Xml.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace keep.grass
{
	public class GitHub
	{
		static private string BaseUrl = "https://github.com";

		static private string ProfileUrlFormat = BaseUrl +"/{0}";
		static public string GetProfileUrl(string Id)
		{
			return String.Format(ProfileUrlFormat, Id);
		}

		static private string AcitivityUrlFormat = BaseUrl + "/{0}?tab=activity";
		static public string GetAcitivityUrl(string Id)
		{
			return String.Format(AcitivityUrlFormat, Id);
		}

		static private string AtomUrlFormat = BaseUrl + "/{0}.atom";
		static public string GetAtomUrl(string Id)
		{
			return String.Format (AtomUrlFormat, Id);
		}

		static private string IconUrlFormat = BaseUrl + "/{0}.png";
		static public string GetIconUrl(string Id)
		{
			return String.Format (IconUrlFormat, Id);
		}

		static public async Task<DateTime> GetLastPublicActivityAsync(HttpClient HttpClient, string Id)
		{
			using (var response = await HttpClient.GetAsync(GetAtomUrl(Id)))
			{
				using (var content = response.Content)
				{
					var stream = await content.ReadAsStreamAsync();
					return await Task.Factory.StartNew<DateTime>
					(
						() =>
						DateTime.Parse
						(
							XDocument
							.Load(stream)
							.Descendants()
							.Where(i => i.Name.LocalName == "updated")
							.First()
							.Value
						)
					);
				}
			}
		}
	}
}

