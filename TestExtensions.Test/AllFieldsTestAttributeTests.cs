using NUnit.Framework;

namespace TestExtensions.Test
{
    [TestFixture]
    public class AllFieldsTestAttributeTests
    {
        private class UniqueFieldNames
        {
            public const string Theme = "theme";
            public const string Language = "language";
            public const string Port = "port";
            public const string PollingInterval = "polling interval";
        }

        private class DuplicateFieldNames
        {
            public const string Theme = "theme";
            public const string Language = "theme";
            public const string Port = "port";
            public const string PollingInterval = "polling interval";
        }

        //[AllFieldsTest(typeof(AllFieldsTestAttributeTests), typeof(UniqueFieldNames))]
        [TestCase(UniqueFieldNames.Theme)]
        [TestCase(UniqueFieldNames.Language)]
        [TestCase(UniqueFieldNames.Port)]
        [TestCase(UniqueFieldNames.PollingInterval)]
        public void UniqueFieldNamesOnly(string fieldName)
        {
            var allFieldsTestAttribute = new AllFieldsTestAttribute(GetType(), typeof(UniqueFieldNames));
            Assert.DoesNotThrow(() => allFieldsTestAttribute.ApplyToContext(null));
        }

        //[AllFieldsTest(typeof(AllFieldsTestAttributeTests), typeof(UniqueFieldNames))]
        [TestCase(UniqueFieldNames.Theme)]
        [TestCase(UniqueFieldNames.Port)]
        public void UniqueFieldNamesWithSomeMissing(string fieldName)
        {
            var allFieldsTestAttribute = new AllFieldsTestAttribute(GetType(), typeof(UniqueFieldNames));
            Assert.Throws<AssertionException>(() => allFieldsTestAttribute.ApplyToContext(null));
        }

        //[AllFieldsTest(typeof(AllFieldsTestAttributeTests), typeof(DuplicateFieldNames))]
        [TestCase(DuplicateFieldNames.Theme)]
        [TestCase(DuplicateFieldNames.Language)]
        [TestCase(DuplicateFieldNames.Port)]
        [TestCase(DuplicateFieldNames.PollingInterval)]
        public void DuplicateFieldNamesAreNotPermitted(string fieldName)
        {
            var allFieldsTestAttribute = new AllFieldsTestAttribute(GetType(), typeof(DuplicateFieldNames));
            Assert.Throws<AssertionException>(() => allFieldsTestAttribute.ApplyToContext(null));
        }

        //[AllFieldsTest(typeof(AllFieldsTestAttributeTests), typeof(DuplicateFieldNames), AllFieldsTestAttribute.DuplicateFieldValueBehaviour.PermitDuplicateValues)]
        [TestCase(DuplicateFieldNames.Theme)]
        [TestCase(DuplicateFieldNames.Language)]
        [TestCase(DuplicateFieldNames.Port)]
        [TestCase(DuplicateFieldNames.PollingInterval)]
        public void DuplicateFieldNamesArePermitted(string fieldName)
        {
            var allFieldsTestAttribute = new AllFieldsTestAttribute(GetType(), typeof(UniqueFieldNames), AllFieldsTestAttribute.DuplicateFieldValueBehaviour.PermitDuplicateValues);
            Assert.Throws<AssertionException>(() => allFieldsTestAttribute.ApplyToContext(null));
        }

        //[AllFieldsTest(typeof(AllFieldsTestAttributeTests), typeof(DuplicateFieldNames), AllFieldsTestAttribute.DuplicateFieldValueBehaviour.PermitDuplicateValues)]
        [TestCase(DuplicateFieldNames.Theme)]
        [TestCase(DuplicateFieldNames.Language)]
        [TestCase(DuplicateFieldNames.PollingInterval)]
        public void DuplicateFieldNamesWithSomeMissing(string fieldName)
        {
            var allFieldsTestAttribute = new AllFieldsTestAttribute(GetType(), typeof(DuplicateFieldNames), AllFieldsTestAttribute.DuplicateFieldValueBehaviour.PermitDuplicateValues);
            Assert.Throws<AssertionException>(() => allFieldsTestAttribute.ApplyToContext(null));
        }

        //[AllFieldsTest(typeof(AllFieldsTestAttributeTests), typeof(UniqueFieldNames))]
        [TestCase(UniqueFieldNames.Theme, 0.5)]
        [TestCase(UniqueFieldNames.Language, 10.3)]
        [TestCase(UniqueFieldNames.Port, 11.1)]
        [TestCase(UniqueFieldNames.PollingInterval, 33.1)]
        public void UniqueFieldNamesArePermittedEvenWithOtherValues(string fieldName, double value)
        {
            var allFieldsTestAttribute = new AllFieldsTestAttribute(GetType(), typeof(UniqueFieldNames));
            Assert.DoesNotThrow(() => allFieldsTestAttribute.ApplyToContext(null));
        }
    }
}