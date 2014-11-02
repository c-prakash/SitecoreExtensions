/*
 --------------------------------------------------------------
 ***** Source code from *****
 http://webcmd.wordpress.com/2013/01/24/sitecore-mvc-area-controller-rendering-type/
 --------------------------------------------------------------
 */

using Sitecore.Mvc.Presentation;
using Sitecore.StringExtensions;

namespace Framework.Sc.Extensions.MvcAreas
{
    public class AreaControllerRenderer : Renderer
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
        public string Namespaces { get; set; }

        public override string CacheKey
        {
            get
            {
                return "areacontroller::" + Controller + "#" + this.Action + "#" + Area + "#" + Namespaces;
            }
        }

        public override void Render(System.IO.TextWriter writer)
        {
            AreaControllerRunner controllerRunner = new AreaControllerRunner(this.Controller, this.Action, this.Area, this.Namespaces);

            string value = controllerRunner.Execute();
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            writer.Write(value);
        }

        public override string ToString()
        {
            return "Controller: {0}. Action: {1}. Area {2}. Namespaces {3}".FormatWith(new object[]
			{
				this.Controller,
				this.Action,
                this.Area,
                this.Namespaces
			});
        }
    }
}
