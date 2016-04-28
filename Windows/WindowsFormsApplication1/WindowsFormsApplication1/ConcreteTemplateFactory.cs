using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class ConcreteTemplateFactory: TemplateFactory
    {
        public ConcreteTemplateFactory()
        {
        }

        public ConcreteTemplate makeTemplate(String name, List<TemplateParse> boxes)
        {
            if (name != null && boxes != null)
            {
                 return new ConcreteTemplate(name, boxes);
            }
            return null;
        }
    }
}
