import jwt from 'jsonwebtoken';
import { FilterQuery } from 'mongoose';

import { Env } from '../../constants/Env';
import { TokenDocument, TokenModel } from './schema/token.schema';

class TokenRepository {
  private readonly tokenModel = TokenModel;

  async create(tokenData: any) {
    return await this.tokenModel.create(tokenData);
  }

  async findOne(criteria: FilterQuery<TokenDocument>) {
    return await this.tokenModel.findOne(criteria);
  }

  async deleteOne(criteria: FilterQuery<TokenDocument>) {
    return await this.tokenModel.findOneAndDelete(criteria);
  }

  async validate(tokenData: string) {
    const verifiedToken = jwt.verify(tokenData, Env.JWT_SECRET, { ignoreExpiration: false }) as jwt.JwtPayload;

    const token = await this.tokenModel.findOne({ token: tokenData, userId: verifiedToken.id });

    if (Boolean(token) === false) {
      throw new Error('Invalid token');
    }

    return token;
  }
}

export { TokenRepository };
