using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Application.Interface
{
    public interface ITemplateRendererService
    {
        string Render(string template, object model);
    }
}
