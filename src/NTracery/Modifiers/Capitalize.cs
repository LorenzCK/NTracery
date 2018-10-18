using System;
using System.Collections.Generic;
using System.Text;

namespace NTracery.Modifiers {

    class Capitalize : IModifier {

        public string Modify(string input) {
            return Char.ToUpper(input[0]) + input.Substring(1);
        }

    }

}
