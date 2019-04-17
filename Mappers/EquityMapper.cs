using AutoMapper;
using Microsoft.Security.Application;
using Navigator.Contracts.Models;
using Navigator.Service.Models.DomainModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Navigator.Service.Helpers
{
    /// <summary>
    /// equit and questions mapper
    /// </summary>
    public class EquityMapper
    {
        #region STORY MAPPER
        // map contract to entity
        public static EquityStory EquityContractToEntity(EquityStoryContract contract)
        {
            var entity = new EquityStory
            {
                Id = contract.Id,
                Title = contract.Title,
                Description = contract.Description,
                IsActive = contract.IsActive,
                PublishDate = contract.PublishDate,
                ContactName = contract.ContactName,
                ContactEmail = contract.ContactEmail,
                ContactPhone = contract.ContactPhone,
                UserName = contract.UserName,
                ImgThumb = contract.ImgThumb,
                Questions = JsonConvert.SerializeObject(contract.Questions),
                CreatedBy = contract.ContactName,
                CreatedOn = DateTime.Now,
                ArticleContent = contract.ArticleContent
            };

            return entity;
        }

        // map entity to contract
        public static EquityStoryContract EquityStoryEntityToContract(EquityStory entity)
        {
            var contract = new EquityStoryContract
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                IsActive = entity.IsActive,
                PublishDate = entity.PublishDate,
                ContactName = entity.ContactName,
                ContactEmail = entity.ContactEmail,
                ContactPhone = entity.ContactPhone,
                UserName = entity.UserName,
                ImgThumb = entity.ImgThumb,
                Questions = JsonConvert.DeserializeObject<List<EquityQuestionContract>>(entity.Questions),
                CreatedBy = entity.CreatedBy,
                ArticleContent = entity.ArticleContent
            };

            return contract;
        }
        #endregion

        #region QUESTION MAPPER
        // map contract to entity
        public static EquityQuestionTemplate QuestionTemplateContractToEntity(EquityQuestionTemplateContract contract)
        {
            var entity = new EquityQuestionTemplate
            {
                Id = contract.Id,
                CreatedBy = contract.CreatedBy,
                IsActive = contract.IsActive,
                Question = contract.Question
            };
           
            return entity;
        }

        // map entity to contract
        public static EquityQuestionTemplateContract QuestionTemplateEntityToContract(EquityQuestionTemplate entity)
        {
            var contract = new EquityQuestionTemplateContract
            {
                Id = entity.Id,
                CreatedBy = entity.CreatedBy,
                IsActive = entity.IsActive,
                Question = entity.Question
            };

            return contract;
        }
        #endregion
    }
}