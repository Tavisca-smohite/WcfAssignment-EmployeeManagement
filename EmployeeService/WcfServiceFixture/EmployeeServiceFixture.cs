using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WcfServiceFixture.ServiceReference;

namespace WcfServiceFixture
{
    [TestClass]
    public class EmployeeServiceFixture
    {

        CreateEmployeeServiceClient _getEmployeeClient  = new CreateEmployeeServiceClient("BasicHttpBinding_ICreateEmployeeService");
        RetrieveEmployeeServiceClient _retrieveClient = new RetrieveEmployeeServiceClient("BasicHttpBinding_IRetrieveEmployeeService");

        //to close client instance after every test case
        [TestCleanup]
        public void CloseClientInstance()
        {
            if (_getEmployeeClient != null && _getEmployeeClient.State == System.ServiceModel.CommunicationState.Opened) _getEmployeeClient.Close();
            if (_retrieveClient != null && _retrieveClient.State == System.ServiceModel.CommunicationState.Opened) _retrieveClient.Close();
            
        }
        /// <summary>
        /// tests if it throws fault exception if employee id is being repeated
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException<CustomTypeException>))]
        public void IfIdExistsTest()
        {

            var e = _getEmployeeClient.CreateEmployee(1,"sheetal");
            if (e is Employee)
            {
                e = _getEmployeeClient.CreateEmployee(1,"sheetal");
            }
           
        }

        /// <summary>
        /// tests if it adds unique employees 
        /// </summary>
        [TestMethod]
        public void CreateUniqueEmployee()
        {
            bool isUnique = false;
            try
            {
                var e = _getEmployeeClient.CreateEmployee(1,"sheetal");
                isUnique = e is Employee ? true : false;
            }
            catch (FaultException<CustomTypeException>)
            {
                //it will throw exception if above employee already exists in datalist
                //ignoring the exception we will try to add other employee data with unique id
                //this test will validate that server will only accept data with unique ids

                var testEmployee = _getEmployeeClient.CreateEmployee(17, "tanya");
                isUnique = testEmployee is Employee ? true : false;
                _getEmployeeClient.EmployeeClear();
            }
            Assert.AreEqual(isUnique, true);
        }

        /// <summary>
        /// throws fault exception if some one tries to access empty list
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException<CustomTypeException>))]
        public void RetrivalOfEmptyList()
        {
            _getEmployeeClient.EmployeeClear();
            var testList = _retrieveClient.GetEmployees();
        }

        [TestMethod]
        public void RetrivalOfEmployeeData()
        {
            var testEmployee = _getEmployeeClient.CreateEmployee(12, "tanya");
            var list = _retrieveClient.GetEmployees();

            Assert.AreEqual(list.GetType(), typeof(Employee[]));

        }

        /// <summary>
        /// we cant add remarks for those employee fields which doesnt exist 
        /// in data base or data list
        /// it will give a message to client abt non existance of employee
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException<CustomTypeException>))]
        public void CantAddRemarkToNonExistingEmployee()
        {

            var addedRemark = _getEmployeeClient.AddRemarks(15, "good");
        }

        [TestMethod]
        public void AddRemarkForExistingEmployeeOnly()
        {
            _getEmployeeClient.EmployeeClear();
            var testEmployee = _getEmployeeClient.CreateEmployee(17, "tanya");
            var addedRemark =_getEmployeeClient.AddRemarks(17, "good");
            Assert.AreEqual(addedRemark, "Remark added successfully");
        }

        [TestMethod]
        public void RetrieveById()
        {
            _getEmployeeClient.EmployeeClear();
            var testEmployee =_getEmployeeClient.CreateEmployee(17, "tanya");
            testEmployee = _retrieveClient.SearchById(17);
            Assert.AreEqual(testEmployee.GetType(), typeof(Employee));
        }

        [TestMethod]
        public void RetrieveByName()
        {
            _getEmployeeClient.EmployeeClear();
            var testEmployee = _getEmployeeClient.CreateEmployee(17, "tanya");
            testEmployee = _retrieveClient.SearchByName("tanya");
            Assert.AreEqual(testEmployee.GetType(), typeof(Employee));
        }

        /// <summary>
        /// we will add three employees
        /// and we will test if it returns employee list with remark "good"
        /// in these case it should return two employees
        /// </summary>
        [TestMethod]
        public void RetrivalByParticularRemark()
        {
            _getEmployeeClient .EmployeeClear();
            var testEmployee = _getEmployeeClient .CreateEmployee(17, "tanya");
            var addedRemark = _getEmployeeClient .AddRemarks(17, "good");
            testEmployee = _getEmployeeClient .CreateEmployee(15, "sheetal");
            addedRemark = _getEmployeeClient .AddRemarks(15, "good");
            addedRemark = _getEmployeeClient .AddRemarks(15, "bad");
            testEmployee = _getEmployeeClient .CreateEmployee(1, "tanvi");
            addedRemark = _getEmployeeClient  .AddRemarks(1, "worst");
            var list = _retrieveClient.GetEmployeesByRemark("good");

            Assert.AreEqual(list.Length, 2);
        }

        /// <summary>
        /// we will add four employees and one of them has no remarks added 
        /// and we will get only those employees who has remarks
        /// in these case we will get a list of 3 employees
        /// </summary>

        [TestMethod]
        public void RetrivalOfEmployeesWithRemarks()
        {
            _getEmployeeClient .EmployeeClear();
            var testEmployee = _getEmployeeClient .CreateEmployee(17, "tanya");
            var addedRemark = _getEmployeeClient .AddRemarks(17, "good");
            testEmployee = _getEmployeeClient .CreateEmployee(15, "sheetal");
            addedRemark = _getEmployeeClient .AddRemarks(15, "good");
            addedRemark = _getEmployeeClient .AddRemarks(15, "bad");
            testEmployee = _getEmployeeClient .CreateEmployee(1, "tanvi");
            addedRemark = _getEmployeeClient .AddRemarks(1, "worst");
            testEmployee = _getEmployeeClient .CreateEmployee(5, "swarna");
            var list = _retrieveClient.GetEmployeesHavingRemarks();

            Assert.AreEqual(list.Length, 3);
        }

        //id cant be non positive and it must be a whole number
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void IdShouldBePositiveInteger()
        {
            _getEmployeeClient.EmployeeClear();
            var e = _getEmployeeClient.CreateEmployee(1,"sheetal");
            if (e is Employee)
            {
                
                e = _getEmployeeClient.CreateEmployee(-1, "sheetal");
            }
            
        }

        /// <summary>
        /// names entered should not contain special characters like
        /// @ # $%& or blank name with only spaces and so on
        /// accepted formats are: John, John Doe, O'Dell i.e,
        /// names with lower or upper case letters a space or a apostrophe(')
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void NameShouldNotContainInvalidCharacters()
        {
            _getEmployeeClient.EmployeeClear();
            var e = _getEmployeeClient.CreateEmployee(1, "sheetal");
            if (e is Employee)
            {

                e = _getEmployeeClient.CreateEmployee(1, "@sheetal");
            }

        }
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void NameShouldNotContainonlySpaces()
        {
            _getEmployeeClient.EmployeeClear();
            var e = _getEmployeeClient.CreateEmployee(1, "sheetal");
            if (e is Employee)           
                e = _getEmployeeClient.CreateEmployee(5, "     ");
            

        }

        [TestMethod]
       
        public void NameWithValidCharactersShouldGetAddedInDataList()
        {
            _getEmployeeClient.EmployeeClear();
            var e = _getEmployeeClient.CreateEmployee(1, "sheetal");
            if (e is Employee)           
                e = _getEmployeeClient.CreateEmployee(5, "Tanya Sharma");

            e = _getEmployeeClient.CreateEmployee(10, "John O'Carner");

        }

    }
}
