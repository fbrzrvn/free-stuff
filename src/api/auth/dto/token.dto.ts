import { IsJWT, IsMongoId } from 'class-validator';

import { TokenType } from '../schema/token.schema';

class TokenDto {
  readonly id: string;
  readonly iat: number;
  readonly exp: number;
  readonly type: TokenType;
}

class RefreshTokenDto {
  @IsJWT()
  readonly token: string;

  @IsMongoId()
  readonly userId: string;
}

class ResponseTokenDto {
  accessToken: string;
  refreshToken: string;
}

export { RefreshTokenDto, ResponseTokenDto, TokenDto };
