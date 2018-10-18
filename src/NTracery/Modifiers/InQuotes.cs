using System;
using System.Collections.Generic;
using System.Text;

namespace NTracery.Modifiers {

    class InQuotes : IModifier {

        public string Modify(string input) {
            return string.Format("\"{0}\"", input);
        }

    }

}
