using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using JsonPatch;
using Microsoft.Web.Http;
using Navigator.Contracts.Models;
using Navigator.Service.Filters;
using Navigator.Service.Helpers;
using Navigator.Service.Repositories;
using Navigator.Service.Results;

namespace Navigator.Service.Controllers
{
    /// <summary>
    /// Navigator Equity API
    /// </summary>
    [ApiVersion("1")]
    [RoutePrefix("api/v{version:apiVersion}/equitystories")]
    public class EquityStoriesController : ApiController
    {
        #region DECLARATIONS
        /// <summary>
        /// Access the repository 
        /// </summary>
        private readonly EquityRepository _repository;
        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// Equity Constructor
        /// </summary>
        public EquityStoriesController()
        {
            _repository = new EquityRepository();
        }
        #endregion

        #region GET

        [Route("")]
        [ResponseType(typeof(IEnumerable<EquityStoryContract>))]
        public async Task<IEnumerable<EquityStoryContract>> GetAllStoriesAsync()
        {
            var stories = await _repository.GetStories();

            return stories;
        }
 
        [Route("public")]
        [ResponseType(typeof(IEnumerable<EquityStoryContract>))]
        public async Task<IEnumerable<EquityStoryContract>> GetPublicStoriesAsync()
        {
            return await _repository.GetPublicStoriesAsync(); 
        }

        /// <summary>
        /// Gets equity stories by id
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        [Route("{id:int}", Name = "StoryById")]
        [ResponseType(typeof(EquityStoryContract))]
        public async Task<EquityStoryContract> GetStoryById(int id)
        {
            return await _repository.GetStoryByIdAsync(id);
        }


        #endregion

        #region POST

        /// <summary>
        /// post new equity story
        /// </summary>
        /// /// <param EquityContract="model"></param>
        /// <returns></returns>
        [Route("")]
        [ValidateModelState]
        [ResponseType(typeof(int))]
        public async Task<IHttpActionResult> PostNewStoryAsync([FromBody]EquityStoryContract model)
        {
            var storyId = await _repository.PostStoryAsync(model);

            return CreatedAtRoute("StoryById", new { id = storyId}, storyId);
        }

        #endregion
                
        #region PUT

        /// <summary>
        /// puts edited equity story 
        /// </summary>
        /// /// <param EquityContract="model"></param>
        /// <returns></returns>
        [Route("")]
        [ValidateModelState]
        public async Task<IHttpActionResult> PutExistingStoryAsync([FromBody]EquityStoryContract model)
        {
            await _repository.PutStoryAsync(model);

            return new NoContentResponse();
        }

        #endregion

        #region DELETE
        /// <summary>
        /// deletes equity story using id
        /// </summary>
        /// /// <param id="id"></param>
        /// <returns></returns>
        [Route("{id:int}")]
        public async Task<IHttpActionResult> DeleteStoryAsync(int id)
        {
            await _repository.DeleteStoryAsync(id);

            return new NoContentResponse();
        }
        #endregion

        #region PATCH

        /// <summary>
        /// Change properties of Stories
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchData"></param>
        /// <returns></returns>
        /// <response code="204">No Content</response>
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PatchStoryAsync(int id, JsonPatchDocument<EquityStoryContract> patchData)
        {
            var story = await _repository.GetStoryByIdAsync(id);

            patchData.ApplyUpdatesTo(story);

            await _repository.PutStoryAsync(story);

            return new NoContentResult();
        }
        #endregion
    }
}