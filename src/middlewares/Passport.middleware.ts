import { Request } from 'express';
import passport from 'passport';
import { ExtractJwt, Strategy as JwtStrategy, StrategyOptions } from 'passport-jwt';

// import { JwtTokenDto } from '../api/auth/dto';
import { Env } from '../constants';

const cookieExtrator = (req: Request): string | null => {
  let token: string | null = null;

  if (req && req.cookies) {
    token = req.cookies['access_token'];
  }

  return token;
};

const opts: StrategyOptions = {
  jwtFromRequest: ExtractJwt.fromExtractors([ExtractJwt.fromAuthHeaderAsBearerToken(), cookieExtrator]),
  ignoreExpiration: false,
  secretOrKey: Env.JWT_SECRET
};

const JwtPassportMiddleware = new JwtStrategy(opts, async (tokenPayload: any, done) => {
  try {
    return done(null, tokenPayload);
  } catch (error) {
    return done(error, false);
  }
});

passport.serializeUser(function (user, done) {
  done(null, user);
});

passport.deserializeUser(function (user, done) {
  done(null, user as any);
});

export { JwtPassportMiddleware };
