using WebApplication2.Persistence.Models;
using MimeKit;
using MailKit.Net.Smtp;

namespace WebApplication2
{
    public class EmailNotificationService
    {
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _fromEmail;
        private readonly string _password;
        //twork3320@gmail.com
        public EmailNotificationService(string smtpServer, int port, string fromEmail, string password)
        {
            _smtpServer = smtpServer;
            _port = port;
            _fromEmail = fromEmail;
            _password = password;
        }

        public async Task SendStockDataEmailAsync(StockData stockData, string toEmail)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Server", _fromEmail));
            message.To.Add(new MailboxAddress("Reciever", toEmail));
            message.Subject = "Test Email from MailKit";
            message.Body = new TextPart("html") { Text = GenerateHtmlEmailBody(stockData) };

            using (var client = new SmtpClient())
            {
                // Accept all SSL certificates (not recommended for production)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync(_smtpServer, _port, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_fromEmail, _password); // Use App Password if 2FA is enabled
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        // Generate HTML email body with stock data in a table format
        private string GenerateHtmlEmailBody(StockData stockData)
        {
            return $@"
            <html>
            <body>
                <h2>Stock Data for {stockData.Ticker} on {stockData.Date.ToString("yyyy-MM-dd")}</h2>
                <table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse;'>
                    <thead>
                        <tr>
                            <th>Ticker</th>
                            <th>Date</th>
                            <th>Open Price</th>
                            <th>Close Price</th>
                            <th>High Price</th>
                            <th>Low Price</th>
                            <th>Volume</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>{stockData.Ticker}</td>
                            <td>{stockData.Date.ToString("yyyy-MM-dd")}</td>
                            <td>{stockData.OpenPrice:C}</td>  <!-- Format as currency -->
                            <td>{stockData.ClosePrice:C}</td> <!-- Format as currency -->
                            <td>{stockData.HighPrice:C}</td>  <!-- Format as currency -->
                            <td>{stockData.LowPrice:C}</td>   <!-- Format as currency -->
                            <td>{stockData.Volume:N0}</td>     <!-- Format as number with thousand separator -->
                        </tr>
                    </tbody>
                </table>
            </body>
            </html>";
        }
    }
}
