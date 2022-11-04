import { Request, Response } from 'express';
import morgan from 'morgan';

import { Logger } from '../config';
import { Env } from '../constants';

class MorganHandlerMiddleware {
  private errorResponseFormat: string = ':method :url :status message: :message - :response-time ms';
  private successResponseFormat: string = ':method :url :status - :response-time ms';

  constructor() {
    morgan.token('message', (_req: Request, res: Response) => res.locals.errorMessage || '');

    if (Env.NODE === 'prod') {
      this.errorResponseFormat =
        ':remote-addr - :remote-user [:date[clf]] ":method :url HTTP/:http-version" :status :res[content-length] message: :message ":referrer" ":user-agent"';

      this.successResponseFormat =
        '::remote-addr - :remote-user [:date[clf]] ":method :url HTTP/:http-version" :status :res[content-length] ":referrer" ":user-agent"';
    }
  }

  onError = morgan(this.errorResponseFormat, {
    skip: (_req: Request, res: Response) => res.statusCode < 400,
    stream: { write: (message: string) => Logger.error(message) }
  });

  onSuccess = morgan(this.successResponseFormat, {
    skip: (_req: Request, res: Response) => res.statusCode >= 400,
    stream: { write: (message: string) => Logger.info(message) }
  });
}

const MorganMiddleware = new MorganHandlerMiddleware();

export { MorganMiddleware };
