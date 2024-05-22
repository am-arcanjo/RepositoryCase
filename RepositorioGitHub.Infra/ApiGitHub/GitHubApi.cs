using Newtonsoft.Json;
using RepositorioGitHub.Dominio;
using RepositorioGitHub.Dominio.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RepositorioGitHub.Infra.ApiGitHub
{
    public class GitHubApi : IGitHubApi
    {

        private readonly RestClient _client;

        public GitHubApi()
        {
            _client = new RestClient("https://api.github.com/");
            _client.AddDefaultHeader("Accept", "application/vnd.github.v3+json");
        }

        public ActionResult<List<GitHubRepository>> GetRepositoryByOwner(string owner)
        {
            var request = new RestRequest($"users/{owner}/repos");
            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                var repositories = JsonConvert.DeserializeObject<List<GitHubRepository>>(response.Content);
                return new ActionResult<List<GitHubRepository>>(repositories, response.StatusCode == HttpStatusCode.OK);
            }
            else
            {
                return new ActionResult<List<GitHubRepository>>(null, false);
            }
        }

        public ActionResult<GitHubRepository> GetRepositoryByName(string username, string repoName)
        {
            var request = new RestRequest($"repos/{username}/{repoName}");
            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                var repository = JsonConvert.DeserializeObject<GitHubRepository>(response.Content);
                return new ActionResult<GitHubRepository>(repository, response.StatusCode == HttpStatusCode.OK);
            }
            else
            {
                return new ActionResult<GitHubRepository>(null, false);
            }
        }
    }
}
