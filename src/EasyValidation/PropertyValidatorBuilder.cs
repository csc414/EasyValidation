
using EasyValidation.Rules;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace EasyValidation
{
    public class PropertyValidatorBuilder<T, TProperty>
    {
        public PropertyValidatorBuilder(PropertyValidator validator)
        {
            Validator = validator;
        }

        public PropertyValidator Validator { get; }
    }
}
