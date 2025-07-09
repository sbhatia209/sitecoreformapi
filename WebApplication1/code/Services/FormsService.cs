namespace Custom.Foundation.FormSubmission.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore.DependencyInjection;
    using Sitecore.ExperienceForms.Data.Entities;
    using Sitecore.ExperienceForms.Data;
    using Custom.Foundation.FormSubmission.Model;
    using Microsoft.Extensions.DependencyInjection;
    using System.IO;
    using System.Data;
    public class FormsService
    {
        private IFormDataProvider _dataProvider;

        private IFileStorageProvider _fileStorgeProvider;

        public readonly string fileType= "{7E9A0903-A52C-4843-BBE1-5B26BD162BED}";

        private IFormDataProvider DataProvider
        {
            get
            {
                if (_dataProvider == null)
                {
                    _dataProvider = ServiceLocator.ServiceProvider.GetRequiredService<IFormDataProvider>();
                }
                return _dataProvider;
            }
        }

        private IFileStorageProvider FileStorageProvider
        {
            get
            {
                if (_fileStorgeProvider == null)
                {

                    _fileStorgeProvider = ServiceLocator.ServiceProvider.GetRequiredService<IFileStorageProvider>();
                }
                return _fileStorgeProvider;
            }
        }


        public void SaveFormsData(Guid formID, List<Field> formFieldDataList)
        {
            Guid sessionID = Guid.NewGuid();
            
            var formEntries = formFieldDataList.Where(x => x.Type == fileType).ToList();
            if (formEntries != null && formEntries.Count>0)
            {
                foreach (var field in formEntries) 
                {
                    Guid fileGuid = FileStorageProvider.StoreFile(new MemoryStream(Convert.FromBase64String(field.Value)), field.FileName);
                   
                    int index = formFieldDataList.FindIndex(x => x.FieldId.Equals(field.FieldId));
                    if (index != -1)
                    {
                        formFieldDataList[index].Value = fileGuid.ToString();  
                    }
                }
                
            }
            FormEntry formEntry = new FormEntry
            {
                Created = DateTime.Now,
                FormItemId = formID,
                FormEntryId = sessionID,

                Fields = formFieldDataList.Select(item => new FieldData
                {

                    FieldDataId = Guid.NewGuid(),
                    FieldItemId = item.FieldId,
                    FormEntryId = sessionID,
                    FieldName = item.FormFieldName,
                    Value = item.Value,
                    ValueType = GetFieldDataTypeByGuid(item.Type)

                }).ToList()
            };

            DataProvider.CreateEntry(formEntry);

        }
        public string GetFieldDataTypeByGuid(string fieldGuid)
        {
            // Map Sitecore field types to .NET types
            switch (fieldGuid)
            {
                case "{4EE89EA7-CEFE-4C8E-8532-467EF64591FC}"://single line
                case "{A296A1C1-0DA0-4493-A92E-B8191F43AEC6}"://multile text
                case "{DF74F55B-47E6-4D1C-92F8-B0D46A7B2704}"://phone
                case "{04C39CAC-8976-4910-BE0D-879ED3368429}"://email
                    return "System.String";

                case "{38137D30-7B2A-47D5-BBD8-133252C01B28}"://date
                    return "System.DateTime";

                case "{7E9A0903-A52C-4843-BBE1-5B26BD162BED}"://fileuplaod
                    return "System.Collections.Generic.List`1[Sitecore.ExperienceForms.Data.Entities.StoredFileInfo]";

                case "{5B153FC0-FC3F-474F-8CB8-233FB1BEF292}"://number
                    return "System.Double";

                case "{4DA85E8A-3B48-4BC6-9565-8C1F5F36DD1B}"://checkbox
                    return "System.Boolean";

                case "{E0CFADEE-1AC0-471D-A820-2E70D1547B4B}":
                    return "System.Collections.Generic.List`1[System.String]";

                case "{D86A361A-D4FF-46B2-9E97-A37FC5B1FE1A}": //checkbox List
                    return "System.Collections.Generic.List`1[System.String]";

                default:
                    return "System.String";
            }
        }

    }
}