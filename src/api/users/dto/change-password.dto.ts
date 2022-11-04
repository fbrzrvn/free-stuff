import { IsString, MinLength } from 'class-validator';

class ChangePasswordDto {
  @IsString()
  @MinLength(6)
  oldPassword: string;

  @IsString()
  @MinLength(6)
  newPassword: string;
}

export { ChangePasswordDto };
