import { UserRole } from '../../users/schema/user.schema';

class JwtDto {
  readonly id: string;
  readonly username: string;
  readonly email: string;
  readonly role: UserRole;
  readonly iat: number;
  readonly exp: number;
}

export { JwtDto };
