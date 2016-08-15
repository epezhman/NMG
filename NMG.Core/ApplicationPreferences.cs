using NMG.Core.Domain;

namespace NMG.Core
{
    public class ApplicationPreferences
    {
        public ApplicationPreferences()
        {
            FieldNamingConvention = FieldNamingConvention.SameAsDatabase;
            FieldGenerationConvention = FieldGenerationConvention.Field;
            Prefix = string.Empty;
        }

        public string TableName { get; set; }

        public string FolderPath { get; set; }

        public string NameSpace { get; set; }

        public string AssemblyName { get; set; }

        public ServerType ServerType { get; set; }

        public string ConnectionString { get; set; }

        public string Sequence { get; set; }

        public bool IsFluent { get; set; }

        public bool IsNhFluent { get; set; }

        public bool IsCastle { get; set; }

        public bool IsByCode { get ; set; }

        public bool GeneratePartialClasses { get; set; }

        public string Prefix { get; set; }

        public string ForeignEntityCollectionType { get; set; }

        public string InheritenceAndInterfaces { get; set; }

        public string ClassNamePrefix { get; set; }

        public Language Language { get; set; }

        public FieldNamingConvention FieldNamingConvention { get; set; }

        public FieldGenerationConvention FieldGenerationConvention { get; set; }

        public string EntityName { get; set; }

        public bool GenerateWcfDataContract { get; set; }

        public bool GenerateInFolders { get; set; }
        
        public bool UseLazy { get; set; }

        public string DisplayNameForAttribute { get; set; }

        public string TablePrefix { get; set; }

        public string TablePost { get; set; }

        public string MaxLengthName { get; set; }

        public string Required { get; set; }

        public string ErrorResourse { get; set; }

        public bool IncludeForeignKeys { get; set; }

        public string ResourceReference { get; set; }

        public bool AttributeGeneration { get; set; }

        public bool ResourceGeneration { get; set; }

        public string RangeError { get; set; }

        public string RegxError { get; set; }

        public bool ControllerGeneration { get; set; }

        public bool InheritBaseController { get; set; }

        public bool AuthorizeEnable { get; set; }

        public bool GenerateCRUD { get; set; }

        public bool GenerateLookUps { get; set; }

        public bool GenerateSearchModels { get; set; }

        public bool IncludePaging { get; set; }

        public bool GenerateViews { get; set; }

        public bool WithKendo { get; set; }

        public bool UseAjax { get; set; }

        public bool AnnotationFile { get; set; }

        public bool IncludeVirtual { get; set; }

        public bool DALFiles { get; set; }

        public int ORMView { get; set; }

        public bool SystemDefaults { get; set; }

        public bool Bootstrap { get; set; }

        public bool WithSortExpression { get; set; }

        public string ControllerNamespace { get; set; }

        public string CommonResourseName { get; set; }

        public string ControllerReferences { get; set; }

        public string DALNamespace { get; set; }

        public string DALReferences { get; set; }

        public string DBUtilityName { get; set; }

        public string DataContextName { get; set; }

        public static ApplicationPreferences Default()
        {
            var preferences = new ApplicationPreferences
                                  {
                                      FieldGenerationConvention = FieldGenerationConvention.AutoProperty,
                                      FieldNamingConvention = FieldNamingConvention.SameAsDatabase,
                                      Prefix = string.Empty,
                                      IsNhFluent = true,
                                      Language = Language.CSharp,
                                      ForeignEntityCollectionType = "IList",
                                      InheritenceAndInterfaces = "",
                                      UseLazy = true
                                  };
            return preferences;
        }
    }
}