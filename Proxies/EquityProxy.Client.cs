using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Clients.Adapter.Models;
using Navigator.Contracts.Models;

namespace Navigator.Client.Proxies
{
    public class EquityProxy
    {

        #region DECLARATIONS

        private const string EquityStoriesDiscoveryRoute = "RemovedRouteConfig";
        private const string EquityQuestionTemplatesDiscoveryRoute = "RemovedRouteConfig";

        private readonly IClient _client;

        #endregion

        #region CONSTRUCTORS

        public EquityProxy(IClient client)
        {
            _client = client;
        }

        #endregion

        #region GET

        /// <summary>
        /// Gets all equity stories for administrative users
        /// </summary>
        /// <param start="start"></param>
        /// <param take="take"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquityStoryContract>> GetAllStoriesAsync()
        {
            var uri = EquityStoriesDiscoveryRoute;

            var result = await _client.GetAsync<IEnumerable<EquityStoryContract>>(uri);

            return result;
        }

        /// <summary>
        /// Gets all public stories
        /// </summary>
        /// <param start="start"></param>
        /// <param take="take"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquityStoryContract>> GetPublicStoriesAsync()
        {
            var result = await _client.GetAsync<IEnumerable<EquityStoryContract>>($"{EquityStoriesDiscoveryRoute}/public");

            return result;
        }

        /// <summary>
        /// Gets equity stories by id
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        public async Task<EquityStoryContract> GetStoryByIdAsync(int id)
        {
            var result = await _client.GetAsync<EquityStoryContract>($"{EquityStoriesDiscoveryRoute}/{id}");
            return result;
        }
        /// <summary>
        /// Gets equity question by id
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        public async Task<EquityQuestionTemplateContract> GetQuestionTemplateById(int id)
        {
            var result = await _client.GetAsync<EquityQuestionTemplateContract>($"{EquityQuestionTemplatesDiscoveryRoute}/{id}");
            return result;
        }
        /// <summary>
        /// Gets equity stories questions for admin
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EquityQuestionTemplateContract>> GetQuestionTemplatesAsync()
        {
            var result = await _client.GetAsync<IEnumerable<EquityQuestionTemplateContract>>(EquityQuestionTemplatesDiscoveryRoute);

            return result;
        }
        #endregion

        #region POST

        /// <summary>
        /// post new equity story
        /// </summary>
        /// /// <param EquityContract="model"></param>
        /// <returns></returns>
        public async Task<int> PostNewStoryAsync(EquityStoryContract model)
        {
            var response = await _client.PostWithResultAsync<int>(EquityStoriesDiscoveryRoute, model);
            return response;
        }

        /// <summary>
        /// post new equity question
        /// </summary>
        /// /// <param EquityQuestionContract="model"></param>
        /// <returns></returns>
        public async Task<int> PostNewQuestionTemplateAsync(EquityQuestionTemplateContract model)
        {
            var response = await _client.PostWithResultAsync<int>(EquityQuestionTemplatesDiscoveryRoute, model);

            return response;
        }
        #endregion

        #region PUT

        /// <summary>
        /// puts edited equity story 
        /// </summary>
        /// /// <param EquityContract="model"></param>
        /// <returns></returns>
        public async Task PutExistingStoryAsync(EquityStoryContract model)
        {
            await _client.PutAsync($"{EquityStoriesDiscoveryRoute}", model);
        }
        
        /// <summary>
        /// puts edited equity questions 
        /// </summary>
        /// /// <param EquityQuestionsContract="model"></param>
        /// <returns></returns>
        public async Task PutExistingQuestionTemplateAsync(EquityQuestionTemplateContract model)
        {
            await _client.PutAsync($"{EquityQuestionTemplatesDiscoveryRoute}", model);
        }
        #endregion

        #region DELETE

        /// <summary>
        /// deletes equity story using id
        /// </summary>
        /// /// <param id="id"></param>
        /// <returns></returns>
        public async Task DeleteStoryAsync(int id)
        {
            await _client.DeleteAsync($"{EquityStoriesDiscoveryRoute}/{id}");
        }  
        /// <summary>
        /// deletes equity story question using id
        /// </summary>
        /// /// <param id="id"></param>
        /// <returns></returns>
        public async Task DeleteQuestionTemplateAsync(int id)
        {
            await _client.DeleteAsync($"{EquityQuestionTemplatesDiscoveryRoute}/{id}");

        }

        #endregion

        #region PATCH

        /// <summary>
        /// toggles the equity story's public/non-public stories
        /// </summary>
        /// <param Id="Id"></param>
        /// <param patches="patch"></param>
        /// <returns></returns>
        public async Task PatchStoryAsync(int id, IEnumerable<PatchModel> patch)
        {
            await _client.PatchWithOperationAsync($"{EquityStoriesDiscoveryRoute}/{id}", patch);
        }        
        /// <summary>
        /// toggles the equity questions's active/disabled questions
        /// </summary>
        /// <param Id="Id"></param>
        /// <param patches="patch"></param>
        /// <returns></returns>
        public async Task PatchQuestionTemplateAsync(int Id, IEnumerable<PatchModel> patch)
        {
            await _client.PatchWithOperationAsync($"{EquityQuestionTemplatesDiscoveryRoute}/{Id}", patch);
        }

        #endregion
    }
}