import { NextFunction, Request, Response } from 'express';
import {
  ExpressErrorMiddlewareInterface,
  HttpError,
  Middleware,
} from 'routing-controllers';

@Middleware({ type: 'after' })
class ErrorMiddleware implements ExpressErrorMiddlewareInterface {
  error(error: any, _req: Request, res: Response, next: NextFunction) {
    if (error instanceof HttpError) {
      res.locals.errorMessage = error.message;

      const response = {
        code: error.httpCode,
        message: error.message,
      };

      res.status(error.httpCode).json(response);
    }

    next(error);
  }
}

export { ErrorMiddleware };
