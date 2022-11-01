import { CookieOptions } from 'express';
import jwt from 'jsonwebtoken';
import moment from 'moment';
import { BadRequestError, ForbiddenError } from 'routing-controllers';

import { Env, ONE_DAY_IN_MILLISECONDS } from '../../constants';
import { DocumentResponse } from '../shared/types';
import { ResponseUserDto } from '../users/dto';
import { UserDocument } from '../users/schema/user.schema';
import { userMapper } from '../users/user.mapper';
import { UserRepository } from '../users/user.repository';
import { LoginDto, RegisterDto, TokenDto } from './dto';
import { TokenType } from './schema/token.schema';
import { TokenRepository } from './token.repository';

class AuthService {
  private readonly tokenRepository: TokenRepository;
  private readonly userRepository: UserRepository;

  constructor() {
    this.tokenRepository = new TokenRepository();
    this.userRepository = new UserRepository();
  }

  async register(userData: RegisterDto): Promise<ResponseUserDto | null> {
    const { email, username } = userData;

    const existingEmail = await this.userRepository.findOne({ email });

    if (Boolean(existingEmail) === true) {
      return null;
    }

    const existingUsername = await this.userRepository.findOne({ username });

    if (Boolean(existingUsername) === true) {
      return null;
    }

    await this.userRepository.create({ ...userData });

    const user = await this.userRepository.findOne({ email });

    return userMapper(user);
  }

  async loginWithEmailAndPassword(userData: LoginDto): Promise<any | null> {
    const user = await this.userRepository.validate(userData);

    if (Boolean(user) === false) {
      return null;
    }

    const token = await this.tokenRepository.findOne({ userId: user?._id });

    if (Boolean(token) === true) {
      await token?.remove();
    }

    const { accessToken, refreshToken } = await this._generateAuthTokens(user);

    return { accessToken, refreshToken };
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

  async refreshToken(token: string) {
    try {
      const refreshTokenDocument = await this.tokenRepository.validate(token);

      const user = await this.userRepository.findById(refreshTokenDocument?.userId as string);

      await refreshTokenDocument?.remove();

      const { accessToken, refreshToken } = await this._generateAuthTokens(user);

      return { accessToken, refreshToken };
    } catch (error) {
      const errorMessage = (error as Error).message;

      if (errorMessage === 'Invalid token' || errorMessage === 'jwt expired') {
        throw new ForbiddenError('Invalid token');
      }

      throw new BadRequestError((error as Error).message);
    }
  }

  async removeToken(userId: string) {
    try {
      const tokenDocument = await this.tokenRepository.findOne({ userId });

      await tokenDocument?.remove();
    } catch (error) {
      if ((error as Error).message === 'Invalid token') {
        throw new ForbiddenError('Invalid token');
      }

      throw new BadRequestError((error as Error).message);
    }
  }

  private async _generateAuthTokens(user: DocumentResponse<UserDocument>) {
    const accessToken = this._generateToken(
      user?._id,
      moment().add(Env.JWT_ACCESS_EXPIRE, 'ms').unix(),
      TokenType.Access
    );

    const refreshToken = this._generateToken(
      user?._id,
      moment().add(Env.JWT_REFRESH_EXPIRE, 'd').unix(),
      TokenType.Refresh
    );

    await this._saveToken(
      refreshToken,
      TokenType.Refresh,
      user?._id,
      moment().add(Env.JWT_REFRESH_EXPIRE, 'd').toDate()
    );

    return { accessToken, refreshToken };
  }

  private _generateToken(userId: string, expiresIn: number, type: TokenType) {
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

export { AuthService };
