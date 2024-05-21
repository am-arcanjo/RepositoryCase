using RepositorioGitHub.Business;
using RepositorioGitHub.Business.Contract;
using RepositorioGitHub.Dominio;
using RepositorioGitHub.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RepositorioGitHub.Infra.ApiGitHub;

namespace RepositorioGitHub.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGitHubApiBusiness _business;
        private readonly IGitHubApi _api;
        public HomeController(IGitHubApiBusiness business, IGitHubApi api)
        {
            _business = business;
            _api = api;
        }

        public ActionResult Index()
        {
            
            var model = _business.Get();
            if (model.IsValid)
            {
                TempData["success"] = model.Message;
            }
            else
            {
                TempData["warning"] = model.Message;
            }
            
            return View(model);
        }

        public ActionResult Details(long id)
        {
         
            var model = _business.GetById(id);
            if (model.IsValid)
            {
                TempData["success"] = model.Message;
            }
            else
            {
                TempData["warning"] = model.Message;
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult GetRepositorie(string username)
        {
            RepositoryViewModel viewModel = new RepositoryViewModel();

            if (string.IsNullOrEmpty(username))
            {
                TempData["warning"] = "O campo há de ser preenchido.";
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var repositoriesResult = _api.GetRepositoryByOwner(username);

                if (!repositoriesResult.IsValid || repositoriesResult.Result == null)
                {
                    TempData["warning"] = "Erro ao recuperar repositório.";
                    return Json(viewModel, JsonRequestBehavior.AllowGet);
                }

                viewModel.Repositories = repositoriesResult.Result.ToArray();
                viewModel.TotalCount = repositoriesResult.Result.Count;

                TempData["success"] = "Repositório público recuperado com sucesso.";

                Console.WriteLine(JsonConvert.SerializeObject(viewModel));

                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao recuperar repositório: " + ex.Message);
                TempData["warning"] = "Erro ao recuperar repositório.";
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetRepositorieByName(string username, string repoName)
        {
            var result = _api.GetRepositoryByName(username, repoName);
            return Json(result.Result);
        }

        public ActionResult DetailsRepository(long id, string login)
        {
            ActionResult<GitHubRepositoryViewModel> model = new ActionResult<GitHubRepositoryViewModel>();

            if (string.IsNullOrEmpty(login) && id == 0)
            {
                return RedirectToAction("GetRepositorie");
            }
            else
            {
                
                model = _business.GetRepository(login, id);

                if (model.IsValid)
                {
                    TempData["success"] = model.Message;
                }
                else
                {
                    TempData["warning"] = model.Message;
                }
            }
            
            return View(model);
        }

        public ActionResult Favorite()
        {

            ActionResult<FavoriteViewModel> model = new ActionResult<FavoriteViewModel>();

            var response = _business.GetFavoriteRepository();
           
            model = response;

            if (model.IsValid)
            {
                TempData["success"] = model.Message;
            }
            else
            {
                TempData["warning"] = model.Message;
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult FavoriteSave(string owner, string name, string language, string lastUpdat, string description)
        {     
            ActionResult< FavoriteViewModel> model = new ActionResult<FavoriteViewModel>();

            if(string.IsNullOrEmpty(owner) && string.IsNullOrEmpty(name) && string.IsNullOrEmpty(language)
                && string.IsNullOrEmpty(lastUpdat)&& string.IsNullOrEmpty(description))
            {
                model.IsValid = false;
                model.Message = "Não foi possivel realizar esta operação";

                
                

                return Json(new
                {
                    Data = model
                }, JsonRequestBehavior.AllowGet);


            }
            else
            {
                
                FavoriteViewModel view = new FavoriteViewModel() 
                { 
                    Description = description,
                    Language = language,
                    Owner = owner,
                    UpdateLast =  DateTime.Parse(lastUpdat),
                    Name = name
                    
                };

              var response = _business.SaveFavoriteRepository(view);

                if (model.IsValid)
                {
                    TempData["success"] = model.Message;
                }
                else
                {
                    TempData["warning"] = model.Message;
                }

                return Json(new
                {
                    Data = response
                }, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}