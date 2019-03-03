namespace Jarvis_Brain.Controllers
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using Jarvis_Brain.Services;
    using Jarvis_Brain.Models;
    using Jarvis_Brain.ViewModels;
    using System.Linq;
    public class HomeController : Controller
    {
        private IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider { get; }
        public HomeController(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _actionDescriptorCollectionProvider  = actionDescriptorCollectionProvider;
        }

        public IActionResult Index(){
            return View();
        }

        public IActionResult InternalApi(){
            ViewBag.ControllerActions = ControllerActions("InternalComms");
            return View();
        }

        public IActionResult ExternalApi(){
            var controllerActions = ControllerActions("ExternalComms");
            return View();
        }

        public Dictionary<string,string> ControllerActions (string controllerName){
            var controllerActions = new Dictionary<string,string>();
            foreach (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor actionDescriptor in _actionDescriptorCollectionProvider.ActionDescriptors.Items){
                if(actionDescriptor.ControllerName == controllerName)
                {
                    StringBuilder parametersStringBuilder = new StringBuilder(); 
                    foreach(var parameter in actionDescriptor.Parameters){
                        parametersStringBuilder.AppendLine(string.Format("{0}\t\t\t{1}", parameter.Name, parameter.ParameterType.Name));
                    }

                    controllerActions.Add(
                        actionDescriptor.ActionName,
                        string.Format("Parameters:\r\n{0}\r\nReturns:\r\n{1}", parametersStringBuilder.ToString(), actionDescriptor.MethodInfo.ReturnType.Name)
                    );
                }
            }
            return controllerActions;
        }
    }
}