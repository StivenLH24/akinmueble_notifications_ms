using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using ms_notifications.Models;

namespace akinmueble_notifications_ms.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationsController : ControllerBase
{   
    [Route("Email-confirmation")]
    [HttpPost]
    public async Task<ActionResult> SendEmailconfirmation(ModelEmail data)
    {
        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        var client = new SendGridClient(apiKey);
        
        SendGridMessage msg = this.CreateStandardMessage(data);
        msg.SetTemplateId(Environment.GetEnvironmentVariable("CONFIRMACION_EMAIL_SENDGRID_TEMPLATE_ID"));
        msg.SetTemplateData(new{
            name=data.destinationName,
            message="Sea parte de nuestra comunidad a solo un click"
        });
        var response = await client.SendEmailAsync(msg);
        if(response.StatusCode == System.Net.HttpStatusCode.Accepted){
            return Ok("Correo enviado a la direcion " + data.destinationEmail);
        }
        else {
            return BadRequest("Error enviando el mensaje a la dirrecion " + data.destinationEmail);
        }
    }

    [Route("Email-password-recovery")]
    [HttpPost]
    public async Task<ActionResult> SendEmailPasswordRecovery(ModelEmail data)
    {
        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        var client = new SendGridClient(apiKey);

        SendGridMessage msg = this.CreateStandardMessage(data);
        msg.SetTemplateId(Environment.GetEnvironmentVariable("CONFIRMACION_EMAIL_SENDGRID_TEMPLATE_ID"));
        msg.SetTemplateData(new{
            name=data.destinationName,
            message="recuperacion de clave"
        });
        var response = await client.SendEmailAsync(msg);
        if(response.StatusCode == System.Net.HttpStatusCode.Accepted){
            return Ok("Correo enviado a la dirrecion " + data.destinationEmail);
        }
        else {
            return BadRequest("Error enviando el mensaje a la dirrecion " + data.destinationEmail);
        }
    }

    [Route("send-email-2fa")]
    [HttpPost]
    public async Task<ActionResult> SendEmail2fa(ModelEmail data)
    {
        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        var client = new SendGridClient(apiKey);

        SendGridMessage msg = this.CreateStandardMessage(data);
        msg.SetTemplateId(Environment.GetEnvironmentVariable("TWOFA_EMAIL_SENDGRID_TEMPLATE_ID"));
        msg.SetTemplateData(new{
            name = data.destinationName,
            message= data.contectEmail,
            subject = data.subjectEmail
        });
        var response = await client.SendEmailAsync(msg);
        if(response.StatusCode == System.Net.HttpStatusCode.Accepted){
            return Ok("Correo enviado a la dirrecion " + data.destinationEmail);
        }
        else {
            return BadRequest("Error enviando el mensaje a la dirrecion " + data.destinationEmail);
        }
    }

    private SendGridMessage CreateStandardMessage(ModelEmail data)
    {
        var from = new EmailAddress(Environment.GetEnvironmentVariable("EMAIL_FROM"), Environment.GetEnvironmentVariable("NAME_FROM"));
        var subject = data.subjectEmail;
        var to = new EmailAddress(data.destinationEmail, data.destinationName);
        var plainTextContent = data.contectEmail;
        var htmlContent = data.contectEmail;
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        return msg;
    } 
}
