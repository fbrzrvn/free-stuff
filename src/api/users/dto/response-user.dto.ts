import { Id } from '../../shared/types';
import { UserRole } from '../schema/user.schema';

class ResponseUserDto {
  readonly id: Id;
  readonly username: string;
  readonly email: string;
  readonly role: UserRole;
  readonly firstName?: string;
  readonly lastName?: string;
  readonly avatar?: string;
  readonly country?: string;
  readonly phone?: string;
  readonly location?: number[];
  readonly followers?: Id[];
  readonly following?: Id[];
}

export { ResponseUserDto };
