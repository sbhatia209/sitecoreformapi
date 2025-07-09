namespace Custom.Foundation.FormSubmission.Controllers
{
    using Custom.Foundation.FormSubmission.Model;
    using Custom.Foundation.FormSubmission.Services;
    using System;
    using Newtonsoft.Json;
    using System.Web.Http.Cors;
    using System.Web.Http;
    using System.Web.Mvc;

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [System.Web.Http.RoutePrefix("api/forms")]
    public class FormSubmissionController : ApiController
    {
        private readonly FormsService _formsService;
        public FormSubmissionController()
        {
            _formsService = new FormsService();             
        }
             
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("submit")]
        public JsonResult SubmitFormDetails([FromBody] FormData request)
        {
            var error = string.Empty;
            int statusCode = 0;
            string successMessage = string.Empty;  
            try
            {
                _formsService.SaveFormsData(request.FormId, request.Fields);
                successMessage = "Form submitted successfully with data: " + JsonConvert.SerializeObject(request.Fields);
                statusCode = 200;
                
            }
            catch (Exception ex)
            {
                 
                Sitecore.Diagnostics.Log.Error($"Error in request data: {JsonConvert.SerializeObject(request)}", this);
                Sitecore.Diagnostics.Log.Error($"Error in SubmitFormDetails: {ex}", this);
                error = ex.Message;
                statusCode = 500; // Custom error code
            }

            return new JsonResult()
            {
                Data = new
                {
                    success = string.IsNullOrEmpty(error),
                    status_code = statusCode,
                    error = error,
                    message = string.IsNullOrEmpty(error) ? successMessage : string.Empty // Include success message if no error
                },
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            };
        }
    }
}