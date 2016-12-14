using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NuTranslation.Models;
using Umbraco.Core.Models;
using Umbraco.Web.Editors;
using Umbraco.Web.WebApi;
using Language = NuTranslation.Models.Language;

namespace NuTranslation.Controllers
{
    public class NuTranslationController: UmbracoAuthorizedJsonController
    {

        [HttpPost]
        public HttpResponseMessage Post([FromBody] TranslationRequestDTO request)
        {
            if (request.Languages.Count() < 1)
                return Request.CreateValidationErrorResponse("At Least one Target language is needed");
            if (String.IsNullOrWhiteSpace(request.SourceLang))
                return Request.CreateValidationErrorResponse("Source Language is needed");
            Services.AuditService.Add(AuditType.SendToTranslate, "Request for translation from " + request.SourceLang + " to: " + request.Languages.Aggregate((total, lang) => total + ", " + lang).TrimEnd(',', ' '), Security.GetUserId(), request.ContentId);


            //var cs = ApplicationContext.Services.ContentService;
            //var content = cs.GetById(request.ContentId);



            //TranslationRequest newRequest = TranslationRequest.New(request, content, TranslationStatus.Requested);
            //_translationRequestsStorage.StoreNewTranslationRequest(newRequest);

            return Request.CreateResponse(HttpStatusCode.OK, request.Languages);
        }

        [HttpGet]
        public HttpResponseMessage GetLanguages()
        {
            var languages = new List<Language>();

            languages.AddRange(umbraco.cms.businesslogic.language.Language.GetAllAsList()
                .Select(x => CultureInfo.GetCultureInfo(x.CultureAlias))
                .Select(x => new Language
                {
                    IsoCode = x.Name,
                    Name = x.DisplayName,
                    NativeName = x.NativeName,
                    IsRightToLeft = x.TextInfo.IsRightToLeft
                }));


            return Request.CreateResponse(HttpStatusCode.OK, languages);
        }
    }
}
