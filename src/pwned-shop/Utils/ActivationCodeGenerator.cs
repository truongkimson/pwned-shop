using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pwned_shop.Utils
{
    public class ActivationCodeGenerator
    {
        public static string[] GetCode()
        {
            // mock up code to simulate code generation for each product
            // in the final implementation, GetCode() should take in a list
            // of Product objects and return a list of corresponding activation codes
            string[] codeList = new string[3];
            for (int i = 0; i < 3; i++)
            {
                codeList[i] = Guid.NewGuid().ToString();
            }
            return codeList;
        }
    }
}
