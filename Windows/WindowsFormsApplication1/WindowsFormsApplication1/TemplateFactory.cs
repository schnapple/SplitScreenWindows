using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    interface TemplateFactory
    {
        ConcreteTemplate makeTemplate(String name, List<TemplateParse> boxes);
    }
}
