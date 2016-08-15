using System;
using System.CodeDom;
using NMG.Core.Util;
using NMG.Core.Domain;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace NMG.Core.Generator
{
    public class CodeGenerationHelper
    {

        public PluralizationService Pluralizer
        {
            get { return PluralizationService.CreateService(new CultureInfo("en-US")); }
        }

        public CodeCompileUnit GetCodeCompileUnit(string nameSpace, string className, params bool[] isCastle)
        {
            var codeCompileUnit = new CodeCompileUnit();
            var codeNamespace = new CodeNamespace(nameSpace);
            var codeTypeDeclaration = new CodeTypeDeclaration(className);
            if (null != isCastle && isCastle.Length > 0 && Convert.ToBoolean(isCastle[0]))
            {
                var codeAttributeDeclaration = new CodeAttributeDeclaration("ActiveRecord");
                codeTypeDeclaration.BaseTypes.Add(new CodeTypeReference("ActiveRecordValidationBase<" + className + ">"));
                codeTypeDeclaration.CustomAttributes.Add(codeAttributeDeclaration);
            }

            codeNamespace.Types.Add(codeTypeDeclaration);
            codeCompileUnit.Namespaces.Add(codeNamespace);
            return codeCompileUnit;
        }

        public CodeCompileUnit GetCodeCompileUnitWithInheritanceAndInterface(string nameSpace, string className, string inheritanceAndInterface, params bool[] isCastle)
        {
            var codeCompileUnit = GetCodeCompileUnit(nameSpace, className, isCastle);
            if (!string.IsNullOrEmpty(inheritanceAndInterface))
            {
                foreach (CodeNamespace ns in codeCompileUnit.Namespaces)
                {
                    foreach (CodeTypeDeclaration type in ns.Types)
                    {
                        foreach (var classOrInterface in inheritanceAndInterface.Split(','))
                            type.BaseTypes.Add(new CodeTypeReference(classOrInterface.Replace("<T>", "<" + className + ">").Trim()));
                    }

                }
            }
            return codeCompileUnit;
        }

        public CodeMemberProperty CreateProperty(Type type, string propertyName)
        {
            var codeMemberProperty = new CodeMemberProperty
                                         {
                                             Name = propertyName,
                                             HasGet = true,
                                             HasSet = true,
                                             Attributes = MemberAttributes.Public,
                                             Type = new CodeTypeReference(type)
                                         };

            string fieldName = propertyName.MakeFirstCharLowerCase();
            var codeFieldReferenceExpression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(),
                                                                                fieldName);
            var returnStatement = new CodeMethodReturnStatement(codeFieldReferenceExpression);
            codeMemberProperty.GetStatements.Add(returnStatement);
            var assignStatement = new CodeAssignStatement(codeFieldReferenceExpression,
                                                          new CodePropertySetValueReferenceExpression());
            codeMemberProperty.SetStatements.Add(assignStatement);
            return codeMemberProperty;
        }

        public CodeMemberProperty CreateAutoProperty(Type type, string propertyName, bool fieldIsNull, bool useLazy = true)
        {
            bool setFieldAsNullable = fieldIsNull && IsNullable(type);
            if (setFieldAsNullable)
                type = typeof(Nullable<>).MakeGenericType(type);
            var codeMemberProperty = new CodeMemberProperty
            {
                Name = propertyName,
                HasGet = true,
                HasSet = true,
                Attributes = MemberAttributes.Public,
                Type = new CodeTypeReference(type)
            };
            if (!useLazy)
                codeMemberProperty.Attributes = codeMemberProperty.Attributes | MemberAttributes.Final;
            return codeMemberProperty;
        }

        public CodeMemberProperty CreateAutoProperty(Type type, string propertyName, bool fieldIsNull, int? dataLength, string tableName, Column column, ApplicationPreferences appPref, bool useLazy = true)
        {
            bool setFieldAsNullable = fieldIsNull && IsNullable(type);
            if (setFieldAsNullable)
                type = typeof(Nullable<>).MakeGenericType(type);
            var codeMemberProperty = new CodeMemberProperty
                                         {
                                             Name = propertyName,
                                             HasGet = true,
                                             HasSet = true,
                                             Attributes = MemberAttributes.Public,
                                             Type = new CodeTypeReference(type)
                                         };
            if (!useLazy)
                codeMemberProperty.Attributes = codeMemberProperty.Attributes | MemberAttributes.Final;
            if (appPref.AttributeGeneration)
            {
                var codeAttrArg1 = new CodeAttributeArgument(new CodeSnippetExpression(string.Format("ResourceType = typeof({0}.{1}{2}{3})", appPref.ResourceReference, appPref.TablePrefix, tableName, appPref.TablePost)));
                var codeAttrArg2 = new CodeAttributeArgument(new CodeSnippetExpression(string.Format("Name = \"{0}\"", propertyName)));
                var codeAttrDecl = new CodeAttributeDeclaration("Display", codeAttrArg2, codeAttrArg1);
                codeMemberProperty.CustomAttributes.Add(codeAttrDecl);
                if (dataLength.HasValue)
                {
                    var codeAttrArg0 = new CodeAttributeArgument(new CodePrimitiveExpression(dataLength));
                    codeAttrArg1 = new CodeAttributeArgument("ErrorMessageResourceType", new CodeSnippetExpression(string.Format(" typeof({0}.{1})", appPref.ResourceReference, appPref.ErrorResourse)));
                    codeAttrArg2 = new CodeAttributeArgument("ErrorMessageResourceName", new CodePrimitiveExpression(string.Format("{0}{1}", appPref.MaxLengthName, dataLength)));
                    codeAttrDecl = new CodeAttributeDeclaration("StringLength", codeAttrArg0, codeAttrArg1, codeAttrArg2);
                    codeMemberProperty.CustomAttributes.Add(codeAttrDecl);
                }
                if (!fieldIsNull)
                {
                    codeAttrArg1 = new CodeAttributeArgument("ErrorMessageResourceType", new CodeSnippetExpression(string.Format(" typeof({0}.{1})", appPref.ResourceReference, appPref.ErrorResourse)));
                    codeAttrArg2 = new CodeAttributeArgument("ErrorMessageResourceName", new CodePrimitiveExpression(string.Format("{0}", appPref.Required)));
                    codeAttrDecl = new CodeAttributeDeclaration("Required", codeAttrArg1, codeAttrArg2);
                    codeMemberProperty.CustomAttributes.Add(codeAttrDecl);
                }
                if (!string.IsNullOrEmpty(column.RegX))
                {
                    var codeAttrArg0 = new CodeAttributeArgument(new CodePrimitiveExpression(column.RegX));
                    codeAttrArg1 = new CodeAttributeArgument("ErrorMessageResourceType", new CodeSnippetExpression(string.Format(" typeof({0}.{1})", appPref.ResourceReference, appPref.ErrorResourse)));
                    codeAttrArg2 = new CodeAttributeArgument("ErrorMessageResourceName", new CodePrimitiveExpression(string.Format("{0}{1}", propertyName, appPref.RegxError)));
                    codeAttrDecl = new CodeAttributeDeclaration("RegularExpression", codeAttrArg0, codeAttrArg1, codeAttrArg2);
                    codeMemberProperty.CustomAttributes.Add(codeAttrDecl);
                }
                if (column.BeginOfRange.HasValue && column.EndOfRange.HasValue)
                {
                    var codeAttrArg0 = new CodeAttributeArgument(new CodePrimitiveExpression(column.BeginOfRange));
                    codeAttrArg1 = new CodeAttributeArgument(new CodePrimitiveExpression(column.EndOfRange));
                    codeAttrArg2 = new CodeAttributeArgument("ErrorMessageResourceType", new CodeSnippetExpression(string.Format(" typeof({0}.{1})", appPref.ResourceReference, appPref.ErrorResourse)));
                    var codeAttrArg3 = new CodeAttributeArgument("ErrorMessageResourceName", new CodePrimitiveExpression(string.Format("{0}", appPref.RangeError)));
                    codeAttrDecl = new CodeAttributeDeclaration("Range", codeAttrArg0, codeAttrArg1, codeAttrArg2, codeAttrArg3);
                    codeMemberProperty.CustomAttributes.Add(codeAttrDecl);
                }
            }
            return codeMemberProperty;
        }

        public CodeMemberProperty CreateAutoPropertyWithVirtualConcern(Type type, string propertyName, bool fieldIsNull, int? dataLength, string tableName, Column column, ApplicationPreferences appPref, bool virtualPrefer)
        {
            bool setFieldAsNullable = fieldIsNull && IsNullable(type);
            if (setFieldAsNullable)
                type = typeof(Nullable<>).MakeGenericType(type);
            var codeMemberProperty = new CodeMemberProperty
            {
                Name = propertyName,
                HasGet = true,
                HasSet = true,
                Attributes = MemberAttributes.Public,
                Type = new CodeTypeReference(type)
            };
            if (!virtualPrefer)
                codeMemberProperty.Attributes = codeMemberProperty.Attributes | MemberAttributes.Final;
            if (appPref.AttributeGeneration)
            {
                var codeAttrArg1 = new CodeAttributeArgument(new CodeSnippetExpression(string.Format("ResourceType = typeof({0}.{1}{2}{3})", appPref.ResourceReference, appPref.TablePrefix, tableName, appPref.TablePost)));
                var codeAttrArg2 = new CodeAttributeArgument(new CodeSnippetExpression(string.Format("Name = \"{0}\"", propertyName)));
                var codeAttrDecl = new CodeAttributeDeclaration("Display", codeAttrArg2, codeAttrArg1);
                codeMemberProperty.CustomAttributes.Add(codeAttrDecl);
                if (dataLength.HasValue)
                {
                    var codeAttrArg0 = new CodeAttributeArgument(new CodePrimitiveExpression(dataLength));
                    codeAttrArg1 = new CodeAttributeArgument("ErrorMessageResourceType", new CodeSnippetExpression(string.Format(" typeof({0}.{1})", appPref.ResourceReference, appPref.ErrorResourse)));
                    codeAttrArg2 = new CodeAttributeArgument("ErrorMessageResourceName", new CodePrimitiveExpression(string.Format("{0}{1}", appPref.MaxLengthName, dataLength)));
                    codeAttrDecl = new CodeAttributeDeclaration("StringLength", codeAttrArg0, codeAttrArg1, codeAttrArg2);
                    codeMemberProperty.CustomAttributes.Add(codeAttrDecl);
                }
                if (!fieldIsNull)
                {
                    codeAttrArg1 = new CodeAttributeArgument("ErrorMessageResourceType", new CodeSnippetExpression(string.Format(" typeof({0}.{1})", appPref.ResourceReference, appPref.ErrorResourse)));
                    codeAttrArg2 = new CodeAttributeArgument("ErrorMessageResourceName", new CodePrimitiveExpression(string.Format("{0}", appPref.Required)));
                    codeAttrDecl = new CodeAttributeDeclaration("Required", codeAttrArg1, codeAttrArg2);
                    codeMemberProperty.CustomAttributes.Add(codeAttrDecl);
                }
                if (!string.IsNullOrEmpty(column.RegX))
                {
                    var codeAttrArg0 = new CodeAttributeArgument(new CodePrimitiveExpression(column.RegX));
                    codeAttrArg1 = new CodeAttributeArgument("ErrorMessageResourceType", new CodeSnippetExpression(string.Format(" typeof({0}.{1})", appPref.ResourceReference, appPref.ErrorResourse)));
                    codeAttrArg2 = new CodeAttributeArgument("ErrorMessageResourceName", new CodePrimitiveExpression(string.Format("{0}{1}", propertyName, appPref.RegxError)));
                    codeAttrDecl = new CodeAttributeDeclaration("RegularExpression", codeAttrArg0, codeAttrArg1, codeAttrArg2);
                    codeMemberProperty.CustomAttributes.Add(codeAttrDecl);
                }
                if (column.BeginOfRange.HasValue && column.EndOfRange.HasValue)
                {
                    var codeAttrArg0 = new CodeAttributeArgument(new CodePrimitiveExpression(column.BeginOfRange));
                    codeAttrArg1 = new CodeAttributeArgument(new CodePrimitiveExpression(column.EndOfRange));
                    codeAttrArg2 = new CodeAttributeArgument("ErrorMessageResourceType", new CodeSnippetExpression(string.Format(" typeof({0}.{1})", appPref.ResourceReference, appPref.ErrorResourse)));
                    var codeAttrArg3 = new CodeAttributeArgument("ErrorMessageResourceName", new CodePrimitiveExpression(string.Format("{0}", appPref.RangeError)));
                    codeAttrDecl = new CodeAttributeDeclaration("Range", codeAttrArg0, codeAttrArg1, codeAttrArg2, codeAttrArg3);
                    codeMemberProperty.CustomAttributes.Add(codeAttrDecl);
                }
            }
            return codeMemberProperty;
        }

        public CodeMemberProperty CreateAutoProperty(string typeName, string propertyName, string tableName, ApplicationPreferences appPref, bool useLazy = true)
        {
            var codeMemberProperty = new CodeMemberProperty
                                         {
                                             Name = propertyName,
                                             HasGet = true,
                                             HasSet = true,
                                             Attributes = MemberAttributes.Public,
                                             Type = new CodeTypeReference(typeName)
                                         };
            if (!useLazy)
                codeMemberProperty.Attributes = codeMemberProperty.Attributes | MemberAttributes.Final;
            if (appPref.AttributeGeneration)
            {
                var codeAttrArg1 = new CodeAttributeArgument(new CodeSnippetExpression(string.Format("ResourceType = typeof({0}.{1}{2}{3})", appPref.ResourceReference, appPref.TablePrefix, tableName, appPref.TablePost)));
                var codeAttrArg2 = new CodeAttributeArgument(new CodeSnippetExpression(string.Format("Name = \"{0}\"", propertyName)));
                var codeAttrDecl = new CodeAttributeDeclaration("Display", codeAttrArg2, codeAttrArg1);
                codeMemberProperty.CustomAttributes.Add(codeAttrDecl);
            }
            return codeMemberProperty;
        }

        public CodeMemberProperty CreateAutoPropertyWithVirtualConcern(string typeName, string propertyName, string tableName, ApplicationPreferences appPref, bool virtualPrefer)
        {
            var codeMemberProperty = new CodeMemberProperty
            {
                Name = propertyName,
                HasGet = true,
                HasSet = true,
                Attributes = MemberAttributes.Public,
                Type = new CodeTypeReference(typeName)
            };
            if (!virtualPrefer)
                codeMemberProperty.Attributes = codeMemberProperty.Attributes | MemberAttributes.Final;
            if (appPref.AttributeGeneration)
            {
                var codeAttrArg1 = new CodeAttributeArgument(new CodeSnippetExpression(string.Format("ResourceType = typeof({0}.{1}{2}{3})", appPref.ResourceReference, appPref.TablePrefix, tableName, appPref.TablePost)));
                var codeAttrArg2 = new CodeAttributeArgument(new CodeSnippetExpression(string.Format("Name = \"{0}\"", propertyName)));
                var codeAttrDecl = new CodeAttributeDeclaration("Display", codeAttrArg2, codeAttrArg1);
                codeMemberProperty.CustomAttributes.Add(codeAttrDecl);
            }
            return codeMemberProperty;
        }

        public CodeMemberProperty CreateAutoProperty(string typeName, string propertyName, bool useLazy = true)
        {
            var codeMemberProperty = new CodeMemberProperty
            {
                Name = propertyName,
                HasGet = true,
                HasSet = true,
                Attributes = MemberAttributes.Public,
                Type = new CodeTypeReference(typeName)
            };
            if (!useLazy)
                codeMemberProperty.Attributes = codeMemberProperty.Attributes | MemberAttributes.Final;
            return codeMemberProperty;
        }

        // For Castle
        public CodeMemberProperty CreateAutoProperty(string typeName, string propertyName, CodeAttributeDeclaration attributeArgument)
        {
            var codeMemberProperty = new CodeMemberProperty
                                         {
                                             Name = propertyName,
                                             HasGet = true,
                                             HasSet = true,
                                             Attributes = MemberAttributes.Public,
                                             Type = new CodeTypeReference(typeName)
                                         };

            codeMemberProperty.CustomAttributes.Add(attributeArgument);

            return codeMemberProperty;
        }

        public CodeMemberField CreateField(Type type, string fieldName)
        {
            return new CodeMemberField(type, fieldName);
        }

        // http://bytes.com/topic/c-sharp/answers/515498-typeof-check-nullability
        // Should probably move this elsewhere...
        private static bool IsNullable(Type type)
        {
            return type.IsValueType ||
                   (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public CodeMemberProperty CreateAutoPropertyWithDataMemberAttribute(string type, string propertyName)
        {
            var attributes = new CodeAttributeDeclarationCollection { new CodeAttributeDeclaration("DataMember") };
            var codeMemberProperty = new CodeMemberProperty
            {
                Name = propertyName,
                HasGet = true,
                HasSet = true,
                CustomAttributes = attributes,
                Attributes = MemberAttributes.Public,
                Type = new CodeTypeReference(type)
            };
            return codeMemberProperty;
        }

        public string InstatiationObject(string foreignEntityCollectionType)
        {
            if (foreignEntityCollectionType.Contains("List"))
                return "List";
            if (foreignEntityCollectionType.Contains("Set"))
                return "HashedSet";
            return foreignEntityCollectionType;
        }

        public CodeMemberMethod GenerateCreateMethod(string className, Table table, ApplicationPreferences appPref)
        {
            var createMethod = new CodeMemberMethod();
            createMethod.Attributes = (createMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            createMethod.Name = "Create";
            createMethod.ReturnType = new CodeTypeReference("ActionResult");
            var varDeclare = new CodeVariableDeclarationStatement("var", "dc", new CodeSnippetExpression(string.Format("GetDataContext()")));
            createMethod.Statements.Add(varDeclare);
            var modelName = "model";
            var modelDeclare = new CodeVariableDeclarationStatement("var", modelName, new CodeSnippetExpression(string.Format("new {0}{1}()", appPref.ClassNamePrefix, className)));
            createMethod.Statements.Add(modelDeclare);
            foreach (var fk in table.ForeignKeys)
            {
                if (!string.IsNullOrEmpty(fk.References))
                {
                    if (fk.References != className)
                    {
                        var tableInf = MetaDataGenerator.GetAvailableLookUpCombos(table.DatabaseName, fk.UniquePropertyName);
                        if (string.IsNullOrEmpty(tableInf))
                        {
                            var varDeclareViewbag = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("ViewBag.{0}s", fk.UniquePropertyName)), new CodeVariableReferenceExpression(string.Format("dc.Db<{0}>().Select(x => new {{ id = x.Id, title = x.Title }})", fk.References)));
                            createMethod.Statements.Add(varDeclareViewbag);
                        }
                        else
                        {
                            var varDeclareViewbag = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("ViewBag.{0}s", fk.UniquePropertyName)), new CodeVariableReferenceExpression(string.Format("dc.Db<{0}>().Select(x => new {{ id = x.Id, title = x.Title }})", fk.References)));
                            createMethod.Statements.Add(varDeclareViewbag);
                        }
                    }
                    else
                    {
                        var varDeclareViewbag = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("ViewBag.{0}s", fk.References)), new CodeVariableReferenceExpression(string.Format("dc.Db<{0}>().Select(x => new {{ id = x.Id, title = x.Title }})", fk.References)));
                        createMethod.Statements.Add(varDeclareViewbag);
                    }
                }
            }
            var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("PartialView({0})", modelName)));
            CodeStatement[] trueStatments = { returnStatement };
            createMethod.Statements.Add(new CodeConditionStatement(new CodeVariableReferenceExpression("Request.IsAjaxRequest()"), trueStatments));
            returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("View({0})", modelName)));
            createMethod.Statements.Add(returnStatement);

            return createMethod;
        }

        public CodeMemberMethod GenerateCreateSubmitMethod(string className, Table table, ApplicationPreferences appPref)
        {
            var createMethod = new CodeMemberMethod();
            createMethod.Attributes = (createMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            createMethod.Name = "Create";
            createMethod.ReturnType = new CodeTypeReference("ActionResult");
            createMethod.CustomAttributes.Add(new CodeAttributeDeclaration { Name = "HttpPost" });
            createMethod.CustomAttributes.Add(new CodeAttributeDeclaration { Name = "ValidateAntiForgeryToken" });
            var modelName = "model";
            var createModelName = "obj";
            createMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("{0}{1}", appPref.ClassNamePrefix, className), modelName));
            var varDeclare = new CodeVariableDeclarationStatement();
            if (appPref.WithKendo)
            {
                varDeclare = new CodeVariableDeclarationStatement("var", "bpr", new CodeSnippetExpression(string.Format("new KendoResponse()")));
                createMethod.Statements.Add(varDeclare);
            }
            else
            {
                varDeclare = new CodeVariableDeclarationStatement("var", "bpr", new CodeSnippetExpression(string.Format("new BatchProcessResult_Model()")));
                createMethod.Statements.Add(varDeclare);
            }
            varDeclare = new CodeVariableDeclarationStatement("var", "dc", new CodeSnippetExpression(string.Format("GetDataContext()")));
            createMethod.Statements.Add(varDeclare);
            varDeclare = new CodeVariableDeclarationStatement(typeof(bool), "noErrorFlag", new CodePrimitiveExpression(true));
            createMethod.Statements.Add(varDeclare);
            foreach (var column in table.Columns)
            {
                var columnMetadata = MetaDataGenerator.UpdateColumnMetaData(table, column);
                if (column.CheckRepeptiveError)
                {
                    var addErrorMesssage = new CodeExpressionStatement();
                    var addFailedBool = new CodeExpression();
                    if (appPref.WithKendo)
                    {
                        addErrorMesssage.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddError", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprRepepetiveError", appPref.ResourceReference, appPref.ErrorResourse)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
                    }
                    else
                    {
                        addErrorMesssage.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddError", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprRepepetiveError", appPref.ResourceReference, appPref.ErrorResourse)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
                    }

                    var codeTrue = new CodeStatement[]{addErrorMesssage
                        , new CodeAssignStatement(new CodeVariableReferenceExpression("noErrorFlag"), new CodePrimitiveExpression(false))};
                    var findErrors = new CodeConditionStatement();
                    if (column.DataType.EndsWith("char"))
                    {
                        findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Any(q => q.{2} == {3}.{2}.StringNormalizer())", appPref.ClassNamePrefix, className, column.Name, modelName)), codeTrue);
                    }
                    else
                    {
                        findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Any(q => q.{2} == {3}.{2})", appPref.ClassNamePrefix, className, column.Name, modelName)), codeTrue);
                    }
                    createMethod.Statements.Add(findErrors);
                }
            }
            if (table.Columns.Any(x => x.IsPrimaryKey && x.IdentitySpecification))
            {
                createMethod.Statements.Add(new CodeMethodInvokeExpression(new CodeSnippetExpression("ModelState"), "Remove", new CodeExpression[] { new CodeSnippetExpression(string.Format("\"Id\"")) }));
            }
            var tryCode = new CodeTryCatchFinallyStatement();
            var objectCreate = new CodeObjectCreateExpression(string.Format("{0}{1}", appPref.ClassNamePrefix, className), new CodeExpression[] { });
            var objCreateDeclration = new CodeVariableDeclarationStatement("var", createModelName, objectCreate);
            var objCreateFields = new List<CodeStatement>();
            foreach (var column in table.Columns)
            {
                if (!(column.IsForeignKey))
                {
                    if (column.IsPrimaryKey)
                    {
                        if (!column.IdentitySpecification)
                        {
                            var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("{0}.{1}", modelName, column.Name)));
                            objCreateFields.Add(newFieldAssignment);
                        }
                    }
                    else if (column.DataType.EndsWith("char"))
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("{0}.{1}.StringNormalizer()", modelName, column.Name)));
                        objCreateFields.Add(newFieldAssignment);
                    }
                    else if (column.DataType.EndsWith("int"))
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("{0}.{1}", modelName, column.Name)));
                        objCreateFields.Add(newFieldAssignment);
                    }
                    else if (column.DataType == "uniqueidentifier")
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("Guid.NewGuid()")));
                        objCreateFields.Add(newFieldAssignment);
                    }
                    else if ((column.DataType == "datetime" || column.DataType == "date") && column.Name.ToLower() == "createdate" && appPref.SystemDefaults)
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("DateTime.Now")));
                        objCreateFields.Add(newFieldAssignment);
                    }
                    else
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("{0}.{1}", modelName, column.Name)));
                        objCreateFields.Add(newFieldAssignment);
                    }
                }
            }
            foreach (var fk in table.ForeignKeys)
            {
                if (!string.IsNullOrEmpty(fk.References))
                {
                    if (fk.References != className)
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, fk.UniquePropertyName)), new CodeSnippetExpression(string.Format(@"new {1} {{ Id = {0}.{2}.Id }}", modelName, fk.References, fk.UniquePropertyName)));
                        objCreateFields.Add(newFieldAssignment);
                    }
                    else
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.Parent{1}", createModelName, fk.UniquePropertyName)), new CodeSnippetExpression(string.Format(@"new {1} {{ Id = {0}.Parent{2}.Id }}", modelName, fk.References, fk.UniquePropertyName)));
                        objCreateFields.Add(newFieldAssignment);
                    }
                }
            }
            var addToDB = new CodeExpressionStatement();
            addToDB.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("dc"), "Add", new CodeExpression[] { new CodeVariableReferenceExpression("obj") });
            var submitToDB = new CodeExpressionStatement();
            submitToDB.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("dc"), "Commit");
            objCreateFields.Insert(0, objCreateDeclration);
            objCreateFields.Add(addToDB);
            objCreateFields.Add(submitToDB);
            if (!appPref.WithKendo)
            {
                var bprAddSuccess = new CodeExpressionStatement();
                bprAddSuccess.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddSuccess", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprAddSuccess", appPref.ResourceReference, appPref.CommonResourseName)), new CodePrimitiveExpression(true) });
                var bprSuccessClientScript = new CodeStatement();
                bprSuccessClientScript = new CodeAssignStatement(new CodeSnippetExpression("bpr.SuccessClientScript"), new CodeSnippetExpression(string.Format("\"$('#searchForm').submit();\"")));
                objCreateFields.Add(bprSuccessClientScript);
                objCreateFields.Add(bprAddSuccess);
            }
            var codeTrueStatement = objCreateFields.ToArray();
            var insertObject = new CodeConditionStatement(new CodeVariableReferenceExpression("noErrorFlag"), codeTrueStatement);
            tryCode.TryStatements.Add(insertObject);
            var addCatchErrorMesssage = new CodeExpressionStatement();
            if (!appPref.WithKendo)
            {
                addCatchErrorMesssage.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddError", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprMainUnknownError", appPref.ResourceReference, appPref.ErrorResourse)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
                var catch1 = new CodeCatchClause();
                catch1.Statements.Add(addCatchErrorMesssage);
                tryCode.CatchClauses.Add(catch1);
            }
            else
            {
                addCatchErrorMesssage.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("AddError"), "AddMessage", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprMainUnknownError", appPref.ResourceReference, appPref.ErrorResourse)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
                var catch1 = new CodeCatchClause();
                catch1.Statements.Add(addCatchErrorMesssage);
                tryCode.CatchClauses.Add(catch1);
            }

            var elseModeState = new List<CodeStatement>();
            elseModeState.Add(tryCode);
            CodeStatement[] objEditFieldsElse2 = { new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddModelStateErrors", new CodeExpression[] { new CodeSnippetExpression(string.Format("ModelState")) })) };
            createMethod.Statements.Add(new CodeConditionStatement(new CodeVariableReferenceExpression("ModelState.IsValid"), elseModeState.ToArray(), objEditFieldsElse2));

            if (appPref.WithKendo)
            {
                var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("Json(bpr, JsonRequestBehavior.AllowGet)"));
                createMethod.Statements.Add(returnStatement);
            }
            else
            {
                var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("PartialView(\"ProcessResult\", bpr)"));
                createMethod.Statements.Add(returnStatement);
            }
            return createMethod;
        }

        public CodeMemberMethod GenerateEditMethod(string className, Table table, ApplicationPreferences appPref)
        {
            var editMethod = new CodeMemberMethod();
            editMethod.Attributes = (editMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            editMethod.Name = "Edit";
            editMethod.ReturnType = new CodeTypeReference("ActionResult");
            var varDeclare = new CodeVariableDeclarationStatement("var", "dc", new CodeSnippetExpression(string.Format("GetDataContext()")));
            editMethod.Statements.Add(varDeclare);
            var modelName = "model";
            if (table != null && table.Columns != null && table.Columns.Any())
            {
                if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
                {
                    editMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Guid), "Id"));
                    var modelDeclare = new CodeVariableDeclarationStatement("var", modelName, new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Single(q => q.{2} == Id)", appPref.ClassNamePrefix, className, table.Columns.First(x => x.DataType == "uniqueidentifier").Name)));
                    editMethod.Statements.Add(modelDeclare);
                }
                else
                {
                    editMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "Id"));
                    var modelDeclare = new CodeVariableDeclarationStatement("var", modelName, new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Single(q => q.{2} == Id)", appPref.ClassNamePrefix, className, table.PrimaryKey.Columns.First().Name)));
                    editMethod.Statements.Add(modelDeclare);
                }
            }
            foreach (var fk in table.ForeignKeys)
            {
                if (!string.IsNullOrEmpty(fk.References))
                {
                    if (fk.References != className)
                    {
                        var tableInf = MetaDataGenerator.GetAvailableLookUpCombos(table.DatabaseName, fk.UniquePropertyName);
                        if (string.IsNullOrEmpty(tableInf))
                        {
                            var varDeclareViewbag = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("ViewBag.{0}s", fk.UniquePropertyName)), new CodeVariableReferenceExpression(string.Format("dc.Db<{0}>().Select(x => new {{ id = x.Id, title = x.Title }})", fk.References)));
                            editMethod.Statements.Add(varDeclareViewbag);
                        }
                        else
                        {
                            var varDeclareViewbag = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("ViewBag.{0}s", fk.UniquePropertyName)), new CodeVariableReferenceExpression(string.Format("dc.Db<{0}>().Select(x => new {{ id = x.Id, title = x.Title }})", fk.References)));
                            editMethod.Statements.Add(varDeclareViewbag);
                        }
                    }
                    else
                    {
                        var varDeclareViewbag = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("ViewBag.{0}s", fk.References)), new CodeVariableReferenceExpression(string.Format("dc.Db<{0}>().Select(x => new {{ id = x.Id, title = x.Title }})", fk.References)));
                        editMethod.Statements.Add(varDeclareViewbag);
                    }
                }
            }
            var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("PartialView({0})", modelName)));
            CodeStatement[] trueStatments = { returnStatement };
            editMethod.Statements.Add(new CodeConditionStatement(new CodeVariableReferenceExpression("Request.IsAjaxRequest()"), trueStatments));
            returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("View({0})", modelName)));
            editMethod.Statements.Add(returnStatement);

            return editMethod;
        }

        public CodeMemberMethod GenerateEditSubmitMethod(string className, Table table, ApplicationPreferences appPref)
        {
            var editMethod = new CodeMemberMethod();
            editMethod.Attributes = (editMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            editMethod.Name = "Edit";
            editMethod.ReturnType = new CodeTypeReference("ActionResult");
            editMethod.CustomAttributes.Add(new CodeAttributeDeclaration { Name = "HttpPost" });
            editMethod.CustomAttributes.Add(new CodeAttributeDeclaration { Name = "ValidateAntiForgeryToken" });
            var modelName = "model";
            var createModelName = "obj";
            editMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("{0}{1}", appPref.ClassNamePrefix, className), modelName));
            var varDeclare = new CodeVariableDeclarationStatement();
            if (appPref.WithKendo)
            {
                varDeclare = new CodeVariableDeclarationStatement("var", "bpr", new CodeSnippetExpression(string.Format("new KendoResponse()")));
                editMethod.Statements.Add(varDeclare);
            }
            else
            {
                varDeclare = new CodeVariableDeclarationStatement("var", "bpr", new CodeSnippetExpression(string.Format("new BatchProcessResult_Model()")));
                editMethod.Statements.Add(varDeclare);
            }
            varDeclare = new CodeVariableDeclarationStatement("var", "dc", new CodeSnippetExpression(string.Format("GetDataContext()")));
            editMethod.Statements.Add(varDeclare);
            varDeclare = new CodeVariableDeclarationStatement(typeof(bool), "noErrorFlag", new CodePrimitiveExpression(true));
            editMethod.Statements.Add(varDeclare);
            foreach (var column in table.Columns)
            {
                var columnMetadata = MetaDataGenerator.UpdateColumnMetaData(table, column);
                if (column.CheckRepeptiveError)
                {
                    var addErrorMesssage = new CodeExpressionStatement();
                    if (appPref.WithKendo)
                    {
                        addErrorMesssage.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddError", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprRepepetiveError", appPref.ResourceReference, appPref.ErrorResourse)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
                    }
                    else
                    {
                        addErrorMesssage.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddError", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprRepepetiveError", appPref.ResourceReference, appPref.ErrorResourse)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
                    }
                    var codeTrue = new CodeStatement[]{addErrorMesssage
                        , new CodeAssignStatement(new CodeVariableReferenceExpression("noErrorFlag"), new CodePrimitiveExpression(false))};
                    if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
                    {
                        var findErrors = new CodeConditionStatement();
                        if (column.DataType.EndsWith("char"))
                        {
                            findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Any(q => q.{2} == {3}.{2}.StringNormalizer() && q.{4} != {3}.{4})", appPref.ClassNamePrefix, className, column.Name, modelName, table.Columns.First(x => x.DataType == "uniqueidentifier").Name)), codeTrue);
                        }
                        else
                        {
                            findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Any(q => q.{2} == {3}.{2} && q.{4} != {3}.{4})", appPref.ClassNamePrefix, className, column.Name, modelName, table.Columns.First(x => x.DataType == "uniqueidentifier").Name)), codeTrue);
                        }
                        editMethod.Statements.Add(findErrors);
                    }
                    else
                    {
                        var findErrors = new CodeConditionStatement();
                        if (column.DataType.EndsWith("char"))
                        {
                            findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Any(q => q.{2} == {3}.{2}.StringNormalizer() && q.Id != {3}.Id)", appPref.ClassNamePrefix, className, column.Name, modelName)), codeTrue);
                        }
                        else
                        {
                            findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Any(q => q.{2} == {3}.{2} && q.Id != {3}.Id)", appPref.ClassNamePrefix, className, column.Name, modelName)), codeTrue);
                        }
                        editMethod.Statements.Add(findErrors);
                    }
                }
            }
            var tryCode = new CodeTryCatchFinallyStatement();
            var objectEdit = new CodeStatement();
            if (table != null && table.Columns != null && table.Columns.Any())
            {
                if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
                {
                    objectEdit = new CodeVariableDeclarationStatement("var", createModelName, new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Single(q => q.{2} == {3}.{2})", appPref.ClassNamePrefix, className, table.Columns.First(x => x.DataType == "uniqueidentifier").Name, modelName)));
                }
                else
                {
                    objectEdit = new CodeVariableDeclarationStatement("var", createModelName, new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Single(q => q.{2} == {3}.{2})", appPref.ClassNamePrefix, className, table.PrimaryKey.Columns.First().Name, modelName)));
                }
            }
            var objEditFields = new List<CodeStatement>();
            foreach (var column in table.Columns.Where(q => q.DataType != "uniqueidentifier"))
            {
                if (!(column.IsForeignKey))
                {
                    if (column.IsPrimaryKey)
                    {
                        if (!column.IdentitySpecification)
                        {
                            var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("{0}.{1}", modelName, column.Name)));
                            objEditFields.Add(newFieldAssignment);
                        }
                    }
                    else if (column.DataType.EndsWith("char"))
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("{0}.{1}.StringNormalizer()", modelName, column.Name)));
                        objEditFields.Add(newFieldAssignment);
                    }
                    else if (column.DataType.EndsWith("int"))
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("{0}.{1}", modelName, column.Name)));
                        objEditFields.Add(newFieldAssignment);
                    }
                    else if ((column.DataType == "datetime" || column.DataType == "date") && column.Name.ToLower() == "createdate" && appPref.SystemDefaults)
                    {
                        //Skip this one
                    }
                    else
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("{0}.{1}", modelName, column.Name)));
                        objEditFields.Add(newFieldAssignment);
                    }

                }
            }
            foreach (var fk in table.ForeignKeys)
            {
                if (!string.IsNullOrEmpty(fk.References))
                {
                    if (fk.References != className)
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, fk.UniquePropertyName)), new CodeSnippetExpression(string.Format(@"new {1} {{ Id = {0}.{2}.Id }}", modelName, fk.References, fk.UniquePropertyName)));
                        objEditFields.Add(newFieldAssignment);
                    }
                    else
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.Parent{1}", createModelName, fk.UniquePropertyName)), new CodeSnippetExpression(string.Format(@"new {1} {{ Id = {0}.Parent{2}.Id }}", modelName, fk.References, fk.UniquePropertyName)));
                        objEditFields.Add(newFieldAssignment);
                    }
                }
            }
            var addToDB = new CodeExpressionStatement();
            addToDB.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("dc"), "Edit", new CodeExpression[] { new CodeVariableReferenceExpression("obj") });
            var submitToDB = new CodeExpressionStatement();
            submitToDB.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("dc"), "Commit");
            objEditFields.Insert(0, objectEdit);
            objEditFields.Add(addToDB);
            objEditFields.Add(submitToDB);
            if (!appPref.WithKendo)
            {
                var bprAddSuccess = new CodeExpressionStatement();
                bprAddSuccess.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddSuccess", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprAddSuccess", appPref.ResourceReference, appPref.CommonResourseName)), new CodePrimitiveExpression(true) });
                var bprSuccessClientScript = new CodeStatement();
                bprSuccessClientScript = new CodeAssignStatement(new CodeSnippetExpression("bpr.SuccessClientScript"), new CodeSnippetExpression(string.Format("\"$('#searchForm').submit();\"")));
                objEditFields.Add(bprSuccessClientScript);
                objEditFields.Add(bprAddSuccess);
            }
            var codeTrueStatement = objEditFields.ToArray();
            var insertObject = new CodeConditionStatement(new CodeVariableReferenceExpression("noErrorFlag"), codeTrueStatement);
            tryCode.TryStatements.Add(insertObject);
            var addCatchErrorMesssage = new CodeExpressionStatement();
            if (!appPref.WithKendo)
            {
                addCatchErrorMesssage.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddError", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprMainUnknownError", appPref.ResourceReference, appPref.ErrorResourse)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
                var catch1 = new CodeCatchClause();
                catch1.Statements.Add(addCatchErrorMesssage);
                tryCode.CatchClauses.Add(catch1);
            }
            else
            {
                addCatchErrorMesssage.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddError", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprMainUnknownError", appPref.ResourceReference, appPref.ErrorResourse)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
                var catch1 = new CodeCatchClause();
                catch1.Statements.Add(addCatchErrorMesssage);
                tryCode.CatchClauses.Add(catch1);
            }
            var elseModeState = new List<CodeStatement>();
            elseModeState.Add(tryCode);
            CodeStatement[] objEditFieldsElse2 = { new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddModelStateErrors", new CodeExpression[] { new CodeSnippetExpression(string.Format("ModelState")) })) };
            editMethod.Statements.Add(new CodeConditionStatement(new CodeVariableReferenceExpression("ModelState.IsValid"), elseModeState.ToArray(), objEditFieldsElse2));
            if (appPref.WithKendo)
            {
                var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("Json(bpr, JsonRequestBehavior.AllowGet)"));
                editMethod.Statements.Add(returnStatement);
            }
            else
            {
                var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("PartialView(\"ProcessResult\", bpr)"));
                editMethod.Statements.Add(returnStatement);
            }
            return editMethod;
        }

        public CodeMemberMethod GenerateDeleteMethod(string className, Table table, ApplicationPreferences appPref)
        {
            var deleteMethod = new CodeMemberMethod();
            deleteMethod.Attributes = (deleteMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            deleteMethod.Name = "Delete";
            deleteMethod.ReturnType = new CodeTypeReference("ActionResult");
            var varDeclare = new CodeVariableDeclarationStatement("var", "dc", new CodeSnippetExpression(string.Format("GetDataContext()")));
            deleteMethod.Statements.Add(varDeclare);
            var modelName = "model";
            if (table != null && table.Columns != null && table.Columns.Any())
            {
                if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
                {
                    deleteMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Guid), "Id"));
                    var modelDeclare = new CodeVariableDeclarationStatement("var", modelName, new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Single(q => q.{2} == Id)", appPref.ClassNamePrefix, className, table.Columns.First(x => x.DataType == "uniqueidentifier").Name)));
                    deleteMethod.Statements.Add(modelDeclare);
                }
                else
                {
                    deleteMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "Id"));
                    var modelDeclare = new CodeVariableDeclarationStatement("var", modelName, new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Single(q => q.{2} == Id)", appPref.ClassNamePrefix, className, table.PrimaryKey.Columns.First().Name)));
                    deleteMethod.Statements.Add(modelDeclare);
                }
            }
            var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("PartialView({0})", modelName)));
            CodeStatement[] trueStatments = { returnStatement };
            deleteMethod.Statements.Add(new CodeConditionStatement(new CodeVariableReferenceExpression("Request.IsAjaxRequest()"), trueStatments));
            returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("View({0})", modelName)));
            deleteMethod.Statements.Add(returnStatement);
            return deleteMethod;
        }

        public CodeMemberMethod GenerateDeleteSubmitMethod(string className, Table table, ApplicationPreferences appPref)
        {
            var deleteMethod = new CodeMemberMethod();
            deleteMethod.Attributes = (deleteMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            deleteMethod.Name = "Delete";
            deleteMethod.ReturnType = new CodeTypeReference("ActionResult");
            deleteMethod.CustomAttributes.Add(new CodeAttributeDeclaration { Name = "HttpPost" });
            deleteMethod.CustomAttributes.Add(new CodeAttributeDeclaration { Name = "ValidateAntiForgeryToken" });
            var modelName = "model";
            deleteMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("{0}{1}", appPref.ClassNamePrefix, className), modelName));
            var varDeclare = new CodeVariableDeclarationStatement();
            if (appPref.WithKendo)
            {
                varDeclare = new CodeVariableDeclarationStatement("var", "bpr", new CodeSnippetExpression(string.Format("new KendoResponse()")));
                deleteMethod.Statements.Add(varDeclare);
            }
            else
            {
                varDeclare = new CodeVariableDeclarationStatement("var", "bpr", new CodeSnippetExpression(string.Format("new BatchProcessResult_Model()")));
                deleteMethod.Statements.Add(varDeclare);
            }
            varDeclare = new CodeVariableDeclarationStatement("var", "dc", new CodeSnippetExpression(string.Format("GetDataContext()")));
            deleteMethod.Statements.Add(varDeclare);
            varDeclare = new CodeVariableDeclarationStatement(typeof(bool), "noErrorFlag", new CodePrimitiveExpression(true));
            deleteMethod.Statements.Add(varDeclare);
            var tryCode = new CodeTryCatchFinallyStatement();
            var objectEdit = new CodeStatement();
            if (table != null && table.Columns != null && table.Columns.Any())
            {
                if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
                {
                    objectEdit = new CodeVariableDeclarationStatement("var", "obj", new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Single(q => q.{2} == {3}.{2})", appPref.ClassNamePrefix, className, table.Columns.First(x => x.DataType == "uniqueidentifier").Name, modelName)));
                }
                else
                {
                    objectEdit = new CodeVariableDeclarationStatement("var", "obj", new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Single(q => q.{2} == {3}.{2})", appPref.ClassNamePrefix, className, table.PrimaryKey.Columns.First().Name, modelName)));
                }
            }
            var objEditFields = new List<CodeStatement>();
            var addToDB = new CodeExpressionStatement();
            addToDB.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("dc"), "Delete", new CodeExpression[] { new CodeVariableReferenceExpression("obj") });
            var submitToDB = new CodeExpressionStatement();
            submitToDB.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("dc"), "Commit");
            objEditFields.Insert(0, objectEdit);
            objEditFields.Add(addToDB);
            objEditFields.Add(submitToDB);
            if (!appPref.WithKendo)
            {
                var bprAddSuccess = new CodeExpressionStatement();
                bprAddSuccess.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddSuccess", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprAddSuccess", appPref.ResourceReference, appPref.CommonResourseName)), new CodePrimitiveExpression(true) });
                var bprSuccessClientScript = new CodeStatement();
                bprSuccessClientScript = new CodeAssignStatement(new CodeSnippetExpression("bpr.SuccessClientScript"), new CodeSnippetExpression(string.Format("\"$('#searchForm').submit();\"")));
                objEditFields.Add(bprSuccessClientScript);
                objEditFields.Add(bprAddSuccess);
            }
            var codeTrueStatement = objEditFields.ToArray();
            var insertObject = new CodeConditionStatement(new CodeVariableReferenceExpression("noErrorFlag"), codeTrueStatement);
            tryCode.TryStatements.Add(insertObject);
            var addCatchErrorMesssage = new CodeExpressionStatement();
            if (!appPref.WithKendo)
            {
                addCatchErrorMesssage.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddError", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprMainUnknownError", appPref.ResourceReference, appPref.ErrorResourse)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
                var catch1 = new CodeCatchClause();
                catch1.Statements.Add(addCatchErrorMesssage);
                tryCode.CatchClauses.Add(catch1);
            }
            else
            {
                addCatchErrorMesssage.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddError", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprMainUnknownError", appPref.ResourceReference, appPref.ErrorResourse)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
                var catch1 = new CodeCatchClause();
                catch1.Statements.Add(addCatchErrorMesssage);
                tryCode.CatchClauses.Add(catch1);
            }
            deleteMethod.Statements.Add(tryCode);
            if (appPref.WithKendo)
            {
                var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("Json(bpr, JsonRequestBehavior.AllowGet)"));
                deleteMethod.Statements.Add(returnStatement);
            }
            else
            {
                var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("PartialView(\"ProcessResult\", bpr)"));
                deleteMethod.Statements.Add(returnStatement);
            }
            return deleteMethod;
        }

        public CodeMemberMethod GeneratetLookUpComboMethod(string className, Table table, Column column, ApplicationPreferences appPref)
        {
            var getMethod = new CodeMemberMethod();
            getMethod.Attributes = (getMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            getMethod.Name = string.Format("Get{0}{1}List", className, column.Name);
            getMethod.ReturnType = new CodeTypeReference("ActionResult");
            var tryCode = new CodeTryCatchFinallyStatement();
            var varDeclare = new CodeVariableDeclarationStatement("var", "dc", new CodeSnippetExpression(string.Format("GetDataContext()")));
            tryCode.TryStatements.Add(varDeclare);
            var modelName = "model";
            if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
            {
                getMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Guid?), "Id"));
                var modelDeclare = new CodeVariableDeclarationStatement("var", modelName, new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Where(q => q.{2} == Id).ToList()", appPref.ClassNamePrefix, className, table.Columns.First(x => x.DataType == "uniqueidentifier").Name, column.Name)));
                tryCode.TryStatements.Add(modelDeclare);
                var resultDeclare = new CodeVariableDeclarationStatement("var", "jsonResult", new CodeSnippetExpression(string.Format("from o in {0} select new {{ id = o.{1}, title = o.{2} ,selected = false }}", modelName, table.Columns.First(x => x.DataType == "uniqueidentifier").Name, column.Name)));
                tryCode.TryStatements.Add(resultDeclare);
            }
            else
            {
                getMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int?), "Id"));
                var modelDeclare = new CodeVariableDeclarationStatement("var", modelName, new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Where(q => q.{2} == Id).ToList()", appPref.ClassNamePrefix, className, table.PrimaryKey.Columns.First().Name, column.Name)));
                tryCode.TryStatements.Add(modelDeclare);
                var resultDeclare = new CodeVariableDeclarationStatement("var", "jsonResult", new CodeSnippetExpression(string.Format("from o in {0} select new {{ id = o.{1}, title = o.{2} }}", modelName, table.PrimaryKey.Columns.First().Name, column.Name)));
                tryCode.TryStatements.Add(resultDeclare);
            }
            tryCode.TryStatements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("Json(jsonResult, JsonRequestBehavior.AllowGet)"))));
            var catch1 = new CodeCatchClause();
            catch1.Statements.Add(new CodeArgumentReferenceExpression(string.Format("Json(null, JsonRequestBehavior.AllowGet)")));
            tryCode.CatchClauses.Add(catch1);
            var findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("Id.HasValue", className, column.Name, modelName)), tryCode);
            getMethod.Statements.Add(findErrors);
            getMethod.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("Json(null, JsonRequestBehavior.AllowGet)"))));
            return getMethod;
        }

        public CodeMemberMethod GeneratetLookUpComboMethodDAL(string className, Table table, Column column, ApplicationPreferences appPref)
        {
            var getMethod = new CodeMemberMethod();
            getMethod.Attributes = (getMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            getMethod.Name = string.Format("Get{0}{1}List", className, column.Name);
            getMethod.ReturnType = new CodeTypeReference("ActionResult");
            var tryCode = new CodeTryCatchFinallyStatement();
            var modelName = "model";
            if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
            {
                getMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Guid?), "Id"));
                var modelDeclare = new CodeVariableDeclarationStatement("var", modelName, new CodeSnippetExpression(string.Format("dc.{0}{1}DAL.GetAll()", appPref.ClassNamePrefix, className, table.Columns.First(x => x.DataType == "uniqueidentifier").Name, column.Name)));
                tryCode.TryStatements.Add(modelDeclare);
                var resultDeclare = new CodeVariableDeclarationStatement("var", "jsonResult", new CodeSnippetExpression(string.Format("from o in {0} select new {{ id = o.{1}, title = o.{2} ,selected = false }}", modelName, table.Columns.First(x => x.DataType == "uniqueidentifier").Name, column.Name)));
                tryCode.TryStatements.Add(resultDeclare);
            }
            else
            {
                getMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int?), "Id"));
                var modelDeclare = new CodeVariableDeclarationStatement("var", modelName, new CodeSnippetExpression(string.Format("dc.{0}{1}DAL.GetAll()", appPref.ClassNamePrefix, className, table.PrimaryKey.Columns.First().Name, column.Name)));
                tryCode.TryStatements.Add(modelDeclare);
                var resultDeclare = new CodeVariableDeclarationStatement("var", "jsonResult", new CodeSnippetExpression(string.Format("from o in {0} select new {{ id = o.{1}, title = o.{2} }}", modelName, table.PrimaryKey.Columns.First().Name, column.Name)));
                tryCode.TryStatements.Add(resultDeclare);
            }
            tryCode.TryStatements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("Json(jsonResult, JsonRequestBehavior.AllowGet)"))));
            var catch1 = new CodeCatchClause();
            catch1.Statements.Add(new CodeArgumentReferenceExpression(string.Format("Json(null, JsonRequestBehavior.AllowGet)")));
            tryCode.CatchClauses.Add(catch1);
            var findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("Id.HasValue", className, column.Name, modelName)), tryCode);
            getMethod.Statements.Add(findErrors);
            getMethod.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("Json(null, JsonRequestBehavior.AllowGet)"))));
            return getMethod;
        }

        public CodeMemberMethod GenerateLookUpLabelMethod(string className, Table table, Column column, ApplicationPreferences appPref)
        {
            var getMethod = new CodeMemberMethod();
            getMethod.Attributes = (getMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            getMethod.Name = string.Format("Get{0}{1}", className, column.Name);
            getMethod.ReturnType = new CodeTypeReference("ActionResult");
            var tryCode = new CodeTryCatchFinallyStatement();
            var varDeclare = new CodeVariableDeclarationStatement("var", "dc", new CodeSnippetExpression(string.Format("GetDataContext()")));
            tryCode.TryStatements.Add(varDeclare);
            var modelName = "model";
            if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
            {
                getMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Guid?), "Id"));
                var modelDeclare = new CodeVariableDeclarationStatement("var", modelName, new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Single(q => q.{2} == Id).{3}", appPref.ClassNamePrefix, className, table.Columns.First(x => x.DataType == "uniqueidentifier").Name, column.Name)));
                tryCode.TryStatements.Add(modelDeclare);
            }
            else
            {
                getMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int?), "Id"));
                var modelDeclare = new CodeVariableDeclarationStatement("var", modelName, new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Single(q => q.{2} == Id).{3}", appPref.ClassNamePrefix, className, table.PrimaryKey.Columns.First().Name, column.Name)));
                tryCode.TryStatements.Add(modelDeclare);
            }
            tryCode.TryStatements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("Content({0})", modelName))));
            var catch1 = new CodeCatchClause();
            catch1.Statements.Add(new CodeArgumentReferenceExpression(string.Format("Content(\"\")")));
            tryCode.CatchClauses.Add(catch1);
            var findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("Id.HasValue", className, column.Name, modelName)), tryCode);
            getMethod.Statements.Add(findErrors);
            getMethod.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("Content(\"\")"))));
            return getMethod;
        }

        public CodeMemberMethod GenerateLookUpLabelMethodDAL(string className, Table table, Column column, ApplicationPreferences appPref)
        {
            var getMethod = new CodeMemberMethod();
            getMethod.Attributes = (getMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            getMethod.Name = string.Format("Get{0}{1}", className, column.Name);
            getMethod.ReturnType = new CodeTypeReference("ActionResult");
            var tryCode = new CodeTryCatchFinallyStatement();
            var modelName = "model";
            if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
            {
                getMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Guid?), "Id"));
                var modelDeclare = new CodeVariableDeclarationStatement("var", modelName, new CodeSnippetExpression(string.Format("dc.{0}{1}DAL.Get(Id).{2}", appPref.ClassNamePrefix, className, column.Name)));
                tryCode.TryStatements.Add(modelDeclare);
            }
            else
            {
                getMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int?), "Id"));
                var modelDeclare = new CodeVariableDeclarationStatement("var", modelName, new CodeSnippetExpression(string.Format("dc.{0}{1}DAL.Get(Id).{2}", appPref.ClassNamePrefix, className, column.Name)));
                tryCode.TryStatements.Add(modelDeclare);
            }
            tryCode.TryStatements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("Content({0})", modelName))));
            var catch1 = new CodeCatchClause();
            catch1.Statements.Add(new CodeArgumentReferenceExpression(string.Format("Content(\"\")")));
            tryCode.CatchClauses.Add(catch1);
            var findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("Id.HasValue", className, column.Name, modelName)), tryCode);
            getMethod.Statements.Add(findErrors);
            getMethod.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("Content(\"\")"))));
            return getMethod;
        }

        public CodeMemberProperty GeneratetSearchModel(string columnName, string columnType, ApplicationPreferences appPref, string propertyName, string tableName)
        {
            var codeMemberProperty = new CodeMemberProperty();
            if (columnType.ToLower().EndsWith("string"))
            {
                codeMemberProperty = new CodeMemberProperty
               {
                   Name = "Search" + columnName,
                   HasGet = true,
                   HasSet = true,
                   Type = new CodeTypeReference(columnType)
               };
            }
            else
            {
                codeMemberProperty = new CodeMemberProperty
               {
                   Name = "Search" + columnName,
                   HasGet = true,
                   HasSet = true,
                   Type = new CodeTypeReference(String.Format("System.Nullable<{0}>", columnType))
               };
            }
            codeMemberProperty.Attributes = (codeMemberProperty.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            if (!string.IsNullOrEmpty(propertyName))
            {
                var codeAttrArg1 = new CodeAttributeArgument(new CodeSnippetExpression(string.Format("ResourceType = typeof({0}.{1}{2}{3})", appPref.ResourceReference, appPref.TablePrefix, tableName, appPref.TablePost)));
                var codeAttrArg2 = new CodeAttributeArgument(new CodeSnippetExpression(string.Format("Name = \"{0}\"", propertyName)));
                var codeAttrDecl = new CodeAttributeDeclaration("Display", codeAttrArg2, codeAttrArg1);
                codeMemberProperty.CustomAttributes.Add(codeAttrDecl);
            }
            return codeMemberProperty;
        }

        public CodeMemberMethod GenerateIndex(string className, ApplicationPreferences appPref, int type)
        {
            var method = new CodeMemberMethod();
            if (type == 1)
            {
                method.Attributes = (method.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
                method.Name = "Index";
                method.ReturnType = new CodeTypeReference("ActionResult");
                var modelName = "model";
                method.Parameters.Add(new CodeParameterDeclarationExpression(className + "SearchModel", modelName));
                method.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(String.Format("{0}.Area", modelName)), new CodeSnippetExpression(String.Format("\"\""))));
                var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("View({0})", modelName)));
                method.Statements.Add(returnStatement);
            }
            else if (type == 2)
            {
                method.Attributes = (method.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
                method.Name = className + "SearchModelView";
                method.ReturnType = new CodeTypeReference("ActionResult");
                var modelName = "model";
                method.Parameters.Add(new CodeParameterDeclarationExpression(className + "SearchModel", modelName));
                method.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(String.Format("{0}.Area", modelName)), new CodeSnippetExpression(String.Format("\"\""))));
                var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("PartialView({0})", modelName)));
                method.Statements.Add(returnStatement);
            }
            return method;
        }

        public CodeMemberMethod GenerateIX(Table table, string className, ApplicationPreferences appPref)
        {
            var method = new CodeMemberMethod();
            method.Attributes = (method.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            method.Name = "IX";
            method.ReturnType = new CodeTypeReference("ActionResult");
            var modelName = "model";
            method.Parameters.Add(new CodeParameterDeclarationExpression(className + "SearchModel", modelName));
            method.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(String.Format("{0}.Area", modelName)), new CodeSnippetExpression(String.Format("\"\""))));

            var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("PartialView(SearchFilters({0}))", modelName)));
            method.Statements.Add(returnStatement);
            return method;
        }

        public CodeMemberMethod GenerateSearchFilters(Table table, string className, ApplicationPreferences appPref)
        {
            var method = new CodeMemberMethod();
            method.Attributes = (method.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            method.Name = "SearchFilters";
            method.ReturnType = new CodeTypeReference(string.Format("PagedList<{0}{1}>", appPref.ClassNamePrefix, className));
            var modelName = "model";
            method.Parameters.Add(new CodeParameterDeclarationExpression(className + "SearchModel", modelName));
            method.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(String.Format("{0}.Area", modelName)), new CodeSnippetExpression(String.Format("\"\""))));

            var varDeclare = new CodeVariableDeclarationStatement("var", "dc", new CodeSnippetExpression(string.Format("GetDataContext()")));
            method.Statements.Add(varDeclare);
            method.Statements.Add(new CodeVariableDeclarationStatement("var", "qry", new CodeSnippetExpression(string.Format("from q in dc.Db<{0}{1}>() select q", appPref.ClassNamePrefix, className))));
            var codeTrue = new List<CodeStatement>();
            foreach (var column in table.Columns.Where(x => x.InSearch))
            {
                if (!table.ForeignKeys.Any(x => x.Name == column.Name))
                {
                    if (column.DataType.EndsWith("char"))
                    {
                        codeTrue.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("!string.IsNullOrEmpty({0}.Search{1})", modelName, column.Name)), new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.Where(u => u.{0}.Contains({1}.Search{0}.StringNormalizer()))", column.Name, modelName)))));
                    }
                    else if (column.DataType.EndsWith("int"))
                    {
                        codeTrue.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0}.Search{1}.HasValue && {0}.Search{1} > 0", modelName, column.Name)), new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.Where(u => u.{0} == {1}.Search{0})", column.Name, modelName)))));
                    }
                    else if (column.DataType.StartsWith("date"))
                    {
                        codeTrue.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0}.Search{1}.HasValue && {0}.Search{1} > DateTime.MinValue", modelName, column.Name)), new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.Where(u => u.{0} == {1}.Search{0})", column.Name, modelName)))));
                    }
                    else
                    {
                        codeTrue.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0}.Search{1} != null && {0}.Search{1}.HasValue", modelName, column.Name)), new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.Where(u => u.{0} == {1}.Search{0})", column.Name, modelName)))));
                    }
                }
                else
                {
                    if (table.ForeignKeys.Single(x => x.Name == column.Name).References != className)
                    {
                        codeTrue.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0}.Search{1} > 0", modelName, column.Name)), new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.Where(u => u.{0}.Id == {2}.Search{1})", table.ForeignKeys.Single(x => x.Name == column.Name).UniquePropertyName, column.Name, modelName)))));
                    }
                    else
                    {
                        codeTrue.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0}.Search{1} > 0", modelName, column.Name)), new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.Where(u => u.Parent{0}.Id == {2}.Search{1})", table.ForeignKeys.Single(x => x.Name == column.Name).UniquePropertyName, column.Name, modelName)))));
                    }
                }
            }
            var filterSearch = new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0} != null", modelName)), codeTrue.ToArray());
            method.Statements.Add(filterSearch);
            method.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.OrderBy(u => u.Id)"))));
            method.Statements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(modelName), "Update", new CodeSnippetExpression(string.Format("{0}.PageSize, qry.Count()", modelName)))));
            method.Statements.Add(new CodeVariableDeclarationStatement("var", "ls", new CodeSnippetExpression(string.Format("qry.Skip(model.PageSize * {0}.PageIndex).Take({0}.PageSize).ToList()", modelName))));
            method.Statements.Add(new CodeVariableDeclarationStatement("var", "ql", new CodeSnippetExpression(string.Format("new PagedList<{0}{1}>(ls, {2})", appPref.ClassNamePrefix, className, modelName))));
            var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("ql")));
            method.Statements.Add(returnStatement);
            return method;
        }

        public CodeMemberMethod GenerateIXKendo(Table table, string className, ApplicationPreferences appPref)
        {
            var method = new CodeMemberMethod();
            method.Attributes = (method.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            method.Name = "IX";
            method.ReturnType = new CodeTypeReference("ActionResult");
            var modelName = "model";
            method.Parameters.Add(new CodeParameterDeclarationExpression(className + "SearchModel", modelName));
            var codeTrue = new CodeStatement[]{new CodeAssignStatement(new CodeSnippetExpression(string.Format("{0}.pageSize", modelName)) , new CodePrimitiveExpression(10))                        
                        ,new CodeAssignStatement(new CodeSnippetExpression(string.Format("{0}.take", modelName)) , new CodePrimitiveExpression(10))   
                        , new CodeAssignStatement(new CodeSnippetExpression(string.Format("{0}.page", modelName) ) , new CodePrimitiveExpression(1))   };
            method.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(String.Format("{0}.Area", modelName)), new CodeSnippetExpression(String.Format("\"\""))));

            method.Statements.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0}.page == 0", modelName)), codeTrue));
            method.Statements.Add(new CodeAssignStatement(new CodeSnippetExpression(string.Format("{0}.SearchString", modelName)), new CodeSnippetExpression(string.Format("{0}.GetSearchString()", modelName))));
            var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("PartialView({0})", modelName)));
            method.Statements.Add(returnStatement);
            return method;
        }

        public CodeMemberMethod GenerateIXSearch(Table table, string className, ApplicationPreferences appPref)
        {
            var method = new CodeMemberMethod();
            method.Attributes = (method.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            method.Name = "IXSearch";
            method.ReturnType = new CodeTypeReference("ActionResult");
            var modelName = "model";
            method.Parameters.Add(new CodeParameterDeclarationExpression(className + "SearchModel", modelName));
            method.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(String.Format("{0}.Area", modelName)), new CodeSnippetExpression(String.Format("\"\""))));

            var varDeclare = new CodeVariableDeclarationStatement("var", "dc", new CodeSnippetExpression(string.Format("GetDataContext()")));
            method.Statements.Add(varDeclare);
            method.Statements.Add(new CodeVariableDeclarationStatement("var", "qry", new CodeSnippetExpression(string.Format("from q in dc.Db<{0}{1}>() select q", appPref.ClassNamePrefix, className))));
            var codeTrue = new List<CodeStatement>();
            foreach (var column in table.Columns.Where(x => x.InSearch))
            {
                if (!table.ForeignKeys.Any(x => x.Name == column.Name))
                {
                    if (column.DataType.EndsWith("char"))
                    {
                        codeTrue.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("!string.IsNullOrEmpty({0}.Search{1})", modelName, column.Name)), new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.Where(u => u.{0}.Contains({1}.{0}.StringNormalizer()))", column.Name, modelName)))));
                    }
                    else if (column.DataType.EndsWith("int"))
                    {
                        codeTrue.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0}.Search{1}.HasValue && {0}.Search{1} > 0", modelName, column.Name)), new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.Where(u => u.{0} == {1}.{0})", column.Name, modelName)))));
                    }
                    else if (column.DataType.StartsWith("date"))
                    {
                        codeTrue.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0}.Search{1}.HasValue && {0}.Search{1} > DateTime.MinValue", modelName, column.Name)), new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.Where(u => u.{0} == {1}.{0})", column.Name, modelName)))));
                    }
                    else
                    {
                        codeTrue.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0}.Search{1} != null && {0}.Search{1}.HasValue", modelName, column.Name)), new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.Where(u => u.{0} == {1}.{0})", column.Name, modelName)))));
                    }
                }
                else
                {
                    if (table.ForeignKeys.Single(x => x.Name == column.Name).References != className)
                    {
                        codeTrue.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0}.Search{1} > 0", modelName, column.Name)), new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.Where(u => u.{0}.Id == {2}.{1})", table.ForeignKeys.Single(x => x.Name == column.Name).UniquePropertyName, column.Name, modelName)))));
                    }
                    else
                    {
                        codeTrue.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0}.Search{1} > 0", modelName, column.Name)), new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.Where(u => u.Parent{0}.Id == {2}.{1})", table.ForeignKeys.Single(x => x.Name == column.Name).UniquePropertyName, column.Name, modelName)))));
                    }
                }
            }
            var filterSearch = new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0} != null", modelName)), codeTrue.ToArray());
            method.Statements.Add(filterSearch);
            method.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.OrderBy(u => u.Id)"))));
            method.Statements.Add(new CodeVariableDeclarationStatement("var", "returnModel", new CodeSnippetExpression(string.Format("new KendoResponse()"))));
            method.Statements.Add(new CodeAssignStatement(new CodeSnippetExpression(string.Format("returnModel.Data")), new CodeSnippetExpression(string.Format("qry.Skip({0}.skip).Take({0}.take).ToList()", modelName))));
            method.Statements.Add(new CodeAssignStatement(new CodeSnippetExpression(string.Format("returnModel.Count")), new CodeSnippetExpression(string.Format("qry.Count()"))));
            var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("Json(returnModel, JsonRequestBehavior.AllowGet)")));
            method.Statements.Add(returnStatement);
            return method;
        }

        public CodeTypeDeclaration GenerateGetDALMethod(string className, Table table, ApplicationPreferences appPref, CodeTypeDeclaration codeType, byte ORMType)
        {
            var getMethod = new CodeMemberMethod();
            getMethod.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "Get"));
            bool flag = false;
            foreach (var pk in table.Columns.Where(x => x.IsPrimaryKey || x.IsUnique || x.DataType == "uniqueidentifier"))
            {
                if (flag)
                {
                    getMethod = new CodeMemberMethod();
                }
                getMethod.Attributes = (getMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public | MemberAttributes.Static;
                getMethod.Name = "Get";
                getMethod.ReturnType = new CodeTypeReference(string.Format("{0}{1}", appPref.ClassNamePrefix, className));
                getMethod.Parameters.Add(new CodeParameterDeclarationExpression(appPref.DataContextName, "dc"));
                getMethod.Parameters.Add(new CodeParameterDeclarationExpression(pk.MappedDataType, pk.Name));
                if (ORMType == 1)
                {
                    var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("dc.{0}{1}.SingleOrDefault(u => u.{2} == {2})", appPref.ClassNamePrefix, Pluralizer.IsSingular(className) ? Pluralizer.Pluralize(className) : className, pk.Name)));
                    getMethod.Statements.Add(returnStatement);
                }
                else if (ORMType == 2)
                {
                    var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("dc.Db<{0}{1}>().SingleOrDefault(u => u.{2} == {2})", appPref.ClassNamePrefix, className, pk.Name)));
                    getMethod.Statements.Add(returnStatement);
                }
                codeType.Members.Add(getMethod);
                getMethod = new CodeMemberMethod();
                flag = true;
                getMethod.Attributes = (getMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public | MemberAttributes.Static;
                getMethod.Name = "Get";
                getMethod.ReturnType = new CodeTypeReference(string.Format("{0}{1}", appPref.ClassNamePrefix, className));
                getMethod.Parameters.Add(new CodeParameterDeclarationExpression(pk.MappedDataType, pk.Name));
                var returnStatement2 = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("Get({0}.Get{1}, {2})", appPref.DBUtilityName, appPref.DataContextName, pk.Name)));
                getMethod.Statements.Add(returnStatement2);
                codeType.Members.Add(getMethod);
            }
            getMethod.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, string.Empty));

            return codeType;
        }

        public CodeTypeDeclaration GenerateGetAllDALMethod(string className, Table table, ApplicationPreferences appPref, CodeTypeDeclaration codeType, byte ORMType)
        {
            var getMethod = new CodeMemberMethod();
            getMethod.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "GetAll"));
            getMethod.Attributes = (getMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public | MemberAttributes.Static;
            getMethod.Name = "GetAll";
            getMethod.ReturnType = new CodeTypeReference(string.Format("List<{0}{1}>", appPref.ClassNamePrefix, className));
            getMethod.Parameters.Add(new CodeParameterDeclarationExpression(appPref.DataContextName, "dc"));
            if (ORMType == 1)
            {
                var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("dc.{0}{1}.ToList()", appPref.ClassNamePrefix, Pluralizer.IsSingular(className) ? Pluralizer.Pluralize(className) : className)));
                getMethod.Statements.Add(returnStatement);
            }
            else if (ORMType == 2)
            {
                var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("dc.Db<{0}{1}>().ToList()", appPref.ClassNamePrefix, className)));
                getMethod.Statements.Add(returnStatement);
            }
            codeType.Members.Add(getMethod);
            getMethod = new CodeMemberMethod();
            getMethod.Attributes = (getMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public | MemberAttributes.Static;
            getMethod.Name = "GetAll";
            getMethod.ReturnType = new CodeTypeReference(string.Format("List<{0}{1}>", appPref.ClassNamePrefix, className));
            var returnStatement2 = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("GetAll({0}.Get{1})", appPref.DBUtilityName, appPref.DataContextName)));
            getMethod.Statements.Add(returnStatement2);
            getMethod.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, string.Empty));
            codeType.Members.Add(getMethod);
            return codeType;
        }

        public CodeTypeDeclaration GenerateFindDALMethod(string className, Table table, ApplicationPreferences appPref, CodeTypeDeclaration codeType, byte ORMType)
        {
            var modelName = "model";
            var findMethod = new CodeMemberMethod();
            findMethod.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "Find"));
            findMethod.Attributes = (findMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public | MemberAttributes.Static;
            findMethod.Name = "Find";
            findMethod.ReturnType = new CodeTypeReference(string.Format("IQueryable<{0}{1}>", appPref.ClassNamePrefix, className));
            findMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("{0}{1}SearchModel", appPref.ClassNamePrefix, className), "model"));
            var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("Find({0}.Get{1}, model)", appPref.DBUtilityName, appPref.DataContextName)));
            findMethod.Statements.Add(returnStatement);
            codeType.Members.Add(findMethod);
            findMethod = new CodeMemberMethod();
            findMethod.Attributes = (findMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public | MemberAttributes.Static;
            findMethod.Name = "Find";
            findMethod.ReturnType = new CodeTypeReference(string.Format("IQueryable<{0}{1}>", appPref.ClassNamePrefix, className));
            findMethod.Parameters.Add(new CodeParameterDeclarationExpression(appPref.DataContextName, "dc"));
            findMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("{0}{1}SearchModel", appPref.ClassNamePrefix, className), "model"));
            if (ORMType == 1)
            {
                findMethod.Statements.Add(new CodeVariableDeclarationStatement("var", "qry", new CodeSnippetExpression(string.Format("from p in dc.{0}{1} select p", appPref.ClassNamePrefix, Pluralizer.IsSingular(className) ? Pluralizer.Pluralize(className) : className))));
            }
            else if (ORMType == 2)
            {
                findMethod.Statements.Add(new CodeVariableDeclarationStatement("var", "qry", new CodeSnippetExpression(string.Format("from q in dc.Db<{0}{1}>() select q", appPref.ClassNamePrefix, className))));
            }
            var codeTrue = new List<CodeStatement>();
            foreach (var column in table.Columns.Where(x => x.InSearch))
            {
                if (!table.ForeignKeys.Any(x => x.Name == column.Name))
                {
                    if (column.DataType.EndsWith("char"))
                    {
                        codeTrue.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("!string.IsNullOrEmpty({0}.Search{1})", modelName, column.Name)), new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.Where(u => u.{0}.Contains({1}.Search{0}.StringNormalizer()))", column.Name, modelName)))));
                    }
                    else if (column.DataType.EndsWith("int"))
                    {
                        codeTrue.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0}.Search{1}.HasValue && {0}.Search{1} > 0", modelName, column.Name)), new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.Where(u => u.{0} == {1}.Search{0})", column.Name, modelName)))));
                    }
                    else if (column.DataType.StartsWith("date"))
                    {
                        codeTrue.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0}.Search{1}.HasValue && {0}.Search{1} > DateTime.MinValue", modelName, column.Name)), new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.Where(u => u.{0} == {1}.Search{0})", column.Name, modelName)))));
                    }
                    else
                    {
                        codeTrue.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0}.Search{1} != null && {0}.Search{1}.HasValue", modelName, column.Name)), new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.Where(u => u.{0} == {1}.Search{0})", column.Name, modelName)))));
                    }
                }
                else
                {
                    if (table.ForeignKeys.Single(x => x.Name == column.Name).References != className)
                    {
                        codeTrue.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0}.Search{1} > 0", modelName, column.Name)), new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.Where(u => u.{0}.Id == {2}.Search{1})", table.ForeignKeys.Single(x => x.Name == column.Name).UniquePropertyName, column.Name, modelName)))));
                    }
                    else
                    {
                        codeTrue.Add(new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0}.Search{1} > 0", modelName, column.Name)), new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.Where(u => u.Parent{0}.Id == {2}.Search{1})", table.ForeignKeys.Single(x => x.Name == column.Name).UniquePropertyName, column.Name, modelName)))));
                    }
                }
            }
            var filterSearch = new CodeConditionStatement(new CodeSnippetExpression(string.Format("{0} != null", modelName)), codeTrue.ToArray());
            findMethod.Statements.Add(filterSearch);
            if (appPref.WithSortExpression)
            {
                codeTrue.Clear();
                codeTrue.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.OrderBy({0}.SortExpression)", modelName))));
                filterSearch = new CodeConditionStatement(new CodeSnippetExpression(string.Format("!string.IsNullOrEmpty({0}.SortExpression)", modelName)), codeTrue.ToArray());
                findMethod.Statements.Add(filterSearch);
            }
            findMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("qry"), new CodeSnippetExpression(string.Format("qry.OrderBy(u => u.Id)"))));
            returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("qry")));
            findMethod.Statements.Add(returnStatement);
            findMethod.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, string.Empty));
            codeType.Members.Add(findMethod);
            return codeType;
        }

        public CodeTypeDeclaration GenerateAddDALMethod(string className, Table table, ApplicationPreferences appPref, CodeTypeDeclaration codeType, byte ORMType)
        {
            var modelName = "model";
            var createModelName = "obj";
            var addMethod = new CodeMemberMethod();
            addMethod.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "Add"));
            addMethod.Attributes = (addMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public | MemberAttributes.Static;
            addMethod.Name = "Add";
            addMethod.ReturnType = new CodeTypeReference(string.Format("{0}{1}", appPref.ClassNamePrefix, className));
            addMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("{0}{1}", appPref.ClassNamePrefix, className), "model"));
            addMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("out BatchProcessResult_Model"), "bpr"));
            var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("Add({0}.Get{1}, model, out bpr)", appPref.DBUtilityName, appPref.DataContextName)));
            addMethod.Statements.Add(returnStatement);
            codeType.Members.Add(addMethod);
            addMethod = new CodeMemberMethod();
            addMethod.Attributes = (addMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public | MemberAttributes.Static;
            addMethod.Name = "Add";
            addMethod.ReturnType = new CodeTypeReference(string.Format("{0}{1}", appPref.ClassNamePrefix, className));
            addMethod.Parameters.Add(new CodeParameterDeclarationExpression(appPref.DataContextName, "dc"));
            addMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("{0}{1}", appPref.ClassNamePrefix, className), "model"));
            addMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("out BatchProcessResult_Model"), "bpr"));
            addMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("bpr"), new CodeSnippetExpression(string.Format("new BatchProcessResult_Model()"))));
            var varDeclare = new CodeVariableDeclarationStatement(typeof(bool), "noErrorFlag", new CodePrimitiveExpression(true));
            addMethod.Statements.Add(varDeclare);
            foreach (var column in table.Columns)
            {
                var columnMetadata = MetaDataGenerator.UpdateColumnMetaData(table, column);
                if (column.CheckRepeptiveError)
                {
                    var addErrorMesssage = new CodeExpressionStatement();
                    addErrorMesssage.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddError", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprRepepetiveError", appPref.ResourceReference, appPref.ErrorResourse)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
                    var codeTrue = new CodeStatement[]{
                        addErrorMesssage
                        , new CodeAssignStatement(new CodeVariableReferenceExpression("noErrorFlag"), new CodePrimitiveExpression(false))};
                    var findErrors = new CodeConditionStatement();
                    if (ORMType == 1)
                    {
                        if (column.DataType.EndsWith("char"))
                        {
                            findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("dc.{0}{1}.Any(q => q.{2} == {3}.{2}.StringNormalizer())", appPref.ClassNamePrefix, Pluralizer.IsSingular(className) ? Pluralizer.Pluralize(className) : className, column.Name, modelName)), codeTrue);
                        }
                        else
                        {
                            findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("dc.{0}{1}.Any(q => q.{2} == {3}.{2})", appPref.ClassNamePrefix, Pluralizer.IsSingular(className) ? Pluralizer.Pluralize(className) : className, column.Name, modelName)), codeTrue);
                        }
                    }
                    else if (ORMType == 2)
                    {
                        if (column.DataType.EndsWith("char"))
                        {
                            findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Any(q => q.{2} == {3}.{2}.StringNormalizer())", appPref.ClassNamePrefix, className, column.Name, modelName)), codeTrue);
                        }
                        else
                        {
                            findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Any(q => q.{2} == {3}.{2})", appPref.ClassNamePrefix, className, column.Name, modelName)), codeTrue);
                        }
                    }

                    addMethod.Statements.Add(findErrors);
                }
            }
            if (table.Columns.Any(x => x.IsPrimaryKey && x.IdentitySpecification))
            {
                addMethod.Statements.Add(new CodeMethodInvokeExpression(new CodeSnippetExpression("ModelState"), "Remove", new CodeExpression[] { new CodeSnippetExpression(string.Format("\"Id\"")) }));
            }
            var tryCode = new CodeTryCatchFinallyStatement();
            var objectCreate = new CodeObjectCreateExpression(string.Format("{0}{1}", appPref.ClassNamePrefix, className), new CodeExpression[] { });
            var objCreateDeclration = new CodeVariableDeclarationStatement("var", createModelName, objectCreate);
            var objCreateFields = new List<CodeStatement>();
            foreach (var column in table.Columns)
            {
                if (!(column.IsForeignKey))
                {
                    if (column.IsPrimaryKey)
                    {
                        if (!column.IdentitySpecification)
                        {
                            var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("{0}.{1}", modelName, column.Name)));
                            objCreateFields.Add(newFieldAssignment);
                        }
                    }
                    else if (column.DataType.EndsWith("char"))
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("{0}.{1}.StringNormalizer()", modelName, column.Name)));
                        objCreateFields.Add(newFieldAssignment);
                    }
                    else if (column.DataType.EndsWith("int"))
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("{0}.{1}", modelName, column.Name)));
                        objCreateFields.Add(newFieldAssignment);
                    }
                    else if (column.DataType == "uniqueidentifier")
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("Guid.NewGuid()")));
                        objCreateFields.Add(newFieldAssignment);
                    }
                    else if ((column.DataType == "datetime" || column.DataType == "date") && column.Name.ToLower() == "createdate" && appPref.SystemDefaults)
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("DateTime.Now")));
                        objCreateFields.Add(newFieldAssignment);
                    }
                    else
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("{0}.{1}", modelName, column.Name)));
                        objCreateFields.Add(newFieldAssignment);
                    }
                }
            }
            foreach (var fk in table.ForeignKeys)
            {
                if (!string.IsNullOrEmpty(fk.References))
                {
                    if (ORMType == 1)
                    {
                        if (fk.References != className)
                        {
                            var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, fk.Name)), new CodeSnippetExpression(string.Format("{0}.{1}", modelName, fk.Name)));
                            objCreateFields.Add(newFieldAssignment);
                        }
                        else
                        {
                            var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, fk.Name)), new CodeSnippetExpression(string.Format("{0}.{1}", modelName, fk.Name)));
                            objCreateFields.Add(newFieldAssignment);
                        }
                    }
                    else if (ORMType == 2)
                    {
                        if (fk.References != className)
                        {
                            var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, fk.UniquePropertyName)), new CodeSnippetExpression(string.Format(@"new {1} {{ Id = {0}.{2}.Id }}", modelName, fk.References, fk.UniquePropertyName)));
                            objCreateFields.Add(newFieldAssignment);
                        }
                        else
                        {
                            var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.Parent{1}", createModelName, fk.UniquePropertyName)), new CodeSnippetExpression(string.Format(@"new {1} {{ Id = {0}.Parent{2}.Id }}", modelName, fk.References, fk.UniquePropertyName)));
                            objCreateFields.Add(newFieldAssignment);
                        }
                    }
                }
            }
            var addToDB = new CodeExpressionStatement();
            if (ORMType == 1)
            {
                addToDB.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression(string.Format("dc.{0}{1}", appPref.ClassNamePrefix, Pluralizer.IsSingular(className) ? Pluralizer.Pluralize(className) : className)), "InsertOnSubmit", new CodeExpression[] { new CodeVariableReferenceExpression("obj") });
                var submitToDB = new CodeExpressionStatement();
                submitToDB.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("dc"), "SubmitChanges");
                objCreateFields.Insert(0, objCreateDeclration);
                objCreateFields.Add(addToDB);
                objCreateFields.Add(submitToDB);
            }
            else if (ORMType == 2)
            {
                addToDB.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("dc"), "Add", new CodeExpression[] { new CodeVariableReferenceExpression("obj") });
                var submitToDB = new CodeExpressionStatement();
                submitToDB.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("dc"), "Commit");
                objCreateFields.Insert(0, objCreateDeclration);
                objCreateFields.Add(addToDB);
                objCreateFields.Add(submitToDB);
            }
            var bprAddSuccess = new CodeExpressionStatement();
            bprAddSuccess.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddSuccess", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprAddSuccess", appPref.ResourceReference, appPref.CommonResourseName)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
            objCreateFields.Add(bprAddSuccess);
            objCreateFields.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format(createModelName))));
            var codeTrueStatement = objCreateFields.ToArray();
            var insertObject = new CodeConditionStatement(new CodeVariableReferenceExpression("noErrorFlag"), codeTrueStatement);
            tryCode.TryStatements.Add(insertObject);
            var addCatchErrorMesssage = new CodeExpressionStatement();
            addCatchErrorMesssage.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddError", new CodeExpression[] { new CodeSnippetExpression(string.Format("ex.Message")), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
            var catch1 = new CodeCatchClause("ex");
            catch1.Statements.Add(addCatchErrorMesssage);
            tryCode.CatchClauses.Add(catch1);
            var elseModeState = new List<CodeStatement>();
            elseModeState.Add(tryCode);
            CodeStatement[] objEditFieldsElse2 = { new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddModelStateErrors", new CodeExpression[] { new CodeSnippetExpression(string.Format("ModelState")) })) };
            addMethod.Statements.Add(new CodeConditionStatement(new CodeVariableReferenceExpression("ModelState.IsValid"), elseModeState.ToArray(), objEditFieldsElse2)); returnStatement = new CodeMethodReturnStatement(new CodePrimitiveExpression(null));
            addMethod.Statements.Add(returnStatement);
            addMethod.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, string.Empty));
            codeType.Members.Add(addMethod);
            return codeType;
        }

        public CodeTypeDeclaration GenerateUpdateDALMethod(string className, Table table, ApplicationPreferences appPref, CodeTypeDeclaration codeType, byte ORMType)
        {
            var modelName = "model";
            var createModelName = "obj";
            var editMethod = new CodeMemberMethod();
            editMethod.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "Update"));
            editMethod.Attributes = (editMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public | MemberAttributes.Static;
            editMethod.Name = "Update";
            editMethod.ReturnType = new CodeTypeReference(string.Format("{0}{1}", appPref.ClassNamePrefix, className));
            editMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("{0}{1}", appPref.ClassNamePrefix, className), "model"));
            editMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("out BatchProcessResult_Model"), "bpr"));
            editMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(bool), "InsertIfNotExist = false"));
            var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("Update({0}.Get{1}, model, out bpr ,InsertIfNotExist)", appPref.DBUtilityName, appPref.DataContextName)));
            editMethod.Statements.Add(returnStatement);
            codeType.Members.Add(editMethod);
            editMethod = new CodeMemberMethod();
            editMethod.Attributes = (editMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public | MemberAttributes.Static;
            editMethod.Name = "Update";
            editMethod.ReturnType = new CodeTypeReference(string.Format("{0}{1}", appPref.ClassNamePrefix, className));
            editMethod.Parameters.Add(new CodeParameterDeclarationExpression(appPref.DataContextName, "dc"));
            editMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("{0}{1}", appPref.ClassNamePrefix, className), "model"));
            editMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("out BatchProcessResult_Model"), "bpr"));
            editMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(bool), "InsertIfNotExist = false"));
            editMethod.Statements.Add(new CodeVariableDeclarationStatement(string.Format("{0}{1}", appPref.ClassNamePrefix, className), createModelName, new CodePrimitiveExpression(null)));
            editMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("bpr"), new CodeSnippetExpression(string.Format("new BatchProcessResult_Model()"))));
            var varDeclare = new CodeVariableDeclarationStatement(typeof(bool), "noErrorFlag", new CodePrimitiveExpression(true));
            editMethod.Statements.Add(varDeclare);
            if (ORMType == 1)
            {

                if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
                {
                    editMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(createModelName), new CodeSnippetExpression(string.Format("dc.{0}{1}.SingleOrDefault(u => u.{3} == {2}.{3})", appPref.ClassNamePrefix, Pluralizer.IsSingular(className) ? Pluralizer.Pluralize(className) : className, modelName, table.Columns.First(x => x.DataType == "uniqueidentifier").Name))));
                }
                else
                {
                    editMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(createModelName), new CodeSnippetExpression(string.Format("dc.{0}{1}.SingleOrDefault(u => u.Id == {2}.Id)", appPref.ClassNamePrefix, Pluralizer.IsSingular(className) ? Pluralizer.Pluralize(className) : className, modelName))));
                }
            }
            else if (ORMType == 2)
            {
                if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
                {
                    editMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(createModelName), new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().SingleOrDefault(u => u.{3} == {2}.{3})", appPref.ClassNamePrefix, className, modelName, table.Columns.First(x => x.DataType == "uniqueidentifier").Name))));
                }
                else
                {
                    editMethod.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(createModelName), new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().SingleOrDefault(u => u.Id == {2}.Id)", appPref.ClassNamePrefix, className, modelName))));
                }
            }

            foreach (var column in table.Columns)
            {
                var columnMetadata = MetaDataGenerator.UpdateColumnMetaData(table, column);
                if (column.CheckRepeptiveError)
                {
                    var addErrorMesssage = new CodeExpressionStatement();
                    addErrorMesssage.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddError", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprRepepetiveError", appPref.ResourceReference, appPref.ErrorResourse)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
                    var codeTrue = new CodeStatement[]{addErrorMesssage
                        , new CodeAssignStatement(new CodeVariableReferenceExpression("noErrorFlag"), new CodePrimitiveExpression(false))};
                    if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
                    {
                        var findErrors = new CodeConditionStatement();
                        if (ORMType == 1)
                        {
                            if (column.DataType.EndsWith("char"))
                            {
                                findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("dc.{0}{1}.Any(q => q.{2} == {3}.{2}.StringNormalizer() && q.{4} != {3}.{4})", appPref.ClassNamePrefix, Pluralizer.IsSingular(className) ? Pluralizer.Pluralize(className) : className, column.Name, modelName, table.Columns.First(x => x.DataType == "uniqueidentifier").Name)), codeTrue);
                            }
                            else
                            {
                                findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("dc.{0}{1}.Any(q => q.{2} == {3}.{2} && q.{4} != {3}.{4})", appPref.ClassNamePrefix, Pluralizer.IsSingular(className) ? Pluralizer.Pluralize(className) : className, column.Name, modelName, table.Columns.First(x => x.DataType == "uniqueidentifier").Name)), codeTrue);
                            }
                        }
                        else if (ORMType == 2)
                        {
                            if (column.DataType.EndsWith("char"))
                            {
                                findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Any(q => q.{2} == {3}.{2}.StringNormalizer() && q.{4} != {3}.{4})", appPref.ClassNamePrefix, className, column.Name, modelName, table.Columns.First(x => x.DataType == "uniqueidentifier").Name)), codeTrue);
                            }
                            else
                            {
                                findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Any(q => q.{2} == {3}.{2} && q.{4} != {3}.{4})", appPref.ClassNamePrefix, className, column.Name, modelName, table.Columns.First(x => x.DataType == "uniqueidentifier").Name)), codeTrue);
                            }
                        }
                        editMethod.Statements.Add(findErrors);
                    }
                    else
                    {
                        var findErrors = new CodeConditionStatement();
                        if (ORMType == 1)
                        {
                            if (column.DataType.EndsWith("char"))
                            {
                                findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("dc.{0}{1}.Any(q => q.{2} == {3}.{2}.StringNormalizer() && q.Id != {3}.Id)", appPref.ClassNamePrefix, Pluralizer.IsSingular(className) ? Pluralizer.Pluralize(className) : className, column.Name, modelName)), codeTrue);
                            }
                            else
                            {
                                findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("dc.{0}{1}.Any(q => q.{2} == {3}.{2} && q.Id != {3}.Id)", appPref.ClassNamePrefix, Pluralizer.IsSingular(className) ? Pluralizer.Pluralize(className) : className, column.Name, modelName)), codeTrue);
                            }
                        }
                        else if (ORMType == 2)
                        {
                            if (column.DataType.EndsWith("char"))
                            {
                                findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Any(q => q.{2} == {3}.{2}.StringNormalizer() && q.Id != {3}.Id)", appPref.ClassNamePrefix, className, column.Name, modelName)), codeTrue);
                            }
                            else
                            {
                                findErrors = new CodeConditionStatement(new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Any(q => q.{2} == {3}.{2} && q.Id != {3}.Id)", appPref.ClassNamePrefix, className, column.Name, modelName)), codeTrue);
                            }
                        }
                        editMethod.Statements.Add(findErrors);
                    }
                }
            }
            var tryCode = new CodeTryCatchFinallyStatement();
            var objEditFields = new List<CodeStatement>();
            foreach (var column in table.Columns.Where(q => q.DataType != "uniqueidentifier"))
            {
                if (!(column.IsForeignKey))
                {
                    if (column.IsPrimaryKey)
                    {
                        if (!column.IdentitySpecification)
                        {
                            var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("{0}.{1}", modelName, column.Name)));
                            objEditFields.Add(newFieldAssignment);
                        }
                    }
                    else if (column.DataType.EndsWith("char"))
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("{0}.{1}.StringNormalizer()", modelName, column.Name)));
                        objEditFields.Add(newFieldAssignment);
                    }
                    else if (column.DataType.EndsWith("int"))
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("{0}.{1}", modelName, column.Name)));
                        objEditFields.Add(newFieldAssignment);
                    }
                    else if ((column.DataType == "datetime" || column.DataType == "date") && column.Name.ToLower() == "createdate" && appPref.SystemDefaults)
                    {
                        //Skip this one
                    }
                    else
                    {
                        var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, column.Name)), new CodeSnippetExpression(string.Format("{0}.{1}", modelName, column.Name)));
                        objEditFields.Add(newFieldAssignment);
                    }
                }
            }
            foreach (var fk in table.ForeignKeys)
            {
                if (!string.IsNullOrEmpty(fk.References))
                {
                    if (ORMType == 1)
                    {
                        if (fk.References != className)
                        {
                            var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, fk.Name)), new CodeSnippetExpression(string.Format("{0}.{1}", modelName, fk.Name)));
                            objEditFields.Add(newFieldAssignment);
                        }
                        else
                        {
                            var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, fk.Name)), new CodeSnippetExpression(string.Format("{0}.{1}", modelName, fk.Name)));
                            objEditFields.Add(newFieldAssignment);
                        }
                    }
                    else if (ORMType == 2)
                    {
                        if (fk.References != className)
                        {
                            var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.{1}", createModelName, fk.UniquePropertyName)), new CodeSnippetExpression(string.Format(@"new {1} {{ Id = {0}.{2}.Id }}", modelName, fk.References, fk.UniquePropertyName)));
                            objEditFields.Add(newFieldAssignment);
                        }
                        else
                        {
                            var newFieldAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("{0}.Parent{1}", createModelName, fk.UniquePropertyName)), new CodeSnippetExpression(string.Format(@"new {1} {{ Id = {0}.Parent{2}.Id }}", modelName, fk.References, fk.UniquePropertyName)));
                            objEditFields.Add(newFieldAssignment);
                        }
                    }
                }
            }
            if (ORMType == 1)
            {
                var submitToDB = new CodeExpressionStatement();
                submitToDB.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("dc"), "SubmitChanges");
                objEditFields.Add(submitToDB);
            }
            else if (ORMType == 2)
            {
                var addToDB = new CodeExpressionStatement();
                addToDB.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("dc"), "Edit", new CodeExpression[] { new CodeVariableReferenceExpression(createModelName) });
                var submitToDB = new CodeExpressionStatement();
                submitToDB.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("dc"), "Commit");
                objEditFields.Add(addToDB);
                objEditFields.Add(submitToDB);
            }
            var bprAddSuccess = new CodeExpressionStatement();
            bprAddSuccess.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddSuccess", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprAddSuccess", appPref.ResourceReference, appPref.CommonResourseName)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
            objEditFields.Add(bprAddSuccess);
            var codeTrueStatement = objEditFields.ToArray();
            var codeTrueStatementElse = new List<CodeStatement>();
            if (ORMType == 1)
            {
                var submitToDB1 = new CodeExpressionStatement();
                submitToDB1.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression(string.Format("dc.{0}{1}", appPref.ClassNamePrefix, Pluralizer.IsSingular(className) ? Pluralizer.Pluralize(className) : className)), "InsertOnSubmit", new CodeExpression[] { new CodeVariableReferenceExpression(createModelName) });
                var submitToDB2 = new CodeExpressionStatement();
                submitToDB2.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("dc"), "SubmitChanges");
                var submitToDB3 = new CodeExpressionStatement();
                submitToDB3.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddSuccess", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprAddSuccess", appPref.ResourceReference, appPref.CommonResourseName)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
                codeTrueStatementElse.Add(submitToDB1);
                codeTrueStatementElse.Add(submitToDB2);
                codeTrueStatementElse.Add(submitToDB3);
            }
            else if (ORMType == 2)
            {
                var submitToDB1 = new CodeExpressionStatement();
                submitToDB1.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("dc"), "Add", new CodeExpression[] { new CodeVariableReferenceExpression(createModelName) });
                var submitToDB2 = new CodeExpressionStatement();
                submitToDB2.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("dc"), "Commit");
                var submitToDB3 = new CodeExpressionStatement();
                submitToDB3.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddSuccess", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprAddSuccess", appPref.ResourceReference, appPref.CommonResourseName)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
                codeTrueStatementElse.Add(submitToDB1);
                codeTrueStatementElse.Add(submitToDB2);
                codeTrueStatementElse.Add(submitToDB3);

            }
            CodeStatement[] objEditFieldsElse = { new CodeConditionStatement(new CodeVariableReferenceExpression(string.Format("InsertIfNotExist")), codeTrueStatementElse.ToArray()) };
            var insertObject = new CodeConditionStatement(new CodeVariableReferenceExpression(string.Format("{0} != null", createModelName)), codeTrueStatement, objEditFieldsElse);
            var insertObject2 = new CodeConditionStatement(new CodeVariableReferenceExpression("noErrorFlag"), insertObject);
            tryCode.TryStatements.Add(insertObject2);
            var addCatchErrorMesssage = new CodeExpressionStatement();
            addCatchErrorMesssage.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddError", new CodeExpression[] { new CodeSnippetExpression(string.Format("ex.Message")), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
            var catch1 = new CodeCatchClause("ex");
            catch1.Statements.Add(addCatchErrorMesssage);
            tryCode.CatchClauses.Add(catch1);

            var elseModeState = new List<CodeStatement>();
            elseModeState.Add(tryCode);
            CodeStatement[] objEditFieldsElse2 = { new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddModelStateErrors", new CodeExpression[] { new CodeSnippetExpression(string.Format("ModelState")) })) };
            editMethod.Statements.Add(new CodeConditionStatement(new CodeVariableReferenceExpression("ModelState.IsValid"), elseModeState.ToArray(), objEditFieldsElse2));

            returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(createModelName));
            editMethod.Statements.Add(returnStatement);
            editMethod.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, string.Empty));
            codeType.Members.Add(editMethod);
            return codeType;
        }

        public CodeTypeDeclaration GenerateDeleteDALMethod(string className, Table table, ApplicationPreferences appPref, CodeTypeDeclaration codeType, byte ORMType)
        {
            var modelName = "model";
            var delMethod = new CodeMemberMethod();
            delMethod.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "Delete"));
            delMethod.Attributes = (delMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public | MemberAttributes.Static;
            delMethod.Name = "Delete";
            delMethod.ReturnType = new CodeTypeReference(typeof(bool));
            delMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("{0}{1}", appPref.ClassNamePrefix, className), "model"));
            var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("Delete({0}.Get{1}, model)", appPref.DBUtilityName, appPref.DataContextName)));
            delMethod.Statements.Add(returnStatement);
            codeType.Members.Add(delMethod);
            delMethod = new CodeMemberMethod();
            delMethod.Attributes = (delMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public | MemberAttributes.Static;
            delMethod.Name = "Delete";
            delMethod.ReturnType = new CodeTypeReference(typeof(bool));
            delMethod.Parameters.Add(new CodeParameterDeclarationExpression(appPref.DataContextName, "dc"));
            delMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("{0}{1}", appPref.ClassNamePrefix, className), "model"));
            var tryCode = new CodeTryCatchFinallyStatement();
            var objectEdit = new CodeStatement();
            var objEditFields = new List<CodeStatement>();
            var addToDB = new CodeExpressionStatement();
            var submitToDB = new CodeExpressionStatement();
            if (ORMType == 1)
            {
                if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
                {
                    objectEdit = new CodeVariableDeclarationStatement("var", "obj", new CodeSnippetExpression(string.Format("dc.{0}{1}.Single(q => q.{2} == {3}.{2})", appPref.ClassNamePrefix, Pluralizer.IsSingular(className) ? Pluralizer.Pluralize(className) : className, table.Columns.First(x => x.DataType == "uniqueidentifier").Name, modelName)));
                }
                else
                {
                    objectEdit = new CodeVariableDeclarationStatement("var", "obj", new CodeSnippetExpression(string.Format("dc.{0}{1}.Single(q => q.{2} == {3}.{2})", appPref.ClassNamePrefix, Pluralizer.IsSingular(className) ? Pluralizer.Pluralize(className) : className, table.PrimaryKey.Columns.First().Name, modelName)));
                }
                addToDB.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression(string.Format("dc.{0}{1}", appPref.ClassNamePrefix, Pluralizer.IsSingular(className) ? Pluralizer.Pluralize(className) : className)), "DeleteOnSubmit", new CodeExpression[] { new CodeVariableReferenceExpression("obj") });
                submitToDB.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("dc"), "SubmitChanges");
                objEditFields.Insert(0, objectEdit);
                objEditFields.Add(addToDB);
                objEditFields.Add(submitToDB);
            }
            else if (ORMType == 2)
            {
                if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
                {
                    objectEdit = new CodeVariableDeclarationStatement("var", "obj", new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Single(q => q.{2} == {3}.{2})", appPref.ClassNamePrefix, className, table.Columns.First(x => x.DataType == "uniqueidentifier").Name, modelName)));
                }
                else
                {
                    objectEdit = new CodeVariableDeclarationStatement("var", "obj", new CodeSnippetExpression(string.Format("dc.Db<{0}{1}>().Single(q => q.{2} == {3}.{2})", appPref.ClassNamePrefix, className, table.PrimaryKey.Columns.First().Name, modelName)));
                }
                addToDB.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("dc"), "Delete", new CodeExpression[] { new CodeVariableReferenceExpression("obj") });
                submitToDB.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("dc"), "Commit");
                objEditFields.Insert(0, objectEdit);
                objEditFields.Add(addToDB);
                objEditFields.Add(submitToDB);
            }
            objEditFields.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(true)));
            var codeTrueStatement = objEditFields.ToArray();
            tryCode.TryStatements.AddRange(codeTrueStatement);
            var addCatchErrorMesssage = new CodeExpressionStatement();
            var catch1 = new CodeCatchClause();
            catch1.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(false)));
            tryCode.CatchClauses.Add(catch1);
            delMethod.Statements.Add(tryCode);
            returnStatement = new CodeMethodReturnStatement(new CodePrimitiveExpression(false));
            delMethod.Statements.Add(returnStatement);
            delMethod.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, string.Empty));
            codeType.Members.Add(delMethod);
            return codeType;
        }

        public CodeMemberMethod GenerateIndexDAL(string className, ApplicationPreferences appPref, int type)
        {
            var method = new CodeMemberMethod();
            if (type == 1)
            {
                method.Attributes = (method.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
                method.Name = "Index";
                method.ReturnType = new CodeTypeReference("ActionResult");
                var modelName = "model";
                method.Parameters.Add(new CodeParameterDeclarationExpression(className + "SearchModel", modelName));
                method.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(String.Format("{0}.Area", modelName)), new CodeSnippetExpression(String.Format("\"\""))));
                var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("View({0})", modelName)));
                method.Statements.Add(returnStatement);
            }
            else if (type == 2)
            {
                method.Attributes = (method.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
                method.Name = className + "SearchModelView";
                method.ReturnType = new CodeTypeReference("ActionResult");
                var modelName = "model";
                method.Parameters.Add(new CodeParameterDeclarationExpression(className + "SearchModel", modelName));
                method.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(String.Format("{0}.Area", modelName)), new CodeSnippetExpression(String.Format("\"\""))));
                var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("PartialView({0})", modelName)));
                method.Statements.Add(returnStatement);
            }
            return method;
        }

        public CodeMemberMethod GenerateSearchFiltersDAL(Table table, string className, ApplicationPreferences appPref)
        {
            var method = new CodeMemberMethod();
            method.Attributes = (method.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            method.Name = "SearchFilters";
            method.ReturnType = new CodeTypeReference(string.Format("PagedList<{0}{1}>", appPref.ClassNamePrefix, className));
            var modelName = "model";
            method.Parameters.Add(new CodeParameterDeclarationExpression(className + "SearchModel", modelName));
            method.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(String.Format("{0}.Area", modelName)), new CodeSnippetExpression(String.Format("\"\""))));
            method.Statements.Add(new CodeVariableDeclarationStatement("var", "qry", new CodeSnippetExpression(string.Format("{0}{1}DAL.Find({2})", appPref.ClassNamePrefix, className, modelName))));
            method.Statements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(modelName), "Update", new CodeSnippetExpression(string.Format("{0}.PageSize, qry.Count()", modelName)))));
            method.Statements.Add(new CodeVariableDeclarationStatement("var", "ls", new CodeSnippetExpression(string.Format("qry.Skip(model.PageSize * {0}.PageIndex).Take({0}.PageSize).ToList()", modelName))));
            method.Statements.Add(new CodeVariableDeclarationStatement("var", "ql", new CodeSnippetExpression(string.Format("new PagedList<{0}{1}>(ls, {2})", appPref.ClassNamePrefix, className, modelName))));
            var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("ql")));
            method.Statements.Add(returnStatement);
            return method;
        }

        public CodeMemberMethod GenerateIXDAL(Table table, string className, ApplicationPreferences appPref)
        {
            var method = new CodeMemberMethod();
            method.Attributes = (method.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            method.Name = "IX";
            method.ReturnType = new CodeTypeReference("ActionResult");
            var modelName = "model";
            method.Parameters.Add(new CodeParameterDeclarationExpression(className + "SearchModel", modelName));
            method.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(String.Format("{0}.Area", modelName)), new CodeSnippetExpression(String.Format("\"\""))));
            var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("PartialView(SearchFilters({0}))", modelName)));
            method.Statements.Add(returnStatement);
            return method;
        }

        public CodeMemberMethod GenerateCreateMethodDAL(string className, Table table, ApplicationPreferences appPref)
        {
            var createMethod = new CodeMemberMethod();
            createMethod.Attributes = (createMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            createMethod.Name = "Create";
            createMethod.ReturnType = new CodeTypeReference("ActionResult");
            var modelName = "model";
            var modelDeclare = new CodeVariableDeclarationStatement("var", modelName, new CodeSnippetExpression(string.Format("new {0}{1}()", appPref.ClassNamePrefix, className)));
            createMethod.Statements.Add(modelDeclare);
            foreach (var fk in table.ForeignKeys)
            {
                if (!string.IsNullOrEmpty(fk.References))
                {
                    if (fk.References != className)
                    {
                        var tableInf = MetaDataGenerator.GetAvailableLookUpCombos(table.DatabaseName, fk.UniquePropertyName);
                        if (string.IsNullOrEmpty(tableInf))
                        {
                            var varDeclareViewbag = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("ViewBag.{0}s", fk.UniquePropertyName)), new CodeVariableReferenceExpression(string.Format("{0}{1}DAL.GetAll()", appPref.ClassNamePrefix, fk.References)));
                            createMethod.Statements.Add(varDeclareViewbag);
                        }
                        else
                        {
                            var varDeclareViewbag = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("ViewBag.{0}s", fk.UniquePropertyName)), new CodeVariableReferenceExpression(string.Format("{0}{1}DAL.GetAll()", appPref.ClassNamePrefix, fk.References)));
                            createMethod.Statements.Add(varDeclareViewbag);
                        }
                    }
                    else
                    {
                        var varDeclareViewbag = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("ViewBag.{0}s", fk.References)), new CodeVariableReferenceExpression(string.Format("{0}{1}DAL.GetAll()", appPref.ClassNamePrefix, fk.References)));
                        createMethod.Statements.Add(varDeclareViewbag);
                    }
                }
            }
            var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("PartialView({0})", modelName)));
            CodeStatement[] trueStatments = { returnStatement };
            createMethod.Statements.Add(new CodeConditionStatement(new CodeVariableReferenceExpression("Request.IsAjaxRequest()"), trueStatments));
            returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("View({0})", modelName)));
            createMethod.Statements.Add(returnStatement);

            return createMethod;
        }

        public CodeMemberMethod GenerateCreateSubmitMethodDAL(string className, Table table, ApplicationPreferences appPref)
        {
            var createMethod = new CodeMemberMethod();
            createMethod.Attributes = (createMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            createMethod.Name = "Create";
            createMethod.ReturnType = new CodeTypeReference("ActionResult");
            createMethod.CustomAttributes.Add(new CodeAttributeDeclaration { Name = "HttpPost" });
            createMethod.CustomAttributes.Add(new CodeAttributeDeclaration { Name = "ValidateAntiForgeryToken" });
            var modelName = "model";
            createMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("{0}{1}", appPref.ClassNamePrefix, className), modelName));
            var varDeclare = new CodeVariableDeclarationStatement();
            varDeclare = new CodeVariableDeclarationStatement("var", "bpr", new CodeSnippetExpression(string.Format("new BatchProcessResult_Model()")));
            createMethod.Statements.Add(varDeclare);
            var tryCode = new CodeTryCatchFinallyStatement();
            var objCreateFields = new List<CodeStatement>();
            objCreateFields.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeSnippetExpression(String.Format("{0}{1}DAL", appPref.ClassNamePrefix, className)), "Add", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}", modelName)), new CodeSnippetExpression(string.Format("out bpr")) })));
            var objCreateElse = new List<CodeStatement>();
            var bprAddSuccess = new CodeExpressionStatement();
            //bprAddSuccess.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddSuccess", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprAddSuccess", appPref.ResourceReference, appPref.CommonResourseName)), new CodePrimitiveExpression(true) });
            var bprSuccessClientScript = new CodeStatement();
            bprSuccessClientScript = new CodeAssignStatement(new CodeSnippetExpression("bpr.SuccessClientScript"), new CodeSnippetExpression(string.Format("\"$('#searchForm').submit();\"")));
            objCreateElse.Add(bprSuccessClientScript);
            //objCreateElse.Add(bprAddSuccess);
            CodeStatement[] trueStatements = { new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("PartialView(\"ProcessResult\", bpr)")) };
            objCreateFields.Add(new CodeConditionStatement(new CodeVariableReferenceExpression("bpr.Failed > 0"), trueStatements, objCreateElse.ToArray()));
            var codeTrueStatement = objCreateFields.ToArray();
            tryCode.TryStatements.AddRange(codeTrueStatement);
            var addCatchErrorMesssage = new CodeExpressionStatement();
            addCatchErrorMesssage.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddError", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprMainUnknownError", appPref.ResourceReference, appPref.ErrorResourse)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
            var catch1 = new CodeCatchClause();
            catch1.Statements.Add(addCatchErrorMesssage);
            tryCode.CatchClauses.Add(catch1);
            createMethod.Statements.Add(tryCode);
            var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("PartialView(\"ProcessResult\", bpr)"));
            createMethod.Statements.Add(returnStatement);
            return createMethod;
        }

        public CodeMemberMethod GenerateEditMethodDAL(string className, Table table, ApplicationPreferences appPref)
        {
            var editMethod = new CodeMemberMethod();
            editMethod.Attributes = (editMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            editMethod.Name = "Edit";
            editMethod.ReturnType = new CodeTypeReference("ActionResult");
            var modelName = "model";
            if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
            {
                editMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Guid), "Id"));
                var modelDeclare = new CodeVariableDeclarationStatement("var", modelName, new CodeSnippetExpression(string.Format("{0}{1}DAL.Get({2})", appPref.ClassNamePrefix, className, table.Columns.First(x => x.DataType == "uniqueidentifier").Name)));
                editMethod.Statements.Add(modelDeclare);
            }
            else
            {
                editMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "Id"));
                var modelDeclare = new CodeVariableDeclarationStatement("var", modelName, new CodeSnippetExpression(string.Format("{0}{1}DAL.Get({2})", appPref.ClassNamePrefix, className, table.PrimaryKey.Columns.First().Name)));
                editMethod.Statements.Add(modelDeclare);
            }
            foreach (var fk in table.ForeignKeys)
            {
                if (!string.IsNullOrEmpty(fk.References))
                {
                    if (fk.References != className)
                    {
                        var tableInf = MetaDataGenerator.GetAvailableLookUpCombos(table.DatabaseName, fk.UniquePropertyName);
                        if (string.IsNullOrEmpty(tableInf))
                        {
                            var varDeclareViewbag = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("ViewBag.{0}s", fk.UniquePropertyName)), new CodeVariableReferenceExpression(string.Format("{0}{1}DAL.GetAll()", appPref.ClassNamePrefix, fk.References)));
                            editMethod.Statements.Add(varDeclareViewbag);
                        }
                        else
                        {
                            var varDeclareViewbag = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("ViewBag.{0}s", fk.UniquePropertyName)), new CodeVariableReferenceExpression(string.Format("{0}{1}DAL.GetAll()", appPref.ClassNamePrefix, fk.References)));
                            editMethod.Statements.Add(varDeclareViewbag);
                        }
                    }
                    else
                    {
                        var varDeclareViewbag = new CodeAssignStatement(new CodeVariableReferenceExpression(string.Format("ViewBag.{0}s", fk.References)), new CodeVariableReferenceExpression(string.Format("{0}{1}DAL.GetAll()", appPref.ClassNamePrefix, fk.References)));
                        editMethod.Statements.Add(varDeclareViewbag);
                    }
                }
            }
            var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("PartialView({0})", modelName)));
            CodeStatement[] trueStatments = { returnStatement };
            editMethod.Statements.Add(new CodeConditionStatement(new CodeVariableReferenceExpression("Request.IsAjaxRequest()"), trueStatments));
            returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("View({0})", modelName)));
            editMethod.Statements.Add(returnStatement);

            return editMethod;
        }

        public CodeMemberMethod GenerateEditSubmitMethodDAL(string className, Table table, ApplicationPreferences appPref)
        {
            var createMethod = new CodeMemberMethod();
            createMethod.Attributes = (createMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            createMethod.Name = "Edit";
            createMethod.ReturnType = new CodeTypeReference("ActionResult");
            createMethod.CustomAttributes.Add(new CodeAttributeDeclaration { Name = "HttpPost" });
            createMethod.CustomAttributes.Add(new CodeAttributeDeclaration { Name = "ValidateAntiForgeryToken" });
            var modelName = "model";
            createMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("{0}{1}", appPref.ClassNamePrefix, className), modelName));
            var varDeclare = new CodeVariableDeclarationStatement();
            varDeclare = new CodeVariableDeclarationStatement("var", "bpr", new CodeSnippetExpression(string.Format("new BatchProcessResult_Model()")));
            createMethod.Statements.Add(varDeclare);
            var tryCode = new CodeTryCatchFinallyStatement();
            var objCreateFields = new List<CodeStatement>();
            objCreateFields.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeSnippetExpression(String.Format("{0}{1}DAL", appPref.ClassNamePrefix, className)), "Update", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}", modelName)), new CodeSnippetExpression(string.Format("out bpr")) })));
            var objCreateElse = new List<CodeStatement>();
            var bprAddSuccess = new CodeExpressionStatement();
            //bprAddSuccess.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddSuccess", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprAddSuccess", appPref.ResourceReference, appPref.CommonResourseName)), new CodePrimitiveExpression(true) });
            var bprSuccessClientScript = new CodeStatement();
            bprSuccessClientScript = new CodeAssignStatement(new CodeSnippetExpression("bpr.SuccessClientScript"), new CodeSnippetExpression(string.Format("\"$('#searchForm').submit();\"")));
            objCreateElse.Add(bprSuccessClientScript);
            //objCreateElse.Add(bprAddSuccess);
            CodeStatement[] trueStatements = { new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("PartialView(\"ProcessResult\", bpr)")) };
            objCreateFields.Add(new CodeConditionStatement(new CodeVariableReferenceExpression("bpr.Failed > 0"), trueStatements, objCreateElse.ToArray()));
            var codeTrueStatement = objCreateFields.ToArray();
            tryCode.TryStatements.AddRange(codeTrueStatement);
            var addCatchErrorMesssage = new CodeExpressionStatement();
            addCatchErrorMesssage.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddError", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprMainUnknownError", appPref.ResourceReference, appPref.ErrorResourse)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
            var catch1 = new CodeCatchClause();
            catch1.Statements.Add(addCatchErrorMesssage);
            tryCode.CatchClauses.Add(catch1);
            createMethod.Statements.Add(tryCode);
            var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("PartialView(\"ProcessResult\", bpr)"));
            createMethod.Statements.Add(returnStatement);
            return createMethod;
        }

        public CodeMemberMethod GenerateDeleteMethodDAL(string className, Table table, ApplicationPreferences appPref)
        {
            var deleteMethod = new CodeMemberMethod();
            deleteMethod.Attributes = (deleteMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            deleteMethod.Name = "Delete";
            deleteMethod.ReturnType = new CodeTypeReference("ActionResult");
            var modelName = "model";
            if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
            {
                deleteMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Guid), "Id"));
                var modelDeclare = new CodeVariableDeclarationStatement("var", modelName, new CodeSnippetExpression(string.Format("{0}{1}DAL.Get({2})", appPref.ClassNamePrefix, className, table.Columns.First(x => x.DataType == "uniqueidentifier").Name)));
                deleteMethod.Statements.Add(modelDeclare);
            }
            else
            {
                deleteMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "Id"));
                var modelDeclare = new CodeVariableDeclarationStatement("var", modelName, new CodeSnippetExpression(string.Format("{0}{1}DAL.Get({2})", appPref.ClassNamePrefix, className, table.PrimaryKey.Columns.First().Name)));
                deleteMethod.Statements.Add(modelDeclare);
            }
            var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("PartialView({0})", modelName)));
            CodeStatement[] trueStatments = { returnStatement };
            deleteMethod.Statements.Add(new CodeConditionStatement(new CodeVariableReferenceExpression("Request.IsAjaxRequest()"), trueStatments));
            returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(string.Format("View({0})", modelName)));
            deleteMethod.Statements.Add(returnStatement);
            return deleteMethod;
        }

        public CodeMemberMethod GenerateDeleteSubmitMethodDAL(string className, Table table, ApplicationPreferences appPref)
        {
            var deleteMethod = new CodeMemberMethod();
            deleteMethod.Attributes = (deleteMethod.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
            deleteMethod.Name = "Delete";
            deleteMethod.ReturnType = new CodeTypeReference("ActionResult");
            deleteMethod.CustomAttributes.Add(new CodeAttributeDeclaration { Name = "HttpPost" });
            deleteMethod.CustomAttributes.Add(new CodeAttributeDeclaration { Name = "ValidateAntiForgeryToken" });
            var modelName = "model";
            deleteMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("{0}{1}", appPref.ClassNamePrefix, className), modelName));
            var varDeclare = new CodeVariableDeclarationStatement();
            varDeclare = new CodeVariableDeclarationStatement("var", "bpr", new CodeSnippetExpression(string.Format("new BatchProcessResult_Model()")));
            deleteMethod.Statements.Add(varDeclare);
            var tryCode = new CodeTryCatchFinallyStatement();
            var objectEdit = new CodeStatement();
            var objEditFields = new List<CodeStatement>();
            if (!appPref.WithKendo)
            {
                var bprAddSuccess = new CodeExpressionStatement();
                bprAddSuccess.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddSuccess", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprAddSuccess", appPref.ResourceReference, appPref.CommonResourseName)), new CodePrimitiveExpression(true) });
                var bprSuccessClientScript = new CodeStatement();
                bprSuccessClientScript = new CodeAssignStatement(new CodeSnippetExpression("bpr.SuccessClientScript"), new CodeSnippetExpression(string.Format("\"$('#searchForm').submit();\"")));
                objEditFields.Add(bprSuccessClientScript);
                objEditFields.Add(bprAddSuccess);
            }
            var codeTrueStatement = objEditFields.ToArray();
            CodeStatement[] falseStatements = { new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddError", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprMainUnknownError", appPref.ResourceReference, appPref.ErrorResourse)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) })) };
            var insertObject = new CodeConditionStatement(new CodeVariableReferenceExpression(String.Format("{0}{1}DAL.Delete({2})", appPref.ClassNamePrefix, className, modelName)), codeTrueStatement, falseStatements);
            tryCode.TryStatements.Add(insertObject);
            var addCatchErrorMesssage = new CodeExpressionStatement();
            addCatchErrorMesssage.Expression = new CodeMethodInvokeExpression(new CodeSnippetExpression("bpr"), "AddError", new CodeExpression[] { new CodeSnippetExpression(string.Format("{0}.{1}.BprMainUnknownError", appPref.ResourceReference, appPref.ErrorResourse)), new CodePrimitiveExpression(true), new CodePrimitiveExpression(true) });
            var catch1 = new CodeCatchClause();
            catch1.Statements.Add(addCatchErrorMesssage);
            tryCode.CatchClauses.Add(catch1);
            deleteMethod.Statements.Add(tryCode);
            var returnStatement = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("PartialView(\"ProcessResult\", bpr)"));
            deleteMethod.Statements.Add(returnStatement);
            return deleteMethod;
        }

    }
}