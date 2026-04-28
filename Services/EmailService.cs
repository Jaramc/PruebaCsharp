using Microsoft.EntityFrameworkCore;
using PruebaCsharp.Data;
using PruebaCsharp.Models;
using System.Net;
using System.Net.Mail;

namespace PruebaCsharp.Services
{
    public class EmailService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public EmailService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task SendReservationConfirmationAsync(int reservationId)
        {
            Reservation? reservation = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.SportsFacility)
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            if (reservation == null)
            {
                return;
            }

            string subject = "Reservation Confirmation";

            string body = $@"
Hello {reservation.User.Name},

Your reservation has been created successfully.

Reservation details:

User: {reservation.User.Name}
Document: {reservation.User.DocumentId}
Sports Facility: {reservation.SportsFacility.Name}
Type Of Space: {reservation.SportsFacility.TypeOfSpace}
Date: {reservation.ReservationDate:dd/MM/yyyy}
Start Time: {reservation.StartTime:hh\:mm}
End Time: {reservation.EndTime:hh\:mm}
Status: {reservation.Status}

Thank you for using the Sports Reservation System.
";

            Notification notification = new Notification
            {
                ReservationId = reservation.Id,
                EmailRecipient = reservation.User.Email,
                Subject = subject,
                DateSent = DateTime.Now,
                WasSent = false,
                ErrorMessage = string.Empty
            };

            try
            {
                string host = _configuration["SmtpSettings:Host"] ?? string.Empty;
                string portText = _configuration["SmtpSettings:Port"] ?? "587";
                string senderEmail = _configuration["SmtpSettings:SenderEmail"] ?? string.Empty;
                string senderPassword = _configuration["SmtpSettings:SenderPassword"] ?? string.Empty;
                string enableSslText = _configuration["SmtpSettings:EnableSsl"] ?? "true";

                if (string.IsNullOrWhiteSpace(host) ||
                    string.IsNullOrWhiteSpace(senderEmail) ||
                    string.IsNullOrWhiteSpace(senderPassword))
                {
                    notification.WasSent = false;
                    notification.ErrorMessage = "SMTP settings are incomplete.";
                }
                else
                {
                    int port = int.Parse(portText);
                    bool enableSsl = bool.Parse(enableSslText);

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(senderEmail);
                    mailMessage.To.Add(reservation.User.Email);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = false;

                    SmtpClient smtpClient = new SmtpClient(host);
                    smtpClient.Port = port;
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtpClient.EnableSsl = enableSsl;

                    await smtpClient.SendMailAsync(mailMessage);

                    notification.WasSent = true;
                    notification.ErrorMessage = string.Empty;
                }
            }
            catch (Exception ex)
            {
                notification.WasSent = false;
                notification.ErrorMessage = ex.Message;
            }

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
    }
}