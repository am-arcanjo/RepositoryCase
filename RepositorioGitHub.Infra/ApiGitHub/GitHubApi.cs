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

        public ActionResult<GitHubRepository> GetRepositoryByOwner(string owner)
        {
            var request = new RestRequest($"users/{owner}/repos");
            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                var repositories = JsonConvert.DeserializeObject<List<GitHubRepository>>(response.Content);
                return new ActionResult<GitHubRepository>(repositories.FirstOrDefault(), response.StatusCode == HttpStatusCode.OK);
            }
            else
            {
                return new ActionResult<GitHubRepository>(null, false);
            }
        }

        public ActionResult<RepositoryModel> GetRepositoryByName(string name)
        {
            var request = new RestRequest($"search/repositories?q={name}");
            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                var model = JsonConvert.DeserializeObject<RepositoryModel>(response.Content);
                if (!model.IncompleteResults && model.Repositories.Any())
                {
                    return new ActionResult<RepositoryModel>(model, response.StatusCode == HttpStatusCode.OK);
                }
                else if (!model.IncompleteResults)
                {
                    return new ActionResult<RepositoryModel>(null, false);
                }
            }

            return new ActionResult<RepositoryModel>(null, false);
        }
    }
}
