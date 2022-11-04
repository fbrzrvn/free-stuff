import { Transform } from 'class-transformer';
import { IsEmail, IsNotEmpty, IsString, MaxLength, MinLength } from 'class-validator';

class RegisterDto {
  @IsString()
  @MinLength(4)
  @MaxLength(15)
  @Transform(({ value }) => value?.trim())
  readonly username: string;

  @IsEmail()
  @IsNotEmpty()
  @Transform(({ value }) => value?.toLowerCase().trim())
  readonly email: string;

  @IsString()
  @MinLength(6)
  readonly password: string;
}

export { RegisterDto };
