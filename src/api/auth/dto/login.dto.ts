import { Transform } from 'class-transformer';
import { IsEmail, IsNotEmpty, IsString, MinLength } from 'class-validator';

class LoginDto {
  @IsEmail()
  @IsNotEmpty()
  @Transform(({ value }) => value?.toLowerCase().trim())
  readonly email: string;

  @IsString()
  @MinLength(6)
  @Transform(({ value }) => value?.trim())
  readonly password: string;
}

export { LoginDto };
