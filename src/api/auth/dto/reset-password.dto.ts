import { IsAscii, IsMongoId, IsNotEmpty, IsString, MinLength } from 'class-validator';

class ResetPasswordDto {
  @IsString()
  @MinLength(6)
  readonly password: string;
}

class ResetPasswordParamsDto {
  @IsMongoId()
  readonly id: string;

  @IsString()
  @IsAscii()
  @IsNotEmpty()
  @MinLength(32)
  token: string;
}

export { ResetPasswordDto, ResetPasswordParamsDto };
