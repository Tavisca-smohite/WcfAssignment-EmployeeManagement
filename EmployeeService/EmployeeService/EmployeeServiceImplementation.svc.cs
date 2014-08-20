using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace EmployeeService
{
    /// <summary>
    /// 
    /// </summary>
    public class EmployeeServiceImplementation : ICreateEmployeeService, IRetrieveEmployeeService
    {
        public static List<Employee> _employeeList = new List<Employee>();

        public Employee CreateEmployee(int id, string name)
        {
            var result = _employeeList.Where(t => t.Id == id).FirstOrDefault();
            if (result != null)
            {
                CustomTypeException exceptionDetails = new CustomTypeException();
                exceptionDetails.Reason = "can not make entry";
                exceptionDetails.Description = "Record Already Present with same ID";
                throw new FaultException<CustomTypeException>(exceptionDetails);
            }

            /*foreach (var item in _employeeList)
            {
                if (item.Id == employee.Id)
                {
                    CustomTypeException exceptionDetails = new CustomTypeException();
                    exceptionDetails.Reason = "can not make entry";
                    exceptionDetails.Description = "Record Already Present with same ID";
                    throw new FaultException<CustomTypeException>(exceptionDetails);
                }
            }*/
            var employee = new Employee(id, name);
            employee.RemarkDate = DateTime.Now;
            employee.RemarkText = new List<string>();
            employee.RemarkText.Add("No Remarks Added Yet");
            _employeeList.Add(employee);
            return employee;




        }

        public string AddRemarks(int id, string remark)
        {
            var result = _employeeList.Where(t => t.Id == id).FirstOrDefault();
            if (result != null)
            {
                result.RemarkDate = System.DateTime.Now;
                if (result.RemarkText[0] == "No Remarks Added Yet")
                    result.RemarkText[0] = remark;
                else
                    result.RemarkText.Add(remark);
                return "Remark added successfully";
            }
            //foreach (var item in _employeeList)
            //{
            //    if (item.Id == id)
            //    {
            //        item.RemarkDate = System.DateTime.Now;
            //        if (item.RemarkText[0] == "No Remarks Added Yet")
            //            item.RemarkText[0] = remark;
            //        else
            //            item.RemarkText.Add(remark);
            //        return "Remark added successfully";
            //    }
            //}
            CustomTypeException exceptionDetails = new CustomTypeException();
            exceptionDetails.Reason = "no record found";
            exceptionDetails.Description = "can not add remarks for non existing employees";
            throw new FaultException<CustomTypeException>(exceptionDetails);
        }

        public List<Employee> GetEmployees()
        {
            if (_employeeList.Count == 0)
            {
                CustomTypeException exceptionDetails = new CustomTypeException
                {
                    Reason = "no records found",
                    Description = "no data present can not return empty list"
                };

                throw new FaultException<CustomTypeException>(exceptionDetails, "This is to test");
            }
            else
                return _employeeList;
        }

        public Employee GetEmployee(int Id)
        {
            var result = _employeeList.Where(t => t.Id == Id).FirstOrDefault();           
            return result;

        }

        public Employee GetEmployee(string Name)
        {
            var result = _employeeList.Where(t => t.Name == Name).FirstOrDefault();
            //foreach (var item in _employeeList)
            //{
            //    if (item.Name == Name)
            //    {
            //        return item;
            //    }
            //}
            return result;

        }


        public void EmployeeClear()
        {
            _employeeList.Clear();
        }


        public List<Employee> GetEmployeesByRemark(string searchRemark)
        {
            List<Employee> resultList = new List<Employee>();
          
            _employeeList.ForEach(e =>
            {
                
                foreach (var item in e.RemarkText)
                {
                    if (item == searchRemark)
                    {
                        resultList.Add(e);
                        break;
                    }
                }
            });

            return resultList;
        }

        public List<Employee> GetEmployeesHavingRemarks()
        {
            List<Employee> resultList = new List<Employee>();
            _employeeList.ForEach(e =>
            {
                foreach (var item in e.RemarkText)
                {
                    if (item != "No Remarks Added Yet")
                    {
                        resultList.Add(e);
                        break;
                    }
                }
            });

            return resultList;
        }


    }
}
