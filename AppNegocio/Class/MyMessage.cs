using AppNegocio.Models.Commons;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace AppNegocio.Class
{
    public  class MyMessage : ValueChangedMessage<string>
    {
        public Cliente Cliente { get; set; }

        public MyMessage(string value):base(value)
        {

        }
    }
}
