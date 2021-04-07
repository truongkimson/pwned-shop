using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pwned_shop.Utils
{
    public class ActivationCodeGenerator
    {
        public static Guid[] GetCode()
        {
            // mock up code to simulate code generation for each product
            // in the final implementation, GetCode() should take in a list
            // of Product objects and return a list of corresponding activation codes
            Guid[] codeList = new Guid[3];
            for (int i = 0; i < 3; i++)
            {
                codeList[i] = Guid.NewGuid();
            }
            return codeList;
        }
    }
}
