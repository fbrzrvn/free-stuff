import nodemailer from 'nodemailer';

import { Logger } from '../../../config';

class EmailService {
  send(to: string, subject: string, html: string) {
    const transporter = nodemailer.createTransport({
      host: 'smtp.ethereal.email',
      port: 587,
      auth: {
        user: 'vallie.turcotte43@ethereal.email',
        pass: '6ramB5j3hjGdDhbJDr'
      }
    });

    const mailOptions = {
      from: '"Free Stuff 😃" <freestuff@email.com>',
      to,
      subject,
      html
    };

    transporter.sendMail(mailOptions, function (error, info) {
      if (error) {
        Logger.error(error);
      } else {
        Logger.info(`EmailService ${JSON.stringify(info)}`);
      }
    });
  }
}

export { EmailService };
