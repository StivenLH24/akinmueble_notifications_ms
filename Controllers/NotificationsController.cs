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
    [Route("Email")]
    [HttpPost]
    public async Task<ActionResult> SendEmail(ModelEmail data)
    {
        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("johan.1701922249@ucaldas.edu.co", "Johan Stiven Osorio Grisales");
        var subject = data.subjectEmail;
        var to = new EmailAddress(data.destinationEmail, data.destinationName);
        var plainTextContent = "plain text content";
        var htmlContent = data.contectEmail;
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);
        if(response.StatusCode == System.Net.HttpStatusCode.Accepted){
            return Ok("Correo enviado a la dirrecion " + data.destinationEmail);
        }
        else {
            return BadRequest("Error enviando el mensaje a la dirrecion " + data.destinationEmail);
        }
    }
}
