import crypto from 'crypto';
import { CookieOptions } from 'express';
import jwt from 'jsonwebtoken';
import moment from 'moment';
import { BadRequestError, ForbiddenError, UnauthorizedError } from 'routing-controllers';

import { Env, ONE_DAY_IN_MILLISECONDS } from '../../constants';
import { HttpException } from '../../exceptions';
import { ResponseTokenDto, TokenDto } from './dto';
import { TokenType } from './schema/token.schema';
import { TokenRepository } from './token.repository';

class TokenService {
  private readonly tokenRepository: TokenRepository;

  constructor() {
    this.tokenRepository = new TokenRepository();
  }

  async validateEmailToken(token: string): Promise<void> {
    const existingToken = await this.tokenRepository.findOne({ token });

    if (Boolean(existingToken) === false) {
      throw new ForbiddenError('Invalid token or expired');
    }

    if (moment(existingToken?.expires).isSameOrAfter(moment())) {
      throw new ForbiddenError('Invalid token or expired');
    }

    await existingToken?.remove();

    return;
  }

  async findTokenByUserId(userId: string) {
    const token = await this.tokenRepository.findOne({ userId });

    if (Boolean(token) === false) {
      return null;
    }

    return token;
  }

  async generateLinkToResetPassword(userId: string): Promise<string> {
    const existingToken = await this.tokenRepository.findOne({ userId });

    if (Boolean(existingToken) === true) {
      await existingToken?.remove();
    }

    const token = crypto.randomBytes(32).toString('hex');

    await this._saveToken(token, TokenType.Reset, userId, moment().add(Env.JWT_RESET_EXPIRE, 'ms').toDate());

    return `http://localhost:3000/api/auth/reset-password/${userId}/${token}`;
  }

  generateCookieOptions() {
    const accessTokenCookieOptions: CookieOptions = {
      expires: moment().add(Env.JWT_ACCESS_EXPIRE, 'ms').toDate(),
      maxAge: parseInt(Env.JWT_ACCESS_EXPIRE),
      httpOnly: true,
      sameSite: 'lax'
    };

    const refreshTokenCookieOptions: CookieOptions = {
      expires: moment().add(Env.JWT_REFRESH_EXPIRE, 'd').toDate(),
      maxAge: parseInt(Env.JWT_REFRESH_EXPIRE) * ONE_DAY_IN_MILLISECONDS,
      httpOnly: true,
      sameSite: 'lax'
    };

    return { accessTokenCookieOptions, refreshTokenCookieOptions };
  }

  async generateAndSaveAuthTokens(userId: string): Promise<ResponseTokenDto> {
    const accessToken = this._generateToken(userId, moment().add(Env.JWT_ACCESS_EXPIRE, 'ms').unix(), TokenType.Access);

    const refreshToken = this._generateToken(
      userId,
      moment().add(Env.JWT_REFRESH_EXPIRE, 'd').unix(),
      TokenType.Refresh
    );

    await this._saveToken(refreshToken, TokenType.Refresh, userId, moment().add(Env.JWT_REFRESH_EXPIRE, 'd').toDate());

    return { accessToken, refreshToken };
  }

  async refreshAuthTokens(token: string, userId: string): Promise<ResponseTokenDto | HttpException> {
    try {
      const refreshTokenDocument = await this.tokenRepository.validate(token);

      await refreshTokenDocument?.remove();

      const { accessToken, refreshToken } = await this.generateAndSaveAuthTokens(userId);

      return { accessToken, refreshToken };
    } catch (error) {
      const errorMessage = (error as Error).message;

      if (errorMessage === 'Invalid token' || errorMessage === 'jwt expired') {
        throw new UnauthorizedError('Invalid token');
      }

      throw new BadRequestError((error as Error).message);
    }
  }

  async removeToken(userId: string): Promise<void | HttpException> {
    try {
      const tokenDocument = await this.tokenRepository.findOne({ userId, type: TokenType.Refresh });

      await tokenDocument?.remove();
    } catch (error) {
      throw new BadRequestError((error as Error).message);
    }
  }

  private _generateToken(userId: string, expiresIn: number, type: TokenType): string {
    const payload: TokenDto = {
      id: userId,
      iat: moment().unix(),
      exp: expiresIn,
      type
    };

    return jwt.sign(payload, Env.JWT_SECRET);
  }

  private async _saveToken(token: string, type: TokenType, userId: string, expires: Date) {
    return await this.tokenRepository.create({
      token,
      type,
      userId,
      expires
    });
  }
}

export { TokenService };
