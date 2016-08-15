using NMG.Core;
using NMG.Core.Domain;
using NMG.Core.Generator;

namespace NHibernateMappingGenerator
{
    public class ApplicationController
    {
        private readonly ApplicationPreferences applicationPreferences;
        private readonly CastleGenerator castleGenerator;
        private readonly CodeGenerator codeGenerator;
        private readonly FluentGenerator fluentGenerator;
        private readonly NHFluentGenerator nhFluentGenerator;
        private readonly MappingGenerator mappingGenerator;
        private readonly ContractGenerator contractGenerator;
        private readonly ByCodeGenerator byCodeGenerator;
        private readonly ControllerGenerator controllerGenerator;
        private readonly ResourceGenerator resourceGenerator;
        private readonly SearchModelGenerator searchModelGenerator;
        private readonly ViewGenerator viewGenerator;
        private readonly AnnotationGenerator annotationGenerator;
        private readonly DALGenerator _DALGenerator;
        private readonly DALControllerGenerator _DALControllerGenerator;

        public ApplicationController(ApplicationPreferences applicationPreferences, Table table)
        {
            this.applicationPreferences = applicationPreferences;
            codeGenerator = new CodeGenerator(applicationPreferences, table);
            fluentGenerator = new FluentGenerator(applicationPreferences, table);
            nhFluentGenerator = new NHFluentGenerator(applicationPreferences, table);
            castleGenerator = new CastleGenerator(applicationPreferences, table);
            contractGenerator = new ContractGenerator(applicationPreferences, table);
            byCodeGenerator = new ByCodeGenerator(applicationPreferences, table);
            controllerGenerator = new ControllerGenerator(applicationPreferences, table);
            searchModelGenerator = new SearchModelGenerator(applicationPreferences, table);
            resourceGenerator = new ResourceGenerator(applicationPreferences, table);
            viewGenerator = new ViewGenerator(applicationPreferences, table);
            annotationGenerator = new AnnotationGenerator(applicationPreferences, table);
            _DALGenerator = new DALGenerator(applicationPreferences, table);
            _DALControllerGenerator = new DALControllerGenerator(applicationPreferences, table);

            if (applicationPreferences.ServerType == ServerType.Oracle)
            {
                mappingGenerator = new OracleMappingGenerator(applicationPreferences, table);
            }
            else
            {
                mappingGenerator = new SqlMappingGenerator(applicationPreferences, table);
            }
        }

        public void Generate()
        {
            codeGenerator.Generate();
            if (applicationPreferences.IsNhFluent)
            {
                nhFluentGenerator.Generate();
            }
            else if (applicationPreferences.IsFluent)
            {
                fluentGenerator.Generate();
            }
            else if (applicationPreferences.IsCastle)
            {
                castleGenerator.Generate();
            }
            else if (applicationPreferences.IsByCode)
            {
                byCodeGenerator.Generate();
            }
            else
            {
                mappingGenerator.Generate();
            }
            if (applicationPreferences.GenerateWcfDataContract)
            {
                contractGenerator.Generate();
            }
            if (applicationPreferences.ControllerGeneration)
            {
                controllerGenerator.Generate();
            }
            if (applicationPreferences.ResourceGeneration)
            {
                resourceGenerator.Generate();
            }
            if (applicationPreferences.GenerateSearchModels)
            {
                searchModelGenerator.Generate();
            }
            if (applicationPreferences.GenerateViews)
            {
                viewGenerator.Generate();
            }
            if (applicationPreferences.AnnotationFile)
            {
                annotationGenerator.Generate();
            }
            if (applicationPreferences.DALFiles)
            {
                _DALGenerator.Generate();
                _DALControllerGenerator.Generate();
            }
        }
    }
}