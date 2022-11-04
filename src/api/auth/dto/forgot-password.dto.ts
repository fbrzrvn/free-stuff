import { IsEmail, IsNotEmpty } from 'class-validator';

class ForgotPasswordDto {
  @IsEmail()
  @IsNotEmpty()
  readonly email: string;
}

export { ForgotPasswordDto };
