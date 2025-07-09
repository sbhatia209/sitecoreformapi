namespace Custom.Foundation.FormSubmission.Model
{
    using Sitecore.Reflection;
    using System;
    using System.Collections.Generic;

    public class FormData
    {
        public Guid FormId { get; set; }
     
        public List<Field> Fields { get; set; }
    }
    public class Field
    {
        public string FormFieldName { get; set; }
        public Guid FieldId { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
    }
}