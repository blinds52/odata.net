﻿//---------------------------------------------------------------------
// <copyright file="EdmModelBaseTests.cs" company="Microsoft">
//      Copyright (C) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.
// </copyright>
//---------------------------------------------------------------------

namespace Microsoft.Test.Edm.TDD.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections.ObjectModel;
    using FluentAssertions;
    using Microsoft.OData.Edm;
    using Microsoft.OData.Edm.Library;
    using Microsoft.OData.Edm.Library.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EdmModelBaseTests
    {
        #region FindDeclaredOperations Tests
        [TestMethod]
        public void FindDeclaredOperationsShouldFindFullyQualifiedName()
        {
            var model = new TestEdmModel();
            model.Add(new EdmAction("n", "a", null));
            model.Add(new EdmAction("n", "a2", null));

            var findResults = model.FindDeclaredOperations("n.a").ToList();
            findResults.Should().HaveCount(1);
            findResults[0].Name.Should().Be("a");
        }

        [TestMethod]
        public void FindDeclaredOperationsShouldFindAllActionsWithSameNameAndDifferentNamespaces()
        {
            var model = new TestEdmModel();
            model.Add(new EdmAction("n", "a", null));
            model.Add(new EdmAction("n2", "a", null));

            var findResults = model.FindDeclaredOperations("a").ToList();
            findResults.Should().HaveCount(0);
        }

        [TestMethod]
        public void FindDeclaredOperationsShouldFindAllActionsShouldNotFindUnknownActionWithName()
        {
            var model = new TestEdmModel();
            model.Add(new EdmAction("n", "a", null));
            model.Add(new EdmAction("n2", "a", null));

            var findResults = model.FindDeclaredOperations("a2").Should().HaveCount(0);
        }

        [TestMethod]
        public void FindDeclaredOperationsShouldFindAllActionsShouldNotFindUnknownActionWithFullyQualifiedName()
        {
            var model = new TestEdmModel();
            model.Add(new EdmAction("n", "a", null));
            model.Add(new EdmAction("n2", "a", null));

            var findResults = model.FindDeclaredOperations("n.a2").Should().HaveCount(0);
        }
        #endregion

        internal class TestEdmModel : EdmModelBase
        {
            private List<IEdmSchemaElement> schemaElements;

            public TestEdmModel()
                : base(new List<IEdmModel>(), new EdmDirectValueAnnotationsManager())
            {
                this.schemaElements = new List<IEdmSchemaElement>();
            }

            public void Add(IEdmSchemaElement element)
            {
                this.schemaElements.Add(element);
                this.RegisterElement(element);
            }

            public override IEnumerable<IEdmSchemaElement> SchemaElements
            {
                get { return schemaElements; }
            }

            public override IEnumerable<string> DeclaredNamespaces
            {
                get { return this.SchemaElements.Select(s => s.Namespace).Distinct(); }
            }

            public override IEnumerable<IEdmStructuredType> FindDirectlyDerivedTypes(IEdmStructuredType baseType)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
