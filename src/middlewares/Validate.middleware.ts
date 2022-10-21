import { plainToInstance } from 'class-transformer';
import { validate, ValidationError } from 'class-validator';
import { NextFunction, Request, RequestHandler, Response } from 'express';

import { HttpException } from '../exceptions/HttpException';

class ValidateMiddleware {
  private static _getAllNestedErrors(error: ValidationError): any {
    if (error.constraints) {
      return Object.values(error.constraints);
    }
    return error.children?.map(this._getAllNestedErrors).join(',');
  }

  static validate(
    type: any,
    value: 'body' | 'query' | 'params' = 'body',
    skipMissingProperties = false,
    whitelist = true,
    forbidNonWhitelisted = true
  ): RequestHandler {
    return (req: Request, _res: Response, next: NextFunction) => {
      const obj = plainToInstance(type, req[value]);
      validate(obj, {
        skipMissingProperties,
        whitelist,
        forbidNonWhitelisted
      }).then((errors: ValidationError[]) => {
        if (errors.length > 0) {
          const message = errors.map(this._getAllNestedErrors).join(', ');
          next(new HttpException(400, message));
        } else {
          next();
        }
      });
    };
  }
}

export { ValidateMiddleware };
