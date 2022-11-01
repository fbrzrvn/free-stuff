import { Request } from 'express';
import passport from 'passport';
import { ExtractJwt, Strategy as JwtStrategy, StrategyOptions } from 'passport-jwt';
import { ForbiddenError } from 'routing-controllers';

import { TokenDto } from '../api/auth/dto';
import { TokenType } from '../api/auth/schema/token.schema';
import { ACCESS_TOKEN, Env } from '../constants';

const cookieExtrator = (req: Request): string | null => {
  let token: string | null = null;

  if (req && req.cookies) {
    token = req.cookies[ACCESS_TOKEN];
  }

  return token;
};

const opts: StrategyOptions = {
  jwtFromRequest: ExtractJwt.fromExtractors([ExtractJwt.fromAuthHeaderAsBearerToken(), cookieExtrator]),
  secretOrKey: Env.JWT_SECRET,
  ignoreExpiration: false
};

const JwtPassportMiddleware = new JwtStrategy(opts, async (tokenPayload: TokenDto, done) => {
  try {
    if (tokenPayload.type !== TokenType.Access) {
      throw new ForbiddenError('Invalid token');
    }

    return done(null, tokenPayload);
  } catch (error) {
    return done(error, false);
  }
});

passport.serializeUser(function (user: Express.User, done) {
  done(null, user);
});

passport.deserializeUser(function (user: Express.User, done) {
  done(null, user);
});

export { JwtPassportMiddleware };
