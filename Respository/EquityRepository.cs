using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Navigator.Contracts.Models;
using Navigator.Service.Helpers;
using Navigator.Service.Models.DomainModels;
using NinjaNye.SearchExtensions;
using System.Linq.Dynamic;


namespace Navigator.Service.Repositories
{
    /// <summary>
    /// Equity Repository
    /// </summary>
    public class EquityRepository : BaseRepository
    {
        //Equity Stories CRUD Operations
        #region Equity Stories

        #region GET
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquityStoryContract>> GetStories()
        {
            var stories = await Context.EquityStories
                .OrderByDescending(x => x.PublishDate)
                .ToListAsync();

            var result = stories.Select(EquityMapper.EquityStoryEntityToContract);

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
            var stories = await Context.EquityStories.Where(x => x.IsActive == true)
                .OrderBy(x => x.PublishDate)
                .ToListAsync();

            var result = stories.Select(EquityMapper.EquityStoryEntityToContract);

            return result;
        }

        /// <summary>
        /// Gets equity stories by id
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        public async Task<EquityStoryContract> GetStoryByIdAsync(int id)
        {
            var story = await Context.EquityStories.FirstOrDefaultAsync(x => x.Id == id);

            if (story == null)
            {
                throw new CannotFindIdException("story", id);
            }

            var result = EquityMapper.EquityStoryEntityToContract(story);

            return result;
        }

        #endregion

        #region POST
        /// <summary>
        /// post new equity story
        /// </summary>
        /// /// <param EquityContract="model"></param>
        /// <returns></returns>
        public async Task<int> PostStoryAsync(EquityStoryContract model)
        {
            if (Context.EquityStories.Find(model.Id) != null)
            {
                throw new EntityConflictException("story", model.Id);
            }

            var newStory = EquityMapper.EquityContractToEntity(model);

            Context.EquityStories.Add(newStory);
            Context.Entry(newStory).State = EntityState.Added;

            var saved = await Context.SaveChangesAsync();

            if (saved <= 0)
            {
                throw new CannotSaveToDatabaseException("story");
            }
            var storyId = newStory.Id;

            return storyId;
        }
        #endregion

        #region PUT
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task PutStoryAsync(EquityStoryContract model)
        {
            var updatedStory = await Context.EquityStories.FindAsync(model.Id);

            if(updatedStory == null)
            {
                throw new CannotFindIdException("story", model.Id);
            }

            var editedStory = EquityMapper.EquityContractToEntity(model);

            Context.Entry(updatedStory).CurrentValues.SetValues(editedStory);
            Context.Entry(updatedStory).State = EntityState.Modified;

            if(await Context.SaveChangesAsync() <= 0)
            {
                throw new CannotSaveToDatabaseException("Story");
            }
        }
        #endregion

        #region DELETE

        public async Task DeleteStoryAsync(int id)
        {
            var story = await Context.EquityStories.FindAsync(id);

            if(story == null)
            {
                throw new CannotFindIdException("story", id);
            }

            // TODO: verify that this line is necessary
            Context.EquityStories.Attach(story);
            Context.EquityStories.Remove(story);
      
            if (await Context.SaveChangesAsync() <= 0)
            {
                throw new CannotSaveToDatabaseException("story");
            }
        }
        #endregion

        #endregion

        //Equity Questions CRUD Operations
        #region Equity Question Templates
        #region GET
        /// <summary>
        /// Gets equity question by id
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        public async Task<EquityQuestionTemplateContract> GetQuestionTemplateByIdAsync(int id)
        {
            var question = await Context.EquityQuestionTemplates.FirstOrDefaultAsync(x => x.Id == id);

            if (question == null)
            {
                throw new CannotFindIdException("questionTemplate", id);
            }

            var result = EquityMapper.QuestionTemplateEntityToContract(question);

            return result;
        }
        /// <summary>
        /// Gets equity stories questions for admin
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EquityQuestionTemplateContract>> GetQuestionTemplatesAsync()
        {
           try {
                var questions = await Context.EquityQuestionTemplates
               .ToListAsync();

                var result = questions.Select(EquityMapper.QuestionTemplateEntityToContract);

                return result;
            }
            catch(Exception ex)
            {
                var test = ex;

                return new List<EquityQuestionTemplateContract>();
            }
        }

        #endregion

        #region POST
        /// <summary>
        /// post new equity question template
        /// </summary>
        /// /// <param EquityQuestionTemplateContract="model"></param>
        /// <returns></returns>
        public async Task<int> PostQuestionTemplateAsync(EquityQuestionTemplateContract model)
        {
            if (Context.EquityQuestionTemplates.Find(model.Id) != null)
            {
                throw new EntityConflictException("questionTemplate", model.Id);
            }

            var newQuestion = EquityMapper.QuestionTemplateContractToEntity(model);

            Context.EquityQuestionTemplates.Add(newQuestion);
            Context.Entry(newQuestion).State = EntityState.Added;

            var saved = await Context.SaveChangesAsync();

            if (saved <= 0)
            {
                throw new CannotSaveToDatabaseException("questionTemplate");
            }
            var questionId = newQuestion.Id;

            return questionId;
        }
        #endregion

        #region PUT
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task PutQuestionTemplateAsync(EquityQuestionTemplateContract model)
        {
            var updatedQuestion = await Context.EquityQuestionTemplates.FindAsync(model.Id);

            if (updatedQuestion == null)
            {
                throw new CannotFindIdException("questionTemplate", model.Id);
            }

            var editedQuestion = EquityMapper.QuestionTemplateContractToEntity(model);

            Context.Entry(updatedQuestion).CurrentValues.SetValues(editedQuestion);
            Context.Entry(updatedQuestion).State = EntityState.Modified;

            if (await Context.SaveChangesAsync() <= 0)
            {
                throw new CannotSaveToDatabaseException("questionTemplate");
            }
        }
        #endregion

        #region DELETE
        public async Task DeleteQuestionTemplateAsync(int id)
        {
            var question = await Context.EquityQuestionTemplates.FindAsync(id);

            if (question == null)
            {
                throw new CannotFindIdException("questionTemplate", id);
            }

            // TODO: verify that this line is necessary
            Context.EquityQuestionTemplates.Attach(question);
            Context.EquityQuestionTemplates.Remove(question);

            if (await Context.SaveChangesAsync() <= 0)
            {
                throw new CannotSaveToDatabaseException("questionTemplate");
            }
        }
        #endregion
        #endregion
    }
}