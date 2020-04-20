using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Xunit;

namespace EasyValidation.Tests
{
    public class UnitTest1
    {
        public enum VerifyType 
        {
            Login,
            FindPassword
        }


        [Fact]
        public void Test2()
        {
            Validator.Model<Student>(builder => {
                builder.Property(o => o.Name).DisplayName("名称").DataAnnotation().Length(3, 5);
                //builder.Property(o => o.NickName).When(o => o.Age >= 18, o => o.NotEmpty());
                builder.Property(o => o.Teachers).Validate(message: "请输入正确的 {0}");
            });

            Validator.Model<Student>(VerifyType.FindPassword, builder => {
                builder.Property(o => o.Age).DisplayName("年龄").Range(5,10);
            });

            Validator.Model<Teacher>(builder => {
                builder.Property(o => o.Name).NotEmpty();
            });

            Validator.Model<GoodStudent>().Inherited(VerifyType.FindPassword);

            var student = new Student { Name="he", Age = 18, Teachers = new List<Teacher>() { new Teacher { Name = "" } } };

            var goodStudent = new GoodStudent();

            ValidationResult result = Validator.Validate(goodStudent);
            if (result.IsValid)
            {
                
            }
            else
            {
                var error = result.ToString(",");
            }
        }

        class Teacher
        {
            public string Name { get; set; }
        }

        class GoodStudent : Student
        {

        }

        class Student
        {
            [Required(ErrorMessage = "{0}不能为空")]
            public string Name { get; set; }

            public string NickName { get; set; }

            public int Age { get; set; }

            public IList<string> Names { get; set; } = new List<string>();

            public Teacher Teacher { get; set; }

            public IList<Teacher> Teachers { get; set; }
        }
    }
}
