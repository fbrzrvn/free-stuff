import { IsJWT } from 'class-validator';
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
}

export { RefreshTokenDto, TokenDto };
