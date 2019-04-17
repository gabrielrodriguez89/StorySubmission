using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Clients.Adapter.Models;
using Mailer.Adapter;
using Microsoft.Ajax.Utilities;
using Microsoft.Practices.Unity;
using Navigator.Client.Filters;
using Navigator.Client.Models.ViewModels;
using Navigator.Client.Helpers;
using Navigator.Client.Proxies;
using Navigator.Contracts.Models;
using Microsoft.AspNet.Identity;
using System.Web.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Navigator.Client.Controllers
{
    
    public class EquityStoriesController : BaseController
    {
        #region DECLARATIONS

        private readonly EquityProxy _proxy;
        
        #endregion
      
        #region Constructors

        public EquityStoriesController()
        {
            _proxy = new EquityProxy(UnityConfig.GetConfiguredContainer().Resolve<IClient>());
           
        }

        #endregion

        #region GET

        public async Task<ActionResult> Index()
        {
            //return View(new EquityStoriesViewModel() {});
            var stories = await _proxy.GetPublicStoriesAsync();
            return View(stories);
        }

        ////GET all public stories for display
        public async Task<ActionResult> LazyLoadStories(int start = 0, int take = 10)
        {
            var stories = (await _proxy.GetPublicStoriesAsync()).Skip(start).Take(take).OrderByDescending(x => x.PublishDate);

            return Json(stories, JsonRequestBehavior.AllowGet);
        }

        // GET story for full view
        [Route("EquityStories/View/{id:int}", Name = "equityRoute")]
        public async Task<ActionResult> View(int id)
        {
            var model = await _proxy.GetStoryByIdAsync(id);

            return View(model);
        }

        // GET stories and questions for admin view
        [CustomAuthorization(Key = "ESSMods")]
        public async Task<ActionResult> Admin()
        {
            var stories = await _proxy.GetAllStoriesAsync();
            
            return View(stories);
        }

        [CustomAuthorization(Key = "ESSMods")]
        public async Task<ActionResult> AdminQuestions()
        {
            var questionTemplates = await _proxy.GetQuestionTemplatesAsync();
            
            return View(questionTemplates);
        }

        // GET employee's and questions for form to display to users to create new stories
        public async Task<ActionResult> CreateStory()
        {
            var questionTemplates = (await _proxy.GetQuestionTemplatesAsync())
                .Where(x => x.IsActive)
                .ToList();
            var employees = await DirectoryHelper.GetDirectoryEmployees();
            ViewBag.Employees = employees.OrderBy(x => x.MenuName).ToList();
            var employee = employees.FirstOrDefault(x => x.Id == GetUserId());
            ViewBag.AddStory = true;

            var story = new EquityStoryContract
            {
                ContactName = employee.Name,
                UserName = employee.Id,
                ContactEmail = employee.Email,
                ContactPhone = employee.WorkPhone,
                Questions = questionTemplates.Select(x => new EquityQuestionContract
                {
                    QuestionText = x.Question,
                    AnswerText = ""
                }).ToList()
            };

            return View("Story", story);
        }

        [CustomAuthorization(Key = "ESSMods")]
        public async Task<ActionResult> CreateQuestion()
        {
            var employees = await DirectoryHelper.GetDirectoryEmployees();
            ViewBag.Employees = employees.OrderBy(x => x.MenuName).ToList();
            var employee = employees.FirstOrDefault(x => x.Id == GetUserId());
            ViewBag.AddQuestion = true;

            var question = new EquityQuestionTemplateContract
            {
                CreatedBy = employee.Name,
                IsActive = false,
                Id = 0,
                Question = string.Empty
            };

            return View("Question", question);
        }

        public ActionResult Success(int id)
        {
            return View("Success", id);
        }

        [CustomAuthorization(Key = "ESSMods")]
        // GET a story by id so admin users can edit the content if needed
        public async Task<ActionResult> EditStory(int id)
        {
            ViewBag.AddStory = false;

            var story = await _proxy.GetStoryByIdAsync(id);

            if (string.IsNullOrWhiteSpace(story.ArticleContent))
            {
                var ab = new StringBuilder();
                foreach (var question in story.Questions)
                {
                    ab.Append($"<p><strong>{question.QuestionText}</strong></p>");
                    ab.Append($"<p>{question.AnswerText}</p>");
                }
                story.ArticleContent = ab.ToString();
            }
            else
            {
                // remove line breaks (formatting) for vue
                story.ArticleContent = Regex.Replace(story.ArticleContent, @"\t|\n|\r", "");

            }

            // escape single quote for vue
            story.ArticleContent = Regex.Replace(story.ArticleContent, "'", @"\'");

            return View("Story", story);
        }

        [CustomAuthorization(Key = "ESSMods")]
        public async Task<ActionResult> EditQuestion(int id)
        {
            ViewBag.AddQuestion = false;

            var question = await _proxy.GetQuestionTemplateById(id);
    
            return View("Question", question);
        }
        #endregion

        #region POST

        // POST new equity stories and redirect to Index
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<ActionResult> CreateStory(EquityStoryContract story)
        {

            if(!ModelState.IsValid)
            {
                var questionTemplates = (await _proxy.GetQuestionTemplatesAsync())
                .Where(x => x.IsActive)
                .ToList();
                var employees = await DirectoryHelper.GetDirectoryEmployees();
                ViewBag.Employees = employees.OrderBy(x => x.MenuName).ToList();
                var employee = employees.FirstOrDefault(x => x.Id == GetUserId());
                // COMMENT: This ViewBag.AddStory = true is twice (see below model) and I think it should be false anyway
                ViewBag.AddStory = true;

                return View("Story", story);
            }
            story.IsActive = false;
            var newStoryId = await _proxy.PostNewStoryAsync(story);

            if (newStoryId > 0)
            {
                var emailService = new EmailService(Config.ApplicationMode);
                var urlScheme = Request.Url != null ? Request.Url.Scheme : "https";
                var urlHost = Request.Url != null ? Request.Url.Host : Config.ApplicationRootUri;

                var postUrl = Url.Action("View", "EquityStories", new RouteValueDictionary(new { id = newStoryId }), urlScheme, urlHost);

                var message = new IdentityMessage
                {
                    Subject = "Equity Success stories - New Story",
                    Body = $"<div>Equity Story {story.Title} has been submitted by {story.ContactName}.To view and edit this post <a href='{postUrl}'>click here.</a></div>",
                    Destination = Config.EquityStoriesAdminEmail
                };

                await emailService.SendAsync(message);

                return RedirectToAction("Success", new { id = newStoryId });
            }
            return View("Story", story);
        }

        // POST new questions added by administrative user
        [HttpPost]
        [CustomAuthorization(Key = "ESSMods")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateQuestion(EquityQuestionTemplateContract questionTemplate)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.AddQuestion = true;
                return View("Question", questionTemplate);
            }
            //TODO: What if Id == null?
            if(questionTemplate.Id == 0)
            {
                await _proxy.PostNewQuestionTemplateAsync(questionTemplate);
            }
            else
            {
                await _proxy.PutExistingQuestionTemplateAsync(questionTemplate);
            }

            return RedirectToAction("AdminQuestions");
        }
        #endregion

        #region PUTS

        // PUT equity story after editing by an administrative user 
        [HttpPut]
        [CustomAuthorization(Key = "ESSMods")]
        public async Task<ActionResult> EditStory(EquityStoryContract story)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AddStory = false;
       
                return View("Story", story);
            }
            
            await _proxy.PutExistingStoryAsync(story);
            return RedirectToAction("Admin");
        }

        #endregion

        #region Delete

        [HttpDelete]
        [CustomAuthorization(Key = "ESSMods")]
        public async Task<HttpStatusCodeResult> DeleteStory(int id)
        {
            await _proxy.DeleteStoryAsync(id);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpDelete]
        public async Task<HttpStatusCodeResult> DeleteQuestion(int id)
        {
            await _proxy.DeleteQuestionTemplateAsync(id);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        #endregion

        #region PATCH

        // PATCH equity stories/questions to toggle between public/non-public status in administrative view
        [HttpPatch]
        [CustomAuthorization(Key = "ESSMods")]
        public async Task<HttpStatusCodeResult> ChangeStoryPublicStatus(int Id, bool isActive, bool question = false)
        {
            var patch = new PatchModel
            {
                Op = PatchOps.replace,
                Path = "/IsActive",
                Value = isActive
            };     

            if (!question)
            {
                await _proxy.PatchStoryAsync(Id, new List<PatchModel> { patch });
            }
            else
            {
                await _proxy.PatchQuestionTemplateAsync(Id, new List<PatchModel> { patch });
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        #endregion
}
