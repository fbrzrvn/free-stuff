import { Document, model, Schema } from 'mongoose';

export enum TokenType {
  Access = 'access',
  Refresh = 'refresh',
  Reset = 'reset'
}

export interface TokenDocument extends Document {
  createdAt: Date
  expires: Date
  token: string
  type: TokenType
  userId: string
}

const TokenSchema: Schema = new Schema({
  createdAt: {
    type: Date,
    require: true,
    default: new Date()
  },
  expires: {
    type: Date,
    require: true
  },
  token: {
    type: String,
    require: true
  },
  type: {
    type: String,
    enum: TokenType,
    require: true
  },
  userId: {
    type: Schema.Types.ObjectId,
    require: true,
    ref: 'User'
  }
});

const TokenModel = model<TokenDocument>('Token', TokenSchema);

export { TokenModel };
