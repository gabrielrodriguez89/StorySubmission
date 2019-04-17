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
    /// Navigator Equity Questions API
    /// </summary>
    [ApiVersion("1")]
    [RoutePrefix("api/v{version:apiVersion}/equityquestiontemplates")]
    public class EquityQuestionTemplatesController : ApiController
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
        public EquityQuestionTemplatesController()
        {
            _repository = new EquityRepository();
        }
        #endregion

        #region GET
        // GET: EquityQuestions

        /// <summary>
        /// Gets equity question templates for admin
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(IEnumerable<EquityQuestionTemplateContract>))]
        public async Task<IEnumerable<EquityQuestionTemplateContract>> GetQuestionTemplatesAsync()
        {
            return await _repository.GetQuestionTemplatesAsync();
        }

        /// <summary>
        /// Gets equity question by id
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        [Route("{id:int}", Name = "QuestionTemplateById")]
        [ResponseType(typeof(EquityQuestionTemplateContract))]
        public async Task<EquityQuestionTemplateContract> GetQuestionTemplateById(int id)
        {
            return await _repository.GetQuestionTemplateByIdAsync(id);
        }
        #endregion

        #region POST
        /// <summary>
        /// post new equity question
        /// </summary>
        /// /// <param EquityQuestionContract="model"></param>
        /// <returns></returns>
        [Route("")]
        [ValidateModelState]
        [ResponseType(typeof(int))]
        public async Task<IHttpActionResult> PostNewQuestionTemplateAsync(EquityQuestionTemplateContract model)
        {
            var questionId = await _repository.PostQuestionTemplateAsync(model);

            return CreatedAtRoute("QuestionTemplateById", new { id = questionId }, questionId);
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
        public async Task<IHttpActionResult> PutExistingQuestionTemplateAsync([FromBody]EquityQuestionTemplateContract model)
        {
            await _repository.PutQuestionTemplateAsync(model);

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
        public async Task<IHttpActionResult> DeleteQuestionTemplateAsync(int id)
        {
            await _repository.DeleteQuestionTemplateAsync(id);

            return new NoContentResponse();
        }
        #endregion

        #region PATCH

        /// <summary>
        /// Change properties of questions
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchData"></param>
        /// <returns></returns>
        /// <response code="204">No Content</response>
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PatchQuestionTemplateAsync(int id, JsonPatchDocument<EquityQuestionTemplateContract> patchData)
        {
            var question = await _repository.GetQuestionTemplateByIdAsync(id);

            patchData.ApplyUpdatesTo(question);

            await _repository.PutQuestionTemplateAsync(question);

            return new NoContentResult();
        }
        #endregion
    }
}