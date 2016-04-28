using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class ConcreteTemplate: Template
    {
        private List<TemplateParse> tempParseArr;
        private String templateID;

        public ConcreteTemplate(String ident, List<TemplateParse> templateList)
        {
            templateID = ident;
            tempParseArr = templateList;
        }

        public String getId()
        {
            return templateID;
        }

        public List<TemplateParse> getList()
        {
            return tempParseArr;
        }
    }
}
