using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public interface Template
    {
        List<TemplateParse> getList();

        String getId();
    }
}
