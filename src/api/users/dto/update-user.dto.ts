import { Transform } from 'class-transformer';
import {
  IsArray,
  IsEmail,
  IsEnum,
  IsNotEmpty,
  IsNumber,
  IsOptional,
  IsString,
  MaxLength,
  MinLength
} from 'class-validator';

import { UserRole } from '../schema/user.schema';

class UpdateUserDto {
  @IsOptional()
  @IsString()
  @MinLength(4)
  @MaxLength(15)
  @Transform(({ value }) => value?.trim())
  readonly username?: string;

  @IsOptional()
  @IsEmail()
  @IsNotEmpty()
  @Transform(({ value }) => value?.toLowerCase().trim())
  readonly email?: string;

  @IsOptional()
  @IsString()
  @MinLength(6)
  readonly password?: string;

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

  @IsOptional()
  @IsArray()
  @IsNumber(undefined, { each: true })
  readonly location?: number[];
}

export { UpdateUserDto };
