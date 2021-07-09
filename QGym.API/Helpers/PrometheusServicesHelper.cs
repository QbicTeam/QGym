using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using prometheus.dto.gym.PrometheusMgr;
using prometheus.model.gym;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace QGym.API.Helpers
{
    public class PrometheusServicesHelper
    {
        private readonly IOptions<AppSettings> _appSettings;

        public PrometheusServicesHelper(IOptions<AppSettings> appSettings)
        {
            this._appSettings = appSettings;
        }

        public void SendEmail(string emailTo, string memberName, string confirmationCode)
        {
            // var emailUrl = "http://prometheusapis.net/emailapi/emails";

            WebRequest oRequest = WebRequest.Create(this._appSettings.Value.EmailUrl);  //emailUrl);
            oRequest.Method = "post";
            oRequest.ContentType = "application/json;charset-UTF-8";
            // oRequest.Headers["x-api-key"] = "03ffbf2f-f820-4655-90f2-ea7dc1689fba";
            oRequest.Headers.Add("x-api-key", this._appSettings.Value.EmailApiKey); // "03ffbf2f-f820-4655-90f2-ea7dc1689fba");
            using (var oSW = new StreamWriter(oRequest.GetRequestStream()))
            {
                var values = new List<Variables>();
                values.Add(new Variables() { Variable = "Socio", Value = memberName });
                values.Add(new Variables() { Variable = "Codigo", Value = confirmationCode });
                var emailCode = new SendEmailDto()
                {
                    To = emailTo,
                    Body = "",
                    Subject = this._appSettings.Value.SubjectConfirmationEmail, //"Codigo de Confirmacion",
                    IsHtml = true,
                    TemplateId = this._appSettings.Value.TemplateIdConfirmationEmail,//4,
                    Values = values
                };
                string json = JsonConvert.SerializeObject(emailCode);
                oSW.Write(json);
                oSW.Flush();
                oSW.Close();
            }
            WebResponse oResponse = oRequest.GetResponse();
            using (var oSR = new StreamReader(oResponse.GetResponseStream()))
            {
                oSR.ReadToEnd();
            }
        }
    }
}
