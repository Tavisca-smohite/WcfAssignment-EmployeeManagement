using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmployeeTestLibrary.ServiceReference;

namespace EmployeeTestLibrary
{
    public class EmployeeTest
    {

        
        CreateEmployeeServiceClient GetEmployeeclient = new CreateEmployeeServiceClient("BasicHttpBinding_ICreateEmployeeService");
            public bool IfIdExists(int id)
            {
                
                foreach ()
                {
                    if (item.Id == id)
                        return true;
                }
                return false;
            }

            public bool IfEmployeeListEmpty()
            {

                if (EmployeeServiceImplementation.EmployeeList.Count == 0)
                    return true;

                return false;
            }

            public bool ifRemarkListEmpty(Employee e)
            {
                if (e.RemarkText.Count == 0)
                    return true;
                return false;
            }
        
    }
}
