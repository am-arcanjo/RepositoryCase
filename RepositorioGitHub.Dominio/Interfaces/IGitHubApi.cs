using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorioGitHub.Dominio.Interfaces
{
    public interface IGitHubApi
    {
        ActionResult<List<GitHubRepository>> GetRepositoryByOwner(string owner);
        ActionResult<GitHubRepository> GetRepositoryByName(string username, string repoName);     
    }
}
