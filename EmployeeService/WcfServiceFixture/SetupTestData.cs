using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcfServiceFixture.ServiceReference;

namespace WcfServiceFixture
{
    public class SetupTestData
    {
        
        public static Employee TestEmployeeData()
        {
            Employee testEmployee=new Employee();
            testEmployee.Id=1;
            testEmployee.Name="sheetal";
            testEmployee.RemarkDate = DateTime.Now;
            return testEmployee;

        }

        public static Employee CreateTestEmployee(int id,string name)
        {
            Employee testEmployee = new Employee();
            testEmployee.Id = id;
            testEmployee.Name = name;
            testEmployee.RemarkDate = DateTime.Now;
            return testEmployee;

        }

    }
}
