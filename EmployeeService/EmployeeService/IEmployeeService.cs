using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace EmployeeService
{
    [ServiceContract]
    public interface ICreateEmployeeService
    {
        [OperationContract]
        [FaultContract(typeof(CustomTypeException))]
        Employee CreateEmployee(int id,string name);

        [OperationContract]
        [FaultContract(typeof(CustomTypeException))]
        string AddRemarks(int id, string remarks);

        [OperationContract]
        void EmployeeClear();
    }



    [ServiceContract]
    public interface IRetrieveEmployeeService
    {
        [OperationContract]
        [FaultContract(typeof(CustomTypeException))]
        List<Employee> GetEmployees();

        [OperationContract(Name = "SearchById")]
        Employee GetEmployee(int Id);

        [OperationContract(Name = "SearchByName")]
        Employee GetEmployee(string Name);

        [OperationContract]       
        List<Employee> GetEmployeesByRemark(string searchRemark);

        [OperationContract]
        List<Employee> GetEmployeesHavingRemarks();
    }
   
    [DataContract]
    public class Employee
    {

        public Employee(int id,string name)
        {
            this.Id = id;
            this.Name = name;
        }
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTime RemarkDate { get; set; }


        [DataMember]
        public List<string> RemarkText = new List<string>();

         
        

     
        
        
    }
}
