using System;
using System.Collections.Generic;
using System.Text;

namespace SLProject.SLCompilerLib.Parser
{
    class Accessor
    {
        private List<string> idents;
        public Accessor(List<string> idents)
        {
            this.idents = idents;
        }

        public List<string> GetIdentifiers { get => idents; set => idents = value; }

        public override string ToString()
        {
            string res = "";
            foreach (var item in idents)
            {
                res += item + ".";
            }
            return res;
        }
    }
}
