using HoangNV.HotelBooking.Web.Localization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace HoangNV.HotelBooking.Web.Helper
{
    public static class StringExtensions
    {
        public static string ReplaceAt(this string str, int index, int length, string replace)
        {
            return string.Create(str.Length - length + replace.Length, (str, index, length, replace),
                (span, state) =>
                {
                    state.str.AsSpan().Slice(0, state.index).CopyTo(span);
                    state.replace.AsSpan().CopyTo(span[state.index..]);
                    state.str.AsSpan()[(state.index + state.length)..].CopyTo(span[(state.index + state.replace.Length)..]);
                });
        }
    }
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync<TModel>(string viewName, TModel model, string controller = "EmailBookTemplate");

        Task<string> RenderToStringWithResourceAsync<TModel>(string controller, string viewName, TModel model);

        string GenerateEmailContent<T>(string template, T input, bool forTitle = false);
    }

    public class ViewRenderService : IViewRenderService
    {
        private readonly ILogger<ViewRenderService> _logger;
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStringLocalizer<SharedResource> _localizer;

        private const string Email_Body_Format = @"
                <!DOCTYPE html>
                <html lang='ja'>
                <head>
                    <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
                    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                </head>
                <body>
                    <pre>{0}</pre>
                </body>
                </html>
                ";

        public ViewRenderService(
            IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider,
            ILogger<ViewRenderService> logger,
            IWebHostEnvironment webHostEnvironment,
            IStringLocalizer<SharedResource> localizer)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _localizer = localizer;
        }

        private void CheckNullViewResult(string viewName, ViewEngineResult viewEngineResult)
        {
            if (viewEngineResult.View == null)
            {
                string error = $"{viewName} does not match any available view";
                _logger.LogError(error);
                throw new ArgumentNullException(error);
            }
        }

        public async Task<string> RenderToStringAsync<TModel>(string viewName, TModel model, string controller = "EmailBookTemplate")
        {
            using IServiceScope requestServices = _serviceProvider.CreateScope();
            var httpContext = new DefaultHttpContext { RequestServices = requestServices.ServiceProvider };
            var routeData = new RouteData();
            routeData.Values.Add("controller", controller);
            var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());

            using var sw = new StringWriter();
            var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);

            CheckNullViewResult(viewName, viewResult);

            var viewDictionary = new ViewDataDictionary(
                new EmptyModelMetadataProvider(),
                new ModelStateDictionary());

            if (model != null)
                viewDictionary.Model = model;

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewDictionary,
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                sw,
                new HtmlHelperOptions());

            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }

        public async Task<string> RenderToStringWithResourceAsync<TModel>(string controller, string viewName, TModel model)
        {
            var htmlString = await RenderToStringAsync(viewName, model, controller);
            htmlString = EmbedImagesInHTML(htmlString);

            var cssString = await GetCSSContent();
            cssString = EmbedImagesInCSS(cssString);

            var viewString = htmlString.Replace("</head>", "<style>" + cssString + "</style></head>");
            return viewString;
        }

        private string EmbedImagesInHTML(string htmlString)
        {
            var urlRegex = @"src=""([^""]+)"""; // src="([^"]+)"
            var matchResults = Regex.Matches(htmlString, urlRegex);

            if (matchResults.Count > 0)
            {
                for (int i = matchResults.Count - 1; i >= 0; i--)
                {
                    var match = matchResults[i];
                    var originalUrl = match.Groups[1].Value;

                    string imageString = ConvertImageFileToBase64(originalUrl);
                    if (string.IsNullOrEmpty(imageString)) continue;
                    string urlImageString = $"src=\"{imageString}\"";

                    htmlString = htmlString.ReplaceAt(match.Index, match.Groups[1].Value.Length + "src=\"\"".Length, urlImageString);
                }
            }

            return htmlString;
        }

        private async Task<string> GetCSSContent()
        {
            var cssStream = _webHostEnvironment.WebRootFileProvider.GetFileInfo("css/site.min.css").CreateReadStream();
            using var streamReader = new StreamReader(cssStream);
            var cssString = await streamReader.ReadToEndAsync();
            return cssString;
        }

        // Replace url("[image path]") in CSS with base64-encoded data
        private string EmbedImagesInCSS(string cssString)
        {
            var urlRegex = @"url\(""([^\)]+)""\)"; // url\("([^\)]+)"\)
            var matchResults = Regex.Matches(cssString, urlRegex);

            if (matchResults.Count > 0)
            {
                for (int i = matchResults.Count - 1; i >= 0; i--)
                {
                    var match = matchResults[i];
                    var originalUrl = match.Groups[1].Value;
                    var formattedUrl = originalUrl.StartsWith("../") ? originalUrl.Remove(0, 3) : originalUrl;

                    string imageString = ConvertImageFileToBase64(formattedUrl);
                    if (string.IsNullOrEmpty(imageString)) continue;
                    string urlImageString = $"url(\"{imageString}\")";

                    cssString = cssString.ReplaceAt(match.Index, match.Groups[1].Value.Length + "url(\"\")".Length, urlImageString);
                }
            }

            return cssString;
        }

        private string ConvertImageFileToBase64(string webRootFilePath)
        {
            var fileInfo = _webHostEnvironment.WebRootFileProvider.GetFileInfo(webRootFilePath);
            if (!fileInfo.Exists) return string.Empty;

            var filePath = fileInfo.PhysicalPath;
            var fileBytes = File.ReadAllBytes(filePath);
            var imageString = $"data:image/{Path.GetExtension(filePath)[1..]};base64,{Convert.ToBase64String(fileBytes)}";
            return imageString;
        }

        public string GenerateEmailContent<T>(string template, T input, bool forTitle = false)
        {
            if (!forTitle) template = CheckToUseHtmlTable(template);

            var pros = input.GetType().GetProperties();
            foreach (var pro in pros)
            {
                var displayName = GetDisplayAttributeValue(pro);

                if (!string.IsNullOrWhiteSpace(displayName))
                {
                    var value = string.Empty;
                    if (pro.PropertyType == typeof(DateTime?) || pro.PropertyType == typeof(DateTime))
                    {
                        var dtValue = pro.GetValue(input) as DateTime?;
                        value = _localizer["Booking_HealthDate_Null"].Value;
                        if (dtValue.HasValue)
                            value = dtValue.Value.ToString(_localizer["Email_Template_HealthDate"].Value);
                    }
                    else
                    {
                        if (pro.PropertyType == typeof(string))
                        {
                            var description = GetDescriptionAttributeValue(pro) ?? "";
                            value = pro.GetValue(input)?.ToString() ?? "";
                            if (description.ToLower() == "url")
                            {
                                value = $"<a href='{value}'>{value}</a><br>";
                            }

                            if (description.ToLower() == "image")
                            {
                                //value = $"<img src='data:image/png;base64, {value}' />";
                                value = $"<img src=cid:{value} />";
                            }
                        }
                    }

                    template = template.Replace($"<<{displayName}>>", value);
                }
            }

            if (!forTitle) template = template.Replace("<<", "&lt;&lt;").Replace(">>", "&gt;&gt;");
            if (!forTitle) template = string.Format(Email_Body_Format, template);

            return template;
        }

        private string CheckToUseHtmlTable(string contentTemplate)
        {
            //need to use the table to display HealthFacilityName and QRCode
            var tableHtml = @"
                <table>
                    <tr>
                        <td style='vertical-align:top'><<HealthFacilityName>></td>
                        <td style='padding:32px'></td>
                        <td style='vertical-align:top'>
                            <<QRCode>>
                        </td>
                    </tr>
                </table>";
            var newStr = new StringBuilder();

            var useTable = false;

            using var sr = new StringReader(contentTemplate);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("<<HealthFacilityName>>") & line.Contains("<<QRCode>>"))
                {
                    useTable = true;
                    newStr.Append(tableHtml);
                }
                else
                {
                    newStr.Append(line);
                }

                newStr.Append(Environment.NewLine);
            }

            return useTable ? newStr.ToString() : contentTemplate;
        }

        private string GetDisplayAttributeValue(PropertyInfo property)
        {
            var attribute = property.GetCustomAttributes<DisplayNameAttribute>().SingleOrDefault();
            return attribute?.DisplayName;
        }

        private string GetDescriptionAttributeValue(PropertyInfo property)
        {
            var attribute = property.GetCustomAttributes<DescriptionAttribute>().SingleOrDefault();
            return attribute?.Description;
        }
    }
    public class Interview_SendEmailRequestModel
    {
        public string UrlInterview { get; set; }
        public string Email { get; set; }
        public string HealthFacilityName { get; set; }
    }
    public partial class EmailQueue
    {
        public EmailQueue()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public int Id { get; set; }

        public string Email { get; set; }

        public string Content { get; set; }

        public DateTime? SendDate { get; set; }

        public bool Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public string Subject { get; set; }

        #endregion

        #region Generated Relationships
        #endregion

    }
    public class AttachFileInEmailModel
    {
        public byte[] Bytes { get; set; }
        public string MediaType { get; set; }
        public string Id { get; set; }
    }
    public class EmailModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public object Data { get; set; }

        public List<AttachFileInEmailModel> AttachFiles { get; set; }
    }
    public interface IThrottlingEmail
    {
        void SendEmailsAsync(Queue<EmailModel> emails, bool isReSend = false);
    }
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, params AttachFileInEmailModel[] attachFiles);
    }
    public class SmtpSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string FromEmail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
   
    public static class EmailHelper
    {
        public static readonly List<(string domain, string charsert, Encoding encoding)> ENCODING_IN_DOMAINS = new()
        {
            ("@docomo.ne.jp", "Shift-JIS", Encoding.GetEncoding("shift_jis")),
            ("@softbank.ne.jp", "Shift-JIS", Encoding.GetEncoding("shift_jis")),
            ("@ezweb.ne.jp", "iso-2022-jp", Encoding.GetEncoding("iso-2022-jp")),
        };
        public static string GetCharsetFormEmail(string email)
        {
            var charset =ENCODING_IN_DOMAINS.FirstOrDefault(d => email.EndsWith(d.domain)).charsert;
            return string.IsNullOrEmpty(charset) ? "UTF-8" : charset;
        }

        public static Encoding GetEncodingFormEmail(string email)
        {
            var encoding = ENCODING_IN_DOMAINS.FirstOrDefault(d => email.EndsWith(d.domain)).encoding;
            return encoding ?? Encoding.UTF8;
        }

        public static string SetSubjectEncoding(string subject, Encoding encoding)
        {
            if (Encoding.Equals(encoding, Encoding.GetEncoding("iso-2022-jp")))
            {
                string base64str = Convert.ToBase64String(encoding.GetBytes(subject));
                return string.Format("=?{0}?B?{1}?=", encoding.BodyName, base64str);
            }
            return subject;
        }
    }
    public class EmailSender : IEmailSender
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailSender(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message, params AttachFileInEmailModel[] attachFiles)
        {
            SmtpClient client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryFormat = SmtpDeliveryFormat.International,
                Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password)
            };

            MailAddress from = new MailAddress(_smtpSettings.FromEmail);
            MailAddress to = new MailAddress(email);
            var encoding = EmailHelper.GetEncodingFormEmail(email);
            MailMessage mailMessage = new MailMessage(from, to)
            {
                //Body = message,
                BodyEncoding = encoding,
                Subject = EmailHelper.SetSubjectEncoding(subject, encoding),
                SubjectEncoding = encoding,
                IsBodyHtml = true
            };

            Stream stream = new MemoryStream();
            if (attachFiles != null && attachFiles.Any())
            {
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(message, encoding, "text/html");
                foreach (var attachFile in attachFiles)
                {
                    stream = new MemoryStream(attachFile.Bytes);
                    LinkedResource theEmailImage = new LinkedResource(stream, attachFile.MediaType);
                    theEmailImage.ContentId = attachFile.Id;

                    //Add the Image to the Alternate view
                    htmlView.LinkedResources.Add(theEmailImage);
                }

                //Add view to the Email Message
                mailMessage.AlternateViews.Add(htmlView);
            }
            else mailMessage.Body = message;

            client.SendCompleted += (a, e) =>
            {
                mailMessage.Dispose();
                client.Dispose();
                stream.Close();
            };

            return client.SendMailAsync(mailMessage);
        }
    }
    public class ThrottlingEmail : IThrottlingEmail

    {
        private readonly Timer _timer;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<EmailQueue> _logger;

        public ThrottlingEmail(IEmailSender emailSender, ILogger<EmailQueue> logger, IConfiguration configuration)
        {
            _emailSender = emailSender;
            _logger = logger;
            _timer = new Timer(100* 1);
        }

        public void SendEmailsAsync(Queue<EmailModel> emails, bool isReSend = false)
        {
            _timer.Elapsed += async (sender, e) => await SendEmailEventAsync(emails, isReSend);
            _timer.AutoReset = true;
            _timer.Start();
        }

        private async Task SendEmailEventAsync(Queue<EmailModel> emailQueue, bool isReSend)
        {
            _timer.Enabled = false;
            if (emailQueue.TryDequeue(out var email))
            {
                try
                {
                    await SendEmailAsync(email, isReSend);
                }
                catch (Exception exception)
                {
                    await HandlerOtherExceptionAsync(exception, email, isReSend);
                }
                finally
                {
                    _timer.Enabled = true;
                }
            }
            else
            {
                _timer.Stop();
            }
        }

        private async Task SendEmailAsync(EmailModel email, bool isReSend)
        {
            if (string.IsNullOrEmpty(email.Email)) return;
            LogEmailData(email.Data);
            await _emailSender.SendEmailAsync(email.Email, email.Subject, email.Content, email.AttachFiles?.ToArray());
        }

        private async Task HandlerOtherExceptionAsync(Exception exception, EmailModel email, bool isReSend)
        {
             _logger.LogError("SendEmailEventAsync", exception.Message);
        }

        private EmailQueue ConvertEmailModelToEntity(EmailModel email)
        {
            return new EmailQueue()
            {
                Email = email.Email,
                Subject = email.Subject,
                Content = email.Content,
                Status = false,
            };
        }

        private void LogEmailData(object data)
        {
            if (data == null)
            {
                return;
            }

            if (data is Interview_SendEmailRequestModel)
            {
                var emailData = data as Interview_SendEmailRequestModel;
                _logger.LogInformation($"SendEmailEventAsync {emailData?.UrlInterview}");
            }
        }
    }
}
