using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace MyResumeSite.Services
{
    public class NotificationService
    {
        private readonly IJSRuntime _jSRuntime;

        public NotificationService(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }

        public async Task ConsoleLog(object obj)
        {
            try
            {
                if (obj == null)
                {
                    await _jSRuntime.InvokeVoidAsync("console.log", "Was null");
                }
                else
                {
                    await _jSRuntime.InvokeVoidAsync("console.log", Newtonsoft.Json.JsonConvert.SerializeObject(obj));
                }
             
            }
            catch (Exception)
            {
            }
        }
    }
}
