using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCustomCell.Models
{
    public class InputReferenceItem
    {
        public string ReferenceColumnName { get; set; }

        private string referenceValue;

        public string ReferenceValue
        {
            get { return referenceValue; }
            set { referenceValue = value; }
        }
    }
}
