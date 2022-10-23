import { IsOptional, IsString } from 'class-validator';

class QueryDto {
  @IsOptional()
  @IsString()
  readonly limit?: string;

  @IsOptional()
  @IsString()
  readonly page?: string;
}

export { QueryDto };
