import { Transform } from 'class-transformer';
import { IsEmail, IsEnum, IsNotEmpty, IsOptional, IsString, MaxLength, MinLength } from 'class-validator';

import { UserRole } from '../schema/user.schema';

class CreateUserDto {
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
  @Transform(({ value }) => value?.trim())
  readonly password: string;

  @IsOptional()
  @IsEnum(UserRole)
  @IsNotEmpty()
  readonly role?: UserRole;

  @IsOptional()
  @IsString()
  @IsNotEmpty()
  readonly firstName?: string;

  @IsOptional()
  @IsString()
  @IsNotEmpty()
  readonly lastName?: string;

  @IsOptional()
  @IsString()
  @IsNotEmpty()
  readonly avatar?: string;

  @IsOptional()
  @IsString()
  @IsNotEmpty()
  readonly country?: string;

  @IsOptional()
  @IsString()
  @IsNotEmpty()
  readonly phone?: string;
}

export { CreateUserDto };
