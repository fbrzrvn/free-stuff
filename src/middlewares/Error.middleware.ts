import { NextFunction, Request, Response } from 'express';
import mongoose from 'mongoose';
import { ExpressErrorMiddlewareInterface, HttpError, Middleware } from 'routing-controllers';

@Middleware({ type: 'after' })
class ErrorMiddleware implements ExpressErrorMiddlewareInterface {
  error(error: any, _req: Request, res: Response, next: NextFunction) {
    if (error instanceof HttpError || error instanceof mongoose.Error) {
      res.locals.errorMessage = error.message;

      const response = {
        code: error instanceof HttpError ? error.httpCode : 400,
        message: error.message
      };

      res.status(response.code).json(response);
    } else {
      next(error);
    }
  }
}

export { ErrorMiddleware };
