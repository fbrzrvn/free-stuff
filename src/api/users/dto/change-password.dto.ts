import { Transform } from "class-transformer";
import { IsString, MinLength } from "class-validator";

class ChangePasswordDto {
  @IsString()
  @MinLength(6)
  @Transform(({ value }) => value?.trim())
  oldPassword: string;

  @IsString()
  @MinLength(6)
  @Transform(({ value }) => value?.trim())
  newPassword: string;
}

export { ChangePasswordDto };
