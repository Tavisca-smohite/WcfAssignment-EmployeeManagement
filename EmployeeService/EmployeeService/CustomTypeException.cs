using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
namespace EmployeeService
{
    [DataContract]
    public class CustomTypeException
    {
      
        private string _description;
        private string _reason;
       
        [DataMember]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        [DataMember]
        public string Reason
        {
            get { return _reason; }
            set { _reason = value; }
        }
    }


}