using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace TestExtensions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AllFieldsTestAttribute : Attribute, IApplyToContext
    {
        public enum DuplicateFieldValueBehaviour
        {
            EnforceUniqueness,
            PermitDuplicateValues
        }

        private readonly Dictionary<string, bool> _fieldsToTest;
        private readonly DuplicateFieldValueBehaviour _permitNonUniqueValues;
        private readonly object[] _testFields;
        private readonly bool _uniqueFieldValues;

        public AllFieldsTestAttribute(Type unitTestClass, Type fieldType, DuplicateFieldValueBehaviour permitNonUniqueValues = DuplicateFieldValueBehaviour.EnforceUniqueness,
            [CallerMemberName] string unitTestMethod = null)
        {
            _permitNonUniqueValues = permitNonUniqueValues;

            // create a new object of the requested type and get the field values (all + unique)
            var fieldTypeInstance = Activator.CreateInstance(fieldType);
            var allFieldValues = fieldTypeInstance.GetType().GetFields().ToList();
            var uniqueFieldValues = allFieldValues.GroupBy(y => fieldType.GetField(y.Name).GetValue(fieldTypeInstance).ToString()).ToList();

            // create a lookup of value to tested flag (false)
            _fieldsToTest = uniqueFieldValues.ToDictionary(x => x.Key, x => false);

            // were all field values unique
            _uniqueFieldValues = allFieldValues.Count == uniqueFieldValues.Count;

            // get all of the test case attributes on the unit test method
            _testFields = unitTestClass.GetMethod(unitTestMethod).GetCustomAttributes(typeof(TestCaseAttribute), false);
        }

        public void ApplyToContext(TestExecutionContext context)
        {
            if (_permitNonUniqueValues == DuplicateFieldValueBehaviour.EnforceUniqueness && !_uniqueFieldValues)
            {
                Assert.Fail("Field values are not unique");
            }

            // identify values that have test cases
            foreach (var testField in _testFields)
            {
                var testCaseAttribute = testField as TestCaseAttribute;
                if (_fieldsToTest.ContainsKey(testCaseAttribute.Arguments[0].ToString()))
                {
                    _fieldsToTest[testCaseAttribute.Arguments[0].ToString()] = true;
                }
                else
                {
                    Assert.Fail($"Unrecognised test case field value {testCaseAttribute.Arguments[0].ToString()}");
                }
            }

            if (_fieldsToTest.Any(x => x.Value == false))
            {
                Assert.Fail("Insufficient test cases");
            }
        }
    }
}