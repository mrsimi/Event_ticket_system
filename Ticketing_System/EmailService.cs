using iTextSharp.text;
using iTextSharp.text.pdf;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ticketing_System
{
    public class EmailService
    {
        private string _firstName;
        private string _lastName;
        private string _seatNumber;
        private string _email;
        private string _path; 
        public EmailService(string firstName, string lastName, string seatNumber, string email)
        {
            _firstName = firstName;
            _lastName = lastName;
            _seatNumber = seatNumber;
            _email = email;
        }

        public void CreatPdf()
        {
            string name = _firstName + " " + _lastName;
            _path = Path.Combine(Environment.CurrentDirectory, "ticket.pdf");

            FileStream file = File.Create(_path);
            //document 
            Rectangle rectangle = new Rectangle(400, 250);
            Document document = new Document(rectangle);

            document.SetMargins(5f, 5f, 10f, 5f);
            PdfWriter writer = PdfWriter.GetInstance(document, file);

            try
            {
                document.AddAuthor("Event Company");
                document.AddKeywords("Event");
                document.AddTitle("The ticket for this person");

                document.Open();
                Font heading = FontFactory.GetFont("courier", 13);
                Font body = FontFactory.GetFont("courier", 10);

                Paragraph p1 = new Paragraph("Dot Net CONFERENCE", heading);
                p1.Alignment = Element.ALIGN_CENTER;
                Paragraph p2 = new Paragraph(" ");

                Paragraph p3 = new Paragraph("VENUE", heading);
                p3.Alignment = Element.ALIGN_CENTER;
                Paragraph p4 = new Paragraph("12 Life Avenue, Black Street, Victoria Island, Lagos State", body);
                p4.Alignment = Element.ALIGN_CENTER;

                Paragraph p5 = new Paragraph("DATE", heading);
                p5.Alignment = Element.ALIGN_CENTER;
                Paragraph p6 = new Paragraph("20 October, 2019 at 1.00 pm CAT", body);
                p6.Alignment = Element.ALIGN_CENTER;

                Paragraph p7 = new Paragraph(" ");

                Paragraph p8 = new Paragraph("NAME", heading);
                p8.Alignment = Element.ALIGN_CENTER;
                Paragraph p9 = new Paragraph(name, body);
                p9.Alignment = Element.ALIGN_CENTER;

                Chunk chk1 = new Chunk("SEAT NUMBER: ", heading);
                Chunk chk2 = new Chunk(_seatNumber, body);

                Phrase phrase = new Phrase();
                phrase.Add(chk1);
                phrase.Add(chk2);

                Paragraph p10 = new Paragraph();
                p10.Add(phrase);
                p10.Alignment = Element.ALIGN_CENTER;

                Phrase[] phrasesArray = new Phrase[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 };
                for (int i = 0; i < phrasesArray.Length; i++)
                {
                    document.Add(phrasesArray[i]);
                }

            }
            catch (DocumentException documentException)
            {
                throw documentException;
            }
            catch (IOException ioException)
            {
                throw ioException;
            }
            finally
            {
                document.Close();
                writer.Close();
                file.Close();                
            }
        }

        public void SendEmaill()
        {
            string name = _firstName + " " + _lastName;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("dotnet", "adegokesimi@gmail.com"));
            message.To.Add(new MailboxAddress("DotNet Conference", _email));

            message.Subject = "Ticket for DotNet Conference" ;
            var body = new TextPart("plain")
            {
                Text = "Thank you " + name + " for registering for this event, we wish you a ncie time till then and during the event."
                            + "attached is the copy of the ticket with all the necessary details"
            };

            var attachment = new MimePart("ticket", "pdf")
            {
                Content = new MimeContent(File.OpenRead(_path), ContentEncoding.Default),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName(_path)
            };

            var multipart = new Multipart("mixed");
            multipart.Add(body);
            multipart.Add(attachment);

            message.Body = multipart;

            //configuring smtp.client 
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("adegokesimi@gmail.com", "adegoke1234");
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
